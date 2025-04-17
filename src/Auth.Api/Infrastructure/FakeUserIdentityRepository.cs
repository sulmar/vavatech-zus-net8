using Auth.Api.Abstractions;
using Auth.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace Auth.Api.Infrastructure;

public class FakeUserIdentityRepository : IUserIdentityRepository
{
    private readonly IPasswordHasher<UserIdentity> passwordHasher;

    private IDictionary<string, UserIdentity> _users = new Dictionary<string, UserIdentity>();

    public FakeUserIdentityRepository(IPasswordHasher<UserIdentity> passwordHasher)
    {
        this.passwordHasher = passwordHasher;

        var userIdentity = new UserIdentity
        {
            FirstName = "John",
            LastName = "Smith",
            Email = "john@domain.com",
            Username = "John",
            Roles = ["Admin", "User"]
        };

        userIdentity.HashedPassword = passwordHasher.HashPassword(userIdentity, "123");

        _users.Add(userIdentity.Username, userIdentity);
    }

    public Task<UserIdentity> GetUserIdentityAsync(string username)
    {
        if (_users.TryGetValue(username, out var userIdentity))
        {
            return Task.FromResult(userIdentity);
        }

        return Task.FromResult<UserIdentity>(null);
    }
}
