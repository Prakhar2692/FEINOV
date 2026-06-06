namespace Feinov.Application.Common.Interfaces;

public interface IDateTimeService
{
    DateTimeOffset UtcNow { get; }
}
