using Feinov.Application.Features.Customers;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Feinov.API.Endpoints;

public static class CustomerEndpoints
{
    public static IEndpointRouteBuilder MapCustomerEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/customers").WithTags("Customers");

        group.MapPost("/addresses", AddCustomerAddress)
            .WithName("AddCustomerAddress")
            .WithSummary("Save a customer address and optionally make it the default")
            .WithOpenApi();

        group.MapGet("/{userId:guid}/addresses", GetCustomerAddresses)
            .WithName("GetCustomerAddresses")
            .WithSummary("List all customer addresses and mark the default address")
            .WithOpenApi();

        return app;
    }

    private static async Task<IResult> AddCustomerAddress(AddCustomerAddressRequest request, ISender sender, CancellationToken cancellationToken)
    {
        var command = new AddCustomerAddressCommand(
            request.UserId,
            request.AddressType,
            request.FullName,
            request.MobileNumber,
            request.AddressLine1,
            request.AddressLine2,
            request.Landmark,
            request.City,
            request.State,
            request.PostalCode,
            request.Country,
            request.IsDefault);

        try
        {
            var result = await sender.Send(command, cancellationToken);
            return Results.Created($"/api/customers/addresses/{result.AddressId}", result);
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(new { success = false, message = ex.Message });
        }
    }

    private static async Task<IResult> GetCustomerAddresses(Guid userId, ISender sender, CancellationToken cancellationToken)
    {
        try
        {
            var result = await sender.Send(new GetCustomerAddressesQuery(userId), cancellationToken);
            return Results.Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(new { success = false, message = ex.Message });
        }
    }
}

public sealed record AddCustomerAddressRequest(
    Guid UserId,
    string? AddressType,
    string FullName,
    string MobileNumber,
    string AddressLine1,
    string? AddressLine2,
    string? Landmark,
    string City,
    string State,
    string PostalCode,
    string Country,
    bool IsDefault = false);
