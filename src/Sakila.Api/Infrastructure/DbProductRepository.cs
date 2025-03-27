using Sakila.Api.Domain.Abstractions;
using Sakila.Api.Domain.Models;
namespace Sakila.Api.Infrastructure;

public class DbProductRepository : IProductRepository
{
    private readonly SakilaContext context;

    public DbProductRepository(SakilaContext context)
    {
        this.context = context;
    }

    public Product Get(int id)
    {
        return context.Products.Find(id);
    }

    public IEnumerable<Product> GetAll()
    {
        return context.Products.ToList();
    }
}
