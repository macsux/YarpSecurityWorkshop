using System.ComponentModel;

namespace MetarParserCore.Enums
{
    /// <summary>
    /// Enum of weather conditions
    /// </summary>
    public enum WeatherCondition
    {
        [Description("NONE")]
        None = 0,

        #region Intensity or proximity

        [Description("-")]
        Light = 1,

        [Description("+")]
        Heavy = 2,

        [Description("VC")]
        Vicinity = 3,

        #endregion

        #region Descriptor

        [Description("MI")]
        Shallow = 4,

        [Description("BC")]
        Patches = 5,

        [Description("PR")]
        Partial = 6,

        [Description("DR")]
        Drifting = 7,

        [Description("BL")]
        Blowing = 8,

        [Description("SH")]
        Shower = 9,

        [Description("TS")]
        Thunderstorm = 10,

        [Description("FZ")]
        Freezing = 11,

        #endregion

        #region Precipitation

        [Description("DZ")]
        Drizzle = 12,

        [Description("RA")]
        Rain = 13,

        [Description("SN")]
        Snow = 14,

        [Description("SG")]
        SnowGrains = 15,

        [Description("IC")]
        IceCrystals = 16,

        [Description("PL")]
        IcePellets = 17,

        [Description("GR")]
        Hail = 18,

        [Description("GS")]
        SnowPellets = 19,

        [Description("UP")]
        UnknownPrecipitation = 20,

        #endregion

        #region Obscuration

        /// <summary>
        /// Mist - visibility is 1000 meters or more
        /// </summary>
        [Description("BR")]
        Mist = 21,

        /// <summary>
        /// Fog - visibility is less than 1000 meters
        /// </summary>
        [Description("FG")]
        Fog = 22,

        [Description("FU")]
        Smoke = 23,

        [Description("VA")]
        VolcanicAsh = 24,

        [Description("DU")]
        Dust = 25,

        [Description("SA")]
        Sand = 26,

        [Description("HZ")]
        Haze = 27,

        [Description("PY")]
        Spray = 28,

        #endregion

        #region Other

        [Description("PO")]
        DustWhirls = 29,

        [Description("SQ")]
        Squalls = 30,

        [Description("FC")]
        FunnelCloud = 31,

        [Description("SS")]
        SandStorm = 32,

        [Description("DS")]
        DustStorm = 33,

        #endregion

        [Description("NSW")]
        NoSignificantWeather = 34
    }
}
