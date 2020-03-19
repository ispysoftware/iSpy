using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using iSpyApplication.Controls;
using iSpyApplication.Properties;
using iSpyApplication.Utilities;
using iSpyPRO.DirectShow;

namespace iSpyApplication
{
    public partial class MainForm
    {
        //do not remove - used for translation indexing
        //LocRm.GetString("movement");LocRm.GetString( "nomovement");LocRm.GetString( "objectcount");LocRm.GetString( "Two Frames");LocRm.GetString( "Custom Frame");LocRm.GetString( "Background Modeling");LocRm.GetString( "Two Frames (Color)");LocRm.GetString( "Custom Frame (Color)");LocRm.GetString( "Background Modeling (Color)");LocRm.GetString( "None");LocRm.GetString("Grid Processing");LocRm.GetString( "Object Tracking");LocRm.GetString( "Border Highlighting");LocRm.GetString( "Area Highlighting");LocRm.GetString( "None");LocRm.GetString("Alert");LocRm.GetString( "Connection Lost");LocRm.GetString( "Reconnect");LocRm.GetString( "Normal");LocRm.GetString("Minimised");LocRm.GetString("Maximised");LocRm.GetString("FullScreen");LocRm.GetString( "ExecuteFile");LocRm.GetString("CallURL");LocRm.GetString("NetworkMessage");LocRm.GetString("PlaySound");LocRm.GetString("ShowWindow");LocRm.GetString("Beep");LocRm.GetString("Maximise");LocRm.GetString("SwitchObjectOn");LocRm.GetString("SoundThroughCamera");LocRm.GetString("TriggerAlertOn");LocRm.GetString("SendEmail");LocRm.GetString( "SendSMS");LocRm.GetString("SendTwitterMessage");LocRm.GetString("Normal");LocRm.GetString("AboveNormal");LocRm.GetString("High");LocRm.GetString("RealTime");LocRm.GetString("NewRecording");LocRm.GetString("disconnect");LocRm.GetString("reconnect");LocRm.GetString("alert");LocRm.GetString("Webservices_CouldNotConnect");LocRm.GetString( "Webservices_LoginFailed");LocRm.GetString("AlertStopped");LocRm.GetString("RecordingAlertStarted");LocRm.GetString("RecordingAlertStopped");LocRm.GetString("ReconnectFailed")

        private static configuration _conf;
        private static FilterInfoCollection _videoFilters;
        private static Color _backColor = Color.Empty;
        private static Color _borderDefaultColor = Color.Empty;
        private static Color _borderHighlightColor = Color.Empty;
        private static Color _floorPlanHighlightColor = Color.Empty;
        private static Color _volumeLevelColor = Color.Empty;
        private static Color _activityColor = Color.Empty;
        private static Color _noActivityColor = Color.Empty;
        public static string[] AlertNotifications = { "alert", "disconnect", "reconnect" };

        private static List<objectsActionsEntry> _actions;
        private static List<objectsScheduleEntry> _schedule;
        private static List<objectsMicrophone> _microphones;
        private static List<objectsFloorplan> _floorplans;
        private static List<objectsCommand> _remotecommands;
        private static List<objectsCamera> _cameras;

        public static string GoogleClientId = "648753488389.apps.googleusercontent.com";
        public static string GoogleClientSecret = "Guvru7Ug8DrGcOupqEs6fTB1";

        public static void ReloadColors()
        {
            _backColor =
                _borderDefaultColor =
                _borderHighlightColor =
                _floorPlanHighlightColor = _volumeLevelColor = _activityColor = _noActivityColor = Color.Empty;
        }

        public static Color BackgroundColor
        {
            get
            {
                if (_backColor == Color.Empty)
                    _backColor = Conf.BackColor.ToColor();
                return _backColor;
            }
            set { _backColor = value; }
        }


        public static Color BorderDefaultColor
        {
            get
            {
                if (_borderDefaultColor == Color.Empty)
                    _borderDefaultColor = Conf.BorderDefaultColor.ToColor();
                return _borderDefaultColor;
            }
            set { _borderDefaultColor = value; }
        }


        public static Color BorderHighlightColor
        {
            get
            {
                if (_borderHighlightColor == Color.Empty)
                    _borderHighlightColor = Conf.BorderHighlightColor.ToColor();
                return _borderHighlightColor;
            }
            set { _borderHighlightColor = value; }
        }


        public static Color FloorPlanHighlightColor
        {
            get
            {
                if (_floorPlanHighlightColor == Color.Empty)
                    _floorPlanHighlightColor = Conf.FloorPlanHighlightColor.ToColor();
                return _floorPlanHighlightColor;
            }
            set { _floorPlanHighlightColor = value; }
        }


        public static Color VolumeLevelColor
        {
            get
            {
                if (_volumeLevelColor == Color.Empty)
                    _volumeLevelColor = Conf.VolumeLevelColor.ToColor();
                return _volumeLevelColor;
            }
            set { _volumeLevelColor = value; }
        }


        public static Color ActivityColor
        {
            get
            {
                if (_activityColor == Color.Empty)
                    _activityColor = Conf.ActivityColor.ToColor();
                return _activityColor;
            }
            set { _activityColor = value; }
        }


        public static Color NoActivityColor
        {
            get
            {
                if (_noActivityColor == Color.Empty)
                    _noActivityColor = Conf.NoActivityColor.ToColor();
                return _noActivityColor;
            }
            set { _noActivityColor = value; }
        }




        public static FilterInfoCollection VideoFilters => _videoFilters ?? (_videoFilters = new FilterInfoCollection(FilterCategory.VideoInputDevice));

        public static ImageCodecInfo Encoder
        {
            get
            {
                if (_encoder != null)
                    return _encoder;
                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

                foreach (ImageCodecInfo codec in codecs)
                {
                    if (codec.FormatID == ImageFormat.Jpeg.Guid)
                    {
                        _encoder = codec;
                        return codec;
                    }
                }
                return null;
            }
        }

