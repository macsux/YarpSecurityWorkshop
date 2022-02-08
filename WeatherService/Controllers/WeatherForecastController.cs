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

    [HttpGet("station")]
    public async Task<List<Station>> GetStations()
    {
        var stations = await _metarService.GetStations();
        return stations.Select(x => x.Value).ToList();
    }
    [HttpGet("forecast")]
    public async Task<Metar?> GetForecasts(string stationId)
    {
        return await _metarService.GetMetar(stationId);
    }
}
