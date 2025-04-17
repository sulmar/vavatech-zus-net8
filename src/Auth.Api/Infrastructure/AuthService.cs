using Auth.Api.Abstractions;
using Auth.Api.Models;

namespace Auth.Api.Infrastructure;

public class AuthService : IAuthService
{
    public Task<AuthorizeResult> AuthorizeAsync(string username, string password)
    {
        // TODO: Przenieść do repozytorium
        var userIdentity = new UserIdentity 
        { 
            FirstName = "John", 
            LastName = "Smith", 
            Email = "john@domain.com", 
            Username = "John", 
            HashedPassword = "123" };

        if (username == userIdentity.Username && password == userIdentity.HashedPassword)
        {
            return Task.FromResult(new AuthorizeResult(true, userIdentity));
            
        }


        return Task.FromResult(new AuthorizeResult(false, null));

    }
}
    