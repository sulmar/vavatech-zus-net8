using Auth.Api.Abstractions;

namespace Auth.Api.Infrastructure;

public class InMemoryRefreshTokenRepository : IRefreshTokenRepository
{
    private readonly IDictionary<string, string> refreshTokens = new Dictionary<string, string>();

    public string? GetRefreshToken(string username)
    {
        return refreshTokens.TryGetValue(username, out var refreshToken) ? refreshToken : null;
    }

    public void Update(string username, string refreshToken)
    {
        refreshTokens[username] = refreshToken;
    }
}
