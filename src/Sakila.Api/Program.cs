using Sakila.Api.Extensions;
using Sakila.Api.Services;

var builder = WebApplication.CreateBuilder(args);

string environmentName = builder.Environment.EnvironmentName;

builder.Configuration.AddJsonFile("appsettings.json");
builder.Configuration.AddJsonFile($"appsettings.{environmentName}.json", optional: true);
builder.Configuration.AddXmlFile("appsettings.xml");
builder.Configuration.AddUserSecrets<Program>();
builder.Configuration.AddCommandLine(args); // --NbpApiService:Table=B
builder.Configuration.AddInMemoryCollection();

builder.AddFakeRepositories();

builder.Services.Configure<NbpApiCurrencyServiceOptions>(builder.Configuration.GetSection("NbpApiService"));

var app = builder.Build();
app.MapApi();   

//var level = app.Configuration["Logging:LogLevel:Microsoft.AspNetCore"];
//var url1 = app.Configuration.GetValue<string>("NbpApiService:Url");
var googleMapsKey = app.Configuration["GoogleMaps"];
Console.WriteLine(googleMapsKey);

var connectionString = app.Configuration.GetConnectionString("DefaultConnection");

app.Run();

