using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Sockets;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using iSpy.Video.FFMPEG;
using iSpyApplication.Cloud;
using iSpyApplication.Server;
using iSpyApplication.Sources;
using iSpyApplication.Sources.Audio;
using iSpyApplication.Sources.Video;
using iSpyApplication.Sources.Video.Ximea;
using iSpyApplication.Vision;
using iSpyPRO.DirectShow;
using iSpyPRO.DirectShow.Internals;
using xiApi.NET;
using Encoder = System.Drawing.Imaging.Encoder;
using Image = System.Drawing.Image;

namespace iSpyApplication.Controls
{
    /// <summary>
    /// Summary description for CameraWindow.
    /// </summary>
    public sealed class CameraWindow : PictureBox, ISpyControl
    {
        #region Private

        internal MainForm MainClass;
        internal DateTime LastAutoTrackSent = DateTime.MinValue;
        private Color _customColor = Color.Black;
        private DateTime _lastRedraw = DateTime.MinValue;
        private DateTime _recordingStartTime;
        private readonly ManualResetEvent _stopWrite = new ManualResetEvent(false);
        private double _autoofftimer;
        private bool _raiseStop;
        private double _timeLapse;
        private double _timeLapseFrames;
        private double _timeLapseTotal;
        private double _timeLapseFrameCount;
        private double _secondCountNew;
        private Point _mouseLoc;
        public ConcurrentQueue<Helper.FrameAction> Buffer = new ConcurrentQueue<Helper.FrameAction>();
        private DateTime _errorTime = DateTime.MinValue;
        private DateTime _reconnectTime = DateTime.MinValue;
        private bool _firstFrame = true;
        private Thread _recordingThread;
        private int _calibrateTarget;
        private Camera _camera;
        private DateTime _lastFrameUploaded = Helper.Now;
        private DateTime _lastFrameSaved = Helper.Now;
        private DateTime _lastScheduleCheck = DateTime.MinValue;
        private DateTime _dtPTZLastCheck = DateTime.Now;
        private long _lastRun = Helper.Now.Ticks;

        private Int64 _lastMovementDetected = DateTime.MinValue.Ticks;
        private Int64 _lastAlerted = DateTime.MinValue.Ticks;

        public DateTime LastMovementDetected
        {
            get { return new DateTime(_lastMovementDetected); }
            set { Interlocked.Exchange(ref _lastMovementDetected, value.Ticks); }
        }

        public DateTime LastAlerted
        {
            get { return new DateTime(_lastAlerted); }
            set { Interlocked.Exchange(ref _lastAlerted, value.Ticks); }
        }

        private DateTime _mouseMove = DateTime.MinValue;
        private List<FilesFile> _filelist = new List<FilesFile>();
        private VideoFileWriter _timeLapseWriter;
        private readonly ToolTip _toolTipCam;
        private int _ttind = -1;
        private int _reconnectFailCount;
        public volatile bool IsReconnect;
        private bool _suspendPTZSchedule;
        private bool _requestRefresh;
        private readonly StringBuilder _motionData = new StringBuilder(100000);

        public volatile bool AbortedAudio;
        private const int ButtonCount = 8;
        private Rectangle ButtonPanel
        {
            get
            {
                int w = ButtonCount * 22 + 3;
                int h = 28;
                if (MainForm.Conf.BigButtons)
                {
                    w = ButtonCount * (31) + 3;
                    h = 34;
                    
                }
                return new Rectangle(Width / 2 - w / 2, Height - 25 - h, w, h);

            }
        }
        private Bitmap _lastFrame;
        private readonly object _lockobject = new object();
        #endregion

        internal bool Ptzneedsstop;
        internal bool IsClone;
        internal bool HasClones;

        private VideoFileWriter _writer;
        private bool _minimised;
        internal bool LoadedFiles;
        #region Public

        #region Events
        public event Delegates.RemoteCommandEventHandler RemoteCommand;
        public event Delegates.NotificationEventHandler Notification;
        public event Delegates.FileListUpdatedEventHandler FileListUpdated;
        public event Delegates.ErrorHandler ErrorHandler;
        #endregion

        private event EventHandler CameraReconnect, CameraDisabled, CameraEnabled, CameraReconnected;
        
        public bool Talking { get; set; }
        public bool IsEnabled { get; private set; }

        public bool Listening
        {
            get
            {
                var vc = VolumeControl;
                if (vc == null)
                    return false;
                return vc.Listening;
            }
        }
        public bool ForcedRecording;
        public bool NeedMotionZones = true;
        public XimeaVideoSource XimeaSource;
        public bool Alerted;
        public double MovementCount;
        public double CalibrateCount, ReconnectCount;
        public Rectangle RestoreRect = Rectangle.Empty;
        

        private bool _calibrating;
        public bool Calibrating
        {
            get { return _calibrating; }
            set
            {
                _calibrating = value;
                if (value)
                {
                    CalibrateCount = 0;
                    _calibrateTarget = Camobject.detector.calibrationdelay;
                }
            }
        }
        public Graphics CurrentFrame;
        public PTZController PTZ;
        public DateTime FlashCounter = DateTime.MinValue;
        public double InactiveRecord;
        public bool IsEdit;
        public volatile bool MovementDetected;
        public bool PTZNavigate;
        public Point PTZReference;
        public bool NeedSizeUpdate;
        public bool ResizeParent;
        public bool ShuttingDown;
        public string TimeLapseVideoFileName = "";
        public string VideoFileName = "";
        public string VideoSourceErrorMessage = "";
        public bool VideoSourceErrorState;
        public DateTime TimelapseStart = DateTime.MinValue;
        public objectsCamera Camobject;
        internal Color BackgroundColor;
        public bool Seekable;

        public VolumeLevel VolumeControl;
        //{
        //    get
        //    {
        //        if (Camobject != null && Camobject.settings.micpair > -1)
        //        {
        //            VolumeLevel vl = MainClass.GetVolumeLevel(Camobject.settings.micpair);
        //            if (vl != null && vl.Micobject != null)
        //                return vl;
                    
        //        }
        //        return null;
        //    }
        //}
        public bool Recording => _recordingThread != null && !_recordingThread.Join(TimeSpan.Zero);

        public string ObjectName => Camobject.name;

        public bool CanTalk => IsEnabled && Camobject.settings.audiomodel != "None";

        public bool CanListen => Camobject.settings.micpair>-1;

        public bool CanRecord => IsEnabled && Helper.HasFeature(Enums.Features.Recording);

        public bool CanGrab => IsEnabled && Helper.HasFeature(Enums.Features.Save_Frames);

        public bool HasFiles => true;

        public bool CanEnable => true;

        public bool SavingTimeLapse => _timeLapseWriter != null;


        private string CodecExtension
        {
            get
            {
                switch (Camobject.recorder.profile)
                {
                    default:
                        return ".mp4";
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                        return ".avi";
                }
            }
        }

        private int CodecFramerate
        {
            get
            {
                switch (Camobject.recorder.profile)
                {
                    default:
                        return 0;
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                        int i = Convert.ToInt32(Camera.Framerate);
                        if (i == 0)
                            return 1;
                        return i;
                }
            }
        }

        private VideoCodec Codec
        {
            get
            {
                switch (Camobject.recorder.profile)
                {
                    default:
                        return VideoCodec.H264;
                    case 3:
                        return VideoCodec.WMV1;
                    case 4:
                        return VideoCodec.WMV2;
                    case 5:
                        return VideoCodec.MPEG4;
                    case 6:
                        return VideoCodec.MSMPEG4v3;
                    case 7:
                        return VideoCodec.Raw;
                    case 8:
                        return VideoCodec.MJPEG;
                }
            }
        }

        private AudioCodec CodecAudio
        {
            get
            {
                switch (Camobject.recorder.profile)
                {
                    default:
                        return AudioCodec.AAC;
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                        return AudioCodec.MP3;
                }
            }
        }


