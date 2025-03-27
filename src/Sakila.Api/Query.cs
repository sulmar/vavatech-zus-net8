using Sakila.Api.Domain.Models;

namespace Sakila.Api;

public class Query
{
    public string Hello()
    {
        return "Hello World!";
    }


    public Product GetProduct(int id)
    {
        return new Product
        {
            Id = id,
            Name = $"Product {id}",
            Price = 100,
            Color = "red",
            Category = new Category { Id = 1, Name = "Category 1" }
        };
    }
}
