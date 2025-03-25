using Sakila.Api.Domain.Models;

namespace Sakila.Api.Domain.Abstractions;

public interface IOrderRepository
{
    Order Get(int id);
}