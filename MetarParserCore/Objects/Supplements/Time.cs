using System.Runtime.Serialization;

namespace MetarParserCore.Objects.Supplements
{
    /// <summary>
    /// Custom time class
    /// </summary>
    [DataContract(Name = "time")]
    public class Time
    {
        /// <summary>
        /// Hours
        /// </summary>
        [DataMember(Name = "hours", EmitDefaultValue = false)]
        public int Hours { get; init; }

        /// <summary>
        /// Minutes
        /// </summary>
        [DataMember(Name = "minutes", EmitDefaultValue = false)]
        public int Minutes { get; init; }

        #region Constructors

        /// <summary>
        /// Default
        /// </summary>
        public Time() { }

        /// <summary>
        /// Internal constructor
        /// </summary>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        internal Time(int hours, int minutes)
        {
            Hours = hours;
            Minutes = minutes;
        }

        #endregion
    }
}
