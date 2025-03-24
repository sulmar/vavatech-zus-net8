using Sakila.Api.Extensions;
using Sakila.Api.Services;

var builder = WebApplication.CreateBuilder(args);

string environmentName = builder.Environment.EnvironmentName;

builder.Configuration.AddJsonFile("appsettings.json");
builder.Configuration.AddXmlFile("appsettings.xml");
builder.Configuration.AddCommandLine(args); // --NbpApiService:Table=B
builder.Configuration.AddInMemoryCollection();

builder.Configuration.AddJsonFile($"appsettings.{environmentName}.json", optional: true);



builder.AddFakeRepositories();

builder.Services.Configure<NbpApiCurrencyServiceOptions>(builder.Configuration.GetSection("NbpApiService"));

var app = builder.Build();
app.MapApi();   

//var level = app.Configuration["Logging:LogLevel:Microsoft.AspNetCore"];
//var url1 = app.Configuration.GetValue<string>("NbpApiService:Url");
var googleMapsKey = app.Configuration["GoogleMaps:AccessKey"];
Console.WriteLine(googleMapsKey);

app.Run();

