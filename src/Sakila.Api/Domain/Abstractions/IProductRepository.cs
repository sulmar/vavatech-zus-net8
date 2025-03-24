using Sakila.Api.Domain.Models;

namespace Sakila.Api.Domain.Abstractions;

public interface IProductRepository
{
    Product Get(int id);
}
