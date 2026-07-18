using Feinov.Application.Features.Customers;

namespace Feinov.Application.Common.Interfaces;

public interface ICustomerAddressService
{
    Task<AddCustomerAddressResult> AddCustomerAddressAsync(
        Guid userId,
        string? addressType,
        string fullName,
        string mobileNumber,
        string addressLine1,
        string? addressLine2,
        string? landmark,
        string city,
        string state,
        string postalCode,
        string country,
        bool isDefault,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CustomerAddressDto>> GetCustomerAddressesAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}
