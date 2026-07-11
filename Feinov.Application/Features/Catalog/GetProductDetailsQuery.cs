using MediatR;

namespace Feinov.Application.Features.Catalog;

public sealed record GetProductDetailsQuery(Guid ProductId) : IRequest<ProductDetailsResult>;

public sealed record ProductDetailsResult(
    Guid ProductId,
    string CategoryName,
    string SubcategoryName,
    string ProductName,
    string? Description,
    string? Brand,
    IReadOnlyList<ProductImageDetailItem> Images,
    IReadOnlyList<ProductVariantDetailItem> Variants);

public sealed record ProductImageDetailItem(
    Guid ImageId,
    string ImageUrl,
    int DisplayOrder);

public sealed record ProductVariantDetailItem(
    Guid VariantId,
    string Sku,
    string VariantName,
    int? SizeMl,
    int PackSize,
    decimal? WeightGrams,
    decimal Mrp,
    decimal SellingPrice,
    decimal? DiscountPercentage,
    string? Barcode,
    bool IsDefault,
    bool IsActive,
    int AvailableStock,
    int ReservedStock,
    int TotalStock,
    int ReorderLevel);
