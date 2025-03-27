using Microsoft.EntityFrameworkCore;
using Sakila.Api.Domain.Models;

namespace Sakila.Api.Infrastructure;

public class SakilaContext : DbContext
{
    public SakilaContext(DbContextOptions options) : base(options)
    {
    }

    protected SakilaContext()
    {
    }


    public DbSet<Product> Products { get; set; }
}
