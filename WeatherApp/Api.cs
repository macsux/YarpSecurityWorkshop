namespace WeatherApp;

public class Api
{
    private readonly IConfiguration _configuration;

    public Api(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string BasePath => _configuration.GetValue<string>("ApiUrl");
}