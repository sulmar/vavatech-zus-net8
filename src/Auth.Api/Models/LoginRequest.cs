namespace Auth.Api.Models;

public record LoginRequest(string Username, string Password);

public record RefreshTokenRequest(string Username, string RefreshToken);
