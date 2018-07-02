using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using AForge.Imaging.Filters;
using iSpyApplication.Controls;
using iSpyApplication.Kinect;
using iSpyApplication.Sources.Audio;
using iSpyApplication.Utilities;
using Microsoft.Kinect;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace iSpyApplication.Sources.Video
{
    internal class KinectStream : VideoBase, IVideoSource, IAudioSource, ISupportsAudio
    {
        private readonly Pen _inferredBonePen = new Pen(Brushes.Gray, 1);
        private readonly Pen _trackedBonePen = new Pen(Brushes.Green, 2);
        private readonly Brush _trackedJointBrush = new SolidBrush(Color.FromArgb(255, 68, 192, 68));
        private readonly Brush _inferredJointBrush = Brushes.Yellow;
        internal static Pen TripWirePen = new Pen(Color.DarkOrange);
        private Skeleton[] _skeletons = new Skeleton[0];
        private const int JointThickness = 3;
        private KinectSensor _sensor;
        private readonly bool _skeleton, _tripwires;
        private DateTime _lastWarnedTripWire = DateTime.MinValue;
        //private readonly bool _bound;
        public int StreamMode;//color
        private ReasonToFinishPlaying _res;
        private ManualResetEvent _abort;

        private string _uniqueKinectId;
        ////Depth Stuff
        private short[] _depthPixels;
        private byte[] _colorPixels;

        public IAudioSource OutAudio;

        #region Audio
        private float _gain;
        private bool _listening;

        public int BytePacket = 400;

        private Stream _audioStream;
        private BufferedWaveProvider _waveProvider;
        private SampleChannel _sampleChannel;

        public BufferedWaveProvider WaveOutProvider { get; set; }

        public event DataAvailableEventHandler DataAvailable;
        public event LevelChangedEventHandler LevelChanged;
        public event AudioFinishedEventHandler AudioFinished;
        public event HasAudioStreamEventHandler HasAudioStream;
        /// <summary>
        /// Buffer used to hold audio data read from audio stream.
        /// </summary>
        private readonly byte[] _audioBuffer = new byte[50 * 16 * 2];

        public float Gain
        {
            get { return _gain; }
            set
            {
                _gain = value;
                if (_sampleChannel != null)
                {
                    _sampleChannel.Volume = value;
                }
            }
        }

        public bool Listening
        {
            get
            {
                if (IsRunning && _listening)
                    return true;
                return false;

            }
            set
            {
                if (RecordingFormat == null)
                {
                    _listening = false;
                    return;
                }

                if (WaveOutProvider != null)
                {
                    if (WaveOutProvider.BufferedBytes>0) WaveOutProvider.ClearBuffer();
                    WaveOutProvider = null;
                }

                if (value)
                {
                    WaveOutProvider = new BufferedWaveProvider(RecordingFormat) { DiscardOnBufferOverflow = true, BufferDuration = TimeSpan.FromMilliseconds(500) };
                }

                _listening = value;
            }
        }

        public WaveFormat RecordingFormat { get; set; }

        #endregion

        public KinectStream(CameraWindow source): base(source)
        {
            _tripwires = Convert.ToBoolean(source.Nv(source.Camobject.settings.namevaluesettings, "TripWires"));
            _uniqueKinectId = source.Nv(source.Camobject.settings.namevaluesettings, "UniqueKinectId");
            _skeleton = Convert.ToBoolean(source.Nv(source.Camobject.settings.namevaluesettings, "KinectSkeleton"));
            StreamMode = Convert.ToInt32(source.Nv(source.Camobject.settings.namevaluesettings, "StreamMode"));
        }

        public int Tilt
        {
            get
            {
                if (_sensor != null)
                {
                    return _sensor.ElevationAngle;
                }
                return 0;
            }
            set
            {
                if (value < _sensor?.MaxElevationAngle && value > _sensor.MinElevationAngle)
                    _sensor.ElevationAngle = value;
            }
        }

        #region IVideoSource Members

        public event NewFrameEventHandler NewFrame;

        public event PlayingFinishedEventHandler PlayingFinished;

        public virtual string Source
        {
            get { return _uniqueKinectId; }
            set { _uniqueKinectId = value; }
        }


        public bool IsRunning
        {
            get
            {
                if (_thread == null)
                    return false;

                try
                {
                    return !_thread.Join(TimeSpan.Zero);
                }
                catch
                {
                    return true;
                }
            }
        }

        public bool MousePointer;

        public void Start()
        {
            if (_sensor != null)
                Stop();

            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected && _uniqueKinectId == potentialSensor.UniqueKinectId)
                {
                    _sensor = potentialSensor;
                    break;
                }
            }
            if (_sensor==null)
            {
                Logger.LogMessage("Sensor not found: "+_uniqueKinectId,"KinectStream");
                return;
            }

            
            if (_skeleton)
            {
                _sensor.SkeletonStream.Enable();
                _sensor.SkeletonFrameReady += SensorSkeletonFrameReady;
            }

            switch (StreamMode)
            {
                case 0://color
                    _sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                    _sensor.ColorFrameReady += SensorColorFrameReady;
                    break;
                case 1://depth
                    _sensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                    _sensor.DepthFrameReady += SensorDepthFrameReady;
                    // Allocate space to put the depth pixels we'll receive
                    _depthPixels = new short[_sensor.DepthStream.FramePixelDataLength];
                    // Allocate space to put the color pixels we'll create
                    _colorPixels = new byte[_sensor.DepthStream.FramePixelDataLength * sizeof(int)];
                    break;
                case 2://infrared
                    _sensor.ColorStream.Enable(ColorImageFormat.InfraredResolution640x480Fps30);
                    _sensor.ColorFrameReady += SensorColorFrameReady;
                    break;
            }
            

            // Start the sensor
            try
            {
                _sensor.Start();
                _audioStream = _sensor.AudioSource.Start();

                RecordingFormat = new WaveFormat(16000, 16, 1);

                _waveProvider = new BufferedWaveProvider(RecordingFormat) { DiscardOnBufferOverflow = true, BufferDuration = TimeSpan.FromMilliseconds(500) };


                _sampleChannel = new SampleChannel(_waveProvider);
                _sampleChannel.PreVolumeMeter += SampleChannelPreVolumeMeter;

                if (HasAudioStream != null)
                {
                    HasAudioStream(this, EventArgs.Empty);
                    HasAudioStream = null;
                }

                _res = ReasonToFinishPlaying.DeviceLost;

                // create and start new thread
                _thread = new Thread(AudioThread) { Name = "kinect audio", IsBackground = true};
                _thread.Start();
            }
            catch (Exception ex)//IOException)
            {
                Logger.LogException(ex, "KinectStream");
                _sensor = null;
            }
        }

        private Thread _thread;

        void SampleChannelPreVolumeMeter(object sender, StreamVolumeEventArgs e)
        {
            LevelChanged?.Invoke(this, new LevelChangedEventArgs(e.MaxSampleValues));
        }

        private void AudioThread()
        {
            _abort = new ManualResetEvent(false);
            while (!_abort.WaitOne(0) && !MainForm.ShuttingDown)
            {
                int dataLength = _audioStream.Read(_audioBuffer, 0, _audioBuffer.Length);
                if (DataAvailable != null)
                {
                    _waveProvider.AddSamples(_audioBuffer, 0, dataLength);

                    if (Listening)
                    {
                        WaveOutProvider.AddSamples(_audioBuffer, 0, dataLength);
                    }

                    //forces processing of volume level without piping it out
                    var sampleBuffer = new float[dataLength];
                    int read = _sampleChannel.Read(sampleBuffer, 0, dataLength);

                    DataAvailable?.Invoke(this, new DataAvailableEventArgs((byte[])_audioBuffer.Clone(),read));
                }
            }


            try
            {
                if (_sensor != null)
                {
                    _sensor.AudioSource?.Stop();

                    _sensor.Stop();
                    _sensor.SkeletonFrameReady -= SensorSkeletonFrameReady;
                    _sensor.ColorFrameReady -= SensorColorFrameReady;
                    _sensor.DepthFrameReady -= SensorDepthFrameReady;

                    _sensor.Dispose();

                    _sensor = null;
                }
            }
            catch
            {
                // ignored
            }

            if (_sampleChannel!=null)
                _sampleChannel.PreVolumeMeter -= SampleChannelPreVolumeMeter;

            if (_waveProvider != null && _waveProvider.BufferedBytes > 0)
                _waveProvider.ClearBuffer();

            Listening = false;

            PlayingFinished?.Invoke(this, new PlayingFinishedEventArgs(_res));
            _abort.Close();
        }

        void SensorDepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {

            using (DepthImageFrame depthFrame = e.OpenDepthImageFrame())
            {
                if (depthFrame != null)
                {
                    if (ShouldEmitFrame)
                    {
                        // Copy the pixel data from the image to a temporary array
                        depthFrame.CopyPixelDataTo(_depthPixels);

                        // Convert the depth to RGB
                        int colorPixelIndex = 0;
                        foreach (short t in _depthPixels)
                        {
                            // discard the portion of the depth that contains only the player index
                            short depth = (short) (t >> DepthImageFrame.PlayerIndexBitmaskWidth);

                            // to convert to a byte we're looking at only the lower 8 bits
                            // by discarding the most significant rather than least significant data
                            // we're preserving detail, although the intensity will "wrap"
                            // add 1 so that too far/unknown is mapped to black
                            byte intensity = (byte) ((depth + 1) & byte.MaxValue);

                            // Write out blue byte
                            _colorPixels[colorPixelIndex++] = intensity;

                            // Write out green byte
                            _colorPixels[colorPixelIndex++] = intensity;

                            // Write out red byte                        
                            _colorPixels[colorPixelIndex++] = intensity;

                            // We're outputting BGR, the last byte in the 32 bits is unused so skip it
                            // If we were outputting BGRA, we would write alpha here.
                            ++colorPixelIndex;
                        }

                        // Write the pixel data into our bitmap

                        var bmap = new Bitmap(
                            depthFrame.Width,depthFrame.Height,PixelFormat.Format32bppRgb);

                        BitmapData bmapdata = bmap.LockBits(
                            new Rectangle(0, 0,depthFrame.Width, depthFrame.Height),
                            ImageLockMode.WriteOnly,
                            bmap.PixelFormat);

                        var ptr = bmapdata.Scan0;

                        Marshal.Copy(_colorPixels, 0, ptr,_colorPixels.Length);

                        bmap.UnlockBits(bmapdata);
                        
                        using (Graphics g = Graphics.FromImage(bmap))
                        {
                            lock (_skeletons)
                            {
                                foreach (Skeleton skel in _skeletons)
                                {
                                    DrawBonesAndJoints(skel, g);
                                }
                            }
                            if (_tripwires)
                            {
                                foreach (var dl in TripWires)
                                {
                                    g.DrawLine(TripWirePen, dl.StartPoint, dl.EndPoint);
                                }
                            }
                        }
                        // notify client
                        NewFrame?.Invoke(this, new NewFrameEventArgs(bmap));
                        // release the image
                        bmap.Dispose();
                    }
                }
            }
        }

        void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            lock (_skeletons)
            {
                using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
                {
                    if (skeletonFrame != null)
                    {
                        _skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                        skeletonFrame.CopySkeletonDataTo(_skeletons);
                    }
                }
            }
        }

        void SensorColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            if (ShouldEmitFrame)
            {
                using (ColorImageFrame imageFrame = e.OpenColorImageFrame())
                {
                    if (imageFrame != null)
                    {
                        Bitmap bmap;
                        switch (imageFrame.Format)
                        {
                            default:
                                bmap = ColorImageToBitmap(imageFrame);
                                break;
                            case ColorImageFormat.InfraredResolution640x480Fps30:
                                bmap = GrayScaleImageToBitmap(imageFrame);
                                break;
                        }

                        if (bmap != null)
                        {
                            using (Graphics g = Graphics.FromImage(bmap))
                            {
                                lock (_skeletons)
                                {
                                    foreach (Skeleton skel in _skeletons)
                                    {
                                        DrawBonesAndJoints(skel, g);
                                    }
                                }
                                if (_tripwires)
                                {
                                    foreach (var dl in TripWires)
                                    {
                                        g.DrawLine(TripWirePen, dl.StartPoint, dl.EndPoint);
                                    }
                                }
                            }
                            // notify client
                            NewFrame?.Invoke(this, new NewFrameEventArgs(bmap));
                            // release the image
                            bmap.Dispose();
                        }
                    }
                }
            }

        }

        void DrawBonesAndJoints(Skeleton skeleton, Graphics g)
        {
            // Render Torso
            DrawBone(skeleton, g, JointType.Head, JointType.ShoulderCenter);
            DrawBone(skeleton, g, JointType.ShoulderCenter, JointType.ShoulderLeft);
            DrawBone(skeleton, g, JointType.ShoulderCenter, JointType.ShoulderRight);
            DrawBone(skeleton, g, JointType.ShoulderCenter, JointType.Spine);
            DrawBone(skeleton, g, JointType.Spine, JointType.HipCenter);
            DrawBone(skeleton, g, JointType.HipCenter, JointType.HipLeft);
            DrawBone(skeleton, g, JointType.HipCenter, JointType.HipRight);

            // Left Arm
            DrawBone(skeleton, g, JointType.ShoulderLeft, JointType.ElbowLeft);
            DrawBone(skeleton, g, JointType.ElbowLeft, JointType.WristLeft);
            DrawBone(skeleton, g, JointType.WristLeft, JointType.HandLeft);

            // Right Arm
            DrawBone(skeleton, g, JointType.ShoulderRight, JointType.ElbowRight);
            DrawBone(skeleton, g, JointType.ElbowRight, JointType.WristRight);
            DrawBone(skeleton, g, JointType.WristRight, JointType.HandRight);

            // Left Leg
            DrawBone(skeleton, g, JointType.HipLeft, JointType.KneeLeft);
            DrawBone(skeleton, g, JointType.KneeLeft, JointType.AnkleLeft);
            DrawBone(skeleton, g, JointType.AnkleLeft, JointType.FootLeft);

            // Right Leg
            DrawBone(skeleton, g, JointType.HipRight, JointType.KneeRight);
            DrawBone(skeleton, g, JointType.KneeRight, JointType.AnkleRight);
            DrawBone(skeleton, g, JointType.AnkleRight, JointType.FootRight);

            // Render Joints
            if (_skeleton)
            {
                foreach (Joint joint in skeleton.Joints)
                {
                    Brush drawBrush = null;

                    if (joint.TrackingState == JointTrackingState.Tracked)
                    {
                        drawBrush = _trackedJointBrush;
                    }
                    else if (joint.TrackingState == JointTrackingState.Inferred)
                    {
                        drawBrush = _inferredJointBrush;
                    }

                    if (drawBrush != null)
                    {
                        var p = SkeletonPointToScreen(joint.Position);
                        g.FillEllipse(drawBrush, p.X, p.Y, JointThickness, JointThickness);
                    }
                }
            }
        }

        private void DrawBone(Skeleton skeleton, Graphics g, JointType jointType0, JointType jointType1)
        {
            Joint joint0 = skeleton.Joints[jointType0];
            Joint joint1 = skeleton.Joints[jointType1];

            // If we can't find either of these joints, exit
            if (joint0.TrackingState == JointTrackingState.NotTracked ||
                joint1.TrackingState == JointTrackingState.NotTracked)
            {
                return;
            }

            // Don't draw if both points are inferred
            if (joint0.TrackingState == JointTrackingState.Inferred &&
                joint1.TrackingState == JointTrackingState.Inferred)
            {
                return;
            }

            // We assume all drawn bones are inferred unless BOTH joints are tracked
            Pen drawPen = _inferredBonePen;
            if (joint0.TrackingState == JointTrackingState.Tracked && joint1.TrackingState == JointTrackingState.Tracked)
            {
                drawPen = _trackedBonePen;
            }

            Point p1 = SkeletonPointToScreen(joint0.Position);
            Point p2 = SkeletonPointToScreen(joint1.Position);

            if (_skeleton)
            {
                g.DrawLine(drawPen, p1, p2);
            }


            if (_tripwires && TripWire != null)
            {
                if ((from t in TripWires let dl = t where joint1.Position.Z * 1000 >= dl.DepthMin && joint1.Position.Z * 1000 <= dl.DepthMax select t).Any(t => ProcessIntersection(p1, p2, t)))
                {
                    if ((DateTime.UtcNow - _lastWarnedTripWire).TotalSeconds > 5)
                    {
                        TripWire(this, EventArgs.Empty);
                        _lastWarnedTripWire = DateTime.UtcNow;
                    }
                }
            }
        }

        public void InitTripWires(String cfg)
        {
            TripWires.Clear();
            if (!string.IsNullOrEmpty(cfg))
            {
                try
                {
                    var tw = cfg.Trim().Split(';');
                    foreach (string t in tw)
                    {
                        var twe = t.Split(',');
                        if (!string.IsNullOrEmpty(twe[0]))
                        {
                            var sp = new Point(Convert.ToInt32(twe[0]), Convert.ToInt32(twe[1]));
                            var ep = new Point(Convert.ToInt32(twe[2]), Convert.ToInt32(twe[3]));
                            int dmin = Convert.ToInt32(twe[4]);
                            int dmax = Convert.ToInt32(twe[5]);
                            TripWires.Add(new DepthLine(sp, ep, dmin, dmax));
                        }
                    }
                }
                catch (Exception)
                {
                    TripWires.Clear();
                }
            }
        }

        public event TripWireEventHandler TripWire;
        public delegate void TripWireEventHandler(object sender, EventArgs e);
        public List<DepthLine> TripWires = new List<DepthLine>();


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
            if (!IsRunning)
                return;

            _res = ReasonToFinishPlaying.Restart;
            _abort?.Set();

        }

        #endregion

        /// <summary>
        /// Maps a SkeletonPoint to lie within our render space and converts to Point
        /// </summary>
        /// <param name="skelpoint">point to map</param>
        /// <returns>mapped point</returns>
        private Point SkeletonPointToScreen(SkeletonPoint skelpoint)
        {
            // Convert point to depth space.  
            // We are not using depth directly, but we do want the points in our 640x480 output resolution.

            DepthImagePoint depthPoint = _sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(skelpoint, DepthImageFormat.Resolution640x480Fps30);
            return new Point(depthPoint.X, depthPoint.Y);
        }

        static Bitmap ColorImageToBitmap(
                     ColorImageFrame image)
        {
            try
            {
                if (image != null)
                {
                    var pixeldata =
                        new byte[image.PixelDataLength];
                    
                    image.CopyPixelDataTo(pixeldata);

                    var bitmapFrame = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppRgb);

                    BitmapData bmapdata = bitmapFrame.LockBits(
                        new Rectangle(0, 0,
                                      image.Width, image.Height),
                        ImageLockMode.WriteOnly,
                        bitmapFrame.PixelFormat);
                    var ptr = bmapdata.Scan0;
                    Marshal.Copy(pixeldata, 0, ptr,
                                 image.PixelDataLength);
                    bitmapFrame.UnlockBits(bmapdata);
                    
                    return bitmapFrame;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "KinectStream");
            }
            return null;
        }

         static Bitmap GrayScaleImageToBitmap(ColorImageFrame image)
        {
            try
            {
                if (image != null)
                {
                    var pixeldata =
                        new byte[image.PixelDataLength];
                    
                    image.CopyPixelDataTo(pixeldata);

                    var bitmapFrame = new Bitmap(image.Width, image.Height, PixelFormat.Format16bppGrayScale);

                    BitmapData bmapdata = bitmapFrame.LockBits(
                        new Rectangle(0, 0,
                                      image.Width, image.Height),
                        ImageLockMode.WriteOnly,
                        bitmapFrame.PixelFormat);
                    var ptr = bmapdata.Scan0;
                    Marshal.Copy(pixeldata, 0, ptr,
                                 image.PixelDataLength);
                    bitmapFrame.UnlockBits(bmapdata);
                    
                    var  filter = new GrayscaleToRGB();
                    return filter.Apply(AForge.Imaging.Image.Convert16bppTo8bpp(bitmapFrame));
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "KinectStream");
            }
            return null;
        }


        private static bool ProcessIntersection(Point a, Point b, DepthLine dl)
        {
            var c = dl.StartPoint;
            var d = dl.EndPoint;

            float ua = (d.X - c.X) * (a.Y - c.Y) - (d.Y - c.Y) * (a.X - c.X);
            float ub = (b.X - a.X) * (a.Y - c.Y) - (b.Y - a.Y) * (a.X - c.X);
            float denominator = (d.Y - c.Y) * (b.X - a.X) - (d.X - c.X) * (b.Y - a.Y);

            //bool intersection, coincident;

            if (Math.Abs(denominator) <= 0.00001f)
            {
                if (Math.Abs(ua) <= 0.00001f && Math.Abs(ub) <= 0.00001f)
                {
                    return true;
                    //intersection = coincident = true;
                    //intersectionPoint = (A + B) / 2;
                }
            }
            else
            {
                ua /= denominator;
                ub /= denominator;

                if (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1)
                {
                    return true;
                    //intersection = true;
                    //intersectionPoint.X = A.X + ua * (B.X - A.X);
                    //intersectionPoint.Y = A.Y + ua * (B.Y - A.Y);
                }
            }
            return false;
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
                _inferredBonePen.Dispose();
                _trackedBonePen.Dispose();
                _trackedJointBrush.Dispose();
                _inferredJointBrush.Dispose();
                TripWirePen.Dispose();
            }

            // Free any unmanaged objects here. 
            //
            _disposed = true;
        }

    }
}