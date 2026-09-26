using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Anexcs.Distro.Application.Common.Interfaces;
using Anexcs.Distro.Domain.Entities.Central;
using Anexcs.Distro.Domain.Entities.Tenants;
using JasperFx.Core;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Anexcs.Distro.Infrastructure.Authentication;

public class JwtTokenGenerator(IOptions<JwtSettings> jwtOptions) : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;

    public string GenerateCentralUserToken(CentralUser user, IList<string> roles)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim("user_type", "central")
        };
        
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        return GenerateToken(claims);
    }

    public string GenerateTenantUserToken(TenantUser user, string tenantId, IList<string> roles)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim("tenant_id", tenantId),
            new Claim("user_type", "tenant")
        };
        
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        
        return GenerateToken(claims);
    }

    private string GenerateToken(IEnumerable<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}