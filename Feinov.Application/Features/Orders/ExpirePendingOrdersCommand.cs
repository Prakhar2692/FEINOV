using MediatR;

namespace Feinov.Application.Features.Orders;

public sealed record ExpirePendingOrdersCommand() : IRequest<Unit>;
