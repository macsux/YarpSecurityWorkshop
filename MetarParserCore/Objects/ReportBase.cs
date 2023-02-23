using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using MetarParserCore.Enums;
using MetarParserCore.Extensions;

namespace MetarParserCore.Objects
{
    /// <summary>
    /// Base abstract class of all meteorological reports
    /// </summary>
    [DataContract(Name = "reportBase")]
    public abstract class ReportBase
    {
        /// <summary>
        /// METAR report type
        /// </summary>
        [DataMember(Name = "reportType")]
        public ReportType ReportType { get; init; }

        /// <summary>
        /// Report is empty
        /// </summary>
        [DataMember(Name = "isNil", EmitDefaultValue = false)]
        public bool IsNil { get; init; }

        /// <summary>
        /// Current month
        /// </summary>
        [DataMember(Name = "month", EmitDefaultValue = false)]
        public Month Month { get; init; }

        /// <summary>
        /// METAR modifier
        /// </summary>
        [DataMember(Name = "modifier", EmitDefaultValue = false)]
        public MetarModifier Modifier { get; init; }

        /// <summary>
        /// Info about surface wind
        /// </summary>
        [DataMember(Name = "surfaceWind", EmitDefaultValue = false)]
        public SurfaceWind SurfaceWind { get; init; }

        /// <summary>
        /// Info about visibility
        /// </summary>
        [DataMember(Name = "prevailingVisibility", EmitDefaultValue = false)]
        public PrevailingVisibility PrevailingVisibility { get; init; }

        /// <summary>
        /// Special weather conditions
        /// </summary>
        [DataMember(Name = "presentWeather", EmitDefaultValue = false)]
        public WeatherPhenomena[] PresentWeather { get; init; }

        /// <summary>
        /// Info about clouds (Cloud layers)
        /// </summary>
        [DataMember(Name = "cloudLayers", EmitDefaultValue = false)]
        public CloudLayer[] CloudLayers { get; init; }

        /// <summary>
        /// Array of parse errors
        /// </summary>
        [DataMember(Name = "parseErrors", EmitDefaultValue = false)]
        public string[] ParseErrors { get; set; }

        /// <summary>
        /// Unrecognized tokens by METAR TokenRecognizer
        /// </summary>
        [DataMember(Name = "unrecognized", EmitDefaultValue = false)]
        public string[] Unrecognized { get; set; }

        #region Constructors

        /// <summary>
        /// Default
        /// </summary>
        public ReportBase() { }

        /// <summary>
        /// Parse constructor
        /// </summary>
        internal ReportBase(Dictionary<TokenType, string[]> groupedTokens, Month currentMonth)
        {
            if (groupedTokens.Count == 0)
            {
                ParseErrors = new[] { "Grouped tokens dictionary is empty" };
                return;
            }

            var errors = new List<string>();

            Month = currentMonth;
            Modifier = GetMetarModifier(groupedTokens);
            SurfaceWind = GetDataObjectOrNull<SurfaceWind>(groupedTokens.GetTokenGroupOrDefault(TokenType.SurfaceWind), errors);
            PrevailingVisibility = GetDataObjectOrNull<PrevailingVisibility>(groupedTokens.GetTokenGroupOrDefault(TokenType.PrevailingVisibility), errors);
            PresentWeather = GetParsedDataArray<WeatherPhenomena>(groupedTokens.GetTokenGroupOrDefault(TokenType.PresentWeather), errors);
            CloudLayers = GetParsedDataArray<CloudLayer>(groupedTokens.GetTokenGroupOrDefault(TokenType.CloudLayer), errors);
            IsNil = groupedTokens.ContainsKey(TokenType.Nil);
            ParseErrors = errors.Count == 0 ? null : errors.ToArray();
            Unrecognized = GetUnrecognizedTokens(groupedTokens.GetTokenGroupOrDefault(TokenType.Unknown));
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Provides parsed data object or null if parse error occur
        /// </summary>
        /// <typeparam name="T">For object type</typeparam>
        /// <param name="groupTokens">Current group of tokens</param>
        /// <param name="errors">List of parse errors</param>
        /// <returns></returns>
        protected T GetDataObjectOrNull<T>(string[] groupTokens, List<string> errors) where T : class
        {
            if (groupTokens.Length == 0)
                return null;

            var previousErrorsCount = errors.Count;
            var data = (T)Activator.CreateInstance(typeof(T), BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { groupTokens, errors }, null);

            return errors.Count - previousErrorsCount > 1
                ? null
                : data;
        }

        /// <summary>
        /// Get parsed data array from tokens array
        /// </summary>
        /// <typeparam name="T">Class name</typeparam>
        /// <param name="tokens">Array of tokens</param>
        /// <param name="errors">Errors list</param>
        /// <returns></returns>
        protected T[] GetParsedDataArray<T>(string[] tokens, List<string> errors) where T : class
        {
            var outcome = tokens.Select(x => GetDataObjectOrNull<T>(new[] { x }, errors)).ToArray();
            return outcome.Length != 0
                ? outcome
                : null;
        }

        /// <summary>
        /// Get parse errors
        /// </summary>
        /// <param name="errors">Errors list</param>
        /// <returns></returns>
        protected string[] GetParseErrors(List<string> errors)
        {
            var errorsListIsEmpty = errors is null or { Count: 0 };
            var parseErrorsIsEmpty = ParseErrors is null or { Length: 0 };

            if (errorsListIsEmpty && parseErrorsIsEmpty)
                return null;

            var outcome = Array.Empty<string>();
            if (!errorsListIsEmpty)
                outcome = outcome.Concat(errors).ToArray();

            if (!parseErrorsIsEmpty)
                outcome = outcome.Concat(ParseErrors).ToArray();

            return outcome;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Get METAR report modifier
        /// </summary>
        /// <param name="groupedTokens">Dictionary of grouped tokens</param>
        /// <returns></returns>
        private MetarModifier GetMetarModifier(Dictionary<TokenType, string[]> groupedTokens)
        {
            var modifierValue = groupedTokens.GetTokenGroupOrDefault(TokenType.Modifier);
            if (modifierValue.Length == 0)
                return MetarModifier.None;

            return modifierValue[0].Equals("AUTO")
                ? MetarModifier.Auto
                : MetarModifier.Cor;
        }

        /// <summary>
        /// Get array of unrecognized tokens
        /// </summary>
        /// <param name="tokens">Array of tokens</param>
        /// <returns></returns>
        private string[] GetUnrecognizedTokens(string[] tokens)
        {
            return tokens is null or { Length: 0 } ? null : tokens;
        }

        #endregion
    }
}
