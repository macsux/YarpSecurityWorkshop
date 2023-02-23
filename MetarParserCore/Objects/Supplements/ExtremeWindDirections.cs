using System.Runtime.Serialization;

namespace MetarParserCore.Objects.Supplements
{
    /// <summary>
    /// Info about two extreme wind directions
    /// during the 10 minute period of the observation
    /// </summary>
    [DataContract(Name = "extremeWindDirections")]
    public class ExtremeWindDirections
    {
        /// <summary>
        /// First value of the extreme wind direction interval
        /// </summary>
        [DataMember(Name = "firstExtremeDirection", EmitDefaultValue = false)]
        public int FirstExtremeDirection { get; init; }

        /// <summary>
        /// Last value of the extreme wind direction interval
        /// </summary>
        [DataMember(Name = "lastExtremeWindDirection", EmitDefaultValue = false)]
        public int LastExtremeWindDirection { get; init; }
    }
}
