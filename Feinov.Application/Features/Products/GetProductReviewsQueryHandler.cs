using Feinov.Application.Common.Interfaces;
using MediatR;

namespace Feinov.Application.Features.Products;

public sealed class GetProductReviewsQueryHandler(IProductReviewService productReviewService)
    : IRequestHandler<GetProductReviewsQuery, GetProductReviewsResult>
{
    public Task<GetProductReviewsResult> Handle(GetProductReviewsQuery request, CancellationToken cancellationToken)
        => productReviewService.GetApprovedReviewsAsync(request.ProductId, cancellationToken);
}
