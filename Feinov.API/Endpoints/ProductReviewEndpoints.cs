using Feinov.Application.Features.Products;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Feinov.API.Endpoints;

public static class ProductReviewEndpoints
{
    public static IEndpointRouteBuilder MapProductReviewEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/reviews").WithTags("ProductReviews");

        group.MapPost("", AddProductReview)
            .WithName("AddProductReview")
            .WithSummary("Submit a review for a purchased product")
            .WithOpenApi();

        group.MapGet("/products/{productId:guid}", GetProductReviews)
            .WithName("GetProductReviews")
            .WithSummary("Return approved reviews and average rating for a product")
            .WithOpenApi();

        return app;
    }

    private static async Task<IResult> AddProductReview(AddProductReviewRequest request, ISender sender, CancellationToken cancellationToken)
    {
        var command = new AddProductReviewCommand(
            request.UserId,
            request.ProductId,
            request.Rating,
            request.ReviewTitle,
            request.ReviewText);

        try
        {
            var result = await sender.Send(command, cancellationToken);
            return Results.Created($"/api/reviews/{result.ReviewId}", result);
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(new { success = false, message = ex.Message });
        }
    }

    private static async Task<IResult> GetProductReviews(Guid productId, ISender sender, CancellationToken cancellationToken)
    {
        try
        {
            var result = await sender.Send(new GetProductReviewsQuery(productId), cancellationToken);
            return Results.Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(new { success = false, message = ex.Message });
        }
    }
}

public sealed record AddProductReviewRequest(
    Guid UserId,
    Guid ProductId,
    int Rating,
    string? ReviewTitle,
    string? ReviewText);
