
using Microsoft.AspNetCore.Builder;
using Sakila.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();


app.MapGet("/", () => "Hello, World!");

var homeHandler = () => "Hello, World!";

app.MapGet("/", homeHandler);

app.MapGroup("/customers").MapCustomersApi();
app.MapGroup("/orders").MapOrdersApi();


app.Map("/test", (HttpContext context) =>
{
    HttpRequest request = context.Request;   
    
    var requestMethod = context.Request.Method;
    var requestPath = context.Request.Path;
    var id = context.Request.Query["id"];
    var name = context.Request.Query["name"];

    HttpResponse response = context.Response;
});

app.MapGet("/hello", (HttpRequest req, HttpResponse res) => Results.Ok("Hello World!"));

app.Run();

