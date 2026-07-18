using MediatR;

namespace Feinov.Application.Features.Products;

public sealed record GetProductReviewsQuery(Guid ProductId) : IRequest<GetProductReviewsResult>;

public sealed record GetProductReviewsResult(
    IReadOnlyList<ProductReviewDto> Reviews,
    decimal AverageRating);
