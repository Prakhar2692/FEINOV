using Feinov.Application.Features.Admin.Orders;

namespace Feinov.Application.Common.Interfaces;

public interface IAdminOrderService
{
    Task<PagedAdminOrderListResult> GetOrderListAsync(
        string? orderNumber,
        string? status,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<AdminOrderStatusUpdateResult> UpdateOrderStatusAsync(
        Guid orderId,
        string status,
        CancellationToken cancellationToken = default);
}
