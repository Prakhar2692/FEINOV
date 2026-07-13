using Feinov.Application.Common.Interfaces;
using MediatR;

namespace Feinov.Application.Features.Cart;

public sealed class AddToCartCommandHandler(ICartService cartService)
    : IRequestHandler<AddToCartCommand, AddToCartResult>
{
    public Task<AddToCartResult> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        => cartService.AddAsync(request.UserId, request.VariantId, request.Quantity, cancellationToken);
}
