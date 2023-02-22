using System.Drawing;
using System.IO.Compression;
using System.Security.Claims;
using System.Security.Principal;
using System.Xml;
using System.Xml.Serialization;
using Common;
using MetarParserCore;
using MetarParserCore.Enums;
using MetarParserCore.Objects;
using Microsoft.AspNetCore.Authorization;
using NMica.Utils.Collections;

namespace WeatherService;

public class MetarService : DataFeedService
{
    
    private readonly IHttpContextAccessor _context;
    private const string MetarDownload = "https://aviationweather.gov/adds/dataserver_current/current/metars.cache.xml.gz";
    private Dictionary<string, Metar> _metars = new();

    private ClaimsPrincipal User => _context.HttpContext?.User ?? new ClaimsPrincipal();

    public MetarService(ILogger<MetarService> logger, IHttpContextAccessor context) : base(logger)
    {
        _context = context;
    }
    
   
    public async Task<Metar?> GetMetar(string stationId)
    {
        await Initialized.Task;
        _metars.TryGetValue(stationId.ToUpper(), out var result);
        return result;
    }

   
  
    protected override void Import()
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
            .Where(x => x?.ObservationDayTime != null && x.Airport != null)
            .Select(x => x!)
            .Where(x => x.ObservationDayTime.Day == DateTime.UtcNow.Day) // only for today
            .ToLookup(x => x.Airport)
            .Select(x => x.MaxBy(y => new DateTimeOffset(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, y.ObservationDayTime.Time.Hours, y.ObservationDayTime.Time.Minutes, 0, TimeSpan.Zero))!) // only latest
            .ToDictionary(x => x.Airport, x => x);

        
    }
    
    protected override async Task Download(CancellationToken cancellationToken)
    {
        var client = new HttpClient();
        var stream = await client.GetStreamAsync(MetarDownload, cancellationToken);
        await using var gZipStream = new GZipStream(stream, CompressionMode.Decompress);
        
        await using var fileStream = File.OpenWrite(Path.Combine("data", "metar.xml"));
        fileStream.SetLength(0);
        await gZipStream.CopyToAsync(fileStream, cancellationToken);
    }



}