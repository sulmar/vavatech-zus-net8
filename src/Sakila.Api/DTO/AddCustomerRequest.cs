namespace Sakila.Api.DTO;

public record AddCustomerRequest
{
    public string CustomerName { get; set; }
    public string Email { get; set; }
}
