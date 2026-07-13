using Feinov.Application.Common.Interfaces;
using MediatR;

namespace Feinov.Application.Features.Orders;

public sealed class GetCustomerOrderHistoryQueryHandler(IOrderService orderService)
    : IRequestHandler<GetCustomerOrderHistoryQuery, PagedCustomerOrderHistoryResult>
{
    public Task<PagedCustomerOrderHistoryResult> Handle(GetCustomerOrderHistoryQuery request, CancellationToken cancellationToken)
        => orderService.GetCustomerOrderHistoryAsync(request.UserId, request.PageNumber, request.PageSize, cancellationToken);
}
