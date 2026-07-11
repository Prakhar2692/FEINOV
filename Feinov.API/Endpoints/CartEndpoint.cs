using Feinov.Application.Features.Cart;
using MediatR;

namespace Feinov.API.Endpoints;

public static class CartEndpoint
{
    public static IEndpointRouteBuilder MapCartEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/cart").WithTags("Cart");

        group.MapPost("/items", AddToCart)
            .WithName("AddToCart")
            .WithSummary("Create or update cart item for a logged-in user")
            .WithOpenApi();

        group.MapPut("/items/{cartItemId:guid}/quantity", UpdateCartItemQuantity)
            .WithName("UpdateCartItemQuantity")
            .WithSummary("Update quantity for an existing cart item")
            .WithOpenApi();

        group.MapGet("/{userId:guid}", GetCart)
            .WithName("GetCart")
            .WithSummary("Return the current cart with products, images, quantities, and totals")
            .WithOpenApi();

        return app;
    }

    private static async Task<IResult> AddToCart(AddToCartRequest request, ISender sender, CancellationToken cancellationToken)
    {
        var command = new AddToCartCommand(request.UserId, request.VariantId, request.Quantity);

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

    private static async Task<IResult> UpdateCartItemQuantity(Guid cartItemId, UpdateCartItemQuantityRequest request, ISender sender, CancellationToken cancellationToken)
    {
        var command = new UpdateCartItemQuantityCommand(request.UserId, cartItemId, request.Quantity);

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

    private static async Task<IResult> GetCart(Guid userId, ISender sender, CancellationToken cancellationToken)
    {
        var query = new GetCartQuery(userId);
        var result = await sender.Send(query, cancellationToken);
        return Results.Ok(result);
    }
}

public sealed record AddToCartRequest(Guid UserId, Guid VariantId, int Quantity);
public sealed record UpdateCartItemQuantityRequest(Guid UserId, int Quantity);
