// AForge Vision Library
// AForge.NET framework
// http://www.aforgenet.com/framework/
//
// Copyright © AForge.NET, 2005-2011
// contacts@aforgenet.com
//

using System.Drawing;
using System.Drawing.Imaging;
using AForge;
using AForge.Imaging;

namespace iSpyApplication.Vision
{
    /// <summary>
    /// Motion detection wrapper class, which performs motion detection and processing.
    /// </summary>
    ///
    /// <remarks><para>The class serves as a wrapper class for
    /// <see cref="IMotionDetector">motion detection</see> and
    /// <see cref="IMotionProcessing">motion processing</see> algorithms, allowing to call them with
    /// single call. Unlike motion detection and motion processing interfaces, the class also
    /// provides additional methods for convenience, so the algorithms could be applied not
    /// only to <see cref="AForge.Imaging.UnmanagedImage"/>, but to .NET's <see cref="Bitmap"/> class
    /// as well.</para>
    /// 
    /// <para>In addition to wrapping of motion detection and processing algorthms, the class provides
    /// some additional functionality. Using <see cref="MotionZones"/> property it is possible to specify
    /// set of rectangular zones to observe - only motion in these zones is counted and post procesed.</para>
    /// 
    /// <para>Sample usage:</para>
    /// <code>
    /// // create motion detector
    /// MotionDetector detector = new MotionDetector(
    ///     new SimpleBackgroundModelingDetector( ),
    ///     new MotionAreaHighlighting( ) );
    /// 
    /// // continuously feed video frames to motion detector
    /// while ( ... )
    /// {
    ///     // process new video frame and check motion level
    ///     if ( detector.ProcessFrame( videoFrame ) > 0.02 )
    ///     {
    ///         // ring alarm or do somethng else
    ///     }
    /// }
    /// </code>
    /// </remarks>
    ///
    public class MotionDetector
    {
        private IMotionDetector   _detector;
        private IMotionProcessing _processor;

        // motion detection zones
        private Rectangle[] _motionZones;
        // image of motion zones
        private UnmanagedImage _zonesFrame;
        // size of video frame
        private int _videoWidth, _videoHeight;

        // dummy object to lock for synchronization
        private readonly object _sync = new object( );

        /// <summary>
        /// Motion detection algorithm to apply to each video frame.
        /// </summary>
        ///
        /// <remarks><para>The property sets motion detection algorithm, which is used by
        /// <see cref="ProcessFrame(UnmanagedImage)"/> method in order to calculate
        /// <see cref="IMotionDetector.MotionLevel">motion level</see> and
        /// <see cref="IMotionDetector.MotionFrame">motion frame</see>.
        /// </para></remarks>
        ///
        public IMotionDetector MotionDetectionAlgorithm
        {
            get { return _detector; }
            set
            {
                lock ( _sync )
                {
                    _detector = value;
                }
            }
        }

        /// <summary>
        /// Motion processing algorithm to apply to each video frame after
        /// motion detection is done.
        /// </summary>
        /// 
        /// <remarks><para>The property sets motion processing algorithm, which is used by
        /// <see cref="ProcessFrame(UnmanagedImage)"/> method after motion detection in order to do further
        /// post processing of motion frames. The aim of further post processing depends on
        /// actual implementation of the specified motion processing algorithm - it can be
        /// highlighting of motion area, objects counting, etc.
        /// </para></remarks>
        /// 
        public IMotionProcessing MotionProcessingAlgorithm
        {
            get { return _processor; }
            set
            {
               // lock ( _sync )
                {
                    _processor = value;
                }
            }
        }

        /// <summary>
        /// Set of zones to detect motion in.
        /// </summary>
        /// 
        /// <remarks><para>The property keeps array of rectangular zones, which are observed for motion detection.
        /// Motion outside of these zones is ignored.</para>
        /// 
        /// <para>In the case if this property is set, the <see cref="ProcessFrame(UnmanagedImage)"/> method
        /// will filter out all motion witch was detected by motion detection algorithm, but is not
        /// located in the specified zones.</para>
        /// </remarks>
        /// 
        public Rectangle[] MotionZones
        {
            get { return _motionZones; }
            set
            {
                _motionZones = value;
                if (value!=null)
                    CreateMotionZonesFrame( );
            }
        }

