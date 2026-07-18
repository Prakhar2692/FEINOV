using Feinov.Application.Features.Products;

namespace Feinov.Application.Common.Interfaces;

public interface IProductReviewService
{
    Task<AddProductReviewResult> AddReviewAsync(
        Guid userId,
        Guid productId,
        int rating,
        string? reviewTitle,
        string? reviewText,
        CancellationToken cancellationToken = default);

    Task<GetProductReviewsResult> GetApprovedReviewsAsync(
        Guid productId,
        CancellationToken cancellationToken = default);
}
