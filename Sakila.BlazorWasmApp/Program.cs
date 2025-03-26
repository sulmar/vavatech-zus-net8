using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Sakila.BlazorWasmApp;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


var sakilaApiUrl = builder.Configuration["SakilaApiUrl"];

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Rejestracja nazwanego klienta
builder.Services.AddHttpClient("sakilaClient", client =>
{
    client.BaseAddress = new Uri(sakilaApiUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
       
await builder.Build().RunAsync();
