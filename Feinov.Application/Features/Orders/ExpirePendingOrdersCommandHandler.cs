using Feinov.Application.Common.Interfaces;
using MediatR;

namespace Feinov.Application.Features.Orders;

public sealed class ExpirePendingOrdersCommandHandler(IOrderExpirationService orderExpirationService)
    : IRequestHandler<ExpirePendingOrdersCommand, Unit>
{
    public async Task<Unit> Handle(ExpirePendingOrdersCommand request, CancellationToken cancellationToken)
    {
        await orderExpirationService.ExpirePendingOrdersAsync(cancellationToken);
        return Unit.Value;
    }
}
