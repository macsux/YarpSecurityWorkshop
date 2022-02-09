using System.IO.Compression;
using System.Xml;
using System.Xml.Serialization;
using Common;

namespace WeatherService;

public class StationService : DataFeedService
{
    private const string StationsDownload = "https://aviationweather.gov/adds/dataserver_current/httpparam?dataSource=stations&requestType=retrieve&format=xml";
    private List<Station> _stations = new();
    public StationService(ILogger<StationService> logger) : base(logger)
    {
    }

    public Task<List<Station>> GetStations() => Task.FromResult(_stations);

    protected override async Task Download(CancellationToken cancellationToken)
    {
        var client = new HttpClient();
        var stream = await client.GetStreamAsync(StationsDownload, cancellationToken);
        
        await using var fileStream = File.OpenWrite(Path.Combine("data", "stations.xml"));
        fileStream.SetLength(0);
        await stream.CopyToAsync(fileStream, cancellationToken);
    }

    protected override void Import()
    {
        var doc = new XmlDocument();
        doc.Load(Path.Combine("data", "stations.xml"));
        var serializer = new XmlSerializer(typeof(Station));
        _stations = doc.SelectNodes("//Station")!
            .Cast<XmlNode>()
            .Select(node =>
            {
                using var reader = new XmlNodeReader(node);
                return (Station)serializer.Deserialize(reader)!;
            })
            .ToList();
    }
    
}