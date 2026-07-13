using Feinov.Application.Features.Catalog;

namespace Feinov.Application.Common.Interfaces;

public interface ICatalogService
{
    Task<PagedCatalogResult> GetPagedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<ProductDetailsResult> GetByIdAsync(
        Guid productId,
        CancellationToken cancellationToken = default);
}
