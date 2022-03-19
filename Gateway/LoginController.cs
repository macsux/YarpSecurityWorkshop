using System.Security.Claims;
using System.Text;
using IdentityServer4;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway;

[Controller]
public class LoginController : Controller
{
    [HttpGet("/login")]
    [Authorize]
    public async Task<IActionResult> Login(string returnUrl)
    {
        if (returnUrl != null)
        {
            return Redirect(returnUrl);
        }
        return Ok(User.Identity?.Name);
        if (!User.IsAuthenticated())
        {
            return Challenge(OpenIdConnectDefaults.AuthenticationScheme);
            // await HttpContext.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }
        // var claims = new List<Claim> {  
        //     new(ClaimTypes.NameIdentifier, "astakhov"),  
        //     new(ClaimTypes.Name, "astakhov"),  
        // };
        // var identity = new ClaimsIdentity(claims, "auto");
        // var principal = new ClaimsPrincipal(identity);
        // await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        // return Redirect(returnUrl);
        
        return Ok();
    }

    [HttpGet("/logout")]
    [Authorize]
    public async Task<IActionResult> Logout(string returnUrl)
    {
        return new SignOutResult(new []
        {
            OpenIdConnectDefaults.AuthenticationScheme, 
            CookieAuthenticationDefaults.AuthenticationScheme
        }, 
        new AuthenticationProperties()
        {
            RedirectUri = returnUrl
        });
    }
    //
    // [HttpGet("/jwt")]
    // public async Task<string> Jwt([FromServices]IdentityServerTools identityServerTools)
    // {
    //     
    //     return await identityServerTools.IssueJwtAsync(60, User.Claims);
    // }

    [HttpGet("/whoami")]
    // [Authorize("authenticated")]
    public async Task<List<ClaimRecord>> WhoAmI()
    {
        return User.Claims.Select(x => new ClaimRecord(x.Type, x.Value)).ToList();
    }
    public record ClaimRecord(string Type, object Value);
}