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
    public Station Get(double latitude, double longitude)
    {
        return _locationService.FindClosestStation(latitude, longitude);
    }
}