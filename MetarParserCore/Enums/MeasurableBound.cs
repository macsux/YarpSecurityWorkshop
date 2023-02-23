using System.ComponentModel;

namespace MetarParserCore.Enums
{
    /// <summary>
    /// Enum of marks indicating that a value is outside the bounds of measurement
    /// </summary>
    public enum MeasurableBound
    {
        [Description("None")]
        None = 0,

        /// <summary>
        /// Preceding the lowest measurable value
        /// </summary>
        [Description("M")]
        Lower = 1,

        /// <summary>
        /// Preceding the highest measurable value
        /// </summary>
        [Description("P")]
        Higher = 2
    }
}
