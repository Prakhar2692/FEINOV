using Feinov.Application.Common.Interfaces;
using Feinov.Application.Features.Catalog;
using Feinov.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Feinov.Infrastructure.Services;

public sealed class CatalogService(Context dbContext) : ICatalogService
{
    public async Task<PagedCatalogResult> GetPagedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await dbContext.Products
            .AsNoTracking()
            .CountAsync(x => x.IsActive, cancellationToken);

        var totalPages = totalCount == 0
            ? 0
            : (int)Math.Ceiling(totalCount / (double)pageSize);

        var products = await dbContext.Products
            .AsNoTracking()
            .Where(x => x.IsActive)
            .Include(x => x.Subcategory)
                .ThenInclude(x => x.Category)
            .Include(x => x.ProductImages)
            .Include(x => x.ProductVariants)
                .ThenInclude(x => x.VariantDiscounts)
            .OrderBy(x => x.ProductName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var items = products
            .Select(product =>
            {
                var lowestSellingPrice = product.ProductVariants
                    .Where(variant => variant.IsActive)
                    .Select(variant => MapSellingPrice(variant))
                    .DefaultIfEmpty(0m)
                    .Min();

                return new CatalogProductListItem(
                    product.ProductId,
                    product.Subcategory.Category.CategoryName,
                    product.Subcategory.SubcategoryName,
                    product.ProductName,
                    product.ProductImages
                        .OrderBy(image => image.DisplayOrder)
                        .ThenBy(image => image.CreatedDate)
                        .Select(image => image.ImageUrl)
                        .FirstOrDefault(),
                    lowestSellingPrice,
                    product.ProductVariants.Count);
            })
            .ToList();

        return new PagedCatalogResult(items, pageNumber, pageSize, totalCount, totalPages);
    }

    public async Task<ProductDetailsResult> GetByIdAsync(
        Guid productId,
        CancellationToken cancellationToken = default)
    {
        var product = await dbContext.Products
            .AsNoTracking()
            .Where(x => x.ProductId == productId && x.IsActive)
            .Include(x => x.Subcategory)
                .ThenInclude(x => x.Category)
            .Include(x => x.ProductImages)
            .Include(x => x.ProductVariants)
                .ThenInclude(x => x.VariantDiscounts)
            .Include(x => x.ProductVariants)
                .ThenInclude(x => x.CartItems)
            .Include(x => x.ProductVariants)
                .ThenInclude(x => x.OrderItems)
            .FirstOrDefaultAsync(cancellationToken);

        if (product == null)
            throw new InvalidOperationException($"Product '{productId}' was not found.");

        var variants = product.ProductVariants
            .Where(variant => variant.IsActive)
            .Select(variant =>
            {
                var inventory = dbContext.Inventories
                    .AsNoTracking()
                    .FirstOrDefault(x => x.VariantId == variant.VariantId);

                var activeDiscount = variant.VariantDiscounts
                    .Where(discount => discount.IsActive == true
                        && (!discount.StartDate.HasValue || discount.StartDate <= DateTime.UtcNow)
                        && (!discount.EndDate.HasValue || discount.EndDate >= DateTime.UtcNow))
                    .OrderByDescending(discount => discount.StartDate)
                    .FirstOrDefault();

                var sellingPrice = MapSellingPrice(variant);
                var discountPercentage = activeDiscount?.DiscountPercentage;

                return new ProductVariantDetailItem(
                    variant.VariantId,
                    variant.Sku,
                    variant.VariantName,
                    variant.SizeMl,
                    variant.PackSize,
                    variant.WeightGrams,
                    variant.Mrp,
                    sellingPrice,
                    discountPercentage,
                    variant.Barcode,
                    variant.IsDefault,
                    variant.IsActive,
                    inventory?.AvailableStock ?? 0,
                    inventory?.ReservedStock ?? 0,
                    inventory?.TotalStock ?? 0,
                    inventory?.ReorderLevel ?? 0);
            })
            .ToList();

        var images = product.ProductImages
            .OrderBy(image => image.DisplayOrder)
            .ThenBy(image => image.CreatedDate)
            .Select(image => new ProductImageDetailItem(image.ImageId, image.ImageUrl, image.DisplayOrder))
            .ToList();

        return new ProductDetailsResult(
            product.ProductId,
            product.Subcategory.Category.CategoryName,
            product.Subcategory.SubcategoryName,
            product.ProductName,
            product.Description,
            product.Brand,
            images,
            variants);
    }

    private static decimal MapSellingPrice(ProductVariant variant)
    {
        var activeDiscount = variant.VariantDiscounts
            .Where(discount => discount.IsActive == true
                && (!discount.StartDate.HasValue || discount.StartDate <= DateTime.UtcNow)
                && (!discount.EndDate.HasValue || discount.EndDate >= DateTime.UtcNow))
            .OrderByDescending(discount => discount.StartDate)
            .FirstOrDefault();

        var discountPercentage = activeDiscount?.DiscountPercentage ?? 0m;
        return discountPercentage > 0
            ? variant.Mrp - (variant.Mrp * discountPercentage / 100m)
            : variant.Mrp;
    }
}
