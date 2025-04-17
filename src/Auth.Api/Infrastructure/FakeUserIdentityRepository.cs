using Auth.Api.Abstractions;
using Auth.Api.Models;

namespace Auth.Api.Infrastructure;

public class FakeUserIdentityRepository : IUserIdentityRepository
{
    public Task<UserIdentity> GetUserIdentityAsync(string username)
    {
        // TODO: Przenieść do repozytorium
        var userIdentity = new UserIdentity
        {
            FirstName = "John",
            LastName = "Smith",
            Email = "john@domain.com",
            Username = "John",
            HashedPassword = "123"
        };

        return Task.FromResult(userIdentity);
    }
}
