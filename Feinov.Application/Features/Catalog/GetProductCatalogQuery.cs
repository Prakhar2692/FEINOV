using MediatR;

namespace Feinov.Application.Features.Catalog;

public sealed record GetProductCatalogQuery(int PageNumber, int PageSize) : IRequest<PagedCatalogResult>;

public sealed record PagedCatalogResult(
    IReadOnlyList<CatalogProductListItem> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages);

public sealed record CatalogProductListItem(
    Guid ProductId,
    string CategoryName,
    string SubcategoryName,
    string ProductName,
    string? DefaultImageUrl,
    decimal LowestSellingPrice,
    int VariantCount);
