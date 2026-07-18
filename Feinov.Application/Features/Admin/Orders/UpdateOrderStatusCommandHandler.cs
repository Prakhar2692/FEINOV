using Feinov.Application.Common.Interfaces;
using MediatR;

namespace Feinov.Application.Features.Admin.Orders;

public sealed class UpdateOrderStatusCommandHandler(IAdminOrderService adminOrderService)
    : IRequestHandler<UpdateOrderStatusCommand, AdminOrderStatusUpdateResult>
{
    public Task<AdminOrderStatusUpdateResult> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        => adminOrderService.UpdateOrderStatusAsync(request.OrderId, request.Status, cancellationToken);
}
