using Feinov.Application.Common.Interfaces;
using MediatR;

namespace Feinov.Application.Features.Admin.Inventory;

public sealed class GetInventoryListQueryHandler(IInventoryService inventoryService)
    : IRequestHandler<GetInventoryListQuery, PagedInventoryResult>
{
    public Task<PagedInventoryResult> Handle(GetInventoryListQuery request, CancellationToken cancellationToken)
        => inventoryService.GetPagedAsync(request.PageNumber, request.PageSize, cancellationToken);
}
