namespace Sakila.Api.DTO;

public class OrderDto
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }

    public string CustomerName { get; set; }
}
