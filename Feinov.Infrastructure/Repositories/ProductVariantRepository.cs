using Feinov.Application.Common.Interfaces;
using Feinov.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Feinov.Infrastructure.Repositories;

public sealed class ProductVariantRepository(Context dbContext) : IProductVariantRepository
{
    public async Task<Guid> CreateVariantAsync(
        Guid productId,
        string sku,
        string variantName,
        int? sizeMl,
        decimal mrp,
        decimal sellingPrice,
        string? barcode,
        CancellationToken cancellationToken = default)
    {
        var skuExists = await dbContext.ProductVariants.AnyAsync(x => x.Sku == sku, cancellationToken);
        if (skuExists)
            throw new InvalidOperationException($"SKU '{sku}' already exists.");

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var variant = new ProductVariant
            {
                VariantId = Guid.NewGuid(),
                ProductId = productId,
                Sku = sku,
                VariantName = variantName,
                SizeMl = sizeMl,
                PackSize = 1,
                Mrp = mrp,
                Barcode = barcode,
                IsDefault = false,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            dbContext.ProductVariants.Add(variant);

            var inventory = new Inventory
            {
                VariantId = variant.VariantId,
                TotalStock = 0,
                ReservedStock = 0,
                AvailableStock = 0,
                ReorderLevel = 10,
                LastStockUpdated = DateTime.UtcNow
            };

            dbContext.Inventories.Add(inventory);
            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return variant.VariantId;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
