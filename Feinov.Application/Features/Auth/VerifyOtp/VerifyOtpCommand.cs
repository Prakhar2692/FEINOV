using MediatR;

namespace Feinov.Application.Features.Auth.VerifyOtp;

public sealed record VerifyOtpCommand(string MobileNumber, string OtpCode) : IRequest<VerifyOtpResponse>;

public sealed record VerifyOtpResponse(
    bool Success,
    string Message,
    string? Token,
    UserDto? User
);

public sealed record UserDto(Guid UserId, string MobileNumber, string? Name);
