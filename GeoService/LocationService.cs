using Common;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using NetTopologySuite.Operation.Distance;
using Coordinate = NetTopologySuite.Geometries.Coordinate;

namespace GeoService;

public class LocationService : BackgroundService
{
    private readonly ILogger<LocationService> _logger;
    private const int EPSG4326_SRID = 4326; // projection format for world
    private GeometryFactory _gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(EPSG4326_SRID);
    private MultiPoint _stations;
    private readonly HttpClient _httpClient;
    TaskCompletionSource _initialized = new();

    public LocationService(ILogger<LocationService> logger, IHttpClientFactory clientFactory)
    {
        _logger = logger;
        _httpClient = clientFactory.CreateClient("WeatherService");
        _stations = _gf.CreateMultiPoint();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var stationsList = await _httpClient.GetFromJsonAsync<List<Station>>("station", stoppingToken) ?? new();
        
        var stationPoints = stationsList.Select(station =>
        {
            var point = _gf.CreatePoint(new Coordinate(station.Longitude, station.Latitude));
            point.UserData = station;
            return point;
        }).ToArray();
        _stations = _gf.CreateMultiPoint(stationPoints);
        _logger.LogInformation("Loaded {StationCount} stations", stationsList.Count);
        _initialized.TrySetResult();
    }

    public async Task<Station> FindClosestStation(double latitude, double longitude)
    {
        await _initialized.Task;
        var sourceLocation = _gf.CreatePoint(new Coordinate(longitude, latitude));
        var distOp = new DistanceOp(sourceLocation, _stations);
        
        var closestPoint = distOp.NearestLocations()
            .Skip(1) // closest point is the one we actually searched by
            .First()
            .GeometryComponent;
        return (Station)closestPoint.UserData;
    }
}