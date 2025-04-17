using Microsoft.AspNetCore.Authentication;
using Sakila.Api.Domain.Abstractions;
using System.Security.Claims;

namespace Sakila.Api.Authorization;

public class PermissionClaimsTransformer : IClaimsTransformation
{
    private readonly IPermissionService _permissionService;

    public PermissionClaimsTransformer(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var permissions = await _permissionService.GetPermissionsAsync(principal.Identity.Name);

        foreach (var permission in permissions)
        {
            var identity = principal.Identity as ClaimsIdentity;

            identity.AddClaim(new Claim("permission", permission));
        }

        return principal;
    }
}
