using Sakila.Api.Domain.Abstractions;
using Sakila.Api.Domain.Models;
using Sakila.Api.Infrastructure;
using Sakila.Api.Services;

namespace Sakila.Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddFakeRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IProductRepository, FakeProductRepository>();
        builder.Services.Decorate<IProductRepository, CurrencyProductRepository>(); // dotnet add package Scrutor

        builder.Services.AddTransient<Category>(sp =>
        {
            var category1 = new Category
            {
                Id = 1,
                Name = "Category 1"

            };

            return category1;
        });

        builder.Services.AddTransient<IEnumerable<Product>>(sp =>
        {
            var category = sp.GetService<Category>();

            var products = new List<Product>()
            {
                            new Product { Id = 1, Name = $"Product 1", Description = "Lorem ipsum", Price = 100m, Category = category },
                            new Product { Id = 2, Name = $"Product 2", Description = "Lorem ipsum", Price = 100m, Category = category },
                            new Product { Id = 3, Name = $"Product 3", Description = "Lorem ipsum", Price = 100m, Category = category },
            };

            category.Products = products;

            return products;
        });




        builder.Services.AddScoped<ICurrencyService, ForexApiCurrencyService>();
        builder.Services.AddScoped<ICurrencyService, NbpApiCurrencyService>();


        return builder;
    }



}
