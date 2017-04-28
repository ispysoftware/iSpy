using System;

namespace iSpyApplication.Sources
{    
    /// <summary>
    /// Delegate for new frame event handler.
    /// </summary>
    /// 
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    /// 
    public delegate void NewFrameEventHandler(object sender, NewFrameEventArgs e);

    /// <summary>
    /// Delegate for playing finished event handler.
    /// </summary>
    /// 
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Reason of finishing video playing.</param>
    /// 
    public delegate void PlayingFinishedEventHandler(object sender, PlayingFinishedEventArgs e);

    /// <summary>
    /// Reason of finishing video playing.
    /// </summary>
    /// 
    /// <remarks><para>When video source class fire the <see cref="IVideoSource.PlayingFinished"/> event, they
    /// need to specify reason of finishing video playing. For example, it may be end of stream reached.</para></remarks>
    /// 
    public enum ReasonToFinishPlaying
    {
        /// <summary>
        /// Video playing has finished because it end was reached.
        /// </summary>
        EndOfStreamReached,
        /// <summary>
        /// Video playing has finished because it was stopped by user.
        /// </summary>
        StoppedByUser,
        /// <summary>
        /// Video playing has finished because the device was lost (unplugged).
        /// </summary>
        DeviceLost,
        /// <summary>
        /// Video playing has finished because of some error happened the video source (camera, stream, file, etc.).
        /// A error reporting event usually is fired to provide error information.
        /// </summary>
        VideoSourceError,
        Restart
    }

    /// <summary>
    /// Arguments for new frame event from video source.
    /// </summary>
    /// 
    public class NewFrameEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewFrameEventArgs"/> class.
        /// </summary>
        /// 
        /// <param name="frame">New frame.</param>
        /// 
        public NewFrameEventArgs(System.Drawing.Bitmap frame)
        {
            Frame = frame;
        }

        /// <summary>
        /// New frame from video source.
        /// </summary>
        /// 
        public System.Drawing.Bitmap Frame { get; }
    }

    /// <summary>
    /// Arguments for source error event from source.
    /// </summary>
    /// 
    public class PlayingFinishedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayingFinishedEventArgs"/> class.
        /// </summary>
        /// 
        /// <param name="reason">Reason</param>
        /// 
        public PlayingFinishedEventArgs(ReasonToFinishPlaying reason)
        {
            ReasonToFinishPlaying = reason;
        }

        /// <summary>
        /// Audio source error description.
        /// </summary>
        /// 
        public ReasonToFinishPlaying ReasonToFinishPlaying { get; }
    }

    

    public delegate void AlertEventHandler(object sender, AlertEventArgs e);

    /// <summary>
    /// Arguments for Audio source error event from Audio source.
    /// </summary>
    /// 
    public class AlertEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlertEventArgs"/> class.
        /// </summary>
        /// 
        /// <param name="description">Error description.</param>
        /// 
        public AlertEventArgs(string description)
        {
            Description = description;
        }

        /// <summary>
        /// Audio source error description.
        /// </summary>
        /// 
        public string Description { get; }
    }
}

