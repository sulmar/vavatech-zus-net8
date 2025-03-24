namespace Sakila.Api.DTO;

public class UpdateCustomerRequest
{
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string Email { get; set; }
    public Address HomeAddress { get; set; }

}

public class Address
{
    public string City { get; set; }
    public string Region { get; set; }
    public string PostalCode { get; set; }
}
