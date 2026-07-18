using MediatR;

namespace Feinov.Application.Features.Admin.Orders;

public sealed record GetAdminOrderListQuery(
    string? OrderNumber,
    string? Status,
    int PageNumber,
    int PageSize) : IRequest<PagedAdminOrderListResult>;

public sealed record PagedAdminOrderListResult(
    IReadOnlyList<AdminOrderListItem> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages);

public sealed record AdminOrderListItem(
    Guid OrderId,
    string OrderNumber,
    string OrderStatus,
    string PaymentStatus,
    decimal TotalAmount,
    DateTime CreatedDate);
