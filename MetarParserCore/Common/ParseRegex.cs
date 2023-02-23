namespace MetarParserCore.Common
{
    /// <summary>
    /// Class of regex patterns for tokens parsing
    /// </summary>
    public class ParseRegex
    {
        #region Prevailing visibility

        public static string VisibilityWholeNumber => @"^[1-2]{1}$";

        public static string StatuteMilesVisibility => @"^M?(\d{1}\/\d{1,2}|\d{1,2})SM$";

        public static string MetersVisibility => @"^\d{4}(N|S)?(E|W)?$";

        public static string MetersVisibilityContainsDirections = @"^\d{4}(N|S|E|W){1,2}$";

        #endregion
    }
}
