using System.Collections.Generic;
using System.Runtime.Serialization;
using MetarParserCore.Objects.Supplements;

namespace MetarParserCore.Objects
{
    /// <summary>
    /// Date and time of the airport by Zulu
    /// </summary>
    [DataContract(Name = "observationDayTime")]
    public class ObservationDayTime
    {
        /// <summary>
        /// Day of the current month
        /// </summary>
        [DataMember(Name = "day", EmitDefaultValue = false)]
        public int Day { get; init; }

        /// <summary>
        /// Time of the observation
        /// </summary>
        [DataMember(Name = "time", EmitDefaultValue = false)]
        public Time Time { get; init; }

        #region Constructors

        /// <summary>
        /// Default
        /// </summary>
        public ObservationDayTime() { }

        /// <summary>
        /// Parser constructor
        /// </summary>
        /// <param name="tokens">Array of tokens</param>
        /// <param name="errors">List of parse errors</param>
        internal ObservationDayTime(string[] tokens, List<string> errors)
        {
            if (tokens.Length == 0)
            {
                errors.Add("Array of observation day time tokens is empty");
                return;
            }

            Day = int.Parse(tokens[0].Substring(0, 2));
            Time = new Time(int.Parse(tokens[0].Substring(2, 2)), int.Parse(tokens[0].Substring(4, 2)));
        }

        #endregion
    }
}