        internal void GenerateFileList()
        {
            string dir = Dir.Entry + "video\\" +
                         Camobject.directory + "\\";

            if (!Directory.Exists(dir))
            {
                try
                {
                    Directory.CreateDirectory(dir);
                }
                catch (Exception ex)
                {
                    ErrorHandler?.Invoke(ex.Message);
                    _filelist = new List<FilesFile>();
                    FileListUpdated?.Invoke(this);
                    return;
                }
            }

            bool failed = false;
            if (File.Exists(dir + "data.xml"))
            {
                var s = new XmlSerializer(typeof(Files));
                try
                {
                    using (var fs = new FileStream(dir + "data.xml", FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        try
                        {
                            using (TextReader reader = new StreamReader(fs))
                            {
                                fs.Position = 0;
                                lock (_lockobject)
                                {
                                    var t = ((Files)s.Deserialize(reader));
                                    if (t.File == null || !t.File.Any())
                                    {
                                        _filelist = new List<FilesFile>();
                                    }
                                    else
                                        _filelist = t.File.ToList();
                                }
                                reader.Close();
                            }
                            ScanForMissingFiles();
                        }
                        catch (Exception ex)
                        {
                            ErrorHandler?.Invoke(ex.Message);
                            failed = true;
                        }
                        fs.Close();
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler?.Invoke(ex.Message);
                    failed = true;
                }
                if (!failed)
                {
                    FileListUpdated?.Invoke(this);
                    return;
                }

            }

            //else build from directory contents
            _filelist = new List<FilesFile>();
            lock (_lockobject)
            {
                var dirinfo = new DirectoryInfo(Dir.Entry + "video\\" +
                                                Camobject.directory + "\\");

                var lFi = new List<FileInfo>();
                lFi.AddRange(dirinfo.GetFiles());
                lFi = lFi.FindAll(f => f.Extension.ToLower() == ".avi" || f.Extension.ToLower() == ".mp4");
                lFi = lFi.OrderByDescending(f => f.CreationTime).ToList();

                //sanity check existing data

                foreach (FileInfo fi in lFi)
                {
                    FileInfo fi1 = fi;
                    if (_filelist.Count(p => p.Filename == fi1.Name) == 0)
                    {

                        _filelist.Add(new FilesFile
                        {
                            CreatedDateTicks = fi.CreationTime.Ticks,
                            Filename = fi.Name,
                            SizeBytes = fi.Length,
                            MaxAlarm = 0,
                            AlertData = "0",
                            DurationSeconds = 0,
                            IsTimelapse = fi.Name.ToLower().IndexOf("timelapse", StringComparison.Ordinal) != -1,
                            IsMergeFile = fi.Name.ToLower().IndexOf("merge", StringComparison.Ordinal) != -1
                        });
                    }
                }

                for (int index = 0; index < _filelist.Count; index++)
                {
                    FilesFile ff = _filelist[index];
                    if (ff != null && lFi.All(p => p.Name != ff.Filename))
                    {

                        _filelist.Remove(ff);

                        index--;
                    }
                }
                _filelist = _filelist.OrderByDescending(p => p.CreatedDateTicks).ToList();
            }

            FileListUpdated?.Invoke(this);
        }

        public void ClearFileList()
        {
            lock (_lockobject)
            {
                _filelist.Clear();
            }
            MainForm.MasterFileRemoveAll(2, Camobject.id);
        }

        public void RemoveFile(string filename)
        {
            lock (_lockobject)
            {
                _filelist.RemoveAll(p => p.Filename == filename);
            }
            MainForm.MasterFileRemove(filename);
        }

        public void SaveFileList()
        {
            try
            {
                lock (_lockobject)
                {
                    var fl = new Files { File = _filelist.ToArray() };
                    string dir = Dir.Entry + "video\\" +
                                 Camobject.directory + "\\";
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);
                    var s = new XmlSerializer(typeof (Files));
                    using (var fs = new FileStream(dir + "data.xml", FileMode.Create))
                    {
                        using (TextWriter writer = new StreamWriter(fs))
                        {
                            fs.Position = 0;
                            s.Serialize(writer, fl);
                            writer.Close();
                        }
                        fs.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler?.Invoke(ex.Message);
            }
        }

        private Thread _tScan;
        private void ScanForMissingFiles()
        {
            if (_tScan == null || _tScan.Join(TimeSpan.Zero))
            {
                _tScan = new Thread(ScanFiles);
                _tScan.Start();
            }
        }

        private void ScanFiles()
        {
            try
            {
                //check files exist
                var dir = Dir.Entry + "video\\" +
                                                  Camobject.directory + "\\";
                var dirinfo = new DirectoryInfo(dir);

                var lFi = new List<FileInfo>();
                lFi.AddRange(dirinfo.GetFiles());
                lFi = lFi.FindAll(f => f.Extension.ToLower() == ".avi" || f.Extension.ToLower() == ".mp4");
                lFi = lFi.OrderByDescending(f => f.CreationTime).ToList();

                //var farr = _filelist.ToArray();
                lock (_lockobject)
                {
                    for (int j = 0; j < _filelist.Count; j++)
                    {
                        var t = _filelist[j];
                        if (t != null)
                        {
                            var fe = lFi.FirstOrDefault(p => p.Name == t.Filename);
                            if (fe == null)
                            {
                                //file not found
                                _filelist.RemoveAt(j);
                                j--;
                                continue;
                            }
                            lFi.Remove(fe);
                        }
                    }
                    //add missing files
                    foreach (var fi in lFi)
                    {
                        _filelist.Add(new FilesFile
                        {
                            CreatedDateTicks = fi.CreationTime.Ticks,
                            Filename = fi.Name,
                            SizeBytes = fi.Length,
                            MaxAlarm = 0,
                            AlertData = "0",
                            DurationSeconds = 0,
                            IsTimelapse = fi.Name.ToLower().IndexOf("timelapse", StringComparison.Ordinal) != -1,
                            IsMergeFile = fi.Name.ToLower().IndexOf("merge", StringComparison.Ordinal) != -1
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler?.Invoke(ex.Message);
            }
            FileListUpdated?.Invoke(this);
        }
        #endregion

        #region SizingControls

        public void UpdatePosition(int width, int height)
        {
            Monitor.Enter(this);

            Camobject.resolution = width + "x" + height;
            SuspendLayout();

            //resize to max 640xh
            if (width > 640)
            {
                double d = width / 640d;
                width = 640;
                height = Convert.ToInt32(Convert.ToDouble(height) / d);
            }
            Camobject.width = width;
            Camobject.height = height;
            Size = new Size(width + 2, height + 26);
            ResumeLayout();
            NeedSizeUpdate = false;
                
            Monitor.Exit(this);
        }
        #endregion



        public CameraWindow(objectsCamera cam, MainForm mainForm)
        {
            InitializeComponent();
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint, true);
            Margin = new Padding(0, 0, 0, 0);
            Padding = new Padding(0, 0, 5, 5);
            BorderStyle = BorderStyle.None;
            BackgroundColor = MainForm.BackgroundColor;
            Camobject = cam;
            PTZ = new PTZController(this);
            MainClass = mainForm;
            ErrorHandler += CameraWindow_ErrorHandler;

            _toolTipCam = new ToolTip { AutomaticDelay = 500, AutoPopDelay = 1500 };
        }

        void CameraWindow_ErrorHandler(string message)
        {
            MainForm.LogErrorToFile(Camobject.name+": "+message);
        }

        private Thread _tFiles;
        public void GetFiles()
        {
            if (_tFiles == null || _tFiles.Join(TimeSpan.Zero))
            {
                _tFiles = new Thread(GenerateFileList);
                _tFiles.Start();
            }
        }

        public Camera Camera
        {
            get { return _camera; }
            set
            {
                _camera?.Dispose();

                _camera = value;
                if (_camera != null)
                {
                    _camera.CW = this;
                    ClearBuffer();
                }
            }
        }

        public string[] ScheduleDetails
        {
            get
            {
                var entries = new List<string>();
                foreach (objectsCameraScheduleEntry sched in Camobject.schedule.entries)
                {
                    string daysofweek = sched.daysofweek;
                    daysofweek = daysofweek.Replace("0", LocRm.GetString("Sun"));
                    daysofweek = daysofweek.Replace("1", LocRm.GetString("Mon"));
                    daysofweek = daysofweek.Replace("2", LocRm.GetString("Tue"));
                    daysofweek = daysofweek.Replace("3", LocRm.GetString("Wed"));
                    daysofweek = daysofweek.Replace("4", LocRm.GetString("Thu"));
                    daysofweek = daysofweek.Replace("5", LocRm.GetString("Fri"));
                    daysofweek = daysofweek.Replace("6", LocRm.GetString("Sat"));

                    string s = sched.start + " -> " + sched.stop + " (" + daysofweek + ")";
                    if (sched.recordonstart)
                        s += " " + LocRm.GetString("Record").ToUpper();
                    if (sched.alerts)
                        s += " " + LocRm.GetString("Alert").ToUpper();
                    if (sched.recordondetect)
                        s += " " + LocRm.GetString("Detect").ToUpper();
                    if (sched.timelapseenabled)
                        s += " " + LocRm.GetString("Timelapse").ToUpper();
                    if (sched.ptz)
                        s += " " + LocRm.GetString("PTZScheduler").ToUpper();
                    if (sched.messaging)
                        s += " " + LocRm.GetString("Messaging").ToUpper();
                    if (!sched.active)
                        s += " (" + LocRm.GetString("Inactive").ToUpper() + ")";

                    entries.Add(s);
                }
                return entries.ToArray();
            }
        }

        #region MouseEvents

        private MousePos GetMousePos(Point location)
        {
            var result = MousePos.NoWhere;
            int rightSize = Padding.Right;
            int bottomSize = Padding.Bottom;
            var testRect = new Rectangle(Width - rightSize, 0, Width - rightSize, Height - bottomSize);
            if (testRect.Contains(location)) result = MousePos.Right;
            testRect = new Rectangle(0, Height - bottomSize, Width - rightSize, Height);
            if (testRect.Contains(location)) result = MousePos.Bottom;
            testRect = new Rectangle(Width - rightSize, Height - bottomSize, Width, Height);
            if (testRect.Contains(location)) result = MousePos.BottomRight;
            return result;
        }

        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                case Keys.Up:
                case Keys.Left:
                case Keys.Right:
                    e.IsInputKey = true;
                    break;
            }
            base.OnPreviewKeyDown(e);
        }

        private bool _keyPTZHandled;
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (Camera == null)
                return;
            Rectangle r = Camera.ViewRectangle;
            if (r != Rectangle.Empty)
            {
                _keyPTZHandled = true;
                var cmd = Enums.PtzCommand.Center;
                switch (e.KeyCode)
                {
                    default:
                        _keyPTZHandled = false;
                        break;
                    case Keys.Add:
                        cmd = Enums.PtzCommand.ZoomIn;
                        break;
                    case Keys.Subtract:
                        cmd = Enums.PtzCommand.ZoomOut;
                        break;
                    case Keys.Left:
                        cmd = Enums.PtzCommand.Left;
                        break;
                    case Keys.Right:
                        cmd = Enums.PtzCommand.Right;
                        break;
                    case Keys.Up:
                        cmd = Enums.PtzCommand.Up;
                        break;
                    case Keys.Down:
                        cmd = Enums.PtzCommand.Down;
                        break;
                }
                if (_keyPTZHandled)
                {
                    Calibrating = true;
                    PTZ.SendPTZCommand(cmd);
                }
            }
            base.OnKeyDown(e);
        }


        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (_keyPTZHandled)
            {
                PTZ.SendPTZCommand(Enums.PtzCommand.Stop);
            }
            base.OnKeyUp(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if ((e.Location.X >= 0) && (e.Location.X <= Size.Width) &&
            (e.Location.Y >= 0) && (e.Location.Y <= Size.Height))
            {
                base.OnMouseWheel(e);
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {

            base.OnMouseUp(e);

            switch (e.Button)
            {
                case MouseButtons.Left:
                    MousePos mousePos = GetMousePos(e.Location);

                    if (mousePos == MousePos.NoWhere)
                    {
                        if (MainForm.Conf.ShowOverlayControls)
                        {
                            if (Seekable && _seeking && Camera?.VideoSource != null)
                            {
                                //seek video bar
                                var pc = (float)(Convert.ToDouble(_newSeek) / Convert.ToDouble(ButtonPanel.Width));
                                var vlc = Camera.VideoSource as VlcStream;
                                vlc?.Seek(pc);
                            }
                            _seeking = false;
                            _newSeek = 0;
                        }
                    }
                    break;
                case MouseButtons.Middle:
                    PTZNavigate = false;
                    PTZSettings2Camera ptz = MainForm.PTZs.SingleOrDefault(p => p.id == Camobject.ptz);
                    if (!string.IsNullOrEmpty(ptz?.Commands.Stop))
                        PTZ.SendPTZCommand(ptz.Commands.Stop, true);

                    if (PTZ.IsContinuous)
                        PTZ.SendPTZCommand(Enums.PtzCommand.Stop);
                    break;
            }

        }

        private float _newSeek;
        private bool _seeking;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            _seeking = false;
            base.OnMouseDown(e);
            Select();
            IntPtr hwnd = Handle;
            if (ResizeParent && Parent != null && Parent.IsHandleCreated)
            {
                hwnd = Parent.Handle;
            }
            switch (e.Button)
            {
                    case MouseButtons.Left:
                        MousePos mousePos = GetMousePos(e.Location);
                        if (mousePos == MousePos.NoWhere)
                        {
                            if (MainForm.Conf.ShowOverlayControls)
                            {
                                int bpi = GetButtonIndexByLocation(e.Location);
                                switch (bpi)
                                {
                                    case -1:
                                        if (Seekable && Camera?.VideoSource != null)
                                        {
                                            _seeking = true;
                                            return;
                                        }
                                        break;
                                    case 0:
                                        if (IsEnabled)
                                        {
                                            Disable();
                                        }
                                        else
                                        {
                                            Enable();
                                        }
                                        break;
                                    case 1:
                                        if (IsEnabled)
                                        {
                                            RecordSwitch(!(Recording || ForcedRecording));
                                        }
                                        break;
                                    case 2:
                                        if (Helper.HasFeature(Enums.Features.Edit))
                                        {
                                            MainClass.EditCamera(Camobject);
                                        }
                                        break;
                                    case 3:
                                        if (Helper.HasFeature(Enums.Features.Access_Media))
                                        {
                                            string url = MainForm.Webserver + "/watch_new.aspx";
                                            if (WsWrapper.WebsiteLive && MainForm.Conf.ServicesEnabled)
                                            {
                                                MainForm.OpenUrl(url);
                                            }
                                            else
                                                MainClass.Connect(url, false);
                                        }
                                        break;
                                    case 4:
                                        if (IsEnabled)
                                        {
                                            Snapshot();
                                        }
                                        break;
                                    case 5:
                                        Talk();
                                        break;
                                    case 6:
                                        TextToSpeech();
                                        break;
                                    case 7:
                                        Listen();
                                        break;
                                }
                            }
                        }

                        if (MainForm.Conf.LockLayout) return;
                        switch (mousePos)
                        {
                            case MousePos.Right:
                                {
                                    NativeCalls.ReleaseCapture(hwnd);
                                    NativeCalls.SendMessage(hwnd, NativeCalls.WmSyscommand, NativeCalls.ScDragsizeE, IntPtr.Zero);
                                }
                                break;
                            case MousePos.Bottom:
                                {
                                    NativeCalls.ReleaseCapture(hwnd);
                                    NativeCalls.SendMessage(hwnd, NativeCalls.WmSyscommand, NativeCalls.ScDragsizeS, IntPtr.Zero);
                                }
                                break;
                            case MousePos.BottomRight:
                                {
                                    NativeCalls.ReleaseCapture(hwnd);
                                    NativeCalls.SendMessage(hwnd, NativeCalls.WmSyscommand, NativeCalls.ScDragsizeSe,
                                                            IntPtr.Zero);
                                }
                                break;
                        }
                    break;
                    case MouseButtons.Middle:
                        PTZReference = new Point(Width / 2, Height / 2);
                        PTZNavigate = true;
                    break;
            }
        }

        public void Talk(IWin32Window f = null)
        {
            if (Camobject.settings.audiomodel == "None")
            {
                IWin32Window obj = this;
                if (f != null)
                    obj = f;
                if (!InvokeRequired)
                {
                    MessageBox.Show(obj, LocRm.GetString("ConfigureTalk"));
                    if (Helper.HasFeature(Enums.Features.Edit))
                        MainClass.EditCamera(Camobject, f);
                }
            }
            else
            {
                Talking = !Talking;
                MainClass.TalkTo(this, Talking);
                LogToPlugin(Talking ? "Talking Started" : "Talking Finished");
            }
        }

        public void TextToSpeech(IWin32Window f = null)
        {
            if (!InvokeRequired)
            {
                IWin32Window obj = this;
                if (f != null)
                    obj = f;

                if (Camobject.settings.audiomodel == "None")
                {
                    MessageBox.Show(obj, LocRm.GetString("ConfigureTalk"));
                    if (Helper.HasFeature(Enums.Features.Edit))
                        MainClass.EditCamera(Camobject, f);
                }
                else
                {
                    var t = new TextToSpeech(this);
                    t.Show(obj);
                }
            }
        }


        public void Listen()
        {
            if (VolumeControl != null && VolumeControl.IsEnabled)
            {
                VolumeControl.Listening = !VolumeControl.Listening;

                LogToPlugin(VolumeControl.Listening ? "Listening Started" : "Listening Finished");
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            _requestRefresh = true;
            base.OnLostFocus(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            MainForm.InstanceReference.PTZToolUpdate(this);
            MainForm.InstanceReference.LastFocussedControl = this;
            _requestRefresh = true;
            base.OnGotFocus(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_mouseLoc.X == e.X && _mouseLoc.Y == e.Y)
                return;
            _mouseLoc.X = e.X;
            _mouseLoc.Y = e.Y;
            _mouseMove = Helper.Now;
            MousePos mousePos = GetMousePos(e.Location);
            switch (mousePos)
            {
                case MousePos.Right:
                    Cursor = Cursors.SizeWE;
                    break;
                case MousePos.Bottom:
                    Cursor = Cursors.SizeNS;
                    break;
                case MousePos.BottomRight:
                    Cursor = Cursors.SizeNWSE;
                    break;
                default:
                    Cursor = Cursors.Hand;
                    if (_toolTipCam.Active)
                    {
                        _toolTipCam.Hide(this);
                        _ttind = -1;
                    }
                    if (e.Location.X < 30 && e.Location.Y > Height - 24)
                    {
                        string m = "";
                        if (Camobject.alerts.active)
                            m = LocRm.GetString("AlertsActive");

                        if (ForcedRecording)
                            m = LocRm.GetString("ForcedRecording")+", " + m;

                        if (Camobject.detector.recordondetect)
                            m = LocRm.GetString("RecordOnDetect")+", " + m;
                        else
                        {
                            if (Camobject.detector.recordonalert)
                                m = LocRm.GetString("RecordOnAlert")+", " + m;
                            else
                            {
                                m = LocRm.GetString("NoRecording") + ", " + m;
                            }
                        }
                        if (Camobject.schedule.active)
                            m = LocRm.GetString("Scheduled") + ", " + m;

                        var toolTipLocation = new Point(5, Height - 24);
                        _toolTipCam.Show(m, this, toolTipLocation, 1000);
                    }
                    if (MainForm.Conf.ShowOverlayControls)
                    {
                        var rBp = ButtonPanel;
                        
                        if (_seeking && Seekable && Camera?.VideoSource != null)
                        {
                            _newSeek = e.Location.X - rBp.X;
                            if (_newSeek < 0.0001) _newSeek = 0.0001f;
                            if (_newSeek > rBp.Width)
                                _newSeek = rBp.Width;
                            return;
                        }

                        var toolTipLocation = new Point(e.Location.X, rBp.Y + rBp.Height + 1);
                        int bpi = GetButtonIndexByLocation(e.Location);
                        if (_ttind != bpi)
                        {
                            switch (bpi)
                            {
                                case 0:
                                    _toolTipCam.Show(IsEnabled? LocRm.GetString("switchOff"): LocRm.GetString("Switchon"), this, toolTipLocation, 1000);
                                    _ttind = 0;
                                    break;
                                case 1:
                                    if (Helper.HasFeature(Enums.Features.Recording))
                                    {
                                        _toolTipCam.Show(LocRm.GetString("RecordNow"), this, toolTipLocation, 1000);
                                        _ttind = 1;
                                    }
                                    break;
                                case 2:
                                    _toolTipCam.Show(LocRm.GetString("Edit"), this, toolTipLocation, 1000);
                                    _ttind = 2;
                                    break;
                                case 3:
                                    if (Helper.HasFeature(Enums.Features.Access_Media))
                                    {
                                        _toolTipCam.Show(LocRm.GetString("MediaoverTheWeb"), this, toolTipLocation, 1000);
                                        _ttind = 3;
                                    }
                                    break;
                                case 4:
                                    if (Helper.HasFeature(Enums.Features.Save_Frames))
                                    {
                                        _toolTipCam.Show(LocRm.GetString("TakePhoto"), this, toolTipLocation, 1000);
                                        _ttind = 4;
                                    }
                                    break;
                                case 5:
                                    if (_ttind != 5)
                                    {
                                        _toolTipCam.Show(LocRm.GetString("Talk"), this, toolTipLocation, 1000);
                                        _ttind = 5;
                                    }
                                    break;
                                case 6:
                                    if (_ttind != 6)
                                    {
                                        _toolTipCam.Show(LocRm.GetString("Text"), this, toolTipLocation, 1000);
                                        _ttind = 6;
                                    }
                                    break;
                                case 7:
                                    if (_ttind != 7)
                                    {
                                        _toolTipCam.Show(Listening
                                        ? LocRm.GetString("StopListening")
                                        : LocRm.GetString("Listen"), this, toolTipLocation, 1000);
                                        _ttind = 7;
                                    }
                                    break;
                            }
                        }
                    }
                    break;
            }

            base.OnMouseMove(e);

            _requestRefresh = true;

        }

        protected override void OnResize(EventArgs eventargs)
        {

            if ((ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                if (Camera != null)
                {
                    double arW = Convert.ToDouble(Camera.Width) / Convert.ToDouble(Camera.Height);
                    Width = Convert.ToInt32(arW * Height);
                }
            }
            base.OnResize(eventargs);

            if (Width < MinimumSize.Width) Width = MinimumSize.Width;
            if (Height < MinimumSize.Height) Height = MinimumSize.Height;
            if (VolumeControl != null)
                MainForm.NeedsRedraw = true;

            _minimised = Size.Equals(MinimumSize);

        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            Cursor = Cursors.Default;
            _mouseMove = DateTime.MinValue;
            _requestRefresh = true;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            Cursor = Cursors.Hand;
            _requestRefresh = true;
        }

        private enum MousePos
        {
            NoWhere,
            Right,
            Bottom,
            BottomRight
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Invalidate();
            }
            LocationChanged -= CameraWindowLocationChanged;
            Resize-=CameraWindowResize;

            _toolTipCam.RemoveAll();
            _toolTipCam.Dispose();
            _timeLapseWriter = null;
            _writer = null;
            base.Dispose(disposing);
        }

        private double _tickThrottle;
        public void Tick()
        {
            //reset custom border color
            if (Camobject.settings.bordertimeout != 0 && _customSet != DateTime.MinValue &&
                _customSet < Helper.Now.AddSeconds(0 - Camobject.settings.bordertimeout))
            {
                Custom = false;
            }


            try
            {
                //time since last tick
                var ts = new TimeSpan(Helper.Now.Ticks - _lastRun);
                _lastRun = Helper.Now.Ticks;
                _secondCountNew = ts.TotalMilliseconds / 1000.0;

                if (Camobject.schedule.active)
                {
                    if (CheckSchedule()) goto skip;
                }

                if (!IsEnabled) goto skip;

                if (Camera!=null)
                    MovementDetected = Camera.MotionRecentlyDetected;
                
                if (FlashCounter > DateTime.MinValue)
                {
                    double iFc = (FlashCounter - Helper.Now).TotalSeconds;

                    if (iFc <= 2)
                    {
                        if (_suspendPTZSchedule)
                        {
                            _suspendPTZSchedule = false;
                            _dtPTZLastCheck = DateTime.Now;
                        }
                    }

                    if (MovementDetected)
                    {
                        _autoofftimer = 0;
                        InactiveRecord = 0;
                        if (Camobject.alerts.mode != "nomovement" &&
                            (Camobject.detector.recordondetect || Camobject.detector.recordonalert))
                        {
                            var vc = VolumeControl;
                            if (vc != null)
                            {
                                vc.InactiveRecord = 0;
                            }
                        }
                    }

                    if (iFc < 1)
                    {
                        UpdateFloorplans(false);
                        FlashCounter = DateTime.MinValue;
                        if (_raiseStop)
                        {
                            DoAlert("alertstopped");
                            _raiseStop = false;
                        }
                    }
                }

                if (!MovementDetected && Camobject.detector.autooff>0)
                {
                    _autoofftimer += ts.TotalMilliseconds / 1000.0;
                    if (_autoofftimer > Camobject.detector.autooff)
                    {
                        Disable();
                        goto skip;
                    }
                    
                }
                
                _tickThrottle += _secondCountNew;
                if (IsEnabled)
                {
                    FlashBackground();

                    if (_tickThrottle >1) //every second
                    {

                        if (CheckReconnect()) goto skip;

                        CheckReconnectInterval(_tickThrottle);

                        CheckDisconnect();

                        CheckStopPTZTracking();

                        if (Camobject.ptzschedule.active && !_suspendPTZSchedule)
                        {
                            CheckPTZSchedule();
                        }

                        if (Recording && !MovementDetected && !ForcedRecording)
                        {
                            InactiveRecord += _tickThrottle;
                        }

                        if (Camera != null && GotImage)
                        {
                            if (Calibrating)
                            {
                                DoCalibrate(_tickThrottle);
                            }

                            CheckVLCTimeStamp();
                            CheckTimeLapse(_tickThrottle);
                        }
                        if (Helper.HasFeature(Enums.Features.Recording))
                        {
                            CheckRecord();
                        }
                        _tickThrottle = 0;

                    }
                    CheckFTP();
                    CheckSaveFrame();
                }

                if (!Calibrating)
                {
                    CheckAlert(_secondCountNew);
                }

            skip:
                ;
            }
            catch (Exception ex)
            {
                ErrorHandler?.Invoke(ex.Message);
            }
            if (_requestRefresh)
            {
                _requestRefresh = false;
                Invalidate();
            }
        }

        private DateTime _lastFlash = DateTime.MinValue;
        private void FlashBackground()
        {
            bool b = true;
            if (Camobject.alerts.active)
            {
                var dt = Helper.Now;
                if ((dt - _lastFlash).TotalMilliseconds < 500)
                    return;
                _lastFlash = dt;
                b = BackgroundColor != MainForm.BackgroundColor;
                if (FlashCounter > Helper.Now)
                {
                    BackgroundColor = (BackgroundColor == MainForm.ActivityColor)
                        ? MainForm.BackgroundColor
                        : MainForm.ActivityColor;
                    b = false;
                }
                else
                {
                    switch (Camobject.alerts.mode.ToLower())
                    {
                        case "nomovement":
                            if (!MovementDetected)
                            {
                                BackgroundColor = (BackgroundColor == MainForm.NoActivityColor)
                                    ? MainForm.BackgroundColor
                                    : MainForm.NoActivityColor;
                                b = false;
                            }

                            break;
                    }
                }
            }
            if (b)
                BackgroundColor = MainForm.BackgroundColor;

        }

        private void CheckVLCTimeStamp()
        {
            if (Camobject.settings.sourceindex == 5)
            {
                var vlc = Camera.VideoSource as VlcStream;
                vlc?.CheckTimestamp();
            }
        }

        private void CheckRecord()
        {
            if (ForcedRecording)
            {
                StartSaving();
            }
            
            if (Recording)
            {
                var dur = (DateTime.UtcNow - _recordingStartTime).TotalSeconds;

                if (dur > Camobject.recorder.maxrecordtime || 
                    ((!MovementDetected && InactiveRecord > Camobject.recorder.inactiverecord) && !ForcedRecording && dur > Camobject.recorder.minrecordtime))
                    StopSaving();
            }
            
        }

        private void CheckTimeLapse(double since)
        {
            if (Camobject.recorder.timelapseenabled)
            {
                bool err = false;
                if (Camobject.recorder.timelapse > 0)
                {
                    _timeLapseTotal += since;
                    _timeLapse += since;

                    if (_timeLapse >= Camobject.recorder.timelapse)
                    {
                        if (!SavingTimeLapse)
                        {
                            if (!OpenTimeLapseWriter())
                                return;
                        }

                        Bitmap bm = LastFrame;
                        if (bm != null)
                        {
                            try
                            {
                                var pts = (long) TimeSpan.FromSeconds(_timeLapseFrameCount*
                                                                      (1d/Camobject.recorder.timelapseframerate)).
                                    TotalMilliseconds;
                                _timeLapseWriter.WriteVideoFrame(ResizeBitmap(bm), pts);
                                _timeLapseFrameCount++;
                            }
                            catch (Exception ex)
                            {
                                ErrorHandler?.Invoke(ex.Message);
                                err = true;
                            }
                            bm.Dispose();
                        }
                        _timeLapse = 0;
                    }
                    if (_timeLapseTotal >= 60 * Camobject.recorder.timelapsesave || err)
                    {
                        CloseTimeLapseWriter();
                    }
                }
                if (err)
                {
                    _timeLapseFrames = 0;
                }
                else
                {
                    if (Camobject.recorder.timelapseframes > 0 && Camera != null)
                    {
                        _timeLapseFrames += since;
                        if (_timeLapseFrames >= Camobject.recorder.timelapseframes)
                        {
                            Image frame = LastFrame;
                            if (frame != null)
                            {
                                string dir = Dir.Entry + "video\\" +
                                             Camobject.directory + "\\";
                                dir += @"grabs\";

                                DateTime date = DateTime.Now;
                                string filename =
                                    $"Frame_{date.Year}-{Helper.ZeroPad(date.Month)}-{Helper.ZeroPad(date.Day)}_{Helper.ZeroPad(date.Hour)}-{Helper.ZeroPad(date.Minute)}-{Helper.ZeroPad(date.Second)}.jpg";
                                if (!Directory.Exists(dir))
                                    Directory.CreateDirectory(dir);
                                frame.Save(dir + filename, MainForm.Encoder, MainForm.EncoderParams);
                                _timeLapseFrames = 0;
                                frame.Dispose();
                            }
                        }
                    }
                }
            }
        }

        private void CheckAlert(double since)
        {
            if (IsEnabled && Camera != null)
            {
                if (Alerted)
                {
                    if ((Helper.Now - LastAlerted).TotalSeconds > Camobject.alerts.minimuminterval)
                    {
                        Alerted = false;
                        UpdateFloorplans(false);
                    }
                }

                //Check new Alert
               
                if (Camobject.alerts.active && Camera != null)
                {
                    switch (Camobject.alerts.mode)
                    {
                        case "movement":
                            if (MovementDetected)
                            {
                                MovementCount += since;
                                if (MovementCount > Camobject.detector.movementintervalnew)
                                {
                                    if (Helper.CanAlert(Camobject.alerts.groupname, Camobject.alerts.resetinterval))
                                    {
                                        DoAlert("alert");
                                        MovementCount = 0;
                                    }
                                }
                            }
                            else
                            {
                                MovementCount = 0;
                            }
                            break;
                        case "objectcount":
                            var blobalg = Camera.MotionDetector?.MotionProcessingAlgorithm as BlobCountingObjectsProcessing;

                            if (blobalg?.ObjectsCount >= Camobject.alerts.objectcountalert)
                            {
                                if (Helper.CanAlert(Camobject.alerts.groupname, Camobject.alerts.resetinterval))
                                {
                                    DoAlert("alert");
                                    MovementCount = 0;
                                }
                            }
                            break;
                        case "nomovement":
                            if (LastMovementDetected > DateTime.MinValue && (Helper.Now - LastMovementDetected).TotalSeconds > Camobject.detector.nomovementintervalnew)
                            {
                                if (Helper.CanAlert(Camobject.alerts.groupname,Camobject.alerts.resetinterval))
                                {
                                    DoAlert("alert");
                                }
                            }
                            break;
                    }
                }
                
            }
        }

        private void CheckStopPTZTracking()
        {
            if (Camobject.settings.ptzautotrack && !Calibrating && Camobject.ptz != -1)
            {
                if (Ptzneedsstop && LastAutoTrackSent < Helper.Now.AddMilliseconds(-1000))
                {
                    PTZ.SendPTZCommand(Enums.PtzCommand.Stop);
                    Ptzneedsstop = false;
                }
                if (Camobject.settings.ptzautohome && LastAutoTrackSent > DateTime.MinValue &&
                    LastAutoTrackSent < Helper.Now.AddSeconds(0 - Camobject.settings.ptzautohomedelay))
                {
                    LastAutoTrackSent = DateTime.MinValue;
                    Calibrating = true;
                    _calibrateTarget = Camobject.settings.ptztimetohome;
                    if (string.IsNullOrEmpty(Camobject.settings.ptzautohomecommand) ||
                        Camobject.settings.ptzautohomecommand == "Center")
                        PTZ.SendPTZCommand(Enums.PtzCommand.Center);
                    else
                    {
                        PTZ.SendPTZCommand(Camobject.settings.ptzautohomecommand);
                    }
                }
            }
        }

        private void CheckFTP()
        {
            if (Camobject.ftp.mode == 2 && Math.Abs(Camobject.ftp.intervalnew) > double.Epsilon)
            {
                if (Camobject.ftp.enabled && Camobject.ftp.ready)
                {
                    double d = (Helper.Now - _lastFrameUploaded).TotalSeconds;
                    if (d >= Camobject.ftp.intervalnew && d > Camobject.ftp.minimumdelay)
                    {
                        FtpFrame();
                    }
                }
            }
        }
        private void CheckSaveFrame()
        {
            if (Camobject.savelocal.mode == 2 && Math.Abs(Camobject.savelocal.intervalnew) > double.Epsilon)
            {
                if (Camobject.savelocal.enabled)
                {
                    double d = (Helper.Now - _lastFrameSaved).TotalSeconds;
                    if (d >= Camobject.savelocal.intervalnew && d > Camobject.savelocal.minimumdelay)   {
                        SaveFrame();
                    }
                }
            }
        }

        private void CheckPTZSchedule()
        {
            if (Camobject.ptz == -1)
                return;
            DateTime dtnow = DateTime.Now;
            foreach (var entry in Camobject.ptzschedule.entries)
            {
                if (entry != null && entry.time.TimeOfDay < dtnow.TimeOfDay &&
                    entry.time.TimeOfDay > _dtPTZLastCheck.TimeOfDay)
                {
                    if (Camobject.ptz > 0)
                    {
                        PTZSettings2Camera ptz = MainForm.PTZs.FirstOrDefault(p => p.id == Camobject.ptz);
                        if (ptz != null)
                        {
                            objectsCameraPtzscheduleEntry entry1 = entry;
                            PTZSettings2CameraExtendedCommandsCommand extcmd =
                                ptz.ExtendedCommands?.Command?.FirstOrDefault(p => p.Name == entry1.command);
                            if (extcmd != null)
                            {
                                Calibrating = true;
                                PTZ.SendPTZCommand(extcmd.Value);
                            }
                        }
                    }
                    else
                    {
                        Calibrating = true;
                        PTZ.SendPTZCommand(entry.command, false);
                    }
                }
            }
            _dtPTZLastCheck = DateTime.Now;
        }

        private bool CheckSchedule()
        {
            DateTime dtnow = DateTime.Now;

            foreach (var entry in Camobject.schedule.entries.Where(p => p.active))
            {
                if (
                    entry.daysofweek.IndexOf(((int)dtnow.DayOfWeek).ToString(CultureInfo.InvariantCulture),
                                             StringComparison.Ordinal) == -1) continue;
                var stop = entry.stop.Split(':');
                if (stop[0] != "-")
                {
                    if (Convert.ToInt32(stop[0]) == dtnow.Hour)
                    {
                        if (Convert.ToInt32(stop[1]) == dtnow.Minute && dtnow.Second < 2)
                        {
                            Camobject.detector.recordondetect = entry.recordondetect;
                            Camobject.detector.recordonalert = entry.recordonalert;
                            Camobject.ftp.enabled = entry.ftpenabled;
                            Camobject.savelocal.enabled = entry.savelocalenabled;
                            Camobject.ptzschedule.active = entry.ptz;
                            Camobject.alerts.active = entry.alerts;
                            Camobject.settings.messaging = entry.messaging;

                            if (IsEnabled)
                                Disable();
                            return true;
                        }
                    }
                }


                var start = entry.start.Split(':');
                if (start[0] != "-")
                {
                    if (Convert.ToInt32(start[0]) == dtnow.Hour)
                    {
                        if (Convert.ToInt32(start[1]) == dtnow.Minute && dtnow.Second < 3)
                        {
                            if ((dtnow - _lastScheduleCheck).TotalSeconds > 60) //only want to do this once per schedule
                            {
                                if (!IsEnabled)
                                    Enable();

                                Camobject.detector.recordondetect = entry.recordondetect;
                                Camobject.detector.recordonalert = entry.recordonalert;
                                Camobject.ftp.enabled = entry.ftpenabled;
                                Camobject.savelocal.enabled = entry.savelocalenabled;
                                Camobject.ptzschedule.active = entry.ptz;
                                Camobject.alerts.active = entry.alerts;
                                Camobject.settings.messaging = entry.messaging;

                                if (Camobject.recorder.timelapseenabled && !entry.timelapseenabled)
                                {
                                    CloseTimeLapseWriter();
                                }
                                Camobject.recorder.timelapseenabled = entry.timelapseenabled;
                                if (entry.recordonstart)
                                {
                                    ForcedRecording = true;
                                }
                                _lastScheduleCheck = dtnow;
                            }
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void CheckDisconnect()
        {
            if (_errorTime != DateTime.MinValue)
            {
                int sec = Convert.ToInt32((Helper.Now - _errorTime).TotalSeconds);
                if (sec > MainForm.Conf.DisconnectNotificationDelay)
                {
                    //camera has been down for 30 seconds - send notification
                    DoAlert("disconnect");
                    _errorTime = DateTime.MinValue;
                }
            }
        }

        private string MailMerge(string s, string mode, bool recorded = false, string pluginMessage = "")
        {
            s = s.Replace("[OBJECTNAME]", Camobject.name);
            s = s.Replace("[TIME]", DateTime.Now.AddHours(Convert.ToDouble(Camobject.settings.timestampoffset)).ToLongTimeString());
            s = s.Replace("[DATE]", DateTime.Now.AddHours(Convert.ToDouble(Camobject.settings.timestampoffset)).ToShortDateString());
            s = s.Replace("[RECORDED]", recorded?"(recorded)":"");
            s = s.Replace("[PLUGIN]", pluginMessage);
            s = s.Replace("[EVENT]", mode.ToUpper());
            s = s.Replace("[SERVER]", MainForm.Conf.ServerName);

            return s;
        }

        private bool CheckReconnect()
        {
            if (_reconnectTime != DateTime.MinValue && !IsClone && !IsReconnect)
            {
                if (Camera?.VideoSource != null)
                {
                    int sec = Convert.ToInt32((Helper.Now - _reconnectTime).TotalSeconds);
                    if (sec > 10)
                    {
                        //try to reconnect every 10 seconds
                        if (!Camera.VideoSource.IsRunning)
                        {
                            Calibrating = true;
                            if (Camera.VideoSource != null)
                            {
                                Camera.Start();
                            }                           
                        }
                        _reconnectTime = Helper.Now;
                        return true;
                    }
                }
            }
            return false;
        }


        private void CheckReconnectInterval(double since)
        {
            if (Camera?.VideoSource != null && IsEnabled && !IsClone && !IsReconnect && !VideoSourceErrorState)
            {
                if (Camobject.settings.reconnectinterval > 0)
                {

                    ReconnectCount += since;
                    if (ReconnectCount > Camobject.settings.reconnectinterval)
                    {
                        IsReconnect = true;

                        if (VolumeControl != null)
                            VolumeControl.IsReconnect = true;

                        CameraReconnect?.Invoke(this, EventArgs.Empty);

                        try
                        {
                            Camera.SignalToStop();
                            Camera.WaitForStop();
                        }
                        catch (Exception ex)
                        {
                            ErrorHandler?.Invoke(ex.Message);
                        }

                        Application.DoEvents();

                        if (Camobject.settings.calibrateonreconnect)
                        {
                            Calibrating = true;
                        }

                        try
                        {
                            Camera.Start();
                        }
                        catch (Exception ex)
                        {
                            ErrorHandler?.Invoke(ex.Message);
                        }


                        CameraReconnected?.Invoke(this, EventArgs.Empty);

                        if (VolumeControl != null)
                            VolumeControl.IsReconnect = false;

                        IsReconnect = false;
                        ReconnectCount = 0;
                    }
                }
            }
        }

        private void DoCalibrate(double since)
        {
            if (Camera != null)
            {
                if (Camera.MotionDetector != null)
                {
                    var detector = Camera.MotionDetector.MotionDetectionAlgorithm as CustomFrameColorDifferenceDetector;
                    if (detector != null)
                    {
                        detector.
                            SetBackgroundFrame(LastFrame);
                    }
                    else
                    {
                        var algorithm = Camera.MotionDetector.MotionDetectionAlgorithm as CustomFrameDifferenceDetector;
                        if (algorithm != null)
                        {
                            algorithm
                                .
                                SetBackgroundFrame(LastFrame);
                        }
                        else
                        {
                            var modelingDetector = Camera.MotionDetector.MotionDetectionAlgorithm as SimpleBackgroundModelingDetector;
                            if (
                                modelingDetector != null)
                            {
                                modelingDetector.Reset();
                            }
                            else
                            {
                                var detectionAlgorithm = Camera.MotionDetector.MotionDetectionAlgorithm as SimpleColorBackgroundModelingDetector;
                                detectionAlgorithm?.Reset();
                            }
                        }
                    }
                }

                CalibrateCount += since;
                if (CalibrateCount > _calibrateTarget)
                {
                    Calibrating = false;
                    CalibrateCount = 0;
                }
            }
            LastMovementDetected = Helper.Now;
        }

        private Bitmap ResizeBitmap(Bitmap frame)
        {
            if (Camobject.recorder.profile == 0)
                return frame;

            if (frame.Width == _videoWidth && frame.Height == _videoHeight)
                return frame;

            var b = new Bitmap(_videoWidth, _videoHeight);
            var r = new Rectangle(0, 0, _videoWidth, _videoHeight);
            using (var g = Graphics.FromImage(b))
            {
                g.CompositingMode = CompositingMode.SourceCopy;
                g.CompositingQuality = CompositingQuality.HighSpeed;
                g.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                g.SmoothingMode = SmoothingMode.None;
                g.InterpolationMode = InterpolationMode.Default;

                //g.GdiDrawImage(LastFrame, r);
                g.DrawImage(LastFrame, r);
            }

            frame.Dispose();
            return b;
        }

        private int _videoWidth, _videoHeight;

        public void SetVideoSize()
        {
            switch (Camobject.recorder.profile)
            {
                default:
                    if (Camera != null && Camera.Width > -1)
                    {
                        _videoWidth = Camera.Width;
                        _videoHeight = Camera.Height;
                    }
                    else
                    {
                        string[] wh = Camobject.resolution.Split('x');
                        _videoWidth = Convert.ToInt32(wh[0]);
                        _videoHeight = Convert.ToInt32(wh[1]);
                    }
                    break;
                case 1:
                    _videoWidth = 320; _videoHeight = 240;
                    break;
                case 2:
                    _videoWidth = 480; _videoHeight = 320;
                    break;
            }
        }

        public void Snapshot()
        {
            string fn = SaveFrame();
            if (fn != "" && MainForm.Conf.OpenGrabs)
                MainForm.OpenUrl(fn);
        }

        public string SaveFrame(Bitmap bmp = null)
        {
            var c = Camera;
            if (c == null || !c.IsRunning)
                return "";

            if (!Helper.HasFeature(Enums.Features.Save_Frames))
            {
                return "";
            }

            Image myThumbnail = bmp;
            Graphics g = null;
            string fullpath = "";
            var strFormat = new StringFormat();
            try
            {
                if (myThumbnail == null)
                    myThumbnail = LastFrame;
                if (myThumbnail != null)
                {
                    g = Graphics.FromImage(myThumbnail);
                    strFormat.Alignment = StringAlignment.Center;
                    strFormat.LineAlignment = StringAlignment.Far;
                    g.DrawString(Camobject.savelocal.text, MainForm.Drawfont, MainForm.OverlayBrush,
                        new RectangleF(0, 0, myThumbnail.Width, myThumbnail.Height), strFormat);


                    if (MainForm.Encoder != null)
                    {
                        string folder = Dir.Entry + "video\\" + Camobject.directory + "\\";

                        if (!Directory.Exists(folder + @"grabs\"))
                            Directory.CreateDirectory(folder + @"grabs\");

                        int i = 0;
                        string filename = Camobject.savelocal.filename;
                        filename = filename.Replace("{C}", ZeroPad(Camobject.savelocal.counter, Camobject.savelocal.countermax));
                        Camobject.savelocal.counter++;
                        if (Camobject.savelocal.counter > Camobject.savelocal.countermax)
                            Camobject.savelocal.counter = 0;

                        while (filename.IndexOf("{", StringComparison.Ordinal) != -1 && i < 20)
                        {
                            filename = String.Format(CultureInfo.InvariantCulture, filename, DateTime.Now);
                            i++;
                        }

                        //  Set the quality
                        fullpath = folder + @"grabs\" + filename;
                        
                        if (File.Exists(fullpath))
                        {
                            File.Delete(fullpath);
                        }

                        var parameters = new EncoderParameters(1)
                                         {
                                             Param =
                                             {
                                                 [0] =
                                                     new EncoderParameter(
                                                     Encoder.Quality,
                                                     Camobject.savelocal.quality)
                                             }
                                         };

                        myThumbnail.Save(fullpath, MainForm.Encoder, parameters);
                        myThumbnail.Dispose();

                        if (Camobject.settings.cloudprovider.images)
                        {
                            CloudGateway.Upload(2, Camobject.id, fullpath);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorHandler?.Invoke(ex.Message);
            }
            g?.Dispose();
            strFormat.Dispose();
            myThumbnail?.Dispose();
            _lastFrameSaved = Helper.Now;
            return fullpath;
        }

        private string ZeroPad(int counter, int countermax)
        {
            string r = counter.ToString(CultureInfo.InvariantCulture);
            int i = countermax.ToString(CultureInfo.InvariantCulture).Length;
            while (r.Length < i)
            {
                r = "0" + r;
            }
            return r;

        }

        private void FtpFrame(Bitmap bmp = null)
        {
            using (var imageStream = new MemoryStream())
            {
                Image myThumbnail = bmp;
                Graphics g = null;
                var strFormat = new StringFormat();
                try
                {
                    if (myThumbnail==null)
                        myThumbnail = LastFrame;
                    if (myThumbnail != null)
                    {
                        g = Graphics.FromImage(myThumbnail);
                        strFormat.Alignment = StringAlignment.Center;
                        strFormat.LineAlignment = StringAlignment.Far;
                        g.DrawString(Camobject.ftp.text, MainForm.Drawfont, MainForm.OverlayBrush,
                            new RectangleF(0, 0, myThumbnail.Width, myThumbnail.Height), strFormat);

                        int i = 0;
                        string filename = Camobject.ftp.filename;
                        filename = filename.Replace("{C}", ZeroPad(Camobject.ftp.ftpcounter, Camobject.ftp.countermax));
                        Camobject.ftp.ftpcounter++;
                        if (Camobject.ftp.ftpcounter > Camobject.ftp.countermax)
                            Camobject.ftp.ftpcounter = 0;

                        while (filename.IndexOf("{", StringComparison.Ordinal) != -1 && i < 20)
                        {
                            filename = String.Format(CultureInfo.InvariantCulture, filename, DateTime.Now);
                            i++;
                        }

                        if (MainForm.Encoder != null)
                        {
                            //  Set the quality
                            var parameters = new EncoderParameters(1)
                                             {
                                                 Param =
                                                 {
                                                     [0] =
                                                         new EncoderParameter(
                                                         Encoder.Quality,
                                                         Camobject.ftp.quality)
                                                 }
                                             };
                            myThumbnail.Save(imageStream, MainForm.Encoder, parameters);
                        }

                        var ftp = MainForm.Conf.FTPServers.FirstOrDefault(p => p.ident == Camobject.ftp.ident);
                        if (ftp != null)
                        {
                            Camobject.ftp.ready = false;

                            ThreadPool.QueueUserWorkItem((new AsynchronousFtpUpLoader()).FTP,
                                new FTPTask(ftp.server, ftp.port,
                                    ftp.usepassive, ftp.username,
                                    ftp.password, filename,
                                    imageStream.ToArray(), Camobject.id, Camobject.ftp.counter, ftp.rename, ftp.sftp));

                            myThumbnail.Dispose();
                        }
                        myThumbnail.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler?.Invoke(ex.Message);
                    Camobject.ftp.ready = true;
                }
                _lastFrameUploaded = Helper.Now;

                g?.Dispose();
                strFormat.Dispose();
            }

        }

        private bool OpenTimeLapseWriter()
        {
            DateTime date = DateTime.Now;
            String filename =
                $"TimeLapse_{date.Year}-{Helper.ZeroPad(date.Month)}-{Helper.ZeroPad(date.Day)}_{Helper.ZeroPad(date.Hour)}-{Helper.ZeroPad(date.Minute)}-{Helper.ZeroPad(date.Second)}";
            TimeLapseVideoFileName = Camobject.id + "_" + filename;
            string folder = Dir.Entry + "video\\" + Camobject.directory + "\\";

            if (!Directory.Exists(folder + @"thumbs\"))
                Directory.CreateDirectory(folder + @"thumbs\");

            filename = folder + TimeLapseVideoFileName;


            Bitmap bmpPreview = LastFrame;

            if (bmpPreview != null)
            {

                bmpPreview.Save(folder + @"thumbs/" + TimeLapseVideoFileName + "_large.jpg", MainForm.Encoder,
                    MainForm.EncoderParams);
                Image.GetThumbnailImageAbort myCallback = ThumbnailCallback;
                Image myThumbnail = bmpPreview.GetThumbnailImage(96, 72, myCallback, IntPtr.Zero);

                Graphics g = Graphics.FromImage(myThumbnail);
                var strFormat = new StringFormat
                                {
                                    Alignment = StringAlignment.Center,
                                    LineAlignment = StringAlignment.Far
                                };
                var rect = new RectangleF(0, 0, 96, 72);

                g.DrawString(LocRm.GetString("Timelapse"), MainForm.Drawfont, MainForm.OverlayBrush,
                    rect, strFormat);
                strFormat.Dispose();

                myThumbnail.Save(folder + @"thumbs/" + TimeLapseVideoFileName + ".jpg", MainForm.Encoder,
                    MainForm.EncoderParams);

                g.Dispose();
                myThumbnail.Dispose();
                bmpPreview.Dispose();
            }


            _timeLapseWriter = null;
            bool success = false;

            try
            {
                try
                {
                    Program.FfmpegMutex.WaitOne();
                    _timeLapseWriter = new VideoFileWriter();
                    _timeLapseWriter.Open(filename + CodecExtension, _videoWidth, _videoHeight, Codec,
                                          CalcBitRate(Camobject.recorder.quality), Camobject.recorder.timelapseframerate);

                    success = true;
                    TimelapseStart = Helper.Now;
                }
                catch (Exception ex)
                {
                    ErrorHandler?.Invoke(ex.Message);
                    _timeLapseWriter = null;
                    Camobject.recorder.timelapse = 0;
                }
                finally
                {
                    try
                    {
                        Program.FfmpegMutex.ReleaseMutex();
                    }
                    catch (ObjectDisposedException)
                    {
                        //can happen on shutdown
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler?.Invoke(ex.Message);
            }
            return success;
        }

        private void CloseTimeLapseWriter()
        {
            _timeLapseTotal = 0;
            _timeLapseFrameCount = 0;

            if (_timeLapseWriter == null)
                return;

            try
            {
                Program.FfmpegMutex.WaitOne();
                _timeLapseWriter.Dispose();
            }
            catch (Exception ex)
            {
                ErrorHandler?.Invoke(ex.Message);
            }
            finally
            {
                try
                {
                    Program.FfmpegMutex.ReleaseMutex();
                }
                catch (ObjectDisposedException)
                {
                    //can happen on shutdown
                }
            }

            _timeLapseWriter = null;

            var fpath = Dir.Entry + "video\\" + Camobject.directory + "\\" + TimeLapseVideoFileName + CodecExtension;

            var fi = new FileInfo(fpath);
            var dSeconds = Convert.ToInt32((Helper.Now - TimelapseStart).TotalSeconds);

            FilesFile ff = _filelist.FirstOrDefault(p => p.Filename.EndsWith(TimeLapseVideoFileName + CodecExtension));
            bool newfile = false;
            if (ff == null)
            {
                ff = new FilesFile();
                newfile = true;
            }

            ff.CreatedDateTicks = DateTime.Now.Ticks;
            ff.Filename = TimeLapseVideoFileName + CodecExtension;
            ff.MaxAlarm = 0;
            ff.SizeBytes = fi.Length;
            ff.DurationSeconds = dSeconds;
            ff.IsTimelapse = true;
            ff.IsMergeFile = false;
            ff.AlertData = "";
            ff.TriggerLevel = 0;
            ff.TriggerLevelMax = 0;


            if (newfile)
            {
                lock (_lockobject)
                    _filelist.Insert(0, ff);

                MainForm.MasterFileAdd(new FilePreview(TimeLapseVideoFileName + CodecExtension, dSeconds,
                                                            Camobject.name, DateTime.Now.Ticks, 2, Camobject.id,
                                                            ff.MaxAlarm,false,false));
                
                MainForm.NeedsMediaRefresh = Helper.Now;
            }
        }

        private static bool ThumbnailCallback()
        {
            return false;
        }

        public bool Highlighted;


        private bool _custom;
        private DateTime _customSet = DateTime.MinValue;

        internal bool Custom
        {
            get { return _custom; }
            set
            {
                _custom = value;
                if (value)
                    _customSet = Helper.Now;
            }
        }

        public Color BorderColor
        {
            get
            {

                if (Custom)
                    return _customColor;

                if (Highlighted)
                    return MainForm.FloorPlanHighlightColor;

                if (Focused)
                    return MainForm.BorderHighlightColor;

                return MainForm.BorderDefaultColor;

            }
        }

        public int BorderWidth => (Highlighted || Focused || Custom) ? 2 : 1;

        internal bool GotImage;

        public Bitmap LastFrame
        {
            get
            {
                lock (_lockobject)
                {
                    return (Bitmap) _lastFrame?.Clone();
                }
            }
            set
            {
                if (IsEnabled)
                {
                    lock (_lockobject)
                    {
                        _lastFrame?.Dispose();

                        if (value!=null)
                            _lastFrame = (Bitmap)value.Clone();
                        else
                        {
                            _lastFrame = null;
                        }
                    }
                }
                GotImage = value != null;
                Invalidate();
                //not sure about this...
                //BeginInvoke((Action)(Update));
            }
        }


        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics gCam = pe.Graphics;

            var grabBrush = new SolidBrush(BorderColor);
            var borderPen = new Pen(grabBrush, BorderWidth);
            var volBrush = new SolidBrush(MainForm.VolumeLevelColor);

            string m = "", txt = Camobject.name;
            try
            {
                Rectangle rc = ClientRectangle;
                int textpos = rc.Height - 15;


                //bool message = false;
                if (IsEnabled)
                {
                    if (Camera != null && GotImage)
                    {
                        m = $"FPS: {Camera.Framerate:F2}" + ", ";

                        if (Camera.ZFactor > 1)
                        {
                            m = "Z: " + $"{Camera.ZFactor:0.0}" + " " + m;
                        }

                        
                        gCam.CompositingMode = CompositingMode.SourceCopy;
                        gCam.CompositingQuality = CompositingQuality.HighSpeed;
                        gCam.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                        gCam.SmoothingMode = SmoothingMode.None;
                        gCam.InterpolationMode = InterpolationMode.Default;
                        gCam.Clear(BackgroundColor);
                        gCam.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                        if (!_minimised)
                        {
                            var bmp = LastFrame;
                            if (bmp != null)
                            {
                                if (NeedSizeUpdate)
                                {
                                    AutoSize = true;
                                    UpdatePosition(bmp.Width, bmp.Height);
                                }
                                else
                                    AutoSize = false;

                                gCam.DrawImage(bmp, rc.X + 1, rc.Y + 1, rc.Width - 2, rc.Height - 26);
                                bmp.Dispose();
                            }
                        }

                        gCam.CompositingMode = CompositingMode.SourceOver;                      
                        
                        
                        if (PTZNavigate)
                        {
                            RunPTZ(gCam);
                        }

                        if (!VideoSourceErrorState)
                        {
                            if (Calibrating)
                            {
                                int remaining = _calibrateTarget - Convert.ToInt32(CalibrateCount);
                                if (remaining < 0) remaining = 0;

                                txt += ": " + LocRm.GetString("Calibrating") + " (" + remaining + ")";
                            }
                            else
                            {
                                var blobcounter =
                                    Camera.MotionDetector?.MotionProcessingAlgorithm as BlobCountingObjectsProcessing;
                                if (blobcounter != null)
                                {
                                    m += blobcounter.ObjectsCount + " " + LocRm.GetString("Objects") + ", ";
                                }
                            }
                        }
                    }

                    if (VideoSourceErrorState || !GotImage)
                    {
                        var img = Properties.Resources.connecting;
                        gCam.DrawImage(img,Width-img.Width-2,2,img.Width,img.Height);
                    }
                    else
                    {
                        if (Recording)
                        {
                            gCam.FillEllipse(MainForm.RecordBrush, new Rectangle(rc.Width - 12, 4, 8, 8));
                        }

                        if (Camera != null && Camera.IsRunning && Camobject.detector.type != "None")
                        {
                            DrawDetectionGraph(gCam, volBrush, MainForm.CameraLine, rc);
                        }
                    }
                }
                else
                {
                    txt += ": " + LocRm.GetString("Offline");
                    gCam.DrawString(SourceType+": "+Camobject.name, MainForm.Drawfont, MainForm.CameraDrawBrush, new PointF(5, 5));
                }

                string flags = "";
                if (Camobject.alerts.active)
                    flags += "!";

                if (ForcedRecording)
                    flags += "F";
                else
                {
                    if (Camobject.detector.recordondetect)
                        flags += "D";
                    else
                    {
                        if (Camobject.detector.recordonalert)
                            flags += "A";
                        else
                        {
                            flags += "N";
                        }
                    }
                }
                if (Camobject.schedule.active)
                    flags += "S";

                if (flags!="")
                    m = flags +"  "+ m;


                gCam.DrawString(m + txt,MainForm.Drawfont, MainForm.CameraDrawBrush, new PointF(5, textpos));

                if (_mouseMove > Helper.Now.AddSeconds(-3) && MainForm.Conf.ShowOverlayControls && !PTZNavigate)
                {
                    DrawOverlay(gCam);
                }

                gCam.DrawRectangle(borderPen, 0, 0, rc.Width - 1, rc.Height - 1);
                var borderPoints = new[]
                {
                    new Point(rc.Width - 15, rc.Height), new Point(rc.Width, rc.Height - 15),
                    new Point(rc.Width, rc.Height)
                };

                gCam.FillPolygon(grabBrush, borderPoints);
            }
            catch (Exception e)
            {
                ErrorHandler?.Invoke(e.Message);
            }


            borderPen.Dispose();
            grabBrush.Dispose();
            volBrush.Dispose();

            base.OnPaint(pe);
            _lastRedraw = Helper.Now;
        }

        private void DrawDetectionGraph(Graphics gCam, SolidBrush sb, Pen pline, Rectangle rc)
        {
            //draw detection graph
            double d = (Convert.ToDouble(rc.Width - 4) / 100.0);
            int w = 2 + Convert.ToInt32(d * (Camera.MotionLevel * 100.0d));
            int ax = 2 + Convert.ToInt32(d * Camobject.detector.minsensitivity);
            int axmax = 2 + Convert.ToInt32(d * Camobject.detector.maxsensitivity);

            var grabPoints = new[]
                                 {
                                     new Point(2, rc.Height - 22), new Point(w, rc.Height - 22),
                                     new Point(w, rc.Height - 15), new Point(2, rc.Height - 15)
                                 };

            gCam.FillPolygon(sb, grabPoints);

            gCam.DrawLine(pline, new Point(ax, rc.Height - 22), new Point(ax, rc.Height - 15));
            gCam.DrawLine(pline, new Point(axmax, rc.Height - 22), new Point(axmax, rc.Height - 15));
        }

        private int GetButtonIndexByLocation(Point xy)
        {
            var rBp = ButtonPanel;
            if (xy.X >= rBp.X && xy.Y > rBp.Y - 25 && xy.X <= rBp.X + rBp.Width && xy.Y <= rBp.Y + rBp.Height)
            {
                if (xy.Y < rBp.Y)
                    return -1;//seek

                if (xy.Y > 25)
                {
                    double x = xy.X - rBp.X;
                    return Convert.ToInt32(Math.Ceiling((x / rBp.Width) * ButtonCount)) - 1;
                }
            }
            return -999;//nothing
        }

        private Rectangle GetButtonByIndex(int buttonIndex, out Rectangle destRect)
        {
            Rectangle rSrc = Rectangle.Empty;
            bool b = IsEnabled;
            switch (buttonIndex)
            {
                case 0://power
                    rSrc = b ? MainForm.RPowerOn : MainForm.RPower;
                    break;
                case 1://record
                    if (b && Helper.HasFeature(Enums.Features.Recording))
                        rSrc = (Recording || ForcedRecording) ? MainForm.RRecordOn : MainForm.RRecord;
                    else
                    {
                        rSrc = MainForm.RRecordOff;
                    }
                    break;
                case 2://settings
                    rSrc = Helper.HasFeature(Enums.Features.Edit) ? MainForm.REdit : MainForm.REditOff;
                    break;
                case 3://web
                    rSrc = Helper.HasFeature(Enums.Features.Access_Media) ? MainForm.RWeb : MainForm.RWebOff;
                    break;
                case 4://grab
                    if (b && Helper.HasFeature(Enums.Features.Save_Frames))
                        rSrc = MainForm.RGrab;
                    else
                    {
                        rSrc = MainForm.RGrabOff;
                    }
                    break;
                case 5://talk
                    if (b && Camobject.settings.audiomodel != "None")
                        rSrc = Talking ? MainForm.RTalkOn : MainForm.RTalk;
                    else
                        rSrc = MainForm.RTalkOff;
                        
                    break;
                case 6://text
                    if (b && Camobject.settings.audiomodel != "None")
                        rSrc = MainForm.RText;
                    else
                        rSrc = MainForm.RTextOff;
                    break;
                case 7://listen
                    if (b && Camobject.settings.micpair > -1)
                        rSrc = Listening ? MainForm.RListenOn : MainForm.RListen;
                    else
                        rSrc = MainForm.RListenOff;
                    break;

            }

            if (MainForm.Conf.BigButtons)
            {
                rSrc.X -= 2;
                rSrc.Width += 8;
                rSrc.Height += 8;
            }
            var bp = ButtonPanel;
            destRect = new Rectangle(bp.X + buttonIndex * (bp.Width / ButtonCount) + 5, Height - 25 - rSrc.Height - 6, rSrc.Width, rSrc.Height);
            return rSrc;
        }


        private void DrawButton(Graphics gCam, int buttonIndex)
        {
            Rectangle rDest;
            Rectangle rSrc = GetButtonByIndex(buttonIndex, out rDest);

            gCam.DrawImage(MainForm.Conf.BigButtons ? Properties.Resources.icons_big : Properties.Resources.icons,rDest,rSrc, GraphicsUnit.Pixel);
        }

        private void DrawOverlay(Graphics gCam)
        {
            var rPanel = ButtonPanel;
            if (Camera != null && Seekable)
            {
                AddSeekOverlay(gCam);
            }
            if (!Seekable)
            {
                gCam.FillRectangle(MainForm.OverlayBackgroundBrush, rPanel);
            }
            for(int i=0;i<ButtonCount;i++)
                DrawButton(gCam, i);
        }

        private void AddSeekOverlay(Graphics gCam)
        {
            var vs = Camera.VideoSource as VlcStream;
            long time = 0, duration = 0;
            if (vs != null)
            {
                time = vs.Time;
                duration = vs.Duration;
            }
            
            if (duration > 0)
            {
                var rPanel = ButtonPanel;
                string timedisplay =
                    $"{TimeSpan.FromMilliseconds(time).ToString().Substring(0, 8)} / {TimeSpan.FromMilliseconds(duration).ToString().Substring(0, 8)}";

                gCam.FillRectangle(MainForm.OverlayBackgroundBrush, rPanel.X, rPanel.Y - 25, rPanel.Width, rPanel.Height + 25);
                //draw seek bar
                gCam.DrawLine(Pens.White, rPanel.X, rPanel.Y - 2, rPanel.Width + rPanel.X, rPanel.Y -2);
                var xpos = (Convert.ToDouble(time) / Convert.ToDouble(duration)) * rPanel.Width;
                if (_newSeek > 0)
                    xpos = _newSeek;

                int x = rPanel.X + Convert.ToInt32(xpos);
                var navPoints = new[]
                {
                    new Point(x-4,rPanel.Y-8), 
                    new Point(x+4,rPanel.Y-2),
                    new Point(x-4, rPanel.Y+4)
                };

                gCam.FillPolygon(Brushes.White, navPoints);
                gCam.DrawPolygon(Pens.Black, navPoints);
                var s = gCam.MeasureString(timedisplay, MainForm.Drawfont);
                gCam.DrawString(timedisplay, MainForm.Drawfont, MainForm.OverlayBrush, Width / 2f - s.Width / 2,
                                rPanel.Y -s.Height - 6);
            }
        }

        private void RunPTZ(Graphics gCam)
        {
            var overlayBackgroundBrush = new SolidBrush(Color.FromArgb(128, 0, 0, 0));
            gCam.FillEllipse(overlayBackgroundBrush, PTZReference.X - 40, PTZReference.Y - 40, 80, 80);
            overlayBackgroundBrush.Dispose();

            gCam.DrawEllipse(MainForm.CameraNav, PTZReference.X - 10, PTZReference.Y - 10, 20, 20);
            double angle = Math.Atan2(PTZReference.Y - _mouseLoc.Y, PTZReference.X - _mouseLoc.X);

            var x = PTZReference.X - 30 * Math.Cos(angle);
            var y = PTZReference.Y - 30 * Math.Sin(angle);
            gCam.DrawLine(MainForm.CameraNav, PTZReference, new Point((int)x, (int)y));

            if (Camobject.ptz != -1 && Math.Abs(Camera.ZFactor - 1) < double.Epsilon)
            {
                Calibrating = true;
                PTZ.SendPTZDirection(angle);
            }
            else
            {
                var d =
                    (Math.Sqrt(Math.Pow(PTZReference.X - _mouseLoc.X, 2) +
                                Math.Pow(PTZReference.Y - _mouseLoc.Y, 2))) / 5;

                var p = Camera.ZPoint;
                p.X -= Convert.ToInt32(d * Math.Cos(angle));
                p.Y -= Convert.ToInt32(d * Math.Sin(angle));
                Camera.ZPoint = p;

                gCam.DrawString(LocRm.GetString("DIGITAL"), MainForm.Drawfont, MainForm.CameraDrawBrush, PTZReference.X - 21, PTZReference.Y - 25);
                
            }
        }

        public ReadOnlyCollection<FilesFile> FileList => _filelist.AsReadOnly();

        public void AddFile(FilesFile f)
        {
            lock (_lockobject)
            {
                _filelist.Add(f);
            }
        }


        private void CameraNewFrame(object sender, NewFrameEventArgs e)
        {
            try
            {
                if (_firstFrame)
                {
                    Camobject.resolution = e.Frame.Width + "x" + e.Frame.Height;
                    SetVideoSize();
                    _firstFrame = false;
                    var vlc = Camera.VideoSource as VlcStream;
                    if (vlc != null)
                        Seekable = vlc.Seekable;
                }

                if (VideoSourceErrorState)
                {
                    UpdateFloorplans(false);
                    var vl = VolumeControl;
                    if (vl?.AudioSource != null && vl.IsEnabled)
                    {
                        if (vl.AudioSource == Camera.VideoSource || vl.IsClone)
                        {
                            vl.AudioSource.LevelChanged -= vl.AudioDeviceLevelChanged;
                            vl.AudioSource.DataAvailable -= vl.AudioDeviceDataAvailable;
                            vl.AudioSource.AudioFinished -= vl.AudioDeviceAudioFinished;

                            vl.AudioSource.LevelChanged += vl.AudioDeviceLevelChanged;
                            vl.AudioSource.DataAvailable += vl.AudioDeviceDataAvailable;
                            vl.AudioSource.AudioFinished += vl.AudioDeviceAudioFinished;
                        }
                    }
                    VideoSourceErrorState = false;
                    _reconnectFailCount = 0;
                }

                lock (_lockobject)
                {
                    
                    var dt = Helper.Now.AddSeconds(0 - Camobject.recorder.bufferseconds);
                    if (!Recording)
                    {
                        while (Buffer.Count > 0)
                        {
                            Helper.FrameAction fa;
                            if (Buffer.TryPeek(out fa))
                            {
                                if (fa.TimeStamp < dt)
                                {
                                    if (Buffer.TryDequeue(out fa))
                                        fa.Nullify();
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                    Buffer.Enqueue(new Helper.FrameAction(e.Frame, Camera.MotionLevel, Helper.Now));
                }
                

                if (_lastRedraw < Helper.Now.AddMilliseconds(0 - 1000 / MainForm.Conf.MaxRedrawRate))
                {
                    LastFrame = e.Frame;
                }

                if (_reconnectTime != DateTime.MinValue)
                {
                    _errorTime = _reconnectTime = DateTime.MinValue;
                    DoAlert("reconnect");
                }

                NewFrame?.Invoke(this, e);

                _errorTime = DateTime.MinValue;

            }
            catch (Exception ex)
            {
                ErrorHandler?.Invoke(ex.Message);
            }
        }

        public event NewFrameEventHandler NewFrame;

        internal configurationDirectory Dir
        {
            get
            {
                try
                {
                    return MainForm.Conf.MediaDirectories[Camobject.settings.directoryIndex];
                }
                catch
                {
                    return MainForm.Conf.MediaDirectories[0];
                }
            }
        }

        public void StartSaving()
        {
            if (Recording || Dir.StopSavingFlag || IsEdit || !Helper.HasFeature(Enums.Features.Recording))
                return;
            lock (_lockobject)
            {
                if (Recording)
                    return;

                if (!Helper.HasFeature(Enums.Features.Recording))
                {
                    ErrorHandler?.Invoke("Recording has been disabled in settings - feature set");
                    return;
                }

                if (MainForm.RecordingThreads >= MainForm.Conf.MaxRecordingThreads)
                {
                    ErrorHandler?.Invoke("Skipped recording - maximum recording thread limit hit. See settings to modify the limit.");
                    return;
                }
                _recordingStartTime = DateTime.UtcNow;
                _recordingThread = new Thread(Record)
                                   {
                                       Name = "Recording Thread (" + Camobject.id + ")",
                                       IsBackground = true,
                                       Priority = ThreadPriority.Normal
                                   };
                _stopWrite.Reset();
                _recordingThread.Start();
            }
        }

        public void OpenWebInterface()
        {
            if (SupportsWebInterface) { 
                try
                {
                    var uri = new Uri(Camobject.settings.videosourcestring);
                    var url = uri.AbsoluteUri.Replace(uri.PathAndQuery, "");
                    if (!uri.Scheme.StartsWith("http"))
                    {
                        url = url.ReplaceFirst(uri.Scheme, "http");
                        url = url.ReplaceFirst(":" + uri.Port, ":80");
                    }
                    MainForm.OpenUrl(url);
                }
                catch (Exception ex)
                {
                    MainForm.LogExceptionToFile(ex,"open web browser");
                }
            }
        }

        public bool SupportsWebInterface
        {
            get
            {
                switch (Camobject.settings.sourceindex)
                {
                    default:
                        Uri uri;
                        if (Uri.TryCreate(Camobject.settings.videosourcestring, UriKind.Absolute, out uri))
                        {
                            return !uri.IsFile;
                        }
                        return false;
                    case 3:
                    case 4:
                    case 6:
                    case 7:
                    case 8:
                    case 10:
                        return false;
                }
            }
        }
        [HandleProcessCorruptedStateExceptions]
        private void Record()
        {
            try
            {
                MainForm.RecordingThreads++;
                AbortedAudio = false;
                LogToPlugin("Recording Started");
                DoAlert("recordingstarted");
                
                
                string previewImage = "";
                try
                {

                    if (!string.IsNullOrEmpty(Camobject.recorder.trigger))
                    {
                        string[] tid = Camobject.recorder.trigger.Split(',');
                        switch (tid[0])
                        {
                            case "1":
                                VolumeLevel vl = MainForm.InstanceReference.GetVolumeLevel(Convert.ToInt32(tid[1]));
                                if (vl != null && !vl.Recording)
                                    vl.RecordSwitch(true);
                                break;
                            case "2":
                                CameraWindow cw = MainForm.InstanceReference.GetCameraWindow(Convert.ToInt32(tid[1]));
                                if (cw != null && !cw.Recording)
                                    cw.RecordSwitch(true);
                                break;
                        }
                    }

                    try
                    {
                        DateTime date = DateTime.Now.AddHours(Convert.ToDouble(Camobject.settings.timestampoffset));

                        string filename =
                            $"{date.Year}-{Helper.ZeroPad(date.Month)}-{Helper.ZeroPad(date.Day)}_{Helper.ZeroPad(date.Hour)}-{Helper.ZeroPad(date.Minute)}-{Helper.ZeroPad(date.Second)}";


                        var vc = VolumeControl;
                        bool bAudio = vc?.AudioSource != null && vc.Micobject.settings.active;

                        if (bAudio)
                        {
                            vc.StartSaving();
                            vc.ForcedRecording = ForcedRecording;
                        }

                        VideoFileName = Camobject.id + "_" + filename;
                        string folder = Dir.Entry + "video\\" + Camobject.directory + "\\";

                        string videopath = folder + VideoFileName + CodecExtension;
                        bool error = false;
                        double maxAlarm = 0;
                        long lastvideopts = -1, lastaudiopts = -1;
                        DateTime recordingStart = Helper.Now;
                        try
                        {
                            if (!Directory.Exists(folder))
                                Directory.CreateDirectory(folder);

                            try
                            {

                                Program.FfmpegMutex.WaitOne();
                                _writer = new VideoFileWriter();

                                bool bSuccess;
                                if (bAudio)
                                {
                                    bSuccess = _writer.Open(videopath, _videoWidth, _videoHeight, Codec,
                                        CalcBitRate(Camobject.recorder.quality), CodecAudio, CodecFramerate,
                                        vc.Micobject.settings.bits*
                                        vc.Micobject.settings.samples*
                                        vc.Micobject.settings.channels,
                                        vc.Micobject.settings.samples, vc.Micobject.settings.channels);
                                }
                                else
                                {
                                    bSuccess = _writer.Open(videopath, _videoWidth, _videoHeight, Codec,
                                        CalcBitRate(Camobject.recorder.quality), CodecFramerate);
                                }

                                if (!bSuccess)
                                {
                                    throw new Exception("Failed to open up a video writer");
                                }

                            }
                            finally
                            {
                                try
                                {
                                    Program.FfmpegMutex.ReleaseMutex();
                                }
                                catch (ObjectDisposedException)
                                {
                                    //can happen on shutdown
                                }
                            }


                            Helper.FrameAction? peakFrame = null;
                            bool first = true;

                            while (!_stopWrite.WaitOne(5))
                            {
                                Helper.FrameAction fa;
                                if (Buffer.TryDequeue(out fa))
                                {
                                    if (first)
                                    {
                                        recordingStart = fa.TimeStamp;
                                        first = false;
                                    }

                                    WriteFrame(fa, recordingStart, ref lastvideopts, ref maxAlarm, ref peakFrame,
                                        ref lastaudiopts);
                                }
                                if (bAudio)
                                {
                                    if (vc.Buffer.TryDequeue(out fa))
                                    {
                                        if (first)
                                        {
                                            recordingStart = fa.TimeStamp;
                                            first = false;
                                        }

                                        WriteFrame(fa, recordingStart, ref lastvideopts, ref maxAlarm, ref peakFrame,
                                            ref lastaudiopts);
                                    }
                                }
                            }

                            if (!Directory.Exists(folder + @"thumbs\"))
                                Directory.CreateDirectory(folder + @"thumbs\");

                            if (peakFrame != null && peakFrame.Value.Content != null)
                            {
                                try
                                {
                                    using (var ms = new MemoryStream(peakFrame.Value.Content))
                                    {
                                        using (var bmp = (Bitmap) Image.FromStream(ms))
                                        {
                                            bmp.Save(folder + @"thumbs\" + VideoFileName + "_large.jpg",
                                                MainForm.Encoder,
                                                MainForm.EncoderParams);
                                            Image.GetThumbnailImageAbort myCallback = ThumbnailCallback;
                                            using (
                                                var myThumbnail = bmp.GetThumbnailImage(96, 72, myCallback, IntPtr.Zero)
                                                )
                                            {
                                                myThumbnail.Save(folder + @"thumbs\" + VideoFileName + ".jpg",
                                                    MainForm.Encoder,
                                                    MainForm.EncoderParams);
                                            }
                                        }
                                        previewImage = folder + @"thumbs\" + VideoFileName + ".jpg";
                                        ms.Close();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ErrorHandler?.Invoke(ex.Message + ": " + ex.StackTrace);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            error = true;
                            ErrorHandler?.Invoke(ex.Message + " (" + ex.StackTrace + ")");
                        }
                        finally
                        {
                            if (_writer != null && _writer.IsOpen)
                            {
                                try
                                {
                                    Program.FfmpegMutex.WaitOne();
                                    _writer.Dispose();
                                }
                                catch (Exception ex)
                                {
                                    ErrorHandler?.Invoke(ex.Message);
                                }
                                finally
                                {
                                    try
                                    {
                                        Program.FfmpegMutex.ReleaseMutex();
                                    }
                                    catch (ObjectDisposedException)
                                    {
                                        //can happen on shutdown
                                    }
                                }

                                _writer = null;
                            }
                            if (bAudio)
                                vc.StopSaving();
                        }
                        if (_firstFrame)
                        {
                            error = true;
                        }
                        if (error)
                        {
                            try
                            {
                                if (File.Exists(videopath))
                                    FileOperations.Delete(videopath);
                            }
                            catch
                            {
                                // ignored
                            }
                            MainForm.RecordingThreads--;
                            ClearBuffer();

                            goto end;
                        }


                        string path = Dir.Entry + "video\\" + Camobject.directory + "\\" +
                                      VideoFileName;

                        string[] fnpath = (path + CodecExtension).Split('\\');
                        string fn = fnpath[fnpath.Length - 1];
                        //var fpath = Dir.Entry + "video\\" + Camobject.directory + "\\thumbs\\";
                        var fi = new FileInfo(path + CodecExtension);
                        var dSeconds = Convert.ToInt32((Helper.Now - recordingStart).TotalSeconds);

                        var ff = _filelist.FirstOrDefault(p => p.Filename.EndsWith(fn));
                        bool newfile = false;
                        if (ff == null)
                        {
                            ff = new FilesFile();
                            newfile = true;
                        }

                        ff.CreatedDateTicks = DateTime.Now.Ticks;
                        ff.Filename = fn;
                        ff.MaxAlarm = Math.Min(maxAlarm*100, 100);
                        ff.SizeBytes = fi.Length;
                        ff.DurationSeconds = dSeconds;
                        ff.IsTimelapse = false;
                        ff.IsMergeFile = false;
                        ff.AlertData = Helper.GetMotionDataPoints(_motionData);
                        _motionData.Clear();
                        ff.TriggerLevel = (100 - Camobject.detector.minsensitivity); //adjusted
                        ff.TriggerLevelMax = (100 - Camobject.detector.maxsensitivity);

                        if (newfile)
                        {
                            lock (_lockobject)
                                _filelist.Insert(0, ff);

                            MainForm.MasterFileAdd(new FilePreview(fn, dSeconds, Camobject.name, DateTime.Now.Ticks, 2,
                                Camobject.id, ff.MaxAlarm, false, false));
                            MainForm.NeedsMediaRefresh = Helper.Now;
                            if (Camobject.settings.cloudprovider.recordings)
                            {
                                CloudGateway.Upload(2, Camobject.id, path + CodecExtension);
                            }
                        }
                        AbortedAudio = false;

                    }
                    catch (Exception ex)
                    {
                        ErrorHandler?.Invoke(ex.Message);
                    }

                    if (!string.IsNullOrEmpty(Camobject.recorder.trigger))
                    {
                        string[] tid = Camobject.recorder.trigger.Split(',');
                        switch (tid[0])
                        {
                            case "1":
                                VolumeLevel vl = MainForm.InstanceReference.GetVolumeLevel(Convert.ToInt32(tid[1]));
                                if (vl != null)
                                    vl.ForcedRecording = false;
                                break;
                            case "2":
                                CameraWindow cw = MainForm.InstanceReference.GetCameraWindow(Convert.ToInt32(tid[1]));
                                if (cw != null)
                                {
                                    cw.ForcedRecording = false;
                                    var vc = cw.VolumeControl;
                                    if (vc != null)
                                    {
                                        vc.ForcedRecording = false;
                                    }
                                }
                                break;
                        }
                    }
                }
                finally
                {
                    MainForm.RecordingThreads--;
                }
                Camobject.newrecordingcount++;

                Notification?.Invoke(this, new NotificationType("NewRecording", Camobject.name, previewImage));

                end:

                LogToPlugin("Recording Stopped");
                DoAlert("recordingstopped");
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
            }
        }

        [HandleProcessCorruptedStateExceptions]
        private unsafe void WriteFrame(Helper.FrameAction fa, DateTime recordingStart, ref long lastvideopts, ref double maxAlarm,
            ref Helper.FrameAction? peakFrame, ref long lastaudiopts)
        {
            switch (fa.FrameType)
            {
                case Enums.FrameType.Video:
                    using (var ms = new MemoryStream(fa.Content))
                    {
                        using (var bmp = (Bitmap) Image.FromStream(ms))
                        {
                            var pts = (long) (fa.TimeStamp - recordingStart).TotalMilliseconds;
                            if (pts >= lastvideopts)
                            {
                                _writer.WriteVideoFrame(ResizeBitmap(bmp), pts);
                                lastvideopts = pts;
                            }
                        }

                        if (fa.Level > maxAlarm || peakFrame == null)
                        {
                            maxAlarm = fa.Level;
                            peakFrame = fa;
                        }

                        _motionData.Append(String.Format(CultureInfo.InvariantCulture, "{0:0.000}", Math.Min(fa.Level * 100, 100)));
                        
                        
                        
                        _motionData.Append(",");
                        ms.Close();
                    }
                    break;
                case Enums.FrameType.Audio:
                    fixed (byte* p = fa.Content)
                    {
                        var pts = (long) (fa.TimeStamp - recordingStart).TotalMilliseconds;
                        
                        _writer.WriteAudio(p, fa.DataLength, pts);
                        lastaudiopts = pts;
                        
                    }
                    break;
            }
            fa.Nullify();
        }

        public void CameraAlarm(object sender, EventArgs e)
        {
            double d = 0.3;
            if (Camera.Framerate > 3) //prevent division by zero
                d = 1d/Camera.Framerate;

            LastMovementDetected = Helper.Now.AddSeconds(d);


            if (!Calibrating)
            {
                if (Camobject.detector.recordondetect)
                {
                    StartSaving();
                }

                if (Camobject.ptzschedule.active && Camobject.ptzschedule.suspend)
                {
                    _suspendPTZSchedule = true;
                }

                if (Camobject.ftp.mode == 0)
                {
                    if (Camobject.ftp.enabled && Camobject.ftp.ready)
                    {
                        if ((Helper.Now - _lastFrameUploaded).TotalSeconds > Camobject.ftp.minimumdelay)
                        {
                            FtpFrame(); 

                        }
                    }
                }
                if (Camobject.savelocal.mode == 0)
                {
                    if (Camobject.savelocal.enabled)
                    {
                        if ((Helper.Now - _lastFrameSaved).TotalSeconds > Camobject.savelocal.minimumdelay)
                        {
                            SaveFrame();
                        }
                    }
                }

                
            }

            if (sender is Camera)
            {
                FlashCounter = Helper.Now.AddSeconds(10);
                MovementDetected = true;
                
                if (Camera?.Plugin != null)
                {
                    var o = Camera.Plugin.GetType();
                    var m = o.GetMethod("MotionDetect");
                    var r = (string) m?.Invoke(Camera.Plugin, null);
                    if (!string.IsNullOrEmpty(r))
                    {
                        ProcessAlertFromPlugin(r,"Motion Detected");
                    }
                }
                return;
            }

            var c = sender as CameraWindow;
            if (c!=null) //triggered from another camera
            {
                FlashCounter = Helper.Now.AddSeconds(10);
                DoAlert("alert");
                return;
            }

            if (sender is LocalServer || sender is VolumeLevel)
            {
                FlashCounter = Helper.Now.AddSeconds(10);
                DoAlert("alert");
                return;
            }

            if (sender is String || sender is IVideoSource)
            {
                if (Camobject.alerts.active && Camera != null)
                {
                    FlashCounter = Helper.Now.AddSeconds(10);
                    string msg = "";
                    var s = sender as string;
                    if (s != null)
                        msg = s;
                    else
                    {
                        if (sender is KinectStream)
                            msg = "Trip Wire";
                    }
                    DoAlert("alert", msg);                 
                }
            }
        }

        private void DoAlert(string type, string msg = "")
        {
            if (IsEdit)
                return;

            if (type == "alert")
            {
                if (Alerted)
                {
                    if ((Helper.Now - LastAlerted).TotalSeconds < Camobject.alerts.minimuminterval)
                    {
                        return;
                    }
                }
                
                Alerted = true;
                UpdateFloorplans(true);
                LastAlerted = Helper.Now;
                _raiseStop = true;
                RemoteCommand?.Invoke(this, new ThreadSafeCommand("bringtofrontcam," + Camobject.id));
                if (Camobject.detector.recordonalert && !Recording)
                {
                    StartSaving();
                }
            }

            var t = new Thread(() => AlertThread(type, msg, Camobject.id)) { Name = type + " (" + Camobject.id + ")", IsBackground = true };
            t.Start();
        }


        private void AlertThread(string mode, string msg, int oid)
        {
            if (Notification != null)
            {
                if (MainForm.AlertNotifications.Contains(mode))
                    Notification(this, new NotificationType(mode, Camobject.name, ""));
            }

            if (MainForm.Conf.ScreensaverWakeup)
                ScreenSaver.KillScreenSaver();

            string l = mode + ": " + Camobject.name;
            if (msg != "")
                l += " message: " + msg;
            MainForm.LogMessageToFile(l);

            Bitmap bmp = null;
            if (mode == "alert")
            {
                bmp = LastFrame;
            }

            using (var imageStream = new MemoryStream())
            {
                byte[] rawgrab = null;
                if (MainForm.Encoder != null && bmp!=null)
                {
                    //  Set the quality

                    var parameters = new EncoderParameters(1)
                                     {
                                         Param =
                                         {
                                             [0] =
                                                 new EncoderParameter(Encoder.Quality,
                                                 Camobject.ftp.quality)
                                         }
                                     };
                    try
                    {
                        bmp.Save(imageStream, MainForm.Encoder, parameters);
                        rawgrab = imageStream.ToArray();
                    }
                    catch (Exception ex)
                    {
                        MainForm.LogExceptionToFile(ex);
                    }
                }
                

                int i = 0;
                foreach (var ev in MainForm.Actions.Where(p => p.objectid == oid && p.objecttypeid == 2 && p.mode == mode))
                {
                    ProcessAlertEvent(ev.mode, rawgrab, msg, ev.type, ev.param1, ev.param2, ev.param3, ev.param4);
                }
                if (i>0)
                    MainForm.LastAlert = Helper.Now;

            }

            if (bmp != null)
            {
                if (Camobject.ftp.mode == 1)
                {
                    if (Camobject.ftp.enabled && Camobject.ftp.ready)
                    {
                        if ((Helper.Now - _lastFrameUploaded).TotalSeconds > Camobject.ftp.minimumdelay)
                        {
                            FtpFrame(bmp);
                        }
                    }
                        
                }
                if (Camobject.savelocal.mode == 1)   {
                    if (Camobject.savelocal.enabled)
                    {
                        if ((Helper.Now - _lastFrameSaved).TotalSeconds > Camobject.savelocal.minimumdelay)
                        {
                            SaveFrame();
                        }
                    }
                }
                bmp.Dispose();
            }

        }

        private void ProcessAlertEvent(string mode, byte[] rawgrab, string pluginmessage, string type, string param1, string param2, string param3, string param4)
        {
            string id = Camobject.id.ToString(CultureInfo.InvariantCulture);

            param1 = param1.Replace("{ID}", id).Replace("{NAME}", Camobject.name).Replace("{MSG}", pluginmessage);
            param2 = param2.Replace("{ID}", id).Replace("{NAME}", Camobject.name).Replace("{MSG}", pluginmessage);
            param3 = param3.Replace("{ID}", id).Replace("{NAME}", Camobject.name).Replace("{MSG}", pluginmessage);
            param4 = param4.Replace("{ID}", id).Replace("{NAME}", Camobject.name).Replace("{MSG}", pluginmessage);

            try
            {
                switch (type)
                {
                    case "Exe":
                        {
                            if (param1.ToLower() == "ispy.exe" || param1.ToLower() == "ispy")
                            {
                                MainForm.InstanceReference.ProcessCommandString(param2);
                            }
                            else
                            {
                                try
                                {

                                    var startInfo = new ProcessStartInfo
                                                        {
                                                            UseShellExecute = true,
                                                            FileName = param1,
                                                            Arguments = param2
                                                        };
                                    try
                                    {
                                        var fi = new FileInfo(param1);
                                        if (fi.DirectoryName != null)
                                            startInfo.WorkingDirectory = fi.DirectoryName;
                                    }
                                    catch
                                    {
                                        // ignored
                                    }
                                    if (!MainForm.Conf.CreateAlertWindows)
                                    {
                                        startInfo.CreateNoWindow = true;
                                        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                    }

                                    Process.Start(startInfo);
                                }
                                catch (Exception e)
                                {
                                    ErrorHandler?.Invoke(e.Message);
                                }
                            }
                        }
                        break;
                    case "URL":
                        {
                            bool postgrab = Convert.ToBoolean(param2) && rawgrab!=null && rawgrab.Length > 0;
                            if (postgrab)
                            {
                                const string file = "grab.jpg";
                                const string paramName = "file";
                                const string contentType = "image/jpeg";
                                string boundary = "---------------------------" + Helper.Now.Ticks.ToString("x");
                                byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

                                var wr = (HttpWebRequest) WebRequest.Create(param1);
                                wr.ContentType = "multipart/form-data; boundary=" + boundary;
                                wr.Method = "POST";
                                wr.KeepAlive = true;
                                wr.Credentials = CredentialCache.DefaultCredentials;


                                using (Stream rs = wr.GetRequestStream())
                                {
                                    rs.Write(boundarybytes, 0, boundarybytes.Length);

                                    const string headerTemplate =
                                        "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                                    string header = string.Format(headerTemplate, paramName, file, contentType);
                                    byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                                    rs.Write(headerbytes, 0, headerbytes.Length);

                                    rs.Write(rawgrab, 0, rawgrab.Length);

                                    byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                                    rs.Write(trailer, 0, trailer.Length);
                                    rs.Close();

                                    WebResponse wresp = null;
                                    try
                                    {
                                        wresp = wr.GetResponse();
                                        Stream stream2 = wresp.GetResponseStream();
                                        if (stream2 != null)
                                        {
                                            var reader2 = new StreamReader(stream2);
                                            reader2.ReadToEnd();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ErrorHandler?.Invoke(ex.Message);
                                        wresp?.Close();
                                    }
                                }

                            }
                            else
                            {
                                var request = (HttpWebRequest) WebRequest.Create(param1);
                                request.Credentials = CredentialCache.DefaultCredentials;
                                var response = (HttpWebResponse) request.GetResponse();

                                // Get the stream associated with the response.
                                Stream receiveStream = response.GetResponseStream();

                                // Pipes the stream to a higher level stream reader with the required encoding format. 
                                if (receiveStream != null)
                                {
                                    var readStream = new StreamReader(receiveStream, Encoding.UTF8);
                                    readStream.ReadToEnd();
                                    response.Close();
                                    readStream.Close();
                                    receiveStream.Close();
                                }
                                response.Close();
                            }
                        }
                        break;
                    case "NM": //network message
                        switch (param1)
                        {
                            case "TCP":
                                {
                                    IPAddress ip;
                                    if (IPAddress.TryParse(param2, out ip))
                                    {
                                        int port;
                                        if (int.TryParse(param3, out port))
                                        {
                                            using (var tcpClient = new TcpClient())
                                            {
                                                try
                                                {
                                                    tcpClient.Connect(ip, port);
                                                    using (var networkStream = tcpClient.GetStream())
                                                    {
                                                        using (var clientStreamWriter = new StreamWriter(networkStream))
                                                        {
                                                            clientStreamWriter.Write(param4);
                                                            clientStreamWriter.Flush();
                                                            clientStreamWriter.Close();
                                                        }
                                                        networkStream.Close();
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    ErrorHandler?.Invoke(ex.Message);
                                                }
                                                tcpClient.Close();
                                            }

                                        }
                                    }
                                }

                                break;
                            case "UDP":
                                {
                                    IPAddress ip;
                                    if (IPAddress.TryParse(param2, out ip))
                                    {
                                        int port;
                                        if (int.TryParse(param3, out port))
                                        {
                                            using (var udpClient = new UdpClient())
                                            {
                                                try
                                                {

                                                    udpClient.Connect(ip, port);
                                                    var cmd = Encoding.ASCII.GetBytes(param4);
                                                    udpClient.Send(cmd, cmd.Length);

                                                }
                                                catch (Exception ex)
                                                {
                                                    ErrorHandler?.Invoke(ex.Message);
                                                }
                                                finally
                                                {
                                                    udpClient.Close();
                                                }
                                            }

                                        }
                                    }
                                }

                                break;
                        }

                        break;
                    case "S":
                        try
                        {
                            using (var sp = new SoundPlayer(param1))
                            {
                                sp.Play();
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorHandler?.Invoke(ex.Message);
                        }
                        break;
                    case "ATC":
                        AudioSynth.Play(param1, this);
                        break;
                    case "SW":
                        RemoteCommand?.Invoke(this, new ThreadSafeCommand("show"));
                        break;
                    case "B":
                        Console.Beep();
                        break;
                    case "M":
                        MainForm.InstanceReference.Maximise(this, false);
                        break;
                    case "TA":
                        {
                            
                                string[] tid = param1.Split(',');
                                switch (tid[0])
                                {
                                    case "1":
                                        VolumeLevel vl =
                                            MainForm.InstanceReference.GetVolumeLevel(Convert.ToInt32(tid[1]));
                                        vl?.MicrophoneAlarm(this, EventArgs.Empty);
                                        break;
                                    case "2":
                                        CameraWindow cw =
                                            MainForm.InstanceReference.GetCameraWindow(Convert.ToInt32(tid[1]));
                                        if (cw != null && cw!=this) //prevent recursion
                                            cw.CameraAlarm(this, EventArgs.Empty);
                                        break;
                                }
                        }
                        break;
                    case "SOO":
                        {
                            
                                string[] tid = param1.Split(',');
                                switch (tid[0])
                                {
                                    case "1":
                                        VolumeLevel vl =
                                            MainForm.InstanceReference.GetVolumeLevel(Convert.ToInt32(tid[1]));
                                        vl?.Enable();
                                        break;
                                    case "2":
                                        CameraWindow cw =
                                            MainForm.InstanceReference.GetCameraWindow(Convert.ToInt32(tid[1]));
                                        cw?.Enable();
                                        break;
                                }
                            
                        }
                        break;
                    case "SOF":
                        {
                            
                                string[] tid = param1.Split(',');
                                switch (tid[0])
                                {
                                    case "1":
                                        VolumeLevel vl =
                                            MainForm.InstanceReference.GetVolumeLevel(Convert.ToInt32(tid[1]));
                                        vl?.Disable();
                                        break;
                                    case "2":
                                        CameraWindow cw =
                                            MainForm.InstanceReference.GetCameraWindow(Convert.ToInt32(tid[1]));
                                        cw?.Disable();
                                        break;
                                }
                        }
                        break;
                    case "E":
                        {
                            if (Camobject.settings.messaging)
                            {
                                bool includeGrab = Convert.ToBoolean(param2);

                                string subject = MailMerge(MainForm.Conf.MailAlertSubject, mode, Recording,
                                    pluginmessage);
                                string message = MailMerge(MainForm.Conf.MailAlertBody, mode, Recording, pluginmessage);

                                message += MainForm.Conf.AppendLinkText;


                                if (includeGrab)
                                    WsWrapper.SendAlertWithImage(param1, subject, message, rawgrab);
                                else
                                    WsWrapper.SendAlert(param1, subject, message);
                            }

                        }
                        break;
                    case "SMS":
                        {
                            if (Camobject.settings.messaging)
                            {
                                string message = MailMerge(MainForm.Conf.SMSAlert, mode, Recording, pluginmessage);
                                if (message.Length > 160)
                                    message = message.Substring(0, 159);

                                WsWrapper.SendSms(param1, message);
                            }
                        }
                        break;
                    case "TM":
                        {
                            if (Camobject.settings.messaging)
                            {
                                string message = MailMerge(MainForm.Conf.SMSAlert, mode, Recording, pluginmessage);
                                if (message.Length > 160)
                                    message = message.Substring(0, 159);

                                WsWrapper.SendTweet(message + " " + MainForm.Webserver + "/mobile/");
                            }
                        }
                        break;
                    case "MO":
                    {
                            NativeCalls.WakeScreen();
                    }
                        break;

                }
            }
            catch (Exception ex)
            {
                ErrorHandler?.Invoke(ex.Message);
            }
        }

        private void VideoDeviceVideoFinished(object sender, PlayingFinishedEventArgs e)
        {
            if (IsReconnect)
                return;

            switch (e.ReasonToFinishPlaying)
            {
                case ReasonToFinishPlaying.DeviceLost:
                    SetErrorState("Device Lost");
                    break;
                case ReasonToFinishPlaying.EndOfStreamReached:
                    SetErrorState("End of Stream");
                    break;
                case ReasonToFinishPlaying.VideoSourceError:
                    SetErrorState("Source Error");
                    break;
                case ReasonToFinishPlaying.StoppedByUser:
                    Disable(false);
                    break;
            }

            //LastFrame = null;
        }

        private void SetErrorState(string reason)
        {
            VideoSourceErrorMessage = reason;
            if (!VideoSourceErrorState)
            {
                
                VideoSourceErrorState = true;
                ErrorHandler?.Invoke(reason);

                if (_reconnectTime == DateTime.MinValue)
                {
                    _reconnectTime = Helper.Now;
                }
                if (_errorTime == DateTime.MinValue)
                    _errorTime = Helper.Now;
                var vl = VolumeControl;
                if (vl?.AudioSource != null && vl.IsEnabled)
                {
                    if ((Camera!=null && vl.AudioSource == Camera.VideoSource) || vl.IsClone)
                    {
                        vl.AudioSourceErrorMessage = reason;
                        vl.AudioSourceErrorState = true;

                        vl.AudioSource.LevelChanged -= vl.AudioDeviceLevelChanged;
                        vl.AudioSource.DataAvailable -= vl.AudioDeviceDataAvailable;
                        vl.AudioSource.AudioFinished -= vl.AudioDeviceAudioFinished;
                    }
                }
            }
            else
            {
                _reconnectFailCount++;
                if (_reconnectFailCount == 1)
                {
                    DoAlert("reconnectfailed");
                }
            }
            _requestRefresh = true;
        }

        public void Disable(bool stopSource = true)
        {
            if (_disabling)
                return;

            if (InvokeRequired)
            {
                Invoke(new Delegates.DisableDelegate(Disable), stopSource);
                return;
            }
            
            lock (_lockobject)
            {
                if (!IsEnabled)
                    return;
                IsEnabled = false;
            }
            
            _disabling = true;
            IsReconnect = false;

            try
            {
                RecordSwitch(false);

                if (VolumeControl != null && VolumeControl.IsEnabled)
                    VolumeControl.Disable();

                
                    if (MainForm.InstanceReference.TalkCamera == this)
                        MainForm.InstanceReference.TalkTo(this, false);
                

                Application.DoEvents();

                if (SavingTimeLapse)
                {
                    CloseTimeLapseWriter();
                }

                StopSaving();
                if (Camera != null)
                {

                    Calibrating = false;

                    Camera.NewFrame -= CameraNewFrame;
                    Camera.Alarm -= CameraAlarm;
                    Camera.PlayingFinished -= VideoDeviceVideoFinished;
                    Camera.ErrorHandler -= CameraWindow_ErrorHandler;

                    if (Camera?.VideoSource != null)
                    {

                        if (Camera.Plugin != null)
                        {
                            //wait for plugin to exit
                            int i = 0;
                            while (Camera.PluginRunning && i < 10)
                            {
                                Thread.Sleep(100);
                                i++;
                            }
                        }

                        var source = Camera.VideoSource as KinectStream;
                        if (source != null)
                        {
                            source.TripWire -= CameraAlarm;
                        }

                        var source1 = Camera.VideoSource as KinectNetworkStream;
                        if (source1 != null)
                        {
                            //remove the alert handler from the source stream
                            source1.AlertHandler -= CameraWindow_AlertHandler;
                        }

                        var audiostream = Camera.VideoSource as ISupportsAudio;
                        if (audiostream != null)
                        {
                            audiostream.HasAudioStream -= VideoSourceHasAudioStream;
                        }

                        if (!IsClone)
                        {
                            Application.DoEvents();

                            if (stopSource)
                            {
                                lock (_lockobject)
                                {
                                    try
                                    {
                                        Camera.SignalToStop();
                                        if (Camera.VideoSource is VideoCaptureDevice && !ShuttingDown)
                                        {
                                            Camera.VideoSource.WaitForStop();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ErrorHandler?.Invoke(ex.Message);
                                    }
                                }
                            }

                            if (Camera.VideoSource is XimeaVideoSource)
                            {
                                XimeaSource = null;
                            }
                        }
                        else
                        {
                            Camera.DisconnectNewFrameEvent();
                        }
                    }

                    var vl = VolumeControl;
                    if (vl?.AudioSource != null && vl.IsEnabled)
                    {
                        if ((Camera != null && vl.AudioSource == Camera.VideoSource) || vl.IsClone)
                        {
                            vl.AudioSource.LevelChanged -= vl.AudioDeviceLevelChanged;
                            vl.AudioSource.DataAvailable -= vl.AudioDeviceDataAvailable;
                            vl.AudioSource.AudioFinished -= vl.AudioDeviceAudioFinished;
                        }
                    }

                    LastFrame = null;
                    Camera = null; //setter calls dispose
                }
                BackgroundColor = MainForm.BackgroundColor;
                Camobject.settings.active = false;
                InactiveRecord = 0;
                _timeLapseTotal = _timeLapseFrameCount = 0;
                ForcedRecording = false;

                MovementDetected = false;
                Alerted = false;
                FlashCounter = DateTime.MinValue;
                ReconnectCount = 0;
                PTZNavigate = false;
                UpdateFloorplans(false);
                MainForm.NeedsSync = true;
                _errorTime = _reconnectTime = DateTime.MinValue;
                _autoofftimer = 0;

                ClearBuffer();

                if (!ShuttingDown)
                    _requestRefresh = true;

                LastFrame = null;

                CameraDisabled?.Invoke(this, EventArgs.Empty);

                //GC.Collect();
            }
            catch (Exception ex)
            {
                ErrorHandler?.Invoke(ex.Message);
            }
            _disabling = false;

        }

        private void ClearBuffer()
        {
            lock (_lockobject)
            {
                Helper.FrameAction fa;
                while (Buffer.TryDequeue(out fa))
                {
                    fa.Nullify();
                }
            }
            
        }


        internal string SourceType
        {
            get
            {
                switch (Camobject.settings.sourceindex)
                {
                    default:
                        return "JPEG";
                    case 1:
                        return "MJPEG";
                    case 2:
                        return "FFMPEG";
                    case 3:
                        return "Local Device";
                    case 4:
                        return "Desktop";
                    case 5:
                        return "VLC File/Stream";
                    case 6:
                        return "XIMEA Device";
                    case 7:
                        return "Kinect Device";
                    case 8:
                        return "Custom Provider";
                    case 10:
                        return "Cloned";
                }
            }

        }

        private void SetVideoSourceProperty(VideoCaptureDevice device, VideoProcAmpProperty prop, string n)
        {
            try
            {
                int v;
                if (Int32.TryParse(Nv(Camobject.settings.procAmpConfig, n), out v))
                {
                    if (v > Int32.MinValue)
                    {
                        int fv;
                        if (Int32.TryParse(Nv(Camobject.settings.procAmpConfig, "f" + n), out fv))
                        {
                            device.SetProperty(prop, v, (VideoProcAmpFlags)fv);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler?.Invoke(ex.Message);
            }
        }

        private volatile bool _enabling, _disabling;

        public void SetVolumeLevel(int micid)
        {
            Camobject.settings.micpair = micid;
            if (micid == -1)
            {
                if (VolumeControl != null)
                    VolumeControl.CameraControl = null;
                VolumeControl = null;
                return;
            }
            VolumeControl = MainClass.GetVolumeLevel(micid);
            if (VolumeControl != null)
            {
                VolumeControl.CameraControl = this;
                VolumeControl.Micobject.settings.buffer = Camobject.recorder.bufferseconds;
            }
        }

        public void Enable()
        {
            if (_enabling)
                return;

            if (InvokeRequired)
            {
                Invoke(new Delegates.EnableDelegate(Enable));
                return;
            }

            lock (_lockobject)
            {
                if (IsEnabled)
                    return;
                IsEnabled = true;
            }
            _enabling = true;

            try
            {
            IsReconnect = false;
            Seekable = false;
            IsClone = Camobject.settings.sourceindex == 10;
            VideoSourceErrorState = false;
            VideoSourceErrorMessage = "";

            string ckies, hdrs;
            switch (Camobject.settings.sourceindex)
            {
                case 0:
                    ckies = Camobject.settings.cookies ?? "";
                    ckies = ckies.Replace("[USERNAME]", Camobject.settings.login);
                    ckies = ckies.Replace("[PASSWORD]", Camobject.settings.password);
                    ckies = ckies.Replace("[CHANNEL]", Camobject.settings.ptzchannel);

                    hdrs = Camobject.settings.headers ?? "";
                    hdrs = hdrs.Replace("[USERNAME]", Camobject.settings.login);
                    hdrs = hdrs.Replace("[PASSWORD]", Camobject.settings.password);
                    hdrs = hdrs.Replace("[CHANNEL]", Camobject.settings.ptzchannel);

                    var jpegSource = new JpegStream(Camobject.settings.videosourcestring)
                                        {
                                            Login = Camobject.settings.login,
                                            Password = Camobject.settings.password,
                                            ForceBasicAuthentication = Camobject.settings.forcebasic,
                                            RequestTimeout = Camobject.settings.timeout,
                                            UseHttp10 = Camobject.settings.usehttp10,
                                            HttpUserAgent = Camobject.settings.useragent,
                                            Cookies = ckies,
                                            Headers = hdrs
                                        };

                    OpenVideoSource(jpegSource, true);

                    if (Camobject.settings.frameinterval != 0)
                        jpegSource.FrameInterval = Camobject.settings.frameinterval;

                    break;
                case 1:
                    ckies = Camobject.settings.cookies ?? "";
                    ckies = ckies.Replace("[USERNAME]", Camobject.settings.login);
                    ckies = ckies.Replace("[PASSWORD]", Camobject.settings.password);
                    ckies = ckies.Replace("[CHANNEL]", Camobject.settings.ptzchannel);

                    hdrs = Camobject.settings.headers ?? "";
                    hdrs = hdrs.Replace("[USERNAME]", Camobject.settings.login);
                    hdrs = hdrs.Replace("[PASSWORD]", Camobject.settings.password);
                    hdrs = hdrs.Replace("[CHANNEL]", Camobject.settings.ptzchannel);

                    var mjpegSource = new MJPEGStream(Camobject.settings.videosourcestring)
                                        {
                                            Login = Camobject.settings.login,
                                            Password = Camobject.settings.password,
                                            ForceBasicAuthentication = Camobject.settings.forcebasic,
                                            RequestTimeout = Camobject.settings.timeout,
                                            HttpUserAgent = Camobject.settings.useragent,
                                            DecodeKey = Camobject.decodekey,
                                            Cookies = ckies,
                                            Headers = hdrs
                                        };
                    OpenVideoSource(mjpegSource, true);
                    break;
                case 2:
                    string url = Camobject.settings.videosourcestring;
                    var ffmpegSource = new FfmpegStream(url)
                                        {
                                            Cookies = Camobject.settings.cookies,
                                            AnalyzeDuration = Camobject.settings.analyseduration,
                                            Timeout = Camobject.settings.timeout,
                                            UserAgent = Camobject.settings.useragent,
                                            Headers = Camobject.settings.headers,
                                            RTSPMode = Camobject.settings.rtspmode
                                        };
                    OpenVideoSource(ffmpegSource, true);
                    break;
                case 3:
                    string moniker = Camobject.settings.videosourcestring;


                    var videoSource = new VideoCaptureDevice(moniker);
                    string[] wh = Camobject.resolution.Split('x');
                    var sz = new Size(Convert.ToInt32(wh[0]), Convert.ToInt32(wh[1]));

                        
                    string precfg = Nv("video");
                    bool found = false;

                    if (Nv("capturemode") != "snapshots")
                    {
                        VideoCapabilities[] videoCapabilities = videoSource.VideoCapabilities;
                        videoSource.ProvideSnapshots = false;
                        foreach (VideoCapabilities capabilty in videoCapabilities)
                        {

                            string item = string.Format(VideoSource.VideoFormatString, capabilty.FrameSize.Width,
                                Math.Abs(capabilty.FrameSize.Height), capabilty.AverageFrameRate, capabilty.BitCount);
                            if (precfg == item)
                            {
                                videoSource.VideoResolution = capabilty;
                                found = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        precfg = Nv("snapshots");
                        videoSource.ProvideSnapshots = true;
                        VideoCapabilities[] videoCapabilities = videoSource.SnapshotCapabilities;
                        foreach (VideoCapabilities capabilty in videoCapabilities)
                        {

                            string item = string.Format(VideoSource.SnapshotFormatString, capabilty.FrameSize.Width,
                                Math.Abs(capabilty.FrameSize.Height), capabilty.AverageFrameRate, capabilty.BitCount);
                            if (precfg == item)
                            {
                                videoSource.VideoResolution = capabilty;
                                found = true;
                                break;
                            }
                        }
                    }
                    if (!found)
                    {
                        var vc = videoSource.VideoCapabilities.Where(p => p.FrameSize == sz).ToList();
                        if (vc.Count > 0)
                        {
                            var vc2 = vc.FirstOrDefault(p => p.AverageFrameRate == Camobject.settings.framerate) ??
                                        vc.FirstOrDefault();
                            videoSource.VideoResolution = vc2;
                            found = true;
                        }
                        if (!found)
                        {
                            //first available
                            var vcf = videoSource.VideoCapabilities.FirstOrDefault();
                            if (vcf != null)
                                videoSource.VideoResolution = vcf;
                            //else
                            //{
                            //    dont do this, not having an entry is ok for some video providers
                            //    throw new Exception("Unable to find a video format for the capture device");
                            //}
                        }
                    }

                    if (Camobject.settings.crossbarindex != -1 && videoSource.CheckIfCrossbarAvailable())
                    {
                        var cbi =
                            videoSource.AvailableCrossbarVideoInputs.FirstOrDefault(
                                p => p.Index == Camobject.settings.crossbarindex);
                        if (cbi != null)
                        {
                            videoSource.CrossbarVideoInput = cbi;
                        }
                    }

                    OpenVideoSource(videoSource, true);


                    break;
                case 4:
                    Rectangle area = Rectangle.Empty;
                    if (!string.IsNullOrEmpty(Camobject.settings.desktoparea))
                    {
                        var i = System.Array.ConvertAll(Camobject.settings.desktoparea.Split(','), int.Parse);
                        area = new Rectangle(i[0], i[1], i[2], i[3]);
                    }
                    var desktopSource = new DesktopStream(Convert.ToInt32(Camobject.settings.videosourcestring),
                        area) {MousePointer = Camobject.settings.desktopmouse};
                    if (Camobject.settings.frameinterval != 0)
                        desktopSource.FrameInterval = Camobject.settings.frameinterval;
                    OpenVideoSource(desktopSource, true);

                    break;
                case 5:
                    List<String> inargs = Camobject.settings.vlcargs.Split(Environment.NewLine.ToCharArray(),
                        StringSplitOptions.RemoveEmptyEntries).ToList();
                    var vlcSource = new VlcStream(Camobject.settings.videosourcestring, inargs.ToArray())
                                    {
                                        TimeOut = Camobject.settings.timeout
                                    };

                    OpenVideoSource(vlcSource, true);
                    break;
                case 6:
                    if (XimeaSource == null || !XimeaSource.IsRunning)
                        XimeaSource =
                            new XimeaVideoSource(Convert.ToInt32(Nv(Camobject.settings.namevaluesettings, "device")));
                    OpenVideoSource(XimeaSource, true);
                    break;
                case 7:
                    var tw = false;
                    try
                    {
                        if (!string.IsNullOrEmpty(Nv(Camobject.settings.namevaluesettings, "TripWires")))
                            tw = Convert.ToBoolean(Nv(Camobject.settings.namevaluesettings, "TripWires"));
                        var ks = new KinectStream(Nv(Camobject.settings.namevaluesettings, "UniqueKinectId"),
                            Convert.ToBoolean(Nv(Camobject.settings.namevaluesettings, "KinectSkeleton")), tw);
                        if (Nv(Camobject.settings.namevaluesettings, "StreamMode") != "")
                            ks.StreamMode = Convert.ToInt32(Nv(Camobject.settings.namevaluesettings, "StreamMode"));
                        OpenVideoSource(ks, true);
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler?.Invoke(ex.Message);
                    }
                    break;
                case 8:
                    switch (Nv(Camobject.settings.namevaluesettings, "custom"))
                    {
                        case "Network Kinect":
                            // open the network kinect video stream
                            OpenVideoSource(new KinectNetworkStream(Camobject.settings.videosourcestring), true);
                            break;
                        default:
                            lock (_lockobject)
                            {
                                IsEnabled = false;
                            }
                            throw new Exception("No custom provider found for " +
                                                Nv(Camobject.settings.namevaluesettings, "custom"));
                    }
                    break;
                case 9:
                    //there is no 9, spooky hey?
                    break;
                case 10:
                    int icam;
                    if (Int32.TryParse(Camobject.settings.videosourcestring, out icam))
                    {
                        var cw = MainForm.InstanceReference.GetCameraWindow(icam);
                        if (cw != null)
                        {
                            OpenVideoSource(cw);
                        }

                    }
                    break;
            }

            if (Camera != null)
            {
                IMotionDetector motionDetector = null;
                IMotionProcessing motionProcessor = null;

                switch (Camobject.detector.type)
                {
                    default:
                        motionDetector = new TwoFramesDifferenceDetector(Camobject.settings.suppressnoise);
                        break;
                    case "Custom Frame":
                        motionDetector = new CustomFrameDifferenceDetector(Camobject.settings.suppressnoise,
                            Camobject.detector.keepobjectedges);
                        break;
                    case "Background Modeling":
                        motionDetector = new SimpleBackgroundModelingDetector(Camobject.settings.suppressnoise,
                            Camobject.detector.keepobjectedges);
                        break;
                    case "Two Frames (Color)":
                        motionDetector = new TwoFramesColorDifferenceDetector(Camobject.settings.suppressnoise);
                        break;
                    case "Custom Frame (Color)":
                        motionDetector = new CustomFrameColorDifferenceDetector(
                            Camobject.settings.suppressnoise,
                            Camobject.detector.keepobjectedges);
                        break;
                    case "Background Modeling (Color)":
                        motionDetector =
                            new SimpleColorBackgroundModelingDetector(Camobject.settings.suppressnoise,
                                Camobject.detector.
                                    keepobjectedges);
                        break;
                    case "None":
                        break;
                }

                if (motionDetector != null)
                {
                    switch (Camobject.detector.postprocessor)
                    {
                        case "Grid Processing":
                            motionProcessor = new GridMotionAreaProcessing
                                                {
                                                    HighlightColor =
                                                        ColorTranslator.FromHtml(Camobject.detector.color),
                                                    HighlightMotionGrid = Camobject.detector.highlight
                                                };
                            break;
                        case "Object Tracking":
                            motionProcessor = new BlobCountingObjectsProcessing
                                                {
                                                    HighlightColor =
                                                        ColorTranslator.FromHtml(Camobject.detector.color),
                                                    HighlightMotionRegions = Camobject.detector.highlight,
                                                    MinObjectsHeight = Camobject.detector.minheight,
                                                    MinObjectsWidth = Camobject.detector.minwidth
                                                };

                            break;
                        case "Border Highlighting":
                            motionProcessor = new MotionBorderHighlighting
                                                {
                                                    HighlightColor =
                                                        ColorTranslator.FromHtml(Camobject.detector.color)
                                                };
                            break;
                        case "Area Highlighting":
                            motionProcessor = new MotionAreaHighlighting
                                                {
                                                    HighlightColor =
                                                        ColorTranslator.FromHtml(Camobject.detector.color)
                                                };
                            break;
                        case "None":
                            break;
                    }

                    if (Camera.MotionDetector != null)
                    {
                        Camera.MotionDetector.Reset();
                        Camera.MotionDetector = null;
                    }

                    Camera.MotionDetector = motionProcessor == null
                        ? new MotionDetector(motionDetector)
                        : new MotionDetector(motionDetector, motionProcessor);

                    Camera.AlarmLevel = Helper.CalculateTrigger(Camobject.detector.minsensitivity);
                    Camera.AlarmLevelMax = Helper.CalculateTrigger(Camobject.detector.maxsensitivity);
                    NeedMotionZones = true;
                }
                else
                {
                    Camera.MotionDetector = null;
                }

                LastMovementDetected = Helper.Now;

                ClearBuffer();

                if (!Camera.IsRunning)
                {
                    Calibrating = true;
                    _lastRun = Helper.Now.Ticks;
                    Camera.Start();
                }
                if (Camera.VideoSource is XimeaVideoSource)
                {
                    //need to set these after the camera starts
                    try
                    {
                        XimeaSource.SetParam(PRM.IMAGE_DATA_FORMAT, IMG_FORMAT.RGB24);
                    }
                    catch (ApplicationException)
                    {
                        XimeaSource.SetParam(PRM.IMAGE_DATA_FORMAT, IMG_FORMAT.MONO8);
                    }
                    XimeaSource.SetParam(CameraParameter.OffsetX,
                        Convert.ToInt32(Nv(Camobject.settings.namevaluesettings, "x")));
                    XimeaSource.SetParam(CameraParameter.OffsetY,
                        Convert.ToInt32(Nv(Camobject.settings.namevaluesettings, "y")));
                    float gain;
                    float.TryParse(Nv(Camobject.settings.namevaluesettings, "gain"), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out gain);
                    XimeaSource.SetParam(CameraParameter.Gain, gain);
                    float exp;
                    float.TryParse(Nv(Camobject.settings.namevaluesettings, "exposure"), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out exp);
                    XimeaSource.SetParam(CameraParameter.Exposure, exp*1000);
                    XimeaSource.SetParam(CameraParameter.Downsampling,
                        Convert.ToInt32(Nv(Camobject.settings.namevaluesettings, "downsampling")));
                    XimeaSource.SetParam(CameraParameter.Width,
                        Convert.ToInt32(Nv(Camobject.settings.namevaluesettings, "width")));
                    XimeaSource.SetParam(CameraParameter.Height,
                        Convert.ToInt32(Nv(Camobject.settings.namevaluesettings, "height")));
                    XimeaSource.FrameInterval =
                        (int) (1000.0f/XimeaSource.GetParamFloat(CameraParameter.FramerateMax));
                }



                if (File.Exists(Camobject.settings.maskimage))
                {
                    Camera.Mask = (Bitmap) Image.FromFile(Camobject.settings.maskimage);
                }


            }

            Camobject.settings.active = true;
            UpdateFloorplans(false);

            _timeLapseTotal = _timeLapseFrameCount = 0;
            InactiveRecord = 0;
            MovementDetected = false;

            Alerted = false;
            PTZNavigate = false;
            Camobject.ftp.ready = true;
            _lastRun = Helper.Now.Ticks;
            MainForm.NeedsSync = true;
            ReconnectCount = 0;
            _dtPTZLastCheck = DateTime.Now;
                
            _firstFrame = true;
            _autoofftimer = 0;

            if (Camera != null)
            {
                Camera.ZFactor = 1;
            }
            _requestRefresh = true;


            SetVolumeLevel(Camobject.settings.micpair);
            if (VolumeControl != null)
            {
                VolumeControl.Micobject.settings.buffer = Camobject.recorder.bufferseconds;
                VolumeControl.Enable();
            }


            SetVideoSize();

            //cloned initialisation goes here
                CameraEnabled?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                ErrorHandler?.Invoke(ex.Message);
            }
            _enabling = false;
        }

        public void SetVideoSourceProperties()
        {
            var videoSource = Camera.VideoSource as VideoCaptureDevice;
            if (videoSource != null && !string.IsNullOrEmpty(Camobject.settings.procAmpConfig) && videoSource.SupportsProperties && Nv("manual")!="false")
            {
                try
                {
                    SetVideoSourceProperty(videoSource, VideoProcAmpProperty.Brightness, "b");
                    SetVideoSourceProperty(videoSource, VideoProcAmpProperty.Contrast, "c");
                    SetVideoSourceProperty(videoSource, VideoProcAmpProperty.Hue, "h");
                    SetVideoSourceProperty(videoSource, VideoProcAmpProperty.Saturation, "s");
                    SetVideoSourceProperty(videoSource, VideoProcAmpProperty.Sharpness, "sh");
                    SetVideoSourceProperty(videoSource, VideoProcAmpProperty.Gamma, "gam");
                    SetVideoSourceProperty(videoSource, VideoProcAmpProperty.ColorEnable, "ce");
                    SetVideoSourceProperty(videoSource, VideoProcAmpProperty.WhiteBalance, "wb");
                    SetVideoSourceProperty(videoSource, VideoProcAmpProperty.BacklightCompensation, "bc");
                    SetVideoSourceProperty(videoSource, VideoProcAmpProperty.Gain, "g");
                }
                catch
                {
                    //ignore - not supported on the device
                }
            }
        }

        void VideoSourceHasAudioStream(object sender, EventArgs eventArgs)
        {
            if (InvokeRequired)
            {
                Invoke(new Delegates.AddAudioDelegate(AddAudioStream));
                return;
            }
            AddAudioStream();
        }

        private void AddAudioStream()
        {
            if (Camera != null && !Camobject.settings.ignoreaudio)
            {
                var vl = VolumeControl;
                if (vl == null)
                {
                    vl = MainForm.InstanceReference.AddCameraMicrophone(Camobject.id, Camobject.name + " mic");
                    Camobject.settings.micpair = vl.Micobject.id;
                    vl.Micobject.alerts.active = false;
                    vl.Micobject.detector.recordonalert = false;
                    vl.Micobject.detector.recordondetect = false;
                    SetVolumeLevel(vl.Micobject.id);
                }

                var m = vl.Micobject;
                if (m != null && m.settings.typeindex==4)
                {
                    var c = Camera.VideoSource as ISupportsAudio;
                    if (c?.RecordingFormat != null)
                    {
                        m.settings.samples = c.RecordingFormat.SampleRate;
                        m.settings.channels = c.RecordingFormat.Channels;
                    }

                    m.settings.buffer = Camobject.recorder.bufferseconds;
                    m.settings.bits = 16;
                    vl.Enable();
                }
            }
        }

        public string Nv(string name)
        {
            return Nv(Camobject.settings.namevaluesettings, name);
        }

        public string Nv(string csv, string name)
        {
            if (string.IsNullOrEmpty(csv))
                return "";
            name = name.ToLower().Trim();
            string[] settings = csv.Split(',');
            foreach (string[] nv in settings.Select(s => s.Split('=')).Where(nv => nv[0].ToLower().Trim() == name))
            {
                return nv[1];
            }
            return "";
        }

        private static int CalcBitRate(int q)
        {
            return 8000 * (Convert.ToInt32(Math.Pow(2, (q - 1))));
        }

        public void UpdateFloorplans(bool isAlert)
        {
            foreach (
                objectsFloorplan ofp in
                    MainForm.FloorPlans.Where(
                        p => p.objects.@object.Any(q => q.type == "camera" && q.id == Camobject.id)).
                        ToList())
            {
                ofp.needsupdate = true;
                if (!isAlert) continue;
                FloorPlanControl fpc = MainForm.InstanceReference.GetFloorPlan(ofp.id);
                fpc.LastAlertTimestamp = Helper.Now.UnixTicks();
                fpc.LastOid = Camobject.id;
                fpc.LastOtid = 2;
            }
        }

        public string RecordSwitch(bool record)
        {
            if (!Helper.HasFeature(Enums.Features.Recording))
                return "notrecording," + LocRm.GetString("RecordingStopped");

            if (record)
            {
                if (!IsEnabled)
                {
                    Enable();
                }
                ForcedRecording = true;
                _requestRefresh = true;
                return "recording," + LocRm.GetString("RecordingStarted");
            }

            ForcedRecording = false;
            try {
                if (VolumeControl != null)
                    VolumeControl.ForcedRecording = false;
            }
            catch
            {
                // ignored
            }

            StopSaving();

            _requestRefresh = true;
            return "notrecording," + LocRm.GetString("RecordingStopped");
        }

        private void OpenVideoSource(CameraWindow cw)
        {
            cw.CameraReconnect -= CWCameraReconnect;
            cw.CameraDisabled -= CWCameraDisabled;
            cw.CameraReconnected -= CWCameraReconnected;
            cw.CameraEnabled -= CWCameraEnabled;

            cw.CameraReconnect += CWCameraReconnect;
            cw.CameraDisabled += CWCameraDisabled;
            cw.CameraReconnected += CWCameraReconnected;
            cw.CameraEnabled += CWCameraEnabled;
            cw.HasClones = true;

            bool opened = false;
            if (cw.Camera?.VideoSource != null)
            {
                var source = cw.Camera.VideoSource;
                Camera = new Camera(source);

                Camera.NewFrame -= CameraNewFrame;
                Camera.PlayingFinished -= VideoDeviceVideoFinished;
                Camera.Alarm -= CameraAlarm;
                Camera.ErrorHandler -= CameraWindow_ErrorHandler;
                    
                Camera.NewFrame += CameraNewFrame;
                Camera.PlayingFinished += VideoDeviceVideoFinished;
                Camera.Alarm += CameraAlarm;
                Camera.ErrorHandler += CameraWindow_ErrorHandler;
             
                RotateFlipType rft;
                if (Enum.TryParse(Camobject.rotateMode, out rft))
                {
                    Camera.RotateFlipType = rft;
                }

                Calibrating = true;
                _lastRun = Helper.Now.Ticks;
                Camera.Start();
                if (cw.VolumeControl != null && !Camobject.settings.ignoreaudio)
                {
                    if (Camobject.id == -1)
                    {
                        Camobject.id = MainForm.NextCameraId;
                        MainForm.Cameras.Add(Camobject);
                    }
                    var vl = VolumeControl;
                    if (vl == null)
                    {
                        vl = MainForm.InstanceReference.AddCameraMicrophone(Camobject.id, Camobject.name + " mic");
                        Camobject.settings.micpair = vl.Micobject.id;
                    }
                       
                    var m = vl.Micobject;
                    if (m != null)
                    {
                        m.settings.samples = cw.VolumeControl.Micobject.settings.samples;
                        m.settings.channels = cw.VolumeControl.Micobject.settings.channels;
                        m.settings.typeindex = 4;
                        m.settings.buffer = Camobject.recorder.bufferseconds;
                        m.settings.bits = 16;
                        //m.alerts.active = false;
                        //m.detector.recordonalert = false;
                        //m.detector.recordondetect = false;
                    }

                    vl.Disable();
                    vl.Enable();

                    cw.VolumeControl.AudioDeviceEnabled += VLAudioDeviceEnabled;
                        
                }
                opened = true;
            }
            if (!opened)
            {
                SetErrorState("Source camera offline");
                VideoSourceErrorState = true;
                _requestRefresh = true;
            }
        }

        void CWCameraReconnect(object sender, EventArgs e)
        {
            if (Camera != null)
            {
                if (Camera.VideoSource != null)
                {
                    Camera.PlayingFinished -= VideoDeviceVideoFinished;
                }
                Camera.NewFrame -= CameraNewFrame;
                Camera.Alarm -= CameraAlarm;

                if (VolumeControl != null)
                    VolumeControl.IsReconnect = true;
            }
        }

        void CWCameraDisabled(object sender, EventArgs e)
        {
            if (IsEnabled)
            {
                SetErrorState("Source camera offline");
                StopSaving();               
            }

        }

        void CWCameraEnabled(object sender, EventArgs e)
        {
            if (IsEnabled)
            {
                Disable();
                Enable();
            }
        }

        void VLAudioDeviceEnabled(object sender, EventArgs e)
        {
            if (IsClone)
            {
                if (VolumeControl != null)
                {
                    if (VolumeControl.IsEnabled)
                    {
                        VolumeControl.Disable();
                        VolumeControl.Enable();
                    }

                }
            }
        }



        void CWCameraReconnected(object sender, EventArgs e)
        {
            if (Camera != null)
            {
                if (Camera.VideoSource != null)
                {
                    Camera.PlayingFinished += VideoDeviceVideoFinished;
                }

                Camera.NewFrame -= CameraNewFrame;
                Camera.Alarm -= CameraAlarm;

                Camera.NewFrame += CameraNewFrame;
                Camera.Alarm += CameraAlarm;

                if (Camobject.settings.calibrateonreconnect)
                {
                    Calibrating = true;

                }

                if (VolumeControl != null)
                    VolumeControl.IsReconnect = false;
            }

        }



        private void OpenVideoSource(IVideoSource source, bool @override)
        {
            if (!@override && Camera?.VideoSource != null && Camera.VideoSource.Source == source.Source)
            {
                return;
            }
            if (Camera != null && Camera.IsRunning)
            {
                Disable();
            }
            var vlcStream = source as VlcStream;
            if (vlcStream != null)
            {
                vlcStream.FormatWidth = Camobject.settings.desktopresizewidth;
                vlcStream.FormatHeight = Camobject.settings.desktopresizeheight;
            }

            var kinectStream = source as KinectStream;
            if (kinectStream != null)
            {
                kinectStream.InitTripWires(Camobject.alerts.pluginconfig);
                kinectStream.TripWire += CameraAlarm;
            }

            var kinectNetworkStream = source as KinectNetworkStream;
            if (kinectNetworkStream != null)
            {
                //add the camera alert handler hook to the provider
                kinectNetworkStream.AlertHandler += CameraWindow_AlertHandler;
            }

            var audiostream = source as ISupportsAudio;
            if (audiostream != null)
            {
                audiostream.HasAudioStream += VideoSourceHasAudioStream;
            }

            Camera = new Camera(source);
            Camera.NewFrame -= CameraNewFrame;
            Camera.PlayingFinished -= VideoDeviceVideoFinished;
            Camera.Alarm -= CameraAlarm;
            Camera.ErrorHandler -= CameraWindow_ErrorHandler;


            Camera.NewFrame += CameraNewFrame;
            Camera.PlayingFinished += VideoDeviceVideoFinished;
            Camera.Alarm += CameraAlarm;
            Camera.ErrorHandler += CameraWindow_ErrorHandler;

            RotateFlipType rft;
            if (Enum.TryParse(Camobject.rotateMode, out rft))
            {
                Camera.RotateFlipType = rft;
            }
            Camera.PiPConfig = Camobject.settings.pip.config;

            SetVideoSourceProperties();
        }

        public void ConfigurePlugin()
        {
            if (Camera != null)
            {
                Type o = Camera.Plugin.GetType();
                //update plugin with latest list of groups

                SetPluginGroups(o);
                

                var config = (string) o.GetMethod("Configure").Invoke(Camera.Plugin, null);
                Camobject.alerts.pluginconfig = config;
            }
        }

        private void SetPluginGroups(Type o)
        {
            string groups = MainForm.Conf.Permissions.Aggregate("", (current, g) => current + (g.name + ",")).Trim(',');
            if (o.GetProperty("Groups") != null)
                o.GetProperty("Groups").SetValue(Camera.Plugin, groups, null);
            if (o.GetProperty("Group") != null)
                o.GetProperty("Group").SetValue(Camera.Plugin, MainForm.Group, null);
        }

        public List<string> PluginCommands
        {
            get
            {
                if (Camera?.Plugin != null)
                {
                    Type o = Camera.Plugin.GetType();
                    SetPluginGroups(o);

                    var c = o.GetProperty("Commands");
                    if (c!=null)
                    {
                        try
                        {
                            string commands = c.GetValue(Camera.Plugin, null).ToString();
                            if (!string.IsNullOrEmpty(commands))
                            {
                                return commands.Split(',').ToList();
                            }
                        }
                        catch
                        {
                            //not initialised
                        }
                    }
                }
                return null;
            }
        } 

        public void ExecutePluginCommand(string command)
        {
            var a = Camera.Plugin?.GetType().GetMethod("ExecuteCommand");
            if (a != null)
            {
                var b = (String) a.Invoke(Camera.Plugin, new object[] {command});
                if (b != "OK")
                {
                    ErrorHandler?.Invoke("Plugin response: " + a);
                }
            }
        }

        public bool ExecutePluginShortcut(string shortcut)
        {
            var a = Camera.Plugin?.GetType().GetMethod("ExecuteShortcut");
            if (a != null)
            {
                var b = (String)a.Invoke(Camera.Plugin, new object[] { shortcut });
                if (b == "OK")
                {
                    return true;
                }
            }
            return false;
        }

        internal void LogToPlugin(string message)
        {
            var l = Camera?.Plugin?.GetType().GetMethod("LogExternal");
            if (l != null)
            {
                try
                {
                    l.Invoke(Camera.Plugin, new object[] {message});
                }
                catch (Exception ex)
                {
                    ErrorHandler?.Invoke(ex.Message);
                }
            }
        }

        //ispykinect: this processes commands from the plugin
        void CameraWindow_AlertHandler(object sender, AlertEventArgs eventArgs)
        {
            if (Camera.Plugin != null)
            {
                var a = (String)Camera.Plugin.GetType().GetMethod("ProcessAlert").Invoke(Camera.Plugin, new object[] { eventArgs.Description });
                ProcessAlertFromPlugin(a, eventArgs.Description);
            }

        }

        private void ProcessAlertFromPlugin(string a, string description)
        {
            if (!string.IsNullOrEmpty(a))
            {
                string[] actions = a.ToLower().Split(',');
                foreach (var action in actions)
                {
                    if (!string.IsNullOrEmpty(action))
                    {
                        switch (action)
                        {
                            case "alarm":
                                CameraAlarm(description, EventArgs.Empty);
                                break;
                            case "flash":
                                FlashCounter = Helper.Now.AddSeconds(10);
                                break;
                            case "record":
                                RecordSwitch(true);
                                break;
                            case "stoprecord":
                                RecordSwitch(false);
                                break;
                            case "enable_motion":
                                Calibrating = true;
                                Camobject.detector.type = "Two Frames";
                                SetDetector();
                                break;
                            case "disable_motion":
                                Camobject.detector.type = "None";
                                SetDetector();
                                break;
                            default:
                                if (action.StartsWith("border:") && action.Length > 7)
                                {
                                    string col = action.Substring(7);
                                    try
                                    {
                                        _customColor = Color.FromArgb(Convert.ToInt32(col));
                                        Custom = true;
                                    }
                                    catch (Exception e)
                                    {
                                        ErrorHandler?.Invoke(e.Message);
                                    }
                                }
                                break;
                        }

                    }
                }
            }
        }

        public void SetDetector()
        {
            if (Camera == null)
                return;
            Camera.MotionDetector = null;
            switch (Camobject.detector.type)
            {
                case "Two Frames":
                    Camera.MotionDetector =
                        new MotionDetector(
                            new TwoFramesDifferenceDetector(Camobject.settings.suppressnoise));
                    SetProcessor();
                    break;
                case "Custom Frame":
                    Camera.MotionDetector =
                        new MotionDetector(
                            new CustomFrameDifferenceDetector(Camobject.settings.suppressnoise,Camobject.detector.keepobjectedges));
                    SetProcessor();
                    break;
                case "Background Modeling":
                    Camera.MotionDetector =
                        new MotionDetector(
                            new SimpleBackgroundModelingDetector(Camobject.settings.suppressnoise,Camobject.detector.keepobjectedges));
                    SetProcessor();
                    break;
                case "Two Frames (Color)":
                    Camera.MotionDetector =
                        new MotionDetector(
                            new TwoFramesColorDifferenceDetector(Camobject.settings.suppressnoise));
                    SetProcessor();
                    break;
                case "Custom Frame (Color)":
                    Camera.MotionDetector =
                        new MotionDetector(
                            new CustomFrameColorDifferenceDetector(Camobject.settings.suppressnoise,Camobject.detector.keepobjectedges));
                    SetProcessor();
                    break;
                case "Background Modeling (Color)":
                    Camera.MotionDetector =
                        new MotionDetector(
                            new SimpleColorBackgroundModelingDetector(Camobject.settings.suppressnoise,Camobject.detector.keepobjectedges));
                    SetProcessor();
                    break;
                case "None":
                    break;
            }
            if (Camera.MotionDetector != null)
                NeedMotionZones = true;
        }

        public void SetProcessor()
        {
            if (Camera?.MotionDetector == null)
                return;
            Camera.MotionDetector.MotionProcessingAlgorithm = null;

            switch (Camobject.detector.postprocessor)
            {
                case "Grid Processing":
                    Camera.MotionDetector.MotionProcessingAlgorithm = new GridMotionAreaProcessing
                    {
                        HighlightColor = ColorTranslator.FromHtml(Camobject.detector.color),
                        HighlightMotionGrid = Camobject.detector.highlight
                    };
                    break;
                case "Object Tracking":
                    Camera.MotionDetector.MotionProcessingAlgorithm = new BlobCountingObjectsProcessing
                    {
                        HighlightColor = ColorTranslator.FromHtml(Camobject.detector.color),
                        HighlightMotionRegions = Camobject.detector.highlight,
                        MinObjectsHeight = Camobject.detector.minheight,
                        MinObjectsWidth = Camobject.detector.minwidth
                    };

                    break;
                case "Border Highlighting":
                    Camera.MotionDetector.MotionProcessingAlgorithm = new MotionBorderHighlighting
                    {
                        HighlightColor = ColorTranslator.FromHtml(Camobject.detector.color)
                    };
                    break;
                case "Area Highlighting":
                    Camera.MotionDetector.MotionProcessingAlgorithm = new MotionAreaHighlighting
                    {
                        HighlightColor = ColorTranslator.FromHtml(Camobject.detector.color)
                    };
                    break;
                case "None":
                    break;
            }
        }

        public void SetMotionZones()
        {
            if (Camera?.MotionDetector != null)
            {
                Camera.SetMotionZones(Camobject.detector.motionzones);
            }
        }

        private void StopSaving()
        {
            if (Recording)
            {
                lock (_lockobject)
                {
                    _stopWrite.Set();
                }
                try
                {
                    if (_recordingThread != null && !_recordingThread.Join(TimeSpan.Zero))
                    {
                        if (!_recordingThread.Join(3000))
                        {
                            _stopWrite.Set();
                        }
                    }
                }
                catch
                {
                    // ignored
                }
            }
        }

        public void ApplySchedule()
        {
            //find most recent schedule entry
            if (!Camobject.schedule.active || Camobject.schedule?.entries == null || !Camobject.schedule.entries.Any())
                return;

            DateTime dNow = DateTime.Now;
            TimeSpan shortest = TimeSpan.MaxValue;
            objectsCameraScheduleEntry mostrecent = null;
            bool isstart = true;

            foreach (objectsCameraScheduleEntry entry in Camobject.schedule.entries)
            {
                if (entry.active)
                {
                    string[] dows = entry.daysofweek.Split(',');
                    foreach (int dow in dows.Select(dayofweek => Convert.ToInt32(dayofweek)))
                    {
                        //when did this last fire?
                        if (entry.start.IndexOf("-", StringComparison.Ordinal) == -1)
                        {
                            string[] start = entry.start.Split(':');
                            var dtstart = new DateTime(dNow.Year, dNow.Month, dNow.Day, Convert.ToInt32(start[0]),
                                Convert.ToInt32(start[1]), 0);
                            while ((int)dtstart.DayOfWeek != dow || dtstart > dNow)
                                dtstart = dtstart.AddDays(-1);
                            if (dNow - dtstart < shortest)
                            {
                                shortest = dNow - dtstart;
                                mostrecent = entry;
                                isstart = true;
                            }
                        }
                        if (entry.stop.IndexOf("-", StringComparison.Ordinal) == -1)
                        {
                            string[] stop = entry.stop.Split(':');
                            var dtstop = new DateTime(dNow.Year, dNow.Month, dNow.Day, Convert.ToInt32(stop[0]),
                                Convert.ToInt32(stop[1]), 0);
                            while ((int)dtstop.DayOfWeek != dow || dtstop > dNow)
                                dtstop = dtstop.AddDays(-1);
                            if (dNow - dtstop < shortest)
                            {
                                shortest = dNow - dtstop;
                                mostrecent = entry;
                                isstart = false;
                            }
                        }
                    }
                }
            }
            if (mostrecent != null)
            {
                if (isstart)
                {
                    if (!IsEnabled)
                        Enable();

                    Camobject.detector.recordondetect = mostrecent.recordondetect;
                    Camobject.detector.recordonalert = mostrecent.recordonalert;
                    Camobject.ftp.enabled = mostrecent.ftpenabled;
                    Camobject.savelocal.enabled = mostrecent.savelocalenabled;
                    Camobject.ptzschedule.active = mostrecent.ptz;
                    Camobject.alerts.active = mostrecent.alerts;
                    Camobject.settings.messaging = mostrecent.messaging;

                    if (Camobject.recorder.timelapseenabled && !mostrecent.timelapseenabled)
                    {
                        CloseTimeLapseWriter();
                    }
                    Camobject.recorder.timelapseenabled = mostrecent.timelapseenabled;
                    if (mostrecent.recordonstart)
                    {
                        ForcedRecording = true;
                    }
                }
                else
                {
                    if (IsEnabled)
                        Disable();
                }
            }
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // CameraWindow
            // 
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.MinimumSize = new System.Drawing.Size(120, 50);
            this.Size = new System.Drawing.Size(160, 120);
            this.LocationChanged += new System.EventHandler(this.CameraWindowLocationChanged);
            this.Resize += new System.EventHandler(this.CameraWindowResize);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private void CameraWindowResize(object sender, EventArgs e)
        {
            SetVolumeLevelLocation();
        }

        private void CameraWindowLocationChanged(object sender, EventArgs e)
        {
            SetVolumeLevelLocation();
        }

        public void SetVolumeLevelLocation()
        {
            if (VolumeControl != null)
            {
                VolumeControl.Location = new Point(Location.X, Location.Y + Height);
                VolumeControl.Width = Width;
                VolumeControl.Height = 40;
            }
        }
    }


}