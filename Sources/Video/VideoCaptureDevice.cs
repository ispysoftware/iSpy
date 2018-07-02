using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using iSpyApplication.Controls;
using iSpyApplication.Utilities;
using iSpyPRO.DirectShow;
using iSpyPRO.DirectShow.Internals;
using FilterInfo = iSpyPRO.DirectShow.FilterInfo;

namespace iSpyApplication.Sources.Video
{
    using FilterInfo = FilterInfo;

    /// <summary>
    /// Video source for local video capture device (for example USB webcam).
    /// </summary>
    /// 
    /// <remarks><para>This video source class captures video data from local video capture device,
    /// like USB web camera (or internal), frame grabber, capture board - anything which
    /// supports <b>DirectShow</b> interface. For devices which has a shutter button or
    /// support external software triggering, the class also allows to do snapshots. Both
    /// video size and snapshot size can be configured.</para>
    /// 
    /// <para>Sample usage:</para>
    /// <code>
    /// // enumerate video devices
    /// videoDevices = new FilterInfoCollection( FilterCategory.VideoInputDevice );
    /// // create video source
    /// VideoCaptureDevice videoSource = new VideoCaptureDevice( videoDevices[0].MonikerString );
    /// // set NewFrame event handler
    /// videoSource.NewFrame += new NewFrameEventHandler( video_NewFrame );
    /// // start the video source
    /// videoSource.Start( );
    /// // ...
    /// // signal to stop when you no longer need capturing
    /// videoSource.SignalToStop( );
    /// // ...
    /// 
    /// private void video_NewFrame( object sender, NewFrameEventArgs eventArgs )
    /// {
    ///     // get new frame
    ///     Bitmap bitmap = eventArgs.Frame;
    ///     // process the frame
    /// }
    /// </code>
    /// </remarks>
    /// 
    internal class VideoCaptureDevice : VideoBase, IVideoSource
    {
        // moniker string of video capture device
        private string _deviceMoniker;

        // video and snapshot resolutions to set
        private VideoCapabilities _videoResolution;
        private VideoCapabilities _snapshotResolution;

        // provide snapshots or not
        private bool _provideSnapshots;
        private ManualResetEvent _abort;
        private ReasonToFinishPlaying _res = ReasonToFinishPlaying.DeviceLost;

        private Thread _thread;

        private VideoCapabilities[] _videoCapabilities;
        private VideoCapabilities[] _snapshotCapabilities;

        private bool _needToSetVideoInput;
        private bool _needToSimulateTrigger;
        private bool _needToDisplayPropertyPage;
        private bool _needToDisplayCrossBarPropertyPage;
        private IntPtr _parentWindowForPropertyPage = IntPtr.Zero;

        // video capture source object
        private object _sourceObject;

        // time of starting the DirectX graph
        private DateTime _startTime;

        // dummy object to lock for synchronization
        private readonly object _sync = new object();

        // flag specifying if IAMCrossbar interface is supported by the running graph/source object
        private bool? _isCrossbarAvailable;
        private DateTime _lastFrame;

        private VideoInput[] _crossbarVideoInputs;
        private VideoInput _crossbarVideoInput = VideoInput.Default;

        // cache for video/snapshot capabilities and video inputs
        private static readonly Dictionary<string, VideoCapabilities[]> CacheVideoCapabilities = new Dictionary<string, VideoCapabilities[]>();
        private static readonly Dictionary<string, VideoCapabilities[]> CacheSnapshotCapabilities = new Dictionary<string, VideoCapabilities[]>();
        private static readonly Dictionary<string, VideoInput[]> CacheCrossbarVideoInputs = new Dictionary<string, VideoInput[]>();

        /// <summary>
        /// Current video input of capture card.
        /// </summary>
        /// 
        /// <remarks><para>The property specifies video input to use for video devices like capture cards
        /// (those which provide crossbar configuration). List of available video inputs can be obtained
        /// from <see cref="AvailableCrossbarVideoInputs"/> property.</para>
        /// 
        /// <para>To check if the video device supports crossbar configuration, the <see cref="CheckIfCrossbarAvailable"/>
        /// method can be used.</para>
        /// 
        /// <para><note>This property can be set as before running video device, as while running it.</note></para>
        /// 
        /// <para>By default this property is set to <see cref="VideoInput.Default"/>, which means video input
        /// will not be set when running video device, but currently configured will be used. After video device
        /// is started this property will be updated anyway to tell current video input.</para>
        /// </remarks>
        /// 
        public VideoInput CrossbarVideoInput
        {
            get { return _crossbarVideoInput; }
            set
            {
                _needToSetVideoInput = true;
                _crossbarVideoInput = value;
            }
        }

        /// <summary>
        /// Available inputs of the video capture card.
        /// </summary>
        /// 
        /// <remarks><para>The property provides list of video inputs for devices like video capture cards.
        /// Such devices usually provide several video inputs, which can be selected using crossbar.
        /// If video device represented by the object of this class supports crossbar, then this property
        /// will list all video inputs. However if it is a regular USB camera, for example, which does not
        /// provide crossbar configuration, the property will provide zero length array.</para>
        /// 
        /// <para>Video input to be used can be selected using <see cref="CrossbarVideoInput"/>. See also
        /// <see cref="DisplayCrossbarPropertyPage"/> method, which provides crossbar configuration dialog.</para>
        /// 
        /// <para><note>It is recomended not to call this property immediately after <see cref="Start"/> method, since
        /// device may not start yet and provide its information. It is better to call the property
        /// before starting device or a bit after (but not immediately after).</note></para>
        /// </remarks>
        /// 
        public VideoInput[] AvailableCrossbarVideoInputs
        {
            get
            {
                if (_crossbarVideoInputs == null)
                {
                    lock (CacheCrossbarVideoInputs)
                    {
                        if ((!string.IsNullOrEmpty(_deviceMoniker)) && (CacheCrossbarVideoInputs.ContainsKey(_deviceMoniker)))
                        {
                            _crossbarVideoInputs = CacheCrossbarVideoInputs[_deviceMoniker];
                        }
                    }

                    if (_crossbarVideoInputs == null)
                    {
                        if (!IsRunning)
                        {
                            // create graph without playing to collect available inputs
                            WorkerThread(false);
                        }
                        else
                        {
                            for (int i = 0; (i < 500) && (_crossbarVideoInputs == null); i++)
                            {
                                Thread.Sleep(10);
                            }
                        }
                    }
                }
                // don't return null even if capabilities are not provided for some reason
                return _crossbarVideoInputs ?? new VideoInput[0];
            }
        }

