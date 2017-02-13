namespace iSpyApplication.Sources.Video.Ximea
{
    /// <summary>
    /// XIMEA camera's GPO port modes.
    /// </summary>
    public enum GpoMode
    {
        /// <summary>
        /// Output off.
        /// </summary>
        Off = 0,

        /// <summary>
        /// Logical level.
        /// </summary>
        On = 1,

        /// <summary>
        /// High during exposure (integration) time + readout time + data transfer time.
        /// </summary>
        FrameActive = 2,

        /// <summary>
        /// Low during exposure (integration) time + readout time + data trasnfer time. 
        /// </summary>
        FrameActiveNew = 3,

        /// <summary>
        /// High during exposure(integration) time.
        /// </summary>
        ExposureActive = 4,

        /// <summary>
        /// Low during exposure(integration) time.
        /// </summary>
        ExposureActiveNeg = 5
    }
}
