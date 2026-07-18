namespace Feinov.Application.Features.Customers;

public sealed record CustomerAddressDto(
    Guid AddressId,
    string? AddressType,
    string FullName,
    string MobileNumber,
    string AddressLine1,
    string? AddressLine2,
    string? Landmark,
    string City,
    string State,
    string PostalCode,
    string Country,
    bool IsDefault);
