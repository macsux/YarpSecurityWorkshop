using System.Collections.Generic;
using System.Runtime.Serialization;
using MetarParserCore.Enums;
using MetarParserCore.Extensions;

namespace MetarParserCore.Objects
{
    /// <summary>
    /// Weather info on military airfields (When they use color codes)
    /// </summary>
    [DataContract(Name = "militaryWeather")]
    public class MilitaryWeather
    {
        /// <summary>
        /// Array of color codes
        /// </summary>
        [DataMember(Name = "codes", EmitDefaultValue = false)]
        public MilitaryColorCode[] Codes { get; init; }

        /// <summary>
        /// Sign if airfield is closed
        /// BLACK color is defined
        /// </summary>
        [DataMember(Name = "isClosed", EmitDefaultValue = false)]
        public bool IsClosed { get; set; }

        #region Constructors

        /// <summary>
        /// Default
        /// </summary>
        public MilitaryWeather() { }

        /// <summary>
        /// Parser constructor
        /// </summary>
        /// <param name="tokens">Array of tokens</param>
        /// <param name="errors">List of parse errors</param>
        internal MilitaryWeather(string[] tokens, List<string> errors)
        {
            if (tokens.Length == 0)
            {
                errors.Add("Array of military codes is empty");
                return;
            }

            var colorCodes = new List<MilitaryColorCode>();
            foreach (var token in tokens)
            {
                var current = token;
                if (token.StartsWith("BLACK"))
                {
                    IsClosed = true;
                    current = current.Replace("BLACK", "");

                    if (current.Length == 0)
                        continue;
                }

                colorCodes.Add(EnumTranslator.GetValueByDescription<MilitaryColorCode>(current));
            }

            Codes = colorCodes is not {Count: 0} ? colorCodes.ToArray() : null;
        }

        #endregion
    }
}
