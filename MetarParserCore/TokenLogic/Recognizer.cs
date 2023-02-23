using System.Collections.Generic;
using MetarParserCore.Enums;

namespace MetarParserCore.TokenLogic
{
    /// <summary>
    /// Token recognizer and grouper class
    /// </summary>
    internal sealed class Recognizer
    {
        #region Private fields

        private static TokenRecognizer _tokenRecognizer;

        private static TokenGrouper _tokenGrouper;

        private static Recognizer _instance;

        #endregion

        #region Instance logic

        public Recognizer()
        {
            _tokenRecognizer = new TokenRecognizer();
            _tokenGrouper = new TokenGrouper();
        }

        /// <summary>
        /// Get instance of the Recognizer
        /// </summary>
        /// <returns></returns>
        public static Recognizer Instance()
        {
            return _instance ??= new Recognizer();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Recognize all tokens array and transform them into groups
        /// </summary>
        /// <param name="rawTokens">Unrecognized tokens array</param>
        /// <returns>Dictionary with groups</returns>
        public Dictionary<TokenType, string[]> RecognizeAndGroupTokens(string[] rawTokens)
        {
            var tokens = _tokenRecognizer.RecognizeTokens(rawTokens);
            return _tokenGrouper.TransformToGroups(tokens);
        }

        /// <summary>
        /// Recognize all tokens array and transform them into TREND groups
        /// </summary>
        /// <param name="rawTokens">Unrecognized tokens array</param>
        /// <returns>Array of dictionaries with groups</returns>
        public Dictionary<TokenType, string[]>[] RecognizeAndGroupTokensTrend(string[] rawTokens)
        {
            var tokens = _tokenRecognizer.RecognizeTokens(rawTokens, true);
            return _tokenGrouper.TransformIntoGroupsTrend(tokens);
        }

        #endregion

    }
}
