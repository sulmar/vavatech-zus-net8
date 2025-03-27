using Sakila.Api.Domain.Models;

namespace Sakila.Api.Infrastructure;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(SakilaContext context, IEnumerable<Product> products)
    {
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        await context.Products.AddRangeAsync(products);

        await context.SaveChangesAsync();
    }
}
