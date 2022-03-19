using Common;
using MetarParserCore.Objects;
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
    public async Task<Metar?> GetForecasts(string stationId)
    {
        return await _metarService.GetMetar(stationId);
    }
}
