namespace Sakila.Api.Endpoints;

public static class OrderEndpoints
{
    public static RouteGroupBuilder MapOrdersApi(this RouteGroupBuilder group)
    {

        group.MapGet("/", () => "Hello Orders!");
        group.MapGet("/{id:int}", (int id) => $"Hello Order {id}!");

        return group;

    }
}
