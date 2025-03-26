using Sakila.Models;
using System.Net.Http.Json;

namespace Sakila.BlazorWasmApp.Services;

public class SakilaApiClient(HttpClient client) // Primary Constructor
{
    public async Task<Product?> GetProduct(int id)
    {
        return await client.GetFromJsonAsync<Product>($"/products/{id}");
    }
}
