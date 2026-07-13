namespace Feinov.Application.Features.Payments;

public sealed record VerifyPaymentResult(
    Guid OrderId,
    string OrderNumber,
    string OrderStatus,
    string PaymentStatus,
    bool IsSuccess,
    string Message);
