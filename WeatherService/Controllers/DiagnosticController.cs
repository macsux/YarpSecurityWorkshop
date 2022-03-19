using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WeatherService.Controllers;

[Controller]
public class DiagnosticController : Controller
{
    [HttpGet("/diag")]
    // [Authorize("loggedin")]
    public string Diag()
    {
        StringBuilder sb = new();
        foreach (var header in Request.Headers)
        {
            sb.AppendLine($"{header.Key}: {header.Value.FirstOrDefault()}");
        }

        sb.AppendLine("========Claims=======");
        foreach (var claim in User.Claims)
        {
            sb.AppendLine($"{claim.Type}: {claim.Value}");
        }

        return sb.ToString();
    }
}