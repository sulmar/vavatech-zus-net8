namespace Sakila.Api.DTO;

public record SearchCustomerRequest
{
    public string City { get; set; }
    public string Country { get; set; }
}