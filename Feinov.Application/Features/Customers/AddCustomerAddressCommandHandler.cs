using Feinov.Application.Common.Interfaces;
using MediatR;

namespace Feinov.Application.Features.Customers;

public sealed class AddCustomerAddressCommandHandler(ICustomerAddressService customerAddressService)
    : IRequestHandler<AddCustomerAddressCommand, AddCustomerAddressResult>
{
    public Task<AddCustomerAddressResult> Handle(AddCustomerAddressCommand request, CancellationToken cancellationToken)
        => customerAddressService.AddCustomerAddressAsync(
            request.UserId,
            request.AddressType,
            request.FullName,
            request.MobileNumber,
            request.AddressLine1,
            request.AddressLine2,
            request.Landmark,
            request.City,
            request.State,
            request.PostalCode,
            request.Country,
            request.IsDefault,
            cancellationToken);
}
