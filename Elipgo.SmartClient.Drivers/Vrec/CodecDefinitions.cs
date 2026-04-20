namespace Elipgo.SmartClient.Drivers.Vrec
{
    public enum VideoCodecType
    {
        /// <summary>
        /// Sin codec.
        /// </summary>
        None,
        /// <summary>
        /// Raw BGR_Packed.
        /// </summary>
        RawBGR,
        /// <summary>
        /// BMP 24bit (RGB)
        /// </summary>
        BMP24,
        /// <summary>
        /// Standard JPEG.
        /// </summary>
        JPEG,
        /// <summary>
        /// Standard JPEG Intel.
        /// </summary>
        IJL,
        /// <summary>
        /// MPEG4 SP ISO compatible.
        /// </summary>
        MPEG4SimpleProfileISO,
        /// <summary>
        /// MPEG4 Vivotek.
        /// </summary>
        MPEG4Vivotek,
        /// <summary>
        /// H.263 (MPEG4 SHM) Vivotek.
        /// </summary>
        H263Vivotek,
        /// <summary>
        /// H264 (MPEG4-10)
        /// </summary>
        H264
    };

    public enum AudioCodecType
    {
        /// <summary>
        /// Sin codec.
        /// </summary>
        None,
    };
}
