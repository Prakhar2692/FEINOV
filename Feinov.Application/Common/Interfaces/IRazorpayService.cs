using Feinov.Application.Features.Orders;

namespace Feinov.Application.Common.Interfaces;

public interface IRazorpayService
{
    Task<RazorpayOrderDetails> CreateOrderAsync(
        Guid orderId,
        string orderNumber,
        decimal totalAmount,
        string currency = "INR",
        CancellationToken cancellationToken = default);
}
