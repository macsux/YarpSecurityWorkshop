namespace Common;

public class WeatherReport
{
   
    public string StationId { get; init; }
    public int TempC { get; init; }
    public int TempF => (int)(TempC * 1.8 + 32);
    public int? RelativeHumidity { get; set; }
    public string? Clouds { get; set; }
    public int? WindSpeedKmHr { get; set; }
}