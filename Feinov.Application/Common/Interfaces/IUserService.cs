namespace Feinov.Application.Common.Interfaces;

public interface IUserService
{
    Task<AuthenticatedUser> GetOrCreateUserAsync(string mobileNumber, CancellationToken cancellationToken = default);
}

public sealed record AuthenticatedUser(Guid UserId, string MobileNumber, string? Name, string Role);
