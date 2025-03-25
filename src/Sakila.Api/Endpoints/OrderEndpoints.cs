using Sakila.Api.Domain.Abstractions;
using Sakila.Api.Domain.Models;
using Sakila.Api.Mappers;

namespace Sakila.Api.Endpoints;

public static class OrderEndpoints
{
    public static RouteGroupBuilder MapOrdersApi(this RouteGroupBuilder group)
    {

        group.MapGet("/", () => "Hello Orders!");
        group.MapGet("/{id:int}", (int id, IOrderRepository repository, OrderMapper mapper) => mapper.Map(repository.Get(id)));

        group.MapPost("/", (Order order) => Results.Ok(order));

        return group;

    }
}
