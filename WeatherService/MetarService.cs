using System.Drawing;
using System.IO.Compression;
using System.Xml;
using System.Xml.Serialization;
using Common;
using MetarParserCore;
using MetarParserCore.Objects;
using NMica.Utils.Collections;

namespace WeatherService;

public class MetarService : BackgroundService
{
    private readonly ILogger<MetarService> _logger;
    private const string MetarDownload = "https://aviationweather.gov/adds/dataserver_current/current/metars.cache.xml.gz";
    private Dictionary<string, Metar> _metars = new();
    private Dictionary<string, Station> _stationLocations = new();
    private TaskCompletionSource _init = new(TaskCreationOptions.RunContinuationsAsynchronously);

    public MetarService(ILogger<MetarService> logger)
    {
        _logger = logger;
    }

    public async Task<Metar?> GetMetar(string stationId)
    {
        await EnsureInitialized();
        _metars.TryGetValue(stationId.ToUpper(), out var result);
        return result;
    }

    public async Task<IReadOnlyDictionary<string, Station>> GetStations()
    {
        await EnsureInitialized();
        return _stationLocations.AsReadOnly();
    }

    private async Task EnsureInitialized() => await _init.Task;
    public async Task Refresh(CancellationToken cancellationToken)
    {
        try
        {
            await DownloadMetars(cancellationToken);
            
        }
        catch (Exception e)
        {
            _logger.LogWarning($"Unable to download new metars. Using cached version");
            _logger.LogDebug(e, e.Message);
        }
        ImportFile();
        
    }

    private void ImportFile()
    {
        var doc = new XmlDocument();
        doc.Load(Path.Combine("data", "metar.xml"));
        var metarParser = new MetarParser();
        _metars= doc.SelectNodes("//METAR/raw_text")!
            .Cast<XmlNode>()
            .Select(node =>
            {
                try
                {
                    return metarParser.Parse(node.InnerText);
                }
                catch (Exception)
                {
                    return null;
                }
            })
            .Where(x => x != null)
            .Select(x => x!)
            .Where(x => x.ObservationDayTime.Day == DateTime.UtcNow.Day) // only for today
            .ToLookup(x => x.Airport)
            .Select(x => x.MaxBy(y => new DateTimeOffset(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, y.ObservationDayTime.Time.Hours, y.ObservationDayTime.Time.Minutes, 0, TimeSpan.Zero))!) // only latest
            .ToDictionary(x => x.Airport, x => x);

        _stationLocations = doc.SelectNodes("//METAR")!
            .Cast<XmlNode>()
            .Where(node => node.SelectSingleNode("station_id")?.InnerText != null)
            .Select(node =>
            {
                float.TryParse(node.SelectSingleNode("latitude")?.InnerText, out var latitude);
                float.TryParse(node.SelectSingleNode("longitude")?.InnerText, out var longitude);
                return new Station(
                    node.SelectSingleNode("station_id")!.InnerText,
                    latitude,
                    longitude
                );
            })
            .Where(x => x.Latitude != 0 && x.Longitude != 0)
            .DistinctBy(x => x.StationId)
            .ToDictionary(x => x.StationId, x => x);
        _init.TrySetResult();
    }

    // private void Import(string[] metars)
    // {
    //     _metars = new MetarParser().Parse(metars)
    //         .Where(x => x.ObservationDayTime.Day == DateTime.UtcNow.Day) // only for today
    //         .ToLookup(x => x.Airport, x => x)
    //         .Select(x => x.MaxBy(y => new DateTimeOffset(DateTime.UtcNow, new TimeSpan(y.ObservationDayTime.Time.Hours, y.ObservationDayTime.Time.Minutes, 0)))!)
    //         .ToDictionary(x => x.Airport);
    // }

    private async Task DownloadMetars(CancellationToken cancellationToken)
    {
        var client = new HttpClient();
        var stream = await client.GetStreamAsync(MetarDownload, cancellationToken);
        await using var gZipStream = new GZipStream(stream, CompressionMode.Decompress);
        
        await using var fileStream = File.OpenWrite(Path.Combine("data", "metar.xml"));
        fileStream.SetLength(0);
        await gZipStream.CopyToAsync(fileStream, cancellationToken);
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Refresh(stoppingToken);
        _init.TrySetResult();
    }
}