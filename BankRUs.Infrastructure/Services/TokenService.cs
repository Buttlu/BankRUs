using BankRUs.Application.Services;
using BankRUs.Infrastructure.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BankRUs.Infrastructure.Services;

public class TokenService(IOptions<JwtOptions> options) : ITokenService
{
    private readonly JwtOptions _jwt = options.Value;

    public Token CreateToken(string userId, string email)   
    {
        // Användar Info
        var claims = new List<Claim> {
            new(JwtRegisteredClaimNames.Sub, userId),
            new(JwtRegisteredClaimNames.Email, email),
            new(JwtRegisteredClaimNames.PreferredUsername, email)
        };

        // Token info
        // Iss -> issuer (BankRUs.Api)
        // Aud -> audience (BankRUs client)
        // Exp -> expiration (unit timestamp)
        // Nbf -> när böjar token gälla (unix timestamp)

        var issuer = _jwt.Issuer;
        var audience = _jwt.Audience;
        var secret = _jwt.Secret;
        var expiresMinutes = _jwt.ExpiresInMinues;

        // hämta från appsettings
        var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        var creds = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256);

        // hämta från appsettings
        var expiresAtUtc = DateTime.Now.AddMinutes(expiresMinutes);

        var jwt = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAtUtc,
            signingCredentials: creds);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(jwt);

        return new Token(
            AccessToken: tokenString,
            ExpiresAtUtc: DateTime.UtcNow.AddHours(1)
        );
    }
}
