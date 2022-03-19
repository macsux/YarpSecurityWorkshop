using Common;
using Microsoft.AspNetCore.Mvc;

namespace WeatherService.Controllers;

[Controller]
[Route("[controller]")]
public class StationController : Controller
{
    private readonly StationService _stationService;

    public StationController(ILogger<StationController> logger, StationService stationService)
    {
        _stationService = stationService;

    }
    
    [HttpGet]
    public async Task<List<Station>> GetStations()
    {
        var stations = await _stationService.GetStations();
        return stations;
    }
}