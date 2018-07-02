
using System;
using System.Drawing;
using System.Threading;
using iSpyApplication.Controls;
using iSpyApplication.Utilities;

namespace iSpyApplication.Sources.Video.Ximea
{
    /// <summary>
    /// The class provides continues access to XIMEA cameras.
    /// </summary>
    /// 
    /// <remarks><para>The video source class is aimed to provide continues access to XIMEA camera, when
    /// images are continuosly acquired from camera and provided throw the <see cref="NewFrame"/> event.
    /// It just creates a background thread and gets new images from <see cref="XimeaCamera">XIMEA camera</see>
    /// keeping the <see cref="FrameInterval">specified time interval</see> between image acquisition.
    /// Essentially it is a wrapper class around <see cref="XimeaCamera"/> providing <see cref="IVideoSource"/> interface.</para>
    /// 
    /// <para>Sample usage:</para>
    /// <code>
    /// // create video source for the XIMEA camera with ID 0
    /// XimeaVideoSource videoSource = new XimeaVideoSource( 0 );
    /// // set event handlers
    /// videoSource.NewFrame += new NewFrameEventHandler( video_NewFrame );
    /// // start the video source
    /// videoSource.Start( );
    /// 
    /// // set exposure time to 10 milliseconds
    /// videoSource.SetParam( CameraParameter.Exposure, 10 * 1000 );
    /// 
    /// // ...
    /// 
    /// // New frame event handler, which is invoked on each new available video frame
    /// private void video_NewFrame( object sender, NewFrameEventArgs eventArgs )
    /// {
    ///     // get new frame
    ///     Bitmap bitmap = eventArgs.Frame;
    ///     // process the frame
    /// }
    /// </code>
    /// </remarks>
    /// 
    /// <seealso cref="XimeaCamera"/>
    /// 
    internal class XimeaVideoSource : VideoBase, IVideoSource
    {
        // XIMEA camera to capture images from
        private readonly XimeaCamera _camera = new XimeaCamera( );

        // camera ID
        private readonly int _deviceID;
        private Thread _thread;
        private ManualResetEvent _abort = new ManualResetEvent(false);
        private ReasonToFinishPlaying _res = ReasonToFinishPlaying.DeviceLost;

        // dummy object to lock for synchronization
        private readonly object _sync = new object( );

        /// <summary>
        /// New frame event.
        /// </summary>
        /// 
        /// <remarks><para>Notifies clients about new available frames from the video source.</para>
        /// 
        /// <para><note>Since video source may have multiple clients, each client is responsible for
        /// making a copy (cloning) of the passed video frame, because the video source disposes its
        /// own original copy after notifying of clients.</note></para>
        /// </remarks>
        /// 
        public event NewFrameEventHandler NewFrame;


        /// <summary>
        /// Video playing finished event.
        /// </summary>
        /// 
        /// <remarks><para>This event is used to notify clients that the video playing has finished.</para>
        /// </remarks>
        /// 
        public event PlayingFinishedEventHandler PlayingFinished;

        /// <summary>
        /// A string identifying the video source.
        /// </summary>
        /// 
        public virtual string Source => "Ximea:" + _deviceID;

        /// <summary>
        /// State of the video source.
        /// </summary>
        /// 
        /// <remarks>Current state of video source object - running or not.</remarks>
        /// 
        public bool IsRunning
        {
            get
            {
                Thread tempThread;

                lock ( _sync )
                {
                    tempThread = _thread;
                }

                if ( tempThread != null )
                {
                    // check thread status
                    if ( tempThread.Join( 0 ) == false )
                        return true;

                    // the thread is not running, so free resources
                    Free( );
                }

                return false;
            }
        }

       
        public XimeaVideoSource(CameraWindow source):base(source)
        {
            _deviceID = Convert.ToInt32(source.Nv(source.Camobject.settings.namevaluesettings, "device"));
        }

        public XimeaVideoSource(int deviceid) : base(null)
        {
            _deviceID = deviceid;
        }

        /// <summary>
        /// Start video source.
        /// </summary>
        /// 
        /// <remarks>Starts video source and returns execution to caller. Video camera will be started
        /// and will provide new video frames through the <see cref="NewFrame"/> event.</remarks>
        /// 
        /// <exception cref="ArgumentException">There is no XIMEA camera with specified ID connected to the system.</exception>
        /// 
        public void Start( )
        {
            if ( IsRunning )
                return;

            lock ( _sync )
            {
                if ( _thread == null )
                {
                    // check source
                    if ( _deviceID >= XimeaCamera.CamerasCount )
                    {
                        throw new ArgumentException( "There is no XIMEA camera with specified ID connected to the system." );
                    }

                    // prepare the camera
                    _camera.Open( _deviceID );

                    // create events
                    _abort.Reset();
                    _res = ReasonToFinishPlaying.DeviceLost;

                    // create and start new thread
                    _thread = new Thread(WorkerThread) {Name = Source};
                    _thread.Start( );
                }
            }
        }

