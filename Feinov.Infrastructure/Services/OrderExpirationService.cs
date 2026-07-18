using Feinov.Application.Common.Interfaces;
using Feinov.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Feinov.Infrastructure.Services;

public sealed class OrderExpirationService(Context dbContext) : IOrderExpirationService
{
    public async Task ExpirePendingOrdersAsync(CancellationToken cancellationToken = default)
    {
        var cutoff = DateTime.UtcNow.AddMinutes(-15);

        var pendingOrders = await dbContext.Orders
            .Where(x => x.OrderStatus == "PendingPayment" && x.CreatedDate <= cutoff)
            .Include(x => x.OrderItems)
            .ToListAsync(cancellationToken);

        if (!pendingOrders.Any())
            return;

        foreach (var order in pendingOrders)
        {
            foreach (var orderItem in order.OrderItems)
            {
                var inventory = await dbContext.Inventories
                    .FromSqlInterpolated($"SELECT * FROM inventory WHERE variant_id = {orderItem.VariantId} FOR UPDATE")
                    .FirstOrDefaultAsync(cancellationToken);

                if (inventory == null)
                    throw new InvalidOperationException($"Inventory record does not exist for variant '{orderItem.VariantId}'.");

                inventory.ReservedStock = Math.Max(0, inventory.ReservedStock - orderItem.Quantity);
                inventory.AvailableStock = Math.Max(0, inventory.TotalStock - inventory.ReservedStock);
                inventory.LastStockUpdated = DateTime.UtcNow;
            }

            order.OrderStatus = "Expired";
            order.PaymentStatus = "Failed";
            order.UpdatedDate = DateTime.UtcNow;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
