namespace MetarParserCore.Enums
{
    /// <summary>
    /// METAR report types
    /// </summary>
    public enum ReportType
    {
        NotSpecified = 0,

        /// <summary>
        /// METAR report
        /// </summary>
        Metar = 1,

        /// <summary>
        /// TREND Report
        /// </summary>
        Trend = 2,

        /// <summary>
        /// ToDo: Not supported yet
        /// </summary>
        Speci = 3,

        /// <summary>
        /// ToDo: Not supported yet
        /// </summary>
        Taf = 4
    }
}
