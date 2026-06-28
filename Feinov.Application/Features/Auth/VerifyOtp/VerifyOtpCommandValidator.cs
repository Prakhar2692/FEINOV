using FluentValidation;

namespace Feinov.Application.Features.Auth.VerifyOtp;

public sealed class VerifyOtpCommandValidator : AbstractValidator<VerifyOtpCommand>
{
    public VerifyOtpCommandValidator()
    {
        RuleFor(x => x.MobileNumber)
            .NotEmpty().WithMessage("Mobile number is required.")
            .Matches(@"^\d{10}$").WithMessage("Mobile number must be a valid 10-digit number.");
        RuleFor(x => x.OtpCode)
            .NotEmpty().WithMessage("OTP is required.")
            .Length(6).WithMessage("OTP must be 6 digits.");
    }
}
