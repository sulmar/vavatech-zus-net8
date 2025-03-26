using Microsoft.AspNetCore.Http.HttpResults;
using Sakila.Api.Domain.Abstractions;
using Sakila.Api.Domain.Models;
using Sakila.Api.DTO;
using Sakila.Api.Services;

namespace Sakila.Api.Endpoints;

public static class ProductEndpoints
{
    public static RouteGroupBuilder MapProductsApi(this RouteGroupBuilder group)
    {
        group.MapGet("/{id:int}", (int id, IProductRepository repository) =>
        {
            Product product = repository.Get(id);

            if (product == null)
                return Results.NotFound();

            return Results.Ok(product);

        });



        group.MapPost("/", (AddProductRequest request) =>
        {
            // throw new Exception("Test");

            return Results.Problem("Błędne dane");

            // return Results.BadRequest("Błędne dane");



            return Results.Ok(request);
        });

        return group;
    }

}
