namespace iSpyApplication.Sources.Video.Ximea
{  
    /// <summary>
    /// Enumeration of camera's trigger modes.
    /// </summary>
    public enum TriggerSource
    {
        /// <summary>
        /// Camera works in free run mode.
        /// </summary>
        Off = 0,

        /// <summary>
        /// External trigger (rising edge).
        /// </summary>
        EdgeRising = 1,

        /// <summary>
        /// External trigger (falling edge).
        /// </summary>
        EdgeFalling = 2,

        /// <summary>
        /// Software (manual) trigger.
        /// </summary>
        Software = 3
    }
}
