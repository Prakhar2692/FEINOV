using Feinov.Application.Features.Orders;
using MediatR;

namespace Feinov.API.Endpoints;

public static class OrderEndpoint
{
    public static IEndpointRouteBuilder MapOrderEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/orders").WithTags("Orders");

        group.MapPost("", CreateOrder)
            .WithName("CreateOrder")
            .WithSummary("Create an order from the logged-in user's cart and persist a delivery snapshot")
            .WithOpenApi();

        return app;
    }

    private static async Task<IResult> CreateOrder(CreateOrderRequest request, ISender sender, CancellationToken cancellationToken)
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

public sealed record CreateOrderRequest(
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
