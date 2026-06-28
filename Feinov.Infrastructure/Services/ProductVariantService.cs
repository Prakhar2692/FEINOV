using Feinov.Application.Common.Interfaces;

namespace Feinov.Infrastructure.Services;

public sealed class ProductVariantService(IProductVariantRepository productVariantRepository) : IProductVariantService
{
    public Task<Guid> CreateVariantAsync(
        Guid productId,
        string sku,
        string variantName,
        int? sizeMl,
        decimal mrp,
        decimal sellingPrice,
        string? barcode,
        CancellationToken cancellationToken = default)
        => productVariantRepository.CreateVariantAsync(
            productId,
            sku,
            variantName,
            sizeMl,
            mrp,
            sellingPrice,
            barcode,
            cancellationToken);
}
