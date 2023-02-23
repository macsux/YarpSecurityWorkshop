using System.ComponentModel;

namespace MetarParserCore.Enums
{
    /// <summary>
    /// Military color codes
    /// </summary>
    public enum MilitaryColorCode
    {
        /// <summary>
        /// Unknown code
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Visibility is greater 5 mi, ceiling is greater 2500 ft
        /// </summary>
        [Description("BLU")]
        Blue = 1,

        /// <summary>
        /// Visibility 3 3/8 - 5 mi, ceiling 1500 - 2500 ft
        /// </summary>
        [Description("WHT")]
        White = 2,

        /// <summary>
        /// Visibility 2 1/4 - 3 - 1/8, ceiling > 700 - 1500 ft
        /// </summary>
        [Description("GRN")]
        Green = 3,

        /// <summary>
        /// Visibility 1 1/8 - 2 - 1/4, ceiling > 300 - 700 ft
        /// </summary>
        [Description("YLO")]
        Yellow = 4,

        /// <summary>
        /// Visibility 1/2 - 1 1/8 mi, ceiling 200 - 300 ft
        /// </summary>
        [Description("AMB")]
        Amber = 5,

        /// <summary>
        /// Visibility less 1/2 mi, ceiling less 200 ft
        /// </summary>
        [Description("RED")]
        Red = 6
    }
}
