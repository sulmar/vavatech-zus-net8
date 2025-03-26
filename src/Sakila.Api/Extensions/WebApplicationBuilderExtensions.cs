using FluentValidation;
using Sakila.Api.Domain.Abstractions;
using Sakila.Api.Domain.Models;
using Sakila.Api.Infrastructure;
using Sakila.Api.Mappers;
using Sakila.Api.Services;
using Sakila.Api.Validators;

namespace Sakila.Api.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddFakeRepositories(); // Jeśli masz metodę rozszerzającą dla repozytoriów

        services.AddTransient<OrderMapper>();

        services.AddScoped<IValidator<Order>, OrderValidator>();
        services.AddScoped<IValidator<Customer>, CustomerValidator>();


        return services;
    }
    

    public static IServiceCollection AddFakeRepositories(this IServiceCollection services)
    {
        services.AddTransient<IProductRepository, FakeProductRepository>();
        services.Decorate<IProductRepository, CurrencyProductRepository>(); // dotnet add package Scrutor

        services.AddTransient<Category>(sp =>
        {
            var category1 = new Category
            {
                Id = 1,
                Name = "Category 1"

            };

            return category1;
        });

        services.AddTransient<IEnumerable<Product>>(sp =>
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

        
        services.AddTransient<IEnumerable<Order>>(sp =>
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


        services.AddScoped<ICurrencyService, ForexApiCurrencyService>();
        services.AddScoped<ICurrencyService, NbpApiCurrencyService>();


        services.AddScoped<IOrderRepository, FakeOrderRepository>();   


        return services;
    }
}
