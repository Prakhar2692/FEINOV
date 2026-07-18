using Feinov.Application.Common.Interfaces;
using Feinov.Application.Features.Admin.Orders;
using Feinov.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Feinov.Infrastructure.Services;

public sealed class AdminOrderService(Context dbContext) : IAdminOrderService
{
    public async Task<PagedAdminOrderListResult> GetOrderListAsync(
        string? orderNumber,
        string? status,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        if (pageNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be at least 1.");

        if (pageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be at least 1.");

        var query = dbContext.Orders.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(orderNumber))
        {
            var normalizedOrderNumber = orderNumber.Trim();
            query = query.Where(x => x.OrderNumber.Contains(normalizedOrderNumber));
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            var normalizedStatus = status.Trim();
            query = query.Where(x => x.OrderStatus == normalizedStatus);
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var items = await query
            .OrderByDescending(x => x.CreatedDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new AdminOrderListItem(
                x.OrderId,
                x.OrderNumber,
                x.OrderStatus,
                x.PaymentStatus,
                x.TotalAmount,
                x.CreatedDate))
            .ToListAsync(cancellationToken);

        return new PagedAdminOrderListResult(items, pageNumber, pageSize, totalCount, Math.Max(1, totalPages));
    }

    public async Task<AdminOrderStatusUpdateResult> UpdateOrderStatusAsync(
        Guid orderId,
        string status,
        CancellationToken cancellationToken = default)
    {
        if (orderId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(orderId), "Order id is required.");

        if (string.IsNullOrWhiteSpace(status))
            throw new ArgumentOutOfRangeException(nameof(status), "Status is required.");

        var allowedStatuses = new[] { "Confirmed", "Packed", "Shipped", "Delivered", "Cancelled" };
        if (!allowedStatuses.Contains(status))
            throw new InvalidOperationException($"Status must be one of: {string.Join(", ", allowedStatuses)}.");

        var order = await dbContext.Orders.FirstOrDefaultAsync(x => x.OrderId == orderId, cancellationToken);
        if (order == null)
            throw new InvalidOperationException("Order does not exist.");

        var validTransitions = new Dictionary<string, string[]>
        {
            ["PendingPayment"] = new[] { "Cancelled" },
            ["Confirmed"] = new[] { "Packed", "Cancelled" },
            ["Packed"] = new[] { "Shipped", "Cancelled" },
            ["Shipped"] = new[] { "Delivered" },
            ["Delivered"] = Array.Empty<string>(),
            ["Cancelled"] = Array.Empty<string>()
        };

        var currentStatus = order.OrderStatus;
        if (!validTransitions.TryGetValue(currentStatus, out var nextStatuses))
            throw new InvalidOperationException($"Invalid current order status: {currentStatus}.");

        if (!nextStatuses.Contains(status))
            throw new InvalidOperationException($"Cannot transition order from {currentStatus} to {status}.");

        order.OrderStatus = status;
        await dbContext.SaveChangesAsync(cancellationToken);

        return new AdminOrderStatusUpdateResult(order.OrderId, order.OrderNumber, order.OrderStatus);
    }
}
