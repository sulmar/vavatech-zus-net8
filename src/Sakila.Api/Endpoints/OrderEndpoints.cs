using FluentValidation;
using Sakila.Api.Domain.Abstractions;
using Sakila.Api.Domain.Models;
using Sakila.Api.Mappers;
using System.ComponentModel.DataAnnotations;

namespace Sakila.Api.Endpoints;

public static class OrderEndpoints
{
    public static RouteGroupBuilder MapOrdersApi(this RouteGroupBuilder group)
    {

        group.MapGet("/", () => "Hello Orders!");
        group.MapGet("/{id:int}", (int id, IOrderRepository repository, OrderMapper mapper) => mapper.Map(repository.Get(id)));

        group.MapPost("/", (Order order, IValidator<Order> validator) =>
        {
            var validationResult = validator.Validate(order);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                 .GroupBy(error => error.PropertyName)
                 .ToDictionary(
                    group => group.Key,
                    group => group.Select(error => error.ErrorMessage).ToArray());

                return Results.ValidationProblem(errors);
            }


            return Results.Ok(order);
        });

        return group;

    }
}