        public static configuration Conf
        {
            get
            {
                if (_conf != null)
                    return _conf;
                var s = new XmlSerializer(typeof(configuration));
                bool loaded = false;
                lock (ThreadLock)
                {
                    using (var fs = new FileStream(Program.AppDataPath + @"XML\config.xml", FileMode.Open))
                    {
                        try
                        {
                            using (TextReader reader = new StreamReader(fs))
                            {
                                fs.Position = 0;
                                _conf = (configuration)s.Deserialize(reader);
                                loaded = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.LogException(ex);
                        }
                    }
                }

                if (!loaded)
                {
                    //copy over new config file
                    try
                    {
                        var didest = new DirectoryInfo(Program.AppDataPath + @"XML\");
                        var disource = new DirectoryInfo(Program.AppPath + @"XML\");
                        File.Copy(disource + @"config.xml", didest + @"config.xml", true);

                        using (var fs = new FileStream(Program.AppDataPath + @"XML\config.xml", FileMode.Open))
                        {
                            fs.Position = 0;
                            using (TextReader reader = new StreamReader(fs))
                            {
                                _conf = (configuration)s.Deserialize(reader);
                                reader.Close();
                            }
                            fs.Close();
                        }
                    }
                    catch (Exception ex2)
                    {
                        string m = LocRm.GetString("CouldNotLoadRestore")+Environment.NewLine+ex2.Message;
                        MessageBox.Show(m);
                        Logger.LogMessage(m);
                        throw;
                    }
                }
                if (_conf.CPUMax == 0)
                    _conf.CPUMax = 90;
                if (_conf.MaxRecordingThreads == 0)
                    _conf.MaxRecordingThreads = 20;
                if (_conf.Reseller == null)
                    _conf.Reseller = "";
                if (_conf.AllowedIPList == null)
                {
                    _conf.AllowedIPList = "";
                }
                if (_conf.MaxRedrawRate == 0)
                    _conf.MaxRedrawRate = 10;
                if (_conf.PreviewItems == 0)
                {
                    _conf.PreviewItems = 100;
                    _conf.ShowOverlayControls = true;
                    _conf.ShowMediaPanel = true;
                }
                if (_conf.IPMode != "IPv6")
                    _conf.IPMode = "IPv4";

                if (_conf.Priority == 0)
                {
                    _conf.Priority = 3;
                    _conf.Monitor = true;
                }
                if (_conf.JPEGQuality == 0)
                    _conf.JPEGQuality = 80;

                if (string.IsNullOrEmpty(_conf.FloorPlanHighlightColor))
                    _conf.FloorPlanHighlightColor = "0,217,0";

                if (string.IsNullOrEmpty(_conf.YouTubeCategories))
                {
                    _conf.YouTubeCategories =
                        "Film,Autos,Music,Animals,Sports,Travel,Games,Comedy,People,News,Entertainment,Education,Howto,Nonprofit,Tech";
                }
                if (string.IsNullOrEmpty(_conf.BorderHighlightColor))
                {
                    _conf.BorderHighlightColor = "255,0,0";
                }
                if (!string.IsNullOrEmpty(Resources.Vendor))
                {
                    _conf.Vendor = Resources.Vendor;
                }
                if (string.IsNullOrEmpty(_conf.BorderDefaultColor))
                    _conf.BorderDefaultColor = "0,0,0";

                if (string.IsNullOrEmpty(_conf.StartupForm))
                    _conf.StartupForm = "iSpy";

                if (_conf.GridViews == null)
                    _conf.GridViews = new configurationGrid[] { };

                if (_conf.Joystick == null)
                    _conf.Joystick = new configurationJoystick();
                if (_conf.GPU == null)
                    _conf.GPU = new configurationGPU {nVidia = false, QuickSync = false, amd=false};

                if (string.IsNullOrEmpty(_conf.AppendLinkText))
                    _conf.AppendLinkText = "<br/>ispyconnect.com";

                if (_conf.FeatureSet < 1)
                    _conf.FeatureSet = 1;

                _conf.IPv6Disabled = true;

                if (_conf.FTPServers == null)
                    _conf.FTPServers = new configurationServer[] {};

                //can fail on windows xp/vista with a very very nasty error
                if (IsWinSevenOrHigher())
                {
                    _conf.IPv6Disabled = !(Socket.OSSupportsIPv6);
                }
                
                if (string.IsNullOrEmpty(_conf.EncryptCode))
                {
                    _conf.EncryptCode = Guid.NewGuid().ToString();
                }

                if (_conf.Permissions == null)
                {
                    _conf.Permissions = new []
                                        {
                                            new configurationGroup {featureset = 1, name="Admin", password = EncDec.EncryptData(_conf.Password_Protect_Password,_conf.EncryptCode)}
                                        };
                    //lets get rid of the old one..
                    _conf.Password_Protect_Password = "";
                }
                
                Group = _conf.Permissions.First().name;

                if (_conf.Logging == null)
                {
                    _conf.Logging = new configurationLogging
                                    {
                                        Enabled = true,
                                        FileSize = _conf.LogFileSizeKB,
                                        KeepDays = 7
                                    };
                }

                if (_conf.Cloud == null)
                {
                    //upgrade cloud stuff
                    _conf.Cloud = new configurationCloud();
                    if (!string.IsNullOrEmpty(_conf.GoogleDriveConfig))
                    {
                        _conf.Cloud.Drive = "{\"access_token\": \"\",\"token_type\": \"Bearer\",\"expires_in\": 3600,\"refresh_token\": \""+_conf.GoogleDriveConfig+"\"}";
                    }
                }
                if (_conf.ArchiveNew == null)
                {
                    if (!string.IsNullOrEmpty(_conf.Archive))
                    {
                        _conf.ArchiveNew = _conf.Archive;
                        if (!_conf.ArchiveNew.Contains("{"))
                            _conf.ArchiveNew = _conf.Archive + @"{DIR}\";
                    }
                }

                if (!string.IsNullOrEmpty(_conf.WSPassword) && _conf.WSPasswordEncrypted)
                    _conf.WSPassword = EncDec.DecryptData(_conf.WSPassword, "582df37b-b7cc-43f7-a442-30a2b188a888");

                _layoutMode = (_conf.ArrangeMode == 0) ? Enums.LayoutMode.Freeform : Enums.LayoutMode.AutoGrid;
                return _conf;
            }
        }

        static bool IsWinSevenOrHigher()
        {
            return (Environment.OSVersion.Platform == PlatformID.Win32NT) && (Environment.OSVersion.Version.Major >= 7);
        }


        public static List<objectsCamera> Cameras
        {
            get
            {
                if (_cameras == null)
                {
                    LoadObjects(Program.AppDataPath + @"XML\objects.xml");
                }
                return _cameras;
            }
            set { _cameras = value; }
        }

        public static List<objectsActionsEntry> Actions
        {
            get
            {
                if (_actions == null)
                {
                    LoadObjects(Program.AppDataPath + @"XML\objects.xml");
                }
                return _actions;
            }
            set { _actions = value; }
        }

        public static List<objectsScheduleEntry> Schedule
        {
            get
            {
                if (_cameras == null)
                {
                    LoadObjects(Program.AppDataPath + @"XML\objects.xml");
                }
                return _schedule;
            }
            set { _schedule = value; }
        }

        public static List<PTZSettings2Camera> PTZs
        {
            get
            {
                if (_ptzs == null)
                {
                    string p = Program.AppDataPath + @"\XML\PTZ2.xml";
#if DEBUG
                    p = Program.AppPath + @"\XML\PTZ2.xml";
#endif
                    LoadPTZs(p);
                }
                return _ptzs;
            }
            set { _ptzs = value; }
        }

        public static List<ManufacturersManufacturer> Sources
        {
            get
            {
                if (_sources == null)
                {
                    string p = Program.AppDataPath + @"\XML\Sources.xml";
#if DEBUG
                    p = Program.AppPath + @"\XML\Sources.xml";
#endif

                    LoadSources(p);
                }
                return _sources;
            }
            set { _sources = value; }
        }



        public static List<objectsMicrophone> Microphones
        {
            get
            {
                if (_microphones == null)
                {
                    LoadObjects(Program.AppDataPath + @"XML\objects.xml");
                }
                return _microphones;
            }
            set { _microphones = value; }
        }

        public static List<objectsCommand> RemoteCommands
        {
            get
            {
                if (_remotecommands == null)
                {
                    LoadObjects(Program.AppDataPath + @"XML\objects.xml");
                }
                return _remotecommands;
            }
            set { _remotecommands = value; }
        }

        public static List<objectsFloorplan> FloorPlans
        {
            get
            {
                if (_floorplans == null)
                {
                    LoadObjects(Program.AppDataPath + @"XML\objects.xml");
                }
                return _floorplans;
            }
            set { _floorplans = value; }
        }


        public static IPAddress[] AddressListIPv4
        {
            get
            {
                if (_ipv4Addresses != null)
                    return _ipv4Addresses;

                var arr = new List<IPAddress>();

                //get ipv4 from connected NIC
                try
                {
                    using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                    {
                        socket.Connect("10.0.2.4", 65530);
                        IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                        if (endPoint != null)
                        {
                            var localIP = endPoint.Address.ToString();
                            int end = localIP.IndexOf(":", StringComparison.Ordinal);
                            if (end > -1)
                                localIP = localIP.Remove(end);
                            arr.Add(System.Net.IPAddress.Parse(localIP));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex, "IP Lookup");
                }


                try
                {
                    arr.AddRange(
                        Dns.GetHostEntry(Dns.GetHostName())
                            .AddressList.Where(p => p.AddressFamily == AddressFamily.InterNetwork)
                            .ToArray()); //attempts reverse dns lookup

                }
                catch (Exception ex)
                {
                    Logger.LogException(ex, "IP Lookup Add Range");
                    try
                    {
                        arr.AddRange(Dns.GetHostAddresses(Dns.GetHostName()));
                    }
                    catch (Exception ex2)
                    {
                        Logger.LogException(ex2, "IP Lookup GetHostAddresses");
                        //none in the system - just use the loopback address
                        _ipv4Addresses = new[] { System.Net.IPAddress.Parse("127.0.0.1") };
                        return _ipv4Addresses;
                    }
                    //ignore lookup
                }


                _ipv4Addresses = arr.Where(IsValidIP).Distinct().ToArray();

                if (!_ipv4Addresses.Any()) //none in the system - just use the loopback address
                    _ipv4Addresses = new[] { System.Net.IPAddress.Parse("127.0.0.1") };
                return _ipv4Addresses;
            }
        }

        //IPv6
        public static IPAddress[] AddressListIPv6
        {
            get
            {
                if (_ipv6Addresses != null)
                    return _ipv6Addresses;

                var ipv6Adds = new List<IPAddress>();
                if (Conf.IPv6Disabled)
                {
                    _ipv6Addresses = ipv6Adds.ToArray();
                    return _ipv6Addresses;
                }

                try
                {
                    UnicastIPAddressInformationCollection addressInfoCollection =
                        IPGlobalProperties.GetIPGlobalProperties().GetUnicastAddresses();

                    ipv6Adds.AddRange(from addressInfo in addressInfoCollection where addressInfo.Address.IsIPv6Teredo || (addressInfo.Address.AddressFamily == AddressFamily.InterNetworkV6 && !addressInfo.Address.IsIPv6LinkLocal && !addressInfo.Address.IsIPv6SiteLocal) where !System.Net.IPAddress.IsLoopback(addressInfo.Address) select addressInfo.Address);
                }
                catch (Exception ex)
                {
                    //unsupported on win xp
                    Logger.LogException(ex, "Configuration");
                }
                _ipv6Addresses = ipv6Adds.Distinct().ToArray();
                return _ipv6Addresses;

            }
        }

        private static string _ipv4Address = "";
        public static string AddressIPv4
        {
            get
            {
                lock (ThreadLock)
                {
                    if (!string.IsNullOrEmpty(_ipv4Address))
                        return _ipv4Address;

                    var ip = AddressListIPv4.FirstOrDefault(p => p.ToString() == Conf.IPv4Address);
                    if (ip != null)
                    {
                        _ipv4Address = ip.ToString();
                        return _ipv4Address;
                    }
                    ip = AddressListIPv4.FirstOrDefault();
                    if (ip != null)
                    {
                        _ipv4Address = ip.ToString();
                        return _ipv4Address;
                    }
                    Logger.LogError(
                            "Unable to find a suitable IP address, check your network connection. Using the local loopback address.",
                            "Configuration");
                    _ipv4Address = "127.0.0.1";
                    return _ipv4Address;
                }
            }
            set
            {
                lock (ThreadLock)
                {
                    _ipv4Addresses = null;
                    _ipv4Address = null;

                    Conf.IPv4Address = value;
                }
            }
        }

        private static bool IsValidIP(IPAddress ip)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                if (!System.Net.IPAddress.IsLoopback(ip))
                {
                    var sip = ip.ToString().Split('.');
                    switch (sip[0])
                    {
                        default:
                            return true;
                        case "0":
                        case "127":
                        case "169":

                            break;
                    }
                }
            }
            return false;
        }

        //IPv6
        private static string _ipv6Address = "";

        public static string AddressIPv6
        {
            get
            {
                lock (ThreadLock)
                {
                    if (!string.IsNullOrEmpty(_ipv6Address))
                        return _ipv6Address;

                    var ip = AddressListIPv6.FirstOrDefault(p => p.ToString() == Conf.IPv4Address);
                    if (ip != null)
                    {
                        _ipv6Address = ip.ToString();
                        return _ipv6Address;
                    }

                    ip = AddressListIPv6.OrderBy(p => p.IsIPv6Teredo).FirstOrDefault();
                    if (ip != null)
                    {
                        _ipv6Address = ip.ToString();
                        return _ipv6Address;
                    }
                    return "";
                }

            }
            set
            {
                lock (ThreadLock)
                {
                    _ipv6Addresses = null;
                    _ipv6Address = null;
                    Conf.IPv6Address = value;
                }
            }
        }

        public static string IPAddress
        {
            get
            {
                if (Conf.IPMode == "IPv4")
                    return AddressIPv4;
                return MakeIPv6Url(AddressIPv6);
            }
        }

        private static string MakeIPv6Url(string ip)
        {
            //strip scope id
            if (ip.IndexOf("%", StringComparison.Ordinal) != -1)
                ip = ip.Substring(0, ip.IndexOf("%", StringComparison.Ordinal));
            return "[" + ip + "]";
        }


        private static void LoadObjects(string path)
        {
            try
            {
                var o = GetObjects(path);
                _cameras = o.cameras.ToList();
                _microphones = o.microphones.ToList();
                _floorplans = o.floorplans.ToList();
                _remotecommands = o.remotecommands.ToList();
                _actions = o.actions.entries.ToList();
                _schedule = o.schedule.entries.ToList();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                MessageBox.Show(LocRm.GetString("ConfigurationChanged"), LocRm.GetString("Error"));
                _cameras = new List<objectsCamera>();
                _microphones = new List<objectsMicrophone>();
                _remotecommands = GenerateRemoteCommands().ToList();
                _actions = new List<objectsActionsEntry>();
                _schedule = new List<objectsScheduleEntry>();
                _floorplans = new List<objectsFloorplan>();
            }

            Filter.CheckedCameraIDs = new List<int>();
            Filter.CheckedMicIDs = new List<int>();
            Filter.Filtered = false;
            _currentFileName = path;
        }

        public static int MaxOrderIndex = 0;

        public static objects GetObjects(string path)
        {
            var c = new objects();
            try
            {
                lock (ThreadLock)
                {
                    using (var fs = new FileStream(path, FileMode.Open))
                    {
                        var s = new XmlSerializer(typeof (objects));
                        using (TextReader reader = new StreamReader(fs))
                        {
                            fs.Position = 0;
                            c = (objects) s.Deserialize(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                switch (MessageBox.Show($"Error loading file ({ex.Message}) Try again?", "Error", MessageBoxButtons.YesNo))
                {
                    case DialogResult.Yes:
                        return GetObjects(path);
                }
            }
            
            if (c.cameras==null)
                c.cameras = new objectsCamera[] {};

            bool addActions = c.actions == null;
            if (addActions)
                c.actions = new objectsActions { entries = new objectsActionsEntry[] { } };


            bool addSchedule = c.schedule == null;
            if (addSchedule)
                c.schedule = new objectsSchedule {entries = new objectsScheduleEntry[] {}};

            if (c.microphones==null)
                c.microphones = new objectsMicrophone[] {};
            if (c.floorplans == null)
                c.floorplans = new objectsFloorplan[] {};

            if (c.remotecommands == null)
                c.remotecommands = new objectsCommand[] {};

            if (c.remotecommands.Length == 0)
            {
                c.remotecommands = GenerateRemoteCommands();
            }

            bool bVlc = VlcHelper.VLCAvailable;
            MaxOrderIndex = 0;

            bool bAlertVlc = false;
            int camid = 0;
            string path2;
            //load non clones first
            c.cameras = c.cameras.ToList().OrderByDescending(p => p.settings.sourceindex != 10).ToArray();
            c.microphones = c.microphones.ToList().OrderByDescending(p => p.settings.typeindex != 5).ToArray();

            foreach (objectsCamera cam in c.cameras)
            {
                if (cam.id >= camid)
                    camid = cam.id + 1;
                if (cam.settings.order == -1)
                {
                    cam.settings.order = MaxOrderIndex;
                    MaxOrderIndex++;
                }

                path2 = Helper.GetMediaDirectory(cam.settings.directoryIndex) + "video\\" + cam.directory + "\\";
                if (cam.settings.sourceindex == 5 && !bVlc)
                {
                    bAlertVlc = true;
                }
                if (cam.settings.youtube == null)
                {
                    cam.settings.youtube = new objectsCameraSettingsYoutube
                            {
                                category = Conf.YouTubeDefaultCategory,
                                tags = "iSpy, Motion Detection, Surveillance",
                                @public = true
                            };
                }
                if (cam.ptzschedule == null)
                {
                    cam.ptzschedule = new objectsCameraPtzschedule
                            {
                                active = false,
                                entries = new objectsCameraPtzscheduleEntry[] {}
                            };
                }
                if (cam.settings.storagemanagement == null)
                {
                    cam.settings.storagemanagement = new objectsCameraSettingsStoragemanagement
                            {
                                enabled = false,
                                maxage = 72,
                                maxsize = 1024

                            };
                }
                bool migrate = false;
                if (cam.alertevents == null)
                {
                    cam.alertevents = new objectsCameraAlertevents();
                    migrate = true;
                }
                    
                if (cam.settings.cloudprovider==null)
                    cam.settings.cloudprovider = new objectsCameraSettingsCloudprovider();

                if (cam.alertevents.entries == null)
                    cam.alertevents.entries = new objectsCameraAlerteventsEntry[] {};

                if (migrate)
                {
                    var l = new List<objectsCameraAlerteventsEntry>();
                    if (!string.IsNullOrEmpty(cam.alerts.executefile))
                    {
                        l.Add(new objectsCameraAlerteventsEntry
                                {
                                    type = "Exe",
                                    param1 = cam.alerts.executefile,
                                    param2 = cam.alerts.arguments
                                });
                    }
                    if (cam.notifications.sendemail)
                    {
                        l.Add(new objectsCameraAlerteventsEntry
                                {
                                    type = "E",
                                    param1 = cam.settings.emailaddress,
                                    param2 = "True"
                                });
                    }
                    if (cam.notifications.sendsms)
                    {
                        l.Add(new objectsCameraAlerteventsEntry {type = "SMS", param1 = cam.settings.smsnumber});
                    }
                    if (cam.alerts.maximise)
                    {
                        l.Add(new objectsCameraAlerteventsEntry {type = "M"});
                    }
                    if (!string.IsNullOrEmpty(cam.alerts.playsound))
                    {
                        l.Add(new objectsCameraAlerteventsEntry {type = "S", param1 = cam.alerts.playsound});
                    }

                    string[] alertOptions = cam.alerts.alertoptions.Split(','); //beep,restore

                    if (Convert.ToBoolean(alertOptions[0]))
                        l.Add(new objectsCameraAlerteventsEntry {type = "B"});
                    if (Convert.ToBoolean(alertOptions[1]))
                        l.Add(new objectsCameraAlerteventsEntry {type = "SW"});

                    if (cam.notifications.sendtwitter)
                    {
                        l.Add(new objectsCameraAlerteventsEntry {type = "TM"});
                    }

                    if (!string.IsNullOrEmpty(cam.alerts.trigger))
                    {
                        l.Add(new objectsCameraAlerteventsEntry {type = "TA", param1 = cam.alerts.trigger});
                    }
                    cam.alertevents.entries = l.ToArray();

                }
                    
                if (addActions)
                {
                    var l = c.actions.entries.ToList();
                    l.AddRange(cam.alertevents.entries.Select(a => new objectsActionsEntry
                                                                    {
                                                                        mode = "alert", 
                                                                        objectid = cam.id, 
                                                                        objecttypeid = 2,
                                                                        type = a.type,
                                                                        param1 = a.param1, 
                                                                        param2 = a.param2, 
                                                                        param3 = a.param3, 
                                                                        param4 = a.param4,
                                                                        ident = Guid.NewGuid().ToString()
                    }));
                    if (!string.IsNullOrEmpty(cam.settings.emailondisconnect))
                    {
                        l.Add(new objectsActionsEntry
                        {
                            mode = "disconnect",
                            objectid = cam.id,
                            objecttypeid = 2,
                            type = "E",
                            param1 = cam.settings.emailondisconnect,
                            param2 = "False",
                            ident = Guid.NewGuid().ToString()
                        });
                    }
                    c.actions.entries = l.ToArray();
                }
                    
                cam.newrecordingcount = 0;
                if (cam.settings.maxframerate == 0)
                    cam.settings.maxframerate = 10;
                if (cam.settings.maxframeraterecord == 0)
                    cam.settings.maxframeraterecord = 10;
                if (cam.settings.timestampfontsize == 0)
                    cam.settings.timestampfontsize = 10;
                if (cam.recorder.timelapsesave == 0)
                    cam.recorder.timelapsesave = 60;

                if (cam.x < 0)
                    cam.x = 0;
                if (cam.y < 0)
                    cam.y = 0;

                if (cam.detector.minwidth == 0)
                {
                    cam.detector.minwidth = 20;
                    cam.detector.minheight = 20;
                    cam.detector.highlight = true;
                    cam.settings.reconnectinterval = 0;
                }
                if (cam.settings.accessgroups == null)
                    cam.settings.accessgroups = "";
                if (cam.settings.ptztimetohome == 0)
                    cam.settings.ptztimetohome = 100;
                if (cam.settings.ptzautohomedelay == 0)
                    cam.settings.ptzautohomedelay = 30;
                if (cam.settings.ptzurlbase == null)
                    cam.settings.ptzurlbase = "";
                if (cam.settings.audioport <= 0)
                    cam.settings.audioport = 80;
                if (cam.ftp.intervalnew < 0)
                    cam.ftp.intervalnew = cam.ftp.interval;

                if (cam.ftp.server.Length>10)
                {
                    var ftp = Conf.FTPServers.FirstOrDefault(p => p.name == cam.ftp.server && p.username==cam.ftp.username);
                    if (ftp == null)
                    {
                        ftp = new configurationServer
                                {
                                    ident = Guid.NewGuid().ToString(),
                                    name=cam.ftp.server,
                                    password = cam.ftp.password,
                                    port = cam.ftp.port,
                                    rename = cam.ftp.rename,
                                    server = cam.ftp.server,
                                    usepassive = cam.ftp.usepassive,
                                    username = cam.ftp.username
                                };
                        var l = Conf.FTPServers.ToList();
                        l.Add(ftp);
                        Conf.FTPServers = l.ToArray();
                        cam.ftp.ident = ftp.ident;
                        cam.ftp.server = "";
                    }
                }

                if (Conf.MediaDirectories.FirstOrDefault(p => p.ID == cam.settings.directoryIndex) == null)
                    cam.settings.directoryIndex = Conf.MediaDirectories.First().ID;

                if (string.IsNullOrEmpty(cam.settings.emailondisconnect))
                {
                    if (cam.settings.notifyondisconnect)
                    {
                        cam.settings.emailondisconnect = cam.settings.emailaddress;
                    }
                }

                cam.detector.type = cam.detector.type.Replace("Modelling", "Modeling");//fix typo

                if (cam.recorder.quality == 0)
                    cam.recorder.quality = 8;
                if (cam.recorder.timelapseframerate == 0)
                    cam.recorder.timelapseframerate = 5;

                if (cam.detector.movementintervalnew < 0)
                    cam.detector.movementintervalnew = cam.detector.movementinterval;

                if (cam.detector.nomovementintervalnew < 0)
                    cam.detector.nomovementintervalnew = cam.detector.nomovementinterval;

                if (cam.directory == null)
                    throw new Exception("err_old_config");

                if (string.IsNullOrEmpty(cam.settings.ptzpelcoconfig))
                    cam.settings.ptzpelcoconfig = "COM1|9600|8|One|Odd|1";

                if (cam.savelocal == null)
                {
                    cam.savelocal = new objectsCameraSavelocal
                                    {
                                        counter = cam.ftp.counter,
                                        countermax = cam.ftp.countermax,
                                        mode = cam.ftp.mode,
                                        enabled = cam.ftp.savelocal,
                                        filename = cam.ftp.localfilename,
                                        intervalnew = cam.ftp.intervalnew,
                                        minimumdelay = cam.ftp.minimumdelay,
                                        quality = cam.ftp.quality,
                                        text = cam.ftp.text

                                    };
                }

                if (cam.alerts.processmode == null)
                    cam.alerts.processmode = "continuous";
                if (cam.alerts.pluginconfig == null)
                    cam.alerts.pluginconfig = "";
                if (cam.ftp.quality == 0)
                    cam.ftp.quality = 75;

                if (cam.settings.resizeWidth == -1)
                {
                    cam.settings.resizeWidth = cam.settings.desktopresizewidth;
                    cam.settings.resizeHeight = cam.settings.desktopresizeheight;
                }

                if (cam.ftp.countermax == 0)
                    cam.ftp.countermax = 20;

                if (cam.settings.audiousername == null)
                {
                    cam.settings.audiousername = "";
                    cam.settings.audiopassword = "";
                }

                if (string.IsNullOrEmpty(cam.settings.timestampforecolor) || cam.settings.timestampforecolor == "0")
                {
                    cam.settings.timestampforecolor = "255,255,255";
                }

                if (string.IsNullOrEmpty(cam.settings.timestampbackcolor) || cam.settings.timestampbackcolor == "0")
                {
                    cam.settings.timestampbackcolor = "70,70,70";
                }

                if (Math.Abs(cam.detector.minsensitivity - 0) < double.Epsilon)
                {
                    cam.detector.maxsensitivity = 100;
                    //fix for old setting conversion
                    cam.detector.minsensitivity = 100 - cam.detector.sensitivity;

                    if (Math.Abs(cam.detector.minsensitivity - 100) < double.Epsilon)
                    {
                        cam.detector.minsensitivity = 20;
                    }
                }

                if (cam.detector.minsensitivity > cam.detector.maxsensitivity)
                {
                    //reset
                    cam.detector.maxsensitivity = 100;
                    cam.detector.minsensitivity = 20;
                }

                if (cam.settings.tokenconfig == null)
                {
                    cam.settings.tokenconfig = new objectsCameraSettingsTokenconfig();
                }

                if (!Directory.Exists(path2))
                {
                    try
                    {
                        Directory.CreateDirectory(path2);
                    }
                    catch (IOException e)
                    {
                        Logger.LogException(e);
                    }
                }

                if (string.IsNullOrEmpty(cam.ftp.localfilename))
                {
                    cam.ftp.localfilename = "{0:yyyy-MM-dd_HH-mm-ss_fff}.jpg";
                }

                if (string.IsNullOrEmpty(cam.settings.audiomodel))
                    cam.settings.audiomodel = "None";

                if (string.IsNullOrEmpty(cam.settings.timestampfont))
                {
                    cam.settings.timestampfont = FontXmlConverter.ConvertToString(Drawfont);
                    cam.settings.timestampshowback = true;
                }

                path2 = Helper.GetMediaDirectory(cam.settings.directoryIndex) + "video\\" + cam.directory + "\\thumbs\\";
                if (!Directory.Exists(path2))
                {
                    try
                    {
                        Directory.CreateDirectory(path2);
                    }
                    catch
                    {
                        // ignored
                    }
                }

                path2 = Helper.GetMediaDirectory(cam.settings.directoryIndex) + "video\\" + cam.directory + "\\grabs\\";
                if (!Directory.Exists(path2))
                {
                    try
                    {
                        Directory.CreateDirectory(path2);
                    }
                    catch
                    {
                        // ignored
                    }
                }
                if (cam.alerts.trigger == null)
                    cam.alerts.trigger = "";

                if (string.IsNullOrEmpty(cam.rotateMode))
                {
                    cam.rotateMode = "RotateNoneFlipNone";
                    if (cam.rotate90)
                    {
                        cam.rotateMode = RotateFlipType.Rotate90FlipNone.ToString();
                    }
                    if (cam.flipx)
                    {
                        cam.rotateMode = RotateFlipType.RotateNoneFlipX.ToString();
                    }
                    if (cam.flipy)
                    {
                        cam.rotateMode = RotateFlipType.RotateNoneFlipY.ToString();
                    }
                    if (cam.flipx && cam.flipy)
                    {
                        cam.rotateMode = RotateFlipType.RotateNoneFlipXY.ToString();
                    }
                }
                if (cam.settings.pip == null)
                {
                    cam.settings.pip = new objectsCameraSettingsPip {enabled = false, config = ""};
                }
                if (cam.settings.onvif == null)
                {
                    cam.settings.onvif = new objectsCameraSettingsOnvif();
                }

                if (cam.settings.cloudprovider.provider.ToLower() == "google drive")
                    cam.settings.cloudprovider.provider = "drive";
            }
            int micid = 0;
            foreach (objectsMicrophone mic in c.microphones)
            {
                if (mic.id >= micid)
                    micid = mic.id + 1;

                if (mic.settings.order == -1)
                {
                    mic.settings.order = MaxOrderIndex;
                    MaxOrderIndex++;
                }

                if (mic.directory == null)
                    throw new Exception("err_old_config");
                mic.newrecordingcount = 0;
                path2 = Helper.GetMediaDirectory(mic.settings.directoryIndex) + "audio\\" + mic.directory + "\\";
                if (!Directory.Exists(path2))
                    Directory.CreateDirectory(path2);

                if (mic.settings.accessgroups == null)
                    mic.settings.accessgroups = "";

                if (mic.settings.storagemanagement == null)
                {
                    mic.settings.storagemanagement = new objectsMicrophoneSettingsStoragemanagement
                    {
                        enabled = false,
                        maxage = 72,
                        maxsize = 1024

                    };
                }
                if (Math.Abs(mic.detector.minsensitivity - (-1)) < double.Epsilon)
                {
                    mic.detector.minsensitivity = mic.detector.sensitivity;
                    mic.detector.maxsensitivity = 100;
                }
                if (mic.detector.minsensitivity > mic.detector.maxsensitivity)
                {
                    //reset
                    mic.detector.maxsensitivity = 100;
                    mic.detector.minsensitivity = 20;
                }

                if (Conf.MediaDirectories.FirstOrDefault(p => p.ID == mic.settings.directoryIndex) == null)
                    mic.settings.directoryIndex = Conf.MediaDirectories.First().ID;

                bool migrate = false;
                if (mic.alertevents == null)
                {
                    mic.alertevents = new objectsMicrophoneAlertevents();
                    migrate = true;
                }
                if (mic.alertevents.entries == null)
                {
                    mic.alertevents.entries = new objectsMicrophoneAlerteventsEntry[] { };
                }

                if (migrate)
                {
                    var l = new List<objectsMicrophoneAlerteventsEntry>();
                    if (!string.IsNullOrEmpty(mic.alerts.executefile))
                    {
                        l.Add(new objectsMicrophoneAlerteventsEntry { type = "Exe", param1 = mic.alerts.executefile, param2 = mic.alerts.arguments });
                    }
                    if (mic.notifications.sendemail)
                    {
                        l.Add(new objectsMicrophoneAlerteventsEntry { type = "E", param1 = mic.settings.emailaddress, param2 = "True" });
                    }
                    if (mic.notifications.sendsms)
                    {
                        l.Add(new objectsMicrophoneAlerteventsEntry { type = "SMS", param1 = mic.settings.smsnumber });
                    }

                    string[] alertOptions = mic.alerts.alertoptions.Split(','); //beep,restore

                    if (Convert.ToBoolean(alertOptions[0]))
                        l.Add(new objectsMicrophoneAlerteventsEntry { type = "B" });
                    if (Convert.ToBoolean(alertOptions[1]))
                        l.Add(new objectsMicrophoneAlerteventsEntry { type = "SW" });

                    if (mic.notifications.sendtwitter)
                    {
                        l.Add(new objectsMicrophoneAlerteventsEntry { type = "TM" });
                    }

                    if (!string.IsNullOrEmpty(mic.alerts.trigger))
                    {
                        l.Add(new objectsMicrophoneAlerteventsEntry { type = "TA", param1 = mic.alerts.trigger });
                    }
                    mic.alertevents = new objectsMicrophoneAlertevents() {entries = l.ToArray()};
                }

                if (string.IsNullOrEmpty(mic.settings.emailondisconnect))
                {
                    if (mic.settings.notifyondisconnect)
                    {
                        mic.settings.emailondisconnect = mic.settings.emailaddress;
                    }
                }

                if (addActions)
                {
                    var l = c.actions.entries.ToList();
                    l.AddRange(mic.alertevents.entries.Select(a => new objectsActionsEntry
                    {
                        mode = "alert",
                        objectid = mic.id,
                        objecttypeid = 1,
                        type = a.type,
                        param1 = a.param1,
                        param2 = a.param2,
                        param3 = a.param3,
                        param4 = a.param4,
                        ident = Guid.NewGuid().ToString()

                    }));
                    if (!string.IsNullOrEmpty(mic.settings.emailondisconnect))
                    {
                        l.Add(new objectsActionsEntry
                                {
                                    mode = "disconnect",
                                    objectid = mic.id,
                                    objecttypeid = 1,
                                    type = "E",
                                    param1 = mic.settings.emailondisconnect,
                                    param2 = "False",
                                    ident = Guid.NewGuid().ToString()
                        });
                    }
                    c.actions.entries = l.ToArray();
                }

                if (mic.x < 0)
                    mic.x = 0;
                if (mic.y < 0)
                    mic.y = 0;

                if (mic.settings.gain <= 0)
                    mic.settings.gain = 1;

                if (mic.alerts.trigger == null)
                    mic.alerts.trigger = "";
            }

            foreach (var aa in c.actions.entries)
            {
                if (string.IsNullOrEmpty(aa.ident))
                    aa.ident = Guid.NewGuid().ToString();
            }
            int fpid = 0;
            foreach (objectsFloorplan ofp in c.floorplans)
            {
                if (ofp.id >= fpid)
                    fpid = ofp.id + 1;

                if (ofp.x < 0)
                    ofp.x = 0;
                if (ofp.y < 0)
                    ofp.y = 0;
                if (ofp.accessgroups == null)
                    ofp.accessgroups = "";

                if (ofp.order == -1)
                {
                    ofp.order = MaxOrderIndex;
                    MaxOrderIndex++;
                }
            }

            int rcid = 0;
            foreach (objectsCommand ocmd in c.remotecommands)
            {
                if (ocmd.id >= rcid)
                    rcid = ocmd.id + 1;
            }

            if (addSchedule)
            {
                var l = new List<objectsScheduleEntry>();
                foreach (var o in c.cameras)
                {
                    foreach (var se in o.schedule.entries)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            bool startSpecified = se.start.Split(':')[0] != "-";
                            bool stopSpecified = se.stop.Split(':')[0] != "-";
                            if (!startSpecified && !stopSpecified)
                                continue;


                            var ose = new objectsScheduleEntry
                                      {
                                          objectid = o.id,
                                          objecttypeid = 2,
                                          daysofweek = se.daysofweek,
                                          active = se.active,
                                          parameter = "",
                                          typeid = -1,
                                          time = ParseTime(startSpecified ? se.start : se.stop)
                                      };


                            switch (i)
                            {
                                case 0: //power on
                                    if (startSpecified)
                                        ose.typeid = 0;
                                    break;
                                case 1: //power off
                                    if (stopSpecified)
                                    {
                                        ose.time = ParseTime(se.stop);
                                        ose.typeid = 1;
                                    }
                                    break;
                                case 2:
                                    ose.typeid = se.alerts ? 7 : 8;
                                    break;
                                case 3:
                                    ose.typeid = se.ftpenabled ? 13 : 14;
                                    break;
                                case 4:
                                    ose.typeid = se.messaging ? 21 : 22;
                                    break;
                                case 5:
                                    ose.typeid = se.ptz ? 19 : 20;
                                    break;
                                case 6:
                                    if (se.recordonalert)
                                        ose.typeid = 5;
                                    else
                                    {
                                        ose.typeid = se.recordondetect ? 4 : 6;
                                    }
                                    break;
                                case 7:
                                    if (se.recordonstart)
                                        ose.typeid = 2;
                                    break;
                                case 8:
                                    ose.typeid = se.savelocalenabled ? 17 : 18;
                                    break;
                                case 9:
                                    ose.typeid = se.timelapseenabled ? 11 : 12;
                                    break;
                            }
                            if (ose.typeid > -1)
                                l.Add(ose);
                        }
                    }
                }

                foreach (var o in c.microphones)
                {
                    foreach (var se in o.schedule.entries)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            var ose = new objectsScheduleEntry
                                      {
                                          objectid = o.id,
                                          objecttypeid = 1,
                                          daysofweek = se.daysofweek,
                                          active = se.active,
                                          parameter = "",
                                          typeid = -1
                                      };
                            bool startSpecified = se.start.Split(':')[0] != "-";
                            bool stopSpecified = se.stop.Split(':')[0] != "-";
                            ose.time = ParseTime(startSpecified ? se.start : se.stop);
                            switch (i)
                            {
                                case 0: //power on
                                    if (startSpecified)
                                        ose.typeid = 0;
                                    break;
                                case 1: //power off
                                    if (stopSpecified)
                                    {
                                        ose.time = ParseTime(se.stop);
                                        ose.typeid = 1;
                                    }
                                    break;
                                case 2:
                                    ose.typeid = se.alerts ? 7 : 8;
                                    break;
                                case 4:
                                    ose.typeid = se.messaging ? 21 : 22;
                                    break;
                                case 6:
                                    if (se.recordonalert)
                                        ose.typeid = 5;
                                    else
                                    {
                                        ose.typeid = se.recordondetect ? 4 : 6;
                                    }
                                    break;
                                case 7:
                                    if (se.recordonstart)
                                        ose.typeid = 2;
                                    break;
                            }
                            if (ose.typeid > -1)
                                l.Add(ose);
                        }
                    }
                }

                c.schedule = new objectsSchedule() {entries = l.ToArray()};
            }



            if (bAlertVlc)
            {
                MessageBox.Show(Program.Platform == "x64"
                    ? LocRm.GetString("InstallVLCx64")
                        .Replace("[DIR]", Environment.NewLine + Program.AppPath + "VLC64" + Environment.NewLine)
                    : LocRm.GetString("InstallVLCx86"));
                OpenUrl(Program.Platform == "x64" ? VLCx64 : VLCx86);
            }
            SaveConfig();
            NeedsSync = true;
            Logger.LogMessage("Loaded " + c.cameras.Length + " cameras, " + c.microphones.Length + " mics and " + c.floorplans.Length + " floorplans");
            return c;

        }

        private static int ParseTime(string time)
        {
            var d = DateTime.Now;
            var s = time.Split(':');
            var ts = new TimeSpan(Convert.ToInt32(s[0]), Convert.ToInt32(s[1]), 0);
            d = d.Date + ts;
            return Convert.ToInt32(d.TimeOfDay.TotalMinutes);
        }

        internal static int NextCameraId
        {
            get
            {
                if (Cameras != null && Cameras.Count > 0)
                    return Cameras.Max(p => p.id) + 1;
                return 1;
            }
        }

        internal static int NextMicrophoneId
        {
            get
            {
                if (Microphones != null && Microphones.Count > 0)
                    return Microphones.Max(p => p.id) + 1;
                return 1;
            }
        }

        internal static int NextFloorPlanId
        {
            get
            {
                if (FloorPlans != null && FloorPlans.Count > 0)
                    return FloorPlans.Max(p => p.id) + 1;
                return 1;
            }
        }

        internal static int NextCommandId
        {
            get
            {
                if (RemoteCommands != null && RemoteCommands.Count > 0)
                    return RemoteCommands.Max(p => p.id) + 1;
                return 1;
            }
        }

        private static void LoadPTZs(string path)
        {
            try
            {
                var s = new XmlSerializer(typeof(PTZSettings2));
                PTZSettings2 c;
                using (var fs = new FileStream(path, FileMode.Open))
                {
                    fs.Position = 0;
                    using (TextReader reader = new StreamReader(fs))
                    {
                        c = (PTZSettings2)s.Deserialize(reader);
                        reader.Close();
                    }
                    fs.Close();
                }

                _ptzs = c.Camera?.ToList() ?? new List<PTZSettings2Camera>();
            }
            catch (Exception)
            {
                MessageBox.Show(LocRm.GetString("PTZError"), LocRm.GetString("Error"));
            }
        }

        private static void LoadSources(string path)
        {
            try
            {
                var s = new XmlSerializer(typeof(Manufacturers));
                Manufacturers c;
                using (var fs = new FileStream(path, FileMode.Open))
                {
                    fs.Position = 0;
                    using (TextReader reader = new StreamReader(fs))
                    {
                        c = (Manufacturers)s.Deserialize(reader);
                        reader.Close();
                        
                    }
                    fs.Close();
                }
                _sources = c.Manufacturer?.Distinct().ToList() ?? new List<ManufacturersManufacturer>();

                int i = 0;
                foreach (var m in _sources)
                {
                    foreach (var u in m.url)
                    {
                        u.id = i;
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, LocRm.GetString("Error"));
            }
        }

        private void LoadCommands()
        {
            lock (ThreadLock)
            {
                for (int i = 0; i < flCommands.Controls.Count; i++)
                {
                    flCommands.Controls.RemoveAt(i);
                    i--;
                }
                foreach (objectsCommand oc in RemoteCommands)
                {
                    var b = new Button
                    {
                        Tag = oc.id,
                        AutoSize = true,
                        UseVisualStyleBackColor = true,
                        Text = oc.name.StartsWith("cmd_") ? LocRm.GetString(oc.name) : oc.name,
                        Width = 110

                    };
                    b.Click += BClick;
                    flCommands.Controls.Add(b);
                }
            }
        }

        private void RemoveObjects()
        {

            bool removed = true;
            while (removed)
            {
                removed = false;
                foreach (Control c in _pnlCameras.Controls)
                {
                    var window = c as CameraWindow;
                    if (window != null)
                    {
                        var cameraControl = window;
                        RemoveCamera(cameraControl, false);
                        removed = true;
                        break;
                    }
                    var level = c as VolumeLevel;
                    if (level != null)
                    {
                        var volumeControl = level;
                        RemoveMicrophone(volumeControl, false);
                        removed = true;
                        break;
                    }
                    var control = c as FloorPlanControl;
                    if (control != null)
                    {
                        var floorPlanControl = control;
                        RemoveFloorplan(floorPlanControl, false);
                        removed = true;
                        break;
                    }
                }
                Application.DoEvents();
            }


            _pnlCameras.Refresh();

            //lock (ThreadLock)
            try
            {
                Masterfilelist.Clear();
                foreach (Control c in flowPreview.Controls)
                {
                    var pb = c as PreviewBox;
                    if (pb != null)
                    {
                        pb.MouseDown -= PbMouseDown;
                        pb.MouseEnter -= PbMouseEnter;
                        pb.Dispose();
                    }
                }
                flowPreview.Controls.Clear();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        
        private void RenderObjects()
        {
            //the order we do this in is very important
            try
            {
                foreach (objectsCamera oc in Cameras)
                {
                    DisplayCamera(oc);
                }

                foreach (objectsMicrophone om in Microphones)
                {
                    DisplayMicrophone(om);
                }

                foreach (objectsFloorplan ofp in FloorPlans)
                {
                    DisplayFloorPlan(ofp);
                }

                //link em up
                foreach (objectsCamera oc in Cameras)
                {
                    var cw = GetCameraWindow(oc.id);
                    cw?.SetVolumeLevel(oc.settings.micpair);
                }
                
                foreach (objectsCamera oc in Cameras)
                {
                    var cw = GetCameraWindow(oc.id);
                    

                    if (Conf.AutoSchedule && oc.schedule.active && Schedule.Any(p => p.objectid == oc.id && p.objecttypeid == 2 && p.active))
                    {
                        oc.settings.active = false;
                        cw.ApplySchedule();
                    }
                    else
                    {
                        try
                        {
                            if (oc.settings.active)
                                cw.Enable();
                        }
                        catch (Exception ex)
                        {
                            Logger.LogException(ex);
                        }
                    }
                }
                
                foreach (objectsMicrophone om in Microphones)
                {
                    var vl = GetVolumeLevel(om.id);
                    if (Conf.AutoSchedule && om.schedule.active && Schedule.Any(p => p.objectid == om.id && p.objecttypeid == 1 && p.active))
                    {
                        om.settings.active = false;
                        vl.ApplySchedule();
                    }
                    else
                    {
                        if (om.settings.active)
                            vl.Enable();
                    }
                }


                bool cam = false;
                if (_pnlCameras.Controls.Count > 0)
                {
                    //prevents layering issues
                    for (int index = 0; index < _pnlCameras.Controls.Count; index++)
                    {
                        var c = _pnlCameras.Controls[index];
                        var cw = c as CameraWindow;
                        if (cw?.VolumeControl != null)
                        {
                            cam = true;
                            cw.SendToBack();
                            cw.VolumeControl.SendToBack();
                        }
                    }
                    _pnlCameras.Controls[0].Focus();
                }
                if (!cam)
                    flowPreview.Loading = false;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            NeedsSync = true;
        }

        private DateTime _oldestFile = DateTime.MinValue;
        private void DeleteOldFiles()
        {
            bool fileschanged = false;
            //walk through camera specific management first

            foreach (var camobj in Cameras)
            {
                if (camobj.settings.storagemanagement.enabled)
                {
                    try
                    {
                        var cw = GetCameraWindow(camobj.id);
                        if (cw != null)
                        {
                            bool archive = camobj.settings.storagemanagement.archive;
                            var d = Helper.GetMediaDirectory(2, camobj.id) + "video\\" + camobj.directory;
                            if (!Directory.Exists(d))
                            {
                                Logger.LogError("Directory not found: "+d);
                                continue;
                            }
                            var dirinfo = new DirectoryInfo(d);

                            var lFi = new List<FileInfo>();
                            lFi.AddRange(dirinfo.GetFiles("*.*", SearchOption.AllDirectories));

                            lFi = lFi.FindAll(f => f.Extension != ".xml");
                            lFi = lFi.OrderBy(f => f.CreationTime).ToList();

                            var size = lFi.Sum(p => p.Length);
                            var targetSize = (camobj.settings.storagemanagement.maxsize) * 1048576d;
                            if (size > targetSize)
                            {
                                for (int i = 0; i < lFi.Count; i++)
                                {
                                    var fi = lFi[i];
                                    if (FileOperations.DeleteOrArchive(cw, fi.FullName, archive))
                                    {
                                        try
                                        {
                                            cw.RemoveFile(fi.Name);
                                        }
                                        catch
                                        {
                                            // ignored
                                        }
                                        size -= fi.Length;

                                        fileschanged = true;
                                        lFi.Remove(fi);
                                        i--;
                                        if (size < targetSize)
                                            break;
                                        Thread.Sleep(5);
                                    }
                                }
                            }
                            var targetdate = DateTime.Now.AddHours(0 - camobj.settings.storagemanagement.maxage);
                            lFi = lFi.FindAll(p => p.CreationTime < targetdate).ToList();
                            for (int i = 0; i < lFi.Count; i++)
                            {
                                var fi = lFi[i];
                                if (FileOperations.DeleteOrArchive(cw, fi.FullName, archive))
                                {
                                    try
                                    {
                                        cw.RemoveFile(fi.Name);
                                    }
                                    catch
                                    {
                                        // ignored
                                    }
                                    fileschanged = true;
                                    lFi.Remove(fi);
                                    i--;
                                    Thread.Sleep(5);
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex, "DeleteOldFiles: "+ camobj.name);
                    }
                }
            }

            foreach (var micobj in Microphones)
            {
                if (micobj.settings.storagemanagement.enabled)
                {
                    try
                    {
                        var vl = GetVolumeLevel(micobj.id);
                        if (vl != null)
                        {
                            bool archive = micobj.settings.storagemanagement.archive;

                            var d = Helper.GetMediaDirectory(1, micobj.id) + "audio\\" + micobj.directory;
                            if (!Directory.Exists(d))
                            {
                                Logger.LogError("Directory not found: " + d);
                                continue;
                            }
                            var dirinfo = new DirectoryInfo(d);

                            var lFi = new List<FileInfo>();
                            lFi.AddRange(dirinfo.GetFiles("*.*", SearchOption.AllDirectories));

                            lFi = lFi.FindAll(f => f.Extension != ".xml");
                            lFi = lFi.OrderBy(f => f.CreationTime).ToList();

                            var size = lFi.Sum(p => p.Length);
                            var targetSize = (micobj.settings.storagemanagement.maxsize) * 1048576d;
                            while (size > targetSize)
                            {
                                for (int i = 0; i < lFi.Count; i++)
                                {
                                    var fi = lFi[i];
                                    if (FileOperations.DeleteOrArchive(vl,fi.FullName, archive))
                                    {
                                        try
                                        {
                                            vl.RemoveFile(fi.Name);
                                        }
                                        catch
                                        {
                                            // ignored
                                        }
                                        size -= fi.Length;
                                        fileschanged = true;
                                        lFi.Remove(fi);
                                        Thread.Sleep(5);
                                        i--;
                                    }
                                }
                            }
                            var targetdate = DateTime.Now.AddHours(0 - micobj.settings.storagemanagement.maxage);
                            lFi = lFi.FindAll(p => p.CreationTime < targetdate).ToList();
                            for (int i = 0; i < lFi.Count; i++)
                            {
                                var fi = lFi[i];
                                if (FileOperations.DeleteOrArchive(vl, fi.FullName, archive))
                                {
                                    try
                                    {
                                        vl.RemoveFile(fi.Name);
                                    }
                                    catch
                                    {
                                        // ignored
                                    }
                                    fileschanged = true;
                                    lFi.Remove(fi);
                                    Thread.Sleep(5);
                                    i--;
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex, "DeleteOldFiles: " + micobj.name);
                    }
                }
            }

            if (fileschanged)
            {
                UISync.Execute(RefreshControls);
            }
            //run storage management on each directory
            foreach (var d in Conf.MediaDirectories)
            {
                if (d.Enable_Storage_Management)
                {
                    if (d.DeleteFilesOlderThanDays <= 0)
                        continue;

                    DateTime dtref = DateTime.Now.AddDays(0 - d.DeleteFilesOlderThanDays);

                    //don't bother if oldest file isn't past cut-off
                    if (_oldestFile > dtref)
                        continue;

                    var lFi = new List<FileInfo>();
                    try
                    {
                        var dirinfo = new DirectoryInfo(d.Entry + "video\\");
                        lFi.AddRange(dirinfo.GetFiles("*.*", SearchOption.AllDirectories));

                        dirinfo = new DirectoryInfo(d.Entry + "audio\\");
                        lFi.AddRange(dirinfo.GetFiles("*.*", SearchOption.AllDirectories));

                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex, "DeleteOldFiles: "+d.Entry);
                        continue;
                    }

                    lFi = lFi.FindAll(f => f.Extension != ".xml");
                    lFi = lFi.OrderBy(f => f.CreationTime).ToList();

                    var size = lFi.Sum(p => p.Length);
                    var targetSize = (d.MaxMediaFolderSizeMB * 0.7) * 1048576d;

                    if (size < targetSize)
                    {
                        continue;
                    }

                    var lCan = lFi.Where(p => p.CreationTime < dtref).OrderBy(p => p.CreationTime).ToList();
                    bool archive = d.archive;

                    for (int i = 0; i < lCan.Count; i++)
                    {
                        var fi = lCan[i];
                        string folder = "";
                        try
                        {
                            folder = fi.FullName.Replace(d.Entry, "", StringComparison.CurrentCultureIgnoreCase, 1)
                                .Split('\\')[1];
                        }
                        catch (Exception ex)
                        {
                            Logger.LogException(ex,"Storage Management - couldn't find control");
                            continue;
                        }

                        var ctrl = ControlList.FirstOrDefault(p => p.Folder == folder);
                        if (ctrl!=null && FileOperations.DeleteOrArchive(ctrl,fi.FullName, archive))
                        {
                            size -= fi.Length;
                            fileschanged = true;
                            lCan.Remove(fi);
                            i--;
                            lock (ThreadLock)
                            {
                                Masterfilelist.RemoveAll(p => p.Filename.EndsWith(fi.Name));
                            }
                            if (size < targetSize)
                            {
                                break;
                            }
                            Thread.Sleep(5);
                        }
                    
                    }
                    if (lCan.Count > 0)
                        _oldestFile = lCan.First().CreationTime;
                    else
                    {
                        var o = lFi.FirstOrDefault(p => p.CreationTime > dtref);
                        if (o != null)
                            _oldestFile = o.CreationTime;

                    }

                    if (fileschanged)
                    {
                        UISync.Execute(RefreshControls);
                        Logger.LogMessage(LocRm.GetString("MediaStorageLimit").Replace("[AMOUNT]",
                                                                                      d.MaxMediaFolderSizeMB.ToString
                                                                                          (
                                                                                              CultureInfo.
                                                                                                  InvariantCulture)));
                    }

                    if ((size / 1048576) > d.MaxMediaFolderSizeMB && !d.StopSavingFlag && d.StopSavingOnStorageLimit)
                    {
                        d.StopSavingFlag = true;
                    }
                    else
                        d.StopSavingFlag = false;
                }
            }

        }

        private void SetMicrophoneEvents(VolumeLevel vw)
        {
            vw.DoubleClick += VolumeControlDoubleClick;
            vw.MouseDown += VolumeControlMouseDown;
            vw.MouseUp += VolumeControlMouseUp;
            vw.MouseMove += VolumeControlMouseMove;
            vw.RemoteCommand += VolumeControlRemoteCommand;
            vw.Notification += ControlNotification;
            vw.FileListUpdated += VolumeControlFileListUpdated;
        }

        void VolumeControlFileListUpdated(object sender)
        {
            lock (ThreadLock)
            {
                try { 
                    var vl = sender as VolumeLevel;
                    if (vl != null)
                    {
                        Masterfilelist.RemoveAll(p => p.ObjectId == vl.Micobject.id && p.ObjectTypeId == 1);
                        var l = vl.FileList.ToList();
                        foreach (var ff in l)
                        {
                            Masterfilelist.Add(new FilePreview(ff.Filename, ff.DurationSeconds, vl.Micobject.name,
                                                               ff.CreatedDateTicks, 1, vl.Micobject.id, ff.MaxAlarm,ff.IsTimelapse,ff.IsMergeFile));
                        }
                        if (!vl.LoadedFiles)
                        {
                            vl.LoadedFiles = true;
                            //last one?
                            bool all = true;
                            foreach (Control c in _pnlCameras.Controls)
                            {
                                var cameraWindow = c as CameraWindow;
                                if (cameraWindow != null)
                                {
                                    if (!cameraWindow.LoadedFiles)
                                    {
                                        all = false;
                                        break;
                                    }
                                }
                                var volumeLevel = c as VolumeLevel;
                                if (volumeLevel != null)
                                {
                                    if (!volumeLevel.LoadedFiles)
                                    {
                                        all = false;
                                        break;
                                    }
                                }
                            }
                            if (all)
                            {
                                flowPreview.Loading = false;
                                LoadPreviews();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex, "FileListUpdated");
                }
            }

        }

        private void SetFloorPlanEvents(FloorPlanControl fpc)
        {
            fpc.DoubleClick += FloorPlanDoubleClick;
            fpc.MouseDown += FloorPlanMouseDown;
            fpc.MouseUp += FloorPlanMouseUp;
            fpc.MouseMove += FloorPlanMouseMove;
        }

        public void DisplayMicrophone(objectsMicrophone mic)
        {
            var micControl = new VolumeLevel(mic,this);
            SetMicrophoneEvents(micControl);
            micControl.BackColor = Conf.BackColor.ToColor();
            _pnlCameras.Controls.Add(micControl);
            micControl.Location = new Point(mic.x, mic.y);
            micControl.Size = new Size(mic.width, mic.height);
            micControl.BringToFront();
            micControl.Tag = GetControlIndex();

            try
            {
                string path = Helper.GetMediaDirectory(1, mic.id) + "audio\\" + mic.directory + "\\";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            micControl.LoadFileList();
        }

        internal void DisplayFloorPlan(objectsFloorplan ofp)
        {
            var fpControl = new FloorPlanControl(ofp, this);
            SetFloorPlanEvents(fpControl);
            fpControl.BackColor = Conf.BackColor.ToColor();
            _pnlCameras.Controls.Add(fpControl);
            fpControl.Location = new Point(ofp.x, ofp.y);
            fpControl.Size = new Size(ofp.width, ofp.height);
            fpControl.BringToFront();
            fpControl.Tag = GetControlIndex();
        }

        internal void EditCamera(objectsCamera cr, IWin32Window owner = null)
        {
            int cameraId = Convert.ToInt32(cr.id);
            CameraWindow cw = null;

            for (int index = 0; index < _pnlCameras.Controls.Count; index++)
            {
                Control c = _pnlCameras.Controls[index];
                if (c.GetType() != typeof(CameraWindow)) continue;
                var cameraControl = (CameraWindow)c;
                if (cameraControl.Camobject.id == cameraId)
                {
                    cw = cameraControl;
                    break;
                }
            }

            if (cw == null) return;           
            var ac = new AddCamera { CameraControl = cw, MainClass = this };
            ac.ShowDialog(owner ?? this);
            ac.Dispose();
            SetNewStartPosition();
        }

        internal void EditObject(ISpyControl ctrl, IWin32Window owner = null)
        {
            var cw = ctrl as CameraWindow;
            if (cw != null)
            {
                EditCamera(cw.Camobject,owner);
                return;
            }
            var vl = ctrl as VolumeLevel;
            if (vl != null)
            {
                EditMicrophone(vl.Micobject, owner);
                return;
            }
            var fp = ctrl as FloorPlanControl;
            if (fp == null) return;
            EditFloorplan(fp.Fpobject, owner);
        }

        internal void EditMicrophone(objectsMicrophone om, IWin32Window owner = null)
        {
            VolumeLevel vlf = null;

            for (int index = 0; index < _pnlCameras.Controls.Count; index++)
            {
                Control c = _pnlCameras.Controls[index];
                if (c.GetType() != typeof(VolumeLevel)) continue;
                var vl = (VolumeLevel)c;
                if (vl.Micobject.id == om.id)
                {
                    vlf = vl;
                    break;
                }
            }

            if (vlf != null)
            {
                var am = new AddMicrophone { VolumeLevel = vlf, MainClass = this };
                am.ShowDialog(owner ?? this);
                am.Dispose();
            }
        }

        internal void EditFloorplan(objectsFloorplan ofp, IWin32Window owner = null)
        {
            FloorPlanControl fpc = null;

            for (int index = 0; index < _pnlCameras.Controls.Count; index++)
            {
                Control c = _pnlCameras.Controls[index];
                if (c.GetType() != typeof(FloorPlanControl)) continue;
                var fp = (FloorPlanControl)c;
                if (fp.Fpobject.id != ofp.id) continue;
                fpc = fp;
                break;
            }

            if (fpc != null)
            {
                var afp = new AddFloorPlan { Fpc = fpc, MainClass = this };
                afp.ShowDialog(owner ?? this);
                afp.Dispose();
                fpc.Invalidate();
            }
        }

        private CameraWindow GetCamera(int cameraId)
        {
            for (int index = 0; index < _pnlCameras.Controls.Count; index++)
            {
                Control c = _pnlCameras.Controls[index];
                if (c.GetType() != typeof(CameraWindow)) continue;
                var cw = (CameraWindow)c;
                if (cw.Camobject.id != cameraId) continue;
                return cw;
            }
            return null;
        }

        public FloorPlanControl GetFloorPlan(int floorPlanId)
        {
            for (int index = 0; index < _pnlCameras.Controls.Count; index++)
            {
                Control c = _pnlCameras.Controls[index];
                if (c.GetType() != typeof(FloorPlanControl)) continue;
                var fp = (FloorPlanControl)c;
                if (fp.Fpobject.id != floorPlanId) continue;
                return fp;
            }
            return null;
        }

        public void RemoveObject(ISpyControl io, bool confirm = false)
        {
            var cw = io as CameraWindow;
            if (cw != null)
                RemoveCamera(cw, confirm);
            else
            {
                var vl = io as VolumeLevel;
                if (vl != null)
                    RemoveMicrophone(vl, confirm);
                else
                {
                    var fp = io as FloorPlanControl;
                    if (fp!=null)
                        RemoveFloorplan(fp,confirm);
                }
            }
        }

        public void RemoveCamera(CameraWindow cameraControl, bool confirm)
        {
            if (confirm &&
                MessageBox.Show(LocRm.GetString("Delete")+":" +cameraControl.ObjectName, LocRm.GetString("Confirm"), MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Warning) == DialogResult.Cancel)
                return;

            var dr = DialogResult.No;
            if (confirm)
            {
                dr = MessageBox.Show(LocRm.GetString("DeleteAllAssociatedMedia"), LocRm.GetString("Confirm"),
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);
            }
            if (dr == DialogResult.Cancel)
                return;

            string folder = cameraControl.Dir.Entry + "video\\" + cameraControl.Camobject.directory + "\\";

            cameraControl.ShuttingDown = true;
            cameraControl.MouseDown -= CameraControlMouseDown;
            cameraControl.MouseUp -= CameraControlMouseUp;
            cameraControl.MouseMove -= CameraControlMouseMove;
            cameraControl.DoubleClick -= CameraControlDoubleClick;
            cameraControl.RemoteCommand -= CameraControlRemoteCommand;
            cameraControl.Notification -= ControlNotification;
            if (cameraControl.Recording)
                cameraControl.RecordSwitch(false);

            cameraControl.Disable();
            cameraControl.SaveFileList();

            

            if (cameraControl.VolumeControl != null)
                RemoveMicrophone(cameraControl.VolumeControl, false);

            if (InvokeRequired)
                Invoke(new Delegates.CameraCommandDelegate(RemoveCameraPanel), cameraControl);
            else
                RemoveCameraPanel(cameraControl);

            

            if (dr == DialogResult.Yes)
            {
                try
                {
                    Directory.Delete(folder, true);
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }
        }

        private void RemoveCameraPanel(CameraWindow cameraControl)
        {
            _pnlCameras.Controls.Remove(cameraControl);
            if (!_closing)
            {
                CameraWindow control = cameraControl;
                var oc = Cameras.FirstOrDefault(p => p.id == control.Camobject.id);
                if (oc != null)
                {
                    lock (ThreadLock)
                    {
                        Masterfilelist.RemoveAll(p => p.ObjectId == oc.id && p.ObjectTypeId == 2);
                    }
                    Actions.RemoveAll(p => p.objectid == control.Camobject.id && p.objecttypeid == 2);
                    Cameras.Remove(oc);
                }

                foreach (var ofp in FloorPlans)
                    ofp.needsupdate = true;

                NeedsSync = true;
                SetNewStartPosition();
            }
            Application.DoEvents();
            cameraControl.Dispose();
            if (!_shuttingDown)
            {
                LoadPreviews();
            }
        }

        public void RemoveMicrophone(VolumeLevel volumeControl, bool confirm)
        {
            if (confirm &&
                MessageBox.Show(LocRm.GetString("Delete") + ":" + volumeControl.ObjectName, LocRm.GetString("Confirm"), MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Warning) == DialogResult.Cancel)
                return;

            var dr = DialogResult.No;
            if (confirm)
            {
                dr = MessageBox.Show(LocRm.GetString("DeleteAllAssociatedMedia"), LocRm.GetString("Confirm"),
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);
            }
            if (dr == DialogResult.Cancel)
                return;

            string folder = volumeControl.Dir.Entry + "audio\\" + volumeControl.Micobject.directory + "\\";

            volumeControl.ShuttingDown = true;
            volumeControl.MouseDown -= VolumeControlMouseDown;
            volumeControl.MouseUp -= VolumeControlMouseUp;
            volumeControl.MouseMove -= VolumeControlMouseMove;
            volumeControl.DoubleClick -= VolumeControlDoubleClick;
            volumeControl.RemoteCommand -= VolumeControlRemoteCommand;
            volumeControl.Notification -= ControlNotification;
            if (volumeControl.Recording)
                volumeControl.RecordSwitch(false);

            volumeControl.Disable();
            volumeControl.SaveFileList();

            if (InvokeRequired)
                Invoke(new Delegates.MicrophoneCommandDelegate(RemoveMicrophonePanel), volumeControl);
            else
                RemoveMicrophonePanel(volumeControl);

            if (dr == DialogResult.Yes)
            {
                try
                {
                    Directory.Delete(folder, true);
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }
        }

        private void RemoveMicrophonePanel(VolumeLevel volumeControl)
        {
            _pnlCameras.Controls.Remove(volumeControl);

            if (!_closing)
            {
                var control = volumeControl;
                var om = Microphones.SingleOrDefault(p => p.id == control.Micobject.id);
                if (om != null)
                {
                    lock (ThreadLock)
                    {
                        Masterfilelist.RemoveAll(p => p.ObjectId == om.id && p.ObjectTypeId == 1);
                    }
                    for (var index = 0; index < Cameras.Count(p => p.settings.micpair == om.id); index++)
                    {
                        var oc = Cameras.Where(p => p.settings.micpair == om.id).ToList()[index];
                        oc.settings.micpair = -1;
                    }
                    Actions.RemoveAll(p => p.objectid == control.Micobject.id && p.objecttypeid == 1);
                    Microphones.Remove(om);

                    foreach (var ofp in FloorPlans)
                        ofp.needsupdate = true;
                }
                SetNewStartPosition();
                NeedsSync = true;
            }
            Application.DoEvents();
            volumeControl.Dispose();
        }

        private void RemoveFloorplan(FloorPlanControl fpc, bool confirm)
        {
            if (confirm &&
                MessageBox.Show(LocRm.GetString("Delete") + ":" + fpc.ObjectName, LocRm.GetString("Confirm"), MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Warning) == DialogResult.Cancel)
                return;

            if (fpc.Fpobject?.objects?.@object != null)
            {
                foreach (var o in fpc.Fpobject.objects.@object)
                {
                    switch (o.type)
                    {
                        case "camera":
                            CameraWindow cw = GetCameraWindow(o.id);
                            if (cw != null)
                            {
                                //cw.Location = new Point(Location.X + e.X, Location.Y + e.Y);
                                cw.Highlighted = false;
                                cw.Invalidate();
                            }
                            break;
                        case "microphone":
                            VolumeLevel vl = GetVolumeLevel(o.id);
                            if (vl != null)
                            {
                                vl.Highlighted = false;
                                vl.Invalidate();
                            }
                            break;
                    }
                }
            }
            _pnlCameras.Controls.Remove(fpc);


            if (!_closing)
            {
                objectsFloorplan ofp = FloorPlans.SingleOrDefault(p => p.id == fpc.Fpobject.id);
                if (ofp != null)
                    FloorPlans.Remove(ofp);
                SetNewStartPosition();
                NeedsSync = true;
            }
            fpc.Dispose();
        }

        public void SaveFileData()
        {
            try
            {
                foreach (objectsCamera oc in Cameras)
                {
                    CameraWindow occ = GetCameraWindow(oc.id);
                    occ?.SaveFileList();
                }

                foreach (objectsMicrophone om in Microphones)
                {
                    VolumeLevel omc = GetVolumeLevel(om.id);
                    omc?.SaveFileList();
                }
            }
            catch (Exception ex)
            { Logger.LogException(ex); }
        }

        private void RefreshControls()
        {
            LoadPreviews();
        }

        private void AddCamera(int videoSourceIndex, bool startWizard = false)
        {
            CameraWindow cw = NewCameraWindow(videoSourceIndex);
            TopMost = false;
            var ac = new AddCamera { CameraControl = cw, StartWizard = startWizard, IsNew = true, MainClass = this };
            ac.ShowDialog(this);
            if (ac.DialogResult == DialogResult.OK)
            {
                UnlockLayout();
                SetNewStartPosition();
                if (cw.VolumeControl != null && !cw.VolumeControl.IsEnabled)
                    cw.VolumeControl.Enable();
                NeedsSync = true;
                SaveObjects();
            }
            else
            {
                int cid = cw.Camobject.id;
                cw.Disable();
                _pnlCameras.Controls.Remove(cw);
                cw.Dispose();
                Cameras.RemoveAll(p => p.id == cid);
            }
            ac.Dispose();
            TopMost = Conf.AlwaysOnTop;
        }

        private CameraWindow NewCameraWindow(int videoSourceIndex)
        {
            var oc = new objectsCamera
            {
                alerts = new objectsCameraAlerts(),
                detector = new objectsCameraDetector
                {
                    motionzones =
                        new objectsCameraDetectorZone
                        [0]
                },
                notifications = new objectsCameraNotifications(),
                recorder = new objectsCameraRecorder(),
                schedule = new objectsCameraSchedule { entries = new objectsCameraScheduleEntry[0] },
                settings = new objectsCameraSettings { pip = new objectsCameraSettingsPip { enabled = false, config = ""}, onvif = new objectsCameraSettingsOnvif() },
                ftp = new objectsCameraFtp(),
                savelocal = new objectsCameraSavelocal(),
                id = -1,
                directory = RandomString(5),
                ptz = -1,
                x = Convert.ToInt32(Random.NextDouble() * 100),
                y = Convert.ToInt32(Random.NextDouble() * 100),
                ptzschedule = new objectsCameraPtzschedule
                {
                    active = false,
                    entries = new objectsCameraPtzscheduleEntry[] { }
                }                
            };

            string friendlyName = LocRm.GetString("Camera") + " " + NextCameraId;

            string t = friendlyName;
            int i = 1;
            while (Cameras.FirstOrDefault(p => p.name == t) != null)
            {
                t = friendlyName + " (" + i + ")";
                i++;
            }

            oc.name = t;


            oc.flipx = oc.flipy = false;
            oc.width = 320;
            oc.height = 240;
            oc.description = "";
            oc.resolution = "320x240";
            oc.newrecordingcount = 0;

            oc.alerts.active = true;
            oc.alerts.mode = "movement";
            oc.alerts.alertoptions = "false,false";
            oc.alerts.objectcountalert = 1;
            oc.alerts.minimuminterval = 180;
            oc.alerts.processmode = "continuous";
            oc.alerts.pluginconfig = "";
            oc.alerts.trigger = "";

            oc.notifications.sendemail = false;
            oc.notifications.sendsms = false;
            oc.notifications.sendmms = false;
            oc.notifications.emailgrabinterval = 0;

            oc.ftp.enabled = false;
            oc.ftp.port = 21;
            oc.ftp.mode = 0;
            oc.ftp.server = "ftp://";
            oc.ftp.interval = 10;
            oc.ftp.intervalnew = 10;
            oc.ftp.filename = "mylivecamerafeed.jpg";
            oc.ftp.ready = true;
            oc.ftp.text = "www.ispyconnect.com";
            oc.ftp.quality = 75;
            oc.ftp.counter = 0;
            oc.ftp.countermax = 20;
            oc.ftp.minimumdelay = 0;

            oc.savelocal.enabled = false;
            oc.savelocal.mode = 0;
            oc.savelocal.intervalnew = 10;
            oc.savelocal.filename = "{0:yyyy-MM-dd_HH-mm-ss_fff}.jpg";
            oc.savelocal.text = "www.ispyconnect.com";
            oc.savelocal.quality = 75;
            oc.savelocal.counter = 0;
            oc.savelocal.countermax = 20;
            oc.savelocal.minimumdelay = 0;

            oc.schedule.active = false;

            oc.settings.active = false;
            oc.settings.deleteavi = true;
            oc.settings.ffmpeg = Conf.FFMPEG_Camera;
            oc.settings.emailaddress = EmailAddress;
            oc.settings.smsnumber = MobileNumber;
            oc.settings.suppressnoise = true;
            oc.settings.login = "";
            oc.settings.password = "";
            oc.settings.useragent = "Mozilla/5.0";
            oc.settings.sourceindex = videoSourceIndex;
            oc.settings.micpair = -1;
            oc.settings.maxframerate = 10;
            oc.settings.maxframeraterecord = 10;
            oc.settings.ptzautotrack = false;
            oc.settings.framerate = 10;
            oc.settings.timestamplocation = 1;
            oc.settings.ptztimetohome = 100;
            oc.settings.ptzchannel = "0";
            oc.settings.timestampformatter = "FPS: {FPS} {0:G} ";
            oc.settings.timestampfontsize = 10;
            oc.settings.notifyondisconnect = false;
            oc.settings.ptzautohomedelay = 30;
            oc.settings.accessgroups = "";
            oc.settings.nobuffer = true;
            oc.settings.reconnectinterval = 0;
            oc.settings.timestampforecolor = "255,255,255";
            oc.settings.timestampbackcolor = "70,70,70";
            oc.settings.timestampfont = FontXmlConverter.ConvertToString(Drawfont);
            oc.settings.timestampshowback = true;

            oc.settings.youtube = new objectsCameraSettingsYoutube
            {
                category = Conf.YouTubeDefaultCategory,
                tags = "iSpy, Motion Detection, Surveillance",
                @public = true
            };
            oc.settings.cloudprovider = new objectsCameraSettingsCloudprovider();
            
            oc.settings.storagemanagement = new objectsCameraSettingsStoragemanagement
            {
                enabled = false,
                maxage = 72,
                maxsize = 1024

            };
            oc.settings.tokenconfig = new objectsCameraSettingsTokenconfig();

            oc.alertevents = new objectsCameraAlertevents { entries = new objectsCameraAlerteventsEntry[] { } };

            oc.settings.desktopresizeheight = 480; //old
            oc.settings.desktopresizewidth = 640;

            oc.settings.resizeHeight = 0; //auto
            oc.settings.resizeWidth = 640;

            oc.settings.resize = true;
            oc.settings.directoryIndex = Conf.MediaDirectories.First().ID;

            oc.settings.vlcargs = "--rtsp-caching=100";

            oc.detector.recordondetect = Conf.DefaultRecordOnDetect;
            oc.detector.recordonalert = Conf.DefaultRecordOnAlert;
            oc.detector.keepobjectedges = false;
            oc.detector.nomovementintervalnew = oc.detector.nomovementinterval = 30;
            oc.detector.movementintervalnew = oc.detector.movementinterval = 0;
            oc.detector.processframeinterval = 200;

            oc.detector.calibrationdelay = 15;
            oc.detector.color = ColorTranslator.ToHtml(Conf.TrackingColor.ToColor());
            oc.detector.type = "Two Frames";
            oc.detector.postprocessor = "None";
            oc.detector.minsensitivity = 20;
            oc.detector.maxsensitivity = 100;
            oc.detector.minwidth = 20;
            oc.detector.minheight = 20;
            oc.detector.highlight = true;

            oc.recorder.bufferseconds = 0;
            oc.recorder.inactiverecord = 8;
            oc.recorder.timelapse = 0;
            oc.recorder.timelapseframes = 0;
            oc.recorder.maxrecordtime = 900;
            oc.recorder.timelapsesave = 60;
            oc.recorder.quality = 8;
            oc.recorder.timelapseframerate = 5;
            oc.recorder.crf = true;

            oc.settings.audioport = 80;
            oc.settings.audiomodel = "None";
            oc.settings.audioip = "";
            oc.rotateMode = "RotateNoneFlipNone";

            var cameraControl = new CameraWindow(oc,this) { BackColor = Conf.BackColor.ToColor() };
            _pnlCameras.Controls.Add(cameraControl);

            cameraControl.Location = new Point(oc.x, oc.y);
            cameraControl.Size = new Size(320, 240);
            cameraControl.AutoSize = true;
            cameraControl.BringToFront();
            SetCameraEvents(cameraControl);
            if (Conf.AutoLayout)
                _pnlCameras.LayoutObjects(0, 0);

            cameraControl.Tag = GetControlIndex();
            LayoutPanel.NeedsRedraw = true;

            return cameraControl;
        }

        public int AddMicrophone(int audioSourceIndex)
        {
            VolumeLevel vl = NewVolumeLevel(audioSourceIndex);
            TopMost = false;
            var am = new AddMicrophone { VolumeLevel = vl, IsNew = true, MainClass = this };
            am.ShowDialog(this);

            int micid = -1;

            if (am.DialogResult == DialogResult.OK)
            {
                UnlockLayout();
                micid = am.VolumeLevel.Micobject.id = NextMicrophoneId;
                AddObject(vl.Micobject);
                SetNewStartPosition();
                NeedsSync = true;
                SaveObjects();
            }
            else
            {
                vl.Disable();
                _pnlCameras.Controls.Remove(vl);
                vl.Dispose();
            }
            am.Dispose();
            TopMost = Conf.AlwaysOnTop;
            return micid;

        }

        private VolumeLevel NewVolumeLevel(int audioSourceIndex)
        {
            var om = new objectsMicrophone
            {
                alerts = new objectsMicrophoneAlerts(),
                detector = new objectsMicrophoneDetector(),
                notifications = new objectsMicrophoneNotifications(),
                recorder = new objectsMicrophoneRecorder(),
                schedule = new objectsMicrophoneSchedule
                {
                    entries
                        =
                        new objectsMicrophoneScheduleEntry
                        [
                        0
                        ]
                },
                settings = new objectsMicrophoneSettings(),
                id = -1,
                directory = RandomString(5),
                x = Convert.ToInt32(Random.NextDouble() * 100),
                y = Convert.ToInt32(Random.NextDouble() * 100),
                width = 160,
                height = 40,
                description = "",
                newrecordingcount = 0
            };

            string friendlyName = LocRm.GetString("Microphone") + " " + NextMicrophoneId;

            string t = friendlyName;
            int i = 1;
            while (Microphones.FirstOrDefault(p => p.name == t) != null)
            {
                t = friendlyName + " (" + i + ")";
                i++;
            }

            om.name = t;

            om.settings.typeindex = audioSourceIndex;
            om.settings.deletewav = true;
            om.settings.ffmpeg = Conf.FFMPEG_Microphone;
            om.settings.buffer = 4;
            om.settings.samples = 8000;
            om.settings.bits = 16;
            om.settings.gain = 100;
            om.settings.channels = 1;
            om.settings.decompress = true;
            om.settings.smsnumber = MobileNumber;
            om.settings.emailaddress = EmailAddress;
            om.settings.active = false;
            om.settings.notifyondisconnect = false;
            om.settings.directoryIndex = Conf.MediaDirectories.First().ID;
            om.settings.vlcargs = VlcHelper.VLCAvailable ? "--rtsp-caching=100" : "";

            om.settings.storagemanagement = new objectsMicrophoneSettingsStoragemanagement
            {
                enabled = false,
                maxage = 72,
                maxsize = 1024
            };

            om.detector.sensitivity = 60;
            om.detector.minsensitivity = 60;
            om.detector.maxsensitivity = 100;

            om.detector.nosoundinterval = 30;
            om.detector.soundinterval = 0;
            om.detector.recordondetect = Conf.DefaultRecordOnDetect;
            om.detector.recordonalert = Conf.DefaultRecordOnAlert;

            om.alerts.mode = "sound";
            om.alerts.minimuminterval = 180;
            om.alerts.executefile = "";
            om.alerts.active = true;
            om.alerts.alertoptions = "false,false";
            om.alerts.trigger = "";

            om.recorder.inactiverecord = 5;
            om.recorder.maxrecordtime = 900;

            om.notifications.sendemail = false;
            om.notifications.sendsms = false;

            om.schedule.active = false;
            om.alertevents = new objectsMicrophoneAlertevents { entries = new objectsMicrophoneAlerteventsEntry[] { } };

            var volumeControl = new VolumeLevel(om,this) { BackColor = Conf.BackColor.ToColor() };
            _pnlCameras.Controls.Add(volumeControl);

            volumeControl.Location = new Point(om.x, om.y);
            volumeControl.Size = new Size(160, 40);
            volumeControl.BringToFront();
            SetMicrophoneEvents(volumeControl);

            if (Conf.AutoLayout)
                _pnlCameras.LayoutObjects(0, 0);

            volumeControl.Tag = GetControlIndex();
            LayoutPanel.NeedsRedraw = true;
            return volumeControl;
        }

        private int GetControlIndex()
        {
            int i = 0;
            while (true)
            {

                bool b = _pnlCameras.Controls.Cast<Control>().Any(c => c.Tag as int? == i);
                if (!b)
                {
                    return i;
                }
                i++;
            }

        }

        private void AddFloorPlan()
        {
            var ofp = new objectsFloorplan
            {
                objects = new objectsFloorplanObjects { @object = new objectsFloorplanObjectsEntry[0] },
                id = -1,
                image = "",
                height = 480,
                width = 640,
                x = Convert.ToInt32(Random.NextDouble() * 100),
                y = Convert.ToInt32(Random.NextDouble() * 100),
                name = LocRm.GetString("FloorPlan") + " " + NextFloorPlanId
            };

            var fpc = new FloorPlanControl(ofp, this) { BackColor = Conf.BackColor.ToColor() };
            _pnlCameras.Controls.Add(fpc);

            fpc.Location = new Point(ofp.x, ofp.y);
            fpc.Size = new Size(320, 240);
            fpc.BringToFront();
            fpc.Tag = GetControlIndex();

            LayoutPanel.NeedsRedraw = true;

            var afp = new AddFloorPlan { Fpc = fpc, Owner = this, MainClass = this };
            afp.ShowDialog(this);
            if (afp.DialogResult == DialogResult.OK)
            {
                UnlockLayout();
                afp.Fpc.Fpobject.id = NextFloorPlanId;
                AddObject(ofp);
                SetFloorPlanEvents(fpc);
                SetNewStartPosition();
                fpc.Invalidate();
            }
            else
            {
                _pnlCameras.Controls.Remove(fpc);
                fpc.Dispose();
            }
            afp.Dispose();
        }

        private void SetCameraEvents(CameraWindow cameraControl)
        {
            cameraControl.MouseDown += CameraControlMouseDown;
            cameraControl.MouseWheel += CameraControlMouseWheel;
            cameraControl.MouseUp += CameraControlMouseUp;
            cameraControl.MouseMove += CameraControlMouseMove;
            cameraControl.DoubleClick += CameraControlDoubleClick;
            cameraControl.RemoteCommand += CameraControlRemoteCommand;
            cameraControl.Notification += ControlNotification;
            cameraControl.FileListUpdated += CameraControlFileListUpdated;
        }

        void CameraControlFileListUpdated(object sender)
        {
            lock (ThreadLock)
            {
                var cw = sender as CameraWindow;
                if (cw != null)
                {
                    try
                    {
                        Masterfilelist.RemoveAll(p => p.ObjectId == cw.ObjectID && p.ObjectTypeId == 2);
                        var l = cw.FileList.ToList();
                        foreach (var ff in l)
                        {
                            Masterfilelist.Add(new FilePreview(ff.Filename, ff.DurationSeconds, cw.ObjectName,
                                ff.CreatedDateTicks, 2, cw.ObjectID, ff.MaxAlarm, ff.IsTimelapse, ff.IsMergeFile));
                        }
                        if (!cw.LoadedFiles)
                        {
                            cw.LoadedFiles = true;
                            //last one?
                            bool all = true;
                            foreach (Control c in _pnlCameras.Controls)
                            {
                                var cameraWindow = c as CameraWindow;
                                if (cameraWindow != null)
                                {
                                    if (!cameraWindow.LoadedFiles)
                                    {
                                        all = false;
                                        break;
                                    }
                                }
                                var volumeLevel = c as VolumeLevel;
                                if (volumeLevel != null)
                                {
                                    if (!volumeLevel.LoadedFiles)
                                    {
                                        all = false;
                                        break;
                                    }
                                }
                            }
                            if (all)
                            {
                                flowPreview.Loading = false;
                                LoadPreviews();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex,"FileListUpdated - "+cw.ObjectName);
                    }
                }
                
            }

        }


        public PreviewBox AddPreviewControl(FilePreview fp, Image thumb, string movieName, string name)
        {
            var pb = new PreviewBox(fp.ObjectTypeId, fp.ObjectId, this) {Image = thumb, Duration = fp.Duration};
            pb.Width = pb.Image.Width;
            pb.Height = pb.Image.Height + 20;
            pb.Cursor = Cursors.Hand;
            pb.Selected = false;
            pb.FileName = movieName;
            var dt = new DateTime(fp.CreatedDateTicks);
            pb.CreatedDate = dt;
            pb.MouseDown += PbMouseDown;
            pb.MouseEnter += PbMouseEnter;
            string txt = name + ": " + dt.ToString(CultureInfo.CurrentUICulture);
            pb.DisplayName = txt;
            pb.IsMerged = fp.IsMerged;
            lock (ThreadLock)
            {
                flowPreview.Controls.Add(pb);
                
            }
            return pb;
        }
        public PreviewBox AddPreviewControl(FilePreview fp, string thumb, string movieName, string name)
        {
            Image bmp;
            try
            {
                if (!File.Exists(thumb))
                {
                    bmp = Resources.notfound;
                }
                else
                {
                    using (var f = File.Open(thumb, FileMode.Open, FileAccess.Read))
                    {
                        bmp = Image.FromStream(f);
                        f.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
            return AddPreviewControl(fp, bmp, movieName, name);
        }


        //public PreviewBox AddPreviewControl(int otid, int oid, Image thumb, string movieName, int duration, DateTime createdDate, string name)
        //{
        //    var pb = new PreviewBox(otid,oid,this) { Image = thumb };

        //    lock (ThreadLock)
        //    {
        //        pb.Duration = duration;
        //        pb.Width = pb.Image.Width;
        //        pb.Height = pb.Image.Height + 20;
        //        pb.Cursor = Cursors.Hand;
        //        pb.Selected = false;
        //        pb.FileName = movieName;
        //        pb.CreatedDate = createdDate;
        //        pb.MouseDown += PbMouseDown;
        //        pb.MouseEnter += PbMouseEnter;
        //        string txt = name + ": " + createdDate.ToString(CultureInfo.CurrentUICulture);
        //        pb.DisplayName = txt;
        //        flowPreview.Controls.Add(pb);
        //    }

        //    return pb;
        //}

        //public PreviewBox AddPreviewControl(int otid, int oid, string thumbname, string movieName, int duration, DateTime createdDate, string name)
        //{
        //    string thumb = thumbname;
        //    Image bmp;
        //    try
        //    {
        //        if (!File.Exists(thumb))
        //        {
        //            bmp = Resources.notfound;
        //        }
        //        else
        //        {
        //            using (var f = File.Open(thumb, FileMode.Open, FileAccess.Read))
        //            {
        //                bmp = Image.FromStream(f);
        //                f.Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogException(ex);
        //        return null;
        //    }
        //    return AddPreviewControl(otid,oid,bmp, movieName, duration, createdDate, name);
        //}


        private DateTime _lastOver = DateTime.MinValue;

        void PbMouseEnter(object sender, EventArgs e)
        {
            var pb = (PreviewBox)sender;
            tsslMediaInfo.Text = pb.DisplayName;
            _lastOver = Helper.Now;
        }

        void PbMouseDown(object sender, MouseEventArgs e)
        {
            var ctrl = sender as PreviewBox;
            if (ctrl != null)
            {
                ctrl.Focus();
                switch (e.Button)
                {
                    case MouseButtons.Right:
                        ContextTarget = ctrl;
                        ctrl.Selected = true;
                        ctxtPlayer.Show(ctrl, new Point(e.X, e.Y));
                        break;
                }
            }
            _lastClicked = flowPreview;
        }

        public CameraWindow GetCameraWindow(int cameraId)
        {
            for (int index = 0; index < _pnlCameras.Controls.Count; index++)
            {
                var cw = _pnlCameras.Controls[index] as CameraWindow;
                if (cw != null && cw.Camobject.id == cameraId)
                    return cw;
            }
            return null;
        }

        public ISpyControl GetISpyControl(int ot, int id)
        {
            switch (ot)
            {
                case 1:
                    return GetVolumeLevel(id);
                case 2:
                    return GetCameraWindow(id);
                case 3:
                    return GetFloorPlan(id);
            }
            return null;
        }
        public List<ISpyControl> ControlList
        {
            get
            {
                var l = new List<ISpyControl>();
                for (int index = 0; index < _pnlCameras.Controls.Count; index++)
                {
                    var io = _pnlCameras.Controls[index] as ISpyControl;
                    if (io != null)
                        l.Add(io);
                }
                return l;
            }
        }

        public VolumeLevel GetVolumeLevel(int microphoneId)
        {
            for (int index = 0; index < _pnlCameras.Controls.Count; index++)
            {
                var vl = _pnlCameras.Controls[index] as VolumeLevel;
                if (vl != null && vl.Micobject.id == microphoneId)
                    return vl;
            }
            return null;
        }

        private void SaveObjects(string fileName="")
        {
            if (_shuttingDown)
                return;

            bool rename = false;
            if (fileName == "")
            {
                fileName = Program.AppDataPath + @"XML\objects_new.xml";
                rename = true;
            }
            var c = new objects { Version = Convert.ToInt32(Application.ProductVersion.Replace(".", "")), actions = new objectsActions {entries = _actions.ToArray()} };
            foreach (objectsCamera oc in Cameras)
            {
                CameraWindow occ = GetCameraWindow(oc.id);
                if (occ != null)
                {
                    oc.width = occ.Width;
                    oc.height = occ.Height;
                    oc.x = occ.Location.X;
                    oc.y = occ.Location.Y;
                }
            }
            c.cameras = Cameras.ToArray();
            foreach (objectsMicrophone om in Microphones)
            {
                VolumeLevel omc = GetVolumeLevel(om.id);
                if (omc != null)
                {
                    om.width = omc.Width;
                    om.height = omc.Height;
                    om.x = omc.Location.X;
                    om.y = omc.Location.Y;
                }
            }
            c.microphones = Microphones.ToArray();
            foreach (objectsFloorplan of in FloorPlans)
            {
                FloorPlanControl fpc = GetFloorPlan(of.id);
                if (fpc != null)
                {
                    of.width = fpc.Width;
                    of.height = fpc.Height;
                    of.x = fpc.Location.X;
                    of.y = fpc.Location.Y;
                }
            }
            c.floorplans = FloorPlans.ToArray();
            c.remotecommands = RemoteCommands.ToArray();
            c.schedule = new objectsSchedule {entries = _schedule.ToArray()};
            lock (ThreadLock)
            {
                var s = new XmlSerializer(typeof(objects));
                var sb = new StringBuilder();
                using (var writer = new StringWriter(sb))
                {
                    try
                    {
                        s.Serialize(writer, c);
                        File.WriteAllText(fileName, sb.ToString(), Encoding.UTF8);
                        if (rename)
                        {
                            var src = fileName;
                            var dest = Program.AppDataPath + @"XML\objects.xml";
                            var backup = Program.AppDataPath + @"XML\objects_bak.xml";
                            if (File.Exists(dest))
                            {
                                File.Delete(backup);
                                File.Move(dest, backup);
                            }
                            File.Move(src, dest);
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.LogException(e);
                    }
                }
            }
            _currentFileName = fileName;
        }

        public static void SaveConfig()
        {
            lock (ThreadLock)
            {
                string fileName = Program.AppDataPath + @"XML\config.xml";
                //save configuration

                var s = new XmlSerializer(typeof(configuration));
                var sb = new StringBuilder();
                using (var writer = new StringWriter(sb))
                {
                    try
                    {
                        string pwd = _conf.WSPassword;

                        //save the encrypted form
                        if (!string.IsNullOrEmpty(_conf.WSPassword))
                        {

                            _conf.WSPassword = EncDec.EncryptData(_conf.WSPassword, "582df37b-b7cc-43f7-a442-30a2b188a888");
                            _conf.WSPasswordEncrypted = true;
                        }
                        else
                        {
                            _conf.WSPassword = "";
                            _conf.WSPasswordEncrypted = false;
                        }
                        s.Serialize(writer, Conf);

                        //revert to clear text for in memory lookups
                        _conf.WSPassword = pwd;
                        File.WriteAllText(fileName, sb.ToString(), Encoding.UTF8);
                    }
                    catch (Exception e)
                    {
                        Logger.LogException(e);
                    }
                }
            }
        }

        private void LoadObjectList(string fileName)
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
            LoadObjects(fileName);
            RenderObjects();
            Application.DoEvents();
            LayoutPanel.NeedsRedraw = true;
            try
            {
                _houseKeepingTimer.Start();
            }
            catch (Exception)
            {
                // ignored
            }

            if (RemoteCommands.Any(p => p.inwindow))
            {
                ShowCommandButtonWindow();
            }
        }

        public object AddObjectExternal(int objectTypeId, int sourceIndex, int width, int height, string name, string url)
        {
            if (!VlcHelper.VLCAvailable && sourceIndex == 5)
                return null;
            object oret = null;
            switch (objectTypeId)
            {
                case 2:
                    
                    if (name == "") name = "Camera " + NextCameraId;
                    if (InvokeRequired)
                        oret =  Invoke(new Delegates.AddObjectExternalDelegate(AddCameraExternal), sourceIndex, url, width, height,
                                name);
                    else
                        oret = AddCameraExternal(sourceIndex, url, width, height, name);

                    break;
                case 1:
                    
                    if (name == "") name = "Mic " + NextMicrophoneId;
                    if (InvokeRequired)
                        oret = Invoke(new Delegates.AddObjectExternalDelegate(AddMicrophoneExternal), sourceIndex, url, width, height,
                                name);
                    else
                        oret = AddMicrophoneExternal(sourceIndex, url, width, height, name);

                    break;
            }
            NeedsSync = true;
            return oret;
        }

        private CameraWindow AddCameraExternal(int sourceIndex, string url, int width, int height, string name)
        {
            CameraWindow cw = NewCameraWindow(sourceIndex);
            cw.Camobject.settings.resizeWidth = width;
            cw.Camobject.settings.resizeHeight = height;
            cw.Camobject.settings.resize = true;
            cw.Camobject.name = name;
            cw.Camobject.settings.directoryIndex = Conf.MediaDirectories.First().ID;

            cw.Camobject.settings.videosourcestring = url;

            cw.Camobject.id = NextCameraId;
            AddObject(cw.Camobject);

            var dir = Helper.GetMediaDirectory(2, cw.Camobject.id);
            string path = dir + "video\\" + cw.Camobject.directory + "\\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            path = dir + "video\\" + cw.Camobject.directory + "\\thumbs\\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            path = dir + "video\\" + cw.Camobject.directory + "\\grabs\\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            cw.Camobject.settings.accessgroups = "";


            SetNewStartPosition();
            cw.Enable();
            cw.NeedSizeUpdate = true;
            return cw;
        }

        private VolumeLevel AddMicrophoneExternal(int sourceIndex, string url, int width, int height, string name)
        {
            VolumeLevel vl = NewVolumeLevel(sourceIndex);
            vl.Micobject.name = name;
            vl.Micobject.settings.sourcename = url;

            vl.Micobject.id = NextMicrophoneId;
            AddObject(vl.Micobject);

            string path = Helper.GetMediaDirectory(1, vl.Micobject.id) + "audio\\" + vl.Micobject.directory + "\\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            vl.Micobject.settings.accessgroups = "";
            SetNewStartPosition();
            vl.Enable();
            return vl;
        }

        internal VolumeLevel AddCameraMicrophone(int cameraid, string name)
        {
            if (cameraid == -1)
                cameraid = NextCameraId;
            VolumeLevel vl = NewVolumeLevel(4);
            vl.Micobject.name = name;
            vl.Micobject.settings.sourcename = cameraid.ToString(CultureInfo.InvariantCulture);
            vl.Micobject.id = NextMicrophoneId;
            AddObject(vl.Micobject);
            string path = Helper.GetMediaDirectory(1, vl.Micobject.id) + "audio\\" + vl.Micobject.directory + "\\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            vl.Micobject.settings.accessgroups = "";
            SetNewStartPosition();
            return vl;
        }

        #region CameraEvents

        private void CameraControlMouseMove(object sender, MouseEventArgs e)
        {
            if (Resizing) return;
            var cameraControl = (CameraWindow)sender;
            if (e.Button == MouseButtons.Left && !MainForm.LockLayout)
            {
                int newLeft = cameraControl.Left + (e.X - cameraControl.Camobject.x);
                int newTop = cameraControl.Top + (e.Y - cameraControl.Camobject.y);
                if (newLeft < 0) newLeft = 0;
                if (newTop < 0) newTop = 0;
                if (newLeft + cameraControl.Width > 5 && newLeft < ClientRectangle.Width - 5)
                {
                    cameraControl.Left = newLeft;
                }
                if (newTop + cameraControl.Height > 5 && newTop < ClientRectangle.Height - 50)
                {
                    cameraControl.Top = newTop;
                }
            }

        }

        private void CameraControlMouseDown(object sender, MouseEventArgs e)
        {
            _lastClicked = _pnlCameras;
            if (Resizing) return;
            var cameraControl = (CameraWindow)sender;
            cameraControl.Focus();

            switch (e.Button)
            {
                case MouseButtons.Left:
                    cameraControl.Camobject.x = e.X;
                    cameraControl.Camobject.y = e.Y;
                    cameraControl.BringToFront();
                    cameraControl.VolumeControl?.BringToFront();
                    break;
                case MouseButtons.Right:
                    ContextTarget = cameraControl;
                    pluginCommandsToolStripMenuItem.Visible = false;
                    
                    _recordNowToolStripMenuItem.Visible = false;
                    _listenToolStripMenuItem.Visible = false;
                    _recordNowToolStripMenuItem.Visible = false;
                    _takePhotoToolStripMenuItem.Visible = false;
                    _viewMediaOnAMobileDeviceToolStripMenuItem.Visible = _viewMediaToolStripMenuItem.Visible = Helper.HasFeature(Enums.Features.Access_Media);
                    _applyScheduleToolStripMenuItem1.Visible = true;
                    _resetRecordingCounterToolStripMenuItem.Visible = true;
                    openWebInterfaceToolStripMenuItem.Visible = cameraControl.SupportsWebInterface;
                    _resetRecordingCounterToolStripMenuItem.Text =
                        $"{LocRm.GetString("ResetRecordingCounter")} ({cameraControl.Camobject.newrecordingcount})";
                    pTZToolStripMenuItem.Visible = false;

                    //switches
                    switchToolStripMenuItem.Visible = true;

                    onToolStripMenuItem.Visible = !cameraControl.Camobject.settings.active;
                    offToolStripMenuItem.Visible = cameraControl.Camobject.settings.active;

                    alertsOnToolStripMenuItem1.Visible = !cameraControl.Camobject.alerts.active;
                    alertsOffToolStripMenuItem.Visible = cameraControl.Camobject.alerts.active;

                    scheduleOnToolStripMenuItem.Visible = !cameraControl.Camobject.schedule.active;
                    scheduleOffToolStripMenuItem.Visible = cameraControl.Camobject.schedule.active;

                    pTZScheduleOnToolStripMenuItem.Visible = !cameraControl.Camobject.ptzschedule.active;
                    pTZScheduleOffToolStripMenuItem.Visible = cameraControl.Camobject.ptzschedule.active;

                    if (cameraControl.Camobject.settings.active)
                    {
                        if (Helper.HasFeature(Enums.Features.Recording))
                        {
                            _recordNowToolStripMenuItem.Visible = true;
                            _takePhotoToolStripMenuItem.Visible = true;
                        }
                        if (Helper.HasFeature(Enums.Features.Save_Frames))
                            _takePhotoToolStripMenuItem.Visible = true;

                        

                        if (cameraControl.Camobject.ptz > -1)
                        {
                            pTZToolStripMenuItem.Visible = true;
                            while (pTZToolStripMenuItem.DropDownItems.Count > 1)
                                pTZToolStripMenuItem.DropDownItems.RemoveAt(1);

                            PTZSettings2Camera ptz = PTZs.SingleOrDefault(p => p.id == cameraControl.Camobject.ptz);
                            if (ptz?.ExtendedCommands?.Command != null)
                            {
                                foreach (var extcmd in ptz.ExtendedCommands.Command)
                                {
                                    ToolStripItem tsi = new ToolStripMenuItem
                                                        {
                                                            Text = extcmd.Name,
                                                            Tag =
                                                                cameraControl.Camobject.id + "|" + extcmd.Value
                                                        };
                                    tsi.Click += TsiClick;
                                    pTZToolStripMenuItem.DropDownItems.Add(tsi);
                                }
                            }
                        }

                        if (cameraControl.Camera?.Plugin != null)
                        {
                            pluginCommandsToolStripMenuItem.Visible = true;

                            while (pluginCommandsToolStripMenuItem.DropDownItems.Count > 1)
                                pluginCommandsToolStripMenuItem.DropDownItems.RemoveAt(1);

                            var pc = cameraControl.PluginCommands;
                            if (pc != null)
                            {
                                foreach (var c in pc)
                                {
                                    ToolStripItem tsi = new ToolStripMenuItem
                                    {
                                        Text = c,
                                        Tag =
                                            cameraControl.Camobject.id + "|" + c
                                    };
                                    tsi.Click += PCClick;
                                    pluginCommandsToolStripMenuItem.DropDownItems.Add(tsi);
                                }
                            }
                        }

                    }
                    _recordNowToolStripMenuItem.Text =
                        LocRm.GetString(cameraControl.Recording ? "StopRecording" : "StartRecording");
                    ctxtMnu.Show(cameraControl, new Point(e.X, e.Y));
                    break;

            }
        }

        private void TsiClick(object sender, EventArgs e)
        {
            string[] cfg = ((ToolStripMenuItem)sender).Tag.ToString().Split('|');
            int camid = Convert.ToInt32(cfg[0]);
            var cw = GetCameraWindow(camid);
            if (cw?.PTZ != null)
            {
                cw.Calibrating = true;
                cw.PTZ.SendPTZCommand(cfg[1]);
            }
        }

        private void PCClick(object sender, EventArgs e)
        {
            string[] cfg = ((ToolStripMenuItem)sender).Tag.ToString().Split('|');
            int camid = Convert.ToInt32(cfg[0]);
            var cw = GetCameraWindow(camid);
            if (cw?.PluginCommands != null)
            {
                cw.ExecutePluginCommand(cfg[1]);
            }
        }

        private static void CameraControlMouseWheel(object sender, MouseEventArgs e)
        {
            var cameraControl = (CameraWindow)sender;

            cameraControl.PTZNavigate = false;
            if (cameraControl.PTZ != null)
            {


                if (!cameraControl.PTZ.DigitalZoom)
                {
                    cameraControl.Calibrating = true;
                    cameraControl.PTZ.SendPTZCommand(e.Delta > 0 ? Enums.PtzCommand.ZoomIn : Enums.PtzCommand.ZoomOut);
                    cameraControl.PTZ.CheckSendStop();
                }
                else
                {
                    Rectangle r = cameraControl.Camera.ViewRectangle;
                    //map location to point in the view rectangle
                    var ox =
                        Convert.ToInt32((Convert.ToDouble(e.Location.X) / Convert.ToDouble(cameraControl.Width)) *
                                        Convert.ToDouble(r.Width));
                    var oy =
                        Convert.ToInt32((Convert.ToDouble(e.Location.Y) / Convert.ToDouble(cameraControl.Height)) *
                                        Convert.ToDouble(r.Height));

                    cameraControl.Camera.ZPoint = new Point(r.Left + ox, r.Top + oy);
                    var f = cameraControl.Camera.ZFactor;
                    if (e.Delta > 0)
                    {
                        f += 0.2f;
                    }
                    else
                        f -= 0.2f;
                    if (f < 1)
                        f = 1;
                    cameraControl.Camera.ZFactor = f;
                }
                ((HandledMouseEventArgs)e).Handled = true;

            }
        }

        private void CameraControlMouseUp(object sender, MouseEventArgs e)
        {
            if (Resizing) return;
            var cameraControl = (CameraWindow)sender;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    cameraControl.Camobject.x = cameraControl.Left;
                    cameraControl.Camobject.y = cameraControl.Top;
                    break;
            }
        }

        private void CameraControlDoubleClick(object sender, EventArgs e)
        {
            _pnlCameras.Maximise(sender);
        }

        #endregion

        #region VolumeEvents

        private void VolumeControlMouseDown(object sender, MouseEventArgs e)
        {
            _lastClicked = _pnlCameras;
            if (Resizing) return;
            var volumeControl = (VolumeLevel)sender;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (!volumeControl.Paired)
                    {
                        volumeControl.Micobject.x = e.X;
                        volumeControl.Micobject.y = e.Y;
                    }
                    volumeControl.BringToFront();
                    if (volumeControl.Paired)
                    {
                        CameraWindow cw =
                            GetCameraWindow(Cameras.Single(p => p.settings.micpair == volumeControl.Micobject.id).id);
                        cw.BringToFront();
                    }
                    break;
                case MouseButtons.Right:
                    ContextTarget = volumeControl;
                    pluginCommandsToolStripMenuItem.Visible = false;
                    _listenToolStripMenuItem.Visible = true;
                    _takePhotoToolStripMenuItem.Visible = openWebInterfaceToolStripMenuItem.Visible = false;
                    _resetRecordingCounterToolStripMenuItem.Visible = true;
                    _applyScheduleToolStripMenuItem1.Visible = true;
                    pTZToolStripMenuItem.Visible = false;
                    _viewMediaOnAMobileDeviceToolStripMenuItem.Visible = _viewMediaToolStripMenuItem.Visible = Helper.HasFeature(Enums.Features.Access_Media);
                    _resetRecordingCounterToolStripMenuItem.Text =
                        $"{LocRm.GetString("ResetRecordingCounter")} ({volumeControl.Micobject.newrecordingcount})";

                    if (volumeControl.Listening)
                    {
                        _listenToolStripMenuItem.Text = LocRm.GetString("StopListening");
                        _listenToolStripMenuItem.Image = Resources.listenoff2;
                    }
                    else
                    {
                        _listenToolStripMenuItem.Text = LocRm.GetString("Listen");
                        _listenToolStripMenuItem.Image = Resources.listen2;
                    }
                    _recordNowToolStripMenuItem.Visible = false;

                    //switches
                    switchToolStripMenuItem.Visible = true;

                    onToolStripMenuItem.Visible = !volumeControl.Micobject.settings.active;
                    offToolStripMenuItem.Visible = volumeControl.Micobject.settings.active;

                    alertsOnToolStripMenuItem1.Visible = !volumeControl.Micobject.alerts.active;
                    alertsOffToolStripMenuItem.Visible = volumeControl.Micobject.alerts.active;

                    scheduleOnToolStripMenuItem.Visible = !volumeControl.Micobject.schedule.active;
                    scheduleOffToolStripMenuItem.Visible = volumeControl.Micobject.schedule.active;

                    pTZScheduleOnToolStripMenuItem.Visible = pTZScheduleOffToolStripMenuItem.Visible = false;


                    if (volumeControl.Micobject.settings.active)
                    {
                        if (Helper.HasFeature(Enums.Features.Recording))
                        {
                            _recordNowToolStripMenuItem.Visible = true;
                        }

                        _listenToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        _listenToolStripMenuItem.Enabled = false;
                    }
                    _recordNowToolStripMenuItem.Text =
                        LocRm.GetString(volumeControl.ForcedRecording ? "StopRecording" : "StartRecording");
                    ctxtMnu.Show(volumeControl, new Point(e.X, e.Y));
                    break;
            }
            volumeControl.Focus();
        }

        private void VolumeControlMouseUp(object sender, MouseEventArgs e)
        {
            if (Resizing) return;
            var volumeControl = (VolumeLevel)sender;
            if (e.Button == MouseButtons.Left && !volumeControl.Paired)
            {
                volumeControl.Micobject.x = volumeControl.Left;
                volumeControl.Micobject.y = volumeControl.Top;
            }
        }


        private void VolumeControlMouseMove(object sender, MouseEventArgs e)
        {
            if (Resizing) return;
            var volumeControl = (VolumeLevel)sender;
            if (e.Button == MouseButtons.Left && !volumeControl.Paired && !MainForm.LockLayout)
            {
                int newLeft = volumeControl.Left + (e.X - Convert.ToInt32(volumeControl.Micobject.x));
                int newTop = volumeControl.Top + (e.Y - Convert.ToInt32(volumeControl.Micobject.y));
                if (newLeft < 0) newLeft = 0;
                if (newTop < 0) newTop = 0;
                if (newLeft + volumeControl.Width > 5 && newLeft < ClientRectangle.Width - 5)
                {
                    volumeControl.Left = newLeft;
                }
                if (newTop + volumeControl.Height > 5 && newTop < ClientRectangle.Height - 50)
                {
                    volumeControl.Top = newTop;
                }
            }

        }

        #endregion

        #region FloorPlanEvents

        private void FloorPlanMouseDown(object sender, MouseEventArgs e)
        {
            _lastClicked = _pnlCameras;
            if (Resizing) return;
            var fpc = (FloorPlanControl)sender;
            if (e.Button == MouseButtons.Left)
            {
                fpc.Fpobject.x = e.X;
                fpc.Fpobject.y = e.Y;
                fpc.BringToFront();
            }
            else
            {
                if (e.Button == MouseButtons.Right)
                {
                    ContextTarget = fpc;
                    pluginCommandsToolStripMenuItem.Visible = false;
                    switchToolStripMenuItem.Visible = false;
                    _listenToolStripMenuItem.Visible = false;
                    _resetRecordingCounterToolStripMenuItem.Visible = false;
                    _recordNowToolStripMenuItem.Visible = false;
                    _takePhotoToolStripMenuItem.Visible = openWebInterfaceToolStripMenuItem.Visible = false;
                    _applyScheduleToolStripMenuItem1.Visible = false;
                    pTZToolStripMenuItem.Visible = false;
                    pTZScheduleOnToolStripMenuItem.Visible = pTZScheduleOffToolStripMenuItem.Visible = false;
                    ctxtMnu.Show(fpc, new Point(e.X, e.Y));
                }
            }
            fpc.Focus();
        }

        private void FloorPlanMouseUp(object sender, MouseEventArgs e)
        {
            if (Resizing) return;
            var fpc = (FloorPlanControl)sender;
            if (e.Button == MouseButtons.Left)
            {
                fpc.Fpobject.x = fpc.Left;
                fpc.Fpobject.y = fpc.Top;
            }
        }

        private void FloorPlanMouseMove(object sender, MouseEventArgs e)
        {
            if (Resizing) return;
            var fpc = (FloorPlanControl)sender;
            if (e.Button == MouseButtons.Left && !MainForm.LockLayout)
            {
                int newLeft = fpc.Left + (e.X - Convert.ToInt32(fpc.Fpobject.x));
                int newTop = fpc.Top + (e.Y - Convert.ToInt32(fpc.Fpobject.y));
                if (newLeft < 0) newLeft = 0;
                if (newTop < 0) newTop = 0;
                if (newLeft + fpc.Width > 5 && newLeft < ClientRectangle.Width - 5)
                {
                    fpc.Left = newLeft;
                }
                if (newTop + fpc.Height > 5 && newTop < ClientRectangle.Height - 50)
                {
                    fpc.Top = newTop;
                }
            }
        }

        #endregion

        #region RestoreSavedCameras

        public void DisplayCamera(objectsCamera cam, bool enableOnDisplay = false)
        {
            var cameraControl = new CameraWindow(cam,this);
            SetCameraEvents(cameraControl);
            cameraControl.BackColor = Conf.BackColor.ToColor();
            _pnlCameras.Controls.Add(cameraControl);
            cameraControl.Location = new Point(cam.x, cam.y);
            cameraControl.Size = new Size(cam.width, cam.height);
            cameraControl.BringToFront();
            cameraControl.Tag = GetControlIndex();

            var dir = Helper.GetMediaDirectory(2, cam.id);
            string path = dir + "video\\" + cam.directory + "\\";
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                path = dir + "video\\" + cam.directory + "\\thumbs\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    //move existing thumbs into directory
                    var lfi =
                        Directory.GetFiles(dir + "video\\" + cam.directory + "\\", "*.jpg").ToList();
                    foreach (string file in lfi)
                    {
                        string destfile = file;
                        int i = destfile.LastIndexOf(@"\", StringComparison.Ordinal);
                        destfile = file.Substring(0, i) + @"\thumbs" + file.Substring(i);
                        File.Move(file, destfile);
                    }
                }
                path = dir + "video\\" + cam.directory + "\\grabs\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            if (enableOnDisplay)
                cameraControl.Enable();
            cameraControl.LoadFileList();
        }

        private void DoInvoke(string methodName)
        {
            if (methodName == "show")
            {
                ShowIfUnlocked();
                return;
            }
            if (methodName.StartsWith("bringtofrontcam"))
            {
                int camid = Convert.ToInt32(methodName.Split(',')[1]);
                foreach (Control c in _pnlCameras.Controls)
                {
                    var window = c as CameraWindow;
                    var cameraControl = window;
                    if (cameraControl?.Camobject.id == camid)
                    {
                        cameraControl.BringToFront();
                        break;
                    }
                }
                return;
            }
            if (methodName.StartsWith("bringtofrontmic"))
            {
                int micid = Convert.ToInt32(methodName.Split(',')[1]);
                foreach (Control c in _pnlCameras.Controls)
                {
                    var level = c as VolumeLevel;
                    var vl = level;
                    if (vl?.Micobject.id != micid) continue;
                    vl.BringToFront();
                    break;
                }
            }
        }

        private void BClick(object sender, EventArgs e)
        {
            RunCommand((int)((Button)sender).Tag);
        }

        private void CameraControlRemoteCommand(object sender, ThreadSafeCommand e)
        {
            Delegates.InvokeMethod i = DoInvoke;
            Invoke(i, e.Command);
        }

        #endregion

        #region Nested type: ListItem

        public class ListItem
        {
            private readonly string _name;
            internal readonly object Value;

            public ListItem(string name, string[] value)
            {
                _name = name;
                Value = value;
            }

            public ListItem(string name, int value)
            {
                _name = name;
                Value = value;
            }

            public ListItem(string name, string value)
            {
                _name = name;
                Value = value;
            }

            public override string ToString()
            {
                return _name;
            }
        }

        #endregion      

        #region Nested type: clsCompareFileInfo

        public class ClsCompareFileInfo : IComparer
        {
            #region IComparer Members

            public int Compare(object x, object y)
            {
                var file1 = (FileInfo)x;
                var file2 = (FileInfo)y;

                return 0 - DateTime.Compare(file1.CreationTime, file2.CreationTime);
            }

            #endregion
        }

        #endregion
    }

    public class FilePreview
    {
        public string Filename;
        public int Duration;
        public string Name;
        public long CreatedDateTicks;
        public long CreatedDateTicksUTC;
        public int ObjectTypeId;
        public int ObjectId;
        public double MaxAlarm;
        public bool IsTimeLapse;
        public bool IsMerged;

        public FilePreview(string filename, int duration, string name, long createdDateTicks, int objectTypeId, int objectId, double maxAlarm, bool isTimelapse, bool isMerged)
        {
            Filename = filename;
            Duration = duration;
            Name = name;
            ObjectTypeId = objectTypeId;
            ObjectId = objectId;
            CreatedDateTicks = createdDateTicks;
            MaxAlarm = maxAlarm;
            IsMerged = isMerged;
            IsTimeLapse = isTimelapse;
            CreatedDateTicksUTC = (new DateTime(CreatedDateTicks)).ToUniversalTime().AddSeconds(0-Duration).Ticks;
        }
    }
}
