using System.ComponentModel;

namespace MetarParserCore.Enums
{
    /// <summary>
    /// TREND report types
    /// </summary>
    public enum TrendType
    {
        /// <summary>
        /// Not specified
        /// </summary>
        None = 0,

        /// <summary>
        /// A changes may happen or not
        /// </summary>
        [Description("BECMG")]
        Becoming = 1,

        /// <summary>
        /// Changes is definitely happen
        /// </summary>
        [Description("TEMPO")]
        Tempo = 2,

        [Description("NOSIG")]
        NoSignificantChanges = 3
    }
}
