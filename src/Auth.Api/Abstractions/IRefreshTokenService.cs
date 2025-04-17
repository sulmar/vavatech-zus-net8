namespace Auth.Api.Abstractions;

public interface IRefreshTokenService
{
    string CreateAndStoreRefreshToken(string username);
    bool ValidateRefreshToken(string username, string refreshToken);
}