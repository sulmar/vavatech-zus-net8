using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Sakila.Api.Authorization;


public class CustomClaimsTransformer : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var identity = principal.Identity as ClaimsIdentity;

        if (identity != null && identity.IsAuthenticated)
        {
            // Add custom claims here
            identity.AddClaim(new Claim("CustomClaim", "CustomValue"));
        }

        return Task.FromResult(principal);
    }
}
