using Feinov.Application.Common.Interfaces;
using Feinov.Application.Features.Products;
using Feinov.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Feinov.Infrastructure.Services;

public sealed class ProductReviewService(Context dbContext) : IProductReviewService
{
    public async Task<AddProductReviewResult> AddReviewAsync(
        Guid userId,
        Guid productId,
        int rating,
        string? reviewTitle,
        string? reviewText,
        CancellationToken cancellationToken = default)
    {
        if (userId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(userId), "User id is required.");

        if (productId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(productId), "Product id is required.");

        if (rating < 1 || rating > 5)
            throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 1 and 5.");

        var userExists = await dbContext.Users.AnyAsync(x => x.UserId == userId && x.IsActive, cancellationToken);
        if (!userExists)
            throw new InvalidOperationException("User does not exist.");

        var productExists = await dbContext.Products.AnyAsync(x => x.ProductId == productId && x.IsActive, cancellationToken);
        if (!productExists)
            throw new InvalidOperationException("Product does not exist.");

        var purchasedProduct = await dbContext.OrderItems
            .AsNoTracking()
            .Include(x => x.Order)
            .AnyAsync(x => x.Variant.ProductId == productId
                && x.Order.CustomerId == userId
                && x.Order.PaymentStatus == "Paid"
                && x.Order.OrderStatus != "Cancelled",
                cancellationToken);

        if (!purchasedProduct)
            throw new InvalidOperationException("Product must be purchased before submitting a review.");

        var existingReview = await dbContext.ProductReviews
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId, cancellationToken);

        if (existingReview != null)
            throw new InvalidOperationException("You have already reviewed this product.");

        var review = new ProductReview
        {
            ReviewId = Guid.NewGuid(),
            UserId = userId,
            ProductId = productId,
            Rating = rating,
            ReviewTitle = reviewTitle,
            ReviewText = reviewText,
            IsApproved = false,
            CreatedDate = DateTime.UtcNow
        };

        dbContext.ProductReviews.Add(review);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new AddProductReviewResult(review.ReviewId);
    }

    public async Task<GetProductReviewsResult> GetApprovedReviewsAsync(
        Guid productId,
        CancellationToken cancellationToken = default)
    {
        if (productId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(productId), "Product id is required.");

        var productExists = await dbContext.Products
            .AsNoTracking()
            .AnyAsync(x => x.ProductId == productId && x.IsActive, cancellationToken);

        if (!productExists)
            throw new InvalidOperationException("Product does not exist.");

        var reviews = await dbContext.ProductReviews
            .AsNoTracking()
            .Where(x => x.ProductId == productId && x.IsApproved)
            .OrderByDescending(x => x.CreatedDate)
            .Select(x => new ProductReviewDto(
                x.ReviewId,
                x.UserId,
                x.Rating,
                x.ReviewTitle,
                x.ReviewText,
                x.CreatedDate))
            .ToListAsync(cancellationToken);

        var averageRating = reviews.Count == 0
            ? 0m
            : Math.Round(reviews.Average(x => (decimal)x.Rating), 2);

        return new GetProductReviewsResult(reviews, averageRating);
    }
}
