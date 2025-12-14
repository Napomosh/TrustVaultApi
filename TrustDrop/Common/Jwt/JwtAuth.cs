using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using TrustDrop.User.General;

namespace TrustDrop.Common.Jwt;

public static class JwtAuth
{
    public static JwtSettings jwtSettings { get; set; } = new ();

    public static string GenerateJwtToken(Guid userId, UserRole role
        , string login, int secToLive)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Jti, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.NameId, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, login),
            // new Claim("tenantId", tenantId.ToString()),
            new Claim("role", role.ToString()),
            new Claim(JwtRegisteredClaimNames.Exp, secToLive.ToString())
        };

        var creds = new SigningCredentials
        (
            new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings.Key)),
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken
        (
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            expires: DateTime.Now.AddSeconds(secToLive),
            claims: claims,
            signingCredentials: creds
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}