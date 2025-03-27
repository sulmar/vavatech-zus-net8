var builder = WebApplication.CreateBuilder(args);

// dotnet add package AspNetCore.HealthCheck.UI
// dotnet add package AspNetCore.HealthCheck.UI.Client

builder.Services.AddHealthChecksUI(options =>
{
    options.AddHealthCheckEndpoint("Sakila.Api", "https://localhost:7285/hc");
    options.AddHealthCheckEndpoint("Tracking.Api", "https://localhost:7266/hc");
}).AddSqliteStorage("Data Source=HealthChecks.db");


var app = builder.Build();

app.MapHealthChecksUI(options => options.UIPath = "/");

app.Run();
