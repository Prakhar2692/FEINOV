using Feinov.Application.Features.Admin.Categories;
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
}

public sealed record CreateCategoryRequest(string CategoryName, string? Description);
