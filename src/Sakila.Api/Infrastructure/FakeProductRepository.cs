using Sakila.Api.Domain.Abstractions;
using Sakila.Api.Domain.Models;
namespace Sakila.Api.Infrastructure;

public class FakeProductRepository : IProductRepository
{
    private readonly IDictionary<int, Product> _products;

    public FakeProductRepository(IEnumerable<Product> products)
    {
        _products = products.ToDictionary(p=>p.Id);
    }


    public Product Get(int id)
    {
        if (_products.TryGetValue(id, out var product))
            return product;
        else
            return null;
    }
}
