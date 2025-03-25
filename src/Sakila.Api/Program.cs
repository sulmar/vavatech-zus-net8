using Sakila.Api.Domain.Abstractions;
using Sakila.Api.Extensions;
using Sakila.Api.Mappers;
using Sakila.Api.Services;
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

builder.Services.Configure<NbpApiCurrencyServiceOptions>(builder.Configuration.GetSection("NbpApiService"));


builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    // options.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve; // Zawiera referencje zamiast zapêtlonych struktur
    // options.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles; // Zawiera wartoœæ null zamiast zapêtlonych struktur
});

var app = builder.Build();
app.MapApi();

app.MapGet("/test", (ICurrencyService service1, IServiceProvider serviceProvider) =>
{
    var service2 = serviceProvider.GetRequiredService<ICurrencyService>();
    var service3 = serviceProvider.GetRequiredService<ICurrencyService>();

    return Results.Ok();

});

//var level = app.Configuration["Logging:LogLevel:Microsoft.AspNetCore"];
//var url1 = app.Configuration.GetValue<string>("NbpApiService:Url");
var googleMapsKey = app.Configuration["GoogleMaps"];
Console.WriteLine(googleMapsKey);

var connectionString = app.Configuration.GetConnectionString("DefaultConnection");

app.Run();