        /// <summary>
        /// Specifies if snapshots should be provided or not.
        /// </summary>
        /// 
        /// <remarks><para>Some USB cameras/devices may have a shutter button, which may result into snapshot if it
        /// is pressed. So the property specifies if the video source will try providing snapshots or not - it will
        /// check if the camera supports providing still image snapshots. If camera supports snapshots and the property
        /// is set to <see langword="true"/>, then snapshots will be provided through <see cref="SnapshotFrame"/>
        /// event.</para>
        /// 
        /// <para>Check supported sizes of snapshots using <see cref="SnapshotCapabilities"/> property and set the
        /// desired size using <see cref="SnapshotResolution"/> property.</para>
        /// 
        /// <para><note>The property must be set before running the video source to take effect.</note></para>
        /// 
        /// <para>Default value of the property is set to <see langword="false"/>.</para>
        /// </remarks>
        ///
        public bool ProvideSnapshots
        {
            get { return _provideSnapshots; }
            set { _provideSnapshots = value; }
        }

        /// <summary>
        /// New frame event.
        /// </summary>
        /// 
        /// <remarks><para>Notifies clients about new available frame from video source.</para>
        /// 
        /// <para><note>Since video source may have multiple clients, each client is responsible for
        /// making a copy (cloning) of the passed video frame, because the video source disposes its
        /// own original copy after notifying of clients.</note></para>
        /// </remarks>
        /// 
        public event NewFrameEventHandler NewFrame;

        /// <summary>
        /// Snapshot frame event.
        /// </summary>
        /// 
        /// <remarks><para>Notifies clients about new available snapshot frame - the one which comes when
        /// camera's snapshot/shutter button is pressed.</para>
        /// 
        /// <para>See documentation to <see cref="ProvideSnapshots"/> for additional information.</para>
        /// 
        /// <para><note>Since video source may have multiple clients, each client is responsible for
        /// making a copy (cloning) of the passed snapshot frame, because the video source disposes its
        /// own original copy after notifying of clients.</note></para>
        /// </remarks>
        /// 
        /// <seealso cref="ProvideSnapshots"/>
        /// 
        public event NewFrameEventHandler SnapshotFrame;

        /// <summary>
        /// Video playing finished event.
        /// </summary>
        /// 
        /// <remarks><para>This event is used to notify clients that the video playing has finished.</para>
        /// </remarks>
        /// 
        public event PlayingFinishedEventHandler PlayingFinished;

        /// <summary>
        /// Video source.
        /// </summary>
        /// 
        /// <remarks>Video source is represented by moniker string of video capture device.</remarks>
        /// 
        public virtual string Source
        {
            get { return _deviceMoniker; }
            set
            {
                _deviceMoniker = value;

                _videoCapabilities = null;
                _snapshotCapabilities = null;
                _crossbarVideoInputs = null;
                _isCrossbarAvailable = null;
            }
        }

        /// <summary>
        /// State of the video source.
        /// </summary>
        /// 
        /// <remarks>Current state of video source object - running or not.</remarks>
        /// 
        public bool IsRunning => _thread != null && !_thread.Join(0);

        /// <summary>
        /// Obsolete - no longer in use
        /// </summary>
        /// 
        /// <remarks><para>The property is obsolete. Use <see cref="VideoResolution"/> property instead.
        /// Setting this property does not have any effect.</para></remarks>
        /// 
        [Obsolete]
        public Size DesiredFrameSize => Size.Empty;

        /// <summary>
        /// Obsolete - no longer in use
        /// </summary>
        /// 
        /// <remarks><para>The property is obsolete. Use <see cref="SnapshotResolution"/> property instead.
        /// Setting this property does not have any effect.</para></remarks>
        /// 
        [Obsolete]
        public Size DesiredSnapshotSize => Size.Empty;

        /// <summary>
        /// Obsolete - no longer in use.
        /// </summary>
        /// 
        /// <remarks><para>The property is obsolete. Setting this property does not have any effect.</para></remarks>
        /// 
        [Obsolete]
        public int DesiredFrameRate => 0;

        /// <summary>
        /// Video resolution to set.
        /// </summary>
        /// 
        /// <remarks><para>The property allows to set one of the video resolutions supported by the camera.
        /// Use <see cref="VideoCapabilities"/> property to get the list of supported video resolutions.</para>
        /// 
        /// <para><note>The property must be set before camera is started to make any effect.</note></para>
        /// 
        /// <para>Default value of the property is set to <see langword="null"/>, which means default video
        /// resolution is used.</para>
        /// </remarks>
        /// 
        public VideoCapabilities VideoResolution
        {
            get { return _videoResolution; }
            set { _videoResolution = value; }
        }

        /// <summary>
        /// Snapshot resolution to set.
        /// </summary>
        /// 
        /// <remarks><para>The property allows to set one of the snapshot resolutions supported by the camera.
        /// Use <see cref="SnapshotCapabilities"/> property to get the list of supported snapshot resolutions.</para>
        /// 
        /// <para><note>The property must be set before camera is started to make any effect.</note></para>
        /// 
        /// <para>Default value of the property is set to <see langword="null"/>, which means default snapshot
        /// resolution is used.</para>
        /// </remarks>
        /// 
        public VideoCapabilities SnapshotResolution
        {
            get { return _snapshotResolution; }
            set { _snapshotResolution = value; }
        }

        /// <summary>
        /// Video capabilities of the device.
        /// </summary>
        /// 
        /// <remarks><para>The property provides list of device's video capabilities.</para>
        /// 
        /// <para><note>It is recomended not to call this property immediately after <see cref="Start"/> method, since
        /// device may not start yet and provide its information. It is better to call the property
        /// before starting device or a bit after (but not immediately after).</note></para>
        /// </remarks>
        /// 
        public VideoCapabilities[] VideoCapabilities
        {
            get
            {
                if (_videoCapabilities == null)
                {
                    lock (CacheVideoCapabilities)
                    {
                        if ((!string.IsNullOrEmpty(_deviceMoniker)) && (CacheVideoCapabilities.ContainsKey(_deviceMoniker)))
                        {
                            _videoCapabilities = CacheVideoCapabilities[_deviceMoniker];
                        }
                    }

                    if (_videoCapabilities == null)
                    {
                        if (!IsRunning)
                        {
                            // create graph without playing to get the video/snapshot capabilities only.
                            // not very clean but it works
                            WorkerThread(false);
                        }
                        else
                        {
                            for (int i = 0; (i < 300) && (_videoCapabilities == null); i++)
                            {
                                Thread.Sleep(10);
                            }
                        }
                    }
                }
                // don't return null even capabilities are not provided for some reason
                return _videoCapabilities ?? new VideoCapabilities[0];
            }
        }



