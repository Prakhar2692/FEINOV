using Feinov.Application.Features.Health.GetHealthStatus;
using MediatR;

namespace Feinov.API.Endpoints;

public static class HealthEndpoints
{
    public static IEndpointRouteBuilder MapHealthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/health")
            .WithTags("Health");

        group.MapGet("/", async (ISender sender, CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(new GetHealthStatusQuery(), cancellationToken);
                return Results.Ok(result);
            })
            .WithName("GetHealthStatus")
            .WithSummary("Returns application health status.");

        return app;
    }
}
