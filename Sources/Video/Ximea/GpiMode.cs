namespace iSpyApplication.Sources.Video.Ximea
{
    /// <summary>
    /// XIMEA camera's GPI port modes.
    /// </summary>
    public enum GpiMode
    {
        /// <summary>
        /// Input is off.
        /// </summary>
        Off = 0,

        /// <summary>
        /// Trigger input.
        /// </summary>
        Trigger = 1,

        /// <summary>
        /// External signal input.
        /// </summary>
        ExternalEvent = 2
    }
}
