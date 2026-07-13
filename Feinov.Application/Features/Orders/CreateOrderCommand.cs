using MediatR;

namespace Feinov.Application.Features.Orders;

public sealed record CreateOrderCommand(
    Guid UserId,
    string FullName,
    string PhoneNumber,
    string AddressLine1,
    string? AddressLine2,
    string City,
    string State,
    string PostalCode,
    string Country,
    string? Notes = null) : IRequest<CreateOrderResult>;

public sealed record CreateOrderResult(
    Guid OrderId,
    string OrderNumber,
    decimal TotalAmount,
    string OrderStatus,
    RazorpayOrderDetails? RazorpayOrder = null);
