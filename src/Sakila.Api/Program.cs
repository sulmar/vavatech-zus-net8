using Sakila.Api.Extensions;
using Sakila.Api.Services;

var builder = WebApplication.CreateBuilder(args);
builder.AddFakeRepositories();

builder.Services.Configure<NbpApiCurrencyServiceOptions>(builder.Configuration.GetSection("NbpApiService"));

var app = builder.Build();
app.MapApi();   

//var level = app.Configuration["Logging:LogLevel:Microsoft.AspNetCore"];
//var url1 = app.Configuration.GetValue<string>("NbpApiService:Url");
//var url2 = app.Configuration["NbpApiService:Url"];
// Console.WriteLine(url1);

app.Run();

