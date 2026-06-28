using Feinov.Application.Common.Interfaces;
using Feinov.Application.Features.Admin.Inventory;
using Feinov.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Feinov.Infrastructure.Services;

public sealed class InventoryService(Context dbContext) : IInventoryService
{
    public async Task<InventoryUpdateResult> UpdateAsync(
        Guid variantId,
        int totalStock,
        int reorderLevel,
        CancellationToken cancellationToken = default)
    {
        var variantExists = await dbContext.ProductVariants.AnyAsync(x => x.VariantId == variantId, cancellationToken);
        if (!variantExists)
            throw new InvalidOperationException("Variant does not exist.");

        var inventory = await dbContext.Inventories.FirstOrDefaultAsync(x => x.VariantId == variantId, cancellationToken);
        if (inventory == null)
            throw new InvalidOperationException("Inventory record does not exist for the variant.");

        inventory.TotalStock = totalStock;
        inventory.ReorderLevel = reorderLevel;
        inventory.AvailableStock = Math.Max(0, totalStock - inventory.ReservedStock);
        inventory.LastStockUpdated = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return new InventoryUpdateResult(inventory.AvailableStock, inventory.TotalStock, inventory.ReorderLevel);
    }

    public async Task<PagedInventoryResult> GetPagedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await dbContext.Inventories.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var items = await dbContext.Inventories
            .AsNoTracking()
            .OrderBy(x => x.VariantId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new InventoryListItem(
                x.VariantId,
                x.Variant.Product.ProductName,
                x.Variant.VariantName,
                x.TotalStock,
                x.ReservedStock,
                x.AvailableStock,
                x.ReorderLevel))
            .ToListAsync(cancellationToken);

        return new PagedInventoryResult(items, pageNumber, pageSize, totalCount, Math.Max(1, totalPages));
    }
}
