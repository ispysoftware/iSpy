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
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Sockets;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using FFmpeg.AutoGen;
using iSpyApplication.Cloud;
using iSpyApplication.Onvif;
using iSpyApplication.Pelco;
using iSpyApplication.Realtime;
using iSpyApplication.Server;
using iSpyApplication.Sources;
using iSpyApplication.Sources.Audio;
using iSpyApplication.Sources.Video;
using iSpyApplication.Sources.Video.Ximea;
using iSpyApplication.Utilities;
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
        private int _rtindex;
        private static readonly int[] ReconnectTargets = { 2, 5, 10, 30, 60 };
        internal MainForm MainClass;
        internal DateTime LastAutoTrackSent = DateTime.MinValue;
        private Color _customColor = Color.Black;
        private DateTime _lastRedraw = DateTime.MinValue;
        private DateTime _recordingStartTime;
        private readonly ManualResetEvent _stopWrite = new ManualResetEvent(false);
        private readonly ManualResetEvent _writerStopped = new ManualResetEvent(true); 
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
        private DateTime _reconnectTarget = DateTime.MinValue;
        private bool _firstFrame = true;
        private Thread _recordingThread;
        private Camera _camera;
        private DateTime _lastFrameUploaded = Helper.Now;
        private DateTime _lastFrameSaved = Helper.Now;
        private DateTime _dtPTZLastCheck = DateTime.Now;
        private long _lastRun = Helper.Now.Ticks;

        private long _lastMovementDetected = DateTime.MinValue.Ticks;
        private long _lastAlerted = DateTime.MinValue.Ticks;

        public DateTime LastMovementDetected
        {
            get { return new DateTime(_lastMovementDetected); }
            set { Interlocked.Exchange(ref _lastMovementDetected, value.Ticks); }
        }

        public int ObjectTypeID => 2;

        private ONVIFDevice _onvifDevice = null;
        public bool ONVIFConnected
        {
            get
            {
                return _onvifDevice != null;
            }
        }
        public ONVIFDevice ONVIFDevice
        {
            get
            {
                if (_onvifDevice != null)
                    return _onvifDevice;

                try
                {
                    var p = Camobject.settings.onvifident.Split('|');
                    if (p.Length > 1)
                    {
                        Camobject.settings.onvifident = p[0];
                        Helper.NVSet(this, "profilename", p[1]);
                    }

                    string url = Camobject.settings.onvifident;
                    string pn = Nv("profilename");
                    int pi = 0;
                    int.TryParse(pn, out pi);

                    _onvifDevice = new ONVIFDevice(p[0], Camobject.settings.login,
                        Camobject.settings.password, Camobject.settings.onvif.rtspport,
                        Camobject.settings.onvif.timeout);

                    _onvifDevice.SelectProfile(pi);


                    return _onvifDevice;
                }
                catch(Exception ex)
                {
                    Logger.LogException(ex,"Onvif discovery");
                    _onvifDevice = null;
                    return null;
                }
            }
            set
            {
                _onvifDevice = null;
            }
        }

        public int ObjectID => Camobject.id;
        

        public DateTime LastAlerted
        {
            get { return new DateTime(_lastAlerted); }
            set { Interlocked.Exchange(ref _lastAlerted, value.Ticks); }
        }

        private DateTime _mouseMove = DateTime.MinValue;
        private List<FilesFile> _filelist = new List<FilesFile>();
        private MediaWriter _timeLapseWriter;
        private readonly ToolTip _toolTipCam;
        private int _ttind = -1;
        private int _reconnectFailCount;
        private bool _suspendPTZSchedule;
        private bool _requestRefresh;
        private readonly StringBuilder _motionData = new StringBuilder(100000);

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

        private MediaWriter _writer;
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
        public bool ForcedRecording { get; set; }
        public bool NeedMotionZones = true;
        internal XimeaVideoSource XimeaSource;
        public bool Alerted;
        public double MovementCount;
        public DateTime CalibrateTarget;
        private DateTime _lastReconnect = DateTime.MinValue;
        public Rectangle RestoreRect = Rectangle.Empty;
        

        public bool Calibrating
        {
            get
            {
                return DateTime.UtcNow < CalibrateTarget;
            }
            set
            {
                if (value)
                {
                    CalibrateTarget = DateTime.UtcNow.AddSeconds(Camobject.detector.calibrationdelay);
                }
                else
                    CalibrateTarget = DateTime.MinValue;
            }
        }
        public Graphics CurrentFrame;
        public PTZController PTZ;
        public DateTime FlashCounter = DateTime.MinValue;
        public DateTime LastActivity = DateTime.MinValue;
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

        private bool _videoSourceErrorState;
        public bool VideoSourceErrorState
        {
            get { return _videoSourceErrorState; }
            set
            {
                _videoSourceErrorState = value;
                _requestRefresh = true;
            }
        }
        public DateTime TimelapseStart = DateTime.MinValue;
        public objectsCamera Camobject;
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
        public bool Recording
        {
            get
            {
                try
                {
                    return _recordingThread != null;
                }
                catch
                {
                    return true;
                }

            }
            
        } 

        public string ObjectName => Camobject.name;
        public string Folder => Camobject.directory;

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

        private AVCodecID Codec
        {
            get
            {
                switch (Camobject.recorder.profile)
                {
                    default:
                        return AVCodecID.AV_CODEC_ID_H264;
                    case 3:
                        return AVCodecID.AV_CODEC_ID_WMV1;
                    case 4:
                        return AVCodecID.AV_CODEC_ID_WMV2;
                    case 5:
                        return AVCodecID.AV_CODEC_ID_MPEG4;
                    case 6:
                        return AVCodecID.AV_CODEC_ID_MSMPEG4V3;
                    case 7:
                        return AVCodecID.AV_CODEC_ID_RAWVIDEO;
                    case 8:
                        return AVCodecID.AV_CODEC_ID_MJPEG;
                }
            }
        }

        private AVCodecID CodecAudio
        {
            get
            {
                switch (Camobject.recorder.profile)
                {
                    default:
                        return AVCodecID.AV_CODEC_ID_AAC;
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                        return AVCodecID.AV_CODEC_ID_MP3;
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
            try
            {
                if (_tScan == null || _tScan.Join(TimeSpan.Zero))
                {
                    _tScan = new Thread(ScanFiles);
                    _tScan.Start();
                }
            }
            catch
            {
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

            try
            {
                Camobject.resolution = width + "x" + height;

                SuspendLayout();

                //resize to max 640xh
                if (width > 640)
                {
                    double d = width/640d;
                    width = 640;
                    height = Convert.ToInt32(Convert.ToDouble(height)/d);
                }
                Camobject.width = width;
                Camobject.height = height;
                Size = new Size(width + 2, height + 26);
                ResumeLayout();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
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
            BackColor = MainForm.BackgroundColor;
            Camobject = cam;
            PTZ = new PTZController(this);
            MainClass = mainForm;
            ErrorHandler += CameraWindow_ErrorHandler;

            _toolTipCam = new ToolTip { AutomaticDelay = 500, AutoPopDelay = 1500 };
        }

        void CameraWindow_ErrorHandler(string message)
        {
            Logger.LogError(Camobject.name+": "+message);
        }

        private Thread _tFiles;
        public void LoadFileList()
        {
            try
            {
                if (_tFiles == null || _tFiles.Join(TimeSpan.Zero))
                {
                    _tFiles = new Thread(GenerateFileList);
                    _tFiles.Start();
                }
            }
            catch
            {

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

        #region schedule
        private List<objectsScheduleEntry> _schedule;
        public List<objectsScheduleEntry> Schedule
        {
            get
            {
                if (_schedule != null && !_needsNewSchedule)
                    return _schedule;
                _schedule =
                    MainForm.Schedule.Where(p => p.objectid == ObjectID && p.objecttypeid == ObjectTypeID && p.active)
                        .ToList();
                _needsNewSchedule = false;
                return _schedule;
            }
        }

        private bool _needsNewSchedule;
        public void ReloadSchedule()
        {
            _needsNewSchedule = true;
        }

        public int Order
        {
            get { return Camobject.settings.order; }
            set { Camobject.settings.order = value; }
        }

        private int _lastMinute = -1;
        private bool CheckSchedule()
        {
            var t = Convert.ToInt32(Math.Floor(DateTime.Now.TimeOfDay.TotalMinutes));
            bool enable = false, disable = false;
            if (_lastMinute != t)
            {
                _lastMinute = t;
                var lRec = Schedule.Where(p => p.time == _lastMinute).ToList();
                string dow = ((int)DateTime.Now.DayOfWeek).ToString(CultureInfo.InvariantCulture);
                bool b = false;
                foreach (var en in lRec)
                {
                    if (en.daysofweek.Contains(dow))
                    {
                        bool enable2, disable2;
                        ActionSchedule(en, out enable2, out disable2);
                        enable = enable || enable2;
                        disable = disable || disable2;
                        b = true;
                    }
                }
                if (enable && !disable)
                    Enable();
                if (disable && !enable)
                    Disable();
                return b;
            }
            return false;
        }

        public bool ApplySchedule()
        {
            var t = Convert.ToInt32(Math.Floor(DateTime.Now.TimeOfDay.TotalMinutes));
            bool enable = false, disable = false;
            var lRec = Schedule.Where(p => p.time < t).OrderBy(p => p.time).ToList();
            string dow = ((int)DateTime.Now.DayOfWeek).ToString(CultureInfo.InvariantCulture);
            foreach (var en in lRec)
            {
                if (en.daysofweek.Contains(dow))
                {
                    bool enable2, disable2;

                    ActionSchedule(en, out enable2, out disable2);

                    if (enable2)
                    {
                        enable = true;
                        disable = false;
                    }
                    else
                    {
                        if (disable2)
                        {
                            enable = false;
                            disable = true;
                        }
                    }
                }
            }
            if (enable)
                Enable();
            if (disable)
                Disable();

            return !(enable || disable);
        }

        private void ActionSchedule(objectsScheduleEntry en, out bool enable, out bool disable)
        {
            enable = false;
            disable = false;
            switch (en.typeid)
            {
                case 0:
                    enable = true;
                    break;
                case 1:
                    disable = true;
                    break;
                case 2:
                    enable = true;
                    ForcedRecording = true;
                    break;
                case 3:
                    ForcedRecording = false;
                    break;
                case 4:
                    Camobject.detector.recordondetect = true;
                    Camobject.detector.recordonalert = false;
                    break;
                case 5:
                    Camobject.detector.recordondetect = false;
                    Camobject.detector.recordonalert = true;
                    break;
                case 6:
                    Camobject.detector.recordondetect = false;
                    Camobject.detector.recordonalert = false;
                    break;
                case 7:
                    Camobject.alerts.active = true;
                    break;
                case 8:
                    Camobject.alerts.active = false;
                    break;
                case 9:
                    {
                        var a = MainForm.Actions.FirstOrDefault(p => p.ident == en.parameter);

                        if (a != null)
                            a.active = true;
                    }
                    break;
                case 10:
                    {
                        var a = MainForm.Actions.FirstOrDefault(p => p.ident == en.parameter);

                        if (a != null)
                            a.active = false;
                    }
                    break;
                case 11:
                    Camobject.recorder.timelapseenabled = true;
                    break;
                case 12:
                    Camobject.recorder.timelapseenabled = false;
                    CloseTimeLapseWriter();
                    break;
                case 13:
                    Camobject.ftp.enabled = true;
                    break;
                case 14:
                    Camobject.ftp.enabled = false;
                    break;
                case 15:
                    Camobject.recorder.ftpenabled = true;
                    break;
                case 16:
                    Camobject.recorder.ftpenabled = false;
                    break;
                case 17:
                    Camobject.savelocal.enabled = true;
                    break;
                case 18:
                    Camobject.savelocal.enabled = false;
                    break;
                case 19:
                    Camobject.ptzschedule.active = true;
                    break;
                case 20:
                    Camobject.ptzschedule.active = false;
                    break;
                case 21:
                    Camobject.settings.messaging = true;
                    break;
                case 22:
                    Camobject.settings.messaging = false;
                    break;
                case 23:
                    Camobject.settings.ptzautotrack = true;
                    break;
                case 24:
                    Camobject.settings.ptzautotrack = false;
                    break;
                case 25:
                    if (VolumeControl != null && VolumeControl.IsEnabled)
                    {
                        VolumeControl.Listening = true;
                        LogToPlugin(VolumeControl.Listening ? "Listening Started" : "Listening Finished");
                    }
                    break;
                case 26:
                    if (VolumeControl != null && VolumeControl.IsEnabled)
                    {
                        VolumeControl.Listening = false;
                        LogToPlugin(VolumeControl.Listening ? "Listening Started" : "Listening Finished");
                    }
                    break;
            }
        }

        #endregion

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
                                float pc = 0;
                                if (_newSeek == 0)
                                {
                                    var rBp = ButtonPanel;
                                    _newSeek = e.Location.X - rBp.X;
                                    if (_newSeek < 0.0001) _newSeek = 0.0001f;
                                    if (_newSeek > rBp.Width)
                                        _newSeek = rBp.Width;                                    
                                }
                                pc = (float)(Convert.ToDouble(_newSeek) / Convert.ToDouble(ButtonPanel.Width));
                                var vlc = Camera.VideoSource as VlcStream;
                                vlc?.Seek(pc);

                            }
                            _seeking = false;
                            _newSeek = 0;
                        }
                    }
                    ((LayoutPanel)Parent).ISpyControlUp(new Point(Left + e.X, Top + e.Y));
                    break;
                case MouseButtons.Middle:
                    PTZNavigate = false;
                    PTZ.CheckSendStop();
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
                                    case -999:
                                        var layoutPanel = (LayoutPanel) Parent;
                                        layoutPanel?.ISpyControlDown(new Point(this.Left + e.X, this.Top + e.Y));
                                        break;
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
                                            string url = MainForm.Webpage;
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

                        if (MainForm.LockLayout) return;
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
                LayoutPanel.NeedsRedraw = true;

            _minimised = Size.Equals(MinimumSize);
            _rc = Rectangle.Empty;

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
            if (_stopWrite.WaitOne(0))
                _writerStopped.WaitOne(2000);
   
            _stopWrite.Close();
            _writerStopped.Close();
        
                

            _camera?.Dispose();
            _timeLapseWriter = null;
            _writer = null;
            ClearBuffer();
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
                        LastActivity = DateTime.UtcNow;
                        if (Camobject.alerts.mode != "nomovement" &&
                            (Camobject.detector.recordondetect || Camobject.detector.recordonalert))
                        {
                            var vc = VolumeControl;
                            if (vc != null)
                            {
                                vc.LastActivity = DateTime.UtcNow;
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

                    if (_tickThrottle >1) //every second
                    {

                        if (CheckReconnect()) goto skip;

                        CheckReconnectInterval();

                        CheckDisconnect();

                        CheckStopPTZTracking();

                        if (Camobject.ptzschedule.active && !_suspendPTZSchedule)
                        {
                            if (IsEnabled && !VideoSourceErrorState)
                                CheckPTZSchedule();
                        }

                        if (MovementDetected)
                            LastActivity = DateTime.UtcNow;

                        if (Camera != null && GotImage && !VideoSourceErrorState)
                        {
                            if (Calibrating)
                            {
                                DoCalibrate(_tickThrottle);
                            }

                            CheckTimeLapse(_tickThrottle);
                        }
                        if (Helper.HasFeature(Enums.Features.Recording))
                        {
                            CheckRecord();
                        }
                        _tickThrottle = 0;

                    }
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
                    (!MovementDetected && (DateTime.UtcNow - LastActivity).TotalSeconds > Camobject.recorder.inactiverecord && !ForcedRecording && dur > Camobject.recorder.minrecordtime))
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
                                _timeLapseWriter.WriteFrame(ResizeBitmap(bm));
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
            if (Camobject.settings.ptzautotrack && Camobject.ptz != -1)
            {
                if (Ptzneedsstop && LastAutoTrackSent < Helper.Now.AddMilliseconds(-1000))
                {
                    PTZ.SendPTZCommand(Enums.PtzCommand.Stop);
                    Ptzneedsstop = false;
                }

                if (Camobject.settings.ptzautohome && LastAutoTrackSent > DateTime.MinValue && !Calibrating &&
                    LastAutoTrackSent < Helper.Now.AddSeconds(0 - Camobject.settings.ptzautohomedelay))
                {
                    LastAutoTrackSent = DateTime.MinValue;
                    Calibrating = true;
                    CalibrateTarget = DateTime.UtcNow.AddSeconds(Camobject.settings.ptztimetohome);
                    if (string.IsNullOrEmpty(Camobject.settings.ptzautohomecommand) || Camobject.settings.ptzautohomecommand == "Center")
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
            switch(Camobject.savelocal.mode)
            {
                case 2:
                    if (Math.Abs(Camobject.savelocal.intervalnew) > double.Epsilon)
                    {
                        if (Camobject.savelocal.enabled)
                        {
                            double d = (Helper.Now - _lastFrameSaved).TotalSeconds;
                            if (d >= Camobject.savelocal.intervalnew && d > Camobject.savelocal.minimumdelay)
                            {
                                SaveFrame();
                            }
                        }
                    }
                    break;

                case 0:
                    if (Camobject.savelocal.enabled && Camobject.savelocal.motiontimeout > 0)
                    {
                        if (LastActivity > DateTime.UtcNow.AddSeconds(0 - Camobject.savelocal.motiontimeout))
                        {
                            if ((Helper.Now - _lastFrameSaved).TotalSeconds > Camobject.savelocal.minimumdelay)
                            {
                                SaveFrame();
                            }
                        }
                    }
                    break;
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
                        PTZ.SendPTZCommand(entry.token);
                    }
                }
            }
            _dtPTZLastCheck = DateTime.Now;
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
                    if (Recording)
                        StopSaving();
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

        public int NextReconnectTarget
        {

            get
            {
                var i = _rtindex;
                _rtindex = Math.Min(i + 1, ReconnectTargets.Length - 1);
                Logger.LogMessage("Reconnecting " + ObjectName + "  in " + ReconnectTargets[i] + "s");
                return ReconnectTargets[i];
            }
        }

        private bool CheckReconnect()
        {
            if (_reconnectTarget > DateTime.MinValue && _reconnectTarget < DateTime.UtcNow && !IsClone)
            {
                var s = Camera?.VideoSource;
                if (s != null && !s.IsRunning)
                {
                    Calibrating = true;
                    s.Start();
                    return true;
                    
                }
            }
            return false;
        }


        private void CheckReconnectInterval()
        {
            if (Camera?.VideoSource != null && IsEnabled && !IsClone && !VideoSourceErrorState)
            {
                if (Camobject.settings.reconnectinterval > 0)
                {

                    if ((DateTime.UtcNow - _lastReconnect).TotalSeconds > Camobject.settings.reconnectinterval)
                    {
                        _lastReconnect = DateTime.UtcNow;
                        CameraReconnect?.Invoke(this, EventArgs.Empty);

                        try
                        {
                            Camera.Restart();
                        }
                        catch (Exception ex)
                        {
                            ErrorHandler?.Invoke(ex.Message);
                        }

                        if (Camobject.settings.calibrateonreconnect)
                        {
                            Calibrating = true;
                        }
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
                g.DrawImage(LastFrame, r);
            }
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

            if (c.LastFrameEvent < Helper.Now.AddSeconds(-2))
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
                            bool b;
                            CloudGateway.Upload(2, Camobject.id, fullpath, out b);
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
                            filename = string.Format(CultureInfo.InvariantCulture, filename, DateTime.Now);
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
                                    "", Camobject.id, Camobject.ftp.counter, ftp.rename, ftp.sftp, imageStream.ToArray()));

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

        private void FtpRecording(string path)
        {

            try
            {
                int i = 0;
                string filename = Camobject.recorder.ftpfilename;
                if (filename != "")
                {
                    filename = filename.Replace("{C}",
                        ZeroPad(Camobject.recorder.ftpcounter, Camobject.recorder.ftpcountermax));
                    Camobject.recorder.ftpcounter++;
                    if (Camobject.recorder.ftpcounter > Camobject.recorder.ftpcountermax)
                        Camobject.recorder.ftpcounter = 0;

                    while (filename.IndexOf("{", StringComparison.Ordinal) != -1 && i < 20)
                    {
                        filename = String.Format(CultureInfo.InvariantCulture, filename, DateTime.Now);
                        i++;
                    }

                }
                else
                {
                    var fp = path.Split('\\');
                    filename = fp[fp.Length - 1];
                }

                configurationServer ftp = MainForm.Conf.FTPServers.FirstOrDefault(p => p.ident == Camobject.ftp.ident);
                if (ftp != null)
                {
                    Camobject.ftp.ready = false;
                    
                    ThreadPool.QueueUserWorkItem((new AsynchronousFtpUpLoader()).FTP,
                        new FTPTask(ftp.server, ftp.port,
                            ftp.usepassive, ftp.username,
                            ftp.password, filename,
                            path,
                            Camobject.id,
                            Camobject.recorder.ftpcounter,
                            ftp.rename, ftp.sftp,null));
                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                Camobject.ftp.ready = true;
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
                    _timeLapseWriter = new MediaWriter();
                    _timeLapseWriter.Open(filename +CodecExtension,_videoWidth, _videoHeight, Codec, Camobject.recorder.timelapseframerate, AVCodecID.AV_CODEC_ID_NONE, DateTime.UtcNow, Helper.CalcCRF(Camobject.recorder.quality));

                    success = true;
                    TimelapseStart = Helper.Now;
                }
                catch (Exception ex)
                {
                    ErrorHandler?.Invoke(ex.Message);
                    _timeLapseWriter = null;
                    //Camobject.recorder.timelapse = 0;
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
                _timeLapseWriter.Close();
            }
            catch (Exception ex)
            {
                ErrorHandler?.Invoke(ex.Message);
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
                                                            ff.MaxAlarm, true,false));
                
                MainForm.NeedsMediaRefresh = Helper.Now;
            }
        }

        private static bool ThumbnailCallback()
        {
            return false;
        }

        public bool Highlighted { get; set; }


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

        private int _lastSecond;

        public Color BorderColor
        {
            get
            {

                if (FlashCounter > Helper.Now)
                {
                    Color c = MainForm.ActivityColor;
                    if (_lastSecond % 2 == 0)
                    {
                        if (Custom)
                            c = _customColor;
                    }
                    else
                        c = MainForm.BorderDefaultColor;

                    _lastSecond = DateTime.Now.Second;
                    return c;
                }

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

                        if (value != null)
                        {
                            _lastFrame = (Bitmap) value.Clone();

                        }
                        else
                        {
                            _lastFrame = null;
                        }
                    }
                }
                GotImage = value != null;
                Invalidate();
            }
        }

        private Rectangle _rc = Rectangle.Empty;
        internal Rectangle RC
        {
            get
            {
                if (_rc != Rectangle.Empty)
                    return _rc;

                switch (Camobject.settings.fillMode)
                {
                    default:
                        var cr = ClientRectangle;
                        if (Camera == null || Camera.Width == -1)
                        {
                            _rc = ClientRectangle;
                            var lf = _lastFrame;
                            if (lf != null)
                            {
                                

                                cr.X += 1;
                                cr.Y += 1;
                                cr.Width = Math.Max(cr.Width - 2, 2);
                                cr.Height = Math.Max(cr.Height - 26, 26);

                                _rc = Helper.GetArea(cr, lf.Width, lf.Height);
                            }
                            return _rc;
                        }
                        
                        cr.X += 1;
                        cr.Y += 1;
                        cr.Width = Math.Max(cr.Width - 2, 2);
                        cr.Height = Math.Max(cr.Height - 26, 26);

                        _rc = Helper.GetArea(cr, Camera.Width, Camera.Height);
                        return _rc;
                        
                    case 1:
                        _rc = ClientRectangle;
                        break;
                }

                _rc.X += 1;
                _rc.Y += 1;
                _rc.Width = Math.Max(_rc.Width - 2, 2);
                _rc.Height = Math.Max(_rc.Height - 26, 26);


                return _rc;
            }
            set { _rc = value; }
        }
        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics gCam = pe.Graphics;

            string m = "", txt = Camobject.name;
            try
            {

                int textpos = ClientRectangle.Height - 15;

                var rc = RC;
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

                                gCam.DrawImage(bmp, rc.X, rc.Y, rc.Width, rc.Height);
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
                                int remaining = Math.Max(0,
                                    Convert.ToInt32((CalibrateTarget - DateTime.UtcNow).TotalSeconds));
                                
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
                            gCam.FillEllipse(MainForm.RecordBrush, new Rectangle(ClientRectangle.X + ClientRectangle.Width - 12, 4, 8, 8));
                        }

                        if (Camera != null && Camera.IsRunning && Camobject.detector.type != "None")
                        {
                            using (var volBrush = new SolidBrush(MainForm.VolumeLevelColor))
                                DrawDetectionGraph(gCam, volBrush, MainForm.CameraLine, ClientRectangle);
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

                using (var grabBrush = new SolidBrush(BorderColor))
                using (var borderPen = new Pen(grabBrush, BorderWidth))
                {
                    gCam.DrawRectangle(borderPen, 0, 0, ClientRectangle.Width - 1, ClientRectangle.Height - 1);

                    if (!MainForm.LockLayout)
                    {
                        var borderPoints = new[]
                                           {
                                               new Point(ClientRectangle.Width - 15, ClientRectangle.Height),
                                               new Point(ClientRectangle.Width, ClientRectangle.Height - 15),
                                               new Point(ClientRectangle.Width, ClientRectangle.Height)
                                           };

                        gCam.FillPolygon(grabBrush, borderPoints);
                    }
                }
            }
            catch (Exception e)
            {
                ErrorHandler?.Invoke(e.Message);
            }

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
            if (e.Frame == null)
            {
                LastFrame = null;
                return;
            }

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
                    RC = Rectangle.Empty;
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
                    
                var dt = Helper.Now.AddSeconds(0 - Camobject.recorder.bufferseconds);
                    
                if (!Recording) { 
                while (Buffer.Count > 0)
                {
                    Helper.FrameAction fa;
                    if (Buffer.TryPeek(out fa))
                    {
                        if (fa.TimeStamp < dt)
                        {
                            if (Buffer.TryDequeue(out fa))
                                fa.Dispose();
                        }
                        else
                        {
                            break;
                        }
                    }
                }}

                
                
                if (Camobject.recorder.bufferseconds > 0 || Recording)
                    Buffer.Enqueue(new Helper.FrameAction(e.Frame, Camera.MotionLevel, Helper.Now));
                //EnqueueAsync.BeginInvoke(Buffer, new Bitmap(e.Frame), Camera.MotionLevel, Helper.Now, null,null);
                //else
                //{
                //    if (Recording)
                //    {
                //        Buffer.Enqueue(new Helper.FrameAction(e.Frame, Camera.MotionLevel, Helper.Now));
                //    }
                //        //EnqueueAsync.BeginInvoke(Buffer, new Bitmap(e.Frame), Camera.MotionLevel, Helper.Now, null,null);
                //}



                if (_lastRedraw < Helper.Now.AddMilliseconds(0 - 1000 / MainForm.Conf.MaxRedrawRate))
                {
                    LastFrame = e.Frame;
                }
                

                if (_reconnectTarget != DateTime.MinValue)
                {
                    _errorTime = _reconnectTarget = DateTime.MinValue;
                    _rtindex = 0;
                    DoAlert("reconnect");
                }

                CheckSaveFrame();
                CheckFTP();


                NewFrame?.Invoke(this, e);

                _errorTime = DateTime.MinValue;

            }
            catch (Exception ex)
            {
                ErrorHandler?.Invoke(ex.Message);
            }
        }

        private static readonly EnqueueAsyncDelegate EnqueueAsync = (q, b, f, d) => {
                                                               q.Enqueue(new Helper.FrameAction(b, f, d));
                                                               b.Dispose();
                                                           };

        private delegate void EnqueueAsyncDelegate(ConcurrentQueue<Helper.FrameAction> buffer, Bitmap frame, float motionLevel, DateTime frameTime);

        public event NewFrameEventHandler NewFrame;

        internal configurationDirectory Dir
        {
            get
            {
                var dir = MainForm.Conf.MediaDirectories.FirstOrDefault(p => p.ID == Camobject.settings.directoryIndex);
                if (dir == null)
                {
                    dir = MainForm.Conf.MediaDirectories.First();
                    Camobject.settings.directoryIndex = dir.ID;
                }
                return dir;
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
                    url = url.Replace(uri.UserInfo+"@", "");

                    MainForm.OpenUrl(url);
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex,"open web browser");
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
                LogToPlugin("Recording Started");
                string linktofile = "";
                _writerStopped.Reset();

                string previewImage = "";


                Helper.FrameAction fa;
                DateTime recordingStart = DateTime.UtcNow;
                if (Buffer.TryPeek(out fa))
                {
                    recordingStart = fa.TimeStamp;
                }
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
                        DateTime date = DateTime.Now.AddHours(Convert.ToDouble(Camobject.settings.timestampoffset)).AddSeconds(0-Camobject.recorder.bufferseconds);

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
                        try
                        {
                            if (!Directory.Exists(folder))
                                Directory.CreateDirectory(folder);

                            
                            if (bAudio)
                            {
                                _writer = new MediaWriter
                                          {
                                              Gpu = MediaWriter.Encoders.FirstOrDefault(
                                                  p => p.Name == Camobject.settings.encoder)
                                          };
                                _writer.Open(videopath, _videoWidth, _videoHeight, Codec, CodecFramerate,CodecAudio, recordingStart, Helper.CalcCRF(Camobject.recorder.quality));
                            }
                            else
                            {
                                _writer = new MediaWriter
                                          {
                                              Gpu = MediaWriter.Encoders.FirstOrDefault(p => p.Name == Camobject.settings.encoder)
                                          };

                                _writer.Open(videopath, _videoWidth, _videoHeight, Codec, CodecFramerate, AVCodecID.AV_CODEC_ID_NONE, recordingStart, Helper.CalcCRF(Camobject.recorder.quality));
                            }
                                
                            try
                            {
                                linktofile = Uri.EscapeDataString(MainForm.IPAddress + "loadclip.mp4?oid=" + Camobject.id + "&ot=2&fn=" + VideoFileName + CodecExtension + "&auth=" + MainForm.Identifier);
                            }
                            catch (Exception ex)
                            {
                                Logger.LogException(ex, "Generating external link to file");
                            }

                            DoAlert("recordingstarted", linktofile);
                           
                            Helper.FrameAction peakFrame = null;
                            bool _writtenVideo = false;
                            while (!_stopWrite.WaitOne(5))
                            {
                                if (Buffer.TryDequeue(out fa))
                                {
                                    WriteFrame(fa, ref maxAlarm, ref peakFrame);
                                    _writtenVideo = true;
                                }

                                if (bAudio && _writtenVideo)
                                {
                                    if (vc.Buffer.TryDequeue(out fa))
                                    {
                                        WriteFrame(fa, ref maxAlarm, ref peakFrame);
                                    }
                                }
                            }

                            if (!Directory.Exists(folder + @"thumbs\"))
                                Directory.CreateDirectory(folder + @"thumbs\");

                            var bmp = peakFrame?.Frame;
                            if (bmp != null)
                            {
                                try
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
                               
                                    previewImage = folder + @"thumbs\" + VideoFileName + ".jpg";
                                    

                                }
                                catch (Exception ex)
                                {
                                    ErrorHandler?.Invoke(ex.Message + ": " + ex.StackTrace);
                                }
                            }
                            peakFrame?.Dispose();
                        }
                        catch (Exception ex)
                        {
                            error = true;
                            ErrorHandler?.Invoke(ex.Message + " (" + ex.StackTrace + ")");
                        }
                        finally
                        {
                            if (_writer != null && !_writer.Closed)
                            {
                                try
                                {
                                    _writer.Close();
                                }
                                catch (Exception ex)
                                {
                                    ErrorHandler?.Invoke(ex.Message);
                                }
                                
                                _writer = null;
                            }
                            vc?.StopSaving();
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
                            if (Camobject.recorder.ftpenabled)
                            {
                                FtpRecording(path + CodecExtension);
                            }
                        }

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
                DoAlert("recordingstopped", linktofile);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            _recordingThread = null;
            _writerStopped.Set();
            _stopWrite.Reset();
        }


        [HandleProcessCorruptedStateExceptions]
        private void WriteFrame(Helper.FrameAction fa, ref double maxAlarm, ref Helper.FrameAction peakFrame)
        {
            bool keepFrame = false;
            switch (fa.FrameType)
            {
                case Enums.FrameType.Video:
                    _writer.WriteFrame(ResizeBitmap(fa.Frame), fa.TimeStamp);
                    if (fa.Level > maxAlarm || peakFrame == null)
                    {
                        maxAlarm = fa.Level;
                        peakFrame?.Dispose();
                        peakFrame = fa;
                        keepFrame = true;
                    }

                    _motionData.Append(string.Format(CultureInfo.InvariantCulture, "{0:0.000}", Math.Min(fa.Level * 100, 100)));
                    _motionData.Append(",");
                    break;
                case Enums.FrameType.Audio:
                {
                    _writer.WriteAudio(fa.Content, fa.DataLength, 0, fa.TimeStamp);
                }

                    break;
            }
            if (!keepFrame)
                fa.Dispose();
        }

        
        //Motion Detection
        public void Detect(object sender, EventArgs e)
        {
            LastActivity = DateTime.UtcNow;
            
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
                if (Camobject.savelocal.mode == 0 && Camobject.savelocal.motiontimeout == 0)
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
                if (Camera?.Plugin != null && !Calibrating)
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
        }

        public void Alert(object sender, EventArgs e)
        {
            LastActivity = DateTime.UtcNow;
            var c = sender as CameraWindow;
            if (c != null) //triggered from another camera
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

            if (sender is string || sender is IVideoSource)
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
                var wss = MainForm.MWS.WebSocketServer;
                if (wss!=null)
                    wss.SendToAll("alert|" + ObjectName);
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
            Logger.LogMessage(l);

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
                        Logger.LogException(ex);
                    }
                }
                

                int i = 0;
                var le = MainForm.Actions.Where(p => p.objectid == oid && p.objecttypeid == 2 && p.mode == mode && p.active).ToList();
                foreach (var ev in le)
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
                        MainForm.InstanceReference._pnlCameras.Maximise(this, false);
                        break;
                    case "TA":
                        {
                            
                                string[] tid = param1.Split(',');
                                switch (tid[0])
                                {
                                    case "1":
                                        VolumeLevel vl =
                                            MainForm.InstanceReference.GetVolumeLevel(Convert.ToInt32(tid[1]));
                                        vl?.Alert(this, EventArgs.Empty);
                                        break;
                                    case "2":
                                        CameraWindow cw =
                                            MainForm.InstanceReference.GetCameraWindow(Convert.ToInt32(tid[1]));
                                        if (cw != null && cw!=this) //prevent recursion
                                            cw.Alert(this, EventArgs.Empty);
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


                                if (includeGrab && rawgrab!=null)
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
            _onvifDevice = null;
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
                case ReasonToFinishPlaying.Restart:
                    IsEnabled = false;
                    Enable();
                    break;
            }
        }

        private void SetErrorState(string reason)
        {
            VideoSourceErrorMessage = reason;
            _reconnectTarget = DateTime.UtcNow.AddSeconds(NextReconnectTarget);
            if (!VideoSourceErrorState)
            {
                VideoSourceErrorState = true;
                ErrorHandler?.Invoke(reason);

               
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
                    Camera.Detect -= Detect;
                    Camera.Alert -= Alert;
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
                            source.TripWire -= Alert;
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
                                        Camera.Stop();
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
                }
                Camobject.settings.active = false;
                LastActivity = DateTime.MinValue;
                _timeLapseTotal = _timeLapseFrameCount = 0;
                ForcedRecording = false;

                MovementDetected = false;
                Alerted = false;
                FlashCounter = DateTime.MinValue;
                _lastReconnect = DateTime.UtcNow;
                PTZNavigate = false;
                UpdateFloorplans(false);
                MainForm.NeedsSync = true;
                _errorTime = _reconnectTarget = DateTime.MinValue;
                _rtindex = 0;
                BackColor = MainForm.BackgroundColor;
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
            Camera = null;
            _disabling = false;

        }

        private void ClearBuffer()
        {
            lock (_lockobject)
            {
                Helper.FrameAction fa;
                while (Buffer.TryDequeue(out fa))
                {
                    fa.Dispose();
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
                    case 9:
                        return "ONVIF";
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
                if (int.TryParse(Nv(Camobject.settings.procAmpConfig, n), out v))
                {
                    if (v > int.MinValue)
                    {
                        int fv;
                        if (int.TryParse(Nv(Camobject.settings.procAmpConfig, "f" + n), out fv))
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

        public void Restart()
        {
            Camera?.VideoSource?.Restart();
        }

        private string _sourceOverload = "";
        public string Source
        {
            get
            {
                if (!string.IsNullOrEmpty(_sourceOverload))
                {
                    switch(_sourceOverload)
                    {
                        case "onvif":
                            return ONVIFDevice?.StreamEndpoint?.Uri?.Uri.ToString();
                    }
                    return _sourceOverload;
                }
                return Camobject.settings.videosourcestring;
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
            _onvifDevice = null;
            _sourceOverload = null;
            try
            {
                Seekable = false;
                IsClone = Camobject.settings.sourceindex == 10;
                VideoSourceErrorState = false;
                VideoSourceErrorMessage = "";
                _rc = Rectangle.Empty;

                switch (Camobject.settings.sourceindex)
                {
                    case 0:
                        var jpegSource = new JpegStream(this);
                        OpenVideoSource(jpegSource, true);
                        break;
                    case 1:
                        var mjpegSource = new MJPEGStream(this);
                        OpenVideoSource(mjpegSource, true);
                        break;
                    case 2:
                        var ffmpegSource = new MediaStream(this);
                        OpenVideoSource(ffmpegSource, true);
                        break;
                    case 3:                       
                        var videoSource = new VideoCaptureDevice(this);
                        OpenVideoSource(videoSource, true);
                        break;
                    case 4:
                        var desktopSource = new DesktopStream(this);
                        OpenVideoSource(desktopSource, true);
                        break;
                    case 5:
                        var vlcSource = new VlcStream(this);
                        OpenVideoSource(vlcSource, true);
                        break;
                    case 6:
                        if (XimeaSource == null || !XimeaSource.IsRunning)
                            XimeaSource =
                                new XimeaVideoSource(this);
                        OpenVideoSource(XimeaSource, true);
                        break;
                    case 7:
                        var ks = new KinectStream(this);                            
                        OpenVideoSource(ks, true);                        
                        break;
                    case 8:
                        switch (Nv(Camobject.settings.namevaluesettings, "custom"))
                        {
                            case "Network Kinect":
                                // open the network kinect video stream
                                OpenVideoSource(new KinectNetworkStream(this), true);
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
                        _sourceOverload = "onvif";
                        
                        if (Nv("use")=="VLC")
                        {
                            OpenVideoSource(new VlcStream(this), true);
                        }
                        else
                            OpenVideoSource(new MediaStream(this), true);
                        break;
                    case 10:
                        int icam;
                        if (int.TryParse(Camobject.settings.videosourcestring, out icam))
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
                    }
                    Camera.UpdateResources();
                }

                Camobject.settings.active = true;
                UpdateFloorplans(false);

                _timeLapseTotal = _timeLapseFrameCount = 0;
                LastActivity = DateTime.MinValue;
                MovementDetected = false;

                Alerted = false;
                PTZNavigate = false;
                Camobject.ftp.ready = true;
                _lastRun = Helper.Now.Ticks;
                MainForm.NeedsSync = true;
                _lastReconnect = DateTime.UtcNow;
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
        public void Apply()
        {
            if (Camobject.settings.active)
                Enable();
            else
            {
                Disable();
            }
            if (Camera == null) return;
            SetDetector();
            if (Camera != null)
            {
                Camera.AlarmLevel = Helper.CalculateTrigger(Camobject.detector.minsensitivity);
                Camera.AlarmLevelMax = Helper.CalculateTrigger(Camobject.detector.maxsensitivity);
                Camera.UpdateResources();
            }
            
            _firstFrame = true; //for rotate mode changes

            if (Camobject.settings.ignoreaudio)
            {
                if (VolumeControl!=null)
                    MainForm.InstanceReference.RemoveMicrophone(VolumeControl,false);
            }
            else
            {
                //add an event to check for audio
                var audiostream = Camera?.VideoSource as ISupportsAudio;
                if (audiostream != null)
                {
                    audiostream.HasAudioStream -= VideoSourceHasAudioStream;
                    audiostream.HasAudioStream += VideoSourceHasAudioStream;
                }
            }

            if (!Camobject.recorder.timelapseenabled)
                CloseTimeLapseWriter();
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
                    MainForm.NeedsSync = true;
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
                Camera.Alert -= Alert;
                Camera.Detect -= Detect;
                Camera.ErrorHandler -= CameraWindow_ErrorHandler;
                    
                Camera.NewFrame += CameraNewFrame;
                Camera.PlayingFinished += VideoDeviceVideoFinished;
                Camera.Alert += Alert;
                Camera.Detect += Detect;
                Camera.ErrorHandler += CameraWindow_ErrorHandler;
              
                Calibrating = true;
                _lastRun = Helper.Now.Ticks;
                Camera.Start();
                if (cw.VolumeControl != null && !Camobject.settings.ignoreaudio)
                {
                    if (Camobject.id == -1)
                    {
                        Camobject.id = MainForm.NextCameraId;
                        MainForm.AddObject(Camobject);
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
                Camera.Alert -= Alert;
                Camera.Detect -= Detect;
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
                Camera.Alert -= Alert;
                Camera.Detect -= Detect;

                Camera.NewFrame += CameraNewFrame;
                Camera.Alert += Alert;
                Camera.Detect += Detect;

                if (Camobject.settings.calibrateonreconnect)
                {
                    Calibrating = true;
                }
                CameraReconnected?.Invoke(this,EventArgs.Empty);
            }

        }



        private void OpenVideoSource(IVideoSource source, bool @override)
        {
            if (!@override && Camera?.VideoSource != null && Camera.VideoSource.Source == source.Source)
            {
                return;
            }
            var vlcStream = source as VlcStream;
            

            var kinectStream = source as KinectStream;
            if (kinectStream != null)
            {
                kinectStream.InitTripWires(Camobject.alerts.pluginconfig);
                kinectStream.TripWire += Alert;
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

            Camera?.Dispose();

            Camera = new Camera(source);
            Camera.NewFrame -= CameraNewFrame;
            Camera.PlayingFinished -= VideoDeviceVideoFinished;
            Camera.Alert -= Alert;
            Camera.Detect -= Detect;
            Camera.ErrorHandler -= CameraWindow_ErrorHandler;


            Camera.NewFrame += CameraNewFrame;
            Camera.PlayingFinished += VideoDeviceVideoFinished;
            Camera.Alert += Alert;
            Camera.Detect += Detect;
            Camera.ErrorHandler += CameraWindow_ErrorHandler;

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
            var a = Camera?.Plugin?.GetType().GetMethod("ExecuteCommand");
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
            var a = Camera?.Plugin?.GetType().GetMethod("ExecuteShortcut");
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
            if (Camera?.Plugin != null)
            {
                var a = (string)Camera.Plugin.GetType().GetMethod("ProcessAlert").Invoke(Camera.Plugin, new object[] { eventArgs.Description });
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
                                Alert(description, EventArgs.Empty);
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
                _stopWrite.Set();
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