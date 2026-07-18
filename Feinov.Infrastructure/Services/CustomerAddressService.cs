using Feinov.Application.Common.Interfaces;
using Feinov.Application.Features.Customers;
using Feinov.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Feinov.Infrastructure.Services;

public sealed class CustomerAddressService(Context dbContext) : ICustomerAddressService
{
    public async Task<AddCustomerAddressResult> AddCustomerAddressAsync(
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
        CancellationToken cancellationToken = default)
    {
        if (userId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(userId), "User id is required.");

        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentOutOfRangeException(nameof(fullName), "Full name is required.");

        if (string.IsNullOrWhiteSpace(mobileNumber))
            throw new ArgumentOutOfRangeException(nameof(mobileNumber), "Mobile number is required.");

        if (string.IsNullOrWhiteSpace(addressLine1))
            throw new ArgumentOutOfRangeException(nameof(addressLine1), "Address line1 is required.");

        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentOutOfRangeException(nameof(city), "City is required.");

        if (string.IsNullOrWhiteSpace(state))
            throw new ArgumentOutOfRangeException(nameof(state), "State is required.");

        if (string.IsNullOrWhiteSpace(postalCode))
            throw new ArgumentOutOfRangeException(nameof(postalCode), "Postal code is required.");

        if (string.IsNullOrWhiteSpace(country))
            throw new ArgumentOutOfRangeException(nameof(country), "Country is required.");

        var userExists = await dbContext.Users.AnyAsync(x => x.UserId == userId && x.IsActive, cancellationToken);
        if (!userExists)
            throw new InvalidOperationException("User does not exist.");

        if (isDefault)
        {
            var existingDefaultAddresses = await dbContext.CustomerAddresses
                .Where(x => x.UserId == userId && x.IsDefault)
                .ToListAsync(cancellationToken);

            foreach (var existingAddress in existingDefaultAddresses)
            {
                existingAddress.IsDefault = false;
                existingAddress.UpdatedDate = DateTime.UtcNow;
            }
        }

        var address = new CustomerAddress
        {
            AddressId = Guid.NewGuid(),
            UserId = userId,
            AddressType = addressType,
            FullName = fullName,
            MobileNumber = mobileNumber,
            AddressLine1 = addressLine1,
            AddressLine2 = addressLine2,
            Landmark = landmark,
            City = city,
            State = state,
            PostalCode = postalCode,
            Country = country,
            IsDefault = isDefault,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        dbContext.CustomerAddresses.Add(address);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new AddCustomerAddressResult(address.AddressId, address.IsDefault);
    }

    public async Task<IReadOnlyList<CustomerAddressDto>> GetCustomerAddressesAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        if (userId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(userId), "User id is required.");

        var userExists = await dbContext.Users.AnyAsync(x => x.UserId == userId && x.IsActive, cancellationToken);
        if (!userExists)
            throw new InvalidOperationException("User does not exist.");

        return await dbContext.CustomerAddresses
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.IsDefault)
            .ThenByDescending(x => x.CreatedDate)
            .Select(x => new CustomerAddressDto(
                x.AddressId,
                x.AddressType,
                x.FullName,
                x.MobileNumber,
                x.AddressLine1,
                x.AddressLine2,
                x.Landmark,
                x.City,
                x.State,
                x.PostalCode,
                x.Country,
                x.IsDefault))
            .ToListAsync(cancellationToken);
    }
}
