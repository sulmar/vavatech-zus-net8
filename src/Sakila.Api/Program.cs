using Microsoft.AspNetCore.Mvc;
using Sakila.Api.BackgroundServices;
using Sakila.Api.Domain.Abstractions;
using Sakila.Api.DTO;
using Sakila.Api.Extensions;
using Sakila.Api.Filters;
using Sakila.Api.Hubs;
using Sakila.Api.Middlewares;
using Sakila.Api.Services;

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

var app = builder.Build();

app.UseCors();

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

});


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
        var filePath = Path.Combine("Uploads", file.FileName);

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

app.MapGet("/documents/{id:int}", (int id) =>
{
    return Results.Ok();
});


app.MapHub<DashboardHub>("/signalr/dashboard");
app.MapHub<DocumentHub>("/signalr/documents");


//var level = app.Configuration["Logging:LogLevel:Microsoft.AspNetCore"];
//var url1 = app.Configuration.GetValue<string>("NbpApiService:Url");
var googleMapsKey = app.Configuration["GoogleMaps"];
Console.WriteLine(googleMapsKey);

var connectionString = app.Configuration.GetConnectionString("DefaultConnection");

app.Run();

