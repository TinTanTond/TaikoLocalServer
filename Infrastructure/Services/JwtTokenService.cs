using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using Domain.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

public class JwtTokenService(IOptions<AuthSettings> options) : IJwtTokenService
{
    public string GenerateToken(uint baid, bool isAdmin)
    {
        var authSettings = options.Value;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.JwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, baid.ToString()),
            new(ClaimTypes.Role, isAdmin ? "Admin" : "User")
        };

        var token = new JwtSecurityToken(
            issuer: authSettings.JwtIssuer,
            audience: authSettings.JwtAudience,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials,
            claims: claims
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}