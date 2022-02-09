namespace WeatherService;

public abstract class DataFeedService : BackgroundService
{
    protected ILogger Logger { get; }

    protected DataFeedService(ILogger logger)
    {
        Logger = logger;
    }

    public async Task Refresh(CancellationToken cancellationToken)
    {
        try
        {
            await Download(cancellationToken);
            
        }
        catch (Exception e)
        {
            Logger.LogWarning($"Unable to download new data. Using cached version");
            Logger.LogDebug(e, e.Message);
        }
        Import();
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Refresh(stoppingToken);
    }

    protected abstract void Import();

    protected abstract Task Download(CancellationToken cancellationToken);
}