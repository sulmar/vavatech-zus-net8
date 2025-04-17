using Microsoft.AspNetCore.Authorization;
using Sakila.Api.Domain.Models;

namespace Sakila.Api.Authorization;

public record DocumentOwnerRequriment : IAuthorizationRequirement; // mark interface

public class DocumentOwnerRequrimentHandler : AuthorizationHandler<IAuthorizationRequirement, Document>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IAuthorizationRequirement requirement, Document resource)
    {
        var userId = context.User.Identity.Name;

        if (userId == resource.Owner)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }


        return Task.CompletedTask;
    }
}