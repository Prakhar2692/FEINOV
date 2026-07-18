using Feinov.Application.Features.Orders;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

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

        group.MapPost("/history", GetOrderHistory)
            .WithName("GetOrderHistory")
            .WithSummary("Gets order history from the logged-in user's cart and persist a delivery snapshot")
            .WithOpenApi();

        group.MapGet("/{orderId:guid}", GetOrderDetails)
            .WithName("GetOrderDetails")
            .WithSummary("Gets a single customer order with items, payment status, and shipping address")
            .WithOpenApi();

        return app;
    }
private static async Task<IResult> GetOrderDetails(Guid orderId, Guid userId, ISender sender, CancellationToken cancellationToken)
    {
        var query = new GetCustomerOrderDetailsQuery(userId, orderId);

        try
        {
            var result = await sender.Send(query, cancellationToken);
            return Results.Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(new { success = false, message = ex.Message });
        }
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

private static async Task<IResult> GetOrderHistory(CustomerOrderHistoryRequest request, ISender sender, CancellationToken cancellationToken)
{
        var query = new GetCustomerOrderHistoryQuery(
            request.UserId,
            request.PageNumber,
            request.PageSize
        );

        try
        {
            var result = await sender.Send(query, cancellationToken);
            return Results.Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(new { success = false, message = ex.Message });
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


public sealed record CustomerOrderHistoryRequest(Guid UserId, int PageNumber, int PageSize);

}
