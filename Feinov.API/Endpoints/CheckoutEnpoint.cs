using Feinov.Application.Features.Orders;
using MediatR;

namespace Feinov.API.Endpoints;

public static class CheckoutEnpoint
{
    public static IEndpointRouteBuilder MapCheckoutEnpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/checkout").WithTags("Checkout");

        group.MapPost("", Checkout)
            .WithName("Checkout")
            .WithSummary("Reserve stock from the cart and create a pending-payment order")
            .WithOpenApi();

        return app;
    }

    private static async Task<IResult> Checkout(CheckoutRequest request, ISender sender, CancellationToken cancellationToken)
    {
        var command = new CreateOrderCommand(
            request.UserId,
            request.FullName,
            request.PhoneNumber,
            request.AddressLine1,
            request.AddressLine2,
            request.City,
            request.State,
            request.PostalCode,
            request.Country,
            request.Notes);

        try
        {
            var result = await sender.Send(command, cancellationToken);
            return Results.Created($"/api/orders/{result.OrderId}", result);
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(new { success = false, message = ex.Message });
        }
    }
}

public sealed record CheckoutRequest(
    Guid UserId,
    string FullName,
    string PhoneNumber,
    string AddressLine1,
    string? AddressLine2,
    string City,
    string State,
    string PostalCode,
    string Country,
    string? Notes = null);
