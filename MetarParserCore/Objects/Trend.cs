using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using MetarParserCore.Enums;
using MetarParserCore.Extensions;
using MetarParserCore.Objects.Supplements;

namespace MetarParserCore.Objects
{
    /// <summary>
    /// Information about changes of weather forecast (TREND)
    /// </summary>
    [DataContract(Name = "trend")]
    public class Trend : ReportBase
    {
        /// <summary>
        /// TREND report type
        /// </summary>
        [DataMember(Name = "trendType", EmitDefaultValue = false)]
        public TrendType TrendType { get; init; }

        /// <summary>
        /// TREND time
        /// </summary>
        [DataMember(Name = "trendTime", EmitDefaultValue = false)]
        public TrendTime TrendTime { get; init; }

        /// <summary>
        /// Military airfield weather (represents in color codes)
        /// </summary>
        [DataMember(Name = "militaryWeather", EmitDefaultValue = false)]
        public MilitaryWeather MilitaryWeather { get; init; }

        #region Constructors

        /// <summary>
        /// Default
        /// </summary>
        public Trend() { }

        /// <summary>
        /// Parser constructor
        /// </summary>
        /// <param name="groupedTokens">Dictionary of grouped tokens</param>
        /// <param name="currentMonth">Current month</param>
        internal Trend(Dictionary<TokenType, string[]> groupedTokens, Month currentMonth)
            : base(groupedTokens, currentMonth)
        {
            var errors = new List<string>();

            ReportType = ReportType.Trend;
            TrendType = GetTrendType(groupedTokens.GetTokenGroupOrDefault(TokenType.Trend).FirstOrDefault());
            TrendTime = GetTrendTime(groupedTokens.GetTokenGroupOrDefault(TokenType.TrendTime), errors);
            MilitaryWeather = GetDataObjectOrNull<MilitaryWeather>(groupedTokens.GetTokenGroupOrDefault(TokenType.MilitaryColorCode), errors);
            ParseErrors = GetParseErrors(errors);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Get current trend type
        /// </summary>
        /// <param name="trendToken">String trend type</param>
        /// <returns></returns>
        private TrendType GetTrendType(string trendToken)
        {
            return EnumTranslator.GetValueByDescription<TrendType>(trendToken);
        }

        /// <summary>
        /// Get TREND time
        /// </summary>
        /// <param name="tokens">Time tokens</param>
        /// <param name="errors">Errors list</param>
        /// <returns></returns>
        private TrendTime GetTrendTime(string[] tokens, List<string> errors)
        {
            var trendTime = new TrendTime(tokens, errors);
            var lastErrorsCount = errors.Count;
            if (trendTime.AtTime == null && trendTime.FromTime == null && trendTime.TillTime == null
                || lastErrorsCount < errors.Count)
                return null;

            return trendTime;
        }
        #endregion
    }
}
