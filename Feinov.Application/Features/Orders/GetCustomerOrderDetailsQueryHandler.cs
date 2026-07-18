using Feinov.Application.Common.Interfaces;
using MediatR;

namespace Feinov.Application.Features.Orders;

public sealed class GetCustomerOrderDetailsQueryHandler(IOrderService orderService)
    : IRequestHandler<GetCustomerOrderDetailsQuery, CustomerOrderDetailsResult>
{
    public Task<CustomerOrderDetailsResult> Handle(GetCustomerOrderDetailsQuery request, CancellationToken cancellationToken)
        => orderService.GetCustomerOrderDetailsAsync(request.UserId, request.OrderId, cancellationToken);
}
