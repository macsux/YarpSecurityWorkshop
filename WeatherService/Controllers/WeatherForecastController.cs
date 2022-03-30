using System.Security.Claims;
using Common;
using MetarParserCore.Enums;
using MetarParserCore.Objects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WeatherService.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly MetarService _metarService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, MetarService metarService)
    {
        _logger = logger;
        _metarService = metarService;
    }


    [HttpGet("{stationId}")]
    public async Task<WeatherReport?> GetForecasts(string stationId)
    {
        var metar = await _metarService.GetMetar(stationId);
        if (metar == null)
            return null;
        var report = new WeatherReport
        {
            StationId = metar.Airport,
            TempC = metar.Temperature.Value,
        };
        // selectively include extra data based on user's role
        if (User.HasClaim(ClaimTypes.Role, "premium"))
        {
            report.Clouds =  metar.CloudLayers?.Select(x => x.CloudType).FirstOrDefault() switch
            {
                CloudType.Broken => "Broken",
                CloudType.Few => "Few",
                CloudType.Overcast => "Overcast",
                CloudType.Scattered => "Scattered",
                _ => "Clear"
            };
            report.RelativeHumidity = 100 - 5 * (metar.Temperature?.Value - metar.Temperature?.DewPoint ?? 0);
            report.WindSpeedKmHr = (int)(metar.SurfaceWind.Speed * 1.852);
        }

        return report;
    }
}
