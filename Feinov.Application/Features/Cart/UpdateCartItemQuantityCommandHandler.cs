using Feinov.Application.Common.Interfaces;
using MediatR;

namespace Feinov.Application.Features.Cart;

public sealed class UpdateCartItemQuantityCommandHandler(ICartService cartService)
    : IRequestHandler<UpdateCartItemQuantityCommand, UpdateCartItemQuantityResult>
{
    public Task<UpdateCartItemQuantityResult> Handle(UpdateCartItemQuantityCommand request, CancellationToken cancellationToken)
        => cartService.UpdateQuantityAsync(request.UserId, request.CartItemId, request.Quantity, cancellationToken);
}
