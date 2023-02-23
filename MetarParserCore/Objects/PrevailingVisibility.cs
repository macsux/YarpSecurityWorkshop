using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using MetarParserCore.Common;
using MetarParserCore.Objects.Supplements;

namespace MetarParserCore.Objects
{
    /// <summary>
    /// Horizontal visibility at the surface of the earth
    /// </summary>
    [DataContract(Name = "prevailingVisibility")]
    public class PrevailingVisibility
    {
        /// <summary>
        /// Sign if visibility marked as CAVOK
        /// Means Ceiling and Visibility OK
        /// </summary>
        [DataMember(Name = "isCavok", EmitDefaultValue = false)]
        public bool IsCavok { get; init; }

        /// <summary>
        /// Prevailing visibility in meters
        /// </summary>
        [DataMember(Name = "visibilityInMeters", EmitDefaultValue = false)]
        public VisibilityInMeters VisibilityInMeters { get; init; }

        /// <summary>
        /// Prevailing visibility in statute miles
        /// </summary>
        [DataMember(Name = "visibilityInStatuteMiles", EmitDefaultValue = false)]
        public VisibilityInStatuteMiles VisibilityInStatuteMiles { get; init; }

        #region Constructors

        /// <summary>
        /// Default
        /// </summary>
        public PrevailingVisibility() { }

        /// <summary>
        /// Parser constructor
        /// </summary>
        /// <param name="tokens">Array of tokens</param>
        /// <param name="errors">List of parse errors</param>
        internal PrevailingVisibility(string[] tokens, List<string> errors)
        {
            if (tokens.Length == 0)
            {
                errors.Add("Array of prevailing visibility tokens is empty");
                return;
            }

            var visibilityToken = tokens.First();
            if (visibilityToken.Equals("CAVOK"))
            {
                IsCavok = true;
                return;
            }

            if (Regex.IsMatch(visibilityToken, ParseRegex.VisibilityWholeNumber) || Regex.IsMatch(visibilityToken, ParseRegex.StatuteMilesVisibility))
            {
                VisibilityInStatuteMiles = new VisibilityInStatuteMiles(tokens);
                return;
            }

            if (Regex.IsMatch(visibilityToken, ParseRegex.MetersVisibility))
            {
                VisibilityInMeters = new VisibilityInMeters(tokens);
                return;
            }

            errors.Add($"Unexpected token: {visibilityToken}");
        }

        #endregion
    }
}
