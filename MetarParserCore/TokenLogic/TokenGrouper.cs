using System;
using System.Collections.Generic;
using System.Linq;
using MetarParserCore.Enums;

namespace MetarParserCore.TokenLogic
{
    /// <summary>
    /// Transforms array of tokens into groups which represented in dictionary
    /// </summary>
    internal sealed class TokenGrouper
    {
        #region Public methods

        /// <summary>
        /// Transform token array into token groups
        /// </summary>
        /// <param name="tokens">Array of recognized tokens</param>
        /// <returns></returns>
        public Dictionary<TokenType, string[]> TransformToGroups(Token[] tokens)
        {
            if (tokens is null or { Length: 0 })
                throw new Exception("Tokens array is undefined or empty");

            var outcomeDictionary = new Dictionary<TokenType, string[]>();
            var currentTokensGroup = new List<string>();
            var lastTokenType = TokenType.ReportType;
            var groupMode = false;

            for (var i = 0; i < tokens.Length; i++)
            {
                var isLastStep = i == tokens.Length - 1;
                var token = tokens[i];

                if (token.Type == TokenType.Motne || isLastStep)
                {
                    if (isLastStep)
                    {
                        if (token.Type == lastTokenType || groupMode)
                            currentTokensGroup.Add(token.Value);
                        else
                        {
                            outcomeDictionary.Add(lastTokenType, currentTokensGroup.ToArray());
                            outcomeDictionary.Add(token.Type, new []{ token.Value });
                            break;
                        }
                    }

                    (currentTokensGroup, lastTokenType) = SaveGroupInDictionary(token, currentTokensGroup, outcomeDictionary, lastTokenType);
                    groupMode = false;
                    continue;
                }

                if (IsGroupableType(token.Type))
                {
                    (currentTokensGroup, lastTokenType) = SaveGroupInDictionary(token, currentTokensGroup, outcomeDictionary, lastTokenType);
                    groupMode = true;
                    continue;
                }

                if (token.Type == lastTokenType || groupMode)
                {
                    currentTokensGroup.Add(token.Value);
                    continue;
                }

                if (currentTokensGroup.Count == 0)
                {
                    lastTokenType = token.Type;
                    currentTokensGroup.Add(token.Value);
                    continue;
                }

                (currentTokensGroup, lastTokenType) = SaveGroupInDictionary(token, currentTokensGroup, outcomeDictionary, lastTokenType);
            }

            return outcomeDictionary;
        }

        /// <summary>
        /// Transform TREND recognized tokens into TREND groups
        /// </summary>
        /// <param name="tokens">Array of recognized tokens</param>
        /// <returns></returns>
        public Dictionary<TokenType, string[]>[] TransformIntoGroupsTrend(Token[] tokens)
        {
            if (tokens is null or { Length: 0 })
                throw new Exception("Tokens array is undefined or empty");

            var outcomeList = new List<Dictionary<TokenType, string[]>>();
            var currentTokensDictionary = new Dictionary<TokenType, string[]>();
            var currentGroup = new List<string>();
            var previousType = TokenType.ReportType;

            for (var i = 0; i < tokens.Length; i++)
            {
                var token = tokens[i];
                if (token.Type == TokenType.Trend)
                {
                    var groupNotEmpty = currentGroup.Count > 0;
                    if (currentTokensDictionary.Count > 0 || groupNotEmpty)
                    {
                        if (groupNotEmpty)
                        {
                            currentTokensDictionary.Add(previousType, currentGroup.ToArray());
                            currentGroup.Clear();
                        }
                        
                        outcomeList.Add(new Dictionary<TokenType, string[]>(currentTokensDictionary));
                        currentTokensDictionary.Clear();
                    }

                    previousType = TokenType.Trend;

                    if (i != tokens.Length - 1)
                        currentTokensDictionary.Add(previousType, new []{ token.Value });
                    else
                        currentGroup.Add(token.Value);

                    SaveTrendLastStep(i, tokens.Length - 1, previousType, currentGroup, currentTokensDictionary, outcomeList);
                    continue;
                }

                if (previousType == token.Type)
                {
                    currentGroup.Add(token.Value);
                    SaveTrendLastStep(i, tokens.Length - 1, previousType, currentGroup, currentTokensDictionary, outcomeList);
                    continue;
                }

                if (currentGroup.Count > 0)
                    currentTokensDictionary.Add(previousType, currentGroup.ToArray());

                currentGroup.Clear();
                currentGroup.Add(token.Value);
                previousType = token.Type;

                SaveTrendLastStep(i, tokens.Length - 1, previousType, currentGroup, currentTokensDictionary, outcomeList);
            }

            return outcomeList.ToArray();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Determines that current type is groupable (has a chain of several tokens after declare)
        /// </summary>
        /// <param name="type">Current token type</param>
        /// <returns></returns>
        private bool IsGroupableType(TokenType type) =>
            type is TokenType.RecentWeather 
                or TokenType.WindShear 
                or TokenType.Trend 
                or TokenType.Remarks;

        /// <summary>
        /// Save current group
        /// </summary>
        /// <param name="token">Current token</param>
        /// <param name="lastTokenType">Last token type</param>
        /// <param name="currentTokensGroup">List of current tokens group</param>
        /// <param name="outcomeDictionary">Result dictionary</param>
        /// <returns>New current tokens group; new last token type</returns>
        private (List<string>, TokenType) SaveGroupInDictionary(Token token, List<string> currentTokensGroup, IDictionary<TokenType, string[]> outcomeDictionary, TokenType lastTokenType)
        {
            if (outcomeDictionary.ContainsKey(lastTokenType))
            {
                var previousGroup = outcomeDictionary[lastTokenType];
                outcomeDictionary.Remove(lastTokenType);
                currentTokensGroup = previousGroup.Concat(currentTokensGroup).ToList();
            }

            outcomeDictionary.Add(lastTokenType, currentTokensGroup.ToArray());
            currentTokensGroup.Clear();
            currentTokensGroup.Add(token.Value);

            return (currentTokensGroup, token.Type);
        }

        /// <summary>
        /// Save TREND token groups if it is last loop iteration
        /// </summary>
        /// <param name="i">Loop index</param>
        /// <param name="lastIdx">Last index of the loop</param>
        /// <param name="previousType">Previous token type</param>
        /// <param name="currentGroup">Current group list (list of tokens with the same type)</param>
        /// <param name="currentTokensDictionary">Current TREND group</param>
        /// <param name="outcomeList">List of TREND groups</param>
        private void SaveTrendLastStep(
            int i,
            int lastIdx,
            TokenType previousType,
            List<string> currentGroup,
            Dictionary<TokenType, string[]> currentTokensDictionary,
            List<Dictionary<TokenType, string[]>> outcomeList)
        {
            if (i != lastIdx)
                return;

            currentTokensDictionary.Add(previousType, currentGroup.ToArray());
            outcomeList.Add(currentTokensDictionary);
        }

        #endregion
    }
}
