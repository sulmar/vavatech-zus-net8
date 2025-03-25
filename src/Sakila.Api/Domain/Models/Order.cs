using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sakila.Api.Domain.Models;

// od .NET 8
[JsonDerivedType(typeof(RetailOrder), "Retail")]
[JsonDerivedType(typeof(SubscriptionOrder), "Subscription")]
public abstract class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }

    [Range(1, 100)]
    public decimal TotalAmount { get; set; }
    public Customer Customer { get; set; }
}


public class Customer
{
    public int Id { get; set; }

    [Required, MinLength(3), MaxLength(10)]
    public string Name { get; set; }

    [Compare(nameof(ConfirmPassword))]
    public string HashedPassword { get; set; }
    public string ConfirmPassword { get; set; }
}

public class RetailOrder : Order
{
    public string CustomerEmail { get; set; }
}

public class SubscriptionOrder : Order
{
    public int PeriodMonths { get; set; }
}
