using Fleet;
using Tracking.Api.Models;
using Tracking.Api.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<MyDriverService>();

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/send", async (SendMessageRequest request, MyDriverService service) =>
{
    await service.SendMessageToDriver(request.driverId, request.title, request.content);

    return Results.Ok("Wiadomoœæ zosta³a wys³ana");
});


// app.MapGrpcService<MyTrackingService>();
app.MapGrpcService<MyDriverService>();

app.Run();
