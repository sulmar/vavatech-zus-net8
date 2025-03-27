using HealthChecks.UI.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Tracking.Api.Models;
using Tracking.Api.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<MyDriverService>();

builder.Services.AddGrpc();


builder.Services.AddGrpcHealthChecks()
    .AddCheck("grpc", () => HealthCheckResult.Healthy());

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/send", async (SendMessageRequest request, MyDriverService service) =>
{
    await service.SendMessageToDriver(request.driverId, request.title, request.content);

    return Results.Ok("Wiadomoœæ zosta³a wys³ana");
});


// app.MapGrpcService<MyTrackingService>();
app.MapGrpcService<MyDriverService>();
app.MapGrpcHealthChecksService();

// dotnet add package AspNetCore.HealthChecks.UI.Client
app.MapHealthChecks("/hc", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
