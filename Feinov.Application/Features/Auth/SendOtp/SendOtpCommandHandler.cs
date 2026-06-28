using Feinov.Application.Common.Interfaces;
using MediatR;

namespace Feinov.Application.Features.Auth.SendOtp;

public sealed class SendOtpCommandHandler(IOTPService otpService)
    : IRequestHandler<SendOtpCommand, SendOtpResponse>
{
    public async Task<SendOtpResponse> Handle(SendOtpCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var (transactionId, otpCode) = await otpService.GenerateAndStoreOtpAsync(
                request.MobileNumber,
                request.Purpose,
                validityMinutes: 5,
                cancellationToken: cancellationToken);

            // For development, you may want to log or return OTP. In production, send via SMS.
            // Example: await smsSender.SendAsync(request.MobileNumber, $"Your OTP is: {otpCode}");
            
            return new SendOtpResponse(
                Success: true,
                Message: $"OTP sent successfully to {request.MobileNumber}.",
                OtpTransactionId: transactionId);
        }
        catch (InvalidOperationException ex)
        {
            return new SendOtpResponse(
                Success: false,
                Message: ex.Message,
                OtpTransactionId: null);
        }
        catch (Exception ex)
        {
            return new SendOtpResponse(
                Success: false,
                Message: $"An unexpected error occurred: {ex.Message}",
                OtpTransactionId: null);
        }
    }
}
