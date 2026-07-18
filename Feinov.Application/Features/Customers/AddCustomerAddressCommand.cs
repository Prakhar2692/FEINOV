using MediatR;

namespace Feinov.Application.Features.Customers;

public sealed record AddCustomerAddressCommand(
    Guid UserId,
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
    bool IsDefault) : IRequest<AddCustomerAddressResult>;

public sealed record AddCustomerAddressResult(
    Guid AddressId,
    bool IsDefault);
