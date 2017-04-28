﻿// AForge Vision Library
// AForge.NET framework
// http://www.aforgenet.com/framework/
//
// Copyright © AForge.NET, 2005-2011
// contacts@aforgenet.com
//

using System;
using System.Drawing.Imaging;
using AForge.Imaging;
using AForge.Imaging.Filters;

namespace iSpyApplication.Vision
{
    /// <summary>
    /// Motion detector based on two continues frames difference.
    /// </summary>
    /// 
    /// <remarks><para>The class implements the simplest motion detection algorithm, which is
    /// based on difference of two continues frames. The <see cref="MotionFrame">difference frame</see>
    /// is thresholded and the <see cref="MotionLevel">amount of difference pixels</see> is calculated.
    /// To suppress stand-alone noisy pixels erosion morphological operator may be applied, which
    /// is controlled by <see cref="SuppressNoise"/> property.</para>
    /// 
    /// <para>Although the class may be used on its own to perform motion detection, it is preferred
    /// to use it in conjunction with <see cref="MotionDetector"/> class, which provides additional
    /// features and allows to use moton post processing algorithms.</para>
    /// 
    /// <para>Sample usage:</para>
    /// <code>
    /// // create motion detector
    /// MotionDetector detector = new MotionDetector(
    ///     new TwoFramesDifferenceDetector( ),
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
    public class TwoFramesDifferenceDetector : IMotionDetector
    {
        // frame's dimension
        private int _width;
        private int _height;
        private int _frameSize;

        // previous frame of video stream
        private UnmanagedImage _previousFrame;
        // current frame of video sream
        private UnmanagedImage _motionFrame;
        // temporary buffer used for suppressing noise
        private UnmanagedImage _tempFrame;
        // number of pixels changed in the new frame of video stream
        private int _pixelsChanged;

        // suppress noise
        private bool _suppressNoise = true;
        private bool _keepObjectEdges = false;

        // threshold values
        private int _differenceThreshold    =  15;
        private int _differenceThresholdNeg = -15;

        // binary erosion filter
        private readonly BinaryErosion3x3 _erosionFilter = new BinaryErosion3x3( );
        // binary dilatation filter
        private readonly BinaryDilatation3x3 _dilatationFilter = new BinaryDilatation3x3();

        // dummy object to lock for synchronization
        //private readonly object _sync = new object( );

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
                //lock ( _sync )
                {
                    _differenceThreshold = Math.Max( 1, Math.Min( 255, value ) );
                    _differenceThresholdNeg = -_differenceThreshold;
                }
            }
        }

        /// <summary>
        /// Motion level value, [0, 1].
        /// </summary>
        /// 
        /// <remarks><para>Amount of changes in the last processed frame. For example, if value of
        /// this property equals to 0.1, then it means that last processed frame has 10% difference
        /// with previous frame.</para>
        /// </remarks>
        /// 
        public float MotionLevel
        {
            get
            {
                //lock ( _sync )
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
        /// video frame by the algorithm.</note></para>
        /// </remarks>
        ///
        public UnmanagedImage MotionFrame
        {
            get
            {
                //lock ( _sync )
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
        /// filter.</para>
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
                //lock ( _sync )
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
                lock (_sync)
                {
                    _keepObjectEdges = value;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TwoFramesDifferenceDetector"/> class.
        /// </summary>
        /// 
        public TwoFramesDifferenceDetector( ) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TwoFramesDifferenceDetector"/> class.
        /// </summary>
        /// 
        /// <param name="suppressNoise">Suppress noise in video frames or not (see <see cref="SuppressNoise"/> property).</param>
        /// 
        public TwoFramesDifferenceDetector( bool suppressNoise )
        {
            _suppressNoise = suppressNoise;
            _keepObjectEdges = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TwoFramesDifferenceDetector"/> class.
        /// </summary>
        /// 
        /// <param name="suppressNoise">Suppress noise in video frames or not (see <see cref="SuppressNoise"/> property).</param>
        /// <param name="keepObjectEdges">Restore objects edges after noise suppression or not (see <see cref="KeepObjectsEdges"/> property).</param>
        /// 
        public TwoFramesDifferenceDetector( bool suppressNoise, bool keepObjectEdges )
        {
            _suppressNoise = suppressNoise;
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
            //lock ( _sync )
            {
                // check previous frame
                if ( _previousFrame == null )
                {
                    // save image dimension
                    _width = videoFrame.Width;
                    _height = videoFrame.Height;

                    // alocate memory for previous and current frames
                    _previousFrame = UnmanagedImage.Create( _width, _height, PixelFormat.Format8bppIndexed );
                    _motionFrame = UnmanagedImage.Create( _width, _height, PixelFormat.Format8bppIndexed );

                    _frameSize = _motionFrame.Stride * _height;

                    // temporary buffer
                    if ( _suppressNoise )
                    {
                        _tempFrame = UnmanagedImage.Create( _width, _height, PixelFormat.Format8bppIndexed );
                    }

                    // convert source frame to grayscale
                    Tools.ConvertToGrayscale( videoFrame, _previousFrame );

                    return;
                }

                // check image dimension
                if ( ( videoFrame.Width != _width ) || ( videoFrame.Height != _height ) )
                    return;

                // convert current image to grayscale
                Tools.ConvertToGrayscale( videoFrame, _motionFrame );

                // pointers to previous and current frames
                byte* prevFrame = (byte*) _previousFrame.ImageData.ToPointer( );
                byte* currFrame = (byte*) _motionFrame.ImageData.ToPointer( );
                // difference value

                // 1 - get difference between frames
                // 2 - threshold the difference
                // 3 - copy current frame to previous frame
                for ( int i = 0; i < _frameSize; i++, prevFrame++, currFrame++ )
                {
                    // difference
                    var diff = *currFrame -  *prevFrame;
                    // copy current frame to previous
                    *prevFrame = *currFrame;
                    // treshold
                    *currFrame = ( ( diff >= _differenceThreshold ) || ( diff <= _differenceThresholdNeg ) ) ? (byte) 255 : (byte) 0;
                }

                if ( _suppressNoise )
                {
                    // suppress noise and calculate motion amount
                    _erosionFilter.Apply( _motionFrame, _tempFrame );    // src -> dst

                    if ( _keepObjectEdges )
                    {
                        _dilatationFilter.Apply( _tempFrame, _motionFrame );  // src -> dst
                    }
                    else
                    {
                        AForge.SystemTools.CopyUnmanagedMemory( _motionFrame.ImageData, _tempFrame.ImageData, _frameSize );  // dst <- src
                    }
                }

                // calculate amount of motion pixels
                _pixelsChanged = 0;
                byte* motion = (byte*) _motionFrame.ImageData.ToPointer( );

                for ( int i = 0; i < _frameSize; i++, motion++ )
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
        /// </remarks>
        /// 
        public void Reset( )
        {
            //lock ( _sync )
            {
                if ( _previousFrame != null )
                {
                    _previousFrame.Dispose( );
                    _previousFrame = null;
                }

                if ( _motionFrame != null )
                {
                    _motionFrame.Dispose( );
                    _motionFrame = null;
                }

                if ( _tempFrame != null )
                {
                    _tempFrame.Dispose( );
                    _tempFrame = null;
                }
            }
        }
    }
}