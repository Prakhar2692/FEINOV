using MediatR;

namespace Feinov.Application.Features.Orders;

public sealed record GetCustomerOrderDetailsQuery(Guid UserId, Guid OrderId)
    : IRequest<CustomerOrderDetailsResult>;

public sealed record CustomerOrderDetailsResult(
    Guid OrderId,
    string OrderNumber,
    string OrderStatus,
    string PaymentStatus,
    decimal TotalAmount,
    DateTime CreatedDate,
    CustomerOrderShippingAddress ShippingAddress,
    IReadOnlyList<CustomerOrderDetailsItem> Items);

public sealed record CustomerOrderDetailsItem(
    Guid OrderItemId,
    string ProductName,
    string? VariantName,
    string Sku,
    int Quantity,
    decimal UnitPrice,
    decimal DiscountAmount,
    decimal TotalAmount);

public sealed record CustomerOrderShippingAddress(
    string FullName,
    string PhoneNumber,
    string AddressLine1,
    string? AddressLine2,
    string City,
    string State,
    string PostalCode,
    string Country);
