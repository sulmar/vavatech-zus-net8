using Sakila.Api.Domain.Abstractions;
using Sakila.Api.Domain.Models;
using Sakila.Api.DTO;
using Sakila.Api.Services;

namespace Sakila.Api.Endpoints;

public static class ProductEndpoints
{
    public static RouteGroupBuilder MapProductsApi(this RouteGroupBuilder group)
    {
        group.MapGet("/{id:int}", (int id, IProductRepository repository, ICurrencyService currencyService) =>
        {
            Product product = repository.Get(id);

            product.Price = product.Price * currencyService.GetCurrencyRatio("EUR");

            if (product == null)
                return Results.NotFound();

            return Results.Ok(product);

        });



        group.MapPost("/", (AddProductRequest request) => Results.Ok(request));

        return group;
    }

}
