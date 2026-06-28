using Feinov.Application.Common.Interfaces;
using MediatR;

namespace Feinov.Application.Features.Auth.VerifyOtp;

public sealed class VerifyOtpCommandHandler(
    IOTPService otpService,
    IJwtTokenService jwtTokenService,
    IDateTimeService dateTimeService)
    : IRequestHandler<VerifyOtpCommand, VerifyOtpResponse>
{
    public async Task<VerifyOtpResponse> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
    {
        // Validate and consume OTP
        var valid = await otpService.ValidateAndConsumeOtpAsync(request.MobileNumber, request.OtpCode, cancellationToken);
        if (!valid)
            return new VerifyOtpResponse(false, "Invalid or expired OTP.", null, null);

        // For now, create a transient user identity (persistence handled elsewhere)
        var userId = Guid.NewGuid();
        var token = jwtTokenService.GenerateToken(userId, request.MobileNumber, null);
        var userDto = new UserDto(userId, request.MobileNumber, null);
        return new VerifyOtpResponse(true, "OTP verified.", token, userDto);
    }
}
