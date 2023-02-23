using System.ComponentModel;

namespace MetarParserCore.Enums
{
    /// <summary>
    /// Convective cloud types
    /// </summary>
    public enum ConvectiveCloudType
    {
        /// <summary>
        /// Not specified
        /// </summary>
        None = 0,

        [Description("CB")]
        Cumulonimbus = 1,

        [Description("TCU")]
        ToweringCumulus = 2,
    }
}
