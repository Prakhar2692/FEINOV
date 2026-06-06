using Feinov.Application.Common.Interfaces;
using MediatR;

namespace Feinov.Application.Features.Health.GetHealthStatus;

public sealed record GetHealthStatusQuery : IRequest<HealthStatusDto>;

public sealed record HealthStatusDto(string Status, DateTimeOffset CheckedAtUtc);

public sealed class GetHealthStatusQueryHandler(IDateTimeService dateTimeService)
    : IRequestHandler<GetHealthStatusQuery, HealthStatusDto>
{
    public Task<HealthStatusDto> Handle(GetHealthStatusQuery request, CancellationToken cancellationToken)
    {
        var result = new HealthStatusDto("Healthy", dateTimeService.UtcNow);
        return Task.FromResult(result);
    }
}
