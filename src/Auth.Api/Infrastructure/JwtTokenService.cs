using Auth.Api.Abstractions;
using Auth.Api.Models;

namespace Auth.Api.Infrastructure;

public class JwtTokenService : ITokenService
{
    public string CreateAccessToken(UserIdentity identity)
    {
        // TODO: Dodać generowanie tokenu JWT
        return "abc";
    }
}
    