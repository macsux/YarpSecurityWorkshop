using System.Security.Claims;
using Common;
using Microsoft.AspNetCore.Authentication;

namespace Gateway;

public class ClaimsTransformer : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var newClaims = new List<Claim>(principal.Claims);
        var name = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (name == "andrew")
        {
            newClaims.Add(LobClaims.PremiumUser);
            
        }
        principal = new ClaimsPrincipal(new ClaimsIdentity(newClaims, principal!.Identity!.AuthenticationType, ClaimTypes.NameIdentifier, ClaimTypes.Role));
        return Task.FromResult(principal);
    }
}