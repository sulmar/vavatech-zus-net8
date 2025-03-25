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

        group.MapPost("/", (Order order) =>
        {
            ICollection<ValidationResult> validationResults = new List<ValidationResult>();

            var context = new ValidationContext(order);
            bool isValid = Validator.TryValidateObject(order, context, validationResults, true);

            if (!isValid)
            {
                return Results.ValidationProblem(validationResults.ToDictionary(
                    result => result.MemberNames.First(),
                    result => new[] { result.ErrorMessage }));
            }


           return Results.Ok(order);
        });

        return group;

    }
}
