using Sakila.Api.Domain.Abstractions;
using Sakila.Api.Domain.Models;
using Sakila.Api.DTO;
using Sakila.Api.Services;

namespace Sakila.Api.Endpoints;

public static class ProductEndpoints
{
    public static RouteGroupBuilder MapProductsApi(this RouteGroupBuilder group)
    {
        group.MapGet("/{id:int}", (int id, IProductRepository repository, ICurrencyService currencyService) => // Zastosowanie Match Pattern
    repository.Get(id) switch
    {
        Product product => Results.Ok(product),
        _ => Results.NotFound()
    }
);

        group.MapPost("/", (AddProductRequest request) => Results.Ok(request));

        return group;
    }

}
