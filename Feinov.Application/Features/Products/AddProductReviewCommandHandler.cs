using Feinov.Application.Common.Interfaces;
using MediatR;

namespace Feinov.Application.Features.Products;

public sealed class AddProductReviewCommandHandler(IProductReviewService productReviewService)
    : IRequestHandler<AddProductReviewCommand, AddProductReviewResult>
{
    public Task<AddProductReviewResult> Handle(AddProductReviewCommand request, CancellationToken cancellationToken)
        => productReviewService.AddReviewAsync(
            request.UserId,
            request.ProductId,
            request.Rating,
            request.ReviewTitle,
            request.ReviewText,
            cancellationToken);
}
