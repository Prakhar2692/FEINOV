using MediatR;

namespace Feinov.Application.Features.Admin.Inventory;

public sealed record UpdateInventoryCommand(
    Guid VariantId,
    int TotalStock,
    int ReorderLevel) : IRequest<InventoryUpdateResult>;

public sealed record InventoryUpdateResult(int AvailableStock, int TotalStock, int ReorderLevel);
