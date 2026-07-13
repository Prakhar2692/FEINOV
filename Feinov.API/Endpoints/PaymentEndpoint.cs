using Feinov.Application.Features.Payments;
using MediatR;

namespace Feinov.API.Endpoints;

public static class PaymentEndpoint
{
    public static IEndpointRouteBuilder MapPaymentEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/payments").WithTags("Payments");

        group.MapPost("/verify", VerifyPayment)
            .WithName("VerifyPayment")
            .WithSummary("Verify Razorpay payment callback and finalize the pending order")
            .WithOpenApi();

        return app;
    }

    private static async Task<IResult> VerifyPayment(VerifyPaymentRequest request, ISender sender, CancellationToken cancellationToken)
    {
        var command = new VerifyPaymentCommand(
            request.RazorpayOrderId,
            request.RazorpayPaymentId,
            request.RazorpaySignature);

        try
        {
            var result = await sender.Send(command, cancellationToken);
            return Results.Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(new { success = false, message = ex.Message });
        }
    }
}

public sealed record VerifyPaymentRequest(
    string RazorpayOrderId,
    string RazorpayPaymentId,
    string RazorpaySignature);
