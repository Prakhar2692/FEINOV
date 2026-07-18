using Feinov.Application.Common.Interfaces;
using MediatR;

namespace Feinov.Application.Features.Auth.VerifyOtp;

public sealed class VerifyOtpCommandHandler(
    IOTPService otpService,
    IJwtTokenService jwtTokenService,
    IUserService userService)
    : IRequestHandler<VerifyOtpCommand, VerifyOtpResponse>
{
    public async Task<VerifyOtpResponse> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
    {
        // Validate and consume OTP
        var valid = await otpService.ValidateAndConsumeOtpAsync(request.MobileNumber, request.OtpCode, cancellationToken);
        if (!valid)
            return new VerifyOtpResponse(false, "Invalid or expired OTP.", null, null);

        var authenticatedUser = await userService.GetOrCreateUserAsync(request.MobileNumber, cancellationToken);
        var token = jwtTokenService.GenerateToken(authenticatedUser.UserId, authenticatedUser.MobileNumber, authenticatedUser.Name, authenticatedUser.Role);
        var userDto = new UserDto(authenticatedUser.UserId, authenticatedUser.MobileNumber, authenticatedUser.Name);
        return new VerifyOtpResponse(true, "OTP verified.", token, userDto);
    }
}
