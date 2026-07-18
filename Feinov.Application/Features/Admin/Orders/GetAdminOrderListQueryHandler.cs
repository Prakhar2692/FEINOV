using MediatR;
using Feinov.Application.Common.Interfaces;

namespace Feinov.Application.Features.Admin.Orders;

public sealed class GetAdminOrderListQueryHandler(IAdminOrderService adminOrderService)
    : IRequestHandler<GetAdminOrderListQuery, PagedAdminOrderListResult>
{
    public Task<PagedAdminOrderListResult> Handle(GetAdminOrderListQuery request, CancellationToken cancellationToken)
        => adminOrderService.GetOrderListAsync(request.OrderNumber, request.Status, request.PageNumber, request.PageSize, cancellationToken);
}
