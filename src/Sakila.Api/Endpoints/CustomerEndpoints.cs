using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Sakila.Api.Endpoints;

public static class CustomerEndpoints
{
    public static RouteGroupBuilder MapCustomersApi(this RouteGroupBuilder group)
    {
        group.MapGet("/", () => "Hello Customers!");
        group.MapGet("/{id:int}", (int id) => $"Hello Customer {id}!");

        return group;
    }
}
