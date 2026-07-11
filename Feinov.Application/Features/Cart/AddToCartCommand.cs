using MediatR;

namespace Feinov.Application.Features.Cart;

public sealed record AddToCartCommand(
    Guid UserId,
    Guid VariantId,
    int Quantity) : IRequest<AddToCartResult>;

public sealed record AddToCartResult(
    Guid CartId,
    Guid CartItemId,
    int Quantity,
    int AvailableStock);
