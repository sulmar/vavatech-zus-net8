using System.Text.Json.Serialization;

namespace Sakila.Api.Domain.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string Color { get; set; }
    public Category Category { get; set; }

    // Serializacja nie będzie zawierać pola IsSelected
    [JsonIgnore] 
    public bool IsSelected { get; set; }

}

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    [JsonIgnore]    // Serializacja nie będzie zawierać listy produktów
    public List<Product> Products { get; set; } = new();
}
