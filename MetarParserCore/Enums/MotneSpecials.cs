using System.ComponentModel;

namespace MetarParserCore.Enums
{
    /// <summary>
    /// MOTNE special signs
    /// </summary>
    public enum MotneSpecials
    {
        /// <summary>
        /// Special sign not specified
        /// </summary>
        Default = 0,

        /// <summary>
        /// Contamination has disappeared or runway has been cleared
        /// </summary>
        [Description("CLRD")]
        Cleared = 1,

        /// <summary>
        /// Runway closed
        /// </summary>
        [Description("CLSD")]
        Closed = 2,

        /// <summary>
        /// Closed due to snow
        /// </summary>
        [Description("SNOCLO")]
        ClosedToSnow = 3
    }
}