        /// <summary>
        /// Snapshot capabilities of the device.
        /// </summary>
        /// 
        /// <remarks><para>The property provides list of device's snapshot capabilities.</para>
        /// 
        /// <para>If the array has zero length, then it means that this device does not support making
        /// snapshots.</para>
        /// 
        /// <para>See documentation to <see cref="ProvideSnapshots"/> for additional information.</para>
        /// 
        /// <para><note>It is recomended not to call this property immediately after <see cref="Start"/> method, since
        /// device may not start yet and provide its information. It is better to call the property
        /// before starting device or a bit after (but not immediately after).</note></para>
        /// </remarks>
        /// 
        /// <seealso cref="ProvideSnapshots"/>
        /// 
        public VideoCapabilities[] SnapshotCapabilities
        {
            get
            {
                if (_snapshotCapabilities == null)
                {
                    lock (CacheSnapshotCapabilities)
                    {
                        if ((!string.IsNullOrEmpty(_deviceMoniker)) && (CacheSnapshotCapabilities.ContainsKey(_deviceMoniker)))
                        {
                            _snapshotCapabilities = CacheSnapshotCapabilities[_deviceMoniker];
                        }
                    }

                    if (_snapshotCapabilities == null)
                    {
                        if (!IsRunning)
                        {
                            // create graph without playing to get the video/snapshot capabilities only.
                            // not very clean but it works
                            WorkerThread(false);
                        }
                        else
                        {
                            for (int i = 0; (i < 500) && (_snapshotCapabilities == null); i++)
                            {
                                Thread.Sleep(10);
                            }
                        }
                    }
                }
                // don't return null even capabilities are not provided for some reason
                return _snapshotCapabilities ?? new VideoCapabilities[0];
            }
        }

        /// <summary>
        /// Source COM object of camera capture device.
        /// </summary>
        /// 
        /// <remarks><para>The source COM object of camera capture device is exposed for the
        /// case when user may need get direct access to the object for making some custom
        /// configuration of camera through DirectShow interface, for example.
        /// </para>
        /// 
        /// <para>If camera is not running, the property is set to <see langword="null"/>.</para>
        /// </remarks>
        /// 
        public object SourceObject => _sourceObject;

        public VideoCaptureDevice(string source) : base(null)
        {
            _deviceMoniker = source;
        }

        public VideoCaptureDevice(CameraWindow source): base(source)
        {
            var camobject = source.Camobject;
            _deviceMoniker = camobject.settings.videosourcestring;
            string[] wh = camobject.resolution.Split('x');
            var sz = new Size(Convert.ToInt32(wh[0]), Convert.ToInt32(wh[1]));


            string precfg = source.Nv("video");
            bool found = false;

            if (source.Nv("capturemode") != "snapshots")
            {
                VideoCapabilities[] videoCapabilities = VideoCapabilities;
                ProvideSnapshots = false;
                foreach (VideoCapabilities capabilty in videoCapabilities)
                {

                    string item = string.Format(VideoSource.VideoFormatString, capabilty.FrameSize.Width,
                        Math.Abs(capabilty.FrameSize.Height), capabilty.AverageFrameRate, capabilty.BitCount);
                    if (precfg == item)
                    {
                        VideoResolution = capabilty;
                        found = true;
                        break;
                    }
                }
            }
            else
            {
                precfg = source.Nv("snapshots");
                ProvideSnapshots = true;
                VideoCapabilities[] videoCapabilities = SnapshotCapabilities;
                foreach (VideoCapabilities capabilty in videoCapabilities)
                {

                    string item = string.Format(VideoSource.SnapshotFormatString, capabilty.FrameSize.Width,
                        Math.Abs(capabilty.FrameSize.Height), capabilty.AverageFrameRate, capabilty.BitCount);
                    if (precfg == item)
                    {
                        VideoResolution = capabilty;
                        found = true;
                        break;
                    }
                }
            }
            if (!found)
            {
                var vc = VideoCapabilities.Where(p => p.FrameSize == sz).ToList();
                if (vc.Count > 0)
                {
                    var vc2 = vc.FirstOrDefault(p => p.AverageFrameRate == camobject.settings.framerate) ??
                                vc.FirstOrDefault();
                    VideoResolution = vc2;
                    found = true;
                }
                if (!found)
                {
                    //first available
                    var vcf = VideoCapabilities.FirstOrDefault();
                    if (vcf != null)
                        VideoResolution = vcf;
                    //else
                    //{
                    //    dont do this, not having an entry is ok for some video providers
                    //    throw new Exception("Unable to find a video format for the capture device");
                    //}
                }
            }

            if (camobject.settings.crossbarindex != -1 && CheckIfCrossbarAvailable())
            {
                var cbi =
                    AvailableCrossbarVideoInputs.FirstOrDefault(
                        p => p.Index == camobject.settings.crossbarindex);
                if (cbi != null)
                {
                    CrossbarVideoInput = cbi;
                }
            }
        }

        /// <summary>
        /// Start video source.
        /// </summary>
        /// 
        /// <remarks>Starts video source and return execution to caller. Video source
        /// object creates background thread and notifies about new frames with the
        /// help of <see cref="NewFrame"/> event.</remarks>
        /// 
        public void Start()
        {
            if (!IsRunning)
            {
                // check source
                if (string.IsNullOrEmpty(_deviceMoniker))
                    throw new ArgumentException("Video source is not specified.");
                
                _isCrossbarAvailable = null;
                _needToSetVideoInput = true;

                // create events
                _res = ReasonToFinishPlaying.DeviceLost;

                _thread = new Thread(WorkerThread) { Name = _deviceMoniker, IsBackground = true };
                _thread.TrySetApartmentState(ApartmentState.STA);
                _thread.Start();

            }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                _res = ReasonToFinishPlaying.StoppedByUser;
                _abort?.Set();
            }
            else
            {
                _res = ReasonToFinishPlaying.StoppedByUser;
                PlayingFinished?.Invoke(this, new PlayingFinishedEventArgs(_res));
            }
        }

        public void Restart()
        {
            if (!IsRunning) return;
            _res = ReasonToFinishPlaying.Restart;
            _abort?.Set();
        }

        /// <summary>
        /// Display property window for the video capture device providing its configuration
        /// capabilities.
        /// </summary>
        /// 
        /// <param name="parentWindow">Handle of parent window.</param>
        /// 
        /// <remarks><para><note>If you pass parent window's handle to this method, then the
        /// displayed property page will become modal window and none of the controls from the
        /// parent window will be accessible. In order to make it modeless it is required
        /// to pass <see cref="IntPtr.Zero"/> as parent window's handle.
        /// </note></para>
        /// </remarks>
        /// 
        /// <exception cref="NotSupportedException">The video source does not support configuration property page.</exception>
        /// 
        public void DisplayPropertyPage(IntPtr parentWindow)
        {
            // check source
            if (string.IsNullOrEmpty(_deviceMoniker))
                throw new ArgumentException("Video source is not specified.");

            lock (_sync)
            {
                if (IsRunning)
                {
                    // pass the request to backgroud thread if video source is running
                    _parentWindowForPropertyPage = parentWindow;
                    _needToDisplayPropertyPage = true;
                    return;
                }

                object tempSourceObject;

                // create source device's object
                try
                {
                    tempSourceObject = FilterInfo.CreateFilter(_deviceMoniker);
                    if (tempSourceObject == null)
                        throw new Exception("source is null");
                }
                catch
                {
                    throw new ApplicationException("Failed creating device object for moniker.");
                }

                if (!(tempSourceObject is ISpecifyPropertyPages))
                {
                    throw new NotSupportedException("The video source does not support configuration property page.");
                }

                DisplayPropertyPage(parentWindow, tempSourceObject);

                Marshal.FinalReleaseComObject(tempSourceObject);
            }
        }

