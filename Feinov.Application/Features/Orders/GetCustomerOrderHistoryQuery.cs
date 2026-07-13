using MediatR;

namespace Feinov.Application.Features.Orders;

public sealed record GetCustomerOrderHistoryQuery(Guid UserId, int PageNumber, int PageSize)
    : IRequest<PagedCustomerOrderHistoryResult>;

public sealed record PagedCustomerOrderHistoryResult(
    IReadOnlyList<CustomerOrderHistoryItem> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages);

public sealed record CustomerOrderHistoryItem(
    Guid OrderId,
    string OrderNumber,
    string OrderStatus,
    string PaymentStatus,
    decimal TotalAmount,
    DateTime CreatedDate);
