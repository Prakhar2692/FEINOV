using MediatR;

namespace Feinov.Application.Features.Auth.SendOtp;

public sealed record SendOtpCommand(string MobileNumber, string Purpose = "Login") : IRequest<SendOtpResponse>;

public sealed record SendOtpResponse(
    bool Success,
    string Message,
    Guid? OtpTransactionId
);