        /// <summary>
        /// Display property page of video crossbar (Analog Video Crossbar filter).
        /// </summary>
        /// 
        /// <param name="parentWindow">Handle of parent window.</param>
        /// 
        /// <remarks><para>The Analog Video Crossbar filter is modeled after a general switching matrix,
        /// with n inputs and m outputs. For example, a video card might have two external connectors:
        /// a coaxial connector for TV, and an S-video input. These would be represented as input pins on
        /// the filter. The displayed property page allows to configure the crossbar by selecting input
        /// of a video card to use.</para>
        /// 
        /// <para><note>This method can be invoked only when video source is running (<see cref="IsRunning"/> is
        /// <see langword="true"/>). Otherwise it generates exception.</note></para>
        /// 
        /// <para>Use <see cref="CheckIfCrossbarAvailable"/> method to check if running video source provides
        /// crossbar configuration.</para>
        /// </remarks>
        /// 
        /// <exception cref="ApplicationException">The video source must be running in order to display crossbar property page.</exception>
        /// <exception cref="NotSupportedException">Crossbar configuration is not supported by currently running video source.</exception>
        /// 
        public void DisplayCrossbarPropertyPage(IntPtr parentWindow)
        {
            lock (_sync)
            {
                // wait max 5 seconds till the flag gets initialized
                for (int i = 0; (i < 500) && (!_isCrossbarAvailable.HasValue) && (IsRunning); i++)
                {
                    Thread.Sleep(10);
                }

                if ((!IsRunning) || (!_isCrossbarAvailable.HasValue))
                {
                    throw new ApplicationException("The video source must be running in order to display crossbar property page.");
                }

                if (!_isCrossbarAvailable.Value)
                {
                    throw new NotSupportedException("Crossbar configuration is not supported by currently running video source.");
                }

                // pass the request to background thread if video source is running
                _parentWindowForPropertyPage = parentWindow;
                _needToDisplayCrossBarPropertyPage = true;
            }
        }


        /// <summary>
        /// Check if running video source provides crossbar for configuration.
        /// </summary>
        /// 
        /// <returns>Returns <see langword="true"/> if crossbar configuration is available or
        /// <see langword="false"/> otherwise.</returns>
        /// 
        /// <remarks><para>The method reports if the video source provides crossbar configuration
        /// using <see cref="DisplayCrossbarPropertyPage"/>.</para>
        /// </remarks>
        ///
        public bool CheckIfCrossbarAvailable()
        {
            lock (_sync)
            {
                if (!_isCrossbarAvailable.HasValue)
                {
                    if (!IsRunning)
                    {
                        // create graph without playing to collect available inputs
                        WorkerThread(false);
                    }
                    else
                    {
                        for (int i = 0; (i < 500) && (!_isCrossbarAvailable.HasValue); i++)
                        {
                            Thread.Sleep(10);
                        }
                    }
                }

                return (_isCrossbarAvailable.HasValue) && _isCrossbarAvailable.Value;
            }
        }


        /// <summary>
        /// Simulates an external trigger.
        /// </summary>
        /// 
        /// <remarks><para>The method simulates external trigger for video cameras, which support
        /// providing still image snapshots. The effect is equivalent as pressing camera's shutter
        /// button - a snapshot will be provided through <see cref="SnapshotFrame"/> event.</para>
        /// 
        /// <para><note>The <see cref="ProvideSnapshots"/> property must be set to <see langword="true"/>
        /// to enable receiving snapshots.</note></para>
        /// </remarks>
        /// 
        public void SimulateTrigger()
        {
            _needToSimulateTrigger = true;
        }

        /// <summary>
        /// Sets a specified property on the camera.
        /// </summary>
        /// 
        /// <param name="property">Specifies the property to set.</param>
        /// <param name="value">Specifies the new value of the property.</param>
        /// <param name="controlFlags">Specifies the desired control setting.</param>
        /// 
        /// <returns>Returns true on sucee or false otherwise.</returns>
        /// 
        /// <exception cref="ArgumentException">Video source is not specified - device moniker is not set.</exception>
        /// <exception cref="ApplicationException">Failed creating device object for moniker.</exception>
        /// <exception cref="NotSupportedException">The video source does not support camera control.</exception>
        /// 
        public bool SetCameraProperty(CameraControlProperty property, int value, CameraControlFlags controlFlags)
        {
            bool ret;

            // check if source was set
            if (string.IsNullOrEmpty(_deviceMoniker))
            {
                throw new ArgumentException("Video source is not specified.");
            }

            lock (_sync)
            {
                object tempSourceObject;

                // create source device's object
                try
                {
                    tempSourceObject = FilterInfo.CreateFilter(_deviceMoniker);
                }
                catch
                {
                    throw new ApplicationException("Failed creating device object for moniker.");
                }

                if (!(tempSourceObject is IAMCameraControl))
                {
                    throw new NotSupportedException("The video source does not support camera control.");
                }

                var pCamControl = (IAMCameraControl)tempSourceObject;
                int hr = pCamControl.Set(property, value, controlFlags);

                ret = (hr >= 0);

                Marshal.FinalReleaseComObject(tempSourceObject);
            }

            return ret;
        }

        /// <summary>
        /// Gets the current setting of a camera property.
        /// </summary>
        /// 
        /// <param name="property">Specifies the property to retrieve.</param>
        /// <param name="value">Receives the value of the property.</param>
        /// <param name="controlFlags">Receives the value indicating whether the setting is controlled manually or automatically</param>
        /// 
        /// <returns>Returns true on sucee or false otherwise.</returns>
        /// 
        /// <exception cref="ArgumentException">Video source is not specified - device moniker is not set.</exception>
        /// <exception cref="ApplicationException">Failed creating device object for moniker.</exception>
        /// <exception cref="NotSupportedException">The video source does not support camera control.</exception>
        /// 
        public bool GetCameraProperty(CameraControlProperty property, out int value, out CameraControlFlags controlFlags)
        {
            bool ret;

            // check if source was set
            if (string.IsNullOrEmpty(_deviceMoniker))
            {
                throw new ArgumentException("Video source is not specified.");
            }

            lock (_sync)
            {
                object tempSourceObject;

                // create source device's object
                try
                {
                    tempSourceObject = FilterInfo.CreateFilter(_deviceMoniker);
                }
                catch
                {
                    throw new ApplicationException("Failed creating device object for moniker.");
                }

                if (!(tempSourceObject is IAMCameraControl))
                {
                    throw new NotSupportedException("The video source does not support camera control.");
                }

                var pCamControl = (IAMCameraControl)tempSourceObject;
                int hr = pCamControl.Get(property, out value, out controlFlags);

                ret = (hr >= 0);

                Marshal.FinalReleaseComObject(tempSourceObject);
            }

            return ret;
        }

