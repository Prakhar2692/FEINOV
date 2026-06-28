namespace Feinov.Application.Common.Interfaces;

public interface IProductVariantRepository
{
    Task<Guid> CreateVariantAsync(
        Guid productId,
        string sku,
        string variantName,
        int? sizeMl,
        decimal mrp,
        decimal sellingPrice,
        string? barcode,
        CancellationToken cancellationToken = default);
}
