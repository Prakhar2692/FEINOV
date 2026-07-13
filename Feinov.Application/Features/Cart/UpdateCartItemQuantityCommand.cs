using MediatR;

namespace Feinov.Application.Features.Cart;

public sealed record UpdateCartItemQuantityCommand(
    Guid UserId,
    Guid CartItemId,
    int Quantity) : IRequest<UpdateCartItemQuantityResult>;

public sealed record UpdateCartItemQuantityResult(
    Guid CartId,
    Guid CartItemId,
    int Quantity,
    bool Removed,
    int AvailableStock);
