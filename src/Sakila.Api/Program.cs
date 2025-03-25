using FluentValidation;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Sakila.Api.Domain.Abstractions;
using Sakila.Api.Domain.Models;
using Sakila.Api.DTO;
using Sakila.Api.Extensions;
using Sakila.Api.Mappers;
using Sakila.Api.Services;
using Sakila.Api.Validators;
using Serilog;
using Serilog.Formatting.Compact;

var builder = WebApplication.CreateBuilder(args);


builder.Logging.ClearProviders();

var logger = new LoggerConfiguration()
    .WriteTo.Console()              // Serilog.Sinks.Console
    .WriteTo.File("log.txt")   //Serilog.Sinks.File
    .WriteTo.File(new CompactJsonFormatter(), "log.json") // Serilog.Formatting.Compact
    .CreateLogger();

// dotnet add package Serilog.Extensions.Logging
builder.Logging.AddSerilog(logger);

string environmentName = builder.Environment.EnvironmentName;

builder.Configuration.AddJsonFile("appsettings.json");
builder.Configuration.AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true);
builder.Configuration.AddXmlFile("appsettings.xml");
builder.Configuration.AddUserSecrets<Program>();
builder.Configuration.AddCommandLine(args); // --NbpApiService:Table=B
builder.Configuration.AddInMemoryCollection();

builder.AddFakeRepositories();
builder.Services.AddTransient<OrderMapper>();

builder.Services.AddScoped<IValidator<Order>, OrderValidator>();
builder.Services.AddScoped<IValidator<Customer>, CustomerValidator>();

builder.Services.Configure<NbpApiCurrencyServiceOptions>(builder.Configuration.GetSection("NbpApiService"));


builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    // options.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve; // Zawiera referencje zamiast zapêtlonych struktur
    // options.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles; // Zawiera wartoœæ null zamiast zapêtlonych struktur
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 1 * 1024 * 1024; // 1 MB
});

var app = builder.Build();

app.UseDefaultFiles(); // Obs³uga domyœlnych stron default.htm, default.html, index.htm, index.html
app.UseStaticFiles(); // Obs³uga ¿¹dañ statycznych plików (np. stron, zdjêæ, skryptów, styli)


app.MapApi();

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

        using(var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Results.Ok(new { Filepath = filePath });
    }

    return Results.BadRequest("Invalid file");

}).DisableAntiforgery();


//var level = app.Configuration["Logging:LogLevel:Microsoft.AspNetCore"];
//var url1 = app.Configuration.GetValue<string>("NbpApiService:Url");
var googleMapsKey = app.Configuration["GoogleMaps"];
Console.WriteLine(googleMapsKey);

var connectionString = app.Configuration.GetConnectionString("DefaultConnection");

app.Run();

