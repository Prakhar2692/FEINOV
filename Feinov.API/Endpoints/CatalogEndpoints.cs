using Feinov.Application.Features.Catalog;
using MediatR;

namespace Feinov.API.Endpoints;

public static class CatalogEndpoints
{
    public static IEndpointRouteBuilder MapCatalogEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/catalog").WithTags("Catalog");

        group.MapGet("/products", GetProductCatalog)
            .WithName("GetProductCatalog")
            .WithSummary("List catalog products with paging")
            .WithOpenApi();

        group.MapGet("/products/{productId:guid}", GetProductDetails)
            .WithName("GetProductDetails")
            .WithSummary("Return product details with variants, images, stock, and category info")
            .WithOpenApi();

        return app;
    }

    private static async Task<IResult> GetProductCatalog([AsParameters] CatalogListRequest request, ISender sender, CancellationToken cancellationToken)
    {
        var query = new GetProductCatalogQuery(request.PageNumber, request.PageSize);
        var result = await sender.Send(query, cancellationToken);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetProductDetails(Guid productId, ISender sender, CancellationToken cancellationToken)
    {
        var query = new GetProductDetailsQuery(productId);
        var result = await sender.Send(query, cancellationToken);
        return Results.Ok(result);
    }
}

public sealed record CatalogListRequest(int PageNumber = 1, int PageSize = 20);
