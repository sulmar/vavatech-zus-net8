using Riok.Mapperly.Abstractions;
using Sakila.Api.Domain.Models;
using Sakila.Api.DTO;

namespace Sakila.Api.Mappers;

[Mapper]
public partial class OrderMapper
{
    public partial OrderDto Map(Order order);

}
