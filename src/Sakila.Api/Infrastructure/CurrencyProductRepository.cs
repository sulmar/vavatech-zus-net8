using Sakila.Api.Domain.Abstractions;
using Sakila.Api.Domain.Models;

namespace Sakila.Api.Infrastructure;

public class CurrencyProductRepository : IProductRepository
{
    private readonly IProductRepository _decoratedProductRepository;
    private readonly ICurrencyService currencyService;

    public CurrencyProductRepository(IProductRepository decoratedProductRepository, ICurrencyService currencyService)
    {
        _decoratedProductRepository = decoratedProductRepository;
        this.currencyService = currencyService;
    }

    public Product Get(int id)
    {
        var product = _decoratedProductRepository.Get(id);

        if (product != null)
        {
            RecalculatePrice(product);
        }

        return product;

    }

    private void RecalculatePrice(Product? product)
    {
        product.Price = product.Price * currencyService.GetCurrencyRatio("EUR");
    }

    public IEnumerable<Product> GetAll()
    {
        var products = _decoratedProductRepository.GetAll();

        foreach (var product in products)
        {
            RecalculatePrice(product);
        }

        return products;
    }
}