        /// <summary>
        /// Gets the range and default value of a specified camera property.
        /// </summary>
        /// 
        /// <param name="property">Specifies the property to query.</param>
        /// <param name="minValue">Receives the minimum value of the property.</param>
        /// <param name="maxValue">Receives the maximum value of the property.</param>
        /// <param name="stepSize">Receives the step size for the property.</param>
        /// <param name="defaultValue">Receives the default value of the property.</param>
        /// <param name="controlFlags">Receives a member of the <see cref="CameraControlFlags"/> enumeration, indicating whether the property is controlled automatically or manually.</param>
        /// 
        /// <returns>Returns true on sucee or false otherwise.</returns>
        /// 
        /// <exception cref="ArgumentException">Video source is not specified - device moniker is not set.</exception>
        /// <exception cref="ApplicationException">Failed creating device object for moniker.</exception>
        /// <exception cref="NotSupportedException">The video source does not support camera control.</exception>
        /// 
        public bool GetCameraPropertyRange(CameraControlProperty property, out int minValue, out int maxValue, out int stepSize, out int defaultValue, out CameraControlFlags controlFlags)
        {
            bool ret;

            // check if source was set
            if (string.IsNullOrEmpty(_deviceMoniker))
            {
                throw new ArgumentException("Video source is not specified.");
            }

            lock (_sync)
            {
                object tempSourceObject;

                // create source device's object
                try
                {
                    tempSourceObject = FilterInfo.CreateFilter(_deviceMoniker);
                }
                catch
                {
                    throw new ApplicationException("Failed creating device object for moniker.");
                }

                if (!(tempSourceObject is IAMCameraControl))
                {
                    throw new NotSupportedException("The video source does not support camera control.");
                }

                var pCamControl = (IAMCameraControl)tempSourceObject;
                int hr = pCamControl.GetRange(property, out minValue, out maxValue, out stepSize, out defaultValue, out controlFlags);

                ret = (hr >= 0);

                Marshal.FinalReleaseComObject(tempSourceObject);
            }

            return ret;
        }

        /// <summary>
        /// Worker thread.
        /// </summary>
        /// 
        private void WorkerThread()
        {
            WorkerThread(true);
        }

        /// <summary>
        /// Returns property of camera (brightness/gamma etc)
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="setting"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ApplicationException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public bool GetProperty(VideoProcAmpProperty prop, out int setting, out VideoProcAmpFlags flags)
        {
            setting = Int32.MinValue;
            flags = VideoProcAmpFlags.None;

            bool ret = false;
            // check if source was set
            if (string.IsNullOrEmpty(_deviceMoniker))
            {
                return false;
            }

            lock (_sync)
            {
                object tempSourceObject;

                // create source device's object
                try
                {
                    tempSourceObject = FilterInfo.CreateFilter(_deviceMoniker);
                }
                catch
                {
                    return false;
                }

                if (!(tempSourceObject is IAMVideoProcAmp))
                {
                    return false;
                }

                var pCamControl = (IAMVideoProcAmp)tempSourceObject;

                try
                {
                    int hr = pCamControl.Get(prop, out setting, out flags);

                    ret = (hr >= 0);
                }
                catch
                {
                    // ignored
                }

                Marshal.FinalReleaseComObject(tempSourceObject);
            }

            return ret;
        }

        /// <summary>
        /// Whether or not device supports properties (whether they actually work or not is another matter entirely)
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ApplicationException"></exception>
        public bool SupportsProperties
        {
            get
            {
                if (string.IsNullOrEmpty(_deviceMoniker))
                {
                    return false;
                }
                object tempSourceObject;

                // create source device's object
                try
                {
                    tempSourceObject = FilterInfo.CreateFilter(_deviceMoniker);
                }
                catch
                {
                    return false;
                }

                var res = (tempSourceObject is IAMVideoProcAmp);
                try
                {
                    Marshal.FinalReleaseComObject(tempSourceObject);
                }
                catch
                {
                    return false;
                }
                return res;
            }
        }

        /// <summary>
        /// Returns possible range of values for given property
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="steppingData"></param>
        /// <param name="defaultValue"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ApplicationException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public bool GetPropertyRange(VideoProcAmpProperty prop, out int min, out int max, out int steppingData, out int defaultValue, out VideoProcAmpFlags flags)
        {
            min = Int32.MinValue;
            max = Int32.MinValue;
            steppingData = Int32.MinValue;
            defaultValue = Int32.MinValue;
            flags = VideoProcAmpFlags.None;

            bool ret;
            // check if source was set
            if (string.IsNullOrEmpty(_deviceMoniker))
            {
                throw new ArgumentException("Video source is not specified.");
            }

            lock (_sync)
            {
                object tempSourceObject;

                // create source device's object
                try
                {
                    tempSourceObject = FilterInfo.CreateFilter(_deviceMoniker);
                }
                catch
                {
                    throw new ApplicationException("Failed creating device object for moniker.");
                }

                if (!(tempSourceObject is IAMVideoProcAmp))
                {
                    throw new NotSupportedException("The video source does not support properties.");
                }

                var pCamControl = (IAMVideoProcAmp)tempSourceObject;

                int hr = pCamControl.GetRange(prop, out min, out max, out steppingData, out defaultValue, out flags);

                ret = (hr >= 0);

                Marshal.FinalReleaseComObject(tempSourceObject);
            }

            return ret;
        }

        /// <summary>
        /// Sets the given property of the camera
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="setting"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ApplicationException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public bool SetProperty(VideoProcAmpProperty prop, int setting, VideoProcAmpFlags flag)
        {
            bool ret = false;
            // check if source was set
            if (string.IsNullOrEmpty(_deviceMoniker))
            {
                return false;
            }

            lock (_sync)
            {
                object tempSourceObject;

                // create source device's object
                try
                {
                    tempSourceObject = FilterInfo.CreateFilter(_deviceMoniker);
                }
                catch
                {
                    return false;
                }

                if (!(tempSourceObject is IAMVideoProcAmp))
                {
                    return false;
                }

                var pCamControl = (IAMVideoProcAmp)tempSourceObject;

                try
                {
                    int hr = pCamControl.Set(prop, setting, flag);

                    ret = (hr >= 0);
                }
                catch
                {
                    // ignored
                }

                Marshal.FinalReleaseComObject(tempSourceObject);
            }

            return ret;
        }

