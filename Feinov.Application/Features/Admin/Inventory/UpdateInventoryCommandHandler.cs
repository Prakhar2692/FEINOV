using Feinov.Application.Common.Interfaces;
using MediatR;

namespace Feinov.Application.Features.Admin.Inventory;

public sealed class UpdateInventoryCommandHandler(IInventoryService inventoryService)
    : IRequestHandler<UpdateInventoryCommand, InventoryUpdateResult>
{
    public async Task<InventoryUpdateResult> Handle(UpdateInventoryCommand request, CancellationToken cancellationToken)
    {
        return await inventoryService.UpdateAsync(request.VariantId, request.TotalStock, request.ReorderLevel, cancellationToken);
    }
}
