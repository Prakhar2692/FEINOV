using Feinov.Application.Features.Admin.Inventory;

namespace Feinov.Application.Common.Interfaces;

public interface IInventoryService
{
    Task<InventoryUpdateResult> UpdateAsync(
        Guid variantId,
        int totalStock,
        int reorderLevel,
        CancellationToken cancellationToken = default);

    Task<PagedInventoryResult> GetPagedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
}