        private void WorkerThread(bool runGraph)
        {
            bool isSnapshotSupported = false;

            // grabber
            var videoGrabber = new Grabber(this, false);
            var snapshotGrabber = new Grabber(this, true);

            // objects
            object captureGraphObject = null;
            object graphObject = null;
            object videoGrabberObject = null;
            object snapshotGrabberObject = null;
            object crossbarObject = null;

            // interfaces
            IAMVideoControl videoControl = null;
            IPin pinStillImage = null;
            IAMCrossbar crossbar = null;
            _abort = new ManualResetEvent(false);

            try
            {
                // get type of capture graph builder
                Type type = Type.GetTypeFromCLSID(Clsid.CaptureGraphBuilder2);
                if (type == null)
                    throw new ApplicationException("Failed creating capture graph builder");

                // create capture graph builder
                captureGraphObject = Activator.CreateInstance(type);
                var captureGraph = (ICaptureGraphBuilder2)captureGraphObject;

                // get type of filter graph
                type = Type.GetTypeFromCLSID(Clsid.FilterGraph);
                if (type == null)
                    throw new ApplicationException("Failed creating filter graph");

                // create filter graph
                graphObject = Activator.CreateInstance(type);
                var graph = (IFilterGraph2)graphObject;

                // set filter graph to the capture graph builder
                captureGraph.SetFiltergraph(graph);

                // create source device's object
                _sourceObject = FilterInfo.CreateFilter(_deviceMoniker);
                if (_sourceObject == null)
                    throw new ApplicationException("Failed creating device object for moniker");

                // get base filter interface of source device
                var sourceBase = (IBaseFilter)_sourceObject;

                // get video control interface of the device
                try
                {
                    videoControl = (IAMVideoControl)_sourceObject;
                }
                catch (InvalidCastException)
                {
                    // some camera drivers may not support IAMVideoControl interface
                }

                // get type of sample grabber
                type = Type.GetTypeFromCLSID(Clsid.SampleGrabber);
                if (type == null)
                    throw new ApplicationException("Failed creating sample grabber");

                // create sample grabber used for video capture
                videoGrabberObject = Activator.CreateInstance(type);
                var videoSampleGrabber = (ISampleGrabber)videoGrabberObject;
                var videoGrabberBase = (IBaseFilter)videoGrabberObject;
                // create sample grabber used for snapshot capture
                snapshotGrabberObject = Activator.CreateInstance(type);
                var snapshotSampleGrabber = (ISampleGrabber)snapshotGrabberObject;
                var snapshotGrabberBase = (IBaseFilter)snapshotGrabberObject;

                // add source and grabber filters to graph
                graph.AddFilter(sourceBase, "source");
                graph.AddFilter(videoGrabberBase, "grabber_video");
                graph.AddFilter(snapshotGrabberBase, "grabber_snapshot");

                // set media type
                var mediaType = new AMMediaType { MajorType = MediaType.Video, SubType = MediaSubType.RGB24 };

                videoSampleGrabber.SetMediaType(mediaType);
                snapshotSampleGrabber.SetMediaType(mediaType);

                // get crossbar object to to allows configuring pins of capture card
                captureGraph.FindInterface(FindDirection.UpstreamOnly, Guid.Empty, sourceBase, typeof(IAMCrossbar).GUID, out crossbarObject);
                if (crossbarObject != null)
                {
                    crossbar = (IAMCrossbar)crossbarObject;
                }
                _isCrossbarAvailable = (crossbar != null);
                _crossbarVideoInputs = ColletCrossbarVideoInputs(crossbar);

                if (videoControl != null)
                {
                    // find Still Image output pin of the vedio device
                    captureGraph.FindPin(_sourceObject, PinDirection.Output,
                        PinCategory.StillImage, MediaType.Video, false, 0, out pinStillImage);
                    // check if it support trigger mode
                    if (pinStillImage != null)
                    {
                        VideoControlFlags caps;
                        videoControl.GetCaps(pinStillImage, out caps);
                        isSnapshotSupported = ((caps & VideoControlFlags.ExternalTriggerEnable) != 0);
                    }
                }

                // configure video sample grabber
                videoSampleGrabber.SetBufferSamples(false);
                videoSampleGrabber.SetOneShot(false);
                videoSampleGrabber.SetCallback(videoGrabber, 1);

                // configure snapshot sample grabber
                snapshotSampleGrabber.SetBufferSamples(true);
                snapshotSampleGrabber.SetOneShot(false);
                snapshotSampleGrabber.SetCallback(snapshotGrabber, 1);

                // configure pins
                GetPinCapabilitiesAndConfigureSizeAndRate(captureGraph, sourceBase,
                    PinCategory.Capture, _videoResolution, ref _videoCapabilities);
                if (isSnapshotSupported)
                {
                    GetPinCapabilitiesAndConfigureSizeAndRate(captureGraph, sourceBase,
                        PinCategory.StillImage, _snapshotResolution, ref _snapshotCapabilities);
                }
                else
                {
                    _snapshotCapabilities = new VideoCapabilities[0];
                }

                // put video/snapshot capabilities into cache
                lock (CacheVideoCapabilities)
                {
                    if ((_videoCapabilities != null) && (!CacheVideoCapabilities.ContainsKey(_deviceMoniker)))
                    {
                        CacheVideoCapabilities.Add(_deviceMoniker, _videoCapabilities);
                    }
                }
                lock (CacheSnapshotCapabilities)
                {
                    if ((_snapshotCapabilities != null) && (!CacheSnapshotCapabilities.ContainsKey(_deviceMoniker)))
                    {
                        CacheSnapshotCapabilities.Add(_deviceMoniker, _snapshotCapabilities);
                    }
                }

                if (runGraph)
                {
                    // render capture pin
                    captureGraph.RenderStream(PinCategory.Capture, MediaType.Video, sourceBase, null, videoGrabberBase);

                    if (videoSampleGrabber.GetConnectedMediaType(mediaType) == 0)
                    {
                        var vih = (VideoInfoHeader)Marshal.PtrToStructure(mediaType.FormatPtr, typeof(VideoInfoHeader));

                        videoGrabber.Width = vih.BmiHeader.Width;
                        videoGrabber.Height = vih.BmiHeader.Height;

                        mediaType.Dispose();
                    }
                    else
                    {
                        if ((isSnapshotSupported) && (_provideSnapshots))
                        {
                            // render snapshot pin
                            captureGraph.RenderStream(PinCategory.StillImage, MediaType.Video, sourceBase, null, snapshotGrabberBase);

                            if (snapshotSampleGrabber.GetConnectedMediaType(mediaType) == 0)
                            {
                                var vih = (VideoInfoHeader)Marshal.PtrToStructure(mediaType.FormatPtr, typeof(VideoInfoHeader));

                                snapshotGrabber.Width = vih.BmiHeader.Width;
                                snapshotGrabber.Height = vih.BmiHeader.Height;

                                mediaType.Dispose();
                            }
                        }
                    }

                    // get media control
                    var mediaControl = (IMediaControl)graphObject;

                    // get media events' interface
                    var mediaEvent = (IMediaEventEx)graphObject;

                    // run
                    mediaControl.Run();

                    if ((isSnapshotSupported) && (_provideSnapshots))
                    {
                        _startTime = DateTime.Now;
                        videoControl.SetMode(pinStillImage, VideoControlFlags.ExternalTriggerEnable);
                    }

                    _lastFrame = DateTime.UtcNow;
                    do
                    {
                        if (mediaEvent != null)
                        {
                            IntPtr p1;
                            IntPtr p2;
                            DsEvCode code;
                            if (mediaEvent.GetEvent(out code, out p1, out p2, 0) >= 0)
                            {
                                mediaEvent.FreeEventParams(code, p1, p2);

                                if (code == DsEvCode.DeviceLost)
                                {
                                    _res = ReasonToFinishPlaying.DeviceLost;
                                    break;
                                }
                            }
                        }

                        if (_needToSetVideoInput)
                        {
                            _needToSetVideoInput = false;
                            // set/check current input type of a video card (frame grabber)
                            if (_isCrossbarAvailable.Value)
                            {
                                SetCurrentCrossbarInput(crossbar, _crossbarVideoInput);
                                _crossbarVideoInput = GetCurrentCrossbarInput(crossbar);
                            }
                        }

                        if (_needToSimulateTrigger)
                        {
                            _needToSimulateTrigger = false;

                            if ((isSnapshotSupported) && (_provideSnapshots))
                            {
                                videoControl.SetMode(pinStillImage, VideoControlFlags.Trigger);
                            }
                        }

                        if (_needToDisplayPropertyPage)
                        {
                            _needToDisplayPropertyPage = false;
                            DisplayPropertyPage(_parentWindowForPropertyPage, _sourceObject);

                            if (crossbar != null)
                            {
                                _crossbarVideoInput = GetCurrentCrossbarInput(crossbar);
                            }
                        }

                        if (_needToDisplayCrossBarPropertyPage)
                        {
                            _needToDisplayCrossBarPropertyPage = false;

                            if (crossbar != null)
                            {
                                DisplayPropertyPage(_parentWindowForPropertyPage, crossbar);
                                _crossbarVideoInput = GetCurrentCrossbarInput(crossbar);
                            }
                        }

                        if (_lastFrame < DateTime.UtcNow.AddSeconds(-10))
                        {
                            _res = ReasonToFinishPlaying.DeviceLost;
                            _abort?.Set();
                        }
                    }
                    while (!_abort.WaitOne(20, false) && !MainForm.ShuttingDown);

                    mediaControl.Stop();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Device");
                // provide information to clients
                _res = ReasonToFinishPlaying.DeviceLost;
            }
            finally
            {
                // release all objects
                if (graphObject != null)
                {
                    Marshal.FinalReleaseComObject(graphObject);
                }
                if (_sourceObject != null)
                {
                    Marshal.FinalReleaseComObject(_sourceObject);
                    _sourceObject = null;
                }
                if (videoGrabberObject != null)
                {
                    Marshal.FinalReleaseComObject(videoGrabberObject);
                }
                if (snapshotGrabberObject != null)
                {
                    Marshal.FinalReleaseComObject(snapshotGrabberObject);
                }
                if (captureGraphObject != null)
                {
                    Marshal.FinalReleaseComObject(captureGraphObject);
                }
                if (crossbarObject != null)
                {
                    Marshal.FinalReleaseComObject(crossbarObject);
                }
            }

            PlayingFinished?.Invoke(this, new PlayingFinishedEventArgs(_res));
            _abort.Close();
        }

        // Set resolution for the specified stream configuration
        private static void SetResolution(IAMStreamConfig streamConfig, VideoCapabilities resolution)
        {
            if (resolution == null)
            {
                return;
            }

            // iterate through device's capabilities to find mediaType for desired resolution
            int capabilitiesCount, capabilitySize;
            AMMediaType newMediaType = null;
            var caps = new VideoStreamConfigCaps();

            streamConfig.GetNumberOfCapabilities(out capabilitiesCount, out capabilitySize);

            for (int i = 0; i < capabilitiesCount; i++)
            {
                try
                {
                    var vc = new VideoCapabilities(streamConfig, i);

                    if (resolution == vc)
                    {
                        if (streamConfig.GetStreamCaps(i, out newMediaType, caps) == 0)
                        {
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // ignored
                    Logger.LogException(ex, "SetResolution");
                }
            }

            // set the new format
            if (newMediaType != null)
            {
                streamConfig.SetFormat(newMediaType);
                newMediaType.Dispose();
            }
        }

        // Configure specified pin and collect its capabilities if required
        private void GetPinCapabilitiesAndConfigureSizeAndRate(ICaptureGraphBuilder2 graphBuilder, IBaseFilter baseFilter,
            Guid pinCategory, VideoCapabilities resolutionToSet, ref VideoCapabilities[] capabilities)
        {
            object streamConfigObject;
            graphBuilder.FindInterface(pinCategory, MediaType.Video, baseFilter, typeof(IAMStreamConfig).GUID, out streamConfigObject);

            if (streamConfigObject != null)
            {
                IAMStreamConfig streamConfig = null;

                try
                {
                    streamConfig = (IAMStreamConfig)streamConfigObject;
                }
                catch (InvalidCastException ex)
                {
                    Logger.LogException(ex, "GetPinCapabilities");
                }

                if (streamConfig != null)
                {
                    if (capabilities == null)
                    {
                        try
                        {
                            // get all video capabilities
                            capabilities = iSpyPRO.DirectShow.VideoCapabilities.FromStreamConfig(streamConfig);
                        }
                        catch (Exception ex)
                        {
                            Logger.LogException(ex, "Device Caps");

                        }
                    }

                    // check if it is required to change capture settings
                    if (resolutionToSet != null)
                    {
                        SetResolution(streamConfig, resolutionToSet);
                    }
                }
            }

            // if failed resolving capabilities, then just create empty capabilities array,
            // so we don't try again
            if (capabilities == null)
            {
                capabilities = new VideoCapabilities[0];
            }
        }

        // Display property page for the specified object
        private void DisplayPropertyPage(IntPtr parentWindow, object sourceObject)
        {
            try
            {
                // retrieve ISpecifyPropertyPages interface of the device
                var pPropPages = sourceObject as ISpecifyPropertyPages;
                if (pPropPages == null)
                {
                    var e = sourceObject as IAMVfwCompressDialogs;
                    if (e == null)
                    {
                        throw new NotSupportedException("The video source does not support the compressor dialog page.");
                    }
                    e.ShowDialog(VfwCompressDialogs.Config, IntPtr.Zero);
                    return;
                }

                // get property pages from the property bag
                CAUUID caGUID;
                pPropPages.GetPages(out caGUID);

                // get filter info
                var filterInfo = new FilterInfo(_deviceMoniker);

                // create and display the OlePropertyFrame
                NativeMethods.OleCreatePropertyFrame(parentWindow, 0, 0, filterInfo.Name, 1, ref sourceObject, caGUID.cElems, caGUID.pElems, 0, 0, IntPtr.Zero);

                // release COM objects
                Marshal.FreeCoTaskMem(caGUID.pElems);
            }
            catch
            {
                // ignored
            }
        }

        // Collect all video inputs of the specified crossbar
        private VideoInput[] ColletCrossbarVideoInputs(IAMCrossbar crossbar)
        {
            lock (CacheCrossbarVideoInputs)
            {
                if (CacheCrossbarVideoInputs.ContainsKey(_deviceMoniker))
                {
                    return CacheCrossbarVideoInputs[_deviceMoniker];
                }

                var videoInputsList = new List<VideoInput>();

                if (crossbar != null)
                {
                    int inPinsCount, outPinsCount;

                    // gen number of pins in the crossbar
                    if (crossbar.get_PinCounts(out outPinsCount, out inPinsCount) == 0)
                    {
                        // collect all video inputs
                        for (int i = 0; i < inPinsCount; i++)
                        {
                            int pinIndexRelated;
                            PhysicalConnectorType type;

                            if (crossbar.get_CrossbarPinInfo(true, i, out pinIndexRelated, out type) != 0)
                                continue;

                            if (type < PhysicalConnectorType.AudioTuner)
                            {
                                videoInputsList.Add(new VideoInput(i, type));
                            }
                        }
                    }
                }

                var videoInputs = new VideoInput[videoInputsList.Count];
                videoInputsList.CopyTo(videoInputs);

                CacheCrossbarVideoInputs.Add(_deviceMoniker, videoInputs);

                return videoInputs;
            }
        }

        // Get type of input connected to video output of the crossbar
        private VideoInput GetCurrentCrossbarInput(IAMCrossbar crossbar)
        {
            VideoInput videoInput = VideoInput.Default;

            int inPinsCount, outPinsCount;

            // gen number of pins in the crossbar
            if (crossbar.get_PinCounts(out outPinsCount, out inPinsCount) == 0)
            {
                int videoOutputPinIndex = -1;
                int pinIndexRelated;

                // find index of the video output pin
                for (int i = 0; i < outPinsCount; i++)
                {
                    PhysicalConnectorType type;
                    if (crossbar.get_CrossbarPinInfo(false, i, out pinIndexRelated, out type) != 0)
                        continue;

                    if (type == PhysicalConnectorType.VideoDecoder)
                    {
                        videoOutputPinIndex = i;
                        break;
                    }
                }

                if (videoOutputPinIndex != -1)
                {
                    int videoInputPinIndex;

                    // get index of the input pin connected to the output
                    if (crossbar.get_IsRoutedTo(videoOutputPinIndex, out videoInputPinIndex) == 0)
                    {
                        PhysicalConnectorType inputType;

                        crossbar.get_CrossbarPinInfo(true, videoInputPinIndex, out pinIndexRelated, out inputType);

                        videoInput = new VideoInput(videoInputPinIndex, inputType);
                    }
                }
            }

            return videoInput;
        }

        // Set type of input connected to video output of the crossbar
        private void SetCurrentCrossbarInput(IAMCrossbar crossbar, VideoInput videoInput)
        {
            if (videoInput.Type != PhysicalConnectorType.Default)
            {
                int inPinsCount, outPinsCount;

                // gen number of pins in the crossbar
                if (crossbar.get_PinCounts(out outPinsCount, out inPinsCount) == 0)
                {
                    int videoOutputPinIndex = -1;
                    int videoInputPinIndex = -1;
                    int pinIndexRelated;
                    PhysicalConnectorType type;

                    // find index of the video output pin
                    for (int i = 0; i < outPinsCount; i++)
                    {
                        if (crossbar.get_CrossbarPinInfo(false, i, out pinIndexRelated, out type) != 0)
                            continue;

                        if (type == PhysicalConnectorType.VideoDecoder)
                        {
                            videoOutputPinIndex = i;
                            break;
                        }
                    }

                    // find index of the required input pin
                    for (int i = 0; i < inPinsCount; i++)
                    {
                        if (crossbar.get_CrossbarPinInfo(true, i, out pinIndexRelated, out type) != 0)
                            continue;

                        if ((type == videoInput.Type) && (i == videoInput.Index))
                        {
                            videoInputPinIndex = i;
                            break;
                        }
                    }

                    // try connecting pins
                    if ((videoInputPinIndex != -1) && (videoOutputPinIndex != -1) &&
                         (crossbar.CanRoute(videoOutputPinIndex, videoInputPinIndex) == 0))
                    {
                        crossbar.Route(videoOutputPinIndex, videoInputPinIndex);
                    }
                }
            }
        }

        /// <summary>
        /// Notifies clients about new frame.
        /// </summary>
        /// 
        /// <param name="bmp">New frame's image.</param>
        /// 
        private void OnNewFrame(Bitmap bmp)
        {
            var nf = NewFrame;
            if (nf == null || _abort.WaitOne(0) || MainForm.ShuttingDown)
            {
                bmp.Dispose();
                return;
            }
            
            var dae = new NewFrameEventArgs(bmp);
            _lastFrame = DateTime.UtcNow;
            nf.Invoke(this, dae);
            bmp.Dispose();


        }

        /// <summary>
        /// Notifies clients about new snapshot frame.
        /// </summary>
        /// 
        /// <param name="image">New snapshot's image.</param>
        /// 
        private void OnSnapshotFrame(Bitmap image)
        {
            TimeSpan timeSinceStarted = DateTime.Now - _startTime;

            if (timeSinceStarted.TotalSeconds >= 4)
            {
                var sf = SnapshotFrame;
                if (sf != null && !_abort.WaitOne(0) && !MainForm.ShuttingDown)
                {
                    sf(this, new NewFrameEventArgs(image));
                    _lastFrame = DateTime.UtcNow;
                }
            }
        }

        //
        // Video grabber
        //
        private class Grabber : ISampleGrabberCB
        {
            private readonly VideoCaptureDevice _parent;
            private readonly bool _snapshotMode;

            // Width property
            public int Width { private get; set; }

            // Height property
            public int Height { private get; set; }

            // Constructor
            public Grabber(VideoCaptureDevice parent, bool snapshotMode)
            {
                _parent = parent;
                _snapshotMode = snapshotMode;
            }

            // Callback to receive samples
            public int SampleCB(double sampleTime, IntPtr sample)
            {
                return 0;
            }

            // Callback method that receives a pointer to the sample buffer
            public int BufferCB(double sampleTime, IntPtr buffer, int bufferLen)
            {
                if (_parent.NewFrame != null && _parent.ShouldEmitFrame)
                {
                    // create new image
                    using (var image = new Bitmap(Width, Height, PixelFormat.Format24bppRgb))
                    {

                        // lock bitmap data
                        BitmapData imageData = image.LockBits(
                            new Rectangle(0, 0, Width, Height),
                            ImageLockMode.ReadWrite,
                            PixelFormat.Format24bppRgb);

                        // copy image data
                        int srcStride = imageData.Stride;
                        int dstStride = imageData.Stride;

                        unsafe
                        {
                            byte* dst = (byte*) imageData.Scan0.ToPointer() + dstStride*(Height - 1);
                            var src = (byte*) buffer.ToPointer();

                            for (int y = 0; y < Height; y++)
                            {
                                NativeMethods.memcpy(dst, src, srcStride);
                                dst -= dstStride;
                                src += srcStride;
                            }
                        }

                        // unlock bitmap data
                        image.UnlockBits(imageData);

                        // notify parent
                        if (_snapshotMode)
                        {
                            _parent.OnSnapshotFrame(image);
                        }
                        else
                        {
                            _parent.OnNewFrame(image);
                        }

                        // release the image
                    }
                }

                return 0;
            }
        }

        private bool _disposed;
        // Public implementation of Dispose pattern callable by consumers. 
        public void Dispose()
        {
            Dispose(true);
        }

        // Protected implementation of Dispose pattern. 
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
            }

            // Free any unmanaged objects here. 
            //
            _disposed = true;
        }
    }
}
