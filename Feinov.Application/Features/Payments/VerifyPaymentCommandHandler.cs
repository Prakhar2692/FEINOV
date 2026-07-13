using Feinov.Application.Common.Interfaces;
using MediatR;

namespace Feinov.Application.Features.Payments;

public sealed class VerifyPaymentCommandHandler(IPaymentVerificationService paymentVerificationService)
    : IRequestHandler<VerifyPaymentCommand, VerifyPaymentResult>
{
    public Task<VerifyPaymentResult> Handle(VerifyPaymentCommand request, CancellationToken cancellationToken)
        => paymentVerificationService.VerifyAsync(
            request.RazorpayOrderId,
            request.RazorpayPaymentId,
            request.RazorpaySignature,
            cancellationToken);
}
