using Microsoft.AspNetCore.Authorization;

namespace Sakila.Api.Authorization;

// Policy
public record AgeRequirment(int age) : IAuthorizationRequirement; // mark interface

public class AgeRequirmentHandler : AuthorizationHandler<AgeRequirment>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AgeRequirment requirement)
    {
        var user = context.User;

        if (user.HasClaim(c => c.Type == "DateOfBirth"))
        {
            var dateOfBirth = DateTime.Parse(user.FindFirst(c => c.Type == "DateOfBirth").Value);

            var age = DateTime.Today.Year - dateOfBirth.Year;

            if (age >= requirement.age)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }

        return Task.CompletedTask;
    }
}

