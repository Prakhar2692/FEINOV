using MediatR;

namespace Feinov.Application.Features.Cart;

public sealed record GetCartQuery(Guid UserId) : IRequest<GetCartResult>;

public sealed record GetCartResult(
    Guid CartId,
    IReadOnlyList<CartLineItem> Items,
    decimal Subtotal,
    decimal TotalAmount);

public sealed record CartLineItem(
    Guid CartItemId,
    Guid ProductId,
    string ProductName,
    string? Brand,
    string CategoryName,
    string SubcategoryName,
    Guid VariantId,
    string VariantName,
    string Sku,
    int Quantity,
    decimal UnitPrice,
    decimal Subtotal,
    IReadOnlyList<string> Images,
    int AvailableStock);