        /// <summary>
        /// Wait for video source has stopped.
        /// </summary>
        /// 
        /// <remarks><para></para></remarks>
        /// 
        public void WaitForStop( )
        {
            Thread tempThread;

            lock ( _sync )
            {
                tempThread = _thread;
            }

            if ( tempThread != null )
            {
                // wait for thread stop
                tempThread.Join( );

                Free( );
            }
        }

        /// <summary>
        /// Stop video source.
        /// </summary>
        /// 
        /// <remarks><para>The method stops the video source, so it no longer provides new video frames
        /// and does not consume any resources.</para>
        /// </remarks>
        /// 
        public void Stop( )
        {
            if (!IsRunning)
                return;
            _res = ReasonToFinishPlaying.StoppedByUser;
            _abort.Set();
        }

        public void Restart()
        {
            if (!IsRunning)
                return;
            _res = ReasonToFinishPlaying.Restart;
            _abort.Set();
        }

        /// <summary>
        /// Set camera's parameter.
        /// </summary>
        /// 
        /// <param name="parameterName">Parameter name.</param>
        /// <param name="value">Integer parameter value.</param>
        /// 
        /// <remarks><para><note>The call is redirected to <see cref="XimeaCamera.SetParam(string, int)"/>.</note></para></remarks>
        ///
        public void SetParam( string parameterName, int value )
        {
            _camera.SetParam( parameterName, value );
        }

        /// <summary>
        /// Set camera's parameter.
        /// </summary>
        /// 
        /// <param name="parameterName">Parameter name.</param>
        /// <param name="value">Float parameter value.</param>
        /// 
        /// <remarks><para><note>The call is redirected to <see cref="XimeaCamera.GetParamFloat"/>.</note></para></remarks>
        ///
        public void SetParam( string parameterName, float value )
        {
            _camera.SetParam( parameterName, value );
        }

        /// <summary>
        /// Get camera's parameter as integer value.
        /// </summary>
        /// 
        /// <param name="parameterName">Parameter name to get from camera.</param>
        /// 
        /// <returns>Returns integer value of the requested parameter.</returns>
        /// 
        /// <remarks><para><note>The call is redirected to <see cref="XimeaCamera.GetParamFloat"/>.</note></para></remarks>
        ///
        public int GetParamInt( string parameterName )
        {
            return _camera.GetParamInt( parameterName );
        }

        /// <summary>
        /// Get camera's parameter as float value.
        /// </summary>
        /// 
        /// <param name="parameterName">Parameter name to get from camera.</param>
        /// 
        /// <returns>Returns float value of the requested parameter.</returns>
        /// 
        /// <remarks><para><note>The call is redirected to <see cref="XimeaCamera.GetParamFloat"/>.</note></para></remarks>
        ///
        public float GetParamFloat( string parameterName )
        {
            return _camera.GetParamFloat( parameterName );
        }

        /// <summary>
        /// Get camera's parameter as string value.
        /// </summary>
        /// 
        /// <param name="parameterName">Parameter name to get from camera.</param>
        /// 
        /// <returns>Returns string value of the requested parameter.</returns>
        /// 
        /// <remarks><para><note>The call is redirected to <see cref="XimeaCamera.GetParamString"/>.</note></para></remarks>
        ///
        public string GetParamString( string parameterName )
        {
            return _camera.GetParamString( parameterName );
        }

        // Free resources
        private void Free( )
        {
            _thread = null;
            _camera.Close( );
            
        }

        // Worker thread
        private void WorkerThread( )
        {
            try
            {
                _camera.StartAcquisition( );

                // while there is no request for stop
                while ( !_abort.WaitOne(10) && !MainForm.ShuttingDown )
                {
                    // start time
                    DateTime start = DateTime.Now;

                    // get next frame
                    if (ShouldEmitFrame)
                    {
                        using (var bitmap = _camera.GetImage(15000, false))
                        {
                            NewFrame?.Invoke(this, new NewFrameEventArgs(bitmap));
                        }
                    }
                }
            }
            catch ( Exception ex )
            {
                Logger.LogException(ex, "XIMEA");
                _res = ReasonToFinishPlaying.VideoSourceError;
            }
            finally
            {
                try
                {
                    _camera?.StopAcquisition( );
                }
                catch
                {
                }
            }

            PlayingFinished?.Invoke( this, new PlayingFinishedEventArgs(_res));
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
                _abort.Close();
            }

            // Free any unmanaged objects here. 
            //
            _disposed = true;
        }
    }
}
