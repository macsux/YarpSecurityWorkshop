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
    public IActionResult Login(string returnUrl)
    {
        if (returnUrl != null)
        {
            return Redirect(returnUrl);
        }
        return Ok(User.Identity?.Name);
    }

    [HttpGet("/logout")]
    [Authorize]
    public IActionResult Logout(string returnUrl)
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


    [HttpGet("/whoami")]
    public List<ClaimRecord> WhoAmI()
    {
        return User.Claims.Select(x => new ClaimRecord(x.Type, x.Value)).ToList();
    }
    public record ClaimRecord(string Type, object Value);
}