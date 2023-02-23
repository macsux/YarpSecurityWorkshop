namespace MetarParserCore.Enums
{
    /// <summary>
    /// Extent of contamination types
    /// </summary>
    public enum MotneExtentOfContamination
    {
        /// <summary>
        /// Not reported, marked as "/"
        /// </summary>
        NotReported = 0,

        /// <summary>
        /// 10% or less of runway covered
        /// </summary>
        Less10 = 1,

        /// <summary>
        /// 11% to 25% of runway covered
        /// </summary>
        From11To25 = 2,

        /// <summary>
        /// 26% to 50% of runway covered
        /// </summary>
        From26To50 = 5,

        /// <summary>
        /// 51% to 100% of runway covered
        /// </summary>
        From51To100 = 9
    }
}
