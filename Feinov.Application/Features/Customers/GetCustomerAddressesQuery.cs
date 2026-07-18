using MediatR;

namespace Feinov.Application.Features.Customers;

public sealed record GetCustomerAddressesQuery(Guid UserId) : IRequest<IReadOnlyList<CustomerAddressDto>>;
