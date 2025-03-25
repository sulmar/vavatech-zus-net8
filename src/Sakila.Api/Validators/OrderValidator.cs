using FluentValidation;
using Sakila.Api.Domain.Models;

namespace Sakila.Api.Validators;

public class OrderValidator : AbstractValidator<Order>
{
    public OrderValidator(IValidator<Customer> customerValidator)
    {
        RuleFor(p=>p.TotalAmount)
            .InclusiveBetween(1, 100);

        RuleFor(p => p.Customer)
            .NotNull().WithMessage("Klient jest wymagany")
            .SetValidator(customerValidator);
    }
}
