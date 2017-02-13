using System;

namespace iSpyApplication.Sources.Audio.talk
{

    /// <summary>
    /// Audio source interface.
    /// </summary>
    /// 
    /// <remarks>The interface describes common methods for different type of Audio sources.</remarks>
    /// 
    public interface ITalkTarget
    {
        /// <summary>
        /// Start Talking
        /// </summary>
        /// 
        void Start();

        /// <summary>
        /// Stop Talking
        /// </summary>
        void Stop();

        bool Connected { get; }

        event TalkStoppedEventHandler TalkStopped;

    }

    public delegate void TalkStoppedEventHandler(object sender, EventArgs e);
}
