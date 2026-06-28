using Feinov.Application.Features.Admin.Categories;
using Feinov.Application.Features.Admin.Inventory;
using Feinov.Application.Features.Admin.Products;
using MediatR;

namespace Feinov.API.Endpoints;

public static class AdminEndpoints
{
    public static IEndpointRouteBuilder MapAdminEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/admin").WithTags("Admin");

        group.MapPost("/categories", CreateCategory)
            .WithName("CreateCategory")
            .WithSummary("Create a new category")
            .WithOpenApi();

        group.MapPost("/categories/{categoryId:guid}/subcategories", CreateSubcategory)
            .WithName("CreateSubcategory")
            .WithSummary("Create a new subcategory under a category")
            .WithOpenApi();

        group.MapPost("/products", CreateProduct)
            .WithName("CreateProduct")
            .WithSummary("Create a new product")
            .WithOpenApi();

        group.MapPost("/products/{productId:guid}/variants", CreateProductVariant)
            .WithName("CreateProductVariant")
            .WithSummary("Create a new product variant")
            .WithOpenApi();

        group.MapPost("/products/{productId:guid}/images", UploadProductImages)
            .WithName("UploadProductImages")
            .WithSummary("Upload one or more product images")
            .WithOpenApi();

        group.MapPut("/inventory/{variantId:guid}", UpdateInventory)
            .WithName("UpdateInventory")
            .WithSummary("Update inventory stock and reorder level")
            .WithOpenApi();

        group.MapGet("/inventory", GetInventoryList)
            .WithName("GetInventoryList")
            .WithSummary("List inventory records with paging")
            .WithOpenApi();

        return app;
    }

    private static async Task<IResult> CreateCategory(CreateCategoryRequest request, ISender sender, CancellationToken cancellationToken)
    {
        var command = new CreateCategoryCommand(request.CategoryName, request.Description);
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

    private static async Task<IResult> CreateSubcategory(Guid categoryId, CreateSubcategoryRequest request, ISender sender, CancellationToken cancellationToken)
    {
        var command = new CreateSubcategoryCommand(categoryId, request.SubcategoryName, request.Description);
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

    private static async Task<IResult> CreateProduct(CreateProductRequest request, ISender sender, CancellationToken cancellationToken)
    {
        var command = new CreateProductCommand(request.SubcategoryId, request.ProductName, request.Description, request.Brand);
        try
        {
            var result = await sender.Send(command, cancellationToken);
            return Results.Ok(new { productId = result });
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(new { success = false, message = ex.Message });
        }
    }

    private static async Task<IResult> CreateProductVariant(Guid productId, CreateProductVariantRequest request, ISender sender, CancellationToken cancellationToken)
    {
        var command = new CreateProductVariantCommand(productId, request.Sku, request.VariantName, request.SizeMl, request.Mrp, request.SellingPrice, request.Barcode);
        try
        {
            var result = await sender.Send(command, cancellationToken);
            return Results.Ok(new { variantId = result });
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(new { success = false, message = ex.Message });
        }
    }

    private static async Task<IResult> UploadProductImages(Guid productId, IFormFileCollection files, ISender sender, CancellationToken cancellationToken)
    {
        var command = new UploadProductImagesCommand(productId, files);
        try
        {
            var result = await sender.Send(command, cancellationToken);
            return Results.Ok(new { imageUrls = result });
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(new { success = false, message = ex.Message });
        }
    }

    private static async Task<IResult> UpdateInventory(Guid variantId, UpdateInventoryRequest request, ISender sender, CancellationToken cancellationToken)
    {
        var command = new UpdateInventoryCommand(variantId, request.TotalStock, request.ReorderLevel);
        try
        {
            var result = await sender.Send(command, cancellationToken);
            return Results.Ok(new
            {
                availableStock = result.AvailableStock,
                totalStock = result.TotalStock,
                reorderLevel = result.ReorderLevel
            });
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(new { success = false, message = ex.Message });
        }
    }

    private static async Task<IResult> GetInventoryList([AsParameters] InventoryListRequest request, ISender sender, CancellationToken cancellationToken)
    {
        var query = new GetInventoryListQuery(request.PageNumber, request.PageSize);
        var result = await sender.Send(query, cancellationToken);
        return Results.Ok(result);
    }
}

public sealed record CreateCategoryRequest(string CategoryName, string? Description);

public sealed record CreateSubcategoryRequest(string SubcategoryName, string? Description);

public sealed record CreateProductRequest(Guid SubcategoryId, string ProductName, string? Description, string? Brand);

public sealed record CreateProductVariantRequest(string Sku, string VariantName, int? SizeMl, decimal Mrp, decimal SellingPrice, string? Barcode);

public sealed record UpdateInventoryRequest(int TotalStock, int ReorderLevel);

public sealed record InventoryListRequest(int PageNumber = 1, int PageSize = 20);
