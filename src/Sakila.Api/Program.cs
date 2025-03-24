using Sakila.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddFakeRepositories();

var app = builder.Build();
app.MapApi();

app.Run();

