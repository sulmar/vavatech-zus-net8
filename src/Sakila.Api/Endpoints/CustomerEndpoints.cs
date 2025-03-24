using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Sakila.Api.DTO;

namespace Sakila.Api.Endpoints;

public static class CustomerEndpoints
{
    public static RouteGroupBuilder MapCustomersApi(this RouteGroupBuilder group)
    {
        // group.MapGet("/", () => "Hello Customers!");        

        // Parametry Query Route 
        group.MapGet("/{id:int}", (int id) => $"Hello Customer {id}!")
            .WithName("GetCustomerById");


        group.MapGet("/{slug:regex(^[a-z]+$)}", (string slug) => $"Hello {slug}");

        // Dostosowanie nazw parametrów
        // GET /customers/map?lat=51.01&lng=21.01
        group.MapGet("/map", (
            [FromQuery(Name = "lat")] float latitude,
            [FromQuery(Name = "lng")] float longitude) => $"Hello Customer near {latitude} {longitude}");

        // Zamapowanie złożonych parametrów na klasę
        // GET /customers?country=Poland&city=Warsaw
        group.MapGet("/", ([AsParameters] SearchCustomerRequest request) => $"Hello Customers from {request.Country} {request.City}");


        group.MapPost("/", (AddCustomerRequest request) =>
        {
            int id = Random.Shared.Next();

            // TODO: add to db 

            return Results.CreatedAtRoute("GetCustomerById", new { id }, request);
        });

        // PUT /customers/{id}
        group.MapPut("/{id:int}", (int id, UpdateCustomerRequest request) =>
        {
            if (id != request.CustomerId)
                return Results.BadRequest();

            // TODO: update in db

            return Results.Ok(request);

        });


        // json-patch
        // Content-Type: application/json-patch+json
        // { "op": "replace", "path": "/HomeAddress/City", "value": "Kraków" }
        // dotnet add package Microsoft.AspNetCore.JsonPatch

        // json-merge-patch
        // Content-Type: application/merge-patch+json

        // PATCH /customers/{id}
        group.MapPatch("/{id:int}", (int id, [FromBody] JsonPatchDocument<UpdateCustomerRequest> request) =>
        {
            var customer = new UpdateCustomerRequest
            {
                CustomerId = 1,
                CustomerName = "Lorem",
                HomeAddress = new Address
                {
                    City = "Warsaw"
                }
            };

            request.ApplyTo(customer);

            // TODO: update in db

            return Results.Ok(request);

        });


        group.MapDelete("/{id:int}", (int id) => $"Customer {id} Deleted.");

        // Czy zasób istnieje
        group.MapMethods("/", new[] { "HEAD" }, () => "HEAD");



        //                              | REST API
        // GET /GetAllCustomers        | GET    /customers
        // GET /GetOrdersByCustomer    | GET    /customers/{id}/orders
        // GET /GetCustomerById        | GET    /customers/{id}
        // POST /AddCustomer           | POST   /customers
        // PUT /UpdateCustomer/1       | PUT    /customers/{id}
        // DELETE /DeleteCustomer/1    | DELETE /customers/{id}
        //                             | POST   /customers/{id}/reports
        //                             | GET    /customers/{id}/reports/{symbol}

        return group;
    }
}
