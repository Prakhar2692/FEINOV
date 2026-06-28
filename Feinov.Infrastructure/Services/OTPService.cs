using Feinov.Application.Common.Interfaces;
using Feinov.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Feinov.Infrastructure.Services;

public class OTPService(Context dbContext, IDateTimeService dateTimeService) : IOTPService
{
    public async Task<(Guid TransactionId, string OtpCode)> GenerateAndStoreOtpAsync(
        string mobileNumber,
        string purpose,
        int validityMinutes = 5,
        CancellationToken cancellationToken = default)
    {
        var otpCode = GenerateOtpCode();
        var now = dateTimeService.UtcNow.UtcDateTime;
        var expiresAt = now.AddMinutes(validityMinutes);

        var otpTransaction = new OtpTransaction
        {
            OtpTransactionId = Guid.NewGuid(),
            MobileNumber = mobileNumber,
            OtpCode = otpCode,
            Purpose = purpose,
            IsUsed = false,
            CreatedDate = now,
            ExpiresAt = expiresAt
        };

        try
        {
            dbContext.OtpTransactions.Add(otpTransaction);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to store OTP for mobile number {mobileNumber}. Error: {ex.Message}", ex);
        }

        return (otpTransaction.OtpTransactionId, otpCode);
    }

    public async Task<bool> ValidateOtpAsync(
        string mobileNumber,
        string otpCode,
        CancellationToken cancellationToken = default)
    {
        var now = dateTimeService.UtcNow.UtcDateTime;

        try
        {
            var otpTransaction = await dbContext.OtpTransactions
                .FirstOrDefaultAsync(
                    x => x.MobileNumber == mobileNumber &&
                         x.OtpCode == otpCode &&
                         !x.IsUsed &&
                         x.ExpiresAt > now,
                    cancellationToken: cancellationToken);

            return otpTransaction != null;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> MarkOtpAsUsedAsync(
        Guid otpTransactionId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var otpTransaction = await dbContext.OtpTransactions
                .FirstOrDefaultAsync(x => x.OtpTransactionId == otpTransactionId, cancellationToken: cancellationToken);

            if (otpTransaction == null)
                return false;

            otpTransaction.IsUsed = true;
            await dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch
        {
            return false;
        }
    }

    private static string GenerateOtpCode()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }
}

