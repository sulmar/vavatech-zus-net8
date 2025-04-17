using Auth.Api.Models;

namespace Auth.Api.Abstractions;

public interface IAuthService
{
    Task<AuthorizeResult> AuthorizeAsync(string username, string password);
}

public record AuthorizeResult(bool IsAuthorized, UserIdentity Identity);


public interface ITokenService
{
    string CreateAccessToken(UserIdentity identity);
}

public interface IUserIdentityRepository
{
    Task<UserIdentity> GetUserIdentityAsync(string username);
}