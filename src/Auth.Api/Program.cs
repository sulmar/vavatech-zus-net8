using Auth.Api.Abstractions;
using Auth.Api.Infrastructure;
using Auth.Api.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();
builder.Services.AddScoped<IUserIdentityRepository, FakeUserIdentityRepository>();

var app = builder.Build();

app.MapGet("/", () => "Hello Auth.Api!");

app.MapPost("/api/token/create", async ([FromForm] LoginRequest request, IAuthService authService, ITokenService tokenService) =>
{    
    var result = await authService.AuthorizeAsync(request.Username, request.Password);   

    if (result.IsAuthorized)
    {
        var accessToken = tokenService.CreateAccessToken(result.Identity);
        var refreshToken = string.Empty; // TODO: Implement refresh token logic

        return Results.Ok(new
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        });
    }

    return Results.Unauthorized();
}).DisableAntiforgery();

app.Run();
