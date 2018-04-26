using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using Antiufo.Controls;
using FFmpeg.AutoGen;
using iSpyApplication.Cloud;
using iSpyApplication.Controls;
using iSpyApplication.Joystick;
using iSpyApplication.Onvif;
using iSpyApplication.Properties;
using iSpyApplication.Server;
using iSpyApplication.Sources;
using iSpyApplication.Sources.Audio;
using iSpyApplication.Sources.Audio.talk;
using iSpyApplication.Utilities;
using Microsoft.Win32;
using NATUPNPLib;
using NAudio.Wave;
using NETWORKLIST;
using PictureBox = iSpyApplication.Controls.PictureBox;
using Timer = System.Timers.Timer;

namespace iSpyApplication
{
    /// <summary>
    ///     Summary description for MainForm
    /// </summary>
    public partial class MainForm : Form, INetworkListManagerEvents
    {
        public const string VLCx86 = "http://www.videolan.org/vlc/download-windows.html";
        public const string VLCx64 = "http://download.videolan.org/pub/videolan/vlc/last/win64/";

        public const string Website = "http://www.ispyconnect.com";
        public const string ContentSource = Website;
        public static bool NeedsSync;
        private static DateTime _needsMediaRefresh = DateTime.MinValue;
        //private static Player _player = null;

        public static DateTime LastAlert = DateTime.MinValue;

        public static MainForm InstanceReference;

