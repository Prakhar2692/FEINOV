using MediatR;

namespace Feinov.Application.Features.Admin.Orders;

public sealed record UpdateOrderStatusCommand(Guid OrderId, string Status) : IRequest<AdminOrderStatusUpdateResult>;

public sealed record AdminOrderStatusUpdateResult(Guid OrderId, string OrderNumber, string OrderStatus);
