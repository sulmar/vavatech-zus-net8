using Microsoft.Extensions.DependencyInjection;
using Poly.Net.Tcp;
using Sakila.ConsoleApp.Models;
using System.Net.Http.Json;

Console.WriteLine("Hello, World!");

// await HttpClientTest();

// dotnet add package Microsoft.Extensions.DependencyInjection

var services = new ServiceCollection();

// dotnet add package Microsoft.Extensions.Http
services.AddHttpClient();

var provider = services.BuildServiceProvider();

var client = provider.GetRequiredService<HttpClient>();

var productId = 1;

client.BaseAddress = new Uri("https://localhost:7285");

var response1 = await client.GetAsync($"/products/{productId}");

if (response1.IsSuccessStatusCode)
{
    var product = await response1.Content.ReadFromJsonAsync<Product>();

    Console.WriteLine(product.Name);
}
else
{

}




Console.ReadLine();

static async Task HttpClientTest()
{
    HttpClient client = new HttpClient();
    client.BaseAddress = new Uri("https://localhost:7285");

    var productId = 1;

    /// var product = await client.GetFromJsonAsync<Product>($"/product/{productId}");
    /// 


    var response1 = await client.GetAsync($"/products/{productId}");

    if (response1.IsSuccessStatusCode)
    {
        var product = await response1.Content.ReadFromJsonAsync<Product>();

        Console.WriteLine(product.Name);
    }
    else
    {

    }


    var request = new AddProductRequest { Name = "Product 1", Price = 10.99m };

    var response = await client.PostAsJsonAsync("/products", request);

    if (response.IsSuccessStatusCode)
    {
        Console.WriteLine("Success!");
    }
    else
    {
        Console.WriteLine($"{response.StatusCode} {response.ReasonPhrase}");

        var message = await response.Content.ReadAsStringAsync();

        Console.WriteLine(message);

    }
}