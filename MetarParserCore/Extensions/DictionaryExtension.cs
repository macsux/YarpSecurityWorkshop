using System;
using System.Collections.Generic;

namespace MetarParserCore.Extensions
{
    /// <summary>
    /// Extension methods for standard dictionary
    /// </summary>
    internal static class DictionaryExtension
    {
        /// <summary>
        /// Get token group from the dictionary by key
        /// </summary>
        /// <typeparam name="TKey">Key type</typeparam>
        /// <param name="dictionary">Current dictionary</param>
        /// <param name="key">Key</param>
        /// <returns></returns>
        public static string[] GetTokenGroupOrDefault<TKey>(this IDictionary<TKey, string[]> dictionary, TKey key)
        {
            return dictionary.TryGetValue(key, out var currentValue) 
                ? currentValue 
                : Array.Empty<string>();
        }
    }
}
