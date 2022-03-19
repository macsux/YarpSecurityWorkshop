using Common;
using Microsoft.AspNetCore.Mvc;

namespace GeoService.Controllers;

[ApiController]
[Route("[controller]")]
public class StationController : ControllerBase
{
    private readonly LocationService _locationService;

    public StationController(LocationService locationService)
    {
        _locationService = locationService;
    }

    [HttpGet("closest", Name = "Closest")]
    public async Task<Station> Get(double latitude, double longitude)
    {
        return await _locationService.FindClosestStation(latitude, longitude);
    }
}