namespace Feinov.Application.Features.Products;

public sealed record ProductReviewDto(
    Guid ReviewId,
    Guid UserId,
    int Rating,
    string? ReviewTitle,
    string? ReviewText,
    DateTime CreatedDate);
