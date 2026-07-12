using Feinov.Application.Features.Payments;

namespace Feinov.Application.Common.Interfaces;

public interface IPaymentVerificationService
{
    Task<VerifyPaymentResult> VerifyAsync(
        string razorpayOrderId,
        string razorpayPaymentId,
        string razorpaySignature,
        CancellationToken cancellationToken = default);
}
