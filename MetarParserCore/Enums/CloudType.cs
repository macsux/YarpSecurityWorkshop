using System.ComponentModel;

namespace MetarParserCore.Enums
{
    /// <summary>
    /// Enum of cloud types
    /// </summary>
    public enum CloudType
    {
        /// <summary>
        /// Not specified
        /// </summary>
        None = 0,

        /// <summary>
        /// Sky clear - No cloud present
        /// </summary>
        [Description("SKC")]
        SkyClear = 1,

        /// <summary>
        /// Few - 1-2 oktas
        /// </summary>
        [Description("FEW")]
        Few = 2,

        /// <summary>
        /// Scattered - 3-4 oktas
        /// </summary>
        [Description("SCT")]
        Scattered = 3,

        /// <summary>
        /// Broken - 5-7 oktas
        /// </summary>
        [Description("BKN")]
        Broken = 4,

        /// <summary>
        /// Overcast - 8 oktas
        /// </summary>
        [Description("OVC")]
        Overcast = 5,

        /// <summary>
        /// Vertical visibility - indefinite ceiling
        /// </summary>
        [Description("VV")]
        VerticalVisibility = 6,

        /// <summary>
        /// Clear below 10,000 ft as interpreted by an autostation
        /// </summary>
        [Description("CLR")]
        Clear = 7,

        /// <summary>
        /// No significant clouds - clouds present at and above 5,000 ft
        /// </summary>
        [Description("NSC")]
        NoSignificantClouds = 8,

        /// <summary>
        /// No cloud detected
        /// </summary>
        [Description("NCD")]
        NoCloudDetected = 9
    }
}
