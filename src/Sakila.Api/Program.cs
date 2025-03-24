
using Microsoft.AspNetCore.Builder;
using Sakila.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


app.MapGet("/", () => "Hello, World!");

var homeHandler = () => "Hello, World!";

app.MapGet("/", homeHandler);

app.MapGroup("/customers").MapCustomersApi();
app.MapGroup("/orders").MapOrdersApi();

app.Run();

