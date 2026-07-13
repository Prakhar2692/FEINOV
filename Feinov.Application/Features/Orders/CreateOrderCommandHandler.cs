using Feinov.Application.Common.Interfaces;
using MediatR;

namespace Feinov.Application.Features.Orders;

public sealed class CreateOrderCommandHandler(IOrderService orderService)
    : IRequestHandler<CreateOrderCommand, CreateOrderResult>
{
    public Task<CreateOrderResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        => orderService.CreateAsync(
            request.UserId,
            request.FullName,
            request.PhoneNumber,
            request.AddressLine1,
            request.AddressLine2,
            request.City,
            request.State,
            request.PostalCode,
            request.Country,
            request.Notes,
            cancellationToken);
}
