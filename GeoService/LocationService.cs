using Common;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using NetTopologySuite.Operation.Distance;
using Coordinate = NetTopologySuite.Geometries.Coordinate;

namespace GeoService;

public class LocationService : BackgroundService
{
    private readonly ILogger<LocationService> _logger;
    private GeometryFactory _gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(4326);
    private MultiPoint _stations;

    public LocationService(ILogger<LocationService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var httpClient = new HttpClient();
        var stationsList = await httpClient.GetFromJsonAsync<List<Station>>("http://localhost:5130/WeatherForecast/station", stoppingToken);
        // _stations = stationsList.ToDictionary(x => new Coordinate(x.Latitude, x.Longitude), x => x.StationId);
        
        var stationPoints = stationsList.Select(station =>
        {
            var point = _gf.CreatePoint(new Coordinate(station.Longitude, station.Latitude));
            point.UserData = station.StationId;
            return point;
        }).ToArray();
        _stations = _gf.CreateMultiPoint(stationPoints);
    }

    public Station FindClosestStation(double latitude, double longitude)
    {
        var sourceLocation = _gf.CreatePoint(new Coordinate(longitude, latitude));
        var distOp = new DistanceOp(sourceLocation, _stations);
        
        var closestPoint = distOp.NearestLocations()
            .Skip(1) // closest point is the one we actually searched by
            .First()
            .GeometryComponent;
        return new Station((string)closestPoint.UserData, closestPoint.Coordinate.Y, closestPoint.Coordinate.X);
    }
}