using Auth.Api.Abstractions;
using Auth.Api.Infrastructure;
using Auth.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddSingleton<IRefreshTokenRepository, InMemoryRefreshTokenRepository>();
builder.Services.AddScoped<IUserIdentityRepository, FakeUserIdentityRepository>();
builder.Services.AddSingleton<IPasswordHasher<UserIdentity>, PasswordHasher<UserIdentity>>();

builder.Services.AddSingleton<List<UserIdentity>>(sp =>
{
    return new List<UserIdentity>()
    {  
        new UserIdentity
        {
            FirstName = "John",
            LastName = "Smith",
            Email = "john@domain.com",
            Username = "John",
            DateOfBirth = new DateTime(1990, 12, 1),
            Roles = ["Admin", "User"]
        }, 
        new UserIdentity
        {
            FirstName = "Kate",
            LastName = "Smith",
            Email = "kate@domain.com",
            Username = "Kate",
            DateOfBirth = new DateTime(2010, 12, 1),
            Roles = ["User"]
        } };
});

var app = builder.Build();

app.MapGet("/", () => Results.Redirect("login.html"));

app.MapPost("/api/token/create", async ([FromForm] LoginRequest request, IAuthService authService, ITokenService tokenService, IRefreshTokenService refreshTokenService) =>
{
    var result = await authService.AuthorizeAsync(request.Username, request.Password);

    if (result.IsAuthorized)
    {
        var accessToken = tokenService.CreateAccessToken(result.Identity);
        var refreshToken = refreshTokenService.CreateAndStoreRefreshToken(request.Username);

        return Results.Ok(new
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        });
    }

    return Results.Unauthorized();
}).DisableAntiforgery();


app.MapPost("/api/token/refresh", async ([FromForm] RefreshTokenRequest request, IRefreshTokenService refreshTokenService, ITokenService tokenService,
    IUserIdentityRepository repository) =>
{
    if (!refreshTokenService.ValidateRefreshToken(request.Username, request.RefreshToken))
    {
        return Results.Unauthorized();
    }

    var identity = await repository.GetUserIdentityAsync(request.Username);
    var accessToken = tokenService.CreateAccessToken(identity);
    var refreshToken = refreshTokenService.CreateAndStoreRefreshToken(request.Username);

    return Results.Ok(new
    {
        AccessToken = accessToken,
        RefreshToken = refreshToken,
    });

}).DisableAntiforgery();

app.UseStaticFiles();


app.Run();
