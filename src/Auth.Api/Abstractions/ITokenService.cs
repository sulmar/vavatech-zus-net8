using Auth.Api.Models;

namespace Auth.Api.Abstractions;

public interface ITokenService
{
    string CreateAccessToken(UserIdentity identity);
}
