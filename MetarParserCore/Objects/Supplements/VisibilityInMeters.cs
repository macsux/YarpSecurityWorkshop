using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using MetarParserCore.Common;
using MetarParserCore.Enums;
using MetarParserCore.Extensions;

namespace MetarParserCore.Objects.Supplements
{
    /// <summary>
    /// Prevailing visibility in meters
    /// </summary>
    [DataContract(Name = "visibilityInMeters")]
    public class VisibilityInMeters
    {
        /// <summary>
        /// Visibility value in meters
        /// </summary>
        [DataMember(Name = "visibilityValue", EmitDefaultValue = false)]
        public int VisibilityValue { get; init; }

        /// <summary>
        /// Direction of the represented visibility
        /// </summary>
        [DataMember(Name = "visibilityDirection", EmitDefaultValue = false)]
        public VisibilityDirection VisibilityDirection { get; init; }

        /// <summary>
        /// Max visibility value
        /// </summary>
        [DataMember(Name = "maxVisibilityValue", EmitDefaultValue = false)]
        public int MaxVisibilityValue { get; init; }

        /// <summary>
        /// Max visibility direction
        /// </summary>
        [DataMember(Name = "maxVisibilityDirection", EmitDefaultValue = false)]
        public VisibilityDirection MaxVisibilityDirection { get; init; }

        #region Constructors

        /// <summary>
        /// Default
        /// </summary>
        public VisibilityInMeters() { }

        /// <summary>
        /// Parse constructor
        /// </summary>
        /// <param name="tokens">Array of tokens</param>
        internal VisibilityInMeters(string[] tokens)
        {
            var dataTuples = ProcessVisibilityTokens(tokens);

            VisibilityValue = dataTuples[0].Value;
            VisibilityDirection = dataTuples[0].Direction;

            if (dataTuples.Count == 1)
                return;

            MaxVisibilityValue = dataTuples[1].Value;
            MaxVisibilityDirection = dataTuples[1].Direction;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Process all visibility tokens
        /// </summary>
        /// <param name="tokens">Array of tokens</param>
        /// <returns></returns>
        private List<(int Value, VisibilityDirection Direction)> ProcessVisibilityTokens(string[] tokens)
        {
            var outcome = new List<(int Value, VisibilityDirection Direction)>();

            foreach (var token in tokens)
            {
                var direction = VisibilityDirection.NotSet;
                if (Regex.IsMatch(token, ParseRegex.MetersVisibilityContainsDirections))
                {
                    var directionString = token.Substring(4, token.Length - 4);
                    direction = EnumTranslator.GetValueByDescription<VisibilityDirection>(directionString);
                }

                outcome.Add((int.Parse(token.Substring(0, 4)), direction));
            }

            return outcome;
        }

        #endregion
    }
}
