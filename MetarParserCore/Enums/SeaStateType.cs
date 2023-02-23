namespace MetarParserCore.Enums
{
    /// <summary>
    /// Types about the sea state
    /// </summary>
    public enum SeaStateType
    {
        /// <summary>
        /// Waves height = 0 meters
        /// </summary>
        Glassy = 0,

        /// <summary>
        /// Waves height from 0 to 0.1 meters
        /// </summary>
        Rippled = 1,

        /// <summary>
        /// Waves height from 0.1 to 0.5 meters
        /// </summary>
        Wavelets = 2,

        /// <summary>
        /// Waves height from 0.5 to 1.25 meters
        /// </summary>
        Slight = 3,

        /// <summary>
        /// Waves height from 1.25 to 2.5 meters
        /// </summary>
        Moderate = 4,

        /// <summary>
        /// Waves height from 2.5 to 4 meters
        /// </summary>
        Rough = 5,

        /// <summary>
        /// Waves height from 4 to 6 meters
        /// </summary>
        VeryRough = 6,

        /// <summary>
        /// Wave height from 6 to 9 meters
        /// </summary>
        High = 7,

        /// <summary>
        /// Wave height from 9 to 14 meters
        /// </summary>
        VeryHigh = 8,

        /// <summary>
        /// Wave height over 14 meters
        /// </summary>
        Phenomenal = 9,

        /// <summary>
        /// Not reported
        /// </summary>
        None = 10
    }
}
