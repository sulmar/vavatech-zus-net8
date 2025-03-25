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
            product.Price = product.Price * currencyService.GetCurrencyRatio("EUR");
        }

        return product;

    }
}
