using MediatR;

namespace Feinov.Application.Features.Admin.Inventory;

public sealed record GetInventoryListQuery(int PageNumber, int PageSize) : IRequest<PagedInventoryResult>;

public sealed record PagedInventoryResult(
    IReadOnlyList<InventoryListItem> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages);

public sealed record InventoryListItem(
    Guid VariantId,
    string ProductName,
    string VariantName,
    int TotalStock,
    int ReservedStock,
    int AvailableStock,
    int ReorderLevel);
