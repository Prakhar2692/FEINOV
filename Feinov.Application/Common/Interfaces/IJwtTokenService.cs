namespace Feinov.Application.Common.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(Guid userId, string mobileNumber, string? name);
}
