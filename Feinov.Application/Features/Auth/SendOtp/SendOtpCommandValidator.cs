using FluentValidation;

namespace Feinov.Application.Features.Auth.SendOtp;

public sealed class SendOtpCommandValidator : AbstractValidator<SendOtpCommand>
{
    public SendOtpCommandValidator()
    {
        RuleFor(x => x.MobileNumber)
            .NotEmpty().WithMessage("Mobile number is required.")
            .Matches(@"^\d{10}$").WithMessage("Mobile number must be a valid 10-digit number.");

        RuleFor(x => x.Purpose)
            .NotEmpty().WithMessage("Purpose is required.")
            .MaximumLength(50).WithMessage("Purpose must not exceed 50 characters.");
    }
}
