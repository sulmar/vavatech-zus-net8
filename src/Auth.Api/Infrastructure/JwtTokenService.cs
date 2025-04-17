using Auth.Api.Abstractions;
using Auth.Api.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.Api.Infrastructure;

// dotnet add package System.IdentityModel.Tokens.Jwt
public class JwtTokenService : ITokenService
{
    public string CreateAccessToken(UserIdentity identity)
    {
        var claimsIdentity = new ClaimsIdentity
         (new[]
        {
            new Claim(ClaimTypes.Name, identity.Username),
            new Claim(ClaimTypes.Email, identity.Email),
            new Claim(ClaimTypes.GivenName, identity.FirstName),
            new Claim(ClaimTypes.Surname, identity.LastName)
        });

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_long_256_bits_secret_key_your"));
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var securityToken = new JwtSecurityToken(
            issuer: "your_issuer",
            audience: "your_audience",
            claims: claimsIdentity.Claims,
            expires: DateTime.UtcNow.AddMinutes(1),
            signingCredentials: credentials
        );

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.WriteToken(securityToken);

        return token;
    }
}
