using System.Collections.Generic;
using System.Runtime.Serialization;
using MetarParserCore.Enums;
using MetarParserCore.Extensions;

namespace MetarParserCore.Objects
{
    /// <summary>
    /// Info about windshear on runways
    /// </summary>
    [DataContract(Name = "windShear")]
    public class WindShear
    {
        /// <summary>
        /// Wind shear on all runways
        /// </summary>
        [DataMember(Name = "isAll", EmitDefaultValue = false)]
        public bool IsAll { get; init; }

        /// <summary>
        /// Wind shear type
        /// </summary>
        [DataMember(Name = "type")]
        public WindShearType Type { get; init; }

        /// <summary>
        /// Runway number
        /// </summary>
        [DataMember(Name = "runway", EmitDefaultValue = false)]
        public string Runway { get; init; }

        #region Constructors

        /// <summary>
        /// Default
        /// </summary>
        public WindShear() { }

        /// <summary>
        /// Parse constructor
        /// </summary>
        internal WindShear(string[] tokens, List<string> errors)
        {
            var length = tokens.Length;
            var type = WindShearType.Both;
            var runwayTokenIdx = 1;

            if (length < 2)
            {
                errors.Add("Array with wind shear tokens is incorrect");
                return;
            }

            switch (tokens[1])
            {
                case "ALL":
                    IsAll = true;
                    return;
                case "TKOF":
                case "LDG":
                    type = EnumTranslator.GetValueByDescription<WindShearType>(tokens[1]);
                    break;
            }

            if (type != WindShearType.Both)
            {
                if (length < 3)
                {
                    errors.Add("Array with wind shear tokens is incorrect");
                    return;
                }

                runwayTokenIdx = 2;
            }

            Type = type;
            Runway = GetCleanRunwayNumber(tokens[runwayTokenIdx]);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Get runway number without prefix "R" or "RWY"
        /// </summary>
        /// <param name="runwayToken">Current runway token</param>
        /// <returns></returns>
        private string GetCleanRunwayNumber(string runwayToken)
        {
            return runwayToken.StartsWith("RWY")
                ? runwayToken[3..]
                : runwayToken[1..];
        }

        #endregion
    }
}
