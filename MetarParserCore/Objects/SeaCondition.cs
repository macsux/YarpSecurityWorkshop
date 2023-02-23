using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using MetarParserCore.Enums;

namespace MetarParserCore.Objects
{
    /// <summary>
    /// Info about sea-surface temperature and state
    /// </summary>
    [DataContract(Name = "seaCondition")]
    public class SeaCondition
    {
        /// <summary>
        /// Temperature in Celsius
        /// </summary>
        [DataMember(Name = "seaTemperature", EmitDefaultValue = false)]
        public int SeaTemperature { get; init; }

        /// <summary>
        /// Average height of the waves in decimeters
        /// </summary>
        [DataMember(Name = "waveHeight", EmitDefaultValue = false)]
        public int WaveHeight { get; init; }

        /// <summary>
        /// Sea state
        /// </summary>
        [DataMember(Name = "seaState")]
        public SeaStateType SeaState { get; init; }

        #region Constructors

        /// <summary>
        /// Default
        /// </summary>
        public SeaCondition() { }

        /// <summary>
        /// Parser constructor
        /// </summary>
        /// <param name="tokens">Array of tokens</param>
        /// <param name="errors">List of parse errors</param>
        internal SeaCondition(string[] tokens, List<string> errors)
        {
            if (tokens.Length == 0)
            {
                errors.Add("Array of sea condition tokens is empty");
                return;
            }

            var firstToken = tokens.First();
            var splittedToken = firstToken.Split("/");

            SeaTemperature = GetSeaTemperature(splittedToken[0]);

            var stateToken = splittedToken[1];
            if (stateToken.Contains("H"))
            {
                WaveHeight = int.Parse(stateToken[1..]);
                SeaState = SeaStateType.None;
                return;
            }

            SeaState = stateToken.Contains("/")
                ? SeaStateType.None
                : (SeaStateType)int.Parse(stateToken[1..]);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Get sea surface temperature
        /// </summary>
        /// <param name="temperatureString">Temperature string</param>
        /// <returns></returns>
        private int GetSeaTemperature(string temperatureString)
        {
            temperatureString = temperatureString.Replace("W", "");

            if (!temperatureString.StartsWith("M"))
                return int.Parse(temperatureString);

            temperatureString = temperatureString.Replace("M", "");

            return int.Parse(temperatureString) * -1;
        }

        #endregion
    }
}
