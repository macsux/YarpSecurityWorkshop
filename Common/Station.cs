namespace Common;

public struct Station
{
    public Station(string stationId, double latitude, double longitude)
    {
        StationId = stationId;
        Latitude = latitude;
        Longitude = longitude;
    }

    public string StationId { get; set; }
    public double Latitude { get; set;}
    public double Longitude { get; set;}
}