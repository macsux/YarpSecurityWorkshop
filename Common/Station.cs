using System.Xml.Serialization;

namespace Common;

public record Station
{
    [XmlElement("station_id")]
    public string StationId { get; set; }
    [XmlElement("latitude")]
    public double Latitude { get; set;}
    [XmlElement("longitude")]
    public double Longitude { get; set;}
    [XmlElement("site")]
    public string Site { get; set;}
    [XmlElement("state")]
    public string State { get; set;}
}