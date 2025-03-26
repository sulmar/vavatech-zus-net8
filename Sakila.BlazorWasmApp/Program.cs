using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Sakila.BlazorWasmApp;
using Sakila.BlazorWasmApp.MessageHandlers;
using Sakila.BlazorWasmApp.Services;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


var sakilaApiUrl = builder.Configuration["SakilaApiUrl"];

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddTransient<AuthHandler>();

// Rejestracja nazwanego klienta
builder.Services.AddHttpClient("sakilaClient", client =>
{
    client.BaseAddress = new Uri(sakilaApiUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Rejestracja silnie typowanego klienta (Strong Typed)
builder.Services.AddHttpClient<SakilaApiClient>(client =>
{
    client.BaseAddress = new Uri(sakilaApiUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
}).AddHttpMessageHandler<AuthHandler>();
       
await builder.Build().RunAsync();
