using System.Reflection;

namespace Sakila.Api.DTO;

public record AddProductRequest
{
    public string Name { get; set; }
    public decimal Price { get; set; }

    // Dodanie własnej logiki podczas deserializacji obiektu
    public static async ValueTask<AddProductRequest?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        var request = context.Request;

        if (request.ContentType !=null && request.ContentType.Contains("application/json"))
        {
            var product = await request.ReadFromJsonAsync<AddProductRequest>();

            if (product!=null)
            {
                if (string.IsNullOrEmpty(product.Name))
                    product.Name = "Domyślna nazwa";
            }

            return product;

        }

        return null;
    }

}