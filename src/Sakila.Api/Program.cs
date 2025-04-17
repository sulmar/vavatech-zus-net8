using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Sakila.Api;
using Sakila.Api.Authorization;
using Sakila.Api.BackgroundServices;
using Sakila.Api.Domain.Abstractions;
using Sakila.Api.Domain.Models;
using Sakila.Api.DTO;
using Sakila.Api.Extensions;
using Sakila.Api.Filters;
using Sakila.Api.Hubs;
using Sakila.Api.Infrastructure;
using Sakila.Api.Middlewares;
using Sakila.Api.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSerilogLogging(); // U¿ycie metody rozszerzaj¹cej
builder.Configuration.AddCustomConfiguration(builder, args); // U¿ycie metody rozszerzaj¹cej
builder.Services.AddCustomServices(); // U¿ycie metody rozszerzaj¹cej
builder.Services.AddCustomConfigurations(builder.Configuration); // U¿ycie metody rozszerzaj¹cej

builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddResponseCompression();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:7127");
        policy.WithMethods("GET");
        policy.AllowAnyHeader();
    });
});


builder.Services.AddHostedService<DashboardBackgroundService>();
builder.Services.AddSignalR();

builder.Services.AddSingleton<IOcrService, ChannelOcrService>();

// dotnet add package HotChocolate.AspNetCore
builder.Services.AddGraphQLServer().AddQueryType<Query>();


builder.Services.AddDbContext<SakilaContext>(options => options.UseInMemoryDatabase("sakilaDb"));

builder.Services.AddHealthChecks()
    .AddCheck("Ping", () => HealthCheckResult.Healthy())
    .AddCheck("Random", () => DateTime.Now.Minute % 2 == 0 ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy(description: "Nieparzyste minuty"))
    .AddCheck("Upload", () =>
    {
        var filepath = System.IO.Path.Combine("Uploads", "check.txt");
        try
        {
            File.Create(filepath).Close();

            return HealthCheckResult.Healthy();

        }
        catch (IOException e)
        {
            return HealthCheckResult.Degraded();
        }

        finally
        {
            File.Delete(filepath);
        }
    })  // AspNetCore.HealthChecks.SignalR
    .AddSignalRHub("https://localhost:7285/signalr/documents", "SignalR Hub Documents")
    .AddSignalRHub("https://localhost:7285/signalr/dashboard", "SignalR Hub Dashboard");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_long_256_bits_secret_key_your")),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = "your_issuer",
        ValidAudience = "your_audience",
        ValidateLifetime = true,        // Weryfikacja czasu ¿ycia tokenu
        ClockSkew = TimeSpan.Zero       // Domyœlnie tolerancja 5 minut
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token = context.Request.Cookies["access_token"];

            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        },
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdultPolicy", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("DateOfBirth");
        policy.Requirements.Add(new AgeRequirment(18));
    });
});

builder.Services.AddSingleton<IAuthorizationHandler, AgeRequirmentHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, DocumentOwnerRequrimentHandler>();

var app = builder.Build();

var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<SakilaContext>();
var products = app.Services.CreateScope().ServiceProvider.GetRequiredService<IEnumerable<Product>>();

await DatabaseSeeder.SeedAsync(context, products);

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseStopwatch();
app.UseLogger();
//app.UseAuthorize();
app.UseResponseCompression();    // W³¹czenie kompresji odpowiedzi

// Under Construction
//app.Run(async (context) =>
//{
//    await context.Response.WriteAsync("Under construction");

//    context.Response.StatusCode = 500;

//});   

app.UseDefaultFiles(); // Obs³uga domyœlnych stron default.htm, default.html, index.htm, index.html
app.UseStaticFiles(); // Obs³uga ¿¹dañ statycznych plików (np. stron, zdjêæ, skryptów, styli)


app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/ping", () => "pong").RequireAuthorization(options=>options.RequireRole("Admin"));

// dotnet add package AspNetCore.HealthChecks.UI.Client
app.MapHealthChecks("/hc", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapApi();

app.MapGet("/filter", async () =>
{
    await Task.Delay(Random.Shared.Next(500, 2000));

    return Results.Ok("Hello, World!");
}).AddEndpointFilter<StopwatchFilter>();

app.MapGet("/test", (ICurrencyService service1, IServiceProvider serviceProvider) =>
{
    var service2 = serviceProvider.GetRequiredService<ICurrencyService>();
    var service3 = serviceProvider.GetRequiredService<ICurrencyService>();

    return Results.Ok();

});

app.MapGet("/html", ([FromQuery] string name) =>
{
    string html = $"<h1>{name}</h1>";

    return Results.Content(html, "text/html");

}).RequireAuthorization("AdultPolicy");


// Atrybut [FromForm] pozwala na deserializacjê obiektu zakodowanego za pomoc¹ x-www-form-urlencoded
app.MapPost("/login", ([FromForm] LoginRequest request) =>
{
    var username = request.Username;
    var password = request.Password;

}).DisableAntiforgery();


app.MapPost("/login2", (HttpContext context) =>
{
    var username = context.Request.Form["username"].ToString();
    var password = context.Request.Form["password"].ToString();

});

app.MapPost("/upload", async (IFormFile file) =>
{
    if (file.Length > 0)
    {
        var filePath = System.IO.Path.Combine("Uploads", file.FileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Results.Ok(new { Filepath = filePath });
    }

    return Results.BadRequest("Invalid file");

}).DisableAntiforgery();


app.MapPost("/documents", async (HttpContext context, IOcrService service) =>
{
    var files = context.Request.Form.Files;

    if (files == null || files.Count() == 0)
        return Results.BadRequest("Brak przes³anych plików");

    foreach (var file in files)
    {
        await service.AddAsync(file);
    }

    return Results.Accepted();
}).DisableAntiforgery();

app.MapGet("/documents/{id:int}", async (int id, 
    IDocumentRepository repository, 
    IAuthorizationService authorizationService, 
    HttpContext context) =>
{
    var document = repository.Get(id);

    var result = await authorizationService.AuthorizeAsync(context.User, document, new DocumentOwnerRequriment());

    if (!result.Succeeded)
        return Results.Forbid();

    return Results.Ok(document);
}).RequireAuthorization();


app.MapHub<DashboardHub>("/signalr/dashboard");
app.MapHub<DocumentHub>("/signalr/documents");

app.Map("/sse", async context =>
{
    context.Response.ContentType = "text/event-stream";
    context.Response.Headers["Connection"] = "keep-alive";
    context.Response.Headers["Cache-Control"] = "no-cache";

    for (int i = 0; i < 10; i++)
    {
        await context.Response.WriteAsync($"data: zdarzenie {i}\n\n");
        await context.Response.Body.FlushAsync();

        await Task.Delay(5000);
    }
});


app.MapGraphQL();
app.MapNitroApp();

// dotnet add package GraphQL.Server.Ui.Playground
app.UseGraphQLPlayground("/graphql/playground", new GraphQL.Server.Ui.Playground.PlaygroundOptions
{
    GraphQLEndPoint = "/graphql"
});

//var level = app.Configuration["Logging:LogLevel:Microsoft.AspNetCore"];
//var url1 = app.Configuration.GetValue<string>("NbpApiService:Url");
var googleMapsKey = app.Configuration["GoogleMaps"];
Console.WriteLine(googleMapsKey);

var connectionString = app.Configuration.GetConnectionString("DefaultConnection");

app.Run();

