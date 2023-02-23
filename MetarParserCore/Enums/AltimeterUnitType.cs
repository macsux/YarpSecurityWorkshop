using System.ComponentModel;

namespace MetarParserCore.Enums
{
    /// <summary>
    /// Altimeter types
    /// </summary>
    public enum AltimeterUnitType
    {
        /// <summary>
        /// Not specified
        /// </summary>
        None = 0,

        [Description("Q")]
        Hectopascal = 1,

        [Description("A")]
        InchesOfMercury = 2
    }
}
