namespace iSpyApplication.Sources.Video.Ximea
{
    /// <summary>
    /// Enumeration of image formats supported by XIMEA cameras.
    /// </summary>
    public enum ImageFormat : int
    {
        /// <summary>
        /// 8 bits per pixel.
        /// </summary>
        Grayscale8 = 0,

        /// <summary>
        /// 16 bits per pixel.
        /// </summary>
        Grayscale16 = 1,

        /// <summary>
        /// RGB data format.
        /// </summary>
        RGB24 = 2,

        /// <summary>
        /// RGBA data format.
        /// </summary>
        RGB32 = 3
    }
}
