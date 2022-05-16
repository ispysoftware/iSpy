using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using iSpyApplication.Cloud;
using iSpyApplication.Controls;
using iSpyApplication.Onvif;
using iSpyApplication.Utilities;
using NAudio.Wave;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharpDX.DirectInput;
using DateTime = System.DateTime;
using File = System.IO.File;
using IPAddress = System.Net.IPAddress;

namespace iSpyApplication.Server
{
    public partial class LocalServer
    {
        private static Scanner _scanner;
        private static CameraDiscovery.CameraScanner _deviceScanner;

        public static CameraDiscovery.CameraScanner DeviceScanner
        {
            get
            {
                if (_deviceScanner == null)
                {
                    _deviceScanner = new CameraDiscovery.CameraScanner();
                    _deviceScanner.URLFound += DeviceScannerURLFound;
                }
                return _deviceScanner;
            }
        }

        static void DeviceScannerURLFound(object sender, CameraDiscovery.ConnectionOptionEventArgs e)
        {
            _devicescanResults.Add(e.Co);
        }

        private List<NetworkDevice> _scanResults = new List<NetworkDevice>();
        private static List<ConnectionOption> _devicescanResults = new List<ConnectionOption>();

        private void LoadJson(string sPhysicalFilePath, string sRequest, string sBuffer, string sHttpVersion, HttpRequest req)
        {
            var cmd = GetVar(sPhysicalFilePath, "cmd");

            var r = ServerRoot;

            int ot, oid;

            int.TryParse(GetVar(sPhysicalFilePath, "ot"), out ot);
            int.TryParse(GetVar(sPhysicalFilePath, "oid"), out oid);
            //language code
            string lc = GetVar(sPhysicalFilePath, "lc");
            if (LocRm.TranslationSets.All(p => p.CultureCode != lc))
                lc = "en";

            ISpyControl io = null;
            CameraWindow cw;
            VolumeLevel vl;

            if (oid > -1)
                io = MainForm.InstanceReference.GetISpyControl(ot, oid);

            var resp = "";
            const string commandExecuted = "{\"status\":\"ok\"}";
            const string commandFailed = "{{\"status\":\"{0}\"}}";
            string template = "{{\"text\":\"{0}\",\"value\":\"{1}\",\"noTranslate\":true}},";
            string t = "";
            string sd, ed;
            long sdl = 0, edl = 0;
            int total = 0;
            var ptzid = -1;
            bool success;
            switch (cmd)
            {
                case "querylevel":
                {
                    float fLevel = 0;
                    switch (ot)
                    {
                        case 1:
                            vl = MainForm.InstanceReference.GetVolumeLevel(oid);
                            if (vl != null)
                            {
                                var v = vl.Levels;
                                if (v == null)
                                    fLevel = 0;
                                else
                                {
                                    fLevel = vl.Levels.Max();
                                    fLevel = Math.Min(1, fLevel/vl.Micobject.detector.gain);
                                }
                            }
                            break;
                        case 2:
                            cw = MainForm.InstanceReference.GetCameraWindow(oid);
                            if (cw?.Camera != null)
                            {
                                fLevel = cw.Camera.MotionLevel;
                                fLevel = Math.Min(1, (fLevel/cw.Camobject.detector.gain));
                            }
                            break;
                    }
                    resp = "{\"level\":" + fLevel.ToString(CultureInfo.InvariantCulture) + "}";
                }
                    break;
                case "adddevice":
                    var stid = Convert.ToInt32(GetVar(sPhysicalFilePath, "sourceTypeID"));
                    switch (ot)
                    {
                        case 1:
                            oid = MainForm.NextMicrophoneId;
                            break;
                        case 2:
                            oid = MainForm.NextCameraId;
                            break;
                    }
                    MainForm.InstanceReference.AddObjectExternal(ot,stid,640,480,"","");
                    resp = "{\"actionResult\":\"editsource\",\"typeID\":" + ot + ",\"ID\":" + oid + "}";
                    break;
                case "createfromwizard":
                    {
                        switch (ot)
                        {
                            case 2:
                                int channel = Convert.ToInt32(GetVar(sPhysicalFilePath, "channel"));
                                string url = GetVar(sPhysicalFilePath, "url");
                                string username = GetVar(sPhysicalFilePath, "username");
                                string password = GetVar(sPhysicalFilePath, "password");
                                string make = GetVar(sPhysicalFilePath, "make");
                                string model = GetVar(sPhysicalFilePath, "model");

                                int mmid = Convert.ToInt32(GetVar(sPhysicalFilePath, "mmid"));
                                Uri origUri;
                                if (!Uri.TryCreate(url, UriKind.Absolute, out origUri))
                                    break;
                                var source = MainForm.Sources.FirstOrDefault(p => p.url.Any(q => q.id == mmid));
                                var mmurl = source?.url.FirstOrDefault(p => p.id == mmid);
                                if (mmurl == null)
                                    break;

                                string audioUri = "";
                                int audioSourceTypeID = -1;
                                var disc = new CameraDiscovery.URLDiscovery(origUri);
                                if (!string.IsNullOrEmpty(mmurl.AudioSource))
                                {
                                    audioUri = disc.GetAddr(mmurl, channel, username, password, true).ToString();
                                    audioSourceTypeID = Conf.GetSourceType(mmurl.AudioSource, 1);
                                }

                                int sourceTypeID = Conf.GetSourceType(mmurl.Source, 2);
                                string sourceUri = disc.GetAddr(mmurl, channel, username, password).ToString();


                                oid = MainForm.NextCameraId;
                                
                                cw = (CameraWindow) MainForm.InstanceReference.AddObjectExternal(2, sourceTypeID,640,480,"",sourceUri);
                                    
                                cw.Camobject.settings.videosourcestring = sourceUri;
                                cw.Camobject.settings.cookies = mmurl.cookies;
                                cw.Camobject.settings.login = username;
                                cw.Camobject.settings.password = password;

                                if (!string.IsNullOrEmpty(mmurl.flags))
                                {
                                    string[] flags = mmurl.flags.Split(',');
                                    foreach (string f in flags)
                                    {
                                        if (!string.IsNullOrEmpty(f))
                                        {
                                            switch (f.ToUpper())
                                            {
                                                case "FBA":
                                                    cw.Camobject.settings.forcebasic = true;
                                                    break;
                                            }
                                        }
                                    }
                                }

                                int ptzentryid = 0;

                                if (!mmurl.@fixed)
                                {
                                    string modellc = model.ToLower();
                                    string n = make.ToLower();
                                    bool quit = false;
                                    foreach (var ptz in MainForm.PTZs)
                                    {
                                        int j = 0;
                                        foreach (var m in ptz.Makes)
                                        {
                                            if (m.Name.ToLower() == n)
                                            {
                                                ptzid = ptz.id;
                                                ptzentryid = j;
                                                string mdl = m.Model.ToLower();
                                                if (mdl == modellc || mmurl.version.ToLower() == mdl)
                                                {
                                                    ptzid = ptz.id;
                                                    ptzentryid = j;
                                                    quit = true;
                                                    break;
                                                }
                                            }
                                            j++;
                                        }
                                        if (quit)
                                            break;
                                    }
                                }

                                if (ptzid > -1)
                                {
                                    cw.Camobject.ptz = ptzid;
                                    cw.Camobject.ptzentryindex = ptzentryid;
                                    cw.Camobject.settings.ptzchannel = channel.ToString(CultureInfo.InvariantCulture);
                                    cw.Camobject.settings.ptzusername = username;
                                    cw.Camobject.settings.ptzpassword = password;
                                }

                                if (!string.IsNullOrEmpty(mmurl.AudioModel))
                                {
                                    var audUri = new Uri(cw.Camobject.settings.videosourcestring);
                                    if (!string.IsNullOrEmpty(audUri.DnsSafeHost))
                                    {
                                        cw.Camobject.settings.audioip = audUri.DnsSafeHost;
                                    }
                                    cw.Camobject.settings.audiomodel = mmurl.AudioModel;
                                    cw.Camobject.settings.audioport = audUri.Port;
                                    cw.Camobject.settings.audiousername = username;
                                    cw.Camobject.settings.audiopassword = password;
                                }
                                
                                cw.Camobject.settings.tokenconfig.tokenpath = mmurl.tokenPath;
                                cw.Camobject.settings.tokenconfig.tokenpost = mmurl.tokenPost;
                                cw.Camobject.settings.tokenconfig.tokenport = mmurl.tokenPort;
                                

                                if (audioSourceTypeID > -1)
                                {
                                    var vc = cw.VolumeControl;
                                    if (vc == null)
                                    {
                                        vc = MainForm.InstanceReference.AddCameraMicrophone(cw.Camobject.id, cw.Camobject.name + " mic");
                                        vc.Micobject.alerts.active = false;
                                        vc.Micobject.detector.recordonalert = false;
                                        vc.Micobject.detector.recordondetect = false;
                                        cw.SetVolumeLevel(vc.Micobject.id);
                                    }
                                    vc.Disable();
                                    vc.Micobject.settings.typeindex = audioSourceTypeID;
                                    vc.Micobject.settings.sourcename = audioUri;
                                    vc.Micobject.settings.needsupdate = true;
                                }
                                cw.Enable();

                                break;
                        }
                    }
                    resp = "{\"actionResult\":\"waiteditobjectnew\",\"typeID\":" + ot + ",\"ID\":" + oid + "}";
                    break;
                case "scannetwork":
                    {
                        string[] sports = GetVar(sPhysicalFilePath, "ports").Split(',');
                        bool full = GetVar(sPhysicalFilePath, "full") == "true";
                        var ports = new List<int>();
                        foreach (var s in sports)
                        {
                            int i;
                            if (int.TryParse(s, out i))
                            {
                                ports.Add(i);
                            }
                        }
                        if (ports.Count > 0)
                        {

                            NetworkDeviceList.RefreshARP();
                            var lip = new List<IPAddress>();
                            foreach (var ip in NetworkDeviceList.ARPList)
                            {
                                IPAddress ipa;
                                if (IPAddress.TryParse(ip.Key, out ipa))
                                {
                                    lip.Add(ipa);
                                }
                            }
                            if (lip.Count > 0)
                            {
                                _scanResults = new List<NetworkDevice>();
                                _scanner = new Scanner();
                                _scanner.DeviceFound += ScannerDeviceFound;
                                _scanner.ScanFinished += ScannerScanFinished;
                                Thread manager;
                                if (full)
                                {
                                    var ipranges = MainForm.AddressListIPv4.Select(ip => ip.ToString()).Select(subnet => subnet.Substring(0, subnet.LastIndexOf(".", StringComparison.Ordinal) + 1) + "x").ToList();
                                    manager = new Thread(p => _scanner.PortScannerManager(ipranges, ports))
                                    {
                                        Name ="Port Scanner",
                                        IsBackground=true,
                                        Priority=ThreadPriority.Normal
                                    };
                                }
                                else
                                {
                                    manager = new Thread(p => _scanner.ARPScannerManager(lip, ports))
                                    {
                                        Name = "ARP Scanner",
                                        IsBackground = true,
                                        Priority =ThreadPriority.Normal
                                    };
                                }
                                manager.Start();
                            }
                        }
                        resp = "{\"running\":true}";

                    }
                    break;
                case "getscannetworkresults":
                    {
                        resp = "{\"finished\":" + (_scanner == null).ToString().ToLower();
                        resp += ",\"results\":[";
                        if (_scanResults != null)
                        {
                            template = "{{\"deviceName\":\"{0}\",\"IPAddress\":\"{1}\",\"MAC\":\"{2}\",\"port\":{3},\"webServer\":\"{4}\"}},";
                            resp = _scanResults.Aggregate(resp, (current, dev) => current + string.Format(template, dev.DeviceName.JsonSafe(), dev.IPAddress, dev.MAC, dev.Port, dev.WebServer.JsonSafe()));
                            resp = resp.Trim(',');
                        }
                        resp += "]}";

                    }
                    break;
                case "scandevice":
                    {
                        Uri uri;
                        if (!Uri.TryCreate(GetVar(sPhysicalFilePath, "url"), UriKind.Absolute, out uri))
                        {
                            resp = "{\"error\":\""+LocRm.GetString("InvalidURL",lc)+"\"}";
                            break;
                        }
                        var make = GetVar(sPhysicalFilePath, "make");
                        var m = MainForm.Sources.FirstOrDefault(p => string.Equals(p.name, make, StringComparison.InvariantCultureIgnoreCase));
                        if (m == null)
                        {
                            resp = "{\"error\":\""+LocRm.GetString("ChooseMake", lc) +"\"}";
                            break;
                        }
                        make = m.name;

                        _devicescanResults = new List<ConnectionOption>();
                        DeviceScanner.Stop();

                        DeviceScanner.URLFound += DeviceScannerURLFound;

                        DeviceScanner.Channel = Convert.ToInt32(GetVar(sPhysicalFilePath, "channel"));
                        DeviceScanner.Make = make;
                        DeviceScanner.Model = GetVar(sPhysicalFilePath, "model");
                        DeviceScanner.Username = GetVar(sPhysicalFilePath, "username");
                        DeviceScanner.Password = GetVar(sPhysicalFilePath, "password");
                        DeviceScanner.Uri = uri;
                        DeviceScanner.ScanCamera(m);


                        resp = "{\"running\":true}";

                    }
                    break;
                case "getscandeviceresults":
                    {
                        resp = "{\"finished\":" + (!DeviceScanner.Running).ToString().ToLower();
                        resp += ",\"results\":[";
                        if (_devicescanResults != null)
                        {
                            template = "{{\"type\":\"{0}\",\"URL\":\"{1}\",\"mmid\":{2}}},";
                            resp = _devicescanResults.Aggregate(resp, (current, dev) => current + string.Format(template, dev.Source, dev.URL.JsonSafe(), dev.MmUrl.id));

                            resp = resp.Trim(',');
                        }
                        resp += "]}";

                    }
                    break;
                case "loadmakes":
                    {
                        string s = GetVar(sPhysicalFilePath, "search");
                        string makes = "{\"makes\": [";

                        var ml = MainForm.Sources.Where(p => p.name.IndexOf(s, StringComparison.OrdinalIgnoreCase) != -1).OrderBy(p => p.name).Take(20).ToList();
                        var sb = new StringBuilder();
                        foreach (var m in ml)
                        {
                            sb.Append("\"" + m.name.JsonSafe() + "\",");
                        }
                        makes += sb.ToString().Trim(',');

                        resp = makes + "]}";
                    }
                    break;
                case "loadmodels":
                    {
                        string s = GetVar(sPhysicalFilePath, "search");
                        string m = GetVar(sPhysicalFilePath, "make");
                        var make = MainForm.Sources.FirstOrDefault(p => p.name.ToLowerInvariant() == m.ToLowerInvariant());

                        string models = "{\"models\": [\"Other\",";

                        if (make != null)
                        {
                            var lmake = make.url.Where(p => p.version.IndexOf(s, StringComparison.OrdinalIgnoreCase) != -1).OrderBy(p => p.version).ToList();
                            int i = 0;
                            foreach (var u in lmake)
                            {
                                if (!string.IsNullOrEmpty(u.version))
                                {
                                    if (models.IndexOf("\"" + u.version + "\"", StringComparison.OrdinalIgnoreCase) ==
                                        -1)
                                    {
                                        models += "\"" + u.version.JsonSafe() + "\",";
                                        i++;
                                        if (i == 20)
                                            break;
                                    }
                                }
                            }
                        }

                        resp = models.Trim(',') + "]}";
                    }
                    break;
                case "loadapi":
                {
                    resp = BuildApijson(r,lc);
                }
                    break;
                case "authorise":
                    {
                        string provider = GetVar(sPhysicalFilePath, "provider");
                        t = "{{\"provider\":\"{0}\",\"url\":\"{1}\",\"message\":\"{2}\",\"error\":\"{3}\"}}";
                        string url = "", message = "", error = "";
                        if (!MainForm.Conf.Subscribed)
                        {
                            error = LocRm.GetString("NotSubscribed", lc);
                        }
                        else
                        {
                            string code = GetVar(sRequest, "code");
                            switch (provider.ToLower())
                            {
                                case "box":
                                    if (Box.Authorise(code))
                                    {
                                        message = LocRm.GetString("Authorised", lc);
                                    }
                                    else
                                        message = LocRm.GetString("Failed", lc);
                                    break;
                                case "onedrive":
                                    if (OneDrive.Authorise(code))
                                    {
                                        message = LocRm.GetString("Authorised", lc);
                                    }
                                    else
                                        message = LocRm.GetString("Failed", lc);
                                    break;
                                case "drive":
                                    {
                                        if (Drive.Authorise(code))
                                        {
                                            message = LocRm.GetString("Authorised", lc);
                                        }
                                        else
                                            message = LocRm.GetString("Failed", lc);
                                    }
                                    break;
                                case "flickr":
                                    if (Flickr.Authorise(code))
                                    {
                                        message = LocRm.GetString("Authorised", lc);
                                    }
                                    else
                                        message = LocRm.GetString("Failed", lc);
                                    break;
                                case "dropbox":
                                    if (Dropbox.Authorise(code))
                                    {
                                        message = LocRm.GetString("Authorised", lc);
                                    }
                                    else
                                        message = LocRm.GetString("Failed", lc);
                                    break;
                                //case "youtube":
                                //    {
                                //        if (YouTubeUploader.Authorise(code))
                                //        {
                                //            message = LocRm.GetString("Authorised", lc);
                                //        }
                                //        else
                                //            message = LocRm.GetString("Failed", lc);
                                //    }
                                //    break;
                            }
                        }

                        resp = string.Format(t, provider, url, message, error);
                    }
                    break;
                case "getauthoriseurl":
                    {
                        string provider = GetVar(sPhysicalFilePath, "provider");
                        t = "{{\"provider\":\"{0}\",\"url\":\"{1}\",\"message\":\"{2}\",\"error\":\"{3}\"}}";
                        string url = "", message = "", error = "";
                        
                        switch (provider.ToLower())
                        {
                            case "flickr":
                                url = Flickr.GetAuthoriseURL(out error);
                                break;
                        }
                        

                        resp = string.Format(t, provider, url, message, error);
                    }
                    break;
                case "onvifdiscover":
                    {
                        string un = GetVar(sRequest, "un");
                        string pwd = GetVar(sRequest, "pwd");
                        string url = GetVar(sRequest, "surl");

                        try
                        {
                            var dev = new ONVIFDevice(url, un, pwd,0,15);
                            var p = dev.Profiles;
                            if (p == null)
                                throw new ApplicationException("ONVIF failed to connect");
                            for (int i = 0; i < p.Length; i++)
                            {
                                dev.SelectProfile(i);
                                var ep = dev.StreamEndpoint;
                                if (ep != null && ep.Width > 0)
                                {
                                    resp += string.Format(template, dev.Profile.Name + " (" + ep.Width + "x" + ep.Height + ")", i);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            resp = "{\"error\":\"" + ex.Message.JsonSafe() + "\"}";
                        }
                        Uri u;
                        if (!Uri.TryCreate(url, UriKind.Absolute, out u))
                        {
                            resp = "{\"error\":\"" + LocRm.GetString("InvalidURL", lc) + "\"}";
                            break;
                        }
                        resp = "{\"list\":[" + resp.Trim(',') + "]}";
                    }
                    break;
                case "editsource":
                    {
                        switch (ot)
                        {
                            case 1:
                                {
                                    objectsMicrophone om = MainForm.Microphones.FirstOrDefault(p => p.id == oid);
                                    if (om == null)
                                        throw new Exception("Microphone not found");
                                    resp = File.ReadAllText(r + @"api\editaudiosource.json");

                                    var st = om.settings.typeindex.ToString(CultureInfo.InvariantCulture);

                                    var se = File.ReadAllText(r + @"api\sources\audio\" + st + ".json");
                                    string task = "";

                                    string m = GetVar(sPhysicalFilePath, "id");
                                    if (!string.IsNullOrEmpty(m))
                                    {
                                        om.settings.sourcename = m;
                                        task = "rendersourceeditor";
                                    }
                                    switch (st)
                                    {
                                        case "0":
                                            //localdevice

                                            string sr = Conf.SampleRates.Aggregate("", (current, s) => current + string.Format(template, s, s));

                                            if (!Conf.SampleRates.Contains(om.settings.samples))
                                                om.settings.samples = 8000;

                                            se = se.Replace("SAMPLERATES", sr.Trim(','));
                                            try
                                            {
                                                string devs = "";
                                                for (int n = 0; n < WaveIn.DeviceCount; n++)
                                                {
                                                    string name = WaveIn.GetCapabilities(n).ProductName.JsonSafe();
                                                    devs += string.Format(template, name, name);

                                                }
                                                se = se.Replace("DEVICELIST", devs.Trim(','));
                                            }
                                            catch (ApplicationException ex)
                                            {
                                                Logger.LogException(ex, "LocalDevice");
                                            }
                                            break;
                                        case "5":
                                            string miclist = MainForm.Microphones.Where(mic => mic.id != om.id && mic.settings.typeindex != 5).Aggregate("", (current, mic) => current + string.Format(template, mic.name.JsonSafe(), mic.id));
                                            se = se.Replace("MICROPHONES", miclist.Trim(','));
                                            break;
                                    }
                                    resp = resp.Replace("SOURCEEDITOR", se);

                                    dynamic d = PopulateResponse(resp, om,lc);
                                    d.task = task;
                                    resp = JsonConvert.SerializeObject(d);
                                }
                                break;
                            case 2:
                                {
                                    objectsCamera oc = MainForm.Cameras.FirstOrDefault(p => p.id == oid);
                                    if (oc == null)
                                        throw new Exception("Camera not found");
                                    resp = File.ReadAllText(r + @"api\editvideosource.json");

                                    var st = oc.settings.sourceindex.ToString(CultureInfo.InvariantCulture);

                                    var se = File.ReadAllText(r + @"api\sources\video\" + st + ".json");
                                    string task = "";

                                    string m = GetVar(sPhysicalFilePath, "id");
                                    if (!string.IsNullOrEmpty(m))
                                    {
                                        oc.settings.videosourcestring = m;
                                        task = "rendersourceeditor";
                                    }


                                    switch (st)
                                    {
                                        case "3":
                                            //localdevice
                                            string devicelist = "",
                                                videoresolutions = "",
                                                inputlist = "",
                                                snapshotresolutions = "",
                                                capturemodes = "";

                                            var disc = new Sources.Video.discovery.LocalDevice();
                                            string moniker = "";
                                            var ldev = disc.Devices;
                                            foreach (var dev in ldev)
                                            {
                                                devicelist += string.Format(template, dev.ToString().JsonSafe(), dev.ToString().JsonSafe());
                                                if (string.Equals(oc.settings.videosourcestring, dev.ToString(),
                                                    StringComparison.InvariantCultureIgnoreCase))
                                                {
                                                    moniker = dev.ToString();
                                                    oc.settings.videosourcestring = dev.ToString();
                                                }

                                            }
                                            if (moniker == "")
                                            {
                                                if (ldev.Count > 0)
                                                    moniker = ldev.First().ToString();
                                            }
                                            if (moniker != "")
                                            {
                                                disc.Inspect(moniker);
                                                inputlist = disc.Inputs.Aggregate(inputlist, (current, input) => current + string.Format(template, input.ToString().JsonSafe(), input.Value.ToString().JsonSafe()));
                                                videoresolutions = disc.VideoResolutions.Aggregate(videoresolutions, (current, vres) => current + string.Format(template, vres.ToString().JsonSafe(), vres.ToString().JsonSafe()));
                                                snapshotresolutions = disc.SnapshotResolutions.Aggregate(snapshotresolutions, (current, sres) => current + string.Format(template, sres.ToString().JsonSafe(), sres.ToString().JsonSafe()));

                                                if (disc.SupportsSnapshots)
                                                {
                                                    capturemodes += string.Format(template, LocRm.GetString("Snapshots", lc), "snapshots");
                                                }
                                                if (disc.SupportsVideo)
                                                {
                                                    capturemodes += string.Format(template, LocRm.GetString("Video", lc), "video");
                                                }
                                            }

                                            se = se.Replace("DEVICELIST", devicelist.Trim(','));
                                            se = se.Replace("VIDEORESOLUTIONS", videoresolutions.Trim(','));
                                            se = se.Replace("SNAPSHOTRESOLUTIONS", snapshotresolutions.Trim(','));
                                            se = se.Replace("INPUTLIST", inputlist.Trim(','));
                                            se = se.Replace("CAPTUREMODES", capturemodes.Trim(','));
                                            break;
                                        case "4":
                                            string screens = "";
                                            int i = 0,j=0;
                                            foreach (Screen s in Screen.AllScreens)
                                            {
                                                screens += string.Format(template, s.DeviceName.JsonSafe(),
                                                    i.ToString(CultureInfo.InvariantCulture));
                                                i++;
                                                if (oc.settings.videosourcestring == i.ToString(CultureInfo.InvariantCulture))
                                                    j = i;
                                            }
                                            se = se.Replace("SCREENS", screens.Trim(','));
                                            if (oc.settings.videosourcestring == null)
                                                oc.settings.videosourcestring = j.ToString(CultureInfo.InvariantCulture);
                                            var area = "[]";
                                            if (oc.settings.desktoparea != null) { 
                                                int[] arr = System.Array.ConvertAll(oc.settings.desktoparea.Split(','), int.Parse);
                                                if (arr.Length == 4)
                                                {
                                                    var sc = Screen.AllScreens[j];
                                                    arr[0] = Math.Min(100, Convert.ToInt32((Convert.ToDecimal(arr[0])/sc.WorkingArea.Width)*100));
                                                    arr[1] = Math.Min(100, Convert.ToInt32((Convert.ToDecimal(arr[1])/sc.WorkingArea.Height)*100));
                                                    arr[2] = Math.Min(100, Convert.ToInt32((Convert.ToDecimal(arr[2])/sc.WorkingArea.Width)*100));
                                                    arr[3] = Math.Min(100, Convert.ToInt32((Convert.ToDecimal(arr[3])/sc.WorkingArea.Height)*100));
                                                    area = "[{\"x\":" + arr[0] + ",\"y\":" + arr[1] + ",\"w\":" + arr[2] +
                                                           ",\"h\":" + arr[3] + "}]";
                                                }
                                            }
                                            se = se.Replace("SCREENAREA", area);
                                            break;
                                        case "9":
                                            if (oc.settings.namevaluesettings.IndexOf("use=", StringComparison.Ordinal) == -1)
                                                oc.settings.namevaluesettings = "use=ffmpeg,transport=RTSP";

                                            string svlc = "";
                                            if (VlcHelper.VLCAvailable)
                                            {
                                                svlc = ",{\"text\": \"VLC\", \"value\": \"vlc\", \"noTranslate\":true}";
                                            }
                                            se = se.Replace("VLCOPT", svlc);

                                            var ourl = "";
                                            var ss = NV(oc.settings.namevaluesettings, "streamsize");
                                            var pn = NV(oc.settings.namevaluesettings, "profilename");
                                            if (ss != "" && pn != "")
                                            {
                                                ourl = "{\"text\":\"" + ss + "\",\"value\":\"" + pn.JsonSafe() + "\", \"noTranslate\":true}";
                                            }
                                            se = se.Replace("ONVIFURL", ourl);
                                            break;
                                        case "10":
                                            string camlist = MainForm.Cameras.Where(cam => cam.id != oc.id && cam.settings.sourceindex != 10).Aggregate("", (current, cam) => current + string.Format(template, cam.name.JsonSafe(), cam.id));
                                            se = se.Replace("CAMERAS", camlist.Trim(','));
                                            break;
                                    }

                                    resp = resp.Replace("SOURCEEDITOR", se);

                                    dynamic d = PopulateResponse(resp, oc, lc);
                                    d.task = task;
                                    resp = JsonConvert.SerializeObject(d);
                                }
                                break;
                        }

                    }
                    break;
                case "editpelco":
                    {
                        switch (ot)
                        {
                            case 2:
                                objectsCamera oc = MainForm.Cameras.FirstOrDefault(p => p.id == oid);
                                if (oc == null)
                                    throw new Exception("Camera not found");
                                resp = File.ReadAllText(r + @"api\editpelco.json");



                                string[] ports = System.IO.Ports.SerialPort.GetPortNames();
                                string comports = ports.Aggregate("", (current, p) => current + string.Format(template, p, p));

                                resp = resp.Replace("COMPORTS", comports.Trim(','));
                                dynamic d = PopulateResponse(resp, oc, lc);
                                resp = JsonConvert.SerializeObject(d);
                                break;
                        }

                    }
                    break;
                case "editftp":
                    {
                        string ident = GetVar(sPhysicalFilePath, "ident");
                        resp = File.ReadAllText(r + @"api\editftp.json");
                        if (ident != "new")
                        {
                            var ftp = MainForm.Conf.FTPServers.First(p => p.ident == ident);
                            dynamic d = PopulateResponse(resp, ftp, lc);
                            d.ident = ftp.ident;
                            resp = JsonConvert.SerializeObject(d);
                        }
                        else
                        {
                            resp = Translate(resp, lc);
                        }

                    }
                    break;
                case "editstorage":
                {
                        var ident = GetVar(sPhysicalFilePath, "ident");
                        resp = File.ReadAllText(r + @"api\editstorage.json");
                        if (ident != "new")
                        {
                            var dir = MainForm.Conf.MediaDirectories.First(p => p.ID == Convert.ToInt32(ident));
                            dynamic d = PopulateResponse(resp, dir, lc);
                            d.ident = dir.ID;
                            resp = JsonConvert.SerializeObject(d);
                        }
                        else
                        {
                            resp = Translate(resp, lc);
                        }

                    }
                    break;
                case "editschedule":
                    {
                        string ident = GetVar(sPhysicalFilePath, "ident");
                        resp = File.ReadAllText(r + @"api\editschedule.json");

                        var actions = Helper.Actions.ToList();
                        switch (ot)
                        {
                            case 1:
                                actions =
                                    actions.Where(p => p.TypeID != Helper.ScheduleAction.ActionTypeID.CameraOnly).ToList();
                                break;
                        }
                        t = actions.Aggregate(t, (current, a) => current + string.Format(template, Helper.ScheduleDescription(a.ID).JsonSafe(), a.ID));
                        resp = resp.Replace("SCHEDULEACTIONS", t.Trim(','));

                        t = "";
                        foreach (var aa in MainForm.Actions.Where(p => p.objectid == oid && p.objecttypeid == ot))
                        {
                            var ae = new ActionEditor.ActionEntry(aa);
                            t += string.Format(template, (aa.mode + ": " + ae.Summary).JsonSafe(), aa.ident);
                        }
                        resp = resp.Replace("ALERTLIST", t.Trim(','));
                        

                        if (ident != "new")
                        {
                            int id = Convert.ToInt32(ident);
                            var scheds = MainForm.Schedule.Where(p => p.objectid == oid && p.objecttypeid == ot).ToList();
                            if (scheds.Count > id)
                            {
                                dynamic d = PopulateResponse(resp, scheds[id], lc);
                                d.ident = id;
                                resp = JsonConvert.SerializeObject(d);
                            }
                        }
                        else
                        {
                            resp = Translate(resp, lc);
                        }

                        resp = resp.Replace("[TIME]", DateTime.Now.ToString("HH:mm:ss"));
                    }
                    break;
                case "editptzschedule":
                    {
                        string ident = GetVar(sPhysicalFilePath, "ident");
                        resp = File.ReadAllText(r + @"api\editptzschedule.json");
                        var oc = MainForm.Cameras.FirstOrDefault(p => p.id == oid);
                        if (oc != null)
                        {
                            string ptzcommands = "";
                            var lcoms = MainForm.PTZs.FirstOrDefault(p => p.id == oc.ptz);
                            if (lcoms != null)
                            {
                                var coms = lcoms.ExtendedCommands.Command.ToList();
                                ptzcommands = coms.Aggregate(ptzcommands, (current, com) => current + string.Format(template, com.Name.JsonSafe(), com.Name.JsonSafe()));
                                resp = resp.Replace("PTZCOMMANDS", ptzcommands.Trim(','));
                            }
                            else
                                resp = resp.Replace("PTZCOMMANDS", "");

                            if (ident != "new")
                            {
                                int id = Convert.ToInt32(ident);
                                var ptzsched = oc.ptzschedule.entries.ToList();
                                if (ptzsched.Count > id)
                                {
                                    dynamic d = PopulateResponse(resp, ptzsched[id], lc);
                                    d.ident = id;
                                    resp = JsonConvert.SerializeObject(d);
                                }
                            }
                            else
                            {
                                resp = Translate(resp, lc);
                            }

                            resp = resp.Replace("TIME", DateTime.Now.ToString("HH:mm:ss"));
                        }
                    }
                    break;
                case "editaction":
                    {
                        string ident = GetVar(sPhysicalFilePath, "ident");
                        resp = File.ReadAllText(r + @"api\editaction.json");
                        resp = resp.Replace("ACTIONS", Helper.AvailableActionsJson);
                        template = "{{\"text\":\"{0}\",\"value\":{1},\"type\":\"{2}\",\"bindto\":\"{3}\"{4},\"noTranslate\":true}},";

                        var par = "";
                        bool bGrab;
                        var action = MainForm.Actions.FirstOrDefault(p => p.ident == ident) ?? new objectsActionsEntry
                                                                                           {
                                                                                               mode = "alert",
                                                                                               active = true,
                                                                                               objectid = oid,
                                                                                               objecttypeid = ot,
                                                                                               type = "S"
                                                                                           };

                        string id = GetVar(sPhysicalFilePath, "id").ToUpper();
                        if (id != "")
                            action.type = id;

                        string mode = GetVar(sPhysicalFilePath, "mode");

                        if (mode != "") action.mode = mode;
                        bool active;

                        if (bool.TryParse(GetVar(sPhysicalFilePath, "active"), out active))
                            action.active = active;

                        switch (action.type.ToUpper())
                        {
                            case "S":
                            case "ATC":
                                par = string.Format(template, LocRm.GetString("File", lc), "\"" + action.param1.JsonSafe() + "\"", "Select",
                                    "param1",
                                    ",\"options\":[" + GetFileOptionsList("Sounds", "*.wav") +
                                    "],\"help\":\""+LocRm.GetString("AddSounds", lc) +": " + (Program.AppPath + "sounds").JsonSafe() + "\"");
                                break;
                            case "EXE":
                                par = string.Format(template, LocRm.GetString("File", lc), "\"" + action.param1.JsonSafe() + "\"", "Select",
                                    "param1", ",\"options\":[" + GetFileOptionsList("Commands", "*.*") +
                                    "],\"help\":\"" + LocRm.GetString("AddBatch", lc) + ": " + (Program.AppPath + "commands").JsonSafe() + "\"");
                                break;
                            case "URL":
                                par = string.Format(template, LocRm.GetString("URL", lc), "\"" + action.param1.JsonSafe() + "\"", "String",
                                    "param1", "");

                                bool.TryParse(action.param2, out bGrab);
                                par += string.Format(template, LocRm.GetString("UploadImage", lc), bGrab.ToString().ToLower(), "Boolean", "param2",
                                    "");
                                break;
                            case "NM":
                                if (action.param1 == "")
                                    action.param1 = "TCP";
                                if (action.param3 == "")
                                    action.param3 = "1010";
                                par += string.Format(template, LocRm.GetString("Type", lc), "\"" + action.param1.JsonSafe() + "\"", "Select",
                                    "param1",
                                    ", \"options\":[{\"text\":\"TCP\",\"value\":\"TCP\", \"noTranslate\":true},{\"text\":\"UDP\",\"value\":\"UDP\", \"noTranslate\":true}]");
                                par += string.Format(template, LocRm.GetString("IPAddress", lc), "\"" + action.param2.JsonSafe() + "\"",
                                    "String", "param2", "");

                                par += string.Format(template, LocRm.GetString("Port", lc), "\"" + action.param3.JsonSafe() + "\"", "String", "param3",
                                    ",\"min\":0,\"max\":65535");

                                par += string.Format(template, LocRm.GetString("Message", lc), "\"" + action.param4 + "\"", "String", "param4",
                                    "");
                                break;
                            case "SW":
                            case "B":
                            case "M":
                            case "TM":
                                par = "";
                                break;
                            case "TA":
                            case "SOF":
                            case "SOO":
                                var optlist = MainForm.Cameras.Aggregate("", (current, c) => current + ("{\"text\":\"" + c.name.JsonSafe() + "\",\"value\":\"2," + c.id + "\", \"noTranslate\":true},"));
                                optlist = MainForm.Microphones.Aggregate(optlist, (current, c) => current + ("{\"text\":\"" + c.name.JsonSafe() + "\",\"value\":\"1," + c.id + "\", \"noTranslate\":true},"));
                                par += string.Format(template, "Object", "\"" + action.param1.JsonSafe() + "\"",
                                    "Select", "param1", ", \"options\":[" + optlist.Trim(',') + "]");
                                break;
                            case "E":
                                par = string.Format(template, LocRm.GetString("EmailAddress", lc), "\"" + action.param1.JsonSafe() + "\"",
                                    "String", "param1", "");
                                bool.TryParse(action.param2, out bGrab);
                                par += string.Format(template, LocRm.GetString("IncludeImage", lc), bGrab.ToString().ToLower(), "Boolean",
                                    "param2", "");
                                break;
                            case "SMS":
                                par = string.Format(template, LocRm.GetString("SMSNumber", lc), "\"" + action.param1.JsonSafe() + "\"", "String",
                                    "param1", "");
                                break;
                        }
                        resp = par != "" ? resp.Replace("PARAMS", "," + par.Trim(',')) : resp.Replace("PARAMS", "");

                        dynamic d = PopulateResponse(resp, action, lc);
                        d.ident = ident;
                        if (id != "")
                            d.task = "bindActionView";
                        resp = JsonConvert.SerializeObject(d);

                    }
                    break;
                case "deleteschedule":
                    {
                        int id = Convert.ToInt32(GetVar(sPhysicalFilePath, "ident"));
                        var scheds = MainForm.Schedule.Where(p => p.objectid == oid && p.objecttypeid == ot).ToList();
                        if (scheds.Count > id)
                        {
                            scheds.RemoveAt(id);
                            MainForm.Schedule.RemoveAll(p => p.objectid == oid && p.objecttypeid == ot);
                            MainForm.Schedule.AddRange(scheds);
                        }
                        io.ReloadSchedule();
                        resp = "{\"actionResult\":\"reloadSchedule\"}";
                    }
                    break;
                case "deleteptzschedule":
                    {
                        int id = Convert.ToInt32(GetVar(sPhysicalFilePath, "ident"));
                        var oc = MainForm.Cameras.First(p => p.id == oid);
                        var scheds = oc?.ptzschedule.entries.ToList();
                        if (scheds?.Count > id)
                        {
                            scheds.RemoveAt(id);
                            oc.ptzschedule.entries = scheds.ToArray();
                        }
                        resp = "{\"actionResult\":\"reloadPTZSchedule\"}";
                    }
                    break;
                case "showschedule":
                    {
                        var sched = MainForm.Schedule.Where(p => p.objectid == oid && p.objecttypeid == ot).ToList();
                        int i = 0;
                        foreach (var se in sched)
                        {
                            string text = FormatTime(se.time) + " " + FormatDays(se.daysofweek.Split(',')) + " " +
                                         Helper.ScheduleDescription(se.typeid);
                            t += string.Format(template, text.JsonSafe(), i);
                            i++;

                        }
                        resp = File.ReadAllText(r + @"api\showschedule.json");
                        resp = resp.Replace("SCHEDULE", t.Trim(','));

                        resp = Translate(resp, lc);

                    }
                    break;
                case "showptzschedule":
                    {
                        int i = 0;
                        var oc = MainForm.Cameras.FirstOrDefault(p => p.id == oid);
                        if (oc != null)
                        {
                            foreach (var pts in oc.ptzschedule.entries)
                            {
                                string text = pts.time.ToString("HH:mm") + " " + pts.command;
                                t += string.Format(template, text.JsonSafe(), i);
                                i++;
                            }
                        }
                        resp = File.ReadAllText(r + @"api\showptzschedule.json");
                        resp = resp.Replace("SCHEDULE", t.Trim(','));

                        resp = Translate(resp, lc);

                    }
                    break;
                case "showactions":
                    {
                        var actions = MainForm.Actions.Where(p => p.objectid == oid && p.objecttypeid == ot).OrderBy(p => p.mode).ToList();
                        foreach (var se in actions)
                        {
                            string m = se.mode;
                            switch (se.mode)
                            {
                                case "alert":
                                    m = LocRm.GetString("Alert", lc);
                                    break;
                                case "reconnect":
                                    m = LocRm.GetString("Reconnect", lc);
                                    break;
                                case "reconnectfailed":
                                    m = LocRm.GetString("ReconnectFailed", lc);
                                    break;
                                case "disconnect":
                                    m = LocRm.GetString("Disconnect", lc);
                                    break;
                                case "alertstopped":
                                    m = LocRm.GetString("AlertStopped", lc);
                                    break;
                                case "recordingstarted":
                                    m = LocRm.GetString("RecordingStarted", lc);
                                    break;
                                case "recordingstopped":
                                    m = LocRm.GetString("RecordingStopped", lc);
                                    break;
                            }
                            t += string.Format(template, (se.active ? LocRm.GetString("Active", lc) : LocRm.GetString("Inactive", lc)) + ": " + m + " " + Helper.AlertSummary(se).JsonSafe(), se.ident);
                        }
                        resp = File.ReadAllText(r + @"api\showactions.json");
                        resp = resp.Replace("ACTIONS", t.Trim(','));

                        resp = Translate(resp, lc);
                        
                    }
                    break;
                case "deleteaction":
                    {
                        MainForm.Actions.RemoveAll(p => p.ident == GetVar(sPhysicalFilePath, "ident"));
                        MainForm.SaveConfig();
                        resp = "{\"actionResult\":\"reloadActions\"}";
                    }
                    break;
                case "deleteftp":
                    {
                        var lTemp = MainForm.Conf.FTPServers.ToList();
                        lTemp.RemoveAll(p => p.ident == GetVar(sPhysicalFilePath, "ident"));
                        MainForm.Conf.FTPServers = lTemp.ToArray();
                        MainForm.SaveConfig();

                        resp = "{\"actionResult\":\"reloadFTPServers\"}";
                    }
                    break;
                case "deletestorage":
                    {
                        string ident = GetVar(sPhysicalFilePath, "ident");
                        var id = Convert.ToInt32(ident);
                        var lname = new List<string>();
                        lname.AddRange(MainForm.Cameras.Where(p => p.settings.directoryIndex == id).Select(p => p.name));
                        lname.AddRange(MainForm.Microphones.Where(p => p.settings.directoryIndex == id).Select(p => p.name));
                        if (lname.Count > 0)
                        {
                            resp = "{\"error\":\""+LocRm.GetString("ReassignAllCamerasMedia", lc) +"\"}";
                            break;
                        }
                        var lTemp = MainForm.Conf.MediaDirectories.ToList();
                        lTemp.RemoveAll(p => p.ID == Convert.ToInt32(ident));
                        MainForm.Conf.MediaDirectories = lTemp.ToArray();
                        MainForm.SaveConfig();
                        resp = "{\"actionResult\":\"reloadStorage\"}";
                    }
                    break;

                case "loadobjects":
                    {
                        template =
                            "{{\"name\":\"{0}\",\"groups\":\"{1}\",\"typeID\":{2},\"id\":{3},\"active\":{4},\"ptzID\":{5},\"talk\":{6},\"micID\":{7},\"camID\":{8}}},\n";


                        string objectList = "";
                        if (MainForm.Cameras != null)
                        {
                            objectList = MainForm.Cameras.Where(p => !p.deleted)
                                .Aggregate(objectList,
                                    (current, obj) =>
                                        current +
                                        string.Format(template, obj.name.JsonSafe(), obj.settings.accessgroups.JsonSafe(), 2, obj.id,
                                            obj.settings.active.ToString().ToLowerInvariant(), obj.ptz,
                                            (obj.settings.audiomodel != "None").ToString().ToLowerInvariant(),
                                            obj.settings.micpair, -1));
                        }
                        if (MainForm.Microphones != null)
                        {
                            foreach (var obj in MainForm.Microphones.Where(p => !p.deleted))
                            {
                                int camID = -1;
                                var oc = MainForm.Cameras.FirstOrDefault(p => p.settings.micpair == obj.id);
                                if (oc != null)
                                {
                                    camID = oc.id;
                                    if (oc.deleted)
                                        continue;
                                }

                                objectList += string.Format(template, obj.name.JsonSafe(), obj.settings.accessgroups.JsonSafe(), 1, obj.id,
                                                obj.settings.active.ToString().ToLowerInvariant(), -1, "false", -1, camID);
                            }

                        }
                        objectList = objectList.Trim().Trim(',');
                        resp = "{\"objectList\": [" + objectList + "]}";
                    }
                    break;
                case "getlist":
                    {
                        string source = GetVar(sPhysicalFilePath, "source");
                        string[] id = GetVar(sPhysicalFilePath, "id").Split(',');
                        string special = "";
                        var lcomms = new List<Helper.ListItem>();
                        switch (source)
                        {
                            case "ptzcommands":
                                special = "\"url\":\"{0}\",";
                                string url = "";
                                switch (id[0])
                                {
                                    case "-6":
                                    case "-1":
                                    case "-2":
                                        break;
                                    case "-3":
                                    case "-4":
                                        {
                                            lcomms.AddRange(PTZController.PelcoCommands.Select(c => new Helper.ListItem(c, c)));
                                        }
                                        break;

                                    case "-5":
                                    {
                                        var cc = MainForm.InstanceReference.GetCameraWindow(oid);
                                        lcomms.AddRange(cc.PTZ.ONVIFPresets.Select(c => new Helper.ListItem(c.Name, c.Name)));
                                    }
                                        break;

                                    default:
                                        var ptz = MainForm.PTZs.Single(p => p.id == Convert.ToInt32(id[0]));
                                        if (ptz != null)
                                        {
                                            url = ptz.CommandURL;
                                            if (ptz.ExtendedCommands?.Command != null)
                                            {
                                                lcomms.AddRange(ptz.ExtendedCommands.Command.Select(extcmd => new Helper.ListItem(extcmd.Name, extcmd.Value)));
                                            }
                                        }
                                        break;
                                }
                                special = string.Format(special, url);
                                break;
                        }
                        resp = lcomms.Aggregate("", (current, li) => current + string.Format(template, li.Name, li.Value.JsonSafe()));
                        resp = "{\"task\":\"" + GetVar(sPhysicalFilePath, "task") + "\",\"target\":\"" + GetVar(sPhysicalFilePath, "target") + "\"," + special + "\"list\": [" + resp.Trim(',') + "]}";
                    }
                    break;
                case "deleteobject":
                    {
                        if (io != null)
                        {
                            MainForm.InstanceReference.RemoveObject(io);
                        }

                        resp = "{\"actionResult\":\"ok\"}";
                    }
                    break;
                case "getsettings":
                    {

                        resp = File.ReadAllText(r + @"api\settings.json");
                        string ftp = MainForm.Conf.FTPServers.Aggregate("", (current, srv) => current + string.Format(template, srv.name, srv.ident));
                        resp = resp.Replace("FTPSERVERS", ftp.Trim(','));

                        string ds = MainForm.Conf.MediaDirectories.Aggregate("", (current, cfg) => current + string.Format(template, cfg.Entry.JsonSafe(), cfg.ID));
                        resp = resp.Replace("DIRECTORIES", ds.Trim(','));

                        dynamic d = PopulateResponse(resp, MainForm.Conf, lc);
                        resp = JsonConvert.SerializeObject(d);
                    }
                    break;
                case "editobject":
                    {
                        object o = null;
                        switch (ot)
                        {
                            case 1:
                                {
                                    objectsMicrophone om = MainForm.Microphones.FirstOrDefault(p => p.id == oid);
                                    if (om == null)
                                        throw new Exception("Microphone not found");
                                    resp = File.ReadAllText(r + @"api\editmicrophone.json");

                                    var ldev =
                                        Conf.DeviceList.Where(p => p.ObjectTypeID == 1 && !p.ExcludeFromOnline)
                                            .OrderBy(p => p.Name);
                                    string s = ldev.Aggregate("",
                                        (current, source) => current + string.Format(template, source.Name.JsonSafe(), source.SourceTypeID));

                                    resp = resp.Replace("SOURCETYPES", s.Trim(','));

                                    o = om;
                                }
                                break;
                            case 2:
                                {
                                    objectsCamera oc = MainForm.Cameras.FirstOrDefault(p => p.id == oid);
                                    if (oc == null)
                                        throw new Exception("Camera not found");

                                    resp = File.ReadAllText(r + @"api\editcamera.json");
                                    var feats = Enum.GetNames(typeof(RotateFlipType));

                                    string rm = feats.Aggregate("",
                                        (current, f) =>
                                            current + string.Format(template, Regex.Replace(f, "([a-z,0-9])([A-Z])", "$1 $2"), f));
                                    resp = resp.Replace("ROTATEMODES", rm.Trim(','));

                                    string ftps = MainForm.Conf.FTPServers.Aggregate("",
                                        (current, ftp) => current + string.Format(template, ftp.name.JsonSafe(), ftp.ident));
                                    resp = resp.Replace("FTPSERVERS", ftps.Trim(','));


                                    
                                    resp = resp.Replace("MASKS", GetFileOptionsList("Masks", "*.png"));


                                    var audio = "";
                                    audio += string.Format(template, "None", -1);
                                    audio = (from om in MainForm.Microphones.Where(p => !p.deleted) let om1 = om where MainForm.Cameras.Count(p => p.settings.micpair == om1.id && p.id != oc.id) == 0 select om).Aggregate(audio, (current, om) => current + string.Format(template, om.name.JsonSafe(), om.id));
                                    resp = resp.Replace("AUDIOSOURCES", audio.Trim(','));


                                    var ldev =
                                        Conf.DeviceList.Where(p => p.ObjectTypeID == 2 && !p.ExcludeFromOnline)
                                            .OrderBy(p => p.Name);
                                    string s = ldev.Aggregate("",
                                        (current, source) => current + string.Format(template, source.Name.JsonSafe(), source.SourceTypeID));

                                    resp = resp.Replace("SOURCETYPES", s.Trim(','));


                                    var ptzEntry = MainForm.PTZs.SingleOrDefault(p => p.id == oc.ptz);
                                    var commands = "";
                                    if (ptzEntry?.ExtendedCommands?.Command != null)
                                    {
                                        commands = ptzEntry.ExtendedCommands.Command.Aggregate(commands,
                                            (current, extcmd) => current + string.Format(template, extcmd.Name.JsonSafe(), extcmd.Value.JsonSafe()));
                                    }
                                    if (commands == "")
                                    {
                                        commands = string.Format(template, "None", "");
                                    }
                                    resp = resp.Replace("PTZCOMMANDS", commands.Trim(','));

                                    t = "{{ 'x': {0}, 'y': {1} , 'w': {2} , 'h': {3} }},";
                                    string zl = oc.detector.motionzones.Aggregate("", (current, z) => current + string.Format(t, z.left, z.top, z.width, z.height));

                                    resp = resp.Replace("ZONES", zl.Trim(','));

                                    t = "{{ 'id': {0}, 'x': {1}, 'y': {2} , 'w': {3} , 'h': {4} }},";

                                    var l = oc.settings.pip.config.Split('|');
                                    string pip = (from cfg in l where !string.IsNullOrEmpty(cfg) select cfg.Split(',') into p where p.Length == 5 select p).Aggregate("", (current, p) => current + string.Format(t, p[0], p[1], p[2], p[3], p[4]));

                                    resp = resp.Replace("PiPs", pip.Trim(','));

                                    o = oc;
                                }
                                break;
                        }

                        if (o != null)
                        {
                            var ds = MainForm.Conf.MediaDirectories.Aggregate("", (current, cfg) => current + string.Format(template, cfg.Entry.JsonSafe(), cfg.ID));
                            resp = resp.Replace("DIRECTORIES", ds.Trim(','));

                            resp = resp.Replace("OBJECTID", oid.ToString(CultureInfo.InvariantCulture));
                            dynamic d = PopulateResponse(resp, o, lc);
                            resp = JsonConvert.SerializeObject(d);
                            resp = resp.Replace("MASKFOLDER", Program.AppPath.JsonSafe() + "Masks");
                        }
                    }
                    break;
                case "record":
                    io?.RecordSwitch(true);
                    resp = commandExecuted;
                    break;
                case "recordstop":
                    io?.RecordSwitch(false);
                    resp = commandExecuted;
                    break;
                case "snapshot":
                    cw = io as CameraWindow;
                    cw?.SaveFrame();
                    resp = commandExecuted;
                    break;
                case "getptzcommands":
                    ptzid = Convert.ToInt32(GetVar(sPhysicalFilePath, "ptzid"));
                    string cmdlist = "";
                    switch (ptzid)
                    {
                        default:
                            PTZSettings2Camera ptz = MainForm.PTZs.SingleOrDefault(p => p.id == ptzid);
                            if (ptz?.ExtendedCommands?.Command != null)
                            {
                                cmdlist = ptz.ExtendedCommands.Command.Aggregate("",
                                    (current, extcmd) =>
                                        current +
                                        (string.Format(template, extcmd.Name.JsonSafe(), extcmd.Value.JsonSafe())));
                            }
                            break;
                        case -2:
                        case -1: //digital (none)
                        case -6: //(none)
                            break;
                        case -3:
                        case -4:
                            cmdlist = PTZController.PelcoCommands.Aggregate(cmdlist,
                                                                            (current, c) =>
                                                                            current +
                                                                            (string.Format(template, c.JsonSafe(), c.JsonSafe())));
                            break;
                        case -5:
                            cw = io as CameraWindow;
                            if (cw?.PTZ?.ONVIFPresets.Length > 0)
                            {
                                cmdlist = cw.PTZ.ONVIFPresets.Aggregate(cmdlist,
                                    (current, c) =>
                                        current +
                                        (string.Format(template, c.Name.JsonSafe(), c.Name.JsonSafe())));
                            }
                            break;
                    }
                    resp = "{\"elements\":[" + cmdlist.Trim(',') + "]}";
                    break;
                case "switchoff":
                    io?.Disable();

                    resp = commandExecuted;
                    break;
                case "switchon":
                    io?.Enable();

                    resp = commandExecuted;
                    break;
                case "getobjectstatus":
                    {
                        t =
                            "{{\"online\":{0},\"recording\":{1},\"width\":{2},\"height\":{3},\"micpairid\":{4},\"talk\":{5}}}";

                        vl = io as VolumeLevel;
                        if (vl != null)
                        {
                            resp = string.Format(t, vl.IsEnabled.ToString().ToLower(),
                                vl.ForcedRecording.ToString().ToLower(), 320, 40, -1, "false");
                        }
                        cw = io as CameraWindow;
                        if (cw != null)
                        {

                            string[] res = cw.Camobject.resolution.Split('x');
                            string micpairid = "-1";
                            if (cw.VolumeControl != null)
                                micpairid = cw.VolumeControl.Micobject.id.ToString(CultureInfo.InvariantCulture);

                            resp = string.Format(t, cw.IsEnabled.ToString().ToLower(),
                                cw.ForcedRecording.ToString().ToLower(), res[0], res[1], micpairid, (cw.Camobject.settings.audiomodel != "None").ToString().ToLower());
                        }
                    }
                    break;
                case "synthtocam":
                    {
                        var txt = GetVar(sPhysicalFilePath, "text");
                        cw = io as CameraWindow;
                        if (cw != null)
                        {
                            Sources.Audio.SpeechSynth.Say(txt, cw);
                        }
                        resp = commandExecuted;
                    }
                    break;
                case "getcameragrabs":
                    {
                        sd = GetVar(sPhysicalFilePath, "startdate");
                        ed = GetVar(sPhysicalFilePath, "enddate");
                        int pagesize = Convert.ToInt32(GetVar(sPhysicalFilePath, "pagesize"));
                        var page = Convert.ToInt32(GetVar(sPhysicalFilePath, "page"));
                        if (sd != "")
                            sdl = Convert.ToInt64(sd);
                        if (ed != "")
                            edl = Convert.ToInt64(ed);

                        var grablist = new StringBuilder("");
                        var ocgrab = MainForm.Cameras.FirstOrDefault(p => p.id == oid);
                        if (ocgrab != null)
                        {
                            var dirinfo = new DirectoryInfo(Helper.GetMediaDirectory(2, oid) + "video\\" +
                                                            ocgrab.directory + "\\grabs\\");

                            var lFi = new List<FileInfo>();
                            lFi.AddRange(dirinfo.GetFiles());
                            lFi =
                                lFi.FindAll(
                                    f =>
                                    f.Extension.ToLower() == ".jpg" && (sdl == 0 || f.CreationTime.Ticks > sdl) &&
                                    (edl == 0 || f.CreationTime.Ticks < edl));
                            lFi = lFi.OrderByDescending(f => f.CreationTime).ToList();



                            total = lFi.Count;
                            lFi = lFi.Skip(page * pagesize).Take(pagesize).ToList();

                            int max = 10000;
                            if (lFi.Count > 0)
                            {
                                foreach (var f in lFi)
                                {
                                    grablist.Append("{\"name\":\"" + f.Name + "\",\"time\":\"" + f.CreationTime.UnixTicks() + "\"},");
                                    max--;
                                    if (max == 0)
                                        break;
                                }
                            }
                        }
                        resp = "{\"total\":" + total + ",\"page\":" + page + ",\"pagesize\":" + pagesize + ",\"results\":[" +
                               grablist.ToString().Trim(',') + "]}";
                        break;
                    }
                case "getlogfilelist":
                    {
                        var dirinfo = new DirectoryInfo(Program.AppDataPath);
                        var lFi = new List<FileInfo>();
                        lFi.AddRange(dirinfo.GetFiles());
                        lFi = lFi.FindAll(f => f.Extension.ToLower() == ".htm" && f.Name.StartsWith("log_"));
                        lFi = lFi.OrderByDescending(f => f.CreationTime).ToList();
                        string logs = lFi.Aggregate("", (current, f) => current + ("{\"name\":\"" + f.Name + "\"},"));
                        resp = "{\"logs\":[" + logs.Trim(',') + "]}";
                    }
                    break;
                case "getcmdlist":
                {
                    var l = "";
                    t = "{{\"id\":{0},\"name\":\"{1}\"}},";
                    foreach (objectsCommand ocmd in MainForm.RemoteCommands)
                    {
                        string n = ocmd.name;
                        if (n.StartsWith("cmd_"))
                        {
                            n = LocRm.GetString(ocmd.name, lc);
                        }

                        l += string.Format(t, ocmd.id, n.JsonSafe());
                    }
                    resp = "{\"commands\":[" + l.Trim(',') + "]}";
                }
                    break;
                case "executecmd":
                    {
                        objectsCommand oc = MainForm.RemoteCommands.SingleOrDefault(p => p.id == Convert.ToInt32(GetVar(sPhysicalFilePath, "id")));
                        resp = commandExecuted;
                        if (oc != null)
                        {
                            try
                            {
                                if (oc.command.StartsWith("ispy ") || oc.command.StartsWith("ispypro.exe "))
                                {
                                    string cmd2 =
                                        oc.command.Substring(oc.command.IndexOf(" ", StringComparison.Ordinal) + 1).Trim();

                                    int k = cmd2.ToLower().IndexOf("commands ", StringComparison.Ordinal);
                                    if (k != -1)
                                    {
                                        cmd2 = cmd2.Substring(k + 9);
                                    }
                                    cmd2 = cmd2.Trim('"');
                                    string[] commands = cmd2.Split('|');
                                    foreach (string command2 in commands)
                                    {
                                        if (!string.IsNullOrEmpty(command2))
                                        {
                                            MainForm.ProcessCommandInternal(command2.Trim('"'));
                                        }
                                    }
                                }
                                else
                                {
                                    Process.Start(oc.command);
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.LogException(ex, "Server");
                                resp = string.Format(commandFailed, ex.Message);
                            }
                        }
                    }
                    break;
                case "getgraph":
                {
                    FilesFile ff = null;
                    t = "{{\"duration\":{0},\"threshold\":{1},\"raw\":[{2}]}}";

                    vl = io as VolumeLevel;
                    string fn = GetVar(sPhysicalFilePath, "fn");
                    if (vl != null)
                    {
                        ff = vl.FileList.FirstOrDefault(p => p.Filename == fn);
                    }
                    cw = io as CameraWindow;
                    if (cw != null)
                    {
                        ff = cw.FileList.FirstOrDefault(p => p.Filename == fn);
                    }


                    if (ff != null)
                    {
                        resp = string.Format(t, ff.DurationSeconds,
                            string.Format(CultureInfo.InvariantCulture, "{0:0.000}", ff.TriggerLevel), ff.AlertData);
                    }
                    else
                    {
                        resp = string.Format(t, 0, 0, "");
                    }
                }
                    break;
                case "ptzcommand":
                    cw = io as CameraWindow;
                    if (cw != null)
                    {
                        var value = GetVar(sPhysicalFilePath, "value");
                        if (value != "")
                        {
                            try
                            {
                                if (value.StartsWith("ispydir_"))
                                {
                                    cw.PTZ.SendPTZCommand(
                                        (Enums.PtzCommand)Convert.ToInt32(value.Replace("ispydir_", "")));
                                }
                                else
                                    cw.PTZ.SendPTZCommand(value);
                            }
                            catch (Exception ex)
                            {
                                Logger.LogError(LocRm.GetString("Validate_Camera_PTZIPOnly") + ": " +
                                                      ex.Message, "Server");
                            }
                        }
                        resp = commandExecuted;
                    }
                    break;
                case "getevents":
                {
                        string num = GetVar(sPhysicalFilePath, "num");
                        if (num == "")
                            num = "500";
                        int n = Convert.ToInt32(num);

                        DateTime timestamp = Helper.Now;
                        
                        sd = GetVar(sPhysicalFilePath, "startdate");
                        ed = GetVar(sPhysicalFilePath, "enddate");

                        sdl = sd != "" ? Convert.ToInt64(sd) : 0;
                        edl = ed != "" ? Convert.ToInt64(ed) : long.MaxValue;


                        List<FilePreview> ffs = MainForm.MasterFileList.OrderByDescending(p => p.CreatedDateTicksUTC).ToList();

                        if (sdl > 0)
                            ffs = ffs.FindAll(f => f.CreatedDateTicksUTC > sdl);

                        if (edl < long.MaxValue)
                            ffs = ffs.FindAll(f => f.CreatedDateTicksUTC < edl);
                            
                        ffs = ffs.Take(n).ToList();
                        var sb = new StringBuilder();
                        sb.Append("[");
                        foreach (var f in ffs)
                        {
                            sb.Append("{\"ot\":");
                            sb.Append(f.ObjectTypeId);
                            sb.Append(",\"oid\":");
                            sb.Append(f.ObjectId);
                            sb.Append(",\"created\":");
                            sb.Append(string.Format(CultureInfo.InvariantCulture, "{0:0.00}", f.CreatedDateTicksUTC));
                            sb.Append(",\"maxalarm\":");
                            sb.Append(string.Format(CultureInfo.InvariantCulture, "{0:0.0}",f.MaxAlarm));
                            sb.Append(",\"duration\": ");
                            sb.Append(f.Duration);
                            sb.Append(",\"filename\":\"");
                            sb.Append(f.Filename);
                            sb.Append("\"},");
                        }
                        resp = sb.ToString().Trim(',') + "]";

                        var tzo = Convert.ToInt32(TimeZone.CurrentTimeZone.GetUtcOffset(new DateTime()).TotalMinutes);
                        resp = "{\"timeStamp\":" + timestamp.Ticks + ",\"timezoneOffset\":"+tzo+",\"events\": " + resp + "}";
                    }
                    break;
                //post commands
                //case "uploadyoutube":
                //    {
                //        var d = getJSONObject(sBuffer);
                //        t = YouTubeUploader.Upload(oid, Helper.GetFullPath(ot, oid) + d.files[0].name, out success);
                //        resp = "{\"action\":\"" + cmd + "\",\"status\":\"" + (success ? "ok" : "Upload Failed ("+t.JsonSafe()+")") + "\"}";
                //    }
                //    break;
                case "uploadcloud":
                    {
                        var d = getJSONObject(sBuffer);
                        t = CloudGateway.Upload(ot, oid, Helper.GetFullPath(ot, oid) + d.files[0].name, out success);
                        resp = "{\"action\":\"" + cmd + "\",\"status\":\"" + (success ? "ok" : "Upload Failed (" + t.JsonSafe() + ")") + "\"}";
                    }
                    break;
                case "massdelete":
                    {
                        var d = getJSONObject(sBuffer);
                        var files = new List<string>();
                        foreach (var file in d.files)
                        {
                            files.Add(file.name.ToString());
                        }

                        DeleteFiles(oid,ot, files);

                        resp = "{\"action\":\"" + cmd + "\",\"status\":\"ok\"}";
                    }
                    break;
                case "massdeletegrabs":
                    {
                        var d = getJSONObject(sBuffer);
                        var files = new List<string>();
                        foreach (var file in d.files)
                        {
                            files.Add(file.name.ToString());
                        }
                        var folderpath = Helper.GetMediaDirectory(ot, oid) + "video\\" + Helper.GetDirectory(ot, oid) + "\\grabs\\";

                        foreach (string file in files)
                        {
                            FileOperations.Delete(folderpath + file);
                        }

                        resp = "{\"action\":\"" + cmd + "\",\"status\":\"ok\"}";
                    }

                    break;

                case "archive":
                    {
                        success = false;
                        if (io == null)
                        {
                            t = LocRm.GetString("ObjectNotFound", lc);
                        }
                        else
                        {
                            if (Helper.CanArchive)
                            {
                                var d = getJSONObject(sBuffer);
                                string dir = "audio";
                                if (ot == 2)
                                    dir = "video";

                                var folderpath = Helper.GetMediaDirectory(ot, oid) + dir + "\\" +
                                                 Helper.GetDirectory(ot, oid) + "\\";
                                success = true;
                                var files = new List<string>();
                                foreach (var file in d.files)
                                {
                                    success = Helper.ArchiveFile(io, folderpath + file.name.ToString())!="NOK";
                                    if (!success)
                                    {
                                        t = LocRm.GetString("ArchiveFailed", lc);
                                        break;
                                    }
                                    files.Add(file.name.ToString());
                                }

                                if (files.Count>0)
                                {
                                    DeleteFiles(oid,ot, files);
                                }

                            }
                            else
                            {
                                
                                t = LocRm.GetString("InvalidDirectory", lc);
                            }
                        }
                        resp = "{\"action\":\"" + cmd + "\",\"status\":\"" + (success ? "ok" : t) + "\"}";
                    }
                    break;
                default:
                    return;
            }
            SendResponse(sHttpVersion, "application/json", resp, " 200 OK", 0, req);
        }

        void DeleteFiles(int oid, int ot, List<string> files)
        {
            string dir = "audio";
            if (ot == 2)
            {
                dir = "video";
                ot = 2;
            }

            var folderpath = Helper.GetMediaDirectory(ot, oid) + dir + "\\" +
                         Helper.GetDirectory(ot, oid) + "\\";

            VolumeLevel vlUpdate = null;
            CameraWindow cwUpdate = null;
            if (ot == 1)
            {
                vlUpdate = MainForm.InstanceReference.GetVolumeLevel(oid);
                if (vlUpdate == null)
                {
                    return;
                }
            }
            if (ot == 2)
            {
                cwUpdate = MainForm.InstanceReference.GetCameraWindow(oid);
                if (cwUpdate == null)
                {
                    return;
                }
            }
            foreach (string fn3 in files)
            {
                var fi = new FileInfo(folderpath +
                                      fn3);
                string ext = fi.Extension.Trim();
                FileOperations.Delete(folderpath + fn3);
                if (ot == 2)
                {
                    FileOperations.Delete(folderpath + "thumbs\\" + fn3.Replace(ext, ".jpg"));
                    FileOperations.Delete(folderpath + "thumbs\\" + fn3.Replace(ext, "_large.jpg"));
                }
                string filename1 = fn3;
                if (ot == 1)
                {
                    vlUpdate?.RemoveFile(filename1);
                }
                if (ot == 2)
                {
                    cwUpdate?.RemoveFile(filename1);
                }
            }
            MainForm.NeedsMediaRefresh = Helper.Now;
        }


        void ScannerScanFinished(object sender, EventArgs e)
        {
            _scanner = null;
        }

        void ScannerDeviceFound(object sender, DeviceFoundEventArgs e)
        {
            _scanResults.Add(e.Device);
        }

        dynamic getJSONObject(string sBuffer)
        {
            int i = sBuffer.IndexOf("\r\n\r\n", StringComparison.Ordinal);

            if (i > -1)
            {
                string js = sBuffer.Substring(i).Trim();
                return JsonConvert.DeserializeObject(js);
            }
            return null;
        }

        static string FormatTime(int t)
        {
            var ts = TimeSpan.FromMinutes(t);
            string h = ts.Hours.ToString(CultureInfo.InvariantCulture);
            string m = ts.Minutes.ToString(CultureInfo.InvariantCulture);
            if (h.Length == 1)
                h = "0" + h;
            if (m.Length == 1)
                m = "0" + m;
            return h + ":" + m;
        }

        static string FormatDays(IEnumerable<string> d)
        {
            string r = "";
            foreach (var day in d)
            {
                switch (day)
                {
                    case "1":
                        r += "Mon,";
                        break;
                    case "2":
                        r += "Tue,";
                        break;
                    case "3":
                        r += "Wed,";
                        break;
                    case "4":
                        r += "Thu,";
                        break;
                    case "5":
                        r += "Fri,";
                        break;
                    case "6":
                        r += "Sat,";
                        break;
                    case "0":
                        r += "Sun,";
                        break;
                }
            }
            return r.Trim(',');
        }

        void SaveJson(string sPhysicalFilePath, string sHttpVersion, string sBuffer, HttpRequest req)
        {
            string resp = "";
            int ot, oid;

            int.TryParse(GetVar(sPhysicalFilePath, "oid"), out oid);
            int.TryParse(GetVar(sPhysicalFilePath, "ot"), out ot);
            var io = MainForm.InstanceReference.GetISpyControl(ot, oid);

            string lc = GetVar(sPhysicalFilePath, "lc");
            if (LocRm.TranslationSets.All(p => p.CultureCode != lc))
                lc = "en";

            bool saveObjects = true;
            try
            {
                var d = getJSONObject(sBuffer);
                if (d == null)
                    return;

                string n = d.name;
                resp = "{\"actionResult\":\"ok\"}";
                bool apply = false;
                string nl = n.ToLowerInvariant();
                switch (nl)
                {
                    default:
                    {
                        resp = "{\"error\":\"" + nl + " not recognized\"}";
                        saveObjects = false;
                    }
                        break;
                    case "liveupdate":
                    {
                        switch (ot)
                        {
                            case 1:
                            {
                                var c = MainForm.Microphones.FirstOrDefault(p => p.id == oid);
                                if (c != null)
                                {
                                    PopulateObject(d, c);
                                }
                            }
                                break;
                            case 2:
                            {
                                var c = MainForm.Cameras.FirstOrDefault(p => p.id == oid);
                                if (c != null)
                                {
                                    PopulateObject(d, c);
                                }
                            }
                                break;
                        }
                        apply = true;
                        saveObjects = false;
                    }
                        break;
                    case "editpelco":
                    {
                        var c = MainForm.Cameras.FirstOrDefault(p => p.id == oid);
                        if (c != null)
                        {
                            PopulateObject(d, c);
                        }
                    }
                        break;
                    case "settings":
                    {
                        PopulateObject(d, MainForm.Conf);
                        if (!string.IsNullOrEmpty(MainForm.Conf.ArchiveNew))
                        {
                            MainForm.Conf.ArchiveNew = MainForm.Conf.ArchiveNew.Replace("/", @"\");
                            if (!MainForm.Conf.ArchiveNew.EndsWith(@"\"))
                                MainForm.Conf.ArchiveNew += @"\";
                        }
                        ReloadAllowedIPs();
                        ReloadAllowedReferrers();
                        MainForm.SaveConfig();
                        saveObjects = false;
                    }
                        break;
                    case "editftpserver":
                    {
                        resp = "{\"actionResult\":\"reloadFTPServers\"}";
                        if (d.ident == "new")
                        {
                            d.ident = Guid.NewGuid().ToString();
                            var cfgs = new configurationServer {ident = d.ident};
                            var l = MainForm.Conf.FTPServers.ToList();
                            l.Add(cfgs);
                            MainForm.Conf.FTPServers = l.ToArray();
                        }

                        PopulateObject(d, MainForm.Conf.FTPServers.First(p => p.ident == d.ident.ToString()));
                        MainForm.SaveConfig();
                        saveObjects = false;
                    }
                        break;
                    case "editstorage":
                    {
                        resp = "{\"actionResult\":\"reloadStorage\"}";
                        if (d.ident.ToString() == "new")
                        {
                            d.ident = MainForm.Conf.MediaDirectories.Max(p => p.ID) + 1;
                        }
                        int idnew = Convert.ToInt32(d.ident);
                        d.ident = idnew;

                        var md = MainForm.Conf.MediaDirectories.FirstOrDefault(p => p.ID == idnew);
                        bool ndir = false;
                        if (md == null)
                        {
                            md = new configurationDirectory {ID = d.ident, Entry = ""};
                            ndir = true;
                        }

                        var exdir = md.Entry;
                        PopulateObject(d, md);
                        md.Entry = md.Entry.Replace("/", @"\");
                        if (!md.Entry.EndsWith(@"\"))
                            md.Entry += @"\";

                        try
                        {
                            if (!Directory.Exists(md.Entry))
                            {
                                throw new Exception("Invalid Directory");
                            }
                        }
                        catch (Exception ex)
                        {
                            if (exdir != "")
                                md.Entry = exdir;
                            resp = "{\"actionResult\":\"reloadStorage\",\"error\":\"" + ex.Message.JsonSafe() + "\"}";
                            break;
                        }

                        if (ndir)
                        {
                            var l = MainForm.Conf.MediaDirectories.ToList();
                            l.Add(md);
                            MainForm.Conf.MediaDirectories = l.ToArray();
                        }
                        else
                        {
                            var di = new DirectoryInfo(exdir);
                            var di2 = new DirectoryInfo(md.Entry);
                            if (di.ToString() != di2.ToString())
                            {
                                var t = new Thread(() => Helper.CopyFolder(exdir, md.Entry)) {IsBackground = true};
                                t.Start();
                                resp =
                                    "{\"actionResult\":\"reloadStorage\",\"message\":\""+LocRm.GetString("MediaBeingCopied", lc) +"\"}";
                            }
                        }

                        MainForm.SaveConfig();
                        saveObjects = false;
                    }
                        break;
                    case "editcamera":
                        {
                            var c = MainForm.Cameras.FirstOrDefault(p => p.id == oid);
                            if (c != null)
                            {
                                var olddir = c.directory;
                                PopulateObject(d, c);
                                apply = true;
                                var newdir = c.directory;
                                if (olddir.ToLowerInvariant() != newdir.ToLowerInvariant())
                                {
                                    if (!Helper.IsAlphaNumeric(newdir))
                                    {
                                        c.directory = olddir;
                                        resp = "{\"error\":\""+LocRm.GetString("DirectoryInvalid", lc) +"\"}";
                                    }
                                    else
                                    {
                                        var fullolddir = Helper.GetMediaDirectory(2, c.id) + "video\\" + olddir + "\\";
                                        var fullnewdir = Helper.GetMediaDirectory(2, c.id) + "video\\" + newdir + "\\";
                                        try
                                        {
                                            Directory.Move(fullolddir, fullnewdir);
                                        }
                                        catch (Exception ex)
                                        {
                                            c.directory = olddir;
                                            resp = "{\"error\":\""+ex.Message.JsonSafe()+"\"}";
                                        }
                                    }
                                }
                                MainForm.InstanceReference.SaveObjectList(false);
                            }
                        }
                        break;
                    case "motionzones":
                    {
                        var c = MainForm.Cameras.FirstOrDefault(p => p.id == oid);
                        if (c != null)
                        {
                            var lz = new List<objectsCameraDetectorZone>();
                            if (d.zones != null)
                            {
                                foreach (var z in d.zones)
                                {
                                    var x = Convert.ToInt32(z["x"].Value);
                                    var y = Convert.ToInt32(z["y"].Value);
                                    var w = Convert.ToInt32(z["w"].Value);
                                    var h = Convert.ToInt32(z["h"].Value);
                                    lz.Add(new objectsCameraDetectorZone { height = h, left = x, top = y, width = w });
                                }
                            }
                            c.detector.motionzones = lz.ToArray();
                            var cw = MainForm.InstanceReference.GetCameraWindow(oid);
                            cw?.Camera?.SetMotionZones(lz.ToArray());
                        }
                        saveObjects = false;
                    }
                        break;
                    case "screenarea":
                        {
                            var c = MainForm.Cameras.FirstOrDefault(p => p.id == oid);
                            if (c != null)
                            {

                                if (d.zones != null)
                                {
                                    var z = d.zones[0];
                                    int si;
                                    int.TryParse(c.settings.videosourcestring, out si);
                                    if (Screen.AllScreens.Length <= si)
                                        si = 0;
                                    var s = Screen.AllScreens[si];
                                    var x = Convert.ToInt32((Convert.ToDecimal(z["x"].Value) / 100) * s.WorkingArea.Width);
                                    var y = Convert.ToInt32((Convert.ToDecimal(z["y"].Value) / 100) * s.WorkingArea.Height);
                                    var w = Convert.ToInt32((Convert.ToDecimal(z["w"].Value) / 100) * s.WorkingArea.Width);
                                    var h = Convert.ToInt32((Convert.ToDecimal(z["h"].Value) / 100) * s.WorkingArea.Height);
                                    //even height and width
                                    if (w % 2 != 0)
                                        w -= 1;
                                    if (h % 2 != 0)
                                        h -= 1;

                                    c.settings.desktoparea = x + "," + y + "," + w + "," + h;
                                }
                                else
                                    c.settings.desktoparea = "";
                            }
                            saveObjects = false;
                        }
                        
                        break;
                    case "pip":
                        {
                            var c = MainForm.Cameras.FirstOrDefault(p => p.id == oid);
                            if (c != null)
                            {
                                string cfg = "";
                                if (d.zones != null)
                                {
                                    foreach (var z in d.zones)
                                    {
                                        var id = Convert.ToInt32(z["id"].Value);
                                        var x = Convert.ToInt32(z["x"].Value);
                                        var y = Convert.ToInt32(z["y"].Value);
                                        var w = Convert.ToInt32(z["w"].Value);
                                        var h = Convert.ToInt32(z["h"].Value);
                                        cfg += id + "," + x + "," + y + "," + w + "," + h + "|";
                                    }
                                }
                                c.settings.pip.config = cfg.Trim('|');
                                
                                var cw = MainForm.InstanceReference.GetCameraWindow(oid);
                                if (cw?.Camera != null)
                                {
                                    cw.Camera.PiPConfig = c.settings.pip.config;
                                }

                            }
                            saveObjects = false;
                        }
                        
                        break;
                    case "editmicrophone":
                        {
                            var c = MainForm.Microphones.FirstOrDefault(p => p.id == oid);
                            if (c != null)
                            {
                                var olddir = c.directory;
                                PopulateObject(d, c);
                                apply = true;
                                var newdir = c.directory;
                                if (olddir.ToLowerInvariant() != newdir.ToLowerInvariant())
                                {
                                    if (!Helper.IsAlphaNumeric(newdir))
                                    {
                                        c.directory = olddir;
                                        resp = "{\"error\":\""+LocRm.GetString("DirectoryInvalid", lc) +"\"}";
                                    }
                                    else
                                    {
                                        var fullolddir = Helper.GetMediaDirectory(2, c.id) + "video\\" + olddir + "\\";
                                        var fullnewdir = Helper.GetMediaDirectory(2, c.id) + "video\\" + newdir + "\\";
                                        try
                                        {
                                            Directory.Move(fullolddir, fullnewdir);
                                        }
                                        catch (Exception ex)
                                        {
                                            c.directory = olddir;
                                            resp = "{\"error\":\"" + ex.Message.JsonSafe() + "\"}";
                                        }
                                    }
                                } 
                                MainForm.InstanceReference.SaveObjectList(false);
                            }
                        }
                        break;
                    case "editschedule":
                        {
                            resp = "{\"actionResult\":\"reloadSchedule\"}";
                            objectsScheduleEntry s = null;
                            if (d.ident.ToString() == "new")
                            {
                                s = new objectsScheduleEntry
                                {
                                    active = true,
                                    time = 0,
                                    daysofweek = "",
                                    objectid = oid,
                                    objecttypeid = ot,
                                    parameter = "",
                                    typeid = 0
                                };
                                MainForm.Schedule.Add(s);
                            }
                            else
                            {
                                var l = MainForm.Schedule.Where(p => p.objectid == oid && p.objecttypeid == ot).ToList();
                                int i = Convert.ToInt32(d.ident);

                                if (i < l.Count)
                                {
                                    s = l[i];
                                }
                            }
                            if (s != null)
                            {
                                PopulateObject(d, s);
                                io?.ReloadSchedule();
                            }
                        }
                        break;
                    case "editptzschedule":
                        {
                            resp = "{\"actionResult\":\"reloadPTZSchedule\"}";
                            var oc = MainForm.Cameras.First(p => p.id == oid);
                            objectsCameraPtzscheduleEntry pe;
                            if (d.ident.ToString() == "new")
                            {
                                pe = new objectsCameraPtzscheduleEntry
                                {
                                    command = "",
                                    time = new DateTime(),
                                    token = ""
                                };

                                PopulateObject(d, pe);
                                var l = oc.ptzschedule.entries.ToList();
                                l.Add(pe);
                                oc.ptzschedule.entries = l.ToArray();
                            }
                            else
                            {
                                int i = Convert.ToInt32(d.ident);
                                var l = oc.ptzschedule.entries.ToList();
                                if (i < l.Count)
                                {
                                    pe = l[i];
                                    PopulateObject(d, pe);

                                }
                            }
                        }
                        break;
                    case "editaction":
                        resp = "{\"actionResult\":\"reloadActions\"}";
                        objectsActionsEntry a;
                        if (d.ident.ToString() == "new")
                        {
                            a = new objectsActionsEntry
                            {
                                type = "",
                                mode = "",
                                active = true,
                                objectid = oid,
                                objecttypeid = ot,
                                ident = Guid.NewGuid().ToString()
                            };
                            MainForm.Actions.Add(a);
                            resp = resp.Replace("PARAMS", "");
                        }
                        else
                        {
                            a = MainForm.Actions.FirstOrDefault(p => p.ident == d.ident.ToString());
                        }
                        if (a != null)
                        {
                            var p1 = a.param1;
                            PopulateObject(d, a);
                            if (Helper.WebRestrictedAlertTypes.Contains(a.type))
                            {
                                a.param1 = a.param1.Replace("/", "\\");
                                if (a.param1.IndexOf("\\", StringComparison.Ordinal)!=-1)
                                {
                                    resp = "{\"error\":\""+LocRm.GetString("NoPathsAllowed", lc) +"\"}";
                                    a.param1 = p1;
                                }
                            }
                            
                            MainForm.InstanceReference.SaveObjectList(false);
                        }
                        break;
                    case "editaudiosource":
                    {
                        var c = MainForm.Microphones.FirstOrDefault(p => p.id == oid);
                        if (c != null)
                        {
                            PopulateObject(d, c);
                            
                            var vl = MainForm.InstanceReference.GetVolumeLevel(c.id);
                            vl?.Disable();
                        }
                        resp = "{\"actionResult\":\"waiteditobject\"}";
                    }
                        break;
                    case "editvideosource":
                        {
                            var c = MainForm.Cameras.FirstOrDefault(p => p.id == oid);
                            if (c != null)
                            {
                                PopulateObject(d, c);

                                if (c.settings.sourceindex == 9)
                                {
                                    c.ptz = -5;
                                    var ss = NV(c.settings.namevaluesettings, "use");
                                    //c.settings.sourceindex = ss == "VLC" ? 5 : 2;
                                    int pi = 0;
                                    int.TryParse(NV(c.settings.namevaluesettings, "profilename"), out pi);
                                    c.settings.videosourcestring = "";
                                }

                                var cw = MainForm.InstanceReference.GetCameraWindow(c.id);
                                cw?.Disable();
                                resp = "{\"actionResult\":\"waiteditobject\"}";
                            }

                        }
                        break;
                }
                if (apply)
                {
                    io?.Apply();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "JSON Parser");
            }
            if (saveObjects)
            {
                try
                {
                    MainForm.InstanceReference.SaveObjectList(false);
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex, "JSON Parser");
                }
            }

            SendResponse(sHttpVersion, "application/json", resp, " 200 OK", 0, req);
        }


        //private string _apiJson;
        string BuildApijson(string r, string lc)
        {
            string resp = File.ReadAllText(r + @"api\api.json");
            string template =
                "{{\"text\":\"{0}\",\"typeID\":{1},\"sourceTypeID\":{2}, \"noTranslate\":true}},";

            var ldev =
                Conf.DeviceList.Where(p => p.ObjectTypeID == 2 && !p.ExcludeFromOnline).OrderBy(p => p.Name);
            string s = ldev.Aggregate("",
                (current, source) => current + string.Format(template, LocRm.GetString(source.Name, lc), source.ObjectTypeID, source.SourceTypeID));

            resp = resp.Replace("SOURCETYPES", s.Trim(','));

            ldev = Conf.DeviceList.Where(p => p.ObjectTypeID == 1 && !p.ExcludeFromOnline).OrderBy(p => p.Name);
            s = ldev.Aggregate("",
                (current, source) => current + string.Format(template, LocRm.GetString(source.Name, lc), source.ObjectTypeID, source.SourceTypeID));

            resp = resp.Replace("AUDIOTYPES", s.Trim(','));

            template = "{{\"text\":\"{0}\",\"value\":\"{1},{2}\", \"noTranslate\":true}},";

            var ptzList = new List<Helper.ListItem>
                                  {
                                      new Helper.ListItem(":: NONE", "-6"),
                                      new Helper.ListItem(":: DIGITAL", "-1"),
                                      new Helper.ListItem(":: IAM-CONTROL", "-2"),
                                      new Helper.ListItem(":: ONVIF", "-5"),
                                      new Helper.ListItem(":: PELCO-P", "-3"),
                                      new Helper.ListItem(":: PELCO-D", "-4")
                                  };

            foreach (PTZSettings2Camera ptz in MainForm.PTZs)
            {
                int j = 0;
                foreach (var m in ptz.Makes)
                {
                    string ttl = (m.Name + " " + m.Model).Trim();
                    var ptze = new Helper.ListItem(ttl.JsonSafe(), ptz.id.ToString(CultureInfo.InvariantCulture), j);

                    if (!ptzList.Contains(ptze))
                        ptzList.Add(ptze);
                    j++;
                }
            }
            ptzList = ptzList.OrderBy(p => p.Name).ToList();

            string ptzm = ptzList.Aggregate("", (current, ptz) => current + string.Format(template, ptz.Name, ptz.Value, ptz.Index));

            var apiJson = resp.Replace("PTZMODELS", ptzm.Trim(','));

            try
            {
                dynamic d = PopulateResponse(apiJson, null, lc);
                return JsonConvert.SerializeObject(d);
            }
            catch
            {
                Logger.LogException(new Exception(apiJson),"BuildAPIPopulate");
                throw;
            }
        }

        string NV(string source, string name)
        {
            if (string.IsNullOrEmpty(source))
                return "";
            name = name.ToLower().Trim();
            string[] settings = source.Split(',');
            foreach (string[] nv in settings.Select(s => s.Split('=')).Where(nv => nv[0].ToLower().Trim() == name))
            {
                return nv[1];
            }
            return "";
        }

        string NVSet(string source, string name, string value)
        {
            if (source == null) source = "";

            name = name.ToLower().Trim();

            string[] settings = source.Split(',');
            bool isset = false;
            for (int i = 0; i < settings.Length; i++)
            {
                if (settings[i].ToLower().StartsWith(name + "="))
                {
                    settings[i] = name + "=" + value;
                    isset = true;
                    break;
                }
            }
            if (!isset)
            {
                var l = settings.ToList();
                l.Add(name + "=" + value);
                settings = l.ToArray();
            }
            return string.Join(",", settings);
        }

        string Translate(string json, string lc)
        {
            dynamic d = PopulateResponse(json, null, lc);
            return JsonConvert.SerializeObject(d);
        }
        void Translate(ref dynamic item, string lc)
        {
            if (item["noTranslate"] != null)
            {
                return;
            }

            var txt = item["text"];
            if (txt != null)
            {
                string v = txt.Value.ToString();
                string tag = TagUp(v);

                txt.Value = LocRm.GetString(tag, lc);

                var hlp = item["help"];
                if (hlp != null)
                {
                    tag += ".help";
                    hlp.Value = LocRm.GetString(tag, lc);
                }
            }
            var typ = item["type"];
            if (typ != null && (typ.Value == "Button" || typ.Value=="Link"))
            {
                var val = item["value"];
                if (val != null)
                {
                    string v = val.Value.ToString();
                    string tag = TagUp(v);

                    val.Value = LocRm.GetString(tag, lc);
                }
                
            }
        }

        private string TagUp(string v)
        {
            return "json."+v.ToLower().Replace(" ", "").Replace("(", "").Replace(")", "").Replace("/", "");
        }

        dynamic PopulateResponse(string resp, object o, string lc)
        {
            dynamic d = JsonConvert.DeserializeObject(resp);
            if (d.header != null)
            {
                string v = d.header.Value.ToString();
                string t = TagUp(v);

                d.header.Value = LocRm.GetString(t, lc);                
            }
            foreach (var sec in d.sections)
            {
                if (sec.header != null)
                {
                    string v = sec.header.Value.ToString();
                    string t = TagUp(v);

                    sec.header.Value = LocRm.GetString(t, lc);
                }
                if (sec.text != null)
                {
                    var sec2 = sec;
                    Translate(ref sec2, lc);
                }
                if (sec.items != null)
                {
                    foreach (var item in sec.items)
                    {
                        var item2 = item;
                        Translate(ref item2, lc);
                        
                        var bt = item["bindto"];
                        if (bt != null && o!=null)
                        {
                            string[] prop = bt.ToString().Split(',');
                            if (prop.Length == 1)
                            {
                                try
                                {

                                    item["value"] = GetPropValue(o, bt.ToString());
                                    var nv = item["nvident"];
                                    if (nv != null)
                                    {
                                        item["value"] = NV(item["value"].ToString(), nv.ToString());
                                        if (item["value"] != "")
                                        {
                                            if (item["type"] == "Boolean")
                                                item["value"] = Convert.ToBoolean(item["value"]);
                                            if (item["type"] == "Int32")
                                                item["value"] = Convert.ToInt32(item["value"]);
                                            if (item["type"] == "Decimal" || item["type"] == "Single")
                                                item["value"] = Convert.ToDecimal(item["value"]);
                                            if (item["type"] == "Select")
                                                item["value"] = Convert.ToString(item["value"]);
                                        }
                                    }
                                    var conv = item["converter"];
                                    if (conv != null)
                                    {
                                        switch ((string) conv)
                                        {
                                            case "daysofweek":
                                                string[] days = item["value"].ToString().Trim(',').Split(',');
                                                int i = 0;
                                                foreach (var opt in item.options)
                                                {
                                                    if (days.Contains(i.ToString(CultureInfo.InvariantCulture)))
                                                    {
                                                        opt["value"] = true;
                                                    }
                                                    i++;
                                                }
                                                break;
                                            case "datetimetoint":
                                                var dt = (DateTime) item["value"];
                                                item["value"] = dt.TimeOfDay.TotalMinutes;
                                                break;
                                            case "fonttofontsize":
                                                var f = FontXmlConverter.ConvertToFont((string) item["value"]);
                                                item["value"] = f.Size;
                                                break;
                                            case "rgbtohex":
                                                var rgb = (string) item["value"].ToString();
                                                var rgbarr = rgb.Split(',');
                                                if (rgbarr.Length == 3)
                                                {
                                                    item["value"] = "#" + (Convert.ToInt16(rgbarr[0])).ToString("X2") +
                                                                    (Convert.ToInt16(rgbarr[1])).ToString("X2") +
                                                                    (Convert.ToInt16(rgbarr[2])).ToString("X2");
                                                }

                                                break;
                                        }
                                    }
                                }
                                catch
                                {
                                    item["value"] = "Error";
                                }
                            }
                            else
                            {
                                string json = prop.Aggregate("[", (current, s) => current + (GetPropValue(o, s) + ","));
                                json = json.Trim(',');
                                json += "]";
                                item["value"] = JToken.Parse(json);
                            }
                        }
                        if (item.options != null)
                        {
                            foreach (var opt in item.options)
                            {
                                var opt2 = opt;
                                Translate(ref opt2, lc);
                            }
                        }
                    }
                }
            }
            return d;
        }

        void PopulateObject(dynamic d, object o)
        {
            foreach (var sec in d.sections)
            {
                foreach (var item in sec.items)
                {
                    var bt = item["bindto"];
                    if (bt != null)
                    {
                        var val = item["value"];
                        if (val != null)
                        {
                            Populate(item, o);
                        }
                    }
                }
            }
        }

        void Populate(dynamic item, object o)
        {
            var bt = item["bindto"];
            var val = item["value"];
            var conv = item["converter"];
            var nvident = item["nvident"];

            if (conv != null)
            {
                switch ((string) conv)
                {
                    case "daysofweek":
                        string dow = "";
                        int i = 0;
                        foreach (var opt in item.options)
                        {
                            if (opt.value == true)
                            {
                                dow += i.ToString(CultureInfo.InvariantCulture) + ",";
                            }
                            i++;
                        }
                        dow = dow.Trim(',');
                        val = dow;
                        break;
                    case "datetimetoint":
                        TimeSpan ts = TimeSpan.FromMinutes(Convert.ToInt64(val));
                        val = DateTime.MinValue.Add(ts);
                        break;
                    case "fonttofontsize":
                        var oc = (objectsCamera) o;
                        var font = oc.settings.timestampfont;
                        var f = FontXmlConverter.ConvertToFont(font);
                        var sz = (float) Convert.ToDecimal(item["value"]);
                        var f2 = new Font(f.Name, sz, f.Style, f.Unit, f.GdiCharSet, f.GdiVerticalFont);
                        oc.settings.timestampfont = FontXmlConverter.ConvertToString(f2);
                        return;
                    case "rgbtohex":
                        try
                        {
                            var col = ColorTranslator.FromHtml((string) item["value"].ToString());
                            val = col.ToRGBString();
                        }
                        catch (Exception) { }
                        break;
                }
            }

            var props = bt.ToString().Split(',');
            if (props.Length > 1)
            {
                int i = 0;
                if (val.Type.ToString() == "String")
                {
                    val = val.ToString().Split(',');
                }
                foreach (string s in props)
                {
                    try
                    {
                        SetPropValue(o, s, val[i]);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex, "Conversion from web setting");
                    }
                    i++;
                }
            }
            else
            {
                if (nvident != null)
                {
                    var nv = nvident.ToString();
                    var nvstring = GetPropValue(o, props[0]).ToString();
                    val = NVSet(nvstring, nv, val.ToString());
                }
                try
                {
                    SetPropValue(o, props[0], val);
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex, "Conversion from web setting");
                }
            }
        }

        static object GetPropValue(object src, string propName)
        {
            object currentObject = src;
            string[] fieldNames = propName.Split('.');

            foreach (string fieldName in fieldNames)
            {
                // Get type of current record 
                Type curentRecordType = currentObject.GetType();
                PropertyInfo property = curentRecordType.GetProperty(fieldName);

                if (property != null)
                {
                    currentObject = property.GetValue(currentObject, null);
                }
                else
                {
                    return null;
                }
            }
            return currentObject;
        }
        static void SetPropValue(object src, string propName, object propValue)
        {
            object currentObject = src;
            string[] fieldNames = propName.Split('.');

            for (int i = 0; i < fieldNames.Length - 1; i++)
            {
                string fieldName = fieldNames[i];
                currentObject = currentObject.GetType().GetProperty(fieldName).GetValue(currentObject,null);
            }
            var val = currentObject.GetType().GetProperty(fieldNames[fieldNames.Length - 1]);
            var t = val.PropertyType.Name;
            switch (t)
            {
                case "String":
                    val.SetValue(currentObject, propValue.ToString(), null);
                    break;
                case "Int32":
                    val.SetValue(currentObject, Convert.ToInt32(propValue, CultureInfo.InvariantCulture), null);
                    break;
                case "Decimal":
                    val.SetValue(currentObject, Convert.ToDecimal(propValue, CultureInfo.InvariantCulture), null);
                    break;
                case "Single":
                    val.SetValue(currentObject, Convert.ToSingle(propValue, CultureInfo.InvariantCulture), null);
                    break;
                case "Double":
                    val.SetValue(currentObject, Convert.ToDouble(propValue, CultureInfo.InvariantCulture), null);
                    break;
                case "Boolean":
                    val.SetValue(currentObject, Convert.ToBoolean(propValue, CultureInfo.InvariantCulture), null);
                    break;
                case "DateTime":
                    val.SetValue(currentObject, Convert.ToDateTime(propValue, CultureInfo.InvariantCulture), null);
                    break;
                default:
                    throw new Exception("missing conversion (" + t + ")");
            }

        }

        string GetFileOptionsList(string relPath, string filter)
        {
            var di = new DirectoryInfo(Program.AppPath + relPath);
            string m = "";
            var fi = di.GetFiles(filter);
            var t = "{{\"text\":\"{0}\",\"value\":\"{1}\", \"noTranslate\":true}},";
            m += string.Format(t, "None", "");
            m = fi.Aggregate(m, (current, f) => current + string.Format(t, f.Name, f.Name));
            return m.Trim(',');
        }
    }
}
