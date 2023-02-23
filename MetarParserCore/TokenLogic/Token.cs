using MetarParserCore.Enums;

namespace MetarParserCore.TokenLogic
{
    /// <summary>
    /// Detected token
    /// </summary>
    internal class Token
    {
        /// <summary>
        /// Token type
        /// </summary>
        public TokenType Type { get; init; }

        /// <summary>
        /// Content of the token
        /// </summary>
        public string Value { get; set; }

        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}