        /// <summary>
        /// Unmanaged image to use as zone template
        /// </summary>
        /// 
        /// <remarks><para>Needs to be 8bppIndexed. Area will be calculated on setting</para>
        /// 
        /// <para>In the case if this property is set, the <see cref="ProcessFrame(UnmanagedImage)"/> method
        /// will filter out all motion witch was detected by motion detection algorithm, but is not
        /// located in the specified zones.</para>
        /// </remarks>
        /// 
        public UnmanagedImage ZonesFrameImage
        {
            get { return _zonesFrame; }
            set
            {
                //lock (_sync)
                {
                    _area = 0;
                    // free previous motion zones frame
                    if (_zonesFrame != null)
                    {
                        _zonesFrame.Dispose();
                        _zonesFrame = null;
                    }
                    _zonesFrame = value;
                    if (_zonesFrame != null)
                    {
                        unsafe
                        {
                            //calculate area

                            int stride = _zonesFrame.Stride;
                            var ptr = (byte*) _zonesFrame.ImageData.ToPointer();

                            for (int x = 0; x < _zonesFrame.Width; x++)
                            {
                                for (int y = 0; y < _zonesFrame.Height; y++)
                                {
                                    var b = ptr + y*stride + x;
                                    if (*b==255)
                                    {
                                        _area++;
                                    }
                                }
                            }
                        }
                    }
                }
                
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MotionDetector"/> class.
        /// </summary>
        /// 
        /// <param name="detector">Motion detection algorithm to apply to each video frame.</param>
        /// 
        public MotionDetector( IMotionDetector detector ) : this( detector, null ) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MotionDetector"/> class.
        /// </summary>
        /// 
        /// <param name="detector">Motion detection algorithm to apply to each video frame.</param>
        /// <param name="processor">Motion processing algorithm to apply to each video frame after
        /// motion detection is done.</param>
        /// 
        public MotionDetector( IMotionDetector detector, IMotionProcessing processor )
        {
            _detector  = detector;
            _processor = processor;
        }

        /// <summary>
        /// Process new video frame.
        /// </summary>
        /// 
        /// <param name="videoFrame">Video frame to process (detect motion in).</param>
        /// 
        /// <returns>Returns amount of motion, which is provided <see cref="IMotionDetector.MotionLevel"/>
        /// property of the <see cref="MotionDetectionAlgorithm">motion detection algorithm in use</see>.</returns>
        /// 
        /// <remarks><para>See <see cref="ProcessFrame(UnmanagedImage)"/> for additional details.</para>
        /// </remarks>
        /// 
        public float ProcessFrame( Bitmap videoFrame )
        {
            float motionLevel;

            BitmapData videoData = videoFrame.LockBits(
                new Rectangle( 0, 0, videoFrame.Width, videoFrame.Height ),
                ImageLockMode.ReadWrite, videoFrame.PixelFormat );

            try
            {
                motionLevel = ProcessFrame( new UnmanagedImage( videoData ) );
            }
            finally
            {
                videoFrame.UnlockBits( videoData );
            }

            return motionLevel;
        }

        /// <summary>
        /// Process new video frame.
        /// </summary>
        /// 
        /// <param name="videoFrame">Video frame to process (detect motion in).</param>
        /// 
        /// <returns>Returns amount of motion, which is provided <see cref="IMotionDetector.MotionLevel"/>
        /// property of the <see cref="MotionDetectionAlgorithm">motion detection algorithm in use</see>.</returns>
        /// 
        /// <remarks><para>See <see cref="ProcessFrame(UnmanagedImage)"/> for additional details.</para>
        /// </remarks>
        ///
        public float ProcessFrame( BitmapData videoFrame )
        {
            return ProcessFrame( new UnmanagedImage( videoFrame ) );
        }

        /// <summary>
        /// Process new video frame.
        /// </summary>
        /// 
        /// <param name="videoFrame">Video frame to process (detect motion in).</param>
        /// 
        /// <returns>Returns amount of motion, which is provided <see cref="IMotionDetector.MotionLevel"/>
        /// property of the <see cref="MotionDetectionAlgorithm">motion detection algorithm in use</see>.</returns>
        /// 
        /// <remarks><para>The method first of all applies motion detection algorithm to the specified video
        /// frame to calculate <see cref="IMotionDetector.MotionLevel">motion level</see> and
        /// <see cref="IMotionDetector.MotionFrame">motion frame</see>. After this it applies motion processing algorithm
        /// (if it was set) to do further post processing, like highlighting motion areas, counting moving
        /// objects, etc.</para>
        /// 
        /// <para><note>In the case if <see cref="MotionZones"/> property is set, this method will perform
        /// motion filtering right after motion algorithm is done and before passing motion frame to motion
        /// processing algorithm. The method does filtering right on the motion frame, which is produced
        /// by motion detection algorithm. At the same time the method recalculates motion level and returns
        /// new value, which takes motion zones into account (but the new value is not set back to motion detection
        /// algorithm' <see cref="IMotionDetector.MotionLevel"/> property).
        /// </note></para>
        /// </remarks>
        /// 
        public float ProcessFrame( UnmanagedImage videoFrame )
        {
            lock ( _sync )
            {
                if (_detector == null)
                    return 0;

                _videoWidth = videoFrame.Width;
                _videoHeight = videoFrame.Height;

                if (_area == 0)
                    _area = _videoWidth*_videoHeight;

                // call motion detection
                _detector.ProcessFrame(videoFrame);
                var motionLevel = _detector.MotionLevel;

                // check if motion zones are specified
                if (_detector.MotionFrame!=null && _motionZones != null)
                {
                    if (_zonesFrame == null)
                    {
                        CreateMotionZonesFrame();
                    }

                    if (_zonesFrame != null && (_videoWidth == _zonesFrame.Width) && (_videoHeight == _zonesFrame.Height))
                    {
                        unsafe
                        {
                            // pointers to background and current frames
                            var zonesPtr = (byte*) _zonesFrame.ImageData.ToPointer();
                            var motionPtr = (byte*) _detector.MotionFrame.ImageData.ToPointer();

                            motionLevel = 0;

                            for (int i = 0, frameSize = _zonesFrame.Stride*_videoHeight;
                                i < frameSize;
                                i++, zonesPtr++, motionPtr++)
                            {
                                *motionPtr &= *zonesPtr;
                                motionLevel += (*motionPtr & 1);
                            }
                            motionLevel /= _area;
                        }
                    }
                }

                // call motion post processing
                ApplyOverlay(videoFrame);
                return motionLevel;                
            }
        }

        public void ApplyOverlay(UnmanagedImage videoFrame)
        {
            if ((_processor != null) && (_detector?.MotionFrame != null))
            {
                _processor.ProcessFrame(videoFrame, _detector.MotionFrame);
            }
        }

        /// <summary>
        /// Reset motion detector to initial state.
        /// </summary>
        /// 
        /// <remarks><para>The method resets motion detection and motion processing algotithms by calling
        /// their <see cref="IMotionDetector.Reset"/> and <see cref="IMotionProcessing.Reset"/> methods.</para>
        /// </remarks>
        /// 
        public void Reset( )
        {
           // lock ( _sync )
            {
                _detector?.Reset( );
                _processor?.Reset( );

                _videoWidth  = 0;
                _videoHeight = 0;

                if ( _zonesFrame != null )
                {
                    _zonesFrame.Dispose( );
                    _zonesFrame = null;
                }
            }
        }

        private int _area;

        // Create motion zones' image
        private unsafe void CreateMotionZonesFrame( )
        {
            lock ( _sync )
            {
                _area = 0;
                // free previous motion zones frame
                if ( _zonesFrame != null )
                {
                    _zonesFrame.Dispose( );
                    _zonesFrame = null;
                }

                // create motion zones frame only in the case if the algorithm has processed at least one frame
                if ( ( _motionZones != null ) && ( _motionZones.Length != 0 ) && ( _videoWidth != 0 ) )
                {
                    _zonesFrame = UnmanagedImage.Create( _videoWidth, _videoHeight, PixelFormat.Format8bppIndexed );

                    var imageRect = new Rectangle( 0, 0, _videoWidth, _videoHeight );
                    
                    // draw all motion zones on motion frame
                    foreach ( Rectangle rect in _motionZones )
                    {
                        rect.Intersect( imageRect );

                        // rectangle's dimension
                        int rectWidth  = rect.Width;
                        int rectHeight = rect.Height;

                        // start pointer
                        int stride = _zonesFrame.Stride;
                        byte* ptr = (byte*) _zonesFrame.ImageData.ToPointer( ) + rect.Y * stride + rect.X;

                        for ( int y = 0; y < rectHeight; y++ )
                        {
                            SystemTools.SetUnmanagedMemory( ptr, 255, rectWidth );
                            ptr += stride;
                        }
                        _area += rect.Width*rect.Height;
                    }
                }
            }
        }
    }
}
