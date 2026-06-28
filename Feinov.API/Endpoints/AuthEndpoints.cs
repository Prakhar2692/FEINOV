using Feinov.Application.Features.Auth.SendOtp;
using Feinov.Application.Features.Auth.VerifyOtp;
using MediatR;

namespace Feinov.API.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth")
            .WithTags("Auth");

        group.MapPost("/send-otp", SendOtp)
            .WithName("SendOtp")
            .WithSummary("Send OTP to mobile number")
            .WithOpenApi();

        group.MapPost("/verify-otp", VerifyOtp)
            .WithName("VerifyOtp")
            .WithSummary("Verify OTP and login/register user")
            .WithOpenApi();

        return app;
    }

    private static async Task<IResult> SendOtp(SendOtpRequest request, ISender sender, CancellationToken cancellationToken)
    {
        var command = new SendOtpCommand(request.MobileNumber, request.Purpose);
        var result = await sender.Send(command, cancellationToken);

        if (result.Success)
        {
            return Results.Ok(new
            {
                success = true,
                message = result.Message,
                otpTransactionId = result.OtpTransactionId
            });
        }

        return Results.BadRequest(new
        {
            success = false,
            message = result.Message
        });
    }

    private static async Task<IResult> VerifyOtp(VerifyOtpRequest request, ISender sender, CancellationToken cancellationToken)
    {
        var command = new VerifyOtpCommand(request.MobileNumber, request.OtpCode);
        var result = await sender.Send(command, cancellationToken);

        if (result.Success)
        {
            return Results.Ok(new
            {
                success = true,
                message = result.Message,
                token = result.Token,
                user = result.User
            });
        }

        return Results.BadRequest(new
        {
            success = false,
            message = result.Message
        });
    }
}

public sealed record SendOtpRequest(string MobileNumber, string Purpose = "Login");
public sealed record VerifyOtpRequest(string MobileNumber, string OtpCode);
