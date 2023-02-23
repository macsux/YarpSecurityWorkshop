using System.ComponentModel;

namespace MetarParserCore.Enums
{
    /// <summary>
    /// Visibility directions enum
    /// </summary>
    public enum VisibilityDirection
    {
        [Description("None")]
        NotSet = 0,

        [Description("N")]
        North = 1,

        [Description("NE")]
        NorthEast = 2,

        [Description("E")]
        East = 3,

        [Description("SE")]
        SouthEast = 4,

        [Description("S")]
        South = 5,

        [Description("SW")]
        SouthWest = 6,

        [Description("W")]
        West = 7,

        [Description("NW")]
        NorthWest = 8
    }
}
