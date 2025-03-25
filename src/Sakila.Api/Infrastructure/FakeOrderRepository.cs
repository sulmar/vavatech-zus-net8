using Sakila.Api.Domain.Abstractions;
using Sakila.Api.Domain.Models;
namespace Sakila.Api.Infrastructure;

public class FakeOrderRepository : IOrderRepository
{
    private readonly IDictionary<int, Order> _orders;

    public FakeOrderRepository(IEnumerable<Order> orders)
    {
        _orders = orders.ToDictionary(p=>p.Id);
    }

    public Order Get(int id)
    {
        if (_orders.TryGetValue(id, out var order))
            return order;

        else
            return null;
    }
}
