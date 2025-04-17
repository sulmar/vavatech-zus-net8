using Auth.Api.Abstractions;
using Auth.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace Auth.Api.Infrastructure;

public class AuthService : IAuthService
{
    private readonly IUserIdentityRepository repository;
    private readonly IPasswordHasher<UserIdentity> passwordHasher;

    public AuthService(IUserIdentityRepository repository, IPasswordHasher<UserIdentity> passwordHasher)
    {
        this.repository = repository;
        this.passwordHasher = passwordHasher;
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

    private bool IsValid(string username, string password, UserIdentity userIdentity)
    {
        return username == userIdentity.Username && passwordHasher.VerifyHashedPassword(userIdentity, userIdentity.HashedPassword, password) == PasswordVerificationResult.Success;
    }
}
    