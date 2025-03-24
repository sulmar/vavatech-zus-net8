
using Sakila.Api.Domain.Abstractions;
using Sakila.Api.Domain.Models;
using Sakila.Api.DTO;
using Sakila.Api.Endpoints;
using Sakila.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<IProductRepository, FakeProductRepository>();

builder.Services.AddTransient<IEnumerable<Product>>(sp => 
    [
        new Product { Id = 1, Name = $"Product 1", Description = "Lorem ipsum", Price = 100m },
        new Product { Id = 2, Name = $"Product 2", Description = "Lorem ipsum", Price = 100m },
        new Product { Id = 3, Name = $"Product 3", Description = "Lorem ipsum", Price = 100m },
    ]
);

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

app.MapGet("/products/{id:int}", (int id, IProductRepository repository) =>
{
    var product = repository.Get(id);

    if (product == null)
        return Results.NotFound();

    return Results.Ok(product);

});

app.MapPost("/products", (AddProductRequest request) => Results.Ok(request));

app.Run();

