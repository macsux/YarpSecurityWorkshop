using System.Net.Http.Headers;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4;
using Yarp.ReverseProxy.Transforms;

namespace NMica.SecurityProxy.Middleware.Transforms;

public class JwtPrincipalAppender : RequestTransform
{
    private readonly IdentityServerTools _identityServerTools;

    public JwtPrincipalAppender(IdentityServerTools identityServerTools)
    {
        _identityServerTools = identityServerTools;
    }

    public override async ValueTask ApplyAsync(RequestTransformContext context)
    {
        var user = context.HttpContext.User;
        if (user.Identity?.IsAuthenticated ?? false)
        {
            var claims = user.Claims.ToList();
            var nameIdentifierClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (nameIdentifierClaim is not null && !user.HasClaim(x => x.Type == ClaimTypes.Name))
            {
                claims.Remove(nameIdentifierClaim);
                claims.Add(new Claim(ClaimTypes.Name, nameIdentifierClaim.Value));
            }

            claims.Add(new Claim(JwtClaimTypes.Audience, context.DestinationPrefix));
            var jwt = await _identityServerTools.IssueJwtAsync(60, claims);
            context.ProxyRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        }
    }

}