using Sakila.Api.Endpoints;
using Sakila.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddFakeRepositories();

var app = builder.Build();

app.MapGroup("/").MapRootApi();
app.MapGroup("/customers").MapCustomersApi();
app.MapGroup("/orders").MapOrdersApi();
app.MapGroup("/products").MapProductsApi();

app.Run();

