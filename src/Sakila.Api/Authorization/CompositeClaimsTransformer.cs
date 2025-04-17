using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Sakila.Api.Authorization;

// Wzorzec Composite
public class CompositeClaimsTransformer : IClaimsTransformation
{
    private readonly IEnumerable<IClaimsTransformation> _transformers;

    public CompositeClaimsTransformer(IEnumerable<IClaimsTransformation> transformers)
    {
        _transformers = transformers;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        foreach (var transformer in _transformers)
        {
            await transformer.TransformAsync(principal);
        }

        return principal;
    }
}
