using Auth.Api.Models;

namespace Auth.Api.Abstractions;

public interface IUserIdentityRepository
{
    Task<UserIdentity> GetUserIdentityAsync(string username);
}


public interface IRefreshTokenRepository
{
    string? GetRefreshToken(string username);
    void Update(string username, string refreshToken);
}