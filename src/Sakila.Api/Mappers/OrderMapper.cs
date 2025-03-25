using Sakila.Api.Domain.Models;
using Sakila.Api.DTO;

namespace Sakila.Api.Mappers;

public class OrderMapper
{
    public OrderDto Map(Order order)
    {
        return new OrderDto 
        {  
            Id = order.Id, 
            OrderDate = order.OrderDate, 
            TotalAmount = order.TotalAmount,
            CustomerName = order.Customer.Name
        };
    }
}
