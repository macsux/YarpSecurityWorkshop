using System.ComponentModel;

namespace MetarParserCore.Enums
{
    /// <summary>
    /// RVR trend
    /// </summary>
    public enum RvrTrend
    {
        /// <summary>
        /// Not represented
        /// </summary>
        None = 0,

        /// <summary>
        /// Visibility became better
        /// </summary>
        [Description("U")]
        Upward = 1,

        /// <summary>
        /// Visibility became worse
        /// </summary>
        [Description("D")]
        Downward = 2,

        /// <summary>
        /// Without changes
        /// </summary>
        [Description("N")]
        NoChange = 3
    }
}
