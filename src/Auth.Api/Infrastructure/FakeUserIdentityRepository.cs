using Auth.Api.Abstractions;
using Auth.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace Auth.Api.Infrastructure;

public class FakeUserIdentityRepository : IUserIdentityRepository
{
    private IDictionary<string, UserIdentity> _users = new Dictionary<string, UserIdentity>();

    public FakeUserIdentityRepository(IPasswordHasher<UserIdentity> passwordHasher, List<UserIdentity> userIdentities)
    {
        _users = userIdentities.ToDictionary(x => x.Username, x => x);

        foreach (var userIdentity in _users.Values)
        {
            userIdentity.HashedPassword = passwordHasher.HashPassword(userIdentity, "123");
        }
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
