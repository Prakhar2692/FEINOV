using Feinov.Application.Features.Orders;

namespace Feinov.Application.Common.Interfaces;

public interface IOrderService
{
    Task<CreateOrderResult> CreateAsync(
        Guid userId,
        string fullName,
        string phoneNumber,
        string addressLine1,
        string? addressLine2,
        string city,
        string state,
        string postalCode,
        string country,
        string? notes,
        CancellationToken cancellationToken = default);

    Task<PagedCustomerOrderHistoryResult> GetCustomerOrderHistoryAsync(
        Guid userId,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<CustomerOrderDetailsResult> GetCustomerOrderDetailsAsync(
        Guid userId,
        Guid orderId,
        CancellationToken cancellationToken = default);
}
