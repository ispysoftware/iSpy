// AForge Vision Library
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

        // threshold values
        private int _differenceLevel = 16;
        private ulong _differenceThresholMask = 0xF0F0F0F0F0F0F0F0;
        private readonly object _sync = new object();

        /// <summary>
        /// Difference threshold value, [1, 255].
        /// </summary>
        /// 
        /// <remarks><para>The value specifies the amount off difference between pixels, which is treated
        /// as motion pixel.</para>
        /// 
        /// <para>Default value is set to <b>16</b>.</para>
        /// </remarks>
        /// 
        public int DifferenceThreshold
        {
            get { return _differenceLevel; }
            set
            {
                {
                    _differenceLevel = Math.Max(1, Math.Min(255, value));
                    if ((_differenceLevel & 0x80) != 0)
                        _differenceThresholMask = 0x8080808080808080; // difference >= 128 per byte
                    else if ((_differenceLevel & 0x40) != 0)
                        _differenceThresholMask = 0xC0C0C0C0C0C0C0C0; // difference >= 64 per byte
                    else if ((_differenceLevel & 0x20) != 0)
                        _differenceThresholMask = 0xE0E0E0E0E0E0E0E0; // difference >= 32 per byte
                    else if ((_differenceLevel & 0x10) != 0)
                        _differenceThresholMask = 0xF0F0F0F0F0F0F0F0; // difference >= 16 per byte
                    else if ((_differenceLevel & 0x08) != 0)
                        _differenceThresholMask = 0xF8F8F8F8F8F8F8F8; // difference >= 8 per byte
                    else if ((_differenceLevel & 0x04) != 0)
                        _differenceThresholMask = 0xFCFCFCFCFCFCFCFC; // difference >= 4 per byte
                    else if ((_differenceLevel & 0x02) != 0)
                        _differenceThresholMask = 0xFEFEFEFEFEFEFEFE; // difference >= 2 per byte
                    else
                        _differenceThresholMask = 0xFFFFFFFFFFFFFFFF; // difference >= 1 per byte
                }
            }
        }

        // binary erosion filter
        private readonly BinaryErosion3x3 _erosionFilter = new BinaryErosion3x3();

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
                lock ( _sync )
                {
                    return (float)_pixelsChanged / (_width * _height);
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
                lock ( _sync )
                {
                    _suppressNoise = value;

                    // allocate temporary frame if required
                    if ((_suppressNoise) && (_tempFrame == null) && (_motionFrame != null))
                    {
                        _tempFrame = UnmanagedImage.Create(_width, _height, PixelFormat.Format8bppIndexed);
                    }

                    // check if temporary frame is not required
                    if ((!_suppressNoise) && (_tempFrame != null))
                    {
                        _tempFrame.Dispose();
                        _tempFrame = null;
                    }
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TwoFramesDifferenceDetector"/> class.
        /// </summary>
        /// 
        public TwoFramesDifferenceDetector() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TwoFramesDifferenceDetector"/> class.
        /// </summary>
        /// 
        /// <param name="suppressNoise">Suppress noise in video frames or not (see <see cref="SuppressNoise"/> property).</param>
        /// 
        public TwoFramesDifferenceDetector(bool suppressNoise)
        {
            _suppressNoise = suppressNoise;
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
        public unsafe void ProcessFrame(UnmanagedImage videoFrame)
        {
            lock ( _sync )
            {
                // check previous frame
                if (_previousFrame == null)
                {
                    // save image dimension
                    _width = videoFrame.Width;
                    _height = videoFrame.Height;

                    // alocate memory for previous and current frames
                    _previousFrame = UnmanagedImage.Create(_width, _height, PixelFormat.Format8bppIndexed);
                    _motionFrame = UnmanagedImage.Create(_width, _height, PixelFormat.Format8bppIndexed);

                    _frameSize = _motionFrame.Stride * _height;

                    // temporary buffer
                    if (_suppressNoise)
                    {
                        _tempFrame = UnmanagedImage.Create(_width, _height, PixelFormat.Format8bppIndexed);
                    }

                    // convert source frame to grayscale
                    Tools.ConvertToGrayscale(videoFrame, _previousFrame);

                    return;
                }

                // check image dimension
                if ((videoFrame.Width != _width) || (videoFrame.Height != _height))
                    return;

                // convert current image to grayscale
                Tools.ConvertToGrayscale(videoFrame, _motionFrame);

                UInt64* prevFrame = (UInt64*)_previousFrame.ImageData.ToPointer();
                UInt64* currFrame = (UInt64*)_motionFrame.ImageData.ToPointer();
                // difference value

                // 1 - get difference between frames
                // 2 - threshold the difference
                // 3 - copy current frame to previous frame
                for (int i = 0; i < _frameSize / sizeof(UInt64); i++, prevFrame++, currFrame++)
                {
                    // difference
                    var diff = (*currFrame ^ *prevFrame) & _differenceThresholMask;
                    // copy current frame to previous
                    *prevFrame = *currFrame;
                    // treshold
                    *currFrame = 0;
                    if ((diff & 0xFF00000000000000) != 0) // take care of the 1st byte
                        *currFrame |= 0xFF00000000000000;
                    if ((diff & 0x00FF000000000000) != 0) // take care of the 2nd byte
                        *currFrame |= 0x00FF000000000000;
                    if ((diff & 0x0000FF0000000000) != 0) // take care of the 3rd byte
                        *currFrame |= 0x0000FF0000000000;
                    if ((diff & 0x000000FF00000000) != 0) // take care of the 4th byte
                        *currFrame |= 0x000000FF00000000;
                    if ((diff & 0x00000000FF000000) != 0) // take care of the 5th byte
                        *currFrame |= 0x00000000FF000000;
                    if ((diff & 0x0000000000FF0000) != 0) // take care of the 6th byte
                        *currFrame |= 0x0000000000FF0000;
                    if ((diff & 0x000000000000FF00) != 0) // take care of the 7th byte
                        *currFrame |= 0x000000000000FF00;
                    if ((diff & 0x00000000000000FF) != 0) // take care of the 8th byte
                        *currFrame |= 0x00000000000000FF;
                }

                if (_suppressNoise)
                {
                    // suppress noise and calculate motion amount
                    AForge.SystemTools.CopyUnmanagedMemory(_tempFrame.ImageData, _motionFrame.ImageData, _frameSize);
                    _erosionFilter.Apply(_tempFrame, _motionFrame);
                }

                // calculate amount of motion pixels
                _pixelsChanged = 0;
                UInt64* motion = (UInt64*)_motionFrame.ImageData.ToPointer();

                for (int i = 0; i < _frameSize / sizeof(UInt64); i++, motion++)
                {
                    if ((*motion & 0xFF00000000000000) != 0) // take care of the 1st byte
                        _pixelsChanged++;
                    if ((*motion & 0x00FF000000000000) != 0) // take care of the 2nd byte
                        _pixelsChanged++;
                    if ((*motion & 0x0000FF0000000000) != 0) // take care of the 3rd byte
                        _pixelsChanged++;
                    if ((*motion & 0x000000FF00000000) != 0) // take care of the 4th byte
                        _pixelsChanged++;
                    if ((*motion & 0x00000000FF000000) != 0) // take care of the 5th byte
                        _pixelsChanged++;
                    if ((*motion & 0x0000000000FF0000) != 0) // take care of the 6th byte
                        _pixelsChanged++;
                    if ((*motion & 0x000000000000FF00) != 0) // take care of the 7th byte
                        _pixelsChanged++;
                    if ((*motion & 0x00000000000000FF) != 0) // take care of the 8th byte
                        _pixelsChanged++;
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
        public void Reset()
        {
            lock ( _sync )
            {
                if (_previousFrame != null)
                {
                    _previousFrame.Dispose();
                    _previousFrame = null;
                }

                if (_motionFrame != null)
                {
                    _motionFrame.Dispose();
                    _motionFrame = null;
                }

                if (_tempFrame != null)
                {
                    _tempFrame.Dispose();
                    _tempFrame = null;
                }
            }
        }
    }
}