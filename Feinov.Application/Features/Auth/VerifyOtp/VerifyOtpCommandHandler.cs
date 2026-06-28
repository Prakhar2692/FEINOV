using Feinov.Application.Common.Interfaces;
using Feinov.Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Feinov.Application.Features.Auth.VerifyOtp;

public sealed class VerifyOtpCommandHandler(
    IOTPService otpService,
    IJwtTokenService jwtTokenService,
    Context dbContext,
    IDateTimeService dateTimeService)
    : IRequestHandler<VerifyOtpCommand, VerifyOtpResponse>
{
    public async Task<VerifyOtpResponse> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
    {
        // Validate OTP
        var now = dateTimeService.UtcNow.UtcDateTime;
        var otp = await dbContext.OtpTransactions
            .FirstOrDefaultAsync(x => x.MobileNumber == request.MobileNumber && x.OtpCode == request.OtpCode, cancellationToken);
        if (otp == null)
            return new VerifyOtpResponse(false, "Invalid OTP.", null, null);
        if (otp.IsUsed)
            return new VerifyOtpResponse(false, "OTP already used.", null, null);
        if (otp.ExpiresAt < now)
            return new VerifyOtpResponse(false, "OTP expired.", null, null);

        // Mark OTP as used
        otp.IsUsed = true;
        await dbContext.SaveChangesAsync(cancellationToken);

        // Find or create user
        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.MobileNumber == request.MobileNumber, cancellationToken);
        if (user == null)
        {
            user = new User
            {
                UserId = Guid.NewGuid(),
                MobileNumber = request.MobileNumber,
                IsMobileVerified = true,
                IsActive = true,
                CreatedDate = now,
                RoleId = Guid.Empty // Set default role if needed
            };
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        // Generate JWT
        var token = jwtTokenService.GenerateToken(user.UserId, user.MobileNumber, user.FirstName, user.Role?.RoleName);
        var userDto = new UserDto(user.UserId, user.MobileNumber, user.FirstName);
        return new VerifyOtpResponse(true, "OTP verified.", token, userDto);
    }
}
