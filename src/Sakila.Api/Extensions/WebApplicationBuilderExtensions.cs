using Sakila.Api.Domain.Abstractions;
using Sakila.Api.Domain.Models;
using Sakila.Api.Infrastructure;

namespace Sakila.Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddFakeRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IProductRepository, FakeProductRepository>();

        builder.Services.AddTransient<IEnumerable<Product>>(sp =>
            [
                new Product { Id = 1, Name = $"Product 1", Description = "Lorem ipsum", Price = 100m },
        new Product { Id = 2, Name = $"Product 2", Description = "Lorem ipsum", Price = 100m },
        new Product { Id = 3, Name = $"Product 3", Description = "Lorem ipsum", Price = 100m },
        ]
        );

        return builder;
    }
}
