using System.ComponentModel;

namespace MetarParserCore.Enums
{
    /// <summary>
    /// Wind unit types
    /// </summary>
    public enum WindUnit
    {
        None = 0,

        [Description("MPS")]
        MetersPerSecond = 1,

        [Description("KMT")]
        KilometersPerHour = 2,

        [Description("KT")]
        Knots = 3
    }
}
