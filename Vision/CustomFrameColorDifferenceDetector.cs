// AForge Vision Library
// AForge.NET framework
// http://www.aforgenet.com/framework/
//
// Copyright © AForge.NET, 2005-2011
// contacts@aforgenet.com
//

using System;
using System.Drawing;
using System.Drawing.Imaging;
using AForge.Imaging;
using AForge.Imaging.Filters;

namespace iSpyApplication.Vision
{
    /// <summary>
    /// Motion detector based on difference with predefined background frame.
    /// </summary>
    /// 
    /// <remarks><para>The class implements motion detection algorithm, which is based on
    /// difference of current video frame with predefined background frame. The <see cref="MotionFrame">difference frame</see>
    /// is thresholded and the <see cref="MotionLevel">amount of difference pixels</see> is calculated.
    /// To suppress stand-alone noisy pixels erosion morphological operator may be applied, which
    /// is controlled by <see cref="SuppressNoise"/> property.</para>
    /// 
    /// <para><note>In the case if precise motion area's borders are required (for example,
    /// for further motion post processing), then <see cref="KeepObjectsEdges"/> property
    /// may be used to restore borders after noise suppression.</note></para>
    /// 
    /// <para><note>In the case if custom background frame is not specified by using
    /// <see cref="SetBackgroundFrame(Bitmap)"/> method, the algorithm takes first video frame
    /// as a background frame and calculates difference of further video frames with it.</note></para>
    /// 
    /// <para>Unlike <see cref="TwoFramesColorDifferenceDetector"/> motion detection algorithm, this algorithm
    /// allows to identify quite clearly all objects, which are not part of the background (scene) -
    /// most likely moving objects.</para>
    /// 
    /// <para>Sample usage:</para>
    /// <code>
    /// // create motion detector
    /// MotionDetector detector = new MotionDetector(
    ///     new CustomFrameColorDifferenceDetector( ),
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
    /// <seealso cref="MotionDetector"/>
    /// 
    public class CustomFrameColorDifferenceDetector : IMotionDetector
    {
        // frame's dimension
        private int _width;
        private int _height;
		private int _motionSize; // for motion frame

        // previous frame of video stream
        private UnmanagedImage _backgroundFrame;
        // current frame of video sream
        private UnmanagedImage _motionFrame;
        // temporary buffer used for suppressing noise
        private UnmanagedImage _tempFrame;
        // number of pixels changed in the new frame of video stream
        private int _pixelsChanged;

        private bool _manuallySetBackgroundFrame;

        // suppress noise
        private bool _suppressNoise   = true;
        private bool _keepObjectEdges;

        // threshold values
        private int _differenceThreshold    =  15;

        // binary erosion filter
        private readonly BinaryErosion3x3 _erosionFilter = new BinaryErosion3x3( );
        // binary dilatation filter
        private readonly BinaryDilatation3x3 _dilatationFilter = new BinaryDilatation3x3( );

        // dummy object to lock for synchronization
        private readonly object _sync = new object( );

        /// <summary>
        /// Difference threshold value, [1, 255].
        /// </summary>
        /// 
        /// <remarks><para>The value specifies the amount off difference between pixels, which is treated
        /// as motion pixel.</para>
        /// 
        /// <para>Default value is set to <b>15</b>.</para>
        /// </remarks>
        /// 
        public int DifferenceThreshold
        {
            get { return _differenceThreshold; }
            set
            {
                lock ( _sync )
                {
                    _differenceThreshold = Math.Max( 1, Math.Min( 255, value ) );
                }
            }
        }

        /// <summary>
        /// Motion level value, [0, 1].
        /// </summary>
        /// 
        /// <remarks><para>Amount of changes in the last processed frame. For example, if value of
        /// this property equals to 0.1, then it means that last processed frame has 10% difference
        /// with defined background frame.</para>
        /// </remarks>
        /// 
        public float MotionLevel
        {
            get
            {
                lock ( _sync )
                {
                    return (float) _pixelsChanged / ( _width * _height );
                }
            }
        }

        /// <summary>
        /// Motion frame containing detected areas of motion.
        /// </summary>
        /// 
        /// <remarks><para>Motion frame is a grayscale image, which shows areas of detected motion.
        /// All black pixels in the motion frame correspond to areas, where no motion is
        /// detected. But white pixels correspond to areas, where motion is detected.</para>
        /// 
        /// <para><note>The property is set to <see langword="null"/> after processing of the first
        /// video frame by the algorithm in the case if custom background frame was not set manually
        /// by using <see cref="SetBackgroundFrame(Bitmap)"/> method (it will be not <see langword="null"/>
        /// after second call in this case). If correct custom background
        /// was set then the property should bet set to estimated motion frame after
        /// <see cref="ProcessFrame"/> method call.</note></para>
        /// </remarks>
        ///
        public UnmanagedImage MotionFrame
        {
            get
            {
                lock ( _sync )
                {
                    return _motionFrame;
                }
            }
        }

