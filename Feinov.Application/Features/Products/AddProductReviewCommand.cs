using MediatR;

namespace Feinov.Application.Features.Products;

public sealed record AddProductReviewCommand(
    Guid UserId,
    Guid ProductId,
    int Rating,
    string? ReviewTitle,
    string? ReviewText) : IRequest<AddProductReviewResult>;

public sealed record AddProductReviewResult(Guid ReviewId);
