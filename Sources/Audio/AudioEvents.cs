using System;

namespace iSpyApplication.Sources.Audio
{
    /// <summary>
    /// Delegate for new frame event handler.
    /// </summary>
    /// 
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    /// 
    public delegate void DataAvailableEventHandler(object sender, DataAvailableEventArgs e);


    public delegate void LevelChangedEventHandler(object sender, LevelChangedEventArgs e);
    
    /// <summary>
    /// Delegate for Audio source error event handler.
    /// </summary>
    /// 
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    /// 
    public delegate void AudioSourceErrorEventHandler(object sender, AudioSourceErrorEventArgs e);

    /// <summary>
    /// Delegate for playing finished event handler.
    /// </summary>
    /// 
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Reason of finishing Audio playing.</param>
    /// 
    public delegate void AudioFinishedEventHandler(object sender, PlayingFinishedEventArgs e);
    public delegate void TimeUpdateEventHandler(object sender, TimeUpdateEventArgs e);
    /// <summary>
    /// Delegate for notifying about audio stream
    /// </summary>
    /// 
    /// <param name="sender">Sender object.</param>
    /// <param name="e"></param>
    public delegate void HasAudioStreamEventHandler(object sender, EventArgs e);


    public class TimeUpdateEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeUpdateEventArgs"/> class.
        /// </summary>
        /// 
        /// <param name="reason">Reason</param>
        /// 
        public TimeUpdateEventArgs(long time)
        {
            Time = time;
        }

        /// <summary>
        /// Audio source error description.
        /// </summary>
        /// 
        public long Time { get; }
    }
    /// <summary>
    /// Arguments for new frame event from Audio source.
    /// </summary>
    /// 
    public class DataAvailableEventArgs : EventArgs
    {
        public int BytesRecorded;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAvailableEventArgs"/> class.
        /// </summary>
        public DataAvailableEventArgs(byte[] raw)
        {
            RawData = raw;
            BytesRecorded = raw.Length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAvailableEventArgs"/> class.
        /// </summary>
        public DataAvailableEventArgs(byte[] raw, int length)
        {
            RawData = raw;
            BytesRecorded = length;
        }

        /// <summary>
        /// New frame from Audio source.
        /// </summary>
        /// 
        public byte[] RawData { get; set; }
    }

    public class LevelChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataAvailableEventArgs"/> class.
        /// </summary>
        public LevelChangedEventArgs(float[] maxsamples)
        {
            MaxSamples = maxsamples;
        }

        /// <summary>
        /// New frame from Audio source.
        /// </summary>
        /// 
        public float[] MaxSamples { get; }
    }

    /// <summary>
    /// Arguments for Audio source error event from Audio source.
    /// </summary>
    /// 
    public class AudioSourceErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AudioSourceErrorEventArgs"/> class.
        /// </summary>
        /// 
        /// <param name="description">Error description.</param>
        /// 
        public AudioSourceErrorEventArgs(string description)
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