        public static bool VLCRepeatAll;
        public static bool NeedsMediaRebuild = false;
        public static int MediaPanelPage;
        public static bool LoopBack;
        public static string NL = Environment.NewLine;
        public static Font Drawfont = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular, GraphicsUnit.Pixel);
        public static Font DrawfontBig = new Font(FontFamily.GenericSansSerif, 25, FontStyle.Regular, GraphicsUnit.Pixel);
        public static Font DrawfontMed = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Regular, GraphicsUnit.Pixel);
        public static Font Iconfont = new Font(FontFamily.GenericSansSerif, 15, FontStyle.Bold, GraphicsUnit.Pixel);
        public static Brush IconBrush = new SolidBrush(Color.White);
        public static Brush IconBrushOff = new SolidBrush(Color.FromArgb(64, 255, 255, 255));
        public static Brush IconBrushActive = new SolidBrush(Color.Red);
        public static Brush OverlayBrush = new SolidBrush(Color.White);
        public static int ThreadKillDelay = 10000;
        public static SolidBrush CameraDrawBrush = new SolidBrush(Color.White);
        public static Pen CameraLine = new Pen(Color.Green, 2);
        public static Pen CameraNav = new Pen(Color.White, 1);
        public static Brush RecordBrush = new SolidBrush(Color.Red);
        public static Brush OverlayBackgroundBrush = new SolidBrush(Color.FromArgb(128, 0, 0, 0));
        
        public static string Identifier;
        public static DataTable IPTABLE;
        public static bool IPLISTED = true;
        public static bool IPRTSP = false, IPHTTP = true;
        public static string IPADDR = "";
        public static string IPCHANNEL = "0";
        public static string IPMODEL = "";
        public static string IPUN = "";
        public static string IPPORTS = "80,8080";
        public static int IPPORT = 80;
        public static string IPPASS = "";
        public static string IPTYPE = "";
        public static int Affiliateid = 0;
        public static string EmailAddress = "", MobileNumber = "";
        public static string Group="";
        public static int ThrottleFramerate = 40;
        public static float CpuUsage, CpuTotal;
        public static int RecordingThreads;
        public static List<string> Plugins = new List<string>();
        public static bool NeedsResourceUpdate;
        private static readonly List<FilePreview> Masterfilelist = new List<FilePreview>();

        public static EncoderParameters EncoderParams;
        public static bool ShuttingDown = false;
        public static string Webserver = Website;
        public static string WebserverSecure = Website.Replace("http:", "https:");


        public static Rectangle RPower = new Rectangle(94, 3, 16, 16);
        public static Rectangle RPowerOn = new Rectangle(94, 43, 16, 16);
        public static Rectangle RPowerOff = new Rectangle(94, 83, 16, 16);
        public static Rectangle RAdd = new Rectangle(127, 3, 16, 16);
        public static Rectangle RAddOff = new Rectangle(127, 83, 16, 16);
        public static Rectangle REdit = new Rectangle(3, 2, 16, 16);
        public static Rectangle REditOff = new Rectangle(3, 82, 16, 16);
        public static Rectangle RHold = new Rectangle(255, 2, 16, 16);
        public static Rectangle RHoldOn = new Rectangle(284, 42, 16, 16);
        public static Rectangle RHoldOff = new Rectangle(284, 82, 16, 16);
        public static Rectangle RRecord = new Rectangle(188,2,16,16);
        public static Rectangle RRecordOn = new Rectangle(188, 42, 16, 16);
        public static Rectangle RRecordOff = new Rectangle(188, 82, 16, 16);
        public static Rectangle RNext = new Rectangle(65, 3, 16, 16);
        public static Rectangle RNextOff = new Rectangle(65, 82, 16, 16);
        public static Rectangle RGrab = new Rectangle(157,2,16,16);
        public static Rectangle RGrabOff = new Rectangle(157, 82, 16, 16);
        public static Rectangle RTalk = new Rectangle(313, 2, 16,16);
        public static Rectangle RTalkOn = new Rectangle(313, 42, 16, 16);
        public static Rectangle RTalkOff = new Rectangle(313, 82, 16, 16);
        public static Rectangle RFiles = new Rectangle(223,3,16,16);
        public static Rectangle RFilesOff = new Rectangle(223, 83, 16, 16);
        public static Rectangle RListen = new Rectangle(347,2,16,16);
        public static Rectangle RListenOn = new Rectangle(380,43,16,16);
        public static Rectangle RListenOff = new Rectangle(347, 83, 16, 16);
        public static Rectangle RWeb = new Rectangle(411, 3, 16, 16);
        public static Rectangle RWebOff = new Rectangle(411, 83, 16, 16);
        public static Rectangle RText = new Rectangle(443, 3, 16, 16);
        public static Rectangle RTextOff = new Rectangle(443, 83, 16, 16);
        public static Rectangle RFolder = new Rectangle(473, 3, 16, 16);
        public static Rectangle RFolderOff = new Rectangle(473, 83, 16, 16);

        private static List<string> _tags;
        private static bool CustomWebserver;
        public static List<string> Tags
        {
            get
            {
                if (_tags != null)
                    return _tags;
                _tags = new List<string>();
                if (!string.IsNullOrEmpty(Conf.Tags))
                {
                    var l = Conf.Tags.Split(',').ToList();
                    foreach (var t in l)
                    {
                        if (!string.IsNullOrEmpty(t))
                        {
                            var s = t.Trim();
                            if (!s.StartsWith("{"))
                                s = "{" + s;
                            if (!s.EndsWith("}"))
                                s = s + "}";
                            _tags.Add(s.ToUpper(CultureInfo.InvariantCulture));
                        }
                    }
                }
                return _tags;
            }
            set { _tags = value; }
        } 
        public ISpyControl LastFocussedControl = null;

        internal static LocalServer MWS;

        public static string PurchaseLink = "http://www.ispyconnect.com/astore.aspx";
        private static int _storageCounter;
        private static Timer _rescanIPTimer, _tmrJoystick;
        
        private static string _counters = "";
        private static readonly Random Random = new Random();
        private static ViewController _vc;
        private static int _pingCounter;
        private static ImageCodecInfo _encoder;
        private static bool _needsDelete = false;
        

        

        
        
        private static string _browser = String.Empty;

        
        private MenuItem menuItem37;
        private ToolStripMenuItem tagsToolStripMenuItem;
        private MenuItem menuItem38;
        private MenuItem menuItem39;
        private MenuItem menuItem40;
        private ToolStripMenuItem openWebInterfaceToolStripMenuItem;
        

        public static void AddObject(object o)
        {
            var c = o as objectsCamera;
            if (c != null)
            {
                c.settings.order = MaxOrderIndex;
                _cameras.Add(c);
                MaxOrderIndex++;
            }
            var m = o as objectsMicrophone;
            if (m != null)
            {
                m.settings.order = MaxOrderIndex;
                _microphones.Add(m);
                MaxOrderIndex++;
            }
            var f = o as objectsFloorplan;
            if (f != null)
            {
                f.order = MaxOrderIndex;
                _floorplans.Add(f);
                MaxOrderIndex++;

            }
            var a = o as objectsActionsEntry;
            if (a != null)
                _actions.Add(a);
            var oc = o as objectsCommand;
            if(oc!=null)
                _remotecommands.Add(oc);
            LayoutPanel.NeedsRedraw = true;
        }

        private static List<PTZSettings2Camera> _ptzs;
        private static List<ManufacturersManufacturer> _sources;
        private static IPAddress[] _ipv4Addresses, _ipv6Addresses;
        
        private readonly List<float> _cpuAverages = new List<float>();
        private readonly int _mCookie = -1;
        private readonly IConnectionPoint _mIcp;
        private static readonly object ThreadLock = new object();

        private readonly FolderSelectDialog _fbdSaveTo = new FolderSelectDialog
                                                        {
                                                            Title = "Select a folder to copy the file to"
                                                        };


        public object ContextTarget;
        //internal Player Player;
        internal PlayerVLC PlayerVLC;
        public McRemoteControlManager.RemoteControlDevice RemoteManager;
        public bool SilentStartup;
        
        internal CameraWindow TalkCamera;

        private MenuItem _aboutHelpItem;
        private ToolStripMenuItem _addCameraToolStripMenuItem;
        private ToolStripMenuItem _addFloorPlanToolStripMenuItem;
        private ToolStripMenuItem _addMicrophoneToolStripMenuItem;
        private ToolStripMenuItem _applyScheduleToolStripMenuItem;
        private ToolStripMenuItem _applyScheduleToolStripMenuItem1;
        private bool _closing;
        private PerformanceCounter _cpuCounter, _cputotalCounter;
        private ToolStripMenuItem _deleteToolStripMenuItem;
        private ToolStripMenuItem _editToolStripMenuItem;
        private MenuItem _exitFileItem;
        private ToolStripMenuItem _exitToolStripMenuItem;
        private MenuItem _fileItem;
        private ToolStripMenuItem _floorPlanToolStripMenuItem;
        private FileSystemWatcher _fsw;
        private MenuItem _helpItem;
        private ToolStripMenuItem _helpToolstripMenuItem;
        private Timer _houseKeepingTimer;
        private ToolStripMenuItem _iPCameraToolStripMenuItem;
        private static string _lastPath = Program.AppDataPath;
        private static string _currentFileName = "";
        private ToolStripMenuItem _listenToolStripMenuItem;
        private ToolStripMenuItem _localCameraToolStripMenuItem;
        private PersistWindowState _mWindowState;
        private MenuItem _menuItem1;
        private MenuItem _menuItem10;
        private MenuItem _menuItem12;
        private MenuItem _menuItem13;
        private MenuItem _menuItem14;
        private MenuItem _menuItem15;
        private MenuItem _menuItem16;
        private MenuItem _menuItem17;
        private MenuItem _menuItem18;
        private MenuItem _menuItem19;
        private MenuItem _menuItem2;
        private MenuItem _menuItem20;
        private MenuItem _menuItem21;
        private MenuItem _menuItem22;
        private MenuItem _menuItem23;
        private MenuItem _menuItem24;
        private MenuItem _menuItem25;
        private MenuItem _menuItem26;
        private MenuItem _menuItem27;
        private MenuItem _menuItem28;
        private MenuItem _menuItem29;
        private MenuItem _menuItem3;
        private MenuItem _menuItem30;
        private MenuItem _menuItem31;
        private MenuItem _menuItem32;
        private MenuItem _menuItem33;
        private MenuItem _menuItem34;
        private MenuItem _menuItem35;
        private MenuItem _menuItem36;
        private MenuItem _menuItem37;
        private MenuItem _menuItem38;
        private MenuItem _menuItem39;
        private MenuItem _menuItem4;
        private MenuItem _menuItem5;
        private MenuItem _menuItem6;
        private MenuItem _menuItem7;
        private MenuItem _menuItem8;
        private MenuItem _menuItem9;
        private MenuItem _miApplySchedule;
        private MenuItem _miOffAll;
        private MenuItem _miOffSched;
        private MenuItem _miOnAll;
        private MenuItem _miOnSched;
        private ToolStripMenuItem _microphoneToolStripMenuItem;
        private ToolStripMenuItem _onMobileDevicesToolStripMenuItem;
        private PerformanceCounter _pcMem;
        public LayoutPanel _pnlCameras;
        private Panel _pnlContent;
        private ToolStripMenuItem _positionToolStripMenuItem;
        private FormWindowState _previousWindowState = FormWindowState.Normal;
        private PTZTool _ptzTool;
        private ToolStripMenuItem _recordNowToolStripMenuItem;
        private ToolStripMenuItem _remoteCommandsToolStripMenuItem;
        private ToolStripMenuItem _resetRecordingCounterToolStripMenuItem;
        private ToolStripMenuItem _resetSizeToolStripMenuItem;
        private ToolStripMenuItem _settingsToolStripMenuItem;
        private ToolStripMenuItem _showFilesToolStripMenuItem;
        private ToolStripMenuItem _showISpy100PercentOpacityToolStripMenuItem;
        private ToolStripMenuItem _showISpy10PercentOpacityToolStripMenuItem;
        private ToolStripMenuItem _showISpy30OpacityToolStripMenuItem;
        private ToolStripMenuItem _showToolstripMenuItem;
        private bool _shuttingDown;
        private string _startCommand = "";
        private Thread _storageThread;
        private ToolStripMenuItem _switchAllOffToolStripMenuItem;
        private ToolStripMenuItem _switchAllOnToolStripMenuItem;
        private ToolStripMenuItem _takePhotoToolStripMenuItem;
        private IAudioSource _talkSource;
        private ITalkTarget _talkTarget;
        private ToolStripMenuItem _thruWebsiteToolStripMenuItem;
        private ToolStripButton _toolStripButton1;
        private ToolStripButton _toolStripButton4;
        private ToolStripButton _toolStripButton8;
        private ToolStripDropDownButton _toolStripDropDownButton1;
        private ToolStripDropDownButton _toolStripDropDownButton2;
        private ToolStripMenuItem _viewMediaToolStripMenuItem;
        private ToolStripStatusLabel _tsslStats;
        private ToolStripMenuItem _uSbCamerasAndMicrophonesOnOtherToolStripMenuItem;
        private ToolStripMenuItem _unlockToolstripMenuItem;
        private Thread _updateChecker;
        private Timer _updateTimer;
        private ToolStripMenuItem _viewMediaOnAMobileDeviceToolStripMenuItem;
        private ToolStripMenuItem _websiteToolstripMenuItem;
        private ToolStripMenuItem alwaysOnTopToolStripMenuItem1;
        private ToolStripMenuItem autoLayoutToolStripMenuItem;
        private IContainer components;
        private ToolStripMenuItem configurePluginToolStripMenuItem;
        private ContextMenuStrip ctxtMainForm;
        private ContextMenuStrip ctxtMnu;
        private ContextMenuStrip ctxtPlayer;
        private ContextMenuStrip ctxtTaskbar;
        private ToolStripMenuItem defaultPlayerToolStripMenuItem;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ToolStripMenuItem displayToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem fileMenuToolStripMenuItem;
        private FlowLayoutPanel flCommands;
        internal MediaPanel flowPreview;
        private ToolStripMenuItem fullScreenToolStripMenuItem;
        private ToolStripMenuItem fullScreenToolStripMenuItem1;
        private ToolStripMenuItem iPCameraWithWizardToolStripMenuItem;
        private ToolStripMenuItem iSpyToolStripMenuItem;
        private ToolStripMenuItem inExplorerToolStripMenuItem;
        private ToolStripMenuItem layoutToolStripMenuItem;
        private MainMenu mainMenu;
        private ToolStripMenuItem mediaPaneToolStripMenuItem;
        private MenuItem menuItem1;
        private MenuItem menuItem10;
        private MenuItem menuItem11;
        private MenuItem menuItem12;
        private MenuItem menuItem13;
        private MenuItem menuItem14;
        private MenuItem menuItem15;
        private MenuItem menuItem16;
        private MenuItem menuItem17;
        private MenuItem menuItem18;
        private MenuItem menuItem19;
        private MenuItem menuItem2;
        private MenuItem menuItem20;
        private MenuItem menuItem21;
        private MenuItem menuItem22;
        private MenuItem menuItem23;
        private MenuItem menuItem24;
        private MenuItem menuItem26;
        private MenuItem menuItem3;
        private MenuItem menuItem4;
        private MenuItem menuItem5;
        private MenuItem menuItem6;
        private MenuItem menuItem7;
        private MenuItem menuItem8;
        private MenuItem menuItem9;
        private MenuItem mnuResetLayout;
        private MenuItem mnuSaveLayout;
        private NotifyIcon notifyIcon1;

        private ToolStripMenuItem oNVIFCameraToolStripMenuItem;
        private ToolStripMenuItem opacityToolStripMenuItem;
        private ToolStripMenuItem opacityToolStripMenuItem1;
        private ToolStripMenuItem opacityToolStripMenuItem2;
        private ToolStripMenuItem opacityToolStripMenuItem3;
        private ToolStripMenuItem otherVideoSourceToolStripMenuItem;
        private ToolStripMenuItem pTZControllerToolStripMenuItem;
        private ToolStripMenuItem pTZControllerToolStripMenuItem1;
        private ToolStripMenuItem pTZToolStripMenuItem;
        private Panel panel2;
        private ToolStripMenuItem pluginCommandsToolStripMenuItem;
        private ToolStripMenuItem resetLayoutToolStripMenuItem1;
        private ToolStripMenuItem saveLayoutToolStripMenuItem1;
        private ToolStripMenuItem saveToToolStripMenuItem;
        private ToolStripMenuItem showInFolderToolStripMenuItem;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private ToolStripMenuItem statusBarToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStrip toolStripMenu;
        private ToolStripMenuItem toolStripToolStripMenuItem;
        private ToolTip toolTip1;
        private ToolStripButton tsbPlugins;
        private ToolStripStatusLabel tsslMediaInfo;
        private ToolStripStatusLabel tsslMonitor;
        private ToolStripStatusLabel tsslPerformance;
        private ToolStripMenuItem uploadToYouTubePublicToolStripMenuItem;
        private ToolStripMenuItem videoFileToolStripMenuItem;
        private ToolStripMenuItem viewControllerToolStripMenuItem;
        private MenuItem menuItem27;
        private MenuItem menuItem28;
        private ToolStripDropDownButton tssbGridViews;
        private ToolStripMenuItem manageToolStripMenuItem;
        private ToolStripMenuItem archiveToolStripMenuItem;
        private MenuItem menuItem29;
        private MenuItem menuItem30;
        private MenuItem menuItem25;
        private ToolStripMenuItem gridViewsToolStripMenuItem;
        private ToolStripMenuItem maximiseToolStripMenuItem;
        private ToolStripMenuItem uploadToCloudToolStripMenuItem;
        private MediaPanelControl mediaPanelControl1;
        private ToolStripMenuItem viewLogFileToolStripMenuItem;
        private ToolStripMenuItem switchToolStripMenuItem;
        private ToolStripMenuItem alertsOnToolStripMenuItem1;
        private ToolStripMenuItem alertsOffToolStripMenuItem;
        private ToolStripMenuItem scheduleOnToolStripMenuItem;
        private ToolStripMenuItem scheduleOffToolStripMenuItem;
        private ToolStripMenuItem onToolStripMenuItem;
        private ToolStripMenuItem offToolStripMenuItem;
        private ToolStripMenuItem pTZScheduleOnToolStripMenuItem;
        private ToolStripMenuItem pTZScheduleOffToolStripMenuItem;
        private MenuItem menuItem31;
        private MenuItem menuItem32;
        private MenuItem menuItem33;
        private ToolStripMenuItem gridViewsToolStripMenuItem1;
        private ToolStripStatusLabel tsslPRO;
        private MenuItem menuItem34;
        private MenuItem menuItem35;
        private MenuItem menuItem36;
        private ToolStripMenuItem websiteToolStripMenuItem;

        public MainForm(bool silent, string command)
        {
            if (Conf.StartupForm != "iSpy")
            {
                SilentStartup = true;
            }

            SilentStartup = SilentStartup || silent || Conf.Enable_Password_Protect || Conf.StartupMode == 1;

            //need to wrap initialize component
            if (SilentStartup)
            {
                ShowInTaskbar = false;
                ShowIcon = false;
                WindowState = FormWindowState.Minimized;
            }
            else
            {
                switch (Conf.StartupMode)
                {
                    case 0:
                        _mWindowState = new PersistWindowState {Parent = this, RegistryPath = @"Software\ispy\startup"};
                        break;
                    case 2:
                        WindowState = FormWindowState.Maximized;
                        break;
                    case 3:
                        WindowState = FormWindowState.Maximized;
                        FormBorderStyle = FormBorderStyle.None;
                        break;
                }
            }

            InitializeComponent();

            if (!SilentStartup)
            {
                if (Conf.StartupMode == 0)
                    _mWindowState = new PersistWindowState {Parent = this, RegistryPath = @"Software\ispy\startup"};
            }


            RenderResources();

            _startCommand = command;

            Windows7Renderer r = Windows7Renderer.Instance;
            toolStripMenu.Renderer = r;
            statusStrip1.Renderer = r;

            _pnlCameras.BackColor = Conf.MainColor.ToColor();

            RemoteManager = new McRemoteControlManager.RemoteControlDevice();
            RemoteManager.ButtonPressed += RemoteManagerButtonPressed;

            SetPriority();
            Arrange(false);


            _jst = new JoystickDevice();
            bool jsactive = false;
            string[] sticks = _jst.FindJoysticks();
            foreach (string js in sticks)
            {
                string[] nameid = js.Split('|');
                if (nameid[1] == Conf.Joystick.id)
                {
                    Guid g = Guid.Parse(nameid[1]);
                    jsactive = _jst.AcquireJoystick(g);
                }
            }

            if (!jsactive)
            {
                _jst.ReleaseJoystick();
                _jst = null;
            }
            else
            {
                _tmrJoystick = new Timer(100);
                _tmrJoystick.Elapsed += TmrJoystickElapsed;
                _tmrJoystick.Start();
            }
            try
            {
                INetworkListManager mNlm = new NetworkListManager();
                var icpc = (IConnectionPointContainer) mNlm;
                //similar event subscription can be used for INetworkEvents and INetworkConnectionEvents
                Guid tempGuid = typeof (INetworkListManagerEvents).GUID;
                icpc.FindConnectionPoint(ref tempGuid, out _mIcp);
                if (_mIcp != null)
                {
                    _mIcp.Advise(this, out _mCookie);
                }
            }
            catch (Exception)
            {
                _mIcp = null;
            }
            InstanceReference = this;

            try
            {
                Discovery.FindDevices();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public static DateTime NeedsMediaRefresh
        {
            get { return _needsMediaRefresh; }
            set
            {
                //only store first recorded or reset value
                if (value == DateTime.MinValue)
                    _needsMediaRefresh = value;
                else
                {
                    if (_needsMediaRefresh == DateTime.MinValue)
                        _needsMediaRefresh = value;
                }
            }
        }

        public static ReadOnlyCollection<FilePreview> MasterFileList => Masterfilelist.AsReadOnly();


        public bool StorageThreadRunning
        {
            get
            {
                lock (ThreadLock)
                {
                    try
                    {
                        return _storageThread != null && !_storageThread.Join(TimeSpan.Zero);
                    }
                    catch
                    {
                        return true;
                    }
                }
            }
        }

        public static int ProductID => Program.Platform != "x86" ? 19 : 11;

        private static string DefaultBrowser
        {
            get
            {
                if (!string.IsNullOrEmpty(_browser))
                    return _browser;

                _browser = string.Empty;
                RegistryKey key = null;
                try
                {
                    key = Registry.ClassesRoot.OpenSubKey(@"HTTP\shell\open\command", false);

                    //trim off quotes
                    if (key != null) _browser = key.GetValue(null).ToString().ToLower().Replace("\"", "");
                    if (!_browser.EndsWith(".exe"))
                    {
                        _browser = _browser.Substring(0, _browser.LastIndexOf(".exe", StringComparison.Ordinal) + 4);
                    }
                }
                finally
                {
                    key?.Close();
                }
                return _browser;
            }
        }

        public void ConnectivityChanged(NLM_CONNECTIVITY newConnectivity)
        {
            var i = (int) newConnectivity;
            if (!WsWrapper.WebsiteLive)
            {
                if (newConnectivity != NLM_CONNECTIVITY.NLM_CONNECTIVITY_DISCONNECTED)
                {
                    if ((i & (int) NLM_CONNECTIVITY.NLM_CONNECTIVITY_IPV4_INTERNET) != 0 ||
                        ((int) newConnectivity & (int) NLM_CONNECTIVITY.NLM_CONNECTIVITY_IPV6_INTERNET) != 0)
                    {
                        if (!WsWrapper.WebsiteLive)
                        {
                            WsWrapper.LastLiveCheck = Helper.Now.AddMinutes(-5);
                        }
                    }
                }
            }
        }

        public static void MasterFileAdd(FilePreview fp)
        {
            lock (ThreadLock)
            {
                Masterfilelist.Add(fp);
            }
            var wss = MWS.WebSocketServer;
            if (wss != null)
                wss.SendToAll("new events|" + fp.Name);
        }

        public static void MasterFileRemoveAll(int objecttypeid, int objectid)
        {
            lock (ThreadLock)
            {
                Masterfilelist.RemoveAll(p => p.ObjectTypeId == objecttypeid && p.ObjectId == objectid);
            }
        }

        public static void MasterFileRemove(string filename)
        {
            lock (ThreadLock)
            {
                Masterfilelist.RemoveAll(p => p.Filename == filename);
            }
        }

        private bool IsOnScreen(Form form)
        {
            Screen[] screens = Screen.AllScreens;
            var formTopLeft = new Point(form.Left, form.Top);
            //hack for maximised window
            if (form.WindowState == FormWindowState.Maximized)
            {
                formTopLeft.X += 8;
                formTopLeft.Y += 8;
            }

            return screens.Any(screen => screen.WorkingArea.Contains(formTopLeft));
        }

        protected override void WndProc(ref Message message)
        {
            RemoteManager.ProcessMessage(message);
            base.WndProc(ref message);
        }


        private void RemoteManagerButtonPressed(object sender, McRemoteControlManager.RemoteControlEventArgs e)
        {
            ProcessKey(e.Button.ToString().ToLower());
        }

        public static void SetPriority()
        {
            switch (Conf.Priority)
            {
                case 1:
                    Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.Normal;
                    break;
                case 2:
                    Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.AboveNormal;
                    break;
                case 3:
                    Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
                    break;
                case 4:
                    Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;
                    break;
            }
        }


        /// <summary>
        ///     Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                notifyIcon1.Visible = false;
                notifyIcon1.Dispose();

                components?.Dispose();
                _mWindowState?.Dispose();

                Drawfont.Dispose();
                _updateTimer?.Dispose();
                _houseKeepingTimer?.Dispose();
                //sometimes hangs??
                //if (_fsw != null)
                //    _fsw.Dispose();
            }
            base.Dispose(disposing);
        }

        // Close the main form
        private void ExitFileItemClick(object sender, EventArgs e)
        {
            ShuttingDown = true;
            Close();
        }

        // On "Help->About"
        private void AboutHelpItemClick(object sender, EventArgs e)
        {
            var form = new AboutForm();
            form.ShowDialog(this);
            form.Dispose();
        }

        private void VolumeControlDoubleClick(object sender, EventArgs e)
        {
            _pnlCameras.Maximise(sender);
        }

        private void FloorPlanDoubleClick(object sender, EventArgs e)
        {
            _pnlCameras.Maximise(sender);
        }

        private static Enums.LayoutMode _layoutMode;
        public static Enums.LayoutMode LayoutMode
        {
            get
            {
                return _layoutMode;
            }
            set
            {
                _layoutMode = value;
                
                Conf.ArrangeMode = (_layoutMode == Enums.LayoutMode.AutoGrid ? 1 : 0);
                
            }
        }

        public static bool LockLayout => Conf.LockLayout || _layoutMode == Enums.LayoutMode.AutoGrid;

        private static void AddPlugin(FileInfo dll)
        {
            try
            {
                Assembly plugin = Assembly.LoadFrom(dll.FullName);
                object ins = null;
                try
                {
                    ins = plugin.CreateInstance("Plugins.Main", true);
                }
                catch
                {
                    // ignored
                }
                if (ins != null)
                {
                    Logger.LogMessage("Added: " + dll.FullName);
                    Plugins.Add(dll.FullName);
                }
            }
            catch // (Exception ex)
            {
                //Logger.LogException(ex);
            }
        }


        public void Play(string filename, int objectId, string displayName)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => Play(filename,objectId,displayName)));
                return;
            }
            if (PlayerVLC == null)
            {
                PlayerVLC = new PlayerVLC(displayName, this);
                PlayerVLC.Show(this);
                PlayerVLC.Closed += PlayerClosed;
                PlayerVLC.Activate();
                PlayerVLC.BringToFront();
                PlayerVLC.Owner = this;
            }

            PlayerVLC.ObjectID = objectId;
            PlayerVLC.Play(filename, displayName);

        }

        private void PlayerClosed(object sender, EventArgs e)
        {
            //_player = null;
            PlayerVLC = null;
        }

       

        private void MainFormLoad(object sender, EventArgs e)
        {
            MainInit();
        }

        private void MainInit()
        {
            UISync.Init(this);
            Logger.InitLogging();
            try
            {
                File.WriteAllText(Program.AppDataPath + "exit.txt", "RUNNING");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            try
            {
                ffmpeg.avdevice_register_all();
                ffmpeg.avcodec_register_all();
                ffmpeg.avfilter_register_all();
                ffmpeg.avformat_network_init();
                ffmpeg.av_register_all();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            if (!SilentStartup)
            {
                switch (Conf.StartupMode)
                {
                    case 0:
                        break;
                    case 2:
                        break;
                    case 3:
                        WinApi.SetWinFullScreen(Handle);
                        break;
                }
            }

            mediaPanelControl1.MainClass = this;
            EncoderParams = new EncoderParameters(1)
                            {
                                Param =
                                {
                                    [0] =
                                        new EncoderParameter(
                                        System.Drawing.Imaging.Encoder.Quality,
                                        Conf.JPEGQuality)
                                }
                            };

            //this initializes the port mapping collection
            IStaticPortMappingCollection map = NATControl.Mappings;
            if (string.IsNullOrEmpty(Conf.MediaDirectory) || Conf.MediaDirectory == "NotSet")
            {
                Conf.MediaDirectory = Program.AppDataPath + @"WebServerRoot\Media\";
            }

            if (Conf.MediaDirectories == null || Conf.MediaDirectories.Length == 0)
            {
                Conf.MediaDirectories = new[]
                                        {
                                            new configurationDirectory
                                            {
                                                Entry = Conf.MediaDirectory,
                                                DeleteFilesOlderThanDays =
                                                    Conf.DeleteFilesOlderThanDays,
                                                Enable_Storage_Management =
                                                    Conf.Enable_Storage_Management,
                                                MaxMediaFolderSizeMB = Conf.MaxMediaFolderSizeMB,
                                                StopSavingOnStorageLimit =
                                                    Conf.StopSavingOnStorageLimit,
                                                ID = 0
                                            }
                                        };
            }
            else
            {
                if (Conf.MediaDirectories.First().Entry == "NotSet")
                {
                    Conf.MediaDirectories = new[]
                                            {
                                                new configurationDirectory
                                                {
                                                    Entry = Conf.MediaDirectory,
                                                    DeleteFilesOlderThanDays =
                                                        Conf.DeleteFilesOlderThanDays,
                                                    Enable_Storage_Management =
                                                        Conf.Enable_Storage_Management,
                                                    MaxMediaFolderSizeMB =
                                                        Conf.MaxMediaFolderSizeMB,
                                                    StopSavingOnStorageLimit =
                                                        Conf.StopSavingOnStorageLimit,
                                                    ID = 0
                                                }
                                            };
                }
            }

            //reset stop saving flag
            foreach (configurationDirectory d in Conf.MediaDirectories)
            {
                d.StopSavingFlag = false;
            }

            if (!Directory.Exists(Conf.MediaDirectories[0].Entry))
            {
                string notfound = Conf.MediaDirectories[0].Entry;
                Logger.LogError("Media directory could not be found (" + notfound + ") - reset it to " +
                               Program.AppDataPath + @"WebServerRoot\Media\" + " in settings if it doesn't attach.");
            }

            if (!VlcHelper.VlcInstalled)
            {
                Logger.LogWarningToFile(
                    "VLC not installed - install VLC (" + Program.Platform + ") for additional connectivity.");
                if (Program.Platform == "x64")
                {
                    Logger.LogWarningToFile(
                        "VLC64  must be unzipped so the dll files and folders including libvlc.dll and the plugins folder are in " +
                        Program.AppPath + "VLC64\\");
                    Logger.LogWarningToFile("Download: <a href=\""+VLCx64+"\">"+VLCx64+"</a>");
                }
                else
                    Logger.LogWarningToFile("Download: <a href=\"" + VLCx86 + "\">" + VLCx86 + "</a>");
            }
            else
            {
                Version v = VlcHelper.VlcVersion;
                if (v.CompareTo(VlcHelper.VMin) < 0)
                {
                    Logger.LogWarningToFile(
                        "Old VLC installed - update VLC (" + Program.Platform + ") for additional connectivity.");
                }
                else
                {
                    if (v.CompareTo(new Version(2, 0, 2)) == 0)
                    {
                        Logger.LogWarningToFile(
                            "VLC v2.0.2 detected - there are known issues with this version of VLC (HTTP streaming is broken for a lot of cameras) - if you are having problems with VLC connectivity we recommend you install v2.0.1 ( http://download.videolan.org/pub/videolan/vlc/2.0.1/ ) or the latest (if available).");
                    }
                }
            }


            _fsw = new FileSystemWatcher
                   {
                       Path = Program.AppDataPath,
                       IncludeSubdirectories = false,
                       Filter = "external_command.txt",
                       NotifyFilter = NotifyFilters.LastWrite
                   };
            _fsw.Changed += FswChanged;
            _fsw.EnableRaisingEvents = true;

            tsslPRO.Visible = !Conf.Subscribed;

            Menu = mainMenu;
            notifyIcon1.ContextMenuStrip = ctxtTaskbar;
            Identifier = Guid.NewGuid().ToString();
            MWS = new LocalServer
                  {
                      ServerRoot = Program.AppDataPath + @"WebServerRoot\"
                  };

#if DEBUG
            MWS.ServerRoot = Program.AppPath + @"WebServerRoot\";
#endif

            if (Conf.Monitor)
            {
                Process[] w = Process.GetProcessesByName("ispymonitor");
                if (w.Length == 0)
                {
                    try
                    {
                        var si = new ProcessStartInfo(Program.AppPath + "/ispymonitor.exe", "ispy");
                        Process.Start(si);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }

            SetBackground();

            toolStripMenu.Visible = Conf.ShowToolbar;
            statusStrip1.Visible = Conf.ShowStatus && Helper.HasFeature(Enums.Features.View_Status_Bar);
            Menu = !Conf.ShowFileMenu ? null : mainMenu;

            if (SilentStartup)
            {
                WindowState = FormWindowState.Minimized;
            }

            if (Conf.Password_Protect_Startup)
            {
                _locked = true;
                WindowState = FormWindowState.Minimized;
            }

            if (Conf.Fullscreen && !SilentStartup && !_locked)
            {
                WindowState = FormWindowState.Maximized;
                FormBorderStyle = FormBorderStyle.None;
                WinApi.SetWinFullScreen(Handle);
            }
            

            statusBarToolStripMenuItem.Checked = menuItem4.Checked = Conf.ShowStatus;
            toolStripToolStripMenuItem.Checked = menuItem6.Checked = Conf.ShowToolbar;
            fileMenuToolStripMenuItem.Checked = menuItem5.Checked = Conf.ShowFileMenu;
            fullScreenToolStripMenuItem1.Checked = menuItem3.Checked = Conf.Fullscreen;
            alwaysOnTopToolStripMenuItem1.Checked = menuItem8.Checked = Conf.AlwaysOnTop;
            mediaPaneToolStripMenuItem.Checked = menuItem7.Checked = Conf.ShowMediaPanel;
            menuItem22.Checked = Conf.LockLayout;
            menuItem39.Checked = LayoutMode == Enums.LayoutMode.AutoGrid;
            TopMost = Conf.AlwaysOnTop;

            Iconfont = new Font(FontFamily.GenericSansSerif, Conf.BigButtons ? 22 : 15, FontStyle.Bold,
                GraphicsUnit.Pixel);
            double dOpacity;
            Double.TryParse(Conf.Opacity.ToString(CultureInfo.InvariantCulture), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out dOpacity);
            Opacity = dOpacity/100.0;


            if (Conf.ServerName == "NotSet")
            {
                Conf.ServerName = SystemInformation.ComputerName;
            }

            notifyIcon1.Text = Conf.TrayIconText;
            notifyIcon1.BalloonTipClicked += NotifyIcon1BalloonTipClicked;
            autoLayoutToolStripMenuItem.Checked = menuItem26.Checked = Conf.AutoLayout;

            _updateTimer = new Timer(200);
            _updateTimer.Elapsed += UpdateTimerElapsed;
            _updateTimer.AutoReset = true;
            _updateTimer.SynchronizingObject = this;
            //GC.KeepAlive(_updateTimer);

            _houseKeepingTimer = new Timer(1000);
            _houseKeepingTimer.Elapsed += HouseKeepingTimerElapsed;
            _houseKeepingTimer.AutoReset = true;
            _houseKeepingTimer.SynchronizingObject = this;
            //GC.KeepAlive(_houseKeepingTimer);

            //load plugins
            LoadPlugins();

            resetLayoutToolStripMenuItem1.Enabled = mnuResetLayout.Enabled = false; //reset layout

            NetworkChange.NetworkAddressChanged += NetworkChangeNetworkAddressChanged;
            mediaPaneToolStripMenuItem.Checked = Conf.ShowMediaPanel;
            ShowHideMediaPane();
            if (!string.IsNullOrEmpty(Conf.MediaPanelSize))
            {
                string[] dd = Conf.MediaPanelSize.Split('x');
                int d1 = Convert.ToInt32(dd[0]);
                int d2 = Convert.ToInt32(dd[1]);
                try
                {
                    splitContainer1.SplitterDistance = d1;
                    splitContainer2.SplitterDistance = d2;
                }
                catch
                {
                    // ignored
                }
            }
            //load in object list

            if (_startCommand.Trim().StartsWith("open"))
            {
                ParseCommand(_startCommand);
                _startCommand = "";
            }
            else
            {
                if (!File.Exists(Program.AppDataPath + @"XML\objects.xml"))
                {
                    File.Copy(Program.AppPath + @"XML\objects.xml", Program.AppDataPath + @"XML\objects.xml");
                }
                ParseCommand("open " + Program.AppDataPath + @"XML\objects.xml");
            }
            if (_startCommand != "")
            {
                ParseCommand(_startCommand);
            }

            StopAndStartServer();

            if (_mWindowState == null)
            {
                _mWindowState = new PersistWindowState {Parent = this, RegistryPath = @"Software\ispy\startup"};
            }

            if (Conf.Enabled_ShowGettingStarted)
                ShowGettingStarted();

            if (File.Exists(Program.AppDataPath + "custom.txt"))
            {
                string[] cfg =
                    File.ReadAllText(Program.AppDataPath + "custom.txt").Split(Environment.NewLine.ToCharArray());
                bool setSecure = false;
                foreach (string s in cfg)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        string[] nv = s.Split('=');

                        if (nv.Length > 1)
                        {
                            switch (nv[0].ToLower().Trim())
                            {
                                case "business":
                                    Conf.Vendor = nv[1].Trim();
                                    break;
                                case "link":
                                    PurchaseLink = nv[1].Trim();
                                    break;
                                case "manufacturer":
                                    IPTYPE = Conf.DefaultManufacturer = nv[1].Trim();
                                    break;
                                case "model":
                                    IPMODEL = nv[1].Trim();
                                    break;
                                case "affiliateid":
                                case "affiliate id":
                                case "aid":
                                    int aid;
                                    if (Int32.TryParse(nv[1].Trim(), out aid))
                                    {
                                        Affiliateid = aid;
                                    }
                                    break;
                                case "tags":
                                    if (string.IsNullOrEmpty(Conf.Tags))
                                        Conf.Tags = nv[1].Trim();
                                    break;
                                case "featureset":
                                    //only want to set this on install (allow them to modify)
                                    if (Conf.FirstRun)
                                    {
                                        int featureset;
                                        if (Int32.TryParse(nv[1].Trim(), out featureset))
                                        {
                                            Conf.FeatureSet = featureset;
                                        }
                                    }
                                    break;
                                case "permissions":
                                    //only want to set this on install (allow them to modify)
                                    if (Conf.FirstRun)
                                    {
                                        var groups = nv[1].Trim().Split('|');
                                        var l = new List<configurationGroup>();
                                        foreach (var g in groups)
                                        {
                                            if (!string.IsNullOrEmpty(g))
                                            {
                                                var g2 = g.Split(',');
                                                if (g2.Length >= 3)
                                                {
                                                    if (!string.IsNullOrEmpty(g2[0]))
                                                    {
                                                        int perm;
                                                        if (int.TryParse(g2[2], out perm))
                                                        {
                                                            l.Add(new configurationGroup
                                                                  {
                                                                      featureset = perm,
                                                                      name = g2[0],
                                                                      password =
                                                                          EncDec.EncryptData(g2[1],
                                                                              Conf.EncryptCode)
                                                                  });
                                                        }
                                                    }
                                                }
                                            }   
                                        }
                                        if (l.FirstOrDefault(p => p.name.ToLower() == "admin") == null)
                                        {
                                            l.Add(new configurationGroup{
                                                      featureset = 1,
                                                      name = "Admin",
                                                      password = ""
                                                  });
                                        }
                                        if (l.Count>0)
                                            Conf.Permissions = l.ToArray();

                                    }
                                    break;
                                case "webserver":
                                    string ws = nv[1].Trim().Trim('/');
                                    if (!string.IsNullOrEmpty(ws))
                                    {
                                        Webserver = ws;
                                        if (!setSecure)
                                            WebserverSecure = Webserver;
                                        CustomWebserver = true;
                                    }
                                    break;
                                case "webserversecure":
                                    WebserverSecure = nv[1].Trim().Trim('/');
                                    setSecure = true;
                                    break;
                                case "recordondetect":
                                    bool defaultRecordOnDetect;
                                    if (bool.TryParse(nv[1].Trim(), out defaultRecordOnDetect))
                                        Conf.DefaultRecordOnDetect = defaultRecordOnDetect;
                                    break;
                                case "recordonalert":
                                    bool defaultRecordOnAlert;
                                    if (bool.TryParse(nv[1].Trim(), out defaultRecordOnAlert))
                                        Conf.DefaultRecordOnAlert = defaultRecordOnAlert;
                                    break;
                            }
                        }
                    }
                }
                Conf.FirstRun = false;
                Logger.LogMessage("Webserver: " + Webserver);

                string logo = Program.AppDataPath + "logo.jpg";
                if (!File.Exists(logo))
                    logo = Program.AppDataPath + "logo.png";

                if (File.Exists(logo))
                {
                    try
                    {
                        Image bmp = Image.FromFile(logo);
                        var pb = new PictureBox {Image = bmp};
                        pb.Width = pb.Image.Width;
                        pb.Height = pb.Image.Height;

                        pb.Left = _pnlCameras.Width/2 - pb.Width/2;
                        pb.Top = _pnlCameras.Height/2 - pb.Height/2;

                        _pnlCameras.Controls.Add(pb);
                        _pnlCameras.BrandedImage = pb;
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex);
                    }
                }
                _lastClicked = _pnlCameras;
            }

            LoadCommands();
            if (!SilentStartup && Conf.ViewController)
            {
                ShowViewController();
                viewControllerToolStripMenuItem.Checked = menuItem14.Checked = true;
            }

            pTZControllerToolStripMenuItem.Checked =
                menuItem18.Checked = pTZControllerToolStripMenuItem1.Checked = Conf.ShowPTZController;

            if (Conf.ShowPTZController && !SilentStartup)
                ShowHidePTZTool();


            ListGridViews();

            Conf.RunTimes++;

            try
            {
                _cputotalCounter = new PerformanceCounter("Processor", "% Processor Time", "_total", true);
                _cpuCounter = new PerformanceCounter("Process", "% Processor Time",
                    Process.GetCurrentProcess().ProcessName, true);
                try
                {
                    _pcMem = new PerformanceCounter("Process", "Working Set - Private",
                        Process.GetCurrentProcess().ProcessName, true);
                }
                catch
                {
                    try
                    {
                        _pcMem = new PerformanceCounter("Memory", "Available MBytes");
                    }
                    catch (Exception ex2)
                    {
                        Logger.LogException(ex2);
                        _pcMem = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                _cputotalCounter = null;
            }


            if (Conf.StartupForm != "iSpy")
            {
                ShowGridView(Conf.StartupForm);
                
            }

            foreach (var cg in Conf.GridViews)
            {
                if (cg.ShowAtStartup)
                {
                    ShowGridView(cg.name);
                }
            }

            var t = new Thread(()=>ConnectServices()) {IsBackground = true};
            t.Start();

            _updateTimer.Start();
            _houseKeepingTimer.Start();
        }

        internal void ShowGridView(string name)
        {
            configurationGrid cg = Conf.GridViews.FirstOrDefault(p => p.name == name);
            if (cg != null)
            {
                for(int i=0;i<_views.Count;i++)
                {
                    GridView g = _views[i];
                    if (g != null && !g.IsDisposed)
                    {
                        if (g.Cg == cg)
                        {
                            g.BringToFront();
                            g.Focus();
                            return;
                        }
                    }
                    else
                    {
                        _views.RemoveAt(i);
                        i--;
                    }
                        
                }
                var gv = new GridView(this, ref cg);
                gv.Show();
                _views.Add(gv);
            }
        }
        private readonly List<GridView> _views = new List<GridView>();

        public static void LoadPlugins()
        {
            Plugins = new List<string>();
            if (Directory.Exists(Program.AppPath + "Plugins"))
            {
                var plugindir = new DirectoryInfo(Program.AppPath + "Plugins");
                Logger.LogMessage("Checking Plugins...");
                foreach (FileInfo dll in plugindir.GetFiles("*.dll"))
                {
                    AddPlugin(dll);
                }
                foreach (DirectoryInfo d in plugindir.GetDirectories())
                {
                    Logger.LogMessage(d.Name);
                    foreach (FileInfo dll in d.GetFiles("*.dll"))
                    {
                        AddPlugin(dll);
                    }
                }
            }
        }

        private static void NetworkChangeNetworkAddressChanged(object sender, EventArgs e)
        {
            //schedule update check for a few seconds as a network change involves 2 calls to this event - removing and adding.
            if (_rescanIPTimer == null)
            {
                _rescanIPTimer = new Timer(5000);
                _rescanIPTimer.Elapsed += RescanIPTimerElapsed;
                _rescanIPTimer.Start();
            }
        }

        private static void RescanIPTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _rescanIPTimer.Stop();
            _rescanIPTimer = null;
            try
            {
                if (Conf.IPMode == "IPv4")
                {
                    _ipv4Addresses = null;
                    bool iplisted = false;
                    foreach (IPAddress ip in AddressListIPv4)
                    {
                        if (Conf.IPv4Address == ip.ToString())
                            iplisted = true;
                    }
                    if (!iplisted)
                    {
                        _ipv4Address = "";
                        Conf.IPv4Address = AddressIPv4;
                    }
                    if (iplisted)
                        return;
                }
                if (!string.IsNullOrEmpty(Conf.WSUsername) && !string.IsNullOrEmpty(Conf.WSPassword))
                {
                    switch (Conf.IPMode)
                    {
                        case "IPv4":

                            Logger.LogError(
                                "Your IP address has changed. Please set a static IP address for your local computer to ensure uninterrupted connectivity.");
                            //force reload of ip info
                            AddressIPv4 = Conf.IPv4Address;
                            if (Conf.Subscribed)
                            {
                                if (Conf.DHCPReroute && Conf.IPMode == "IPv4")
                                {
                                    //check if IP address has changed
                                    if (Conf.UseUPNP)
                                    {
                                        //change router ports
                                        try
                                        {
                                            if (NATControl.SetPorts(Conf.ServerPort, Conf.LANPort))
                                                Logger.LogMessage("Router port forwarding has been updated. (" +
                                                                        Conf.IPv4Address + ")");
                                        }
                                        catch (Exception ex)
                                        {
                                            Logger.LogException(ex);
                                        }
                                    }
                                    else
                                    {
                                        Logger.LogMessage(
                                            "Please check Use UPNP in web settings to handle this automatically");
                                    }
                                }
                                else
                                {
                                    Logger.LogMessage(
                                        "Enable DHCP Reroute in Web Settings to handle this automatically");
                                }
                            }
                            MWS.StopServer();
                            MWS.StartServer();
                            WsWrapper.ForceSync();
                            break;
                        case "IPv6":
                            _ipv6Addresses = null;
                            bool iplisted = false;
                            foreach (IPAddress ip in AddressListIPv6)
                            {
                                if (Conf.IPv6Address == ip.ToString())
                                    iplisted = true;
                            }
                            if (!iplisted)
                            {
                                Logger.LogError(
                                    "Your IP address has changed. Please set a static IP address for your local computer to ensure uninterrupted connectivity.");
                                _ipv6Address = "";
                                AddressIPv6 = Conf.IPv6Address;
                                Conf.IPv6Address = AddressIPv6;
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex,"network change");
            }
        }

        internal void RenderResources()
        {
            Helper.SetTitle(this);
            uploadToCloudToolStripMenuItem.Text = LocRm.GetString("UploadToCloud");
            uploadToYouTubePublicToolStripMenuItem.Text = LocRm.GetString("UploadToYouTube");
            archiveToolStripMenuItem.Text = LocRm.GetString("Archive");
            saveToToolStripMenuItem.Text = LocRm.GetString("SaveTo");
            deleteToolStripMenuItem.Text = LocRm.GetString("Delete");
            showInFolderToolStripMenuItem.Text = LocRm.GetString("ShowInFolder");
            maximiseToolStripMenuItem.Text = LocRm.GetString("Maximise");
            _aboutHelpItem.Text = LocRm.GetString("About");
            switchToolStripMenuItem.Text = LocRm.GetString("Switch");
            onToolStripMenuItem.Text = LocRm.GetString("On");
            offToolStripMenuItem.Text = LocRm.GetString("Off");
            alertsOnToolStripMenuItem1.Text = LocRm.GetString("AlertsOn");
            alertsOffToolStripMenuItem.Text = LocRm.GetString("AlertsOff");
            scheduleOnToolStripMenuItem.Text = LocRm.GetString("ScheduleOn");
            scheduleOffToolStripMenuItem.Text = LocRm.GetString("ScheduleOff");
            pTZScheduleOnToolStripMenuItem.Text = LocRm.GetString("PTZScheduleOn");
            pTZScheduleOffToolStripMenuItem.Text = LocRm.GetString("PTZScheduleOff");
            openWebInterfaceToolStripMenuItem.Text = LocRm.GetString("OpenWebInterface");
            menuItem33.Text = LocRm.GetString("Lock");
            
            _addCameraToolStripMenuItem.Text = LocRm.GetString("AddCamera");
            _addFloorPlanToolStripMenuItem.Text = LocRm.GetString("AddFloorplan");
            _addMicrophoneToolStripMenuItem.Text = LocRm.GetString("Addmicrophone");
            menuItem26.Text = autoLayoutToolStripMenuItem.Text = LocRm.GetString("AutoLayout");
            gridViewsToolStripMenuItem.Text = gridViewsToolStripMenuItem1.Text = LocRm.GetString("GridViews");
            _deleteToolStripMenuItem.Text = LocRm.GetString("remove");
            _editToolStripMenuItem.Text = LocRm.GetString("Edit");
            _exitFileItem.Text = LocRm.GetString("Exit");
            _exitToolStripMenuItem.Text = LocRm.GetString("Exit");
            _fileItem.Text = LocRm.GetString("file");
            fileMenuToolStripMenuItem.Text = LocRm.GetString("Filemenu");
            menuItem5.Text = LocRm.GetString("Filemenu");
            _floorPlanToolStripMenuItem.Text = LocRm.GetString("FloorPlan");
            fullScreenToolStripMenuItem.Text = LocRm.GetString("fullScreen");
            fullScreenToolStripMenuItem1.Text = LocRm.GetString("fullScreen");
            _helpItem.Text = LocRm.GetString("help");
            _helpToolstripMenuItem.Text = LocRm.GetString("help");
            _iPCameraToolStripMenuItem.Text = LocRm.GetString("IpCamera");
            _menuItem24.Text = LocRm.GetString("ShowGettingStarted");
            _listenToolStripMenuItem.Text = LocRm.GetString("Listen");
            _localCameraToolStripMenuItem.Text = LocRm.GetString("LocalCamera");
            _menuItem1.Text = "-";
            _menuItem10.Text = LocRm.GetString("checkForUpdates");
            _menuItem13.Text = "-";
            _menuItem15.Text = LocRm.GetString("ResetAllRecordingCounters");
            _menuItem16.Text = LocRm.GetString("View");
            _menuItem17.Text = inExplorerToolStripMenuItem.Text = LocRm.GetString("files");
            _menuItem18.Text = LocRm.GetString("clearCaptureDirectories");
            _menuItem19.Text = LocRm.GetString("saveObjectList");
            _menuItem2.Text = LocRm.GetString("help");
            _menuItem20.Text = viewLogFileToolStripMenuItem.Text = LocRm.GetString("Logfile");
            _menuItem21.Text = LocRm.GetString("openObjectList");
            _menuItem22.Text = LocRm.GetString("LogFiles");
            _menuItem23.Text = LocRm.GetString("audiofiles");
            _menuItem25.Text = LocRm.GetString("MediaOnAMobiledeviceiphon");
            _menuItem26.Text = LocRm.GetString("supportIspyWithADonation");
            _menuItem27.Text = "-";
            _menuItem29.Text = LocRm.GetString("Current");
            _menuItem3.Text = LocRm.GetString("MediaoverTheWeb");
            _menuItem30.Text = "-";
            _menuItem31.Text = LocRm.GetString("removeAllObjects");
            _menuItem32.Text = "-";
            _menuItem33.Text = LocRm.GetString("switchOff");
            _menuItem34.Text = LocRm.GetString("Switchon");
            _miOnAll.Text = LocRm.GetString("All");
            _miOffAll.Text = LocRm.GetString("All");
            _miOnSched.Text = LocRm.GetString("Scheduled");
            _miOffSched.Text = LocRm.GetString("Scheduled");
            _miApplySchedule.Text = _applyScheduleToolStripMenuItem1.Text = LocRm.GetString("ApplySchedule");
            _applyScheduleToolStripMenuItem.Text = LocRm.GetString("ApplySchedule");
            _menuItem35.Text = LocRm.GetString("ConfigureremoteCommands");
            _menuItem36.Text = LocRm.GetString("Edit");
            _menuItem37.Text = LocRm.GetString("CamerasAndMicrophones");
            _menuItem38.Text = LocRm.GetString("ViewUpdateInformation");
            _menuItem39.Text = LocRm.GetString("AutoLayoutObjects");
            _menuItem4.Text = LocRm.GetString("ConfigureremoteAccess");
            _menuItem5.Text = LocRm.GetString("GoTowebsite");
            _menuItem6.Text = "-";
            _menuItem7.Text = LocRm.GetString("videofiles");
            _menuItem8.Text = LocRm.GetString("settings");
            _menuItem9.Text = LocRm.GetString("options");
            menuItem39.Text = LocRm.GetString("Grid");
            _microphoneToolStripMenuItem.Text = LocRm.GetString("Microphone");
            notifyIcon1.Text = LocRm.GetString("Ispy");
            _onMobileDevicesToolStripMenuItem.Text = LocRm.GetString("MobileDevices");

            opacityToolStripMenuItem.Text = LocRm.GetString("Opacity");
            opacityToolStripMenuItem1.Text = LocRm.GetString("Opacity10");
            opacityToolStripMenuItem2.Text = LocRm.GetString("Opacity30");
            opacityToolStripMenuItem3.Text = LocRm.GetString("Opacity100");

            menuItem9.Text = LocRm.GetString("Opacity");
            menuItem10.Text = LocRm.GetString("Opacity10");
            menuItem11.Text = LocRm.GetString("Opacity30");
            menuItem12.Text = LocRm.GetString("Opacity100");


            _positionToolStripMenuItem.Text = LocRm.GetString("Position");
            _recordNowToolStripMenuItem.Text = LocRm.GetString("RecordNow");
            _remoteCommandsToolStripMenuItem.Text = LocRm.GetString("RemoteCommands");
            _resetRecordingCounterToolStripMenuItem.Text = LocRm.GetString("ResetRecordingCounter");
            _resetSizeToolStripMenuItem.Text = LocRm.GetString("ResetSize");
            _settingsToolStripMenuItem.Text = LocRm.GetString("settings");
            _showFilesToolStripMenuItem.Text = LocRm.GetString("ShowFiles");
            _showISpy100PercentOpacityToolStripMenuItem.Text = LocRm.GetString("ShowIspy100Opacity");
            _showISpy10PercentOpacityToolStripMenuItem.Text = LocRm.GetString("ShowIspy10Opacity");
            _showISpy30OpacityToolStripMenuItem.Text = LocRm.GetString("ShowIspy30Opacity");
            _showToolstripMenuItem.Text = LocRm.GetString("showIspy");
            statusBarToolStripMenuItem.Text = LocRm.GetString("Statusbar");
            menuItem4.Text = LocRm.GetString("Statusbar");
            _switchAllOffToolStripMenuItem.Text = LocRm.GetString("SwitchAllOff");
            _switchAllOnToolStripMenuItem.Text = LocRm.GetString("SwitchAllOn");
            _takePhotoToolStripMenuItem.Text = LocRm.GetString("TakePhoto");
            _thruWebsiteToolStripMenuItem.Text = LocRm.GetString("Online");
            _toolStripButton1.Text = LocRm.GetString("WebSettings");
            _toolStripButton4.Text = LocRm.GetString("settings");
            _toolStripButton8.Text = LocRm.GetString("Commands");
            _toolStripDropDownButton1.Text = LocRm.GetString("AccessMedia");
            _toolStripDropDownButton2.Text = LocRm.GetString("AddObjects");
            _viewMediaToolStripMenuItem.Text = LocRm.GetString("Viewmedia");
            toolStripToolStripMenuItem.Text = LocRm.GetString("toolStrip");
            menuItem6.Text = LocRm.GetString("toolStrip");
            _tsslStats.Text = LocRm.GetString("Loading");
            _unlockToolstripMenuItem.Text = LocRm.GetString("unlock");
            _viewMediaOnAMobileDeviceToolStripMenuItem.Text = LocRm.GetString("ViewMediaOnAMobiledevice");
            _websiteToolstripMenuItem.Text = LocRm.GetString("website");
            _uSbCamerasAndMicrophonesOnOtherToolStripMenuItem.Text =
                LocRm.GetString("CamerasAndMicrophonesOnOtherComputers");
            fullScreenToolStripMenuItem.Text = LocRm.GetString("Fullscreen");
            menuItem3.Text = LocRm.GetString("Fullscreen");
            alwaysOnTopToolStripMenuItem1.Text = LocRm.GetString("AlwaysOnTop");
            menuItem8.Text = LocRm.GetString("AlwaysOnTop");
           
            menuItem13.Text = LocRm.GetString("PurchaseMoreCameras");
            _exitToolStripMenuItem.Text = LocRm.GetString("Exit");

            layoutToolStripMenuItem.Text = LocRm.GetString("Layout");
            displayToolStripMenuItem.Text = LocRm.GetString("Display");

            mnuSaveLayout.Text = saveLayoutToolStripMenuItem1.Text = LocRm.GetString("SaveLayout");
            mnuResetLayout.Text = resetLayoutToolStripMenuItem1.Text = LocRm.GetString("ResetLayout");
            mediaPaneToolStripMenuItem.Text = LocRm.GetString("ShowMediaPanel");
            menuItem7.Text = LocRm.GetString("ShowMediaPanel");
            iPCameraWithWizardToolStripMenuItem.Text = LocRm.GetString("IPCameraWithWizard");
            tsbPlugins.Text = LocRm.GetString("Plugins");

            menuItem14.Text = viewControllerToolStripMenuItem.Text = LocRm.GetString("ViewController");
            menuItem28.Text = LocRm.GetString("RemoveAllObjects");
            menuItem40.Text = LocRm.GetString("Find");
            

            LocRm.SetString(menuItem15, "ArrangeMedia");
            LocRm.SetString(menuItem22, "LockLayout");
            LocRm.SetString(menuItem16, "Bottom");
            LocRm.SetString(menuItem17, "Left");
            LocRm.SetString(menuItem19, "Right");
            LocRm.SetString(menuItem18, "PTZController");
            LocRm.SetString(tsslPerformance, "PerfTips");
            tssbGridViews.Text = LocRm.GetString("GridViews");
            manageToolStripMenuItem.Text = LocRm.GetString("Manage");
            menuItem25.Text = LocRm.GetString("DefaultDeviceManager");
            LocRm.SetString(menuItem37,"ChangeUser");

            _toolStripDropDownButton1.Visible = menuItem7.Visible = mediaPaneToolStripMenuItem.Visible = (Helper.HasFeature(Enums.Features.Access_Media));
            
            _toolStripButton8.Visible =
                _remoteCommandsToolStripMenuItem.Visible =
                    _menuItem35.Visible = (Helper.HasFeature(Enums.Features.Remote_Commands));
            _toolStripButton1.Visible =
                _viewMediaToolStripMenuItem.Visible =
                    _menuItem3.Visible =
                        _viewMediaOnAMobileDeviceToolStripMenuItem.Visible =
                            _menuItem25.Visible = _menuItem4.Visible = (Helper.HasFeature(Enums.Features.Web_Settings));
            menuItem18.Visible = (Helper.HasFeature(Enums.Features.PTZ));
            tsbPlugins.Visible = (Helper.HasFeature(Enums.Features.Plugins));
            _localCameraToolStripMenuItem.Visible = (Helper.HasFeature(Enums.Features.Source_Local));
            _iPCameraToolStripMenuItem.Visible =
                iPCameraWithWizardToolStripMenuItem.Visible =
                    _addCameraToolStripMenuItem.Visible = (Helper.HasFeature(Enums.Features.IPCameras));
            _floorPlanToolStripMenuItem.Visible =
                _addFloorPlanToolStripMenuItem.Visible = (Helper.HasFeature(Enums.Features.Floorplans));
            videoFileToolStripMenuItem.Visible = (Helper.HasFeature(Enums.Features.Source_VLC)) ||
                                                 (Helper.HasFeature(Enums.Features.Source_FFmpeg));
            otherVideoSourceToolStripMenuItem.Visible = (Helper.HasFeature(Enums.Features.Source_Custom));
            _microphoneToolStripMenuItem.Visible =
                _addMicrophoneToolStripMenuItem.Visible = (Helper.HasFeature(Enums.Features.Microphones));
            _uSbCamerasAndMicrophonesOnOtherToolStripMenuItem.Visible = (Helper.HasFeature(Enums.Features.Source_JPEG)) ||
                                                                        (Helper.HasFeature(Enums.Features.Source_MJPEG));
            oNVIFCameraToolStripMenuItem.Visible = (Helper.HasFeature(Enums.Features.Source_ONVIF));

            splitContainer2.Panel2Collapsed = !Helper.HasFeature(Enums.Features.Remote_Commands);

            tssbGridViews.Visible = menuItem31.Visible = Helper.HasFeature(Enums.Features.Grid_Views);
            _toolStripButton4.Visible = Helper.HasFeature(Enums.Features.Settings);
            menuItem2.Visible = _menuItem20.Visible = _menuItem22.Visible = Helper.HasFeature(Enums.Features.Logs);

            _menuItem17.Visible = _menuItem18.Visible = menuItem17.Visible =
                menuItem23.Visible =
                    menuItem25.Visible = menuItem18.Visible = menuItem7.Visible = Helper.HasFeature(Enums.Features.Access_Media);

            _toolStripDropDownButton2.Visible = _editToolStripMenuItem.Visible = _menuItem36.Visible = _menuItem31.Visible = Helper.HasFeature(Enums.Features.Edit);
            

            _fileItem.Visible = menuItem5.Visible = fileMenuToolStripMenuItem.Visible = Helper.HasFeature(Enums.Features.View_File_Menu);
            _menuItem2.Visible = _menuItem24.Visible = _menuItem10.Visible = _menuItem38.Visible = _menuItem5.Visible = _menuItem27.Visible = _menuItem26.Visible = _menuItem30.Visible = Helper.HasFeature(Enums.Features.View_Ispy_Links);
            if (!Helper.HasFeature(Enums.Features.Access_Media))
                splitContainer1.Panel2Collapsed = true;

            menuItem13.Visible =
                _menuItem26.Visible =
                    tsslPerformance.Visible = tsslPRO.Visible = Helper.HasFeature(Enums.Features.View_Ispy_Links);

            statusStrip1.Visible = Conf.ShowStatus && Helper.HasFeature(Enums.Features.View_Status_Bar);
            menuItem38.Visible = menuItem15.Visible = Helper.HasFeature(Enums.Features.View_Layout_Options);
            menuItem4.Visible = statusBarToolStripMenuItem.Visible = Helper.HasFeature(Enums.Features.View_Status_Bar);

            menuItem28.Visible = Helper.HasFeature(Enums.Features.Edit);

            menuItem31.Text = LocRm.GetString("GridViews");
            menuItem32.Text = LocRm.GetString("Manage");
            menuItem36.Text = LocRm.GetString("ImportObjects");
            tagsToolStripMenuItem.Text = LocRm.GetString("Tags");
            ShowHideMediaPane();
            mediaPanelControl1.RenderResources();
            
        }

        private void HouseKeepingTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _houseKeepingTimer.Stop();
            if (LayoutPanel.NeedsRedraw)
            {
                _pnlCameras.PerformLayout();
                _pnlCameras.Invalidate();
                LayoutPanel.NeedsRedraw = false;
            }
            if (NeedsResourceUpdate)
            {
                RenderResources();
                NeedsResourceUpdate = false;
            }
            if (_cputotalCounter != null)
            {
                try
                {
                    while (_cpuAverages.Count > 4)
                        _cpuAverages.RemoveAt(0);
                    _cpuAverages.Add(_cpuCounter.NextValue()/Environment.ProcessorCount);

                    CpuUsage = _cpuAverages.Sum()/_cpuAverages.Count;
                    CpuTotal = _cputotalCounter.NextValue();
                    _counters = $"CPU: {CpuUsage:0.00}%";

                    if (_pcMem != null)
                    {
                        _counters += " RAM Usage: " + Convert.ToInt32(_pcMem.RawValue/1048576) + "Mb";
                    }
                    tsslMonitor.Text = _counters;
                }
                catch (Exception ex)
                {
                    // _cputotalCounter = null;
                    Logger.LogException(ex);
                }
                if (CpuTotal > _conf.CPUMax)
                {
                    if (ThrottleFramerate > 1)
                        ThrottleFramerate--;
                }
                else
                {
                    if (ThrottleFramerate < 40)
                        ThrottleFramerate++;
                }
            }
            else
            {
                _counters = "Stats Unavailable - See Log File";
            }

            if (_lastOver > DateTime.MinValue)
            {
                if (_lastOver < Helper.Now.AddSeconds(-4))
                {
                    tsslMediaInfo.Text = "";
                    _lastOver = DateTime.MinValue;
                }
            }

            _pingCounter++;

            if (NeedsMediaRefresh > DateTime.MinValue && NeedsMediaRefresh < Helper.Now.AddSeconds(-1))
                LoadPreviews();


            if (Resizing)
            {
                if (_lastResize < Helper.Now.AddSeconds(-1))
                    Resizing = false;
            }

            if (_pingCounter >= 301)
            {
                _pingCounter = 0;
                //auto save
                try
                {
                    SaveObjects("");
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
                try
                {
                    SaveConfig();
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }
            try
            {
                if (!MWS.Running)
                {
                    _tsslStats.Text = "Server Error - see log file";
                    if (MWS.NumErr >= 5)
                    {
                        Logger.LogMessage("Server not running - restarting");
                        StopAndStartServer();
                    }
                }
                else
                {
                    if (WsWrapper.WebsiteLive)
                    {
                        if (Conf.ServicesEnabled && !WsWrapper.LoginFailed)
                        {
                            _tsslStats.Text = LocRm.GetString("Online");
                            if (LoopBack && Conf.Subscribed)
                                _tsslStats.Text += $" ({LocRm.GetString("loopback")})";
                            else
                            {
                                if (!Conf.Subscribed)
                                    _tsslStats.Text += $" ({LocRm.GetString("LANonlynotsubscribed")})";
                                else
                                    _tsslStats.Text += $" ({LocRm.GetString("LANonlyNoLoopback")})";
                            }
                        }
                        else
                        {
                            _tsslStats.Text = LocRm.GetString("Offline");
                        }
                    }
                    else
                    {
                        _tsslStats.Text = LocRm.GetString("Offline");
                    }
                }

                if (Conf.ServicesEnabled && !WsWrapper.LoginFailed)
                {
                    if (NeedsSync)
                    {
                        WsWrapper.ForceSync();
                    }
                    WsWrapper.PingServer();
                }


                _storageCounter++;
                if (_storageCounter == 3600) // every hour
                {
                    RunStorageManagement();
                    _storageCounter = 0;
                }


                if (_pingCounter == 80)
                {
                    var t = new Thread(SaveFileData) {IsBackground = true, Name = "Saving File Data"};
                    t.Start();
                }

                if (_needsDelete)
                {
                    _needsDelete = false;
                    try
                    {
                        if (_tDelete == null || _tDelete.Join(TimeSpan.Zero))
                        {
                            _tDelete = new Thread(DeleteFiles) {IsBackground = true};
                            _tDelete.Start();
                        }
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            Logger.WriteLogs();
            if (!_shuttingDown)
                _houseKeepingTimer.Start();
        }


        public void RunStorageManagement(bool abortIfRunning = false)
        {
            if (InvokeRequired)
            {
                Invoke(new Delegates.RunStorageManagementDelegate(RunStorageManagement), abortIfRunning);
                return;
            }


            if (StorageThreadRunning)
            {

                if (abortIfRunning)
                {
                    try
                    {
                        _storageThread.Abort();
                    }
                    catch
                    {
                        //may have exited
                    }
                }
            }
            if (!StorageThreadRunning)
            {
                lock (ThreadLock)
                {
                    bool r = Conf.MediaDirectories.Any(p => p.Enable_Storage_Management);
                    r = r || Cameras.Any(p => p.settings.storagemanagement.enabled);
                    r = r || Microphones.Any(p => p.settings.storagemanagement.enabled);
                    if (r)
                    {
                        Logger.LogMessage("Running Storage Management");
                        _storageThread = new Thread(DeleteOldFiles) {IsBackground = true};
                        _storageThread.Start();
                    }
                }
            }
            else
                Logger.LogMessage("Storage Management is already running");
        }

        private void UpdateTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _updateTimer.Stop();

            foreach (Control c in _pnlCameras.Controls)
            {
                try
                {
                    var cameraWindow = c as CameraWindow;
                    if (cameraWindow != null)
                    {
                        cameraWindow.Tick();
                        continue;
                    }
                    var volumeLevel = c as VolumeLevel;
                    if (volumeLevel != null)
                    {
                        volumeLevel.Tick();
                        continue;
                    }
                    var floorPlanControl = c as FloorPlanControl;
                    if (floorPlanControl != null)
                    {
                        FloorPlanControl fpc = floorPlanControl;
                        if (fpc.Fpobject.needsupdate)
                        {
                            fpc.NeedsRefresh = true;
                            fpc.Fpobject.needsupdate = false;
                        }
                        fpc.Tick();
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }
            if (!_shuttingDown)
                _updateTimer.Start();
        }

        private void FswChanged(object sender, FileSystemEventArgs e)
        {
            _fsw.EnableRaisingEvents = false;
            bool err = true;
            int i = 0;
            try
            {
                string txt = "";
                while (err && i < 5)
                {
                    try
                    {
                        using (var fs = new FileStream(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            using (var sr = new StreamReader(fs))
                            {
                                while (sr.EndOfStream == false)
                                {
                                    txt = sr.ReadLine();
                                    err = false;
                                }
                                sr.Close();
                            }
                            fs.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex);
                        i++;
                        Thread.Sleep(500);
                    }
                }
                if (!string.IsNullOrEmpty(txt))
                    ParseCommand(txt.Trim());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            _fsw.EnableRaisingEvents = true;
        }

        private void ParseCommand(string command)
        {
            if (command == null) throw new ArgumentNullException("command");
            try
            {
                command = Uri.UnescapeDataString(command);

                if (command.ToLower().StartsWith("open "))
                {
                    Logger.LogMessage("Loading List: " + command);
                    if (InvokeRequired)
                        Invoke(new Delegates.ExternalCommandDelegate(LoadObjectList), command.Substring(5).Trim('"'));
                    else
                        LoadObjectList(command.Substring(5).Trim('"'));

                    return;
                }
                ProcessCommandString(command);

                if (command.ToLower() == "showform")
                {
                    UISync.Execute(ShowIfUnlocked);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                MessageBox.Show(LocRm.GetString("LoadFailed").Replace("[MESSAGE]", ex.Message));
            }
        }

        internal void ProcessCommandString(string command)
        {
            int i = command.ToLower().IndexOf("commands ", StringComparison.Ordinal);
            if (i != -1)
            {
                command = command.Substring(i + 9);
                string[] commands = command.Trim('"').Split('|');
                foreach (string command2 in commands)
                {
                    if (!string.IsNullOrEmpty(command2))
                    {
                        Logger.LogMessage("Running Command: " + command2);
                        if (InvokeRequired)
                            Invoke(new Delegates.ExternalCommandDelegate(ProcessCommandInternal), command2.Trim('"'));
                        else
                            ProcessCommandInternal(command2.Trim('"'));
                    }
                }
            }
        }

        internal static void ProcessCommandInternal(string command)
        {
            //parse command into new format
            string[] cfg = command.Split(',');
            string newcommand;
            switch (cfg.Length)
            {
                default:
                    //generic command
                    newcommand = cfg[0];
                    break;
                case 2:
                    //group command
                    newcommand = cfg[0] + "?group=" + cfg[1];
                    break;
                case 3:
                    //specific device
                    newcommand = cfg[0] + "?ot=" + cfg[1] + "&oid=" + cfg[2];
                    break;
            }
            MWS.ProcessCommandInternal(newcommand);
        }

        public void SetBackground()
        {
            _pnlCameras.BackColor = Conf.MainColor.ToColor();
            _pnlContent.BackColor = SystemColors.AppWorkspace;
        }

        protected override void OnClosed(EventArgs e)
        {
            try
            {
                notifyIcon1.Visible = false;

                notifyIcon1.Icon.Dispose();
                notifyIcon1.Dispose();
            }
            catch
            {
                // ignored
            }
            base.OnClosed(e);
        }

        private void MenuItem2Click(object sender, EventArgs e)
        {
            StartBrowser(Website + "/userguide.aspx");
        }

        internal static string StopAndStartServer()
        {
            string message = "";
            try
            {
                MWS.StopServer();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            Application.DoEvents();
            try
            {
                message = MWS.StartServer();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return message;
        }

        private void MenuItem4Click(object sender, EventArgs e)
        {
            WebConnect();
        }

        private void MenuItem5Click(object sender, EventArgs e)
        {
            StartBrowser(Website + "/");
        }

        private void MenuItem10Click(object sender, EventArgs e)
        {
            CheckForUpdates(false);
        }

        private void CheckForUpdates(bool suppressMessages)
        {
            try
            {
                if (_updateChecker != null && !_updateChecker.Join(TimeSpan.Zero))
                    return;
            }
            catch
            {
                return;
            }

            _updateChecker = new Thread(() => DoUpdateCheck(suppressMessages));
            _updateChecker.Start();
        }

        private void DoUpdateCheck(bool suppressMessages)
        {
            string version = "";
            try
            {
                version = WsWrapper.ProductLatestVersion(ProductID);
                if (version == LocRm.GetString("iSpyDown"))
                {
                    throw new Exception("down");
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                if (!suppressMessages)
                {
                    UISync.Execute(() => MessageBox.Show(LocRm.GetString("CheckUpdateError"), LocRm.GetString("Error")));
                }
            }
            if (version != "" && version != LocRm.GetString("iSpyDown"))
            {
                var verThis = new Version(Application.ProductVersion);
                var verLatest = new Version(version);
                if (verThis < verLatest)
                {
                    UISync.Execute(ShowNewVersion);
                }
                else
                {
                    if (!suppressMessages)
                        UISync.Execute(
                            () =>
                                MessageBox.Show(LocRm.GetString("HaveLatest"), LocRm.GetString("Note"),
                                    MessageBoxButtons.OK));
                }
            }
        }

        private void ShowNewVersion()
        {
            using (var nv = new NewVersion())
            {
                nv.ShowDialog(this);
            }
        }

        private void MenuItem8Click(object sender, EventArgs e)
        {
            ShowSettings(0);
        }

        public void ShowSettings(int tabindex, IWin32Window owner = null)
        {
            int pi = Conf.PreviewItems;
            var settings = new Settings { MainClass = this, InitialTab = tabindex };
            if (settings.ShowDialog(owner ?? this) == DialogResult.OK)
            {
                if (pi != Conf.PreviewItems)
                    NeedsMediaRefresh = Helper.Now;

                _pnlCameras.BackColor = Conf.MainColor.ToColor();
                notifyIcon1.Text = Conf.TrayIconText;

                if (!string.IsNullOrEmpty(Conf.Joystick.id))
                {
                    if (_jst == null)
                    {
                        _jst = new JoystickDevice();
                    }
                    _jst.ReleaseJoystick();
                    if (_tmrJoystick != null)
                    {
                        _tmrJoystick.Stop();
                        _tmrJoystick = null;
                    }

                    bool jsactive = false;
                    string[] sticks = _jst.FindJoysticks();
                    foreach (string js in sticks)
                    {
                        string[] nameid = js.Split('|');
                        if (nameid[1] == Conf.Joystick.id)
                        {
                            Guid g = Guid.Parse(nameid[1]);
                            jsactive = _jst.AcquireJoystick(g);
                        }
                    }

                    if (!jsactive)
                    {
                        _jst.ReleaseJoystick();
                        _jst = null;
                    }
                    else
                    {
                        _tmrJoystick = new Timer(100);
                        _tmrJoystick.Elapsed += TmrJoystickElapsed;
                        _tmrJoystick.Start();
                    }
                }
                else
                {
                    if (_tmrJoystick != null)
                    {
                        _tmrJoystick.Stop();
                        _tmrJoystick = null;
                    }

                    if (_jst != null)
                    {
                        _jst.ReleaseJoystick();
                        _jst = null;
                    }
                }
            }

            if (settings.ReloadResources)
            {
                RenderResources();
                LoadCommands();
            }
            AddressIPv4 = ""; //forces reload
            AddressIPv6 = "";
            settings.Dispose();
            SaveConfig();
            Refresh();
        }

        private void MenuItem11Click(object sender, EventArgs e)
        {
            
        }

        private void MainFormResize(object sender, EventArgs e)
        {
            Resizing = true;
            if (WindowState == FormWindowState.Minimized)
            {
                if (Conf.TrayOnMinimise)
                {
                    Hide();
                    if (Conf.BalloonTips)
                    {
                        if (Conf.BalloonTips)
                        {
                            notifyIcon1.BalloonTipText = LocRm.GetString("RunningInTaskBar");
                            notifyIcon1.ShowBalloonTip(1500);
                        }
                    }
                }
            }
            else
            {
                _previousWindowState = WindowState;
                if (Conf.AutoLayout)
                    _pnlCameras.LayoutObjects(0, 0);
                else
                {
                    _pnlCameras.AutoGrid();
                }
                if (!IsOnScreen(this))
                {
                    Left = 0;
                    Top = 0;
                }
            }
        }

        private void NotifyIcon1DoubleClick(object sender, EventArgs e)
        {
            ShowIfUnlocked();
        }

        private CheckPassword _cp;
        private bool _locked;

        public void ShowIfUnlocked()
        {
            if (Visible == false || WindowState == FormWindowState.Minimized)
            {
                if (Conf.Enable_Password_Protect || _locked)
                {
                    if (_cp == null)
                    {
                        using (_cp = new CheckPassword())
                        {
                            _cp.ShowDialog(this);
                            if (_cp.DialogResult == DialogResult.OK)
                            {
                                _locked = false;
                                ShowForm(-1);
                            }
                        }
                        _cp = null;
                    }
                }
                else
                {
                    ShowForm(-1);
                }
            }
            else
            {
                ShowForm(-1);
            }
        }

        private void MainFormFormClosing1(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.WindowsShutDown)
            {
                if (Conf.MinimiseOnClose && !ShuttingDown)
                {
                    e.Cancel = true;
                    WindowState = FormWindowState.Minimized;
                    return;
                }
            }
            ShuttingDown = true;
            if (_mIcp != null && _mCookie != -1)
            {
                try
                {
                    _mIcp.Unadvise(_mCookie);
                }
                catch
                {
                    // ignored
                }
            }
            Exit();
        }

        private void Exit()
        {
            try
            {
                SaveObjects("");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            _shuttingDown = true;
            WsWrapper.Disconnect();
            try
            {
                MWS.StopServer();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            

            ThreadKillDelay = 3000;

            _houseKeepingTimer?.Stop();
            _updateTimer?.Stop();
            _tmrJoystick?.Stop();


            if (Conf.ShowMediaPanel)
                Conf.MediaPanelSize = splitContainer1.SplitterDistance + "x" + splitContainer2.SplitterDistance;

            if (Conf.BalloonTips)
            {
                if (Conf.BalloonTips)
                {
                    notifyIcon1.BalloonTipText = LocRm.GetString("ShuttingDown");
                    notifyIcon1.ShowBalloonTip(1500);
                }
            }
            _closing = true;
            

            try
            {
                SaveConfig();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            try
            {
                if (_talkSource != null)
                {
                    _talkSource.Stop();
                    _talkSource = null;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            try
            {
                if (_talkTarget != null)
                {
                    _talkTarget.Stop();
                    _talkTarget = null;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            try
            {
                RemoveObjects();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            
            try
            {
                File.WriteAllText(Program.AppDataPath + "exit.txt", "OK");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            if (StorageThreadRunning)
            {
                try
                {
                    _storageThread.Join(ThreadKillDelay);
                }
                catch
                {
                }
            }
            Logger.WriteLogs();
        }


        private void ControlNotification(object sender, NotificationType e)
        {
            if (Conf.BalloonTips)
            {
                notifyIcon1.BalloonTipText =
                    $"{(string.IsNullOrEmpty(e.OverrideMessage) ? LocRm.GetString(e.Type) : e.OverrideMessage).ToUpper()}:{NL}{e.Text}";
                notifyIcon1.ShowBalloonTip(1500);
            }
        }


        private void NotifyIcon1BalloonTipClicked(object sender, EventArgs e)
        {
            ShowIfUnlocked();
        }


        public static string RandomString(int length)
        {
            string b = "";

            for (int i = 0; i < length; i++)
            {
                char ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26*Random.NextDouble() + 65)));
                b += ch;
            }
            return b;
        }

        private void SetNewStartPosition()
        {
            if (LayoutMode == Enums.LayoutMode.AutoGrid)
            {
                _pnlCameras.LayoutControlsInGrid();
                _pnlCameras.AutoGrid();
                return;
            }
            if (Conf.AutoLayout)
                _pnlCameras.LayoutObjects(0, 0);
        }

        private void VolumeControlRemoteCommand(object sender, ThreadSafeCommand e)
        {
            Delegates.InvokeMethod i = DoInvoke;
            Invoke(i, e.Command);
        }

        internal void ConnectServices(bool checkforUpdates = true)
        {
            if (Conf.ServicesEnabled)
            {
                if (Conf.UseUPNP)
                {
                    NATControl.SetPorts(Conf.ServerPort, Conf.LANPort);
                }

                string[] result =
                    WsWrapper.TestConnection(Conf.WSUsername, Conf.WSPassword, Conf.Loopback);

                if (result.Length > 0 && result[0] == "OK")
                {
                    WsWrapper.Connect();
                    NeedsSync = true;
                    EmailAddress = result[2];
                    MobileNumber = result[4];
                    Conf.Reseller = result[5];

                    Conf.ServicesEnabled = true;
                    Conf.Subscribed = (Convert.ToBoolean(result[1]));

                    Helper.SetTitle(this);

                    if (result[3] == "")
                    {
                        LoopBack = Conf.Loopback;
                        WsWrapper.Connect(Conf.Loopback);
                    }
                    else
                    {
                        LoopBack = false;
                    }
                }
            }
            if (checkforUpdates && Conf.Enable_Update_Check && !SilentStartup)
            {
                UISync.Execute(() => CheckForUpdates(true));
            }
            SilentStartup = false;
        }


        private void EditToolStripMenuItemClick(object sender, EventArgs e)
        {
            var window = ContextTarget as CameraWindow;
            if (window != null)
            {
                EditCamera(window.Camobject);
            }
            var level = ContextTarget as VolumeLevel;
            if (level != null)
            {
                EditMicrophone(level.Micobject);
            }
            var target = ContextTarget as FloorPlanControl;
            if (target != null)
            {
                EditFloorplan(target.Fpobject);
            }
        }

        private void DeleteToolStripMenuItemClick(object sender, EventArgs e)
        {
            var window = ContextTarget as CameraWindow;
            if (window != null)
            {
                RemoveCamera(window, true);
                return;
            }
            var level = ContextTarget as VolumeLevel;
            if (level != null)
            {
                RemoveMicrophone(level, true);
                return;
            }
            var fpc = ContextTarget as FloorPlanControl;
            if (fpc != null)
            {
                RemoveFloorplan(fpc, true);
            }
        }


        private void ToolStripButton4Click(object sender, EventArgs e)
        {
            ShowSettings(0);
        }

        public static void GoSubscribe()
        {
            OpenUrl(Website + "/subscribe.aspx");
        }

        public static void OpenUrl(string url)
        {
            try
            {
                var p = new Process {StartInfo = {FileName = DefaultBrowser, Arguments = "\""+url+"\""}};
                p.Start();
            }
            catch (Exception ex2)
            {
                Logger.LogException(ex2);
            }
           
        }

        private void ActivateToolStripMenuItemClick(object sender, EventArgs e)
        {
            
        }

        private void WebsiteToolstripMenuItemClick(object sender, EventArgs e)
        {
            StartBrowser(Website + "/");
        }

        private void HelpToolstripMenuItemClick(object sender, EventArgs e)
        {
            StartBrowser(Website + "/userguide.aspx");
        }

        private void ShowToolstripMenuItemClick(object sender, EventArgs e)
        {
            ShowForm(-1);
        }

        public void ShowForm(double opacity)
        {
            Activate();
            Visible = true;
            if (WindowState == FormWindowState.Minimized)
            {
                Show();
                WindowState = _previousWindowState;
            }
            if (opacity > -1)
                Opacity = opacity;

            //Process currentProcess = Process.GetCurrentProcess();
            //IntPtr hWnd = currentProcess.MainWindowHandle;
            //if (hWnd != IntPtr.Zero)
            //{
            //    SetForegroundWindow(hWnd);
            //}
            TopMost = Conf.AlwaysOnTop;
        }

        private void UnlockToolstripMenuItemClick(object sender, EventArgs e)
        {
            ShowIfUnlocked();
        }

        private void NotifyIcon1Click(object sender, EventArgs e)
        {
        }

        private void AddCameraToolStripMenuItemClick(object sender, EventArgs e)
        {
            AddCamera(3);
        }

        private void AddMicrophoneToolStripMenuItemClick(object sender, EventArgs e)
        {
            AddMicrophone(0);
        }

        private void CtxtMainFormOpening(object sender, CancelEventArgs e)
        {
            if (ctxtMnu.Visible || ctxtPlayer.Visible)
                e.Cancel = true;

            gridViewsToolStripMenuItem.DropDownItems.Clear();
            foreach (var gv in Conf.GridViews)
            {
                gridViewsToolStripMenuItem.DropDownItems.Add(gv.name, null, tsi_Click);
            }
            maximiseToolStripMenuItem.DropDownItems.Clear();
            foreach (Control o in _pnlCameras.Controls)
            {
                var ic = o as ISpyControl;
                if (ic != null)
                {
                    maximiseToolStripMenuItem.DropDownItems.Add(ic.ObjectName, null, tsi_MaximiseClick);
                }

            }
            _addCameraToolStripMenuItem.Visible =
                _addFloorPlanToolStripMenuItem.Visible =
                    _addMicrophoneToolStripMenuItem.Visible = Helper.HasFeature(Enums.Features.Edit);
        }

        public static void StartBrowser(string url)
        {
            if (url != "")
                OpenUrl(url);
        }

        private void ExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            ShuttingDown = true;
            Close();
        }

        private void MenuItem3Click(object sender, EventArgs e)
        {
            Connect(false);
        }

        private void MenuItem18Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show(LocRm.GetString("AreYouSure"), LocRm.GetString("Confirm"), MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning) == DialogResult.Cancel)
                return;

            foreach (configurationDirectory d in Conf.MediaDirectories)
            {
                string loc = d.Entry + "audio\\";

                if (Directory.Exists(loc))
                {
                    string[] files = Directory.GetFiles(loc, "*.*", SearchOption.AllDirectories);
                    foreach (string t in files)
                    {
                        try
                        {
                            FileOperations.Delete(t);
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                }
                loc = d.Entry + "video\\";
                if (Directory.Exists(loc))
                {
                    string[] files = Directory.GetFiles(loc, "*.*", SearchOption.AllDirectories);
                    foreach (string t in files)
                    {
                        try
                        {
                            FileOperations.Delete(t);
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                }
            }
            foreach (objectsCamera oc in Cameras)
            {
                CameraWindow occ = GetCameraWindow(oc.id);
                occ?.ClearFileList();
            }
            foreach (objectsMicrophone om in Microphones)
            {
                VolumeLevel omc = GetVolumeLevel(om.id);
                omc?.ClearFileList();
            }
            LoadPreviews();
            MessageBox.Show(LocRm.GetString("FilesDeleted"), LocRm.GetString("Note"));
        }

        private void MenuItem20Click(object sender, EventArgs e)
        {
            ShowLogFile();
        }

        private void ShowLogFile()
        {
            Process.Start(Program.AppDataPath + "log_" + Logger.NextLog + ".htm");
        }

        private void ResetSizeToolStripMenuItemClick(object sender, EventArgs e)
        {
            _pnlCameras.Minimize(ContextTarget, true);
        }

        private void SettingsToolStripMenuItemClick(object sender, EventArgs e)
        {
            ShowSettings(0);
        }


        private void MenuItem19Click(object sender, EventArgs e)
        {
            SaveObjectList(true);
        }

        public void SaveObjectList(bool warn = true)
        {
            if (Cameras.Count == 0 && Microphones.Count == 0)
            {
                if (warn)
                    MessageBox.Show(LocRm.GetString("NothingToExport"), LocRm.GetString("Error"));
                return;
            }

            bool save = true;
            string filename = _currentFileName;
            if (warn)
            {

                using (var saveFileDialog = new SaveFileDialog
                                            {
                                                InitialDirectory = _lastPath,
                                                Filter = "iSpy Files (*.ispy)|*.ispy|XML Files (*.xml)|*.xml"
                                            })
                {

                    save = saveFileDialog.ShowDialog(this) == DialogResult.OK;
                    filename = saveFileDialog.FileName;
                }
            }
            if (save)
            {
                if (filename.Trim() != "")
                {
                    SaveObjects(filename);
                    try
                    {
                        var fi = new FileInfo(filename);
                        _lastPath = fi.DirectoryName;
                    }
                    catch
                    {
                    }
                }
            }
        }


        private void MenuItem21Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = _lastPath;
                ofd.Filter = "iSpy Files (*.ispy)|*.ispy|XML Files (*.xml)|*.xml";
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    string fileName = ofd.FileName;
                    try
                    {
                        var fi = new FileInfo(fileName);
                        _lastPath = fi.DirectoryName;
                    }
                    catch
                    {
                    }


                    if (fileName.Trim() != "")
                    {
                        LoadObjectList(fileName.Trim());
                    }
                }
            }
        }

        private void ToolStripMenuItem1Click(object sender, EventArgs e)
        {
            if (ContextTarget is CameraWindow)
            {
                //id = ((CameraWindow) ContextTarget).Camobject.id.ToString();
                string url = Webpage;
                if (WsWrapper.WebsiteLive && Conf.ServicesEnabled)
                {
                    OpenUrl(url);
                }
                else
                    Connect(url, false);
            }

            if (ContextTarget is VolumeLevel)
            {
                //id = ((VolumeLevel) ContextTarget).Micobject.id.ToString();
                string url = Webpage;
                if (WsWrapper.WebsiteLive && Conf.ServicesEnabled)
                {
                    OpenUrl(url);
                }
                else
                    Connect(url, false);
            }

            if (ContextTarget is FloorPlanControl)
            {
                string url = Webpage;
                if (WsWrapper.WebsiteLive && Conf.ServicesEnabled)
                {
                    OpenUrl(url);
                }
                else
                    Connect(url, false);
            }
        }

        public void Connect(bool silent)
        {
            Connect(Webpage, silent);
        }

        public void Connect(string successUrl, bool silent)
        {
            if (!MWS.Running)
            {
                string message = StopAndStartServer();
                if (message != "")
                {
                    if (!silent)
                        MessageBox.Show(this, message);
                    return;
                }
            }
            if (WsWrapper.WebsiteLive)
            {
                if (Conf.WSUsername != null && Conf.WSUsername.Trim() != "")
                {
                    if (Conf.UseUPNP)
                    {
                        NATControl.SetPorts(Conf.ServerPort, Conf.LANPort);
                    }
                    WsWrapper.Connect();
                    WsWrapper.ForceSync();
                    if (WsWrapper.WebsiteLive)
                    {
                        if (successUrl != "")
                            StartBrowser(successUrl);
                        return;
                    }
                    if (!silent && !_shuttingDown)
                        Logger.LogMessage(LocRm.GetString("WebsiteDown"));
                    return;
                }
                var ws = new Webservices();
                ws.ShowDialog(this);
                if (!string.IsNullOrEmpty(ws.EmailAddress))
                    EmailAddress = ws.EmailAddress;
                if (ws.DialogResult == DialogResult.Yes || ws.DialogResult == DialogResult.No)
                {
                    ws.Dispose();
                    Connect(successUrl, silent);
                    return;
                }
                ws.Dispose();
            }
            else
            {
                Logger.LogMessage(LocRm.GetString("WebsiteDown"));
            }
        }

        private void MenuItem7Click(object sender, EventArgs e)
        {
            foreach (configurationDirectory s in Conf.MediaDirectories)
            {
                string foldername = s.Entry + "video\\";
                if (!foldername.EndsWith(@"\"))
                    foldername += @"\";
                Process.Start(foldername);
            }
        }

        private void MenuItem23Click(object sender, EventArgs e)
        {
            foreach (configurationDirectory s in Conf.MediaDirectories)
            {
                string foldername = s.Entry + "audio\\";
                if (!foldername.EndsWith(@"\"))
                    foldername += @"\";
                Process.Start(foldername);
            }
        }

        private void MenuItem25Click(object sender, EventArgs e)
        {
            ViewMobile();
        }


        private void MainFormHelpButtonClicked(object sender, CancelEventArgs e)
        {
            OpenUrl(Website + "/userguide.aspx");
        }

        private void menuItem21_Click(object sender, EventArgs e)
        {
            _pnlCameras.LayoutOptimised();
        }

        private void ShowISpy10PercentOpacityToolStripMenuItemClick(object sender, EventArgs e)
        {
            ShowForm(.1);
        }

        private void ShowISpy30OpacityToolStripMenuItemClick(object sender, EventArgs e)
        {
            ShowForm(.3);
        }

        private void ShowISpy100PercentOpacityToolStripMenuItemClick(object sender, EventArgs e)
        {
            ShowForm(1);
        }

        private void CtxtTaskbarOpening(object sender, CancelEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                if (Conf.Enable_Password_Protect)
                {
                    _unlockToolstripMenuItem.Visible = true;
                    _showToolstripMenuItem.Visible =
                        _showISpy10PercentOpacityToolStripMenuItem.Visible =
                            _showISpy30OpacityToolStripMenuItem.Visible =
                                _showISpy100PercentOpacityToolStripMenuItem.Visible = false;
                    _exitToolStripMenuItem.Visible = false;
                    _websiteToolstripMenuItem.Visible = false;
                    _helpToolstripMenuItem.Visible = false;
                    _switchAllOffToolStripMenuItem.Visible = false;
                    _switchAllOnToolStripMenuItem.Visible = false;
                    viewLogFileToolStripMenuItem.Visible = false;
                    gridViewsToolStripMenuItem1.Visible = false;
                }
                else
                {
                    _unlockToolstripMenuItem.Visible = false;
                    _showToolstripMenuItem.Visible =
                        _showISpy10PercentOpacityToolStripMenuItem.Visible =
                            _showISpy30OpacityToolStripMenuItem.Visible =
                                _showISpy100PercentOpacityToolStripMenuItem.Visible = true;
                    _exitToolStripMenuItem.Visible = true;
                    _websiteToolstripMenuItem.Visible = true;
                    _helpToolstripMenuItem.Visible = true;
                    _switchAllOffToolStripMenuItem.Visible = true;
                    _switchAllOnToolStripMenuItem.Visible = true;
                    gridViewsToolStripMenuItem1.Visible = true;
                    viewLogFileToolStripMenuItem.Visible = true;
                }
            }
            else
            {
                _showToolstripMenuItem.Visible = false;
                _showISpy10PercentOpacityToolStripMenuItem.Visible =
                    _showISpy30OpacityToolStripMenuItem.Visible =
                        _showISpy100PercentOpacityToolStripMenuItem.Visible = true;
                _unlockToolstripMenuItem.Visible = false;
                _exitToolStripMenuItem.Visible = true;
                _websiteToolstripMenuItem.Visible = true;
                _helpToolstripMenuItem.Visible = true;
                _switchAllOffToolStripMenuItem.Visible = true;
                _switchAllOnToolStripMenuItem.Visible = true;
            }

            gridViewsToolStripMenuItem1.DropDownItems.Clear();
            foreach (var gv in Conf.GridViews)
            {
                gridViewsToolStripMenuItem1.DropDownItems.Add(gv.name, null, tsi_Click);
            }
        }


        private void MenuItem26Click(object sender, EventArgs e)
        {
            OpenUrl(Website + "/donate.aspx");
        }

        private void RecordNowToolStripMenuItemClick(object sender, EventArgs e)
        {
            var window = ContextTarget as CameraWindow;
            if (window != null)
            {
                var cameraControl = window;
                cameraControl.RecordSwitch(!cameraControl.Recording);
            }

            var level = ContextTarget as VolumeLevel;
            if (level != null)
            {
                var volumeControl = level;
                volumeControl.RecordSwitch(!volumeControl.Recording);
            }
        }

        private void ShowFilesToolStripMenuItemClick(object sender, EventArgs e)
        {
            var cw = ContextTarget as CameraWindow;
            if (cw != null)
            {
                ShowFiles(cw);   
                return;
            }

            var vl = ContextTarget as VolumeLevel;
            if (vl != null)
            {
                ShowFiles(vl);
                return;
            }
            foreach (configurationDirectory s in Conf.MediaDirectories)
            {
                Process.Start(s.Entry);
            }
        }

        internal void ShowFiles(ISpyControl ctrl)
        {
            var cw = ctrl as CameraWindow;
            if (cw != null)
            {
                ShowFiles(cw);
                return;
            }
            var vl = ctrl as VolumeLevel;
            if (vl != null)
            {
                ShowFiles(vl);
                return;
            }
        }
        
        internal void ShowFiles(CameraWindow cw)
        {
            string foldername = Helper.GetMediaDirectory(2, cw.Camobject.id) + "video\\" + cw.Camobject.directory + "\\";
            if (!foldername.EndsWith(@"\"))
                foldername += @"\";
            Process.Start(foldername);
            cw.Camobject.newrecordingcount = 0;
        }

        internal void ShowFiles(VolumeLevel vl)
        {
            string foldername = Helper.GetMediaDirectory(1, vl.Micobject.id) + "audio\\" + vl.Micobject.directory + "\\";
            if (!foldername.EndsWith(@"\"))
                foldername += @"\";
            Process.Start(foldername);
            vl.Micobject.newrecordingcount = 0;
        }

        private void ViewMediaOnAMobileDeviceToolStripMenuItemClick(object sender, EventArgs e)
        {
            ViewMobile();
        }

        private void ViewMobile()
        {
            if (WsWrapper.WebsiteLive && Conf.ServicesEnabled)
            {
                OpenUrl(Webserver + "/mobile/");
            }
            else
                WebConnect();
        }

        private void AddFloorPlanToolStripMenuItemClick(object sender, EventArgs e)
        {
            AddFloorPlan();
        }

        private void ListenToolStripMenuItemClick(object sender, EventArgs e)
        {
            var level = ContextTarget as VolumeLevel;
            if (level != null)
            {
                var vf = level;
                vf.Listening = !vf.Listening;
            }
        }

        private void MenuItem31Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show(LocRm.GetString("AreYouSure"), LocRm.GetString("Confirm"), MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning) == DialogResult.Cancel)
                return;
            RemoveObjects();
        }

        private void MenuItem34Click(object sender, EventArgs e)
        {
        }


        private void MenuItem33Click(object sender, EventArgs e)
        {
        }

        private void ToolStripButton8Click1(object sender, EventArgs e)
        {
            ShowRemoteCommands();
        }

        private void MenuItem35Click(object sender, EventArgs e)
        {
            ShowRemoteCommands();
        }

        private void ToolStrip1ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        private void RemoteCommandsToolStripMenuItemClick(object sender, EventArgs e)
        {
            ShowRemoteCommands();
        }

        private void MenuItem37Click(object sender, EventArgs e)
        {
            MessageBox.Show(LocRm.GetString("EditInstruct"), LocRm.GetString("Note"));
        }

        private void PositionToolStripMenuItemClick(object sender, EventArgs e)
        {
            var p = (PictureBox) ContextTarget;
            int w = p.Width;
            int h = p.Height;
            int x = p.Location.X;
            int y = p.Location.Y;

            var le = new LayoutEditor {X = x, Y = y, W = w, H = h};


            if (le.ShowDialog(this) == DialogResult.OK)
            {
                _pnlCameras.PositionPanel(p, new Point(le.X, le.Y), le.W, le.H);
            }
            le.Dispose();
        }



        private void MenuItem38Click(object sender, EventArgs e)
        {
            StartBrowser(Website + "/producthistory.aspx?productid=11");
        }

        private void MenuItem39Click(object sender, EventArgs e)
        {
        }

        private void TakePhotoToolStripMenuItemClick(object sender, EventArgs e)
        {
            var window = ContextTarget as CameraWindow;
            if (window != null)
            {
                var cameraControl = window;
                string fn = cameraControl.SaveFrame();
                if (fn != "" && Conf.OpenGrabs)
                    OpenUrl(fn);
                //OpenUrl("http://" + IPAddress + ":" + Conf.LANPort + "/livefeed?oid=" + cameraControl.Camobject.id + "&r=" + Random.NextDouble() + "&full=1&auth=" + Identifier);
            }
        }

        private void ToolStripDropDownButton1Click(object sender, EventArgs e)
        {
        }

        private void ThruWebsiteToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (WsWrapper.WebsiteLive && Conf.ServicesEnabled)
            {
                OpenUrl(Webpage);
            }
            else
                WebConnect();
        }

        private void OnMobileDevicesToolStripMenuItemClick(object sender, EventArgs e)
        {
            ViewMobile();
        }

        private void LocalCameraToolStripMenuItemClick(object sender, EventArgs e)
        {
            AddCamera(3);
        }

        private void IpCameraToolStripMenuItemClick(object sender, EventArgs e)
        {
            AddCamera(1);
        }

        private void MicrophoneToolStripMenuItemClick(object sender, EventArgs e)
        {
            AddMicrophone(0);
        }

        private void FloorPlanToolStripMenuItemClick(object sender, EventArgs e)
        {
            AddFloorPlan();
        }

        private void MenuItem12Click(object sender, EventArgs e)
        {
            //+26 height for control bar
            _pnlCameras.LayoutObjects(164, 146);
        }

        private void MenuItem14Click(object sender, EventArgs e)
        {
            _pnlCameras.LayoutObjects(324, 266);
        }

        private void MenuItem29Click1(object sender, EventArgs e)
        {
            _pnlCameras.LayoutObjects(0, 0);
        }

        private void ToolStripButton1Click1(object sender, EventArgs e)
        {
            WebConnect();
        }

        private void WebConnect()
        {
            var ws = new Webservices();
            ws.ShowDialog(this);
            if (ws.EmailAddress != "")
            {
                EmailAddress = ws.EmailAddress;
                MobileNumber = ws.MobileNumber;
            }
            if (ws.DialogResult == DialogResult.Yes)
            {
                Connect(false);
            }
            ws.Dispose();
            Helper.SetTitle(this);
        }

        private void MenuItem17Click(object sender, EventArgs e)
        {
        }

        private void ResetRecordingCounterToolStripMenuItemClick(object sender, EventArgs e)
        {
            var window = ContextTarget as CameraWindow;
            if (window != null)
            {
                var cw = window;
                cw.Camobject.newrecordingcount = 0;
                cw.Custom = false;
                if (cw.VolumeControl != null)
                {
                    cw.VolumeControl.Micobject.newrecordingcount = 0;
                    cw.VolumeControl.Invalidate();
                }
                cw.Invalidate();
            }
            var level = ContextTarget as VolumeLevel;
            if (level != null)
            {
                var vw = level;
                vw.Micobject.newrecordingcount = 0;
                if (vw.Paired)
                {
                    objectsCamera oc = Cameras.SingleOrDefault(p => p.settings.micpair == vw.Micobject.id);
                    if (oc != null)
                    {
                        CameraWindow cw = GetCameraWindow(oc.id);
                        cw.Camobject.newrecordingcount = 0;
                        cw.Invalidate();
                    }
                }
                vw.Invalidate();
            }
        }

        private void MenuItem15Click(object sender, EventArgs e)
        {
            foreach (Control c in _pnlCameras.Controls)
            {
                var window = c as CameraWindow;
                if (window != null)
                {
                    var cameraControl = window;
                    cameraControl.Camobject.newrecordingcount = 0;
                    cameraControl.Invalidate();
                }
                var level = c as VolumeLevel;
                if (level != null)
                {
                    var volumeControl = level;
                    volumeControl.Micobject.newrecordingcount = 0;
                    volumeControl.Invalidate();
                }
            }
        }

        private void SwitchAllOnToolStripMenuItemClick(object sender, EventArgs e)
        {
            SwitchObjects(false, true);
        }

        private void SwitchAllOffToolStripMenuItemClick(object sender, EventArgs e)
        {
            SwitchObjects(false, false);
        }

        private void MenuItem22Click1(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
                      {
                          InitialDirectory = Program.AppDataPath,
                          Filter = "iSpy Log Files (*.htm)|*.htm|XML Files (*.xml)|*.xml|All Files (*.*)|*.*"
                      };

            if (ofd.ShowDialog(this) != DialogResult.OK) return;
            string fileName = ofd.FileName;

            if (fileName.Trim() != "")
            {
                Process.Start(ofd.FileName);
            }
        }

        private void USbCamerasAndMicrophonesOnOtherToolStripMenuItemClick(object sender, EventArgs e)
        {
            OpenUrl(Website + "/download_ispyserver.aspx");
        }

        private void MenuItem24Click(object sender, EventArgs e)
        {
            SwitchObjects(false, true);
        }

        private void MenuItem40Click(object sender, EventArgs e)
        {
            SwitchObjects(false, false);
        }

        private void MenuItem41Click(object sender, EventArgs e)
        {
            SwitchObjects(true, false);
        }

        private void MenuItem28Click1(object sender, EventArgs e)
        {
            SwitchObjects(true, true);
        }

        private void MenuItem24Click1(object sender, EventArgs e)
        {
            ApplySchedule();
        }

        public void ApplySchedule()
        {
            foreach (objectsCamera cam in _cameras)
            {
                if (cam.schedule.active)
                {
                    CameraWindow cw = GetCamera(cam.id);
                    cw.ApplySchedule();
                }
            }

            foreach (objectsMicrophone mic in _microphones)
            {
                if (mic.schedule.active)
                {
                    VolumeLevel vl = GetVolumeLevel(mic.id);
                    vl.ApplySchedule();
                }
            }
        }

        private void ApplyScheduleToolStripMenuItemClick1(object sender, EventArgs e)
        {
            ApplySchedule();
        }

        private void ApplyScheduleToolStripMenuItem1Click(object sender, EventArgs e)
        {
            var window = ContextTarget as CameraWindow;
            if (window != null)
            {
                var cameraControl = window;
                cameraControl.ApplySchedule();
            }
            var level = ContextTarget as VolumeLevel;
            if (level != null)
            {
                var vf = level;
                vf.ApplySchedule();
            }
        }

        private void MenuItem24Click2(object sender, EventArgs e)
        {
            ShowGettingStarted();
        }

        private void ShowGettingStarted()
        {
            var gs = new GettingStarted();
            gs.Closed += _gs_Closed;
            gs.Show(this);
            gs.Activate();
        }

        private void _gs_Closed(object sender, EventArgs e)
        {
            if (((GettingStarted) sender).LangChanged)
            {
                RenderResources();
                LoadCommands();
                Refresh();
            }
        }

        private void MenuItem28Click2(object sender, EventArgs e)
        {
            _pnlCameras.LayoutObjects(644, 506);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShuttingDown = true;
            Close();
        }


        public void ExternalClose()
        {
            if (InvokeRequired)
            {
                Invoke(new Delegates.CloseDelegate(ExternalClose));
                return;
            }
            ShuttingDown = true;
            Close();
        }

        private void fullScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _pnlCameras.Maximise(ContextTarget);
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            OpenUrl(Website + "/userguide.aspx#4");
        }

        private void inExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (configurationDirectory d in Conf.MediaDirectories)
            {
                string foldername = d.Entry;
                if (!foldername.EndsWith(@"\"))
                    foldername += @"\";
                Process.Start(foldername);
            }
        }

        private void menuItem1_Click_1(object sender, EventArgs e)
        {
            _pnlCameras.LayoutObjects(-1, -1);
        }

        private void llblSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
        }

        internal void MediaSelectAll()
        {
            bool check = false, first = true;
            lock (ThreadLock)
            {
                foreach (Control c in flowPreview.Controls)
                {
                    var pb = c as PreviewBox;
                    if (pb != null)
                    {
                        if (first)
                            check = !pb.Selected;
                        first = false;
                        pb.Selected = check;
                    }
                }
                flowPreview.Invalidate(true);
            }
        }


        private void opacityToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowForm(.1);
        }

        private void opacityToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ShowForm(.3);
        }

        private void opacityToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            ShowForm(1);
        }

        private void autoLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            autoLayoutToolStripMenuItem.Checked = menuItem26.Checked = !autoLayoutToolStripMenuItem.Checked;
            Conf.AutoLayout = autoLayoutToolStripMenuItem.Checked;
            if (Conf.AutoLayout)
                _pnlCameras.LayoutObjects(0, 0);
        }

        private void saveLayoutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _pnlCameras.SaveLayout();
            resetLayoutToolStripMenuItem1.Enabled = mnuResetLayout.Enabled = true;
        }

        private void resetLayoutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _pnlCameras.ResetLayout();
        }

        private void fullScreenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MaxMin();
        }

        private void statusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusBarToolStripMenuItem.Checked = menuItem4.Checked = !statusBarToolStripMenuItem.Checked;
            statusStrip1.Visible = statusBarToolStripMenuItem.Checked;

            Conf.ShowStatus = statusBarToolStripMenuItem.Checked;
        }

        private void fileMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileMenuToolStripMenuItem.Checked = menuItem5.Checked = !fileMenuToolStripMenuItem.Checked;
            Menu = !fileMenuToolStripMenuItem.Checked ? null : mainMenu;

            Conf.ShowFileMenu = fileMenuToolStripMenuItem.Checked;
        }

        private void toolStripToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripToolStripMenuItem.Checked = menuItem6.Checked = !toolStripToolStripMenuItem.Checked;
            toolStripMenu.Visible = toolStripToolStripMenuItem.Checked;
            Conf.ShowToolbar = toolStripToolStripMenuItem.Checked;
        }

        private void alwaysOnTopToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            alwaysOnTopToolStripMenuItem1.Checked = menuItem8.Checked = !alwaysOnTopToolStripMenuItem1.Checked;
            Conf.AlwaysOnTop = alwaysOnTopToolStripMenuItem1.Checked;
            TopMost = Conf.AlwaysOnTop;
        }

        private void mediaPaneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mediaPaneToolStripMenuItem.Checked = menuItem7.Checked = !mediaPaneToolStripMenuItem.Checked;
            Conf.ShowMediaPanel = mediaPaneToolStripMenuItem.Checked;
            ShowHideMediaPane();
        }

        private void ShowHideMediaPane()
        {
            if (Conf.ShowMediaPanel && Helper.HasFeature(Enums.Features.Access_Media))
            {
                splitContainer1.Panel2Collapsed = false;
                splitContainer1.Panel2.Show();
            }
            else
            {
                splitContainer1.Panel2Collapsed = true;
                splitContainer1.Panel2.Hide();
            }
        }

        private void iPCameraWithWizardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddCamera(1, true);
        }

        private void menuItem13_Click(object sender, EventArgs e)
        {
            OpenUrl(PurchaseLink);
        }

        private void tsbPlugins_Click(object sender, EventArgs e)
        {
            OpenUrl("http://www.ispyconnect.com/plugins.aspx");
        }

        private void flowPreview_MouseEnter(object sender, EventArgs e)
        {
            //flowPreview.Focus();
        }

        private void flowPreview_Click(object sender, EventArgs e)
        {
        }

        private void flCommands_MouseEnter(object sender, EventArgs e)
        {
            //flCommands.Focus();
        }

        public void PTZToolUpdate(CameraWindow cw)
        {
            if (_ptzTool != null)
            {
                _ptzTool.CameraControl = cw;
            }
        }


        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Handled)
                return;
            if (e.KeyCode == Keys.PageUp)
            {
                ProcessKey("previous_control");
                return;
            }

            if (e.KeyCode == Keys.PageDown)
            {
                ProcessKey("next_control"); return;
            }
            if (!e.Alt && !e.Shift && e.Control)
            {
                if (e.KeyCode == Keys.P )
                {
                    ProcessKey("play"); return;
                }

                if (e.KeyCode == Keys.S)
                {
                    ProcessKey("stop"); return;
                }

                if (e.KeyCode == Keys.R)
                {
                    ProcessKey("record"); return;
                }

                if (e.KeyCode == Keys.Z)
                {
                    ProcessKey("zoom"); return;
                }

                if (e.KeyCode == Keys.T )
                {
                    ProcessKey("talk"); return;
                }

                if (e.KeyCode == Keys.L )
                {
                    ProcessKey("listen"); return;
                }

                if (e.KeyCode == Keys.G )
                {
                    ProcessKey("grab"); return;
                }

                if (e.KeyCode == Keys.E )
                {
                    ProcessKey("edit"); return;
                }

                if (e.KeyCode == Keys.F )
                {
                    ProcessKey("tags"); return;
                }

                if (e.KeyCode == Keys.I)
                {
                    ProcessKey("import"); return;
                }
            }
            if (e.KeyCode == Keys.F4 && e.Alt)
            {
                ProcessKey("power"); return;
            }
            if (e.KeyCode.ToString() == "D0")
            {
                MaximiseControl(10); return;
            }
            if (e.KeyCode.ToString() == "D1")
            {
                MaximiseControl(0); return;
            }
            if (e.KeyCode.ToString() == "D2")
            {
                MaximiseControl(1); return;
            }
            if (e.KeyCode.ToString() == "D3")
            {
                MaximiseControl(2); return;
            }
            if (e.KeyCode.ToString() == "D4")
            {
                MaximiseControl(3); return;
            }
            if (e.KeyCode.ToString() == "D5")
            {
                MaximiseControl(4); return;
            }
            if (e.KeyCode.ToString() == "D6")
            {
                MaximiseControl(5); return;
            }
            if (e.KeyCode.ToString() == "D7")
            {
                MaximiseControl(6); return;
            }
            if (e.KeyCode.ToString() == "D8")
            {
                MaximiseControl(7); return;
            }
            if (e.KeyCode.ToString() == "D9")
            {
                MaximiseControl(8); return;
            }

            if (e.Alt && e.KeyCode == Keys.Enter)
            {
                MaxMin(); return;
            }
            if (e.KeyCode == Keys.Delete)
            {
                if (_lastClicked == _pnlCameras)
                {
                    ProcessKey("delete");
                    ProcessKey("next_control");
                    return;
                }
                if (_lastClicked == flowPreview)
                {
                    MediaDeleteSelected();
                }
            }
            if (e.KeyCode.ToString()=="Menu")
            {
                fileMenuToolStripMenuItem.Checked = menuItem5.Checked = true;
                Menu = !fileMenuToolStripMenuItem.Checked ? null : mainMenu;

                Conf.ShowFileMenu = fileMenuToolStripMenuItem.Checked; return;
            }
            int i = -1;
            var c = GetActiveControl(out i);
            if (i > -1)
            {
                var cw = c as CameraWindow;
                if (cw != null)
                {
                    var converter = new KeysConverter();
                    string cmd = converter.ConvertToString(e.KeyData);
                    if (cw.ExecutePluginShortcut(cmd))
                        return;
                }
            }
            //unhandled
        }

        private void MaximiseControl(int index)
        {
            foreach (Control c in _pnlCameras.Controls)
            {
                if (c.Tag is int)
                {
                    if ((int) c.Tag == index)
                    {
                        _pnlCameras.Maximise(c, true);
                        c.Focus();
                        break;
                    }
                }
            }
        }

        private void menuItem14_Click(object sender, EventArgs e)
        {
            if (_vc != null)
            {
                _vc.Close();
                _vc = null;
            }
            else
                ShowViewController();
        }

        private void ShowViewController()
        {
            if (_vc == null)
            {
                _vc = new ViewController(_pnlCameras);
                if (_pnlCameras.Height > 0)
                {
                    double ar = Convert.ToDouble(_pnlCameras.Height)/Convert.ToDouble(_pnlCameras.Width);
                    _vc.Width = 180;
                    _vc.Height = Convert.ToInt32(ar*_vc.Width);
                }
                _vc.TopMost = true;

                _vc.Show();
                _vc.Closed += _vc_Closed;
                viewControllerToolStripMenuItem.Checked = menuItem14.Checked = Conf.ViewController = true;
            }
            else
            {
                _vc.Show();
            }
        }

        private void _vc_Closed(object sender, EventArgs e)
        {
            _vc = null;
            viewControllerToolStripMenuItem.Checked = menuItem14.Checked = Conf.ViewController = false;
        }

        private void _pnlCameras_Scroll(object sender, ScrollEventArgs e)
        {
            if (_vc != null)
                _vc.Redraw();
        }

        private void _toolStripDropDownButton2_Click(object sender, EventArgs e)
        {
        }

        private void menuItem16_Click(object sender, EventArgs e)
        {
            Conf.LayoutMode = (int) LayoutModes.bottom;
            Arrange(true);
        }

        private void menuItem17_Click(object sender, EventArgs e)
        {
            Conf.LayoutMode = (int) LayoutModes.left;
            Arrange(true);
        }

        private void menuItem19_Click(object sender, EventArgs e)
        {
            Conf.LayoutMode = (int) LayoutModes.right;
            Arrange(true);
        }

        private void Arrange(bool ShowIfHidden)
        {
            if (!Conf.ShowMediaPanel)
            {
                if (ShowIfHidden)
                {
                    mediaPaneToolStripMenuItem.Checked = menuItem7.Checked = true;
                    Conf.ShowMediaPanel = true;
                    ShowHideMediaPane();
                }
                else
                    return;
            }

            SuspendLayout();
            try
            {
                var lm = (LayoutModes) Conf.LayoutMode;


                switch (lm)
                {
                    case LayoutModes.bottom:
                        splitContainer1.Orientation = Orientation.Horizontal;
                        splitContainer1.RightToLeft = RightToLeft.No;

                        splitContainer2.Orientation = Orientation.Vertical;
                        splitContainer2.RightToLeft = RightToLeft.No;

                        splitContainer1.SplitterDistance = splitContainer1.Height - 200;
                        splitContainer2.SplitterDistance = splitContainer2.Width - 200;
                        break;
                    case LayoutModes.left:
                        splitContainer1.Orientation = Orientation.Vertical;
                        splitContainer1.RightToLeft = RightToLeft.Yes;

                        splitContainer2.Orientation = Orientation.Horizontal;
                        splitContainer2.RightToLeft = RightToLeft.No;

                        splitContainer1.SplitterDistance = splitContainer1.Width - 200;
                        splitContainer2.SplitterDistance = splitContainer2.Height - 200;
                        break;
                    case LayoutModes.right:
                        splitContainer1.Orientation = Orientation.Vertical;
                        splitContainer1.RightToLeft = RightToLeft.No;

                        splitContainer2.Orientation = Orientation.Horizontal;
                        splitContainer2.RightToLeft = RightToLeft.No;

                        splitContainer1.SplitterDistance = splitContainer1.Width - 200;
                        splitContainer2.SplitterDistance = splitContainer2.Height - 200;

                        break;
                }
            }
            catch
            {
            }
            ResumeLayout(true);
        }

        private void menuItem18_Click(object sender, EventArgs e)
        {
            ShowHidePTZTool();
        }

        private void pTZControllerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowHidePTZTool();
        }

        private void ShowHidePTZTool()
        {
            bool bShow = true;
            if (_ptzTool != null)
            {
                _ptzTool.Close();
                bShow = false;
            }
            else
            {
                _ptzTool = new PTZTool {Owner = this};
                _ptzTool.Show(this);
                _ptzTool.Closing += PTZToolClosing;
                _ptzTool.CameraControl = null;
                for (int i = 0; i < _pnlCameras.Controls.Count; i++)
                {
                    Control c = _pnlCameras.Controls[i];
                    if (c.Focused && c is CameraWindow)
                    {
                        _ptzTool.CameraControl = (CameraWindow) c;
                        break;
                    }
                }
            }
            pTZControllerToolStripMenuItem.Checked =
                menuItem18.Checked = pTZControllerToolStripMenuItem1.Checked = bShow;
            Conf.ShowPTZController = bShow;
        }

        private void PTZToolClosing(object sender, CancelEventArgs e)
        {
            pTZControllerToolStripMenuItem.Checked =
                menuItem18.Checked = pTZControllerToolStripMenuItem1.Checked = false;
            Conf.ShowPTZController = false;
            _ptzTool = null;
        }

        public void TalkTo(CameraWindow cw, bool talk)
        {
            if (string.IsNullOrEmpty(Conf.TalkMic))
                return;

            if (_talkSource != null)
            {
                _talkSource.Stop();
                _talkSource = null;
            }
            if (_talkTarget != null)
            {
                _talkTarget.Stop();
                _talkTarget = null;
            }

            if (!talk)
            {
                if (cw.VolumeControl != null)
                {
                    cw.VolumeControl.Listening = false;
                }
                return;
            }
            Application.DoEvents();
            TalkCamera = cw;
            _talkSource = new TalkDeviceStream(Conf.TalkMic) {RecordingFormat = new WaveFormat(8000, 16, 1)};
            _talkSource.AudioFinished += _talkSource_AudioFinished;

            if (!_talkSource.IsRunning)
                _talkSource.Start();

            _talkTarget = TalkHelper.GetTalkTarget(cw.Camobject, _talkSource);
            _talkTarget.TalkStopped += TalkTargetTalkStopped;
            _talkTarget.Start();

            //auto listen
            if (cw.VolumeControl != null)
            {
                cw.VolumeControl.Listening = true;
            }
        }

        private void _talkSource_AudioFinished(object sender, PlayingFinishedEventArgs e)
        {
            //Logger.LogMessage("Talk Finished: " + reason);
        }

        private void TalkTargetTalkStopped(object sender, EventArgs e)
        {
            if (TalkCamera != null)
            {
                TalkCamera.Talking = false;
            }
        }

        private void pTZToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void viewControllerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_vc != null)
            {
                _vc.Close();
                _vc = null;
            }
            else
                ShowViewController();
        }

        private void menuItem22_Click(object sender, EventArgs e)
        {
            Conf.LockLayout = !Conf.LockLayout;
            menuItem22.Checked = Conf.LockLayout;
        }

        private void iSpyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((PreviewBox) ContextTarget).PlayMedia(Enums.PlaybackMode.iSpy);
        }

        private void defaultPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((PreviewBox) ContextTarget).PlayMedia(Enums.PlaybackMode.Default);
        }

        private void websiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((PreviewBox) ContextTarget).PlayMedia(Enums.PlaybackMode.Website);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MediaDeleteSelected();
        }

        private void pTZControllerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowHidePTZTool();
        }

        private void showInFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var pb = ((PreviewBox) ContextTarget);

            string argument = @"/select, " + pb.FileName;
            Process.Start("explorer.exe", argument);
        }

        private void otherVideoSourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddCamera(8);
        }

        private void videoFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddCamera(5);
        }

        private void saveToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_fbdSaveTo.ShowDialog(Handle))
                {
                    lock (ThreadLock)
                    {
                        for (int i = 0; i < flowPreview.Controls.Count; i++)
                        {
                            var pb = flowPreview.Controls[i] as PreviewBox;
                            if (pb != null && pb.Selected)
                            {
                                var fi = new FileInfo(pb.FileName);
                                File.Copy(pb.FileName, _fbdSaveTo.FileName + @"\" + fi.Name);
                            }
                        }
                    }                   
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }      

        private void _tsslStats_Click(object sender, EventArgs e)
        {
            if (!MWS.Running)
            {
                ShowLogFile();
            }
            else
            {
                
                if (WsWrapper.WebsiteLive && !WsWrapper.LoginFailed && !string.IsNullOrEmpty(Conf.WSUsername))
                {
                    if (Conf.ServicesEnabled)
                    {
                        OpenUrl(!Conf.Subscribed
                            ? Webserver + "/subscribe.aspx"
                            : Webpage);
                    }
                    else
                    {
                        OpenUrl(Webserver);
                    }
                }
                else
                {
                    WebConnect();
                }
                
            }
        }

        private void UnlockLayout()
        {
            Conf.LockLayout = menuItem22.Checked = false;
        }

        private void MaxMin()
        {
            fullScreenToolStripMenuItem1.Checked = menuItem3.Checked = !fullScreenToolStripMenuItem1.Checked;
            if (fullScreenToolStripMenuItem1.Checked)
            {
                WindowState = FormWindowState.Maximized;
                FormBorderStyle = FormBorderStyle.None;
                WinApi.SetWinFullScreen(Handle);
            }
            else
            {
                WindowState = FormWindowState.Maximized;
                FormBorderStyle = FormBorderStyle.Sizable;
            }
            Conf.Fullscreen = fullScreenToolStripMenuItem1.Checked;
        }

        public static string Webpage
        {
            get
            {
                if (CustomWebserver)
                    return Webserver + "/watch_new.aspx";
                return Webserver + "/monitor/";
            }
        }


        private void ListGridViews()
        {
            while (tssbGridViews.DropDownItems.Count > 1)
            {
                tssbGridViews.DropDownItems.RemoveAt(0);
                menuItem31.MenuItems.RemoveAt(0);
            }

            foreach (configurationGrid gv in Conf.GridViews)
            {
                var tsi = new ToolStripMenuItem(gv.name, Resources.Video2);
                tsi.Click += tsi_Click;
                tssbGridViews.DropDownItems.Insert(0,tsi);
                var mi = new MenuItem(gv.name,mi_click);
                menuItem31.MenuItems.Add(0, mi);
            }
        }

        void mi_click(object sender, EventArgs e)
        {
            var mi = (MenuItem)sender;
            ShowGridView(mi.Text);
        }
        
        void tsi_Click(object sender, EventArgs e)
        {
            var mi = (ToolStripItem)sender;
            ShowGridView(mi.Text);
        }

        void tsi_MaximiseClick(object sender, EventArgs e)
        {
            var mi = (ToolStripItem)sender;
            foreach (Control o in _pnlCameras.Controls)
            {
                var ic = o as ISpyControl;
                if (ic!=null && ic.ObjectName == mi.Text)
                {
                    _pnlCameras.Maximise(ic);
                    return;
                }
            }
        }

        

        public void EditGridView(string name, IWin32Window parent = null)
        {
            if (parent == null)
                parent = this;
            configurationGrid cg = Conf.GridViews.FirstOrDefault(p => p.name == name);
            if (cg != null)
            {
                var gvc = new GridViewCustom
                          {
                              Cols = cg.Columns,
                              Rows = cg.Rows,
                              GridName = cg.name,
                              FullScreen = cg.FullScreen,
                              AlwaysOnTop = cg.AlwaysOnTop,
                              Display = cg.Display,
                              Framerate = cg.Framerate,
                              Mode = cg.ModeIndex,
                              ModeConfig = cg.ModeConfig,
                              Overlays = cg.Overlays,
                              Fill = cg.Fill,
                              ShowAtStartup = cg.ShowAtStartup,
                          };
               // bool b = ((Form) parent).TopMost;
                //((Form) parent).TopMost = false;
                gvc.ShowDialog(parent);
                //((Form)parent).TopMost = b;
                if (gvc.DialogResult == DialogResult.OK)
                {
                    cg.Columns = gvc.Cols;
                    cg.Rows = gvc.Rows;
                    cg.name = gvc.GridName;
                    cg.FullScreen = gvc.FullScreen;
                    cg.AlwaysOnTop = gvc.AlwaysOnTop;
                    cg.Display = gvc.Display;
                    cg.Framerate = gvc.Framerate;
                    cg.ModeIndex = gvc.Mode;
                    cg.ModeConfig = gvc.ModeConfig;
                    cg.Overlays = gvc.Overlays;
                    cg.Fill = gvc.Fill;
                    cg.ShowAtStartup = gvc.ShowAtStartup;
                    ListGridViews();
                }
                gvc.Dispose();
            }
        }

        private void oNVIFCameraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddCamera(9);
        }


        private void configurePluginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cameraControl = ContextTarget as CameraWindow;
            if (cameraControl?.Camera?.Plugin != null)
            {
                cameraControl.ConfigurePlugin();
            }
        }

        private void flowPreview_MouseLeave(object sender, EventArgs e)
        {
            tsslMediaInfo.Text = "";
            //panel1.Hide(); - can't do this as not compatible with touch
        }


        private void llblFilter_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
        }

        internal void MediaFilter()
        {
            var f = new Filter();
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                LoadPreviews();
            }
            f.Dispose(); 
        }

        private void menuItem26_Click(object sender, EventArgs e)
        {
            autoLayoutToolStripMenuItem.Checked = menuItem26.Checked = !menuItem26.Checked;
            Conf.AutoLayout = autoLayoutToolStripMenuItem.Checked;
            if (Conf.AutoLayout)
                _pnlCameras.LayoutObjects(0, 0);
        }

        private void flowPreview_MouseDown(object sender, MouseEventArgs e)
        {
            _lastClicked = flowPreview;
            flowPreview.Focus();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

#region Windows Form Designer generated code

        /// <summary>
        ///     Required method for Designer support - do not modify
        ///     the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this._fileItem = new System.Windows.Forms.MenuItem();
            this._menuItem19 = new System.Windows.Forms.MenuItem();
            this._menuItem21 = new System.Windows.Forms.MenuItem();
            this.menuItem36 = new System.Windows.Forms.MenuItem();
            this.menuItem28 = new System.Windows.Forms.MenuItem();
            this._menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem13 = new System.Windows.Forms.MenuItem();
            this.menuItem33 = new System.Windows.Forms.MenuItem();
            this._exitFileItem = new System.Windows.Forms.MenuItem();
            this._menuItem36 = new System.Windows.Forms.MenuItem();
            this._menuItem37 = new System.Windows.Forms.MenuItem();
            this.menuItem40 = new System.Windows.Forms.MenuItem();
            this._menuItem16 = new System.Windows.Forms.MenuItem();
            this._menuItem17 = new System.Windows.Forms.MenuItem();
            this._menuItem7 = new System.Windows.Forms.MenuItem();
            this._menuItem23 = new System.Windows.Forms.MenuItem();
            this.menuItem29 = new System.Windows.Forms.MenuItem();
            this._menuItem3 = new System.Windows.Forms.MenuItem();
            this._menuItem25 = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.menuItem11 = new System.Windows.Forms.MenuItem();
            this.menuItem12 = new System.Windows.Forms.MenuItem();
            this.menuItem34 = new System.Windows.Forms.MenuItem();
            this.menuItem35 = new System.Windows.Forms.MenuItem();
            this._menuItem13 = new System.Windows.Forms.MenuItem();
            this.menuItem38 = new System.Windows.Forms.MenuItem();
            this.menuItem39 = new System.Windows.Forms.MenuItem();
            this._menuItem39 = new System.Windows.Forms.MenuItem();
            this._menuItem12 = new System.Windows.Forms.MenuItem();
            this._menuItem14 = new System.Windows.Forms.MenuItem();
            this._menuItem28 = new System.Windows.Forms.MenuItem();
            this.menuItem21 = new System.Windows.Forms.MenuItem();
            this._menuItem29 = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem27 = new System.Windows.Forms.MenuItem();
            this.mnuSaveLayout = new System.Windows.Forms.MenuItem();
            this.mnuResetLayout = new System.Windows.Forms.MenuItem();
            this.menuItem26 = new System.Windows.Forms.MenuItem();
            this.menuItem22 = new System.Windows.Forms.MenuItem();
            this.menuItem15 = new System.Windows.Forms.MenuItem();
            this.menuItem16 = new System.Windows.Forms.MenuItem();
            this.menuItem17 = new System.Windows.Forms.MenuItem();
            this.menuItem19 = new System.Windows.Forms.MenuItem();
            this.menuItem20 = new System.Windows.Forms.MenuItem();
            this._menuItem20 = new System.Windows.Forms.MenuItem();
            this._menuItem22 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuItem31 = new System.Windows.Forms.MenuItem();
            this.menuItem32 = new System.Windows.Forms.MenuItem();
            this.menuItem23 = new System.Windows.Forms.MenuItem();
            this.menuItem14 = new System.Windows.Forms.MenuItem();
            this.menuItem18 = new System.Windows.Forms.MenuItem();
            this.menuItem24 = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this._menuItem9 = new System.Windows.Forms.MenuItem();
            this._menuItem18 = new System.Windows.Forms.MenuItem();
            this._menuItem8 = new System.Windows.Forms.MenuItem();
            this._menuItem15 = new System.Windows.Forms.MenuItem();
            this._menuItem6 = new System.Windows.Forms.MenuItem();
            this._menuItem34 = new System.Windows.Forms.MenuItem();
            this._miOnAll = new System.Windows.Forms.MenuItem();
            this._miOnSched = new System.Windows.Forms.MenuItem();
            this._menuItem33 = new System.Windows.Forms.MenuItem();
            this._miOffAll = new System.Windows.Forms.MenuItem();
            this._miOffSched = new System.Windows.Forms.MenuItem();
            this._menuItem31 = new System.Windows.Forms.MenuItem();
            this._miApplySchedule = new System.Windows.Forms.MenuItem();
            this.menuItem37 = new System.Windows.Forms.MenuItem();
            this._menuItem32 = new System.Windows.Forms.MenuItem();
            this._menuItem4 = new System.Windows.Forms.MenuItem();
            this._menuItem35 = new System.Windows.Forms.MenuItem();
            this.menuItem30 = new System.Windows.Forms.MenuItem();
            this.menuItem25 = new System.Windows.Forms.MenuItem();
            this._helpItem = new System.Windows.Forms.MenuItem();
            this._aboutHelpItem = new System.Windows.Forms.MenuItem();
            this._menuItem30 = new System.Windows.Forms.MenuItem();
            this._menuItem2 = new System.Windows.Forms.MenuItem();
            this._menuItem24 = new System.Windows.Forms.MenuItem();
            this._menuItem10 = new System.Windows.Forms.MenuItem();
            this._menuItem38 = new System.Windows.Forms.MenuItem();
            this._menuItem5 = new System.Windows.Forms.MenuItem();
            this._menuItem27 = new System.Windows.Forms.MenuItem();
            this._menuItem26 = new System.Windows.Forms.MenuItem();
            this.ctxtMainForm = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._addCameraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._addMicrophoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._addFloorPlanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._remoteCommandsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._applyScheduleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.opacityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.opacityToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.opacityToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.opacityToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.layoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveLayoutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.resetLayoutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.displayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fullScreenToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.statusBarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileMenuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mediaPaneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pTZControllerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewControllerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alwaysOnTopToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.gridViewsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.maximiseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenu = new System.Windows.Forms.ToolStrip();
            this._toolStripDropDownButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this._localCameraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._iPCameraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iPCameraWithWizardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oNVIFCameraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._floorPlanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.videoFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.otherVideoSourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._microphoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._uSbCamerasAndMicrophonesOnOtherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this._thruWebsiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._onMobileDevicesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tssbGridViews = new System.Windows.Forms.ToolStripDropDownButton();
            this.manageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._toolStripButton8 = new System.Windows.Forms.ToolStripButton();
            this._toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.tsbPlugins = new System.Windows.Forms.ToolStripButton();
            this._toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.ctxtMnu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pluginCommandsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configurePluginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openWebInterfaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._viewMediaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._viewMediaOnAMobileDeviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.switchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.onToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.offToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alertsOnToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.alertsOffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scheduleOnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scheduleOffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pTZScheduleOnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pTZScheduleOffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._recordNowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._takePhotoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pTZToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pTZControllerToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this._listenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tagsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._applyScheduleToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this._positionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fullScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._resetSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._resetRecordingCounterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._showFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxtTaskbar = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._unlockToolstripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._switchAllOnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._switchAllOffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._showToolstripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._showISpy10PercentOpacityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._showISpy30OpacityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._showISpy100PercentOpacityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._helpToolstripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._websiteToolstripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridViewsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.viewLogFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this._tsslStats = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslMonitor = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslPerformance = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslMediaInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslPRO = new System.Windows.Forms.ToolStripStatusLabel();
            this._pnlContent = new System.Windows.Forms.Panel();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.flowPreview = new iSpyApplication.Controls.MediaPanel();
            this.mediaPanelControl1 = new iSpyApplication.Controls.MediaPanelControl();
            this.flCommands = new System.Windows.Forms.FlowLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this._pnlCameras = new iSpyApplication.Controls.LayoutPanel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.ctxtPlayer = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.iSpyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defaultPlayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.websiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showInFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uploadToYouTubePublicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uploadToCloudToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.archiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxtMainForm.SuspendLayout();
            this.toolStripMenu.SuspendLayout();
            this.ctxtMnu.SuspendLayout();
            this.ctxtTaskbar.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this._pnlContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.flCommands.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.ctxtPlayer.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this._fileItem,
            this._menuItem36,
            this._menuItem16,
            this._menuItem9,
            this._helpItem});
            // 
            // _fileItem
            // 
            this._fileItem.Index = 0;
            this._fileItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this._menuItem19,
            this._menuItem21,
            this.menuItem36,
            this.menuItem28,
            this._menuItem1,
            this.menuItem13,
            this.menuItem33,
            this._exitFileItem});
            this._fileItem.Text = "&File";
            // 
            // _menuItem19
            // 
            this._menuItem19.Index = 0;
            this._menuItem19.Text = "&Save Object List";
            this._menuItem19.Click += new System.EventHandler(this.MenuItem19Click);
            // 
            // _menuItem21
            // 
            this._menuItem21.Index = 1;
            this._menuItem21.Text = "&Open Object List";
            this._menuItem21.Click += new System.EventHandler(this.MenuItem21Click);
            // 
            // menuItem36
            // 
            this.menuItem36.Index = 2;
            this.menuItem36.Text = "Import Objects";
            this.menuItem36.Click += new System.EventHandler(this.menuItem36_Click);
            // 
            // menuItem28
            // 
            this.menuItem28.Index = 3;
            this.menuItem28.Text = "Remove All Objects";
            this.menuItem28.Click += new System.EventHandler(this.menuItem28_Click);
            // 
            // _menuItem1
            // 
            this._menuItem1.Index = 4;
            this._menuItem1.Text = "-";
            // 
            // menuItem13
            // 
            this.menuItem13.Index = 5;
            this.menuItem13.Text = "Purchase More Cameras";
            this.menuItem13.Click += new System.EventHandler(this.menuItem13_Click);
            // 
            // menuItem33
            // 
            this.menuItem33.Index = 6;
            this.menuItem33.Text = "Lock";
            this.menuItem33.Click += new System.EventHandler(this.menuItem33_Click);
            // 
            // _exitFileItem
            // 
            this._exitFileItem.Index = 7;
            this._exitFileItem.Text = "E&xit";
            this._exitFileItem.Click += new System.EventHandler(this.ExitFileItemClick);
            // 
            // _menuItem36
            // 
            this._menuItem36.Index = 1;
            this._menuItem36.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this._menuItem37,
            this.menuItem40});
            this._menuItem36.Text = "&Edit";
            // 
            // _menuItem37
            // 
            this._menuItem37.Index = 0;
            this._menuItem37.Text = "Cameras and Microphones";
            this._menuItem37.Click += new System.EventHandler(this.MenuItem37Click);
            // 
            // menuItem40
            // 
            this.menuItem40.Index = 1;
            this.menuItem40.Text = "Find...";
            this.menuItem40.Click += new System.EventHandler(this.menuItem40_Click);
            // 
            // _menuItem16
            // 
            this._menuItem16.Index = 2;
            this._menuItem16.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this._menuItem17,
            this._menuItem3,
            this._menuItem25,
            this.menuItem9,
            this.menuItem34,
            this._menuItem13,
            this.menuItem38,
            this.menuItem15,
            this.menuItem20,
            this._menuItem20,
            this._menuItem22,
            this.menuItem2,
            this.menuItem3,
            this.menuItem4,
            this.menuItem5,
            this.menuItem6,
            this.menuItem7,
            this.menuItem31,
            this.menuItem23,
            this.menuItem14,
            this.menuItem18,
            this.menuItem24,
            this.menuItem8});
            this._menuItem16.Text = "&View";
            // 
            // _menuItem17
            // 
            this._menuItem17.Index = 0;
            this._menuItem17.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this._menuItem7,
            this._menuItem23,
            this.menuItem29});
            this._menuItem17.Text = "Files";
            this._menuItem17.Click += new System.EventHandler(this.MenuItem17Click);
            // 
            // _menuItem7
            // 
            this._menuItem7.Index = 0;
            this._menuItem7.Text = "&Video (files)";
            this._menuItem7.Click += new System.EventHandler(this.MenuItem7Click);
            // 
            // _menuItem23
            // 
            this._menuItem23.Index = 1;
            this._menuItem23.Text = "&Audio (files)";
            this._menuItem23.Click += new System.EventHandler(this.MenuItem23Click);
            // 
            // menuItem29
            // 
            this.menuItem29.Index = 2;
            this.menuItem29.Text = "Archi&ve";
            this.menuItem29.Click += new System.EventHandler(this.menuItem29_Click);
            // 
            // _menuItem3
            // 
            this._menuItem3.Index = 1;
            this._menuItem3.Text = "Media &Over the Web";
            this._menuItem3.Click += new System.EventHandler(this.MenuItem3Click);
            // 
            // _menuItem25
            // 
            this._menuItem25.Index = 2;
            this._menuItem25.Text = "Media on a Mobile &Device (iPhone/ Android/ Windows 7 etc)";
            this._menuItem25.Click += new System.EventHandler(this.MenuItem25Click);
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 3;
            this.menuItem9.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem10,
            this.menuItem11,
            this.menuItem12});
            this.menuItem9.Text = "Opacity";
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 0;
            this.menuItem10.Text = "10%";
            this.menuItem10.Click += new System.EventHandler(this.opacityToolStripMenuItem1_Click);
            // 
            // menuItem11
            // 
            this.menuItem11.Index = 1;
            this.menuItem11.Text = "30%";
            this.menuItem11.Click += new System.EventHandler(this.opacityToolStripMenuItem2_Click);
            // 
            // menuItem12
            // 
            this.menuItem12.Index = 2;
            this.menuItem12.Text = "100%";
            this.menuItem12.Click += new System.EventHandler(this.opacityToolStripMenuItem3_Click);
            // 
            // menuItem34
            // 
            this.menuItem34.Index = 4;
            this.menuItem34.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem35});
            this.menuItem34.Text = "Window";
            // 
            // menuItem35
            // 
            this.menuItem35.Index = 0;
            this.menuItem35.Text = "Command Buttons";
            this.menuItem35.Click += new System.EventHandler(this.menuItem35_Click);
            // 
            // _menuItem13
            // 
            this._menuItem13.Index = 5;
            this._menuItem13.Text = "-";
            // 
            // menuItem38
            // 
            this.menuItem38.Index = 6;
            this.menuItem38.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem39,
            this._menuItem39,
            this.mnuSaveLayout,
            this.mnuResetLayout,
            this.menuItem26,
            this.menuItem22});
            this.menuItem38.Text = "Layout";
            // 
            // menuItem39
            // 
            this.menuItem39.Checked = true;
            this.menuItem39.Index = 0;
            this.menuItem39.Text = "Auto Grid";
            this.menuItem39.Click += new System.EventHandler(this.menuItem39_Click);
            // 
            // _menuItem39
            // 
            this._menuItem39.Index = 1;
            this._menuItem39.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this._menuItem12,
            this._menuItem14,
            this._menuItem28,
            this.menuItem21,
            this._menuItem29,
            this.menuItem1,
            this.menuItem27});
            this._menuItem39.Text = "Auto Layout Objects";
            this._menuItem39.Click += new System.EventHandler(this.MenuItem39Click);
            // 
            // _menuItem12
            // 
            this._menuItem12.Index = 0;
            this._menuItem12.Text = "160 x 120";
            this._menuItem12.Click += new System.EventHandler(this.MenuItem12Click);
            // 
            // _menuItem14
            // 
            this._menuItem14.Index = 1;
            this._menuItem14.Text = "320 x 240";
            this._menuItem14.Click += new System.EventHandler(this.MenuItem14Click);
            // 
            // _menuItem28
            // 
            this._menuItem28.Index = 2;
            this._menuItem28.Text = "640 x 480";
            this._menuItem28.Click += new System.EventHandler(this.MenuItem28Click2);
            // 
            // menuItem21
            // 
            this.menuItem21.Index = 3;
            this.menuItem21.Text = "Optimised";
            this.menuItem21.Click += new System.EventHandler(this.menuItem21_Click);
            // 
            // _menuItem29
            // 
            this._menuItem29.Index = 4;
            this._menuItem29.Text = "Current";
            this._menuItem29.Click += new System.EventHandler(this.MenuItem29Click1);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 5;
            this.menuItem1.Text = "Native";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click_1);
            // 
            // menuItem27
            // 
            this.menuItem27.Index = 6;
            this.menuItem27.Text = "Minimise";
            this.menuItem27.Click += new System.EventHandler(this.menuItem27_Click);
            // 
            // mnuSaveLayout
            // 
            this.mnuSaveLayout.Index = 2;
            this.mnuSaveLayout.Text = "&Save Layout";
            this.mnuSaveLayout.Click += new System.EventHandler(this.saveLayoutToolStripMenuItem1_Click);
            // 
            // mnuResetLayout
            // 
            this.mnuResetLayout.Index = 3;
            this.mnuResetLayout.Text = "&Reset Layout";
            this.mnuResetLayout.Click += new System.EventHandler(this.resetLayoutToolStripMenuItem1_Click);
            // 
            // menuItem26
            // 
            this.menuItem26.Index = 4;
            this.menuItem26.Text = "Auto Layout";
            this.menuItem26.Click += new System.EventHandler(this.menuItem26_Click);
            // 
            // menuItem22
            // 
            this.menuItem22.Checked = true;
            this.menuItem22.Index = 5;
            this.menuItem22.Text = "Lock Layout";
            this.menuItem22.Click += new System.EventHandler(this.menuItem22_Click);
            // 
            // menuItem15
            // 
            this.menuItem15.Index = 7;
            this.menuItem15.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem16,
            this.menuItem17,
            this.menuItem19});
            this.menuItem15.Text = "Arrange Media";
            // 
            // menuItem16
            // 
            this.menuItem16.Index = 0;
            this.menuItem16.Text = "Bottom";
            this.menuItem16.Click += new System.EventHandler(this.menuItem16_Click);
            // 
            // menuItem17
            // 
            this.menuItem17.Index = 1;
            this.menuItem17.Text = "Left";
            this.menuItem17.Click += new System.EventHandler(this.menuItem17_Click);
            // 
            // menuItem19
            // 
            this.menuItem19.Index = 2;
            this.menuItem19.Text = "Right";
            this.menuItem19.Click += new System.EventHandler(this.menuItem19_Click);
            // 
            // menuItem20
            // 
            this.menuItem20.Index = 8;
            this.menuItem20.Text = "-";
            // 
            // _menuItem20
            // 
            this._menuItem20.Index = 9;
            this._menuItem20.Text = "Log &File";
            this._menuItem20.Click += new System.EventHandler(this.MenuItem20Click);
            // 
            // _menuItem22
            // 
            this._menuItem22.Index = 10;
            this._menuItem22.Text = "Log F&iles";
            this._menuItem22.Click += new System.EventHandler(this.MenuItem22Click1);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 11;
            this.menuItem2.Text = "-";
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 12;
            this.menuItem3.Text = "&Full Screen";
            this.menuItem3.Click += new System.EventHandler(this.fullScreenToolStripMenuItem1_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 13;
            this.menuItem4.Text = "Status Bar";
            this.menuItem4.Click += new System.EventHandler(this.statusBarToolStripMenuItem_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 14;
            this.menuItem5.Text = "File Menu";
            this.menuItem5.Click += new System.EventHandler(this.fileMenuToolStripMenuItem_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 15;
            this.menuItem6.Text = "Tool Bar";
            this.menuItem6.Click += new System.EventHandler(this.toolStripToolStripMenuItem_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 16;
            this.menuItem7.Text = "Media Pane";
            this.menuItem7.Click += new System.EventHandler(this.mediaPaneToolStripMenuItem_Click);
            // 
            // menuItem31
            // 
            this.menuItem31.Index = 17;
            this.menuItem31.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem32});
            this.menuItem31.Text = "Grid Views";
            // 
            // menuItem32
            // 
            this.menuItem32.Index = 0;
            this.menuItem32.Text = "Manage";
            this.menuItem32.Click += new System.EventHandler(this.menuItem32_Click);
            // 
            // menuItem23
            // 
            this.menuItem23.Index = 18;
            this.menuItem23.Text = "-";
            // 
            // menuItem14
            // 
            this.menuItem14.Index = 19;
            this.menuItem14.Text = "Layout Controller";
            this.menuItem14.Click += new System.EventHandler(this.menuItem14_Click);
            // 
            // menuItem18
            // 
            this.menuItem18.Index = 20;
            this.menuItem18.Text = "PTZ Controller";
            this.menuItem18.Click += new System.EventHandler(this.menuItem18_Click);
            // 
            // menuItem24
            // 
            this.menuItem24.Index = 21;
            this.menuItem24.Text = "-";
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 22;
            this.menuItem8.Text = "Always on Top";
            this.menuItem8.Click += new System.EventHandler(this.alwaysOnTopToolStripMenuItem1_Click);
            // 
            // _menuItem9
            // 
            this._menuItem9.Index = 3;
            this._menuItem9.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this._menuItem18,
            this._menuItem8,
            this._menuItem15,
            this._menuItem6,
            this._menuItem34,
            this._menuItem33,
            this._menuItem31,
            this._miApplySchedule,
            this.menuItem37,
            this._menuItem32,
            this._menuItem4,
            this._menuItem35,
            this.menuItem30,
            this.menuItem25});
            this._menuItem9.Text = "&Options";
            // 
            // _menuItem18
            // 
            this._menuItem18.Index = 0;
            this._menuItem18.Text = "&Clear Capture Directories";
            this._menuItem18.Click += new System.EventHandler(this.MenuItem18Click);
            // 
            // _menuItem8
            // 
            this._menuItem8.Index = 1;
            this._menuItem8.Text = "&Settings";
            this._menuItem8.Click += new System.EventHandler(this.MenuItem8Click);
            // 
            // _menuItem15
            // 
            this._menuItem15.Index = 2;
            this._menuItem15.Text = "Reset all Recording Counters";
            this._menuItem15.Click += new System.EventHandler(this.MenuItem15Click);
            // 
            // _menuItem6
            // 
            this._menuItem6.Index = 3;
            this._menuItem6.Text = "-";
            // 
            // _menuItem34
            // 
            this._menuItem34.Index = 4;
            this._menuItem34.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this._miOnAll,
            this._miOnSched});
            this._menuItem34.Text = "Switch On";
            this._menuItem34.Click += new System.EventHandler(this.MenuItem34Click);
            // 
            // _miOnAll
            // 
            this._miOnAll.Index = 0;
            this._miOnAll.Text = "All";
            this._miOnAll.Click += new System.EventHandler(this.MenuItem24Click);
            // 
            // _miOnSched
            // 
            this._miOnSched.Index = 1;
            this._miOnSched.Text = "Scheduled";
            this._miOnSched.Click += new System.EventHandler(this.MenuItem28Click1);
            // 
            // _menuItem33
            // 
            this._menuItem33.Index = 5;
            this._menuItem33.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this._miOffAll,
            this._miOffSched});
            this._menuItem33.Text = "Switch Off";
            this._menuItem33.Click += new System.EventHandler(this.MenuItem33Click);
            // 
            // _miOffAll
            // 
            this._miOffAll.Index = 0;
            this._miOffAll.Text = "All";
            this._miOffAll.Click += new System.EventHandler(this.MenuItem40Click);
            // 
            // _miOffSched
            // 
            this._miOffSched.Index = 1;
            this._miOffSched.Text = "Scheduled";
            this._miOffSched.Click += new System.EventHandler(this.MenuItem41Click);
            // 
            // _menuItem31
            // 
            this._menuItem31.Index = 6;
            this._menuItem31.Text = "&Remove All Objects";
            this._menuItem31.Click += new System.EventHandler(this.MenuItem31Click);
            // 
            // _miApplySchedule
            // 
            this._miApplySchedule.Index = 7;
            this._miApplySchedule.Text = "Apply Schedule";
            this._miApplySchedule.Click += new System.EventHandler(this.MenuItem24Click1);
            // 
            // menuItem37
            // 
            this.menuItem37.Index = 8;
            this.menuItem37.Text = "&Change User";
            this.menuItem37.Click += new System.EventHandler(this.menuItem37_Click);
            // 
            // _menuItem32
            // 
            this._menuItem32.Index = 9;
            this._menuItem32.Text = "-";
            // 
            // _menuItem4
            // 
            this._menuItem4.Index = 10;
            this._menuItem4.Text = "Configure &Remote Access";
            this._menuItem4.Click += new System.EventHandler(this.MenuItem4Click);
            // 
            // _menuItem35
            // 
            this._menuItem35.Index = 11;
            this._menuItem35.Text = "Configure &Remote Commands";
            this._menuItem35.Click += new System.EventHandler(this.MenuItem35Click);
            // 
            // menuItem30
            // 
            this.menuItem30.Index = 12;
            this.menuItem30.Text = "-";
            // 
            // menuItem25
            // 
            this.menuItem25.Index = 13;
            this.menuItem25.Text = "Default &Device Manager";
            this.menuItem25.Click += new System.EventHandler(this.menuItem25_Click);
            // 
            // _helpItem
            // 
            this._helpItem.Index = 4;
            this._helpItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this._aboutHelpItem,
            this._menuItem30,
            this._menuItem2,
            this._menuItem24,
            this._menuItem10,
            this._menuItem38,
            this._menuItem5,
            this._menuItem27,
            this._menuItem26});
            this._helpItem.Text = "&Help";
            // 
            // _aboutHelpItem
            // 
            this._aboutHelpItem.Index = 0;
            this._aboutHelpItem.Text = "&About";
            this._aboutHelpItem.Click += new System.EventHandler(this.AboutHelpItemClick);
            // 
            // _menuItem30
            // 
            this._menuItem30.Index = 1;
            this._menuItem30.Text = "-";
            // 
            // _menuItem2
            // 
            this._menuItem2.Index = 2;
            this._menuItem2.Text = "&Help";
            this._menuItem2.Click += new System.EventHandler(this.MenuItem2Click);
            // 
            // _menuItem24
            // 
            this._menuItem24.Index = 3;
            this._menuItem24.Text = "Show &Getting Started";
            this._menuItem24.Click += new System.EventHandler(this.MenuItem24Click2);
            // 
            // _menuItem10
            // 
            this._menuItem10.Index = 4;
            this._menuItem10.Text = "&Check For Updates";
            this._menuItem10.Click += new System.EventHandler(this.MenuItem10Click);
            // 
            // _menuItem38
            // 
            this._menuItem38.Index = 5;
            this._menuItem38.Text = "View Update Information";
            this._menuItem38.Click += new System.EventHandler(this.MenuItem38Click);
            // 
            // _menuItem5
            // 
            this._menuItem5.Index = 6;
            this._menuItem5.Text = "Go to &Website";
            this._menuItem5.Click += new System.EventHandler(this.MenuItem5Click);
            // 
            // _menuItem27
            // 
            this._menuItem27.Index = 7;
            this._menuItem27.Text = "-";
            // 
            // _menuItem26
            // 
            this._menuItem26.Index = 8;
            this._menuItem26.Text = "&Support iSpy With a Donation";
            this._menuItem26.Click += new System.EventHandler(this.MenuItem26Click);
            // 
            // ctxtMainForm
            // 
            this.ctxtMainForm.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ctxtMainForm.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._addCameraToolStripMenuItem,
            this._addMicrophoneToolStripMenuItem,
            this._addFloorPlanToolStripMenuItem,
            this._remoteCommandsToolStripMenuItem,
            this._settingsToolStripMenuItem,
            this._applyScheduleToolStripMenuItem,
            this.opacityToolStripMenuItem,
            this.layoutToolStripMenuItem,
            this.displayToolStripMenuItem,
            this.gridViewsToolStripMenuItem,
            this.maximiseToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.ctxtMainForm.Name = "_ctxtMainForm";
            this.ctxtMainForm.Size = new System.Drawing.Size(185, 316);
            this.ctxtMainForm.Opening += new System.ComponentModel.CancelEventHandler(this.CtxtMainFormOpening);
            // 
            // _addCameraToolStripMenuItem
            // 
            this._addCameraToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_addCameraToolStripMenuItem.Image")));
            this._addCameraToolStripMenuItem.Name = "_addCameraToolStripMenuItem";
            this._addCameraToolStripMenuItem.Size = new System.Drawing.Size(184, 26);
            this._addCameraToolStripMenuItem.Text = "Add &Camera";
            this._addCameraToolStripMenuItem.Click += new System.EventHandler(this.AddCameraToolStripMenuItemClick);
            // 
            // _addMicrophoneToolStripMenuItem
            // 
            this._addMicrophoneToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_addMicrophoneToolStripMenuItem.Image")));
            this._addMicrophoneToolStripMenuItem.Name = "_addMicrophoneToolStripMenuItem";
            this._addMicrophoneToolStripMenuItem.Size = new System.Drawing.Size(184, 26);
            this._addMicrophoneToolStripMenuItem.Text = "Add &Microphone";
            this._addMicrophoneToolStripMenuItem.Click += new System.EventHandler(this.AddMicrophoneToolStripMenuItemClick);
            // 
            // _addFloorPlanToolStripMenuItem
            // 
            this._addFloorPlanToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_addFloorPlanToolStripMenuItem.Image")));
            this._addFloorPlanToolStripMenuItem.Name = "_addFloorPlanToolStripMenuItem";
            this._addFloorPlanToolStripMenuItem.Size = new System.Drawing.Size(184, 26);
            this._addFloorPlanToolStripMenuItem.Text = "Add Floor &Plan";
            this._addFloorPlanToolStripMenuItem.Click += new System.EventHandler(this.AddFloorPlanToolStripMenuItemClick);
            // 
            // _remoteCommandsToolStripMenuItem
            // 
            this._remoteCommandsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_remoteCommandsToolStripMenuItem.Image")));
            this._remoteCommandsToolStripMenuItem.Name = "_remoteCommandsToolStripMenuItem";
            this._remoteCommandsToolStripMenuItem.Size = new System.Drawing.Size(184, 26);
            this._remoteCommandsToolStripMenuItem.Text = "Remote Commands";
            this._remoteCommandsToolStripMenuItem.Click += new System.EventHandler(this.RemoteCommandsToolStripMenuItemClick);
            // 
            // _settingsToolStripMenuItem
            // 
            this._settingsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_settingsToolStripMenuItem.Image")));
            this._settingsToolStripMenuItem.Name = "_settingsToolStripMenuItem";
            this._settingsToolStripMenuItem.Size = new System.Drawing.Size(184, 26);
            this._settingsToolStripMenuItem.Text = "&Settings";
            this._settingsToolStripMenuItem.Click += new System.EventHandler(this.SettingsToolStripMenuItemClick);
            // 
            // _applyScheduleToolStripMenuItem
            // 
            this._applyScheduleToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_applyScheduleToolStripMenuItem.Image")));
            this._applyScheduleToolStripMenuItem.Name = "_applyScheduleToolStripMenuItem";
            this._applyScheduleToolStripMenuItem.Size = new System.Drawing.Size(184, 26);
            this._applyScheduleToolStripMenuItem.Text = "Apply Schedule";
            this._applyScheduleToolStripMenuItem.Click += new System.EventHandler(this.ApplyScheduleToolStripMenuItemClick1);
            // 
            // opacityToolStripMenuItem
            // 
            this.opacityToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.opacityToolStripMenuItem1,
            this.opacityToolStripMenuItem2,
            this.opacityToolStripMenuItem3});
            this.opacityToolStripMenuItem.Name = "opacityToolStripMenuItem";
            this.opacityToolStripMenuItem.Size = new System.Drawing.Size(184, 26);
            this.opacityToolStripMenuItem.Text = "Opacity";
            // 
            // opacityToolStripMenuItem1
            // 
            this.opacityToolStripMenuItem1.Name = "opacityToolStripMenuItem1";
            this.opacityToolStripMenuItem1.Size = new System.Drawing.Size(146, 22);
            this.opacityToolStripMenuItem1.Text = "10% Opacity";
            this.opacityToolStripMenuItem1.Click += new System.EventHandler(this.opacityToolStripMenuItem1_Click);
            // 
            // opacityToolStripMenuItem2
            // 
            this.opacityToolStripMenuItem2.Name = "opacityToolStripMenuItem2";
            this.opacityToolStripMenuItem2.Size = new System.Drawing.Size(146, 22);
            this.opacityToolStripMenuItem2.Text = "30% Opacity";
            this.opacityToolStripMenuItem2.Click += new System.EventHandler(this.opacityToolStripMenuItem2_Click);
            // 
            // opacityToolStripMenuItem3
            // 
            this.opacityToolStripMenuItem3.Name = "opacityToolStripMenuItem3";
            this.opacityToolStripMenuItem3.Size = new System.Drawing.Size(146, 22);
            this.opacityToolStripMenuItem3.Text = "100% Opacity";
            this.opacityToolStripMenuItem3.Click += new System.EventHandler(this.opacityToolStripMenuItem3_Click);
            // 
            // layoutToolStripMenuItem
            // 
            this.layoutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoLayoutToolStripMenuItem,
            this.saveLayoutToolStripMenuItem1,
            this.resetLayoutToolStripMenuItem1});
            this.layoutToolStripMenuItem.Name = "layoutToolStripMenuItem";
            this.layoutToolStripMenuItem.Size = new System.Drawing.Size(184, 26);
            this.layoutToolStripMenuItem.Text = "Layout";
            // 
            // autoLayoutToolStripMenuItem
            // 
            this.autoLayoutToolStripMenuItem.Name = "autoLayoutToolStripMenuItem";
            this.autoLayoutToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.autoLayoutToolStripMenuItem.Text = "Auto Layout";
            this.autoLayoutToolStripMenuItem.Click += new System.EventHandler(this.autoLayoutToolStripMenuItem_Click);
            // 
            // saveLayoutToolStripMenuItem1
            // 
            this.saveLayoutToolStripMenuItem1.Name = "saveLayoutToolStripMenuItem1";
            this.saveLayoutToolStripMenuItem1.Size = new System.Drawing.Size(141, 22);
            this.saveLayoutToolStripMenuItem1.Text = "Save Layout";
            this.saveLayoutToolStripMenuItem1.Click += new System.EventHandler(this.saveLayoutToolStripMenuItem1_Click);
            // 
            // resetLayoutToolStripMenuItem1
            // 
            this.resetLayoutToolStripMenuItem1.Name = "resetLayoutToolStripMenuItem1";
            this.resetLayoutToolStripMenuItem1.Size = new System.Drawing.Size(141, 22);
            this.resetLayoutToolStripMenuItem1.Text = "Reset Layout";
            this.resetLayoutToolStripMenuItem1.Click += new System.EventHandler(this.resetLayoutToolStripMenuItem1_Click);
            // 
            // displayToolStripMenuItem
            // 
            this.displayToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fullScreenToolStripMenuItem1,
            this.statusBarToolStripMenuItem,
            this.fileMenuToolStripMenuItem,
            this.toolStripToolStripMenuItem,
            this.mediaPaneToolStripMenuItem,
            this.pTZControllerToolStripMenuItem,
            this.viewControllerToolStripMenuItem,
            this.alwaysOnTopToolStripMenuItem1});
            this.displayToolStripMenuItem.Name = "displayToolStripMenuItem";
            this.displayToolStripMenuItem.Size = new System.Drawing.Size(184, 26);
            this.displayToolStripMenuItem.Text = "Display";
            // 
            // fullScreenToolStripMenuItem1
            // 
            this.fullScreenToolStripMenuItem1.Checked = true;
            this.fullScreenToolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.fullScreenToolStripMenuItem1.Name = "fullScreenToolStripMenuItem1";
            this.fullScreenToolStripMenuItem1.Size = new System.Drawing.Size(155, 22);
            this.fullScreenToolStripMenuItem1.Text = "Full Screen";
            this.fullScreenToolStripMenuItem1.Click += new System.EventHandler(this.fullScreenToolStripMenuItem1_Click);
            // 
            // statusBarToolStripMenuItem
            // 
            this.statusBarToolStripMenuItem.Checked = true;
            this.statusBarToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.statusBarToolStripMenuItem.Name = "statusBarToolStripMenuItem";
            this.statusBarToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.statusBarToolStripMenuItem.Text = "Status Bar";
            this.statusBarToolStripMenuItem.Click += new System.EventHandler(this.statusBarToolStripMenuItem_Click);
            // 
            // fileMenuToolStripMenuItem
            // 
            this.fileMenuToolStripMenuItem.Checked = true;
            this.fileMenuToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.fileMenuToolStripMenuItem.Name = "fileMenuToolStripMenuItem";
            this.fileMenuToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.fileMenuToolStripMenuItem.Text = "File Menu";
            this.fileMenuToolStripMenuItem.Click += new System.EventHandler(this.fileMenuToolStripMenuItem_Click);
            // 
            // toolStripToolStripMenuItem
            // 
            this.toolStripToolStripMenuItem.Checked = true;
            this.toolStripToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripToolStripMenuItem.Name = "toolStripToolStripMenuItem";
            this.toolStripToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.toolStripToolStripMenuItem.Text = "Tool Strip";
            this.toolStripToolStripMenuItem.Click += new System.EventHandler(this.toolStripToolStripMenuItem_Click);
            // 
            // mediaPaneToolStripMenuItem
            // 
            this.mediaPaneToolStripMenuItem.Checked = true;
            this.mediaPaneToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mediaPaneToolStripMenuItem.Name = "mediaPaneToolStripMenuItem";
            this.mediaPaneToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.mediaPaneToolStripMenuItem.Text = "Media Pane";
            this.mediaPaneToolStripMenuItem.Click += new System.EventHandler(this.mediaPaneToolStripMenuItem_Click);
            // 
            // pTZControllerToolStripMenuItem
            // 
            this.pTZControllerToolStripMenuItem.Checked = true;
            this.pTZControllerToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.pTZControllerToolStripMenuItem.Name = "pTZControllerToolStripMenuItem";
            this.pTZControllerToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.pTZControllerToolStripMenuItem.Text = "PTZ Controller";
            this.pTZControllerToolStripMenuItem.Click += new System.EventHandler(this.pTZControllerToolStripMenuItem_Click);
            // 
            // viewControllerToolStripMenuItem
            // 
            this.viewControllerToolStripMenuItem.Checked = true;
            this.viewControllerToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.viewControllerToolStripMenuItem.Name = "viewControllerToolStripMenuItem";
            this.viewControllerToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.viewControllerToolStripMenuItem.Text = "View Controller";
            this.viewControllerToolStripMenuItem.Click += new System.EventHandler(this.viewControllerToolStripMenuItem_Click);
            // 
            // alwaysOnTopToolStripMenuItem1
            // 
            this.alwaysOnTopToolStripMenuItem1.Checked = true;
            this.alwaysOnTopToolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.alwaysOnTopToolStripMenuItem1.Name = "alwaysOnTopToolStripMenuItem1";
            this.alwaysOnTopToolStripMenuItem1.Size = new System.Drawing.Size(155, 22);
            this.alwaysOnTopToolStripMenuItem1.Text = "Always on Top";
            this.alwaysOnTopToolStripMenuItem1.Click += new System.EventHandler(this.alwaysOnTopToolStripMenuItem1_Click);
            // 
            // gridViewsToolStripMenuItem
            // 
            this.gridViewsToolStripMenuItem.Name = "gridViewsToolStripMenuItem";
            this.gridViewsToolStripMenuItem.Size = new System.Drawing.Size(184, 26);
            this.gridViewsToolStripMenuItem.Text = "Grid Views";
            this.gridViewsToolStripMenuItem.Click += new System.EventHandler(this.gridViewsToolStripMenuItem_Click);
            // 
            // maximiseToolStripMenuItem
            // 
            this.maximiseToolStripMenuItem.Name = "maximiseToolStripMenuItem";
            this.maximiseToolStripMenuItem.Size = new System.Drawing.Size(184, 26);
            this.maximiseToolStripMenuItem.Text = "Maximise";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(184, 26);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolStripMenu
            // 
            this.toolStripMenu.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._toolStripDropDownButton2,
            this._toolStripDropDownButton1,
            this.tssbGridViews,
            this._toolStripButton8,
            this._toolStripButton1,
            this.tsbPlugins,
            this._toolStripButton4});
            this.toolStripMenu.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStripMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.Size = new System.Drawing.Size(887, 39);
            this.toolStripMenu.TabIndex = 0;
            this.toolStripMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ToolStrip1ItemClicked);
            // 
            // _toolStripDropDownButton2
            // 
            this._toolStripDropDownButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._localCameraToolStripMenuItem,
            this._iPCameraToolStripMenuItem,
            this.iPCameraWithWizardToolStripMenuItem,
            this.oNVIFCameraToolStripMenuItem,
            this._floorPlanToolStripMenuItem,
            this.videoFileToolStripMenuItem,
            this.otherVideoSourceToolStripMenuItem,
            this._microphoneToolStripMenuItem,
            this._uSbCamerasAndMicrophonesOnOtherToolStripMenuItem});
            this._toolStripDropDownButton2.Image = global::iSpyApplication.Properties.Resources.DownloadProgram;
            this._toolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolStripDropDownButton2.Name = "_toolStripDropDownButton2";
            this._toolStripDropDownButton2.ShowDropDownArrow = false;
            this._toolStripDropDownButton2.Size = new System.Drawing.Size(74, 36);
            this._toolStripDropDownButton2.Text = "Add...";
            this._toolStripDropDownButton2.Click += new System.EventHandler(this._toolStripDropDownButton2_Click);
            // 
            // _localCameraToolStripMenuItem
            // 
            this._localCameraToolStripMenuItem.Image = global::iSpyApplication.Properties.Resources.Music;
            this._localCameraToolStripMenuItem.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this._localCameraToolStripMenuItem.Name = "_localCameraToolStripMenuItem";
            this._localCameraToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0);
            this._localCameraToolStripMenuItem.Size = new System.Drawing.Size(331, 20);
            this._localCameraToolStripMenuItem.Text = "Local Camera";
            this._localCameraToolStripMenuItem.Click += new System.EventHandler(this.LocalCameraToolStripMenuItemClick);
            // 
            // _iPCameraToolStripMenuItem
            // 
            this._iPCameraToolStripMenuItem.Image = global::iSpyApplication.Properties.Resources.Computer;
            this._iPCameraToolStripMenuItem.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this._iPCameraToolStripMenuItem.Name = "_iPCameraToolStripMenuItem";
            this._iPCameraToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0);
            this._iPCameraToolStripMenuItem.Size = new System.Drawing.Size(331, 20);
            this._iPCameraToolStripMenuItem.Text = "IP Camera";
            this._iPCameraToolStripMenuItem.Click += new System.EventHandler(this.IpCameraToolStripMenuItemClick);
            // 
            // iPCameraWithWizardToolStripMenuItem
            // 
            this.iPCameraWithWizardToolStripMenuItem.Image = global::iSpyApplication.Properties.Resources.Computer;
            this.iPCameraWithWizardToolStripMenuItem.Name = "iPCameraWithWizardToolStripMenuItem";
            this.iPCameraWithWizardToolStripMenuItem.Size = new System.Drawing.Size(331, 22);
            this.iPCameraWithWizardToolStripMenuItem.Text = "IP Camera With Wizard";
            this.iPCameraWithWizardToolStripMenuItem.Click += new System.EventHandler(this.iPCameraWithWizardToolStripMenuItem_Click);
            // 
            // oNVIFCameraToolStripMenuItem
            // 
            this.oNVIFCameraToolStripMenuItem.Image = global::iSpyApplication.Properties.Resources.onvif;
            this.oNVIFCameraToolStripMenuItem.Name = "oNVIFCameraToolStripMenuItem";
            this.oNVIFCameraToolStripMenuItem.Size = new System.Drawing.Size(331, 22);
            this.oNVIFCameraToolStripMenuItem.Text = "ONVIF Camera";
            this.oNVIFCameraToolStripMenuItem.Click += new System.EventHandler(this.oNVIFCameraToolStripMenuItem_Click);
            // 
            // _floorPlanToolStripMenuItem
            // 
            this._floorPlanToolStripMenuItem.Image = global::iSpyApplication.Properties.Resources.Default;
            this._floorPlanToolStripMenuItem.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this._floorPlanToolStripMenuItem.Name = "_floorPlanToolStripMenuItem";
            this._floorPlanToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0);
            this._floorPlanToolStripMenuItem.Size = new System.Drawing.Size(331, 20);
            this._floorPlanToolStripMenuItem.Text = "Floor Plan";
            this._floorPlanToolStripMenuItem.Click += new System.EventHandler(this.FloorPlanToolStripMenuItemClick);
            // 
            // videoFileToolStripMenuItem
            // 
            this.videoFileToolStripMenuItem.Image = global::iSpyApplication.Properties.Resources.HardDrive;
            this.videoFileToolStripMenuItem.Name = "videoFileToolStripMenuItem";
            this.videoFileToolStripMenuItem.Size = new System.Drawing.Size(331, 22);
            this.videoFileToolStripMenuItem.Text = "Video File";
            this.videoFileToolStripMenuItem.Click += new System.EventHandler(this.videoFileToolStripMenuItem_Click);
            // 
            // otherVideoSourceToolStripMenuItem
            // 
            this.otherVideoSourceToolStripMenuItem.Image = global::iSpyApplication.Properties.Resources.Music;
            this.otherVideoSourceToolStripMenuItem.Name = "otherVideoSourceToolStripMenuItem";
            this.otherVideoSourceToolStripMenuItem.Size = new System.Drawing.Size(331, 22);
            this.otherVideoSourceToolStripMenuItem.Text = "Other Video Source";
            this.otherVideoSourceToolStripMenuItem.Click += new System.EventHandler(this.otherVideoSourceToolStripMenuItem_Click);
            // 
            // _microphoneToolStripMenuItem
            // 
            this._microphoneToolStripMenuItem.Image = global::iSpyApplication.Properties.Resources.EMail;
            this._microphoneToolStripMenuItem.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this._microphoneToolStripMenuItem.Name = "_microphoneToolStripMenuItem";
            this._microphoneToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0);
            this._microphoneToolStripMenuItem.Size = new System.Drawing.Size(331, 20);
            this._microphoneToolStripMenuItem.Text = "Microphone";
            this._microphoneToolStripMenuItem.Click += new System.EventHandler(this.MicrophoneToolStripMenuItemClick);
            // 
            // _uSbCamerasAndMicrophonesOnOtherToolStripMenuItem
            // 
            this._uSbCamerasAndMicrophonesOnOtherToolStripMenuItem.Image = global::iSpyApplication.Properties.Resources.Picture;
            this._uSbCamerasAndMicrophonesOnOtherToolStripMenuItem.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this._uSbCamerasAndMicrophonesOnOtherToolStripMenuItem.Name = "_uSbCamerasAndMicrophonesOnOtherToolStripMenuItem";
            this._uSbCamerasAndMicrophonesOnOtherToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0);
            this._uSbCamerasAndMicrophonesOnOtherToolStripMenuItem.Size = new System.Drawing.Size(331, 20);
            this._uSbCamerasAndMicrophonesOnOtherToolStripMenuItem.Text = "Cameras and Microphones on Other Computers ";
            this._uSbCamerasAndMicrophonesOnOtherToolStripMenuItem.Click += new System.EventHandler(this.USbCamerasAndMicrophonesOnOtherToolStripMenuItemClick);
            // 
            // _toolStripDropDownButton1
            // 
            this._toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._thruWebsiteToolStripMenuItem,
            this._onMobileDevicesToolStripMenuItem,
            this.inExplorerToolStripMenuItem});
            this._toolStripDropDownButton1.Image = global::iSpyApplication.Properties.Resources.Video2;
            this._toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolStripDropDownButton1.Name = "_toolStripDropDownButton1";
            this._toolStripDropDownButton1.ShowDropDownArrow = false;
            this._toolStripDropDownButton1.Size = new System.Drawing.Size(115, 36);
            this._toolStripDropDownButton1.Text = "Access Media";
            this._toolStripDropDownButton1.Click += new System.EventHandler(this.ToolStripDropDownButton1Click);
            // 
            // _thruWebsiteToolStripMenuItem
            // 
            this._thruWebsiteToolStripMenuItem.Image = global::iSpyApplication.Properties.Resources.Firefox;
            this._thruWebsiteToolStripMenuItem.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this._thruWebsiteToolStripMenuItem.Name = "_thruWebsiteToolStripMenuItem";
            this._thruWebsiteToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0);
            this._thruWebsiteToolStripMenuItem.Size = new System.Drawing.Size(154, 20);
            this._thruWebsiteToolStripMenuItem.Text = "Online";
            this._thruWebsiteToolStripMenuItem.Click += new System.EventHandler(this.ThruWebsiteToolStripMenuItemClick);
            // 
            // _onMobileDevicesToolStripMenuItem
            // 
            this._onMobileDevicesToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_onMobileDevicesToolStripMenuItem.Image")));
            this._onMobileDevicesToolStripMenuItem.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this._onMobileDevicesToolStripMenuItem.Name = "_onMobileDevicesToolStripMenuItem";
            this._onMobileDevicesToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0);
            this._onMobileDevicesToolStripMenuItem.Size = new System.Drawing.Size(154, 20);
            this._onMobileDevicesToolStripMenuItem.Text = "Mobile Devices";
            this._onMobileDevicesToolStripMenuItem.Click += new System.EventHandler(this.OnMobileDevicesToolStripMenuItemClick);
            // 
            // inExplorerToolStripMenuItem
            // 
            this.inExplorerToolStripMenuItem.Image = global::iSpyApplication.Properties.Resources.HardDrive;
            this.inExplorerToolStripMenuItem.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.inExplorerToolStripMenuItem.Name = "inExplorerToolStripMenuItem";
            this.inExplorerToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.inExplorerToolStripMenuItem.Text = "Files";
            this.inExplorerToolStripMenuItem.Click += new System.EventHandler(this.inExplorerToolStripMenuItem_Click);
            // 
            // tssbGridViews
            // 
            this.tssbGridViews.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manageToolStripMenuItem});
            this.tssbGridViews.Image = global::iSpyApplication.Properties.Resources.Darkfix;
            this.tssbGridViews.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tssbGridViews.Name = "tssbGridViews";
            this.tssbGridViews.ShowDropDownArrow = false;
            this.tssbGridViews.Size = new System.Drawing.Size(98, 36);
            this.tssbGridViews.Text = "Grid Views";
            // 
            // manageToolStripMenuItem
            // 
            this.manageToolStripMenuItem.Name = "manageToolStripMenuItem";
            this.manageToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.manageToolStripMenuItem.Text = "Manage";
            this.manageToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // _toolStripButton8
            // 
            this._toolStripButton8.Image = global::iSpyApplication.Properties.Resources.Run;
            this._toolStripButton8.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolStripButton8.Name = "_toolStripButton8";
            this._toolStripButton8.Size = new System.Drawing.Size(105, 36);
            this._toolStripButton8.Text = "Commands";
            this._toolStripButton8.Click += new System.EventHandler(this.ToolStripButton8Click1);
            // 
            // _toolStripButton1
            // 
            this._toolStripButton1.Image = global::iSpyApplication.Properties.Resources.Network;
            this._toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolStripButton1.Name = "_toolStripButton1";
            this._toolStripButton1.Size = new System.Drawing.Size(112, 36);
            this._toolStripButton1.Text = "Web Settings";
            this._toolStripButton1.Click += new System.EventHandler(this.ToolStripButton1Click1);
            // 
            // tsbPlugins
            // 
            this.tsbPlugins.Image = global::iSpyApplication.Properties.Resources.Desktop;
            this.tsbPlugins.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPlugins.Name = "tsbPlugins";
            this.tsbPlugins.Size = new System.Drawing.Size(82, 36);
            this.tsbPlugins.Text = "Plugins";
            this.tsbPlugins.Click += new System.EventHandler(this.tsbPlugins_Click);
            // 
            // _toolStripButton4
            // 
            this._toolStripButton4.Image = global::iSpyApplication.Properties.Resources.ControlPanel;
            this._toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolStripButton4.Name = "_toolStripButton4";
            this._toolStripButton4.Size = new System.Drawing.Size(85, 36);
            this._toolStripButton4.Text = "Settings";
            this._toolStripButton4.Click += new System.EventHandler(this.ToolStripButton4Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "iSpy";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.Click += new System.EventHandler(this.NotifyIcon1Click);
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.NotifyIcon1DoubleClick);
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // ctxtMnu
            // 
            this.ctxtMnu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ctxtMnu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pluginCommandsToolStripMenuItem,
            this.openWebInterfaceToolStripMenuItem,
            this._viewMediaToolStripMenuItem,
            this._viewMediaOnAMobileDeviceToolStripMenuItem,
            this.switchToolStripMenuItem,
            this._recordNowToolStripMenuItem,
            this._takePhotoToolStripMenuItem,
            this.pTZToolStripMenuItem,
            this._listenToolStripMenuItem,
            this._editToolStripMenuItem,
            this.tagsToolStripMenuItem,
            this._applyScheduleToolStripMenuItem1,
            this._positionToolStripMenuItem,
            this.fullScreenToolStripMenuItem,
            this._resetSizeToolStripMenuItem,
            this._resetRecordingCounterToolStripMenuItem,
            this._showFilesToolStripMenuItem,
            this._deleteToolStripMenuItem});
            this.ctxtMnu.Name = "_ctxtMnu";
            this.ctxtMnu.Size = new System.Drawing.Size(244, 472);
            this.ctxtMnu.Opening += new System.ComponentModel.CancelEventHandler(this.ctxtMnu_Opening);
            // 
            // pluginCommandsToolStripMenuItem
            // 
            this.pluginCommandsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configurePluginToolStripMenuItem});
            this.pluginCommandsToolStripMenuItem.Image = global::iSpyApplication.Properties.Resources.plugin;
            this.pluginCommandsToolStripMenuItem.Name = "pluginCommandsToolStripMenuItem";
            this.pluginCommandsToolStripMenuItem.Size = new System.Drawing.Size(243, 26);
            this.pluginCommandsToolStripMenuItem.Text = "Plugin";
            // 
            // configurePluginToolStripMenuItem
            // 
            this.configurePluginToolStripMenuItem.Name = "configurePluginToolStripMenuItem";
            this.configurePluginToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.configurePluginToolStripMenuItem.Text = "Configure Plugin";
            this.configurePluginToolStripMenuItem.Click += new System.EventHandler(this.configurePluginToolStripMenuItem_Click);
            // 
            // openWebInterfaceToolStripMenuItem
            // 
            this.openWebInterfaceToolStripMenuItem.Image = global::iSpyApplication.Properties.Resources.web;
            this.openWebInterfaceToolStripMenuItem.Name = "openWebInterfaceToolStripMenuItem";
            this.openWebInterfaceToolStripMenuItem.Size = new System.Drawing.Size(243, 26);
            this.openWebInterfaceToolStripMenuItem.Text = "Open Web Interface";
            this.openWebInterfaceToolStripMenuItem.Click += new System.EventHandler(this.openWebInterfaceToolStripMenuItem_Click);
            // 
            // _viewMediaToolStripMenuItem
            // 
            this._viewMediaToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_viewMediaToolStripMenuItem.Image")));
            this._viewMediaToolStripMenuItem.Name = "_viewMediaToolStripMenuItem";
            this._viewMediaToolStripMenuItem.Size = new System.Drawing.Size(243, 26);
            this._viewMediaToolStripMenuItem.Text = "View &Media ";
            this._viewMediaToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem1Click);
            // 
            // _viewMediaOnAMobileDeviceToolStripMenuItem
            // 
            this._viewMediaOnAMobileDeviceToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_viewMediaOnAMobileDeviceToolStripMenuItem.Image")));
            this._viewMediaOnAMobileDeviceToolStripMenuItem.Name = "_viewMediaOnAMobileDeviceToolStripMenuItem";
            this._viewMediaOnAMobileDeviceToolStripMenuItem.Size = new System.Drawing.Size(243, 26);
            this._viewMediaOnAMobileDeviceToolStripMenuItem.Text = "View Media on a Mobile &Device";
            this._viewMediaOnAMobileDeviceToolStripMenuItem.Click += new System.EventHandler(this.ViewMediaOnAMobileDeviceToolStripMenuItemClick);
            // 
            // switchToolStripMenuItem
            // 
            this.switchToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.onToolStripMenuItem,
            this.offToolStripMenuItem,
            this.alertsOnToolStripMenuItem1,
            this.alertsOffToolStripMenuItem,
            this.scheduleOnToolStripMenuItem,
            this.scheduleOffToolStripMenuItem,
            this.pTZScheduleOnToolStripMenuItem,
            this.pTZScheduleOffToolStripMenuItem});
            this.switchToolStripMenuItem.Image = global::iSpyApplication.Properties.Resources._switch;
            this.switchToolStripMenuItem.Name = "switchToolStripMenuItem";
            this.switchToolStripMenuItem.Size = new System.Drawing.Size(243, 26);
            this.switchToolStripMenuItem.Text = "Switch";
            // 
            // onToolStripMenuItem
            // 
            this.onToolStripMenuItem.Name = "onToolStripMenuItem";
            this.onToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.onToolStripMenuItem.Text = "On";
            this.onToolStripMenuItem.Click += new System.EventHandler(this.onToolStripMenuItem_Click);
            // 
            // offToolStripMenuItem
            // 
            this.offToolStripMenuItem.Name = "offToolStripMenuItem";
            this.offToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.offToolStripMenuItem.Text = "Off";
            this.offToolStripMenuItem.Click += new System.EventHandler(this.offToolStripMenuItem_Click);
            // 
            // alertsOnToolStripMenuItem1
            // 
            this.alertsOnToolStripMenuItem1.Name = "alertsOnToolStripMenuItem1";
            this.alertsOnToolStripMenuItem1.Size = new System.Drawing.Size(166, 22);
            this.alertsOnToolStripMenuItem1.Text = "Alerts On";
            this.alertsOnToolStripMenuItem1.Click += new System.EventHandler(this.alertsOnToolStripMenuItem1_Click);
            // 
            // alertsOffToolStripMenuItem
            // 
            this.alertsOffToolStripMenuItem.Name = "alertsOffToolStripMenuItem";
            this.alertsOffToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.alertsOffToolStripMenuItem.Text = "Alerts Off";
            this.alertsOffToolStripMenuItem.Click += new System.EventHandler(this.alertsOffToolStripMenuItem_Click);
            // 
            // scheduleOnToolStripMenuItem
            // 
            this.scheduleOnToolStripMenuItem.Name = "scheduleOnToolStripMenuItem";
            this.scheduleOnToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.scheduleOnToolStripMenuItem.Text = "Schedule On";
            this.scheduleOnToolStripMenuItem.Click += new System.EventHandler(this.scheduleOnToolStripMenuItem_Click);
            // 
            // scheduleOffToolStripMenuItem
            // 
            this.scheduleOffToolStripMenuItem.Name = "scheduleOffToolStripMenuItem";
            this.scheduleOffToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.scheduleOffToolStripMenuItem.Text = "Schedule Off";
            this.scheduleOffToolStripMenuItem.Click += new System.EventHandler(this.scheduleOffToolStripMenuItem_Click);
            // 
            // pTZScheduleOnToolStripMenuItem
            // 
            this.pTZScheduleOnToolStripMenuItem.Name = "pTZScheduleOnToolStripMenuItem";
            this.pTZScheduleOnToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.pTZScheduleOnToolStripMenuItem.Text = "PTZ Schedule On";
            this.pTZScheduleOnToolStripMenuItem.Click += new System.EventHandler(this.pTZScheduleOnToolStripMenuItem_Click);
            // 
            // pTZScheduleOffToolStripMenuItem
            // 
            this.pTZScheduleOffToolStripMenuItem.Name = "pTZScheduleOffToolStripMenuItem";
            this.pTZScheduleOffToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.pTZScheduleOffToolStripMenuItem.Text = "PTZ Schedule Off";
            this.pTZScheduleOffToolStripMenuItem.Click += new System.EventHandler(this.pTZScheduleOffToolStripMenuItem_Click);
            // 
            // _recordNowToolStripMenuItem
            // 
            this._recordNowToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_recordNowToolStripMenuItem.Image")));
            this._recordNowToolStripMenuItem.Name = "_recordNowToolStripMenuItem";
            this._recordNowToolStripMenuItem.Size = new System.Drawing.Size(243, 26);
            this._recordNowToolStripMenuItem.Text = "Record Now";
            this._recordNowToolStripMenuItem.Click += new System.EventHandler(this.RecordNowToolStripMenuItemClick);
            // 
            // _takePhotoToolStripMenuItem
            // 
            this._takePhotoToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_takePhotoToolStripMenuItem.Image")));
            this._takePhotoToolStripMenuItem.Name = "_takePhotoToolStripMenuItem";
            this._takePhotoToolStripMenuItem.Size = new System.Drawing.Size(243, 26);
            this._takePhotoToolStripMenuItem.Text = "Take Photo";
            this._takePhotoToolStripMenuItem.Click += new System.EventHandler(this.TakePhotoToolStripMenuItemClick);
            // 
            // pTZToolStripMenuItem
            // 
            this.pTZToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pTZControllerToolStripMenuItem1});
            this.pTZToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pTZToolStripMenuItem.Image")));
            this.pTZToolStripMenuItem.Name = "pTZToolStripMenuItem";
            this.pTZToolStripMenuItem.Size = new System.Drawing.Size(243, 26);
            this.pTZToolStripMenuItem.Text = "PTZ";
            this.pTZToolStripMenuItem.Click += new System.EventHandler(this.pTZToolStripMenuItem_Click);
            // 
            // pTZControllerToolStripMenuItem1
            // 
            this.pTZControllerToolStripMenuItem1.Name = "pTZControllerToolStripMenuItem1";
            this.pTZControllerToolStripMenuItem1.Size = new System.Drawing.Size(151, 22);
            this.pTZControllerToolStripMenuItem1.Text = "PTZ Controller";
            this.pTZControllerToolStripMenuItem1.Click += new System.EventHandler(this.pTZControllerToolStripMenuItem1_Click);
            // 
            // _listenToolStripMenuItem
            // 
            this._listenToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_listenToolStripMenuItem.Image")));
            this._listenToolStripMenuItem.Name = "_listenToolStripMenuItem";
            this._listenToolStripMenuItem.Size = new System.Drawing.Size(243, 26);
            this._listenToolStripMenuItem.Text = "Listen";
            this._listenToolStripMenuItem.Click += new System.EventHandler(this.ListenToolStripMenuItemClick);
            // 
            // _editToolStripMenuItem
            // 
            this._editToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_editToolStripMenuItem.Image")));
            this._editToolStripMenuItem.Name = "_editToolStripMenuItem";
            this._editToolStripMenuItem.Size = new System.Drawing.Size(243, 26);
            this._editToolStripMenuItem.Text = "&Edit";
            this._editToolStripMenuItem.Click += new System.EventHandler(this.EditToolStripMenuItemClick);
            // 
            // tagsToolStripMenuItem
            // 
            this.tagsToolStripMenuItem.Image = global::iSpyApplication.Properties.Resources.edit;
            this.tagsToolStripMenuItem.Name = "tagsToolStripMenuItem";
            this.tagsToolStripMenuItem.Size = new System.Drawing.Size(243, 26);
            this.tagsToolStripMenuItem.Text = "Tags";
            this.tagsToolStripMenuItem.Click += new System.EventHandler(this.tagsToolStripMenuItem_Click);
            // 
            // _applyScheduleToolStripMenuItem1
            // 
            this._applyScheduleToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("_applyScheduleToolStripMenuItem1.Image")));
            this._applyScheduleToolStripMenuItem1.Name = "_applyScheduleToolStripMenuItem1";
            this._applyScheduleToolStripMenuItem1.Size = new System.Drawing.Size(243, 26);
            this._applyScheduleToolStripMenuItem1.Text = "Apply Schedule";
            this._applyScheduleToolStripMenuItem1.Click += new System.EventHandler(this.ApplyScheduleToolStripMenuItem1Click);
            // 
            // _positionToolStripMenuItem
            // 
            this._positionToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_positionToolStripMenuItem.Image")));
            this._positionToolStripMenuItem.Name = "_positionToolStripMenuItem";
            this._positionToolStripMenuItem.Size = new System.Drawing.Size(243, 26);
            this._positionToolStripMenuItem.Text = "Position";
            this._positionToolStripMenuItem.Click += new System.EventHandler(this.PositionToolStripMenuItemClick);
            // 
            // fullScreenToolStripMenuItem
            // 
            this.fullScreenToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("fullScreenToolStripMenuItem.Image")));
            this.fullScreenToolStripMenuItem.Name = "fullScreenToolStripMenuItem";
            this.fullScreenToolStripMenuItem.Size = new System.Drawing.Size(243, 26);
            this.fullScreenToolStripMenuItem.Text = "Full Screen";
            this.fullScreenToolStripMenuItem.Click += new System.EventHandler(this.fullScreenToolStripMenuItem_Click);
            // 
            // _resetSizeToolStripMenuItem
            // 
            this._resetSizeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_resetSizeToolStripMenuItem.Image")));
            this._resetSizeToolStripMenuItem.Name = "_resetSizeToolStripMenuItem";
            this._resetSizeToolStripMenuItem.Size = new System.Drawing.Size(243, 26);
            this._resetSizeToolStripMenuItem.Text = "Reset Si&ze";
            this._resetSizeToolStripMenuItem.Click += new System.EventHandler(this.ResetSizeToolStripMenuItemClick);
            // 
            // _resetRecordingCounterToolStripMenuItem
            // 
            this._resetRecordingCounterToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_resetRecordingCounterToolStripMenuItem.Image")));
            this._resetRecordingCounterToolStripMenuItem.Name = "_resetRecordingCounterToolStripMenuItem";
            this._resetRecordingCounterToolStripMenuItem.Size = new System.Drawing.Size(243, 26);
            this._resetRecordingCounterToolStripMenuItem.Text = "Reset Recording Counter";
            this._resetRecordingCounterToolStripMenuItem.Click += new System.EventHandler(this.ResetRecordingCounterToolStripMenuItemClick);
            // 
            // _showFilesToolStripMenuItem
            // 
            this._showFilesToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_showFilesToolStripMenuItem.Image")));
            this._showFilesToolStripMenuItem.Name = "_showFilesToolStripMenuItem";
            this._showFilesToolStripMenuItem.Size = new System.Drawing.Size(243, 26);
            this._showFilesToolStripMenuItem.Text = "Show Files";
            this._showFilesToolStripMenuItem.Click += new System.EventHandler(this.ShowFilesToolStripMenuItemClick);
            // 
            // _deleteToolStripMenuItem
            // 
            this._deleteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_deleteToolStripMenuItem.Image")));
            this._deleteToolStripMenuItem.Name = "_deleteToolStripMenuItem";
            this._deleteToolStripMenuItem.Size = new System.Drawing.Size(243, 26);
            this._deleteToolStripMenuItem.Text = "&Remove";
            this._deleteToolStripMenuItem.Click += new System.EventHandler(this.DeleteToolStripMenuItemClick);
            // 
            // ctxtTaskbar
            // 
            this.ctxtTaskbar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ctxtTaskbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._unlockToolstripMenuItem,
            this._switchAllOnToolStripMenuItem,
            this._switchAllOffToolStripMenuItem,
            this._showToolstripMenuItem,
            this._showISpy10PercentOpacityToolStripMenuItem,
            this._showISpy30OpacityToolStripMenuItem,
            this._showISpy100PercentOpacityToolStripMenuItem,
            this._helpToolstripMenuItem,
            this._websiteToolstripMenuItem,
            this.gridViewsToolStripMenuItem1,
            this.viewLogFileToolStripMenuItem,
            this._exitToolStripMenuItem});
            this.ctxtTaskbar.Name = "_ctxtMnu";
            this.ctxtTaskbar.Size = new System.Drawing.Size(223, 316);
            this.ctxtTaskbar.Opening += new System.ComponentModel.CancelEventHandler(this.CtxtTaskbarOpening);
            // 
            // _unlockToolstripMenuItem
            // 
            this._unlockToolstripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_unlockToolstripMenuItem.Image")));
            this._unlockToolstripMenuItem.Name = "_unlockToolstripMenuItem";
            this._unlockToolstripMenuItem.Size = new System.Drawing.Size(222, 26);
            this._unlockToolstripMenuItem.Text = "&Unlock";
            this._unlockToolstripMenuItem.Click += new System.EventHandler(this.UnlockToolstripMenuItemClick);
            // 
            // _switchAllOnToolStripMenuItem
            // 
            this._switchAllOnToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_switchAllOnToolStripMenuItem.Image")));
            this._switchAllOnToolStripMenuItem.Name = "_switchAllOnToolStripMenuItem";
            this._switchAllOnToolStripMenuItem.Size = new System.Drawing.Size(222, 26);
            this._switchAllOnToolStripMenuItem.Text = "Switch All On";
            this._switchAllOnToolStripMenuItem.Click += new System.EventHandler(this.SwitchAllOnToolStripMenuItemClick);
            // 
            // _switchAllOffToolStripMenuItem
            // 
            this._switchAllOffToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_switchAllOffToolStripMenuItem.Image")));
            this._switchAllOffToolStripMenuItem.Name = "_switchAllOffToolStripMenuItem";
            this._switchAllOffToolStripMenuItem.Size = new System.Drawing.Size(222, 26);
            this._switchAllOffToolStripMenuItem.Text = "Switch All Off";
            this._switchAllOffToolStripMenuItem.Click += new System.EventHandler(this.SwitchAllOffToolStripMenuItemClick);
            // 
            // _showToolstripMenuItem
            // 
            this._showToolstripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_showToolstripMenuItem.Image")));
            this._showToolstripMenuItem.Name = "_showToolstripMenuItem";
            this._showToolstripMenuItem.Size = new System.Drawing.Size(222, 26);
            this._showToolstripMenuItem.Text = "&Show iSpy";
            this._showToolstripMenuItem.Click += new System.EventHandler(this.ShowToolstripMenuItemClick);
            // 
            // _showISpy10PercentOpacityToolStripMenuItem
            // 
            this._showISpy10PercentOpacityToolStripMenuItem.Name = "_showISpy10PercentOpacityToolStripMenuItem";
            this._showISpy10PercentOpacityToolStripMenuItem.Size = new System.Drawing.Size(222, 26);
            this._showISpy10PercentOpacityToolStripMenuItem.Text = "Show iSpy @ 10% opacity";
            this._showISpy10PercentOpacityToolStripMenuItem.Click += new System.EventHandler(this.ShowISpy10PercentOpacityToolStripMenuItemClick);
            // 
            // _showISpy30OpacityToolStripMenuItem
            // 
            this._showISpy30OpacityToolStripMenuItem.Name = "_showISpy30OpacityToolStripMenuItem";
            this._showISpy30OpacityToolStripMenuItem.Size = new System.Drawing.Size(222, 26);
            this._showISpy30OpacityToolStripMenuItem.Text = "Show iSpy @ 30% opacity";
            this._showISpy30OpacityToolStripMenuItem.Click += new System.EventHandler(this.ShowISpy30OpacityToolStripMenuItemClick);
            // 
            // _showISpy100PercentOpacityToolStripMenuItem
            // 
            this._showISpy100PercentOpacityToolStripMenuItem.Name = "_showISpy100PercentOpacityToolStripMenuItem";
            this._showISpy100PercentOpacityToolStripMenuItem.Size = new System.Drawing.Size(222, 26);
            this._showISpy100PercentOpacityToolStripMenuItem.Text = "Show iSpy @ 100 % opacity";
            this._showISpy100PercentOpacityToolStripMenuItem.Click += new System.EventHandler(this.ShowISpy100PercentOpacityToolStripMenuItemClick);
            // 
            // _helpToolstripMenuItem
            // 
            this._helpToolstripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_helpToolstripMenuItem.Image")));
            this._helpToolstripMenuItem.Name = "_helpToolstripMenuItem";
            this._helpToolstripMenuItem.Size = new System.Drawing.Size(222, 26);
            this._helpToolstripMenuItem.Text = "&Help";
            this._helpToolstripMenuItem.Click += new System.EventHandler(this.HelpToolstripMenuItemClick);
            // 
            // _websiteToolstripMenuItem
            // 
            this._websiteToolstripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("_websiteToolstripMenuItem.Image")));
            this._websiteToolstripMenuItem.Name = "_websiteToolstripMenuItem";
            this._websiteToolstripMenuItem.Size = new System.Drawing.Size(222, 26);
            this._websiteToolstripMenuItem.Text = "&Website";
            this._websiteToolstripMenuItem.Click += new System.EventHandler(this.WebsiteToolstripMenuItemClick);
            // 
            // gridViewsToolStripMenuItem1
            // 
            this.gridViewsToolStripMenuItem1.Name = "gridViewsToolStripMenuItem1";
            this.gridViewsToolStripMenuItem1.Size = new System.Drawing.Size(222, 26);
            this.gridViewsToolStripMenuItem1.Text = "Grid Views";
            // 
            // viewLogFileToolStripMenuItem
            // 
            this.viewLogFileToolStripMenuItem.Name = "viewLogFileToolStripMenuItem";
            this.viewLogFileToolStripMenuItem.Size = new System.Drawing.Size(222, 26);
            this.viewLogFileToolStripMenuItem.Text = "View &Log File";
            this.viewLogFileToolStripMenuItem.Click += new System.EventHandler(this.viewLogFileToolStripMenuItem_Click);
            // 
            // _exitToolStripMenuItem
            // 
            this._exitToolStripMenuItem.Name = "_exitToolStripMenuItem";
            this._exitToolStripMenuItem.Size = new System.Drawing.Size(222, 26);
            this._exitToolStripMenuItem.Text = "Exit";
            this._exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItemClick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._tsslStats,
            this.tsslMonitor,
            this.tsslPerformance,
            this.tsslMediaInfo,
            this.tsslPRO});
            this.statusStrip1.Location = new System.Drawing.Point(0, 559);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(887, 30);
            this.statusStrip1.TabIndex = 0;
            // 
            // _tsslStats
            // 
            this._tsslStats.ForeColor = System.Drawing.Color.Blue;
            this._tsslStats.IsLink = true;
            this._tsslStats.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
            this._tsslStats.LinkColor = System.Drawing.Color.Blue;
            this._tsslStats.Name = "_tsslStats";
            this._tsslStats.Size = new System.Drawing.Size(59, 25);
            this._tsslStats.Text = "Loading...";
            this._tsslStats.VisitedLinkColor = System.Drawing.Color.Blue;
            this._tsslStats.Click += new System.EventHandler(this._tsslStats_Click);
            // 
            // tsslMonitor
            // 
            this.tsslMonitor.Name = "tsslMonitor";
            this.tsslMonitor.Size = new System.Drawing.Size(76, 25);
            this.tsslMonitor.Text = "Monitoring...";
            // 
            // tsslPerformance
            // 
            this.tsslPerformance.ForeColor = System.Drawing.Color.Blue;
            this.tsslPerformance.IsLink = true;
            this.tsslPerformance.LinkColor = System.Drawing.Color.Blue;
            this.tsslPerformance.Name = "tsslPerformance";
            this.tsslPerformance.Size = new System.Drawing.Size(56, 25);
            this.tsslPerformance.Text = "Perf. Tips";
            this.tsslPerformance.VisitedLinkColor = System.Drawing.Color.Blue;
            this.tsslPerformance.Click += new System.EventHandler(this.toolStripStatusLabel1_Click);
            // 
            // tsslMediaInfo
            // 
            this.tsslMediaInfo.Name = "tsslMediaInfo";
            this.tsslMediaInfo.Size = new System.Drawing.Size(0, 25);
            // 
            // tsslPRO
            // 
            this.tsslPRO.ForeColor = System.Drawing.Color.Blue;
            this.tsslPRO.IsLink = true;
            this.tsslPRO.LinkColor = System.Drawing.Color.Blue;
            this.tsslPRO.Name = "tsslPRO";
            this.tsslPRO.Size = new System.Drawing.Size(58, 25);
            this.tsslPRO.Text = "Try Agent";
            this.tsslPRO.VisitedLinkColor = System.Drawing.Color.Blue;
            this.tsslPRO.Click += new System.EventHandler(this.tsslPRO_Click);
            // 
            // _pnlContent
            // 
            this._pnlContent.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this._pnlContent.Controls.Add(this.splitContainer2);
            this._pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlContent.Location = new System.Drawing.Point(0, 0);
            this._pnlContent.Margin = new System.Windows.Forms.Padding(0);
            this._pnlContent.Name = "_pnlContent";
            this._pnlContent.Size = new System.Drawing.Size(887, 146);
            this._pnlContent.TabIndex = 20;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.flowPreview);
            this.splitContainer2.Panel1.Controls.Add(this.mediaPanelControl1);
            this.splitContainer2.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.flCommands);
            this.splitContainer2.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer2.Size = new System.Drawing.Size(887, 146);
            this.splitContainer2.SplitterDistance = 630;
            this.splitContainer2.TabIndex = 88;
            // 
            // flowPreview
            // 
            this.flowPreview.AutoScroll = true;
            this.flowPreview.BackColor = System.Drawing.Color.Transparent;
            this.flowPreview.ContextMenuStrip = this.ctxtMainForm;
            this.flowPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowPreview.Location = new System.Drawing.Point(0, 32);
            this.flowPreview.Margin = new System.Windows.Forms.Padding(0);
            this.flowPreview.Name = "flowPreview";
            this.flowPreview.Padding = new System.Windows.Forms.Padding(2);
            this.flowPreview.Size = new System.Drawing.Size(630, 114);
            this.flowPreview.TabIndex = 0;
            this.flowPreview.MouseDown += new System.Windows.Forms.MouseEventHandler(this.flowPreview_MouseDown);
            this.flowPreview.MouseEnter += new System.EventHandler(this.flowPreview_MouseEnter);
            this.flowPreview.MouseLeave += new System.EventHandler(this.flowPreview_MouseLeave);
            // 
            // mediaPanelControl1
            // 
            this.mediaPanelControl1.AutoSize = true;
            this.mediaPanelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.mediaPanelControl1.Location = new System.Drawing.Point(0, 0);
            this.mediaPanelControl1.Margin = new System.Windows.Forms.Padding(0);
            this.mediaPanelControl1.Name = "mediaPanelControl1";
            this.mediaPanelControl1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.mediaPanelControl1.Size = new System.Drawing.Size(630, 32);
            this.mediaPanelControl1.TabIndex = 21;
            this.mediaPanelControl1.Load += new System.EventHandler(this.mediaPanelControl1_Load);
            // 
            // flCommands
            // 
            this.flCommands.AutoScroll = true;
            this.flCommands.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.flCommands.Controls.Add(this.panel2);
            this.flCommands.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flCommands.Location = new System.Drawing.Point(0, 0);
            this.flCommands.Name = "flCommands";
            this.flCommands.Size = new System.Drawing.Size(253, 146);
            this.flCommands.TabIndex = 0;
            this.flCommands.MouseDown += new System.Windows.Forms.MouseEventHandler(this.flCommands_MouseDown);
            this.flCommands.MouseEnter += new System.EventHandler(this.flCommands_MouseEnter);
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(0, 0);
            this.panel2.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 39);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this._pnlCameras);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this._pnlContent);
            this.splitContainer1.Panel2MinSize = 20;
            this.splitContainer1.Size = new System.Drawing.Size(887, 520);
            this.splitContainer1.SplitterDistance = 370;
            this.splitContainer1.TabIndex = 21;
            // 
            // _pnlCameras
            // 
            this._pnlCameras.AutoScroll = true;
            this._pnlCameras.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._pnlCameras.BackColor = System.Drawing.Color.DimGray;
            this._pnlCameras.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this._pnlCameras.ContextMenuStrip = this.ctxtMainForm;
            this._pnlCameras.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlCameras.Location = new System.Drawing.Point(0, 0);
            this._pnlCameras.Margin = new System.Windows.Forms.Padding(0);
            this._pnlCameras.Name = "_pnlCameras";
            this._pnlCameras.Size = new System.Drawing.Size(887, 370);
            this._pnlCameras.TabIndex = 19;
            this._pnlCameras.Scroll += new System.Windows.Forms.ScrollEventHandler(this._pnlCameras_Scroll);
            this._pnlCameras.MouseDown += new System.Windows.Forms.MouseEventHandler(this._pnlCameras_MouseDown);
            this._pnlCameras.Resize += new System.EventHandler(this._pnlCameras_Resize);
            // 
            // ctxtPlayer
            // 
            this.ctxtPlayer.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ctxtPlayer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iSpyToolStripMenuItem,
            this.defaultPlayerToolStripMenuItem,
            this.websiteToolStripMenuItem,
            this.showInFolderToolStripMenuItem,
            this.uploadToYouTubePublicToolStripMenuItem,
            this.uploadToCloudToolStripMenuItem,
            this.archiveToolStripMenuItem,
            this.saveToToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.ctxtPlayer.Name = "ctxPlayer";
            this.ctxtPlayer.Size = new System.Drawing.Size(186, 202);
            this.ctxtPlayer.Opening += new System.ComponentModel.CancelEventHandler(this.ctxtPlayer_Opening);
            // 
            // iSpyToolStripMenuItem
            // 
            this.iSpyToolStripMenuItem.Name = "iSpyToolStripMenuItem";
            this.iSpyToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.iSpyToolStripMenuItem.Text = "Play in iSpy";
            this.iSpyToolStripMenuItem.Click += new System.EventHandler(this.iSpyToolStripMenuItem_Click);
            // 
            // defaultPlayerToolStripMenuItem
            // 
            this.defaultPlayerToolStripMenuItem.Name = "defaultPlayerToolStripMenuItem";
            this.defaultPlayerToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.defaultPlayerToolStripMenuItem.Text = "Play in Default Player";
            this.defaultPlayerToolStripMenuItem.Click += new System.EventHandler(this.defaultPlayerToolStripMenuItem_Click);
            // 
            // websiteToolStripMenuItem
            // 
            this.websiteToolStripMenuItem.Name = "websiteToolStripMenuItem";
            this.websiteToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.websiteToolStripMenuItem.Text = "Play on Website";
            this.websiteToolStripMenuItem.Click += new System.EventHandler(this.websiteToolStripMenuItem_Click);
            // 
            // showInFolderToolStripMenuItem
            // 
            this.showInFolderToolStripMenuItem.Name = "showInFolderToolStripMenuItem";
            this.showInFolderToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.showInFolderToolStripMenuItem.Text = "Show in Folder";
            this.showInFolderToolStripMenuItem.Click += new System.EventHandler(this.showInFolderToolStripMenuItem_Click);
            // 
            // uploadToYouTubePublicToolStripMenuItem
            // 
            this.uploadToYouTubePublicToolStripMenuItem.Name = "uploadToYouTubePublicToolStripMenuItem";
            this.uploadToYouTubePublicToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.uploadToYouTubePublicToolStripMenuItem.Text = "Upload to YouTube";
            this.uploadToYouTubePublicToolStripMenuItem.Click += new System.EventHandler(this.uploadToYouTubePublicToolStripMenuItem_Click);
            // 
            // uploadToCloudToolStripMenuItem
            // 
            this.uploadToCloudToolStripMenuItem.Name = "uploadToCloudToolStripMenuItem";
            this.uploadToCloudToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.uploadToCloudToolStripMenuItem.Text = "Upload to Cloud";
            this.uploadToCloudToolStripMenuItem.Click += new System.EventHandler(this.uploadToGoogleDriveToolStripMenuItem_Click);
            // 
            // archiveToolStripMenuItem
            // 
            this.archiveToolStripMenuItem.Name = "archiveToolStripMenuItem";
            this.archiveToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.archiveToolStripMenuItem.Text = "Archive";
            this.archiveToolStripMenuItem.Click += new System.EventHandler(this.archiveToolStripMenuItem_Click);
            // 
            // saveToToolStripMenuItem
            // 
            this.saveToToolStripMenuItem.Name = "saveToToolStripMenuItem";
            this.saveToToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.saveToToolStripMenuItem.Text = "Save to...";
            this.saveToToolStripMenuItem.Click += new System.EventHandler(this.saveToToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(887, 589);
            this.ContextMenuStrip = this.ctxtTaskbar;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStripMenu);
            this.Controls.Add(this.statusStrip1);
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(50, 50);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "iSpy";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.MainFormHelpButtonClicked);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing1);
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.ResizeBegin += new System.EventHandler(this.MainForm_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.Resize += new System.EventHandler(this.MainFormResize);
            this.ctxtMainForm.ResumeLayout(false);
            this.toolStripMenu.ResumeLayout(false);
            this.toolStripMenu.PerformLayout();
            this.ctxtMnu.ResumeLayout(false);
            this.ctxtTaskbar.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this._pnlContent.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.flCommands.ResumeLayout(false);
            this.flCommands.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ctxtPlayer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

#endregion

        

        private enum LayoutModes
        {
            bottom,
            left,
            right
        };

        

        private class UISync
        {
            private static ISynchronizeInvoke _sync;

            public static void Init(ISynchronizeInvoke sync)
            {
                _sync = sync;
            }

            public static void Execute(Action action)
            {
                try
                {
                    _sync.BeginInvoke(action, null);
                }
                catch
                {
                }
            }
        }

        private void menuItem27_Click(object sender, EventArgs e)
        {
            _pnlCameras.LayoutObjects(120,50);
        }

        private void menuItem28_Click(object sender, EventArgs e)
        {
            if (_cameras != null && (_cameras.Count > 0 || _microphones.Count > 0 || _floorplans.Count > 0))
            {
                switch (
                    MessageBox.Show(this, LocRm.GetString("SaveObjectsFirst"), LocRm.GetString("Confirm"),
                                    MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Yes:
                        SaveObjectList();
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        return;
                }
            }
            _houseKeepingTimer.Stop();
            _tsslStats.Text = LocRm.GetString("Loading");
            Application.DoEvents();
            RemoveObjects();
            flowPreview.Loading = true;

            _cameras = new List<objectsCamera>();
            _microphones = new List<objectsMicrophone>();
            _floorplans = new List<objectsFloorplan>();
            _actions = new List<objectsActionsEntry>();

            RenderObjects();
            Application.DoEvents();
            try
            {
                _houseKeepingTimer.Start();
            }
            catch (Exception)
            {
            }

        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageGridViews();
        }

        private void ManageGridViews()
        {
            var gvm = new GridViewManager { MainClass = this };
            gvm.ShowDialog(this);
            gvm.Dispose();
            ListGridViews();
        }

        private void archiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MediaArchiveSelected();

        }

        private void menuItem29_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(Conf.Archive))
            {
                MessageBox.Show(this, LocRm.GetString("SpecifyArchiveLocation"));
                ShowSettings(2);
            }
            if (!String.IsNullOrWhiteSpace(Conf.Archive))
            {
                Process.Start(Conf.Archive);
            }
        }

        private void menuItem25_Click(object sender, EventArgs e)
        {
            var vdm = new VirtualDeviceManager();
            vdm.ShowDialog(this);
            vdm.Dispose();
        }

        private bool _resizing;
        private DateTime _lastResize = Helper.Now;
        private bool Resizing
        {

            get { return _resizing; }
            set
            {
                _resizing = value;
                _lastResize = Helper.Now;
            }
        }

        private void MainForm_ResizeBegin(object sender, EventArgs e)
        {
            Resizing = true;
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            Resizing = false;
        }

        private void gridViewsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void uploadToGoogleDriveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MediaUploadCloud();            
        }

        private void ctxtPlayer_Opening(object sender, CancelEventArgs e)
        {

        }

        private void uploadToYouTubePublicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string msg = "";
            lock (ThreadLock)
            {
                for (int i = 0; i < flowPreview.Controls.Count; i++)
                {
                    var pb = flowPreview.Controls[i] as PreviewBox;
                    if (pb != null && pb.Selected)
                    {
                        bool b;
                        msg = YouTubeUploader.Upload(pb.Oid, pb.FileName, out b);
                    }
                }                
            }
            if (msg != "")
                MessageBox.Show(this, msg);
        }

        private void mediaPanelControl1_Load(object sender, EventArgs e)
        {

        }

        private void viewLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowLogFile();
        }

        private void onToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var obj = ContextTarget as ISpyControl;
            if (obj!=null)
                obj.Enable();
        }

        private void offToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var obj = ContextTarget as ISpyControl;
            if (obj != null)
                obj.Disable();
        }

        private void alertsOnToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var cw = ContextTarget as CameraWindow;
            if (cw != null)
            {
                cw.Camobject.alerts.active = true;
            }
            var vl = ContextTarget as VolumeLevel;
            if (vl != null)
            {
                vl.Micobject.alerts.active = true;
            }
        }

        private void alertsOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cw = ContextTarget as CameraWindow;
            if (cw != null)
            {
                cw.Camobject.alerts.active = false;
            }
            var vl = ContextTarget as VolumeLevel;
            if (vl != null)
            {
                vl.Micobject.alerts.active = false;
            }
        }

        private void scheduleOnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cw = ContextTarget as CameraWindow;
            if (cw != null)
            {
                cw.Camobject.schedule.active = true;
            }
            var vl = ContextTarget as VolumeLevel;
            if (vl != null)
            {
                vl.Micobject.schedule.active = true;
            }
        }

        private void scheduleOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cw = ContextTarget as CameraWindow;
            if (cw != null)
            {
                cw.Camobject.schedule.active = false;
            }
            var vl = ContextTarget as VolumeLevel;
            if (vl != null)
            {
                vl.Micobject.schedule.active = false;
            }
        }

        private void pTZScheduleOnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cw = ContextTarget as CameraWindow;
            if (cw != null)
            {
                cw.Camobject.ptzschedule.active = true;
            }
        }

        private void pTZScheduleOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var cw = ContextTarget as CameraWindow;
            if (cw != null)
            {
                cw.Camobject.ptzschedule.active = false;
            }
        }

        private void menuItem36_Click(object sender, EventArgs e)
        {
            using (var imp = new Importer())
            {
                imp.ShowDialog(this);
            }
        }

        private void menuItem37_Click(object sender, EventArgs e)
        {
            if (_cp == null)
            {
                using (_cp = new CheckPassword())
                {
                    _cp.ShowDialog(this);
                    if (_cp.DialogResult == DialogResult.OK)
                    {
                        _locked = false;
                        ShowForm(-1);
                    }
                }
                _cp = null;
            }
            
        }

        private void tagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessKey("tags");
        }

        private void openWebInterfaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var window = ContextTarget as CameraWindow;
            window?.OpenWebInterface();
        }

        private void menuItem32_Click(object sender, EventArgs e)
        {
            ManageGridViews();
        }

        private Panel _lastClicked;
        private void _pnlCameras_MouseDown(object sender, MouseEventArgs e)
        {
            _lastClicked = _pnlCameras;
        }

        private void flCommands_MouseDown(object sender, MouseEventArgs e)
        {
            _lastClicked = flCommands;
        }

        private void menuItem39_Click(object sender, EventArgs e)
        {
            menuItem39.Checked = !menuItem39.Checked;
            LayoutMode = menuItem39.Checked ? Enums.LayoutMode.AutoGrid : Enums.LayoutMode.Freeform;
            _pnlCameras.Invalidate();
        }

        private void _pnlCameras_Resize(object sender, EventArgs e)
        {
            _pnlCameras.Invalidate();
        }

        private FindObject _fo = new FindObject();
        private void menuItem40_Click(object sender, EventArgs e)
        {
            if (_fo.IsDisposed)
                _fo = new FindObject();
            _fo.Owner = this;
            _fo.Show(this);
        }

        private void menuItem33_Click(object sender, EventArgs e)
        {
            _locked = true;
            WindowState = FormWindowState.Minimized;
        }

        private void tsslPRO_Click(object sender, EventArgs e)
        {
            OpenUrl(Website + "/download-agent.aspx");
        }

        private void ctxtMnu_Opening(object sender, CancelEventArgs e)
        {
            _deleteToolStripMenuItem.Visible = _editToolStripMenuItem.Visible = Helper.HasFeature(Enums.Features.Edit);
            _viewMediaOnAMobileDeviceToolStripMenuItem.Visible =
                _viewMediaToolStripMenuItem.Visible =
                    _showFilesToolStripMenuItem.Visible = Helper.HasFeature(Enums.Features.Access_Media);
            tagsToolStripMenuItem.Visible = Tags.Any();
        }


        private CommandButtons cmdButtons = null;
        private void menuItem35_Click(object sender, EventArgs e)
        {
            ShowCommandButtonWindow();
        }

        internal void ShowCommandButtonWindow()
        {
            if (cmdButtons != null)
            {
                cmdButtons.Close();
                cmdButtons.Dispose();
            }

            cmdButtons = new CommandButtons();
            cmdButtons.Show(this);
        }
    }
}