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
    private readonly StationService _stationService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, MetarService metarService, StationService stationService)
    {
        _logger = logger;
        _metarService = metarService;
        _stationService = stationService;
    }

    [HttpGet("station")]
    public async Task<List<Station>> GetStations()
    {
        var stations = await _stationService.GetStations();
        return stations;
    }
    [HttpGet("forecast/{stationId}")]
    public async Task<Metar?> GetForecasts(string stationId)
    {
        return await _metarService.GetMetar(stationId);
    }
}
