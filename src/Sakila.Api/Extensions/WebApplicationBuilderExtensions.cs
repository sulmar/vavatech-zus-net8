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

        


        builder.Services.AddTransient<IEnumerable<Order>>(sp =>
        {
            var customer = new Customer { Id = 1, Name = "Vavatech", HashedPassword = "123" };

            var orders = new List<Order>()
            {
                new RetailOrder { Id = 1, CustomerEmail = "biuro@vavatech.pl", OrderDate = DateTime.Parse("2025-03-25"), TotalAmount = 100, Customer = customer },
                new RetailOrder { Id = 2, CustomerEmail = "biuro@vavatech.pl", OrderDate = DateTime.Parse("2025-03-25"), TotalAmount = 200, Customer = customer },
                new SubscriptionOrder { Id = 3, OrderDate = DateTime.Parse("2025-03-25"), TotalAmount = 200, PeriodMonths = 12, Customer = customer },
            };

            return orders;

        });


        builder.Services.AddScoped<ICurrencyService, ForexApiCurrencyService>();
        builder.Services.AddScoped<ICurrencyService, NbpApiCurrencyService>();


        builder.Services.AddScoped<IOrderRepository, FakeOrderRepository>();   


        return builder;
    }



}
