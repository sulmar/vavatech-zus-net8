using Sakila.Api.Endpoints;
using Sakila.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddFakeRepositories();

var app = builder.Build();

app.MapGet("/", () => "Hello, World!");
var homeHandler = () => "Hello, World!";

app.MapGet("/", homeHandler);

app.MapGroup("/customers").MapCustomersApi();
app.MapGroup("/orders").MapOrdersApi();
app.MapGroup("/products").MapProductsApi();
app.MapGroup("/").MapContextApi();

app.Run();