        /// <summary>
        /// Suppress noise in video frames or not.
        /// </summary>
        /// 
        /// <remarks><para>The value specifies if additional filtering should be
        /// done to suppress standalone noisy pixels by applying 3x3 erosion image processing
        /// filter. See <see cref="KeepObjectsEdges"/> property, if it is required to restore
        /// edges of objects, which are not noise.</para>
        /// 
        /// <para>Default value is set to <see langword="true"/>.</para>
        /// 
        /// <para><note>Turning the value on leads to more processing time of video frame.</note></para>
        /// </remarks>
        /// 
        public bool SuppressNoise
        {
            get { return _suppressNoise; }
            set
            {
                lock ( _sync )
                {
                    _suppressNoise = value;

                    // allocate temporary frame if required
                    if ( ( _suppressNoise ) && ( _tempFrame == null ) && ( _motionFrame != null ) )
                    {
                        _tempFrame = UnmanagedImage.Create( _width, _height, PixelFormat.Format8bppIndexed );
                    }

                    // check if temporary frame is not required
                    if ( ( !_suppressNoise ) && ( _tempFrame != null ) )
                    {
                        _tempFrame.Dispose( );
                        _tempFrame = null;
                    }
                }
            }
        }

        /// <summary>
        /// Restore objects edges after noise suppression or not.
        /// </summary>
        /// 
        /// <remarks><para>The value specifies if additional filtering should be done
        /// to restore objects' edges after noise suppression by applying 3x3 dilatation
        /// image processing filter.</para>
        /// 
        /// <para>Default value is set to <see langword="false"/>.</para>
        /// 
        /// <para><note>Turning the value on leads to more processing time of video frame.</note></para>
        /// </remarks>
        /// 
        public bool KeepObjectsEdges
        {
            get { return _keepObjectEdges; }
            set
            {
                lock ( _sync )
                {
                    _keepObjectEdges = value;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomFrameColorDifferenceDetector"/> class.
        /// </summary>
        public CustomFrameColorDifferenceDetector( ) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomFrameColorDifferenceDetector"/> class.
        /// </summary>
        /// 
        /// <param name="suppressNoise">Suppress noise in video frames or not (see <see cref="SuppressNoise"/> property).</param>
        /// 
        public CustomFrameColorDifferenceDetector( bool suppressNoise )
        {
            _suppressNoise = suppressNoise;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomFrameColorDifferenceDetector"/> class.
        /// </summary>
        /// 
        /// <param name="suppressNoise">Suppress noise in video frames or not (see <see cref="SuppressNoise"/> property).</param>
        /// <param name="keepObjectEdges">Restore objects edges after noise suppression or not (see <see cref="KeepObjectsEdges"/> property).</param>
        /// 
        public CustomFrameColorDifferenceDetector( bool suppressNoise, bool keepObjectEdges )
        {
            _suppressNoise   = suppressNoise;
            _keepObjectEdges = keepObjectEdges;
        }

        /// <summary>
        /// Process new video frame.
        /// </summary>
        /// 
        /// <param name="videoFrame">Video frame to process (detect motion in).</param>
        /// 
        /// <remarks><para>Processes new frame from video source and detects motion in it.</para>
        /// 
        /// <para>Check <see cref="MotionLevel"/> property to get information about amount of motion
        /// (changes) in the processed frame.</para>
        /// </remarks>
        /// 
        public unsafe void ProcessFrame( UnmanagedImage videoFrame )
        {
            lock ( _sync )
            {
                // check background frame
                if ( _backgroundFrame == null )
                {
                    // save image dimension
                    _width  = videoFrame.Width;
                    _height = videoFrame.Height;

                    // alocate memory for background frame
                    _backgroundFrame = UnmanagedImage.Create( _width, _height, videoFrame.PixelFormat );

                    // convert source frame to grayscale
					videoFrame.Copy(_backgroundFrame);

                    return;
                }

                // check image dimension
                if ( ( videoFrame.Width != _width ) || ( videoFrame.Height != _height ) )
                    return;

                // check motion frame
                if ( _motionFrame == null )
                {
                    _motionFrame = UnmanagedImage.Create( _width, _height, PixelFormat.Format8bppIndexed );
					_motionSize = _motionFrame.Stride * _height;

                    // temporary buffer
                    if ( _suppressNoise )
                    {
                        _tempFrame = UnmanagedImage.Create( _width, _height, PixelFormat.Format8bppIndexed );
                    }
                }


                // pointers to background and current frames

                var backFrame = (byte*) _backgroundFrame.ImageData.ToPointer( );
                var currFrame = (byte*) videoFrame.ImageData.ToPointer( );
                byte* motion = (byte*) _motionFrame.ImageData.ToPointer( );
                int bytesPerPixel = Tools.BytesPerPixel( videoFrame.PixelFormat );

                // 1 - get difference between frames
                // 2 - threshold the difference (accumulated over every channels)
				for ( int i = 0; i < _height; i++ ) {
					var currFrameLocal = currFrame;
					var backFrameLocal = backFrame;
					var motionLocal = motion;
					for ( int j = 0; j < _width; j++ ) {
						var diff = 0;
						for ( int nbBytes = 0; nbBytes < bytesPerPixel; nbBytes++ ) {
					    	// difference
                    		diff += Math.Abs ( *currFrameLocal -  *backFrameLocal);
							currFrameLocal++;
							backFrameLocal++;
						}
						diff /= bytesPerPixel;
						// threshold
						*motionLocal = ( diff >= _differenceThreshold ) ? (byte) 255 : (byte) 0;
						motionLocal++;
					}
					currFrame += videoFrame.Stride;
					backFrame += _backgroundFrame.Stride;
					motion += _motionFrame.Stride;
				}

                if ( _suppressNoise )
                {
                    // suppress noise and calculate motion amount
                    AForge.SystemTools.CopyUnmanagedMemory( _tempFrame.ImageData, _motionFrame.ImageData, _motionSize );
                    _erosionFilter.Apply( _tempFrame, _motionFrame );

                    if ( _keepObjectEdges )
                    {
                        AForge.SystemTools.CopyUnmanagedMemory( _tempFrame.ImageData, _motionFrame.ImageData, _motionSize );
                        _dilatationFilter.Apply( _tempFrame, _motionFrame );
                    }
                }

                // calculate amount of motion pixels
                _pixelsChanged = 0;
                motion = (byte*) _motionFrame.ImageData.ToPointer( );

                for ( int i = 0; i < _motionSize; i++, motion++ )
                {
                    _pixelsChanged += ( *motion & 1 );
                }
            }
        }

        /// <summary>
        /// Reset motion detector to initial state.
        /// </summary>
        /// 
        /// <remarks><para>Resets internal state and variables of motion detection algorithm.
        /// Usually this is required to be done before processing new video source, but
        /// may be also done at any time to restart motion detection algorithm.</para>
        /// 
        /// <para><note>In the case if custom background frame was set using
        /// <see cref="SetBackgroundFrame(Bitmap)"/> method, this method does not reset it.
        /// The method resets only automatically generated background frame.
        /// </note></para>
        /// </remarks>
        /// 
        public void Reset( )
        {
            // clear background frame only in the case it was not set manually
            Reset( false );
        }

        // Reset motion detector to initial state
        private  void Reset( bool force )
        {
            lock ( _sync )
            {
                if (
                    ( _backgroundFrame != null ) &&
                    ( force || ( _manuallySetBackgroundFrame == false ) )
                    )
                {
                    _backgroundFrame.Dispose( );
                    _backgroundFrame = null;
                }

                
                _motionFrame?.Dispose( );
                _motionFrame = null;
                               
                _tempFrame?.Dispose( );
                _tempFrame = null;
                
            }
        }

        /// <summary>
        /// Set background frame.
        /// </summary>
        /// 
        /// <param name="backgroundFrame">Background frame to set.</param>
        /// 
        /// <remarks><para>The method sets background frame, which will be used to calculate
        /// difference with.</para></remarks>
        /// 
        public void SetBackgroundFrame( Bitmap backgroundFrame )
        {
            BitmapData data = backgroundFrame.LockBits(
                new Rectangle( 0, 0, backgroundFrame.Width, backgroundFrame.Height ),
                ImageLockMode.ReadOnly, backgroundFrame.PixelFormat );

            try
            {
                SetBackgroundFrame( data );
            }
            finally
            {
                backgroundFrame.UnlockBits( data );
            }
        }

        /// <summary>
        /// Set background frame.
        /// </summary>
        /// 
        /// <param name="backgroundFrame">Background frame to set.</param>
        /// 
        /// <remarks><para>The method sets background frame, which will be used to calculate
        /// difference with.</para></remarks>
        /// 
        public void SetBackgroundFrame( BitmapData backgroundFrame )
        {
            SetBackgroundFrame( new UnmanagedImage( backgroundFrame ) );
        }

        /// <summary>
        /// Set background frame.
        /// </summary>
        /// 
        /// <param name="backgroundFrame">Background frame to set.</param>
        /// 
        /// <remarks><para>The method sets background frame, which will be used to calculate
        /// difference with.</para></remarks>
        /// 
        public void SetBackgroundFrame( UnmanagedImage backgroundFrame )
        {
            // reset motion detection algorithm
            Reset( true );

            lock ( _sync )
            {
                // save image dimension
                _width  = backgroundFrame.Width;
                _height = backgroundFrame.Height;

                // alocate memory for previous and current frames
                _backgroundFrame = UnmanagedImage.Create( _width, _height, backgroundFrame.PixelFormat );

                // convert source frame to grayscale
				backgroundFrame.Copy(_backgroundFrame );

                _manuallySetBackgroundFrame = true;
            }
        }
    }
}
