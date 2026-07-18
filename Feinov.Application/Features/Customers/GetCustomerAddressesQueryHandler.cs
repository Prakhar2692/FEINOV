using Feinov.Application.Common.Interfaces;
using MediatR;

namespace Feinov.Application.Features.Customers;

public sealed class GetCustomerAddressesQueryHandler(ICustomerAddressService customerAddressService)
    : IRequestHandler<GetCustomerAddressesQuery, IReadOnlyList<CustomerAddressDto>>
{
    public Task<IReadOnlyList<CustomerAddressDto>> Handle(GetCustomerAddressesQuery request, CancellationToken cancellationToken)
        => customerAddressService.GetCustomerAddressesAsync(request.UserId, cancellationToken);
}
