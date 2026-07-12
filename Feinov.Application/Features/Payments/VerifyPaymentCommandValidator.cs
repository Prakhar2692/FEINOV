using FluentValidation;

namespace Feinov.Application.Features.Payments;

public sealed class VerifyPaymentCommandValidator : AbstractValidator<VerifyPaymentCommand>
{
    public VerifyPaymentCommandValidator()
    {
        RuleFor(x => x.RazorpayOrderId)
            .NotEmpty().WithMessage("Razorpay order id is required.");

        RuleFor(x => x.RazorpayPaymentId)
            .NotEmpty().WithMessage("Razorpay payment id is required.");

        RuleFor(x => x.RazorpaySignature)
            .NotEmpty().WithMessage("Razorpay signature is required.");
    }
}
