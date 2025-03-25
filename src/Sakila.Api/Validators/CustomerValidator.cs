using FluentValidation;
using Sakila.Api.Domain.Models;

namespace Sakila.Api.Validators;

public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(10);


        RuleFor(p => p.Pesel).Must(ValidatePesel);
    }

    private bool ValidatePesel(string pesel)
    {
        throw new NotImplementedException();
    }
}