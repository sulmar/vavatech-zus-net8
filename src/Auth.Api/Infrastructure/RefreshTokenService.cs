using Auth.Api.Abstractions;
using System.Security.Cryptography;

namespace Auth.Api.Infrastructure;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IRefreshTokenRepository refreshTokenRepository;

    public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository)
    {
        this.refreshTokenRepository = refreshTokenRepository;
    }

    private string CreateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(4));
    }

    public bool ValidateRefreshToken(string username, string refreshToken)
    {
        return refreshTokenRepository.GetRefreshToken(username) == refreshToken;
    }

    public string CreateAndStoreRefreshToken(string username)
    {
        var refreshToken = CreateRefreshToken();

        refreshTokenRepository.Update(username, refreshToken);

        return refreshToken;


    }
}
