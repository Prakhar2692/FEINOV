using MediatR;

namespace Feinov.Application.Features.Payments;

public sealed record VerifyPaymentCommand(
    string RazorpayOrderId,
    string RazorpayPaymentId,
    string RazorpaySignature) : IRequest<VerifyPaymentResult>;
