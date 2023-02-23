namespace MetarParserCore.Enums
{
    /// <summary>
    /// States of reading raw METAR
    /// </summary>
    internal enum TokenType
    {
        Unknown = 0,

        Airport = 1,

        ObservationDayTime = 2,

        Modifier = 3,

        SurfaceWind = 4,

        PrevailingVisibility = 5,

        RunwayVisualRange = 6,

        PresentWeather = 7,

        CloudLayer = 8,

        Temperature = 9,

        AltimeterSetting = 10,

        RecentWeather = 11,

        WindShear = 12,

        Deneb = 13,

        Motne = 14,

        SeaState = 15,

        Trend = 16,

        Remarks = 17,

        TrendTime = 18,

        MilitaryColorCode = 19,

        Nil = 20,

        ReportType = 99
    }
}
