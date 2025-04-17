using Auth.Api.Abstractions;
using Auth.Api.Models;

namespace Auth.Api.Infrastructure;

public class AuthService : IAuthService
{
    private readonly IUserIdentityRepository repository;

    public AuthService(IUserIdentityRepository repository)
    {
        this.repository = repository;
    }

    public async Task<AuthorizeResult> AuthorizeAsync(string username, string password)
    {
        var userIdentity = await repository.GetUserIdentityAsync(username);

        if (IsValid(username, password, userIdentity))
        {
            return new AuthorizeResult(true, userIdentity);

        }

        return new AuthorizeResult(false, null);

    }

    private static bool IsValid(string username, string password, UserIdentity userIdentity)
    {
        return username == userIdentity.Username && password == userIdentity.HashedPassword;
    }
}
    