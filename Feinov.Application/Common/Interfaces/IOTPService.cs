namespace Feinov.Application.Common.Interfaces;

public interface IOTPService
{
    Task<(Guid TransactionId, string OtpCode)> GenerateAndStoreOtpAsync(
        string mobileNumber,
        string purpose,
        int validityMinutes = 5,
        CancellationToken cancellationToken = default);

    Task<bool> ValidateOtpAsync(
        string mobileNumber,
        string otpCode,
        CancellationToken cancellationToken = default);

    Task<bool> MarkOtpAsUsedAsync(
        Guid otpTransactionId,
        CancellationToken cancellationToken = default);
}
