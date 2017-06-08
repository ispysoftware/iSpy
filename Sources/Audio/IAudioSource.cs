using NAudio.Wave;

namespace iSpyApplication.Sources.Audio
{

    /// <summary>
    /// Audio source interface.
    /// </summary>
    /// 
    /// <remarks>The interface describes common methods for different type of Audio sources.</remarks>
    /// 
    public interface IAudioSource
    {
       // bool IsAudio { get; }
        /// <summary>
        /// New Packet event.
        /// </summary>
        /// 
        /// <remarks><para>This event is used to notify clients about new available Audio Packet.</para>
        /// 
        /// <para><note>Since Audio source may have multiple clients, each client is responsible for
        /// making a copy (cloning) of the passed Audio Packet, but Audio source is responsible for
        /// disposing its own original copy after notifying of clients.</note></para>
        /// </remarks>
        /// 
        event DataAvailableEventHandler DataAvailable;

        /// <summary>
        /// Audio source error event.
        /// </summary>
        /// 
        /// <remarks>This event is used to notify clients about any type of errors occurred in
        /// Audio source object, for example internal exceptions.</remarks>
        /// 
        //event AudioSourceErrorEventHandler AudioSourceError;

        /// <summary>
        /// Level Changed event.
        /// </summary>
        /// 
        /// <remarks>This event is used to notify clients about level (volume) changes
        /// </remarks>
        /// 
        event LevelChangedEventHandler LevelChanged;

        /// <summary>
        /// Audio playing finished event.
        /// </summary>
        /// 
        /// <remarks><para>This event is used to notify clients that the Audio playing has finished.</para>
        /// </remarks>
        /// 
        event AudioFinishedEventHandler AudioFinished;

        /// <summary>
        /// Gain of the microphone
        /// </summary>
        /// 
        //float Gain { get; set; }

        /// <summary>
        /// Audio source.
        /// </summary>
        /// 
        /// <remarks>The meaning of the property depends on particular Audio source.
        /// Depending on Audio source it may be a file name, URL or any other string
        /// describing the Audio source.</remarks>
        /// 
        string Source { get; }

        /// <summary>
        /// Detected format of the audio source
        /// </summary>
        WaveFormat RecordingFormat { get; }

        /// <summary>
        /// Provider for wave-out (live playback)
        /// </summary>
        BufferedWaveProvider WaveOutProvider { get; set; }

        /// <summary>
        /// Playing back through audio device
        /// </summary>
        /// 
        /// <remarks>Current state of Audio source object - listening (playing back through audio device) or not.</remarks>
        /// 
        bool Listening { get; set;}


        /// <summary>
        /// State of the Audio source.
        /// </summary>
        /// 
        /// <remarks>Current state of Audio source object - running or not.</remarks>
        /// 
        bool IsRunning { get; }

        /// <summary>
        /// Start Audio source.
        /// </summary>
        void Start();

        /// <summary>
        /// Stop Audio source.
        /// </summary>
        /// 
        /// <remarks>Stops Audio source aborting its thread.</remarks>
        /// 
        void Stop();

        void Restart();
    }
}
