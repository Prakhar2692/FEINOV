using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Feinov.Application.Common.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Feinov.Infrastructure.Services;

using Microsoft.Extensions.Configuration;

public class JwtTokenService : IJwtTokenService
{
    private readonly string _jwtSecret;
    private readonly string _jwtIssuer;
    private readonly string _jwtAudience;
    private readonly int _jwtExpiryMinutes;

    public JwtTokenService(IConfiguration configuration)
    {
        _jwtSecret = configuration["Jwt:Secret"] ?? throw new ArgumentNullException("Jwt:Secret");
        _jwtIssuer = configuration["Jwt:Issuer"] ?? "feinov";
        _jwtAudience = configuration["Jwt:Audience"] ?? "feinov";
        _jwtExpiryMinutes = int.TryParse(configuration["Jwt:ExpiryMinutes"], out var min) ? min : 60;
    }

    public string GenerateToken(Guid userId, string mobileNumber, string? name, string? role = null)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, mobileNumber),
            new Claim(JwtRegisteredClaimNames.Name, name ?? string.Empty),
            new Claim("userId", userId.ToString()),
            new Claim("mobileNumber", mobileNumber),
            new Claim("role", role ?? string.Empty)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: "feinov",
            audience: "feinov",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtExpiryMinutes),
            signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
