using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using iSpyApplication.Utilities;

namespace iSpyApplication
{
    public partial class FindCameras : Form
    {
        private static bool _vlc;
        private static DataTable _dt;
        private DataRow _drSelected;
        private bool _exiting;

        public int VideoSourceType;
        public int AudioSourceType = -1;
        public string AudioUrl = "";
        public int Ptzid = -1;
        public int Ptzentryid = 0;
        private const int MaxThreads = 10;
        public string FinalUrl = "";
        public string Username = "";
        public string Channel = "";
        public string Password = "";
        public string AudioModel = "";
        public string Flags = "";
        public string Cookies = "";
        public static List<String> DnsEntries = new List<string>();
        private Thread _urlscanner;

        public FindCameras()
        {
            InitializeComponent();
            RenderResources();
        }

        private class UISync
        {
            private static ISynchronizeInvoke _sync;

            public static void Init(ISynchronizeInvoke sync)
            {
                _sync = sync;
            }

            public static void Execute(Action action)
            {
                try { _sync.BeginInvoke(action, null); }
                catch { }
            }
        }

        private void FindCameras_Load(object sender, EventArgs e)
        {
            LastConfig.PromptSave = false;
            _vlc = VlcHelper.VlcInstalled;
            llblDownloadVLC.Text = LocRm.GetString("DownloadVLC");
            llblDownloadVLC.Visible = !_vlc;
            btnBack.Enabled = false;
            RenderResources();
            ddlHost.Items.Add(LocRm.GetString("AllAdaptors"));
            foreach (var ip in MainForm.AddressListIPv4)
            {
                string subnet = ip.ToString();
                subnet = subnet.Substring(0, subnet.LastIndexOf(".", StringComparison.Ordinal) + 1) + "x";
                if (!ddlHost.Items.Contains(subnet))
                    ddlHost.Items.Add(subnet);
            }
            ddlHost.SelectedIndex = 0;

            txtPorts.Text = MainForm.IPPORTS;
            txtMake.Text = MainForm.IPTYPE;
            txtModel.Text = MainForm.IPMODEL;


            if (MainForm.IPTABLE != null)
            {
                _dt = MainForm.IPTABLE.Copy();
                dataGridView1.DataSource = _dt;
                dataGridView1.Invalidate();
            }

            txtUsername.Text = MainForm.IPUN;
            txtPassword.Text = MainForm.IPPASS;
            txtIPAddress.Text = MainForm.IPADDR;
            txtChannel.Text = MainForm.IPCHANNEL;
            numPort.Value = MainForm.IPPORT;
            rdoListed.Checked = MainForm.IPLISTED;
            rdoUnlisted.Checked = !MainForm.IPLISTED;
            chkRTSP.Checked = MainForm.IPRTSP;
            chkHTTP.Checked = MainForm.IPHTTP;

            UISync.Init(this);
            LoadSources();
            ShowPanel(pnlConfig);           
        }

        void ShowPanel(Control p)
        {
            llblScan.Visible = false;
            pnlConfig.Dock = DockStyle.None;
            pnlConfig.Visible = false;
            pnlLogin.Dock = DockStyle.None;
            pnlLogin.Visible = false;
            pnlFindNetwork.Dock = DockStyle.None;
            pnlFindNetwork.Visible = false;
            pnlConnect.Dock = DockStyle.None;
            pnlConnect.Visible = false;
            llblFilter.Visible = false;

            p.Dock = DockStyle.Fill;
            p.Visible = true;

            btnBack.Enabled = p.Name != "pnlConfig";

            if (p.Name == "pnlConnect")
            {
                llblScan.Visible = MainForm.IPLISTED;
                llblFilter.Visible = true;
            }
        }

        private void RenderResources()
        {
            Text = LocRm.GetString("ConnectCamera");
            button1.Text = LocRm.GetString("ScanLocalNetwork");
            label4.Text = LocRm.GetString("IPAddress");
            label2.Text = LocRm.GetString("Username");
            label3.Text = LocRm.GetString("Password");
            label1.Text = label13.Text = LocRm.GetString("Manufacturer");
            //label6.Text = LocRm.GetString("Port");
            label5.Text = LocRm.GetString("ScanInstructions");
            btnBack.Text = LocRm.GetString("Back");
            btnNext.Text = LocRm.GetString("Next");
            label8.Text = LocRm.GetString("Adaptor");

            linkLabel1.Text = LocRm.GetString("GetLatestList");

            rdoListed.Text = LocRm.GetString("Listed");
            rdoUnlisted.Text = LocRm.GetString("NotListed");
            label11.Text = label14.Text = LocRm.GetString("Model");

            chkRTSP.Text = LocRm.GetString("ScanRTSPAddresses");
            chkHTTP.Text = LocRm.GetString("ScanHTTPAddresses");

            LocRm.SetString(label15,"EnterUsernamePassword");
            LocRm.SetString(label12, "Channel");

            LocRm.SetString(label10, "Port");
            LocRm.SetString(label7, "OrFindYourDevice");
            LocRm.SetString(label8, "Adaptor");
            LocRm.SetString(label6, "PortsHTTPOnly");
            LocRm.SetString(label5, "ClickScan");
            LocRm.SetString(label9, "TryTheseURLs");
            LocRm.SetString(llblFilter, "CheckAndFilterResults");
            LocRm.SetString(llblScan, "ScanCameraForMore");
            LocRm.SetString(llblDownloadVLC, "DownloadVLC");
            tsddScanner.Text = LocRm.GetString("Scanner");

        }

        private void PortScannerManager(string host)
        {
            var ports = new List<int>();

            foreach (string s in txtPorts.Text.Split(','))
            {
                int p;
                if (int.TryParse(s, out p))
                {
                    if (p < 65535 && p > 0)
                        ports.Add(p);
                }
            }
            UISync.Execute(() => pbScanner.Value = 0);

            var manualEvents = new ManualResetEvent[MaxThreads];
            int j;
            for (int k = 0; k < MaxThreads; k++)
            {
                manualEvents[k] = new ManualResetEvent(true);
            }

            var ipranges = new List<string>();
            if (host == LocRm.GetString("AllAdaptors"))
            {
                ipranges.AddRange(from string s in ddlHost.Items where s != LocRm.GetString("AllAdaptors") select s);
            }
            else
            {
                ipranges.Add(host);
            }

            UISync.Execute(() => pbScanner.Maximum = ipranges.Count * 254);
            MainForm.LogMessageToFile("Scanning LAN");
            j = 0;
            foreach (string IP in DnsEntries)
            {
                string ip = IP;
                int k = j;
                var scanner = new Thread(p => PortScanner(ports, ip, manualEvents[k]));
                scanner.Start();

                j = WaitHandle.WaitAny(manualEvents);
                UISync.Execute(() => pbScanner.PerformStep());
                if (_exiting)
                    break;
            }

            if (!_exiting)
            {
                j = 0;
                foreach (string shost in ipranges)
                {
                    for (int i = 0; i < 255; i++)
                    {

                        string ip = shost.Replace("x", i.ToString(CultureInfo.InvariantCulture));
                        if (!DnsEntries.Contains(ip))
                        {
                            int k = j;
                            manualEvents[k].Reset();
                            var scanner = new Thread(p => PortScanner(ports, ip, manualEvents[k]));
                            scanner.Start();

                            j = WaitHandle.WaitAny(manualEvents);
                            UISync.Execute(() => pbScanner.PerformStep());
                        }
                        if (_exiting)
                            break;
                    }
                    if (_exiting)
                        break;
                }
            }

            if (j > 0)
                WaitHandle.WaitAll(manualEvents);


            //populate MAC addresses
            try
            {
                var arpStream = ExecuteCommandLine("arp", "-a");
                // Consume first three lines
                for (int i = 0; i < 3; i++)
                {
                    arpStream.ReadLine();
                }
                // Read entries
                while (!arpStream.EndOfStream)
                {
                    var line = arpStream.ReadLine();
                    if (line != null)
                    {
                        line = line.Trim();
                        while (line.Contains("  "))
                        {
                            line = line.Replace("  ", " ");
                        }
                        var parts = line.Trim().Split(' ');

                        if (parts.Length == 3)
                        {
                            for (int i = 0; i < _dt.Rows.Count; i++)
                            {
                                DataRow dr = _dt.Rows[i];
                                string ip = parts[0];
                                if (ip == dr["IP Address"].ToString().Split(':')[0])
                                {
                                    dr["MAC Address"] = parts[1];
                                }
                            }
                        }
                    }
                }
                _dt.AcceptChanges();
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
            }
            
            UISync.Execute(ResetControls);
        }

        private void ResetControls()
        {
            dataGridView1.Refresh();
            button1.Text = LocRm.GetString("ScanLocalNetwork");
            pbScanner.Value = 0;
            button1.Enabled = true;
        }
        
        public static StreamReader ExecuteCommandLine(String file, String arguments = "")
        {
            var startInfo = new ProcessStartInfo
                                {
                                    CreateNoWindow = true,
                                    WindowStyle = ProcessWindowStyle.Hidden,
                                    UseShellExecute = false,
                                    RedirectStandardOutput = true,
                                    FileName = file,
                                    Arguments = arguments
                                };

            Process process = Process.Start(startInfo);

            return process.StandardOutput;
        }


        private void PortScanner(IEnumerable<int> ports, string ipaddress, ManualResetEvent mre)
        {
            bool found;
            if (!DnsEntries.Contains(ipaddress))
            {
                const string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] buffer = Encoding.ASCII.GetBytes(data);

                var netMon = new Ping();
                var options = new PingOptions(128, true);
                PingReply pr = netMon.Send(ipaddress, 3000, buffer, options);
                found = pr != null && pr.Status == IPStatus.Success;
            }
            else
            {
                found = true;
            }
            if (found)
            {
                MainForm.LogMessageToFile("Ping response from " + ipaddress);
                string hostname = "Unknown";
                try
                {
                    var ipToDomainName = Dns.GetHostEntry(ipaddress);
                    hostname = ipToDomainName.HostName;
                }
                catch
                {
                }
                
                foreach (int iport in ports)
                {
                    try
                    {
                        string req = ipaddress + ":" + iport;
                        var request = (HttpWebRequest)WebRequest.Create("http://" + req);
                        request.Referer = "";
                        request.Timeout = 3000;
                        request.UserAgent = "Mozilla/5.0";
                        request.AllowAutoRedirect = false;

                        HttpWebResponse response = null;

                        try
                        {
                            response = (HttpWebResponse)request.GetResponse();
                        }
                        catch (WebException e)
                        {
                            response = (HttpWebResponse)e.Response;
                        }
                        catch (Exception ex)
                        {
                            MainForm.LogMessageToFile("Web error from " + ipaddress + ":" + iport + " " + ex.Message);
                        }
                        if (response != null)
                        {
                            MainForm.LogMessageToFile("Web response from " + ipaddress + ":" + iport + " " +
                                                      response.StatusCode);
                            if (response.Headers != null)
                            {
                                string webserver = "yes";
                                foreach (string k in response.Headers.AllKeys)
                                {
                                    if (k.ToLower().Trim() == "server")
                                        webserver = response.Headers[k];
                                }
                                lock (_dt)
                                {
                                    DataRow dr = _dt.NewRow();
                                    dr[0] = ipaddress;
                                    dr[1] = iport;
                                    dr[2] = hostname;
                                    dr[3] = webserver;
                                    _dt.Rows.Add(dr);
                                    _dt.AcceptChanges();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MainForm.LogMessageToFile("Web error from " + ipaddress + ":" + iport + " " + ex.Message);

                    }
                }
                UISync.Execute(() => dataGridView1.Refresh());
            }
            mre.Set();
        }

        private void dataGridView1_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == LocRm.GetString("ScanLocalNetwork"))
            {
                button1.Text = LocRm.GetString("Cancel");
                Application.DoEvents();
                ScanNetwork();
            }
            else
            {
                _exiting = true;
                button1.Text = LocRm.GetString("ScanLocalNetwork");
            }

        }


        private void ScanNetwork()
        {
            _exiting = false;
            
            _dt = new DataTable("Network");

            _dt.Columns.Add(new DataColumn("IP Address"));
            _dt.Columns.Add(new DataColumn("Port"));
            _dt.Columns.Add(new DataColumn("Device Name"));
            _dt.Columns.Add(new DataColumn("WebServer"));
            _dt.Columns.Add(new DataColumn("MAC Address"));
            _dt.AcceptChanges();
            dataGridView1.DataSource = _dt;
            string host = ddlHost.SelectedItem.ToString();

            var nb = new NetworkBrowser();

            DnsEntries.Clear();
            try
            {
                foreach (string s1 in nb.GetNetworkComputers())
                {
                    var ipEntry = Dns.GetHostEntry(s1.Trim('\\'));
                    var addr = ipEntry.AddressList.Where(p => p.AddressFamily == AddressFamily.InterNetwork);
                    foreach (var t in addr)
                    {
                        DnsEntries.Add(t.ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
            }

            var manager = new Thread(p => PortScannerManager(host)) { Name = "Port Scanner", IsBackground = true, Priority = ThreadPriority.Normal };
            manager.Start();
        }

        private void LoadSources()
        {
            if (MainForm.Sources == null)
                return;
            ddlMake.Items.Clear();
            foreach (var m in MainForm.Sources)
            {
                ddlMake.Items.Add(m.name);
            }
            if (MainForm.IPTYPE != "")
            {
                try
                {
                    ddlMake.SelectedItem = MainForm.IPTYPE;
                    txtMake.Text = MainForm.IPTYPE;
                }
                catch
                {
                    //may have been removed
                }
            }
        }

        private void LoadModels()
        {
            ddlModel.Items.Clear();
            if (MainForm.Sources == null || ddlMake.SelectedIndex==-1)
                return;

            string make = ddlMake.SelectedItem.ToString();
            if (MainForm.IPLISTED)
            {
                txtMake.Text = make;
            }
            var m = MainForm.Sources.FirstOrDefault(p => p.name == make);

            string added = ",";
            ddlModel.Items.Add("Other");

            if (m != null)
            {
                var l = m.url.OrderBy(p => p.version).ToList();
                foreach (var u in l)
                {
                    if (!String.IsNullOrEmpty(u.version) &&
                        added.IndexOf("," + u.version.ToUpper() + ",", StringComparison.Ordinal) == -1)
                    {
                        ddlModel.Items.Add(u.version);
                        added += u.version.ToUpper() + ",";
                    }
                }
            }
            if (ddlModel.SelectedIndex == -1)
                ddlModel.SelectedIndex = 0;

            if (MainForm.IPMODEL != "")
            {
                try
                {
                    ddlModel.SelectedItem = MainForm.IPMODEL;
                }
                catch
                {
                    //may have been removed
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        }

        private void ddlMake_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadModels();
        }

        private void AddConnections()
        {
            pnlOptions.Controls.Clear();
            pnlOptions.AutoScroll = true;
            ShowPanel(pnlConnect);

            string make = txtMake.Text;
            string model = txtModel.Text;
            if (MainForm.IPLISTED)
            {
                llblScan.Visible = true;
                make = ddlMake.SelectedItem.ToString();
                if (ddlModel.SelectedIndex > 0)
                    model = ddlModel.SelectedItem.ToString().ToUpper();
            }
            else
                llblScan.Visible = false;

            
            if (MainForm.IPLISTED)
            {
                ListCameras(make, model);
                Application.DoEvents();
                if (pnlOptions.Controls.Count > 1)
                {
                    if (MessageBox.Show(this, LocRm.GetString("CheckURLValid"), LocRm.GetString("Confirm"), MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        CheckAndFilterResults();
                    }
                }
                else
                {
                    if (MessageBox.Show(this, LocRm.GetString("ScanForFeeds"), LocRm.GetString("Confirm"), MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        ScanCamera(make);
                    }
                }
            }
            else
            {
                ScanCamera(make);
            }
        }

        private void ScanCamera(string make)
        {
            if (_urlscanner != null && !_urlscanner.Join(TimeSpan.Zero))
            {
                QuitScanner();
            }
            _urlscanner = new Thread(() => ScanCameras(make));
            _urlscanner.Start();
            tsddScanner.Enabled = true;
        }

        private void QuitScanner()
        {
            _quiturlscanner = true;
            if (_urlscanner != null)
            {
                var i = 0;
                while (!_urlscanner.Join(TimeSpan.Zero) && i < 10)
                {
                    Thread.Sleep(200);
                    i++;
                }
                if (!_urlscanner.Join(TimeSpan.Zero))
                    _urlscanner.Abort();
                _urlscanner = null;
            }

            UISync.Execute(() => tsslCurrent.Text = LocRm.GetString("ScannerInactive"));
        }


        private volatile bool _quiturlscanner;

        private void ScanCameras(string make)
        {
            _quiturlscanner = false;
            //scan all possible urls
            

            var lp = new List<String>();
            string login =  Uri.EscapeDataString(txtUsername.Text);
            string password = Uri.EscapeDataString(txtPassword.Text);

            //list urls for current make first
            var m = MainForm.Sources.Where(p=>p.name.ToUpper()==make.ToUpper()).ToList();           
            ListCameras(m, ref lp, login, password);
            if (!_quiturlscanner)
            {
                m = MainForm.Sources.Where(p => p.name.ToUpper() != make.ToUpper()).ToList();
                ListCameras(m, ref lp, login, password);
            }

            int i = 0;
            while (i < pnlOptions.Controls.Count)
            {
                if (pnlOptions.Controls[i].Enabled)
                {
                    try
                    {
                        ((RadioButton) pnlOptions.Controls[i]).Checked = true;
                    }
                    catch
                    {
                    }
                    break;
                }
                i++;
            }

            UISync.Execute(() => tsslCurrent.Text = "OK");
            UISync.Execute(() => tsddScanner.Enabled = false);
        }

        private void ScanListedURLs()
        {
            _quiturlscanner = false;
            string login = Uri.EscapeDataString(txtUsername.Text);
            string password = Uri.EscapeDataString(txtPassword.Text);

            
            var mmurl = new List<ManufacturersManufacturerUrl>();
            int k = 0;
            for (; k < pnlOptions.Controls.Count; k++)
                mmurl.Add((ManufacturersManufacturerUrl)pnlOptions.Controls[k].Tag);

            k = 0;
            int j = 0;

            while (k < mmurl.Count)
            {
                var u = mmurl[k];

                string addr = GetAddr(u);
                switch (u.prefix.ToUpper())
                {
                    default:
                        UISync.Execute(() => tsslCurrent.Text = "Trying: " + addr);
                        //test this url
                        if (!SendHTTPReq(addr, u.cookies, login, password))
                        {
                            int j1 = j;
                            UISync.Execute(() => pnlOptions.Controls.RemoveAt(j1));
                            j--;
                        }
                        break;
                    case "RTSP://":
                        addr = GetAddr(u);
                        UISync.Execute(() => tsslCurrent.Text = "Trying: " + addr);
                        if (!SendRTSPReq(addr, login, password))
                        {
                            int j1 = j;
                            UISync.Execute(() => pnlOptions.Controls.RemoveAt(j1));
                            j--;
                        }
                        break;

                }
                j++;
                k++;
                if (_quiturlscanner)
                    break;
            }
            
            int i = 0;
            while (i < pnlOptions.Controls.Count)
            {
                if (pnlOptions.Controls[i].Enabled)
                {
                    try
                    {
                        ((RadioButton)pnlOptions.Controls[i]).Checked = true;
                    }
                    catch
                    {
                    }
                    break;
                }
                i++;
            }

            UISync.Execute(() => tsslCurrent.Text = "OK");
            UISync.Execute(() => tsddScanner.Enabled = false);

        }

        private void ListCameras(IEnumerable<ManufacturersManufacturer> m, ref List<string> lp, string login, string password)
        {
            foreach(var s in m)
            {
                
                var cand = s.url.ToList();
                cand = cand.OrderBy(p => p.Source).ToList();

                foreach (var u in cand)
                {
                    string addr;
                    switch (u.prefix.ToUpper())
                    {
                        default:
                            if (MainForm.IPHTTP)
                            {
                                addr = GetAddr(u);
                                if (!lp.Contains(addr))
                                {
                                    lp.Add(addr);
                                    UISync.Execute(() => tsslCurrent.Text =  addr);
                                    //test this url
                                    if (SendHTTPReq(addr, u.cookies, login, password))
                                    {
                                        AddCamera(addr, s, u);
                                    }
                                }
                            }
                            break;
                        case "RTSP://":
                            if (MainForm.IPRTSP)
                            {
                                addr = GetAddr(u);
                                if (!lp.Contains(addr))
                                {
                                    lp.Add(addr);
                                    UISync.Execute(() => tsslCurrent.Text = addr);
                                    //test this url
                                    if (SendRTSPReq(addr, login, password))
                                    {
                                        AddCamera(addr, s, u);
                                    }
                                }
                            }
                            break;
                    }
                    if (_quiturlscanner)
                        break;
                }
                if (_quiturlscanner)
                    break;
            }
        }

        private void AddCamera(string addr, ManufacturersManufacturer m,  ManufacturersManufacturerUrl u)
        {
            string st = m.name+":";
            if (!String.IsNullOrEmpty(u.version))
                st += u.version;
            else
                st += "Other";
            string source = u.Source;
            if (source == "VLC" && !_vlc)
                source = "FFMPEG";
            st += ": " + source +" " + addr.Replace("&", "&&");

            var rb = new RadioButton { Text = st, AutoSize = true, Tag = u };
            if (u.Source == "FFMPEG" || u.Source == "VLC")
                rb.Font = new Font(rb.Font, FontStyle.Bold);

            UISync.Execute(() => pnlOptions.Controls.Add(rb));
 
        }

        private static bool SendHTTPReq(string source, string cookies, string login, string password)
        {
            bool b = false;
            HttpStatusCode sc = 0;

            HttpWebRequest req;
            var res = ConnectionFactory.GetResponse(source, cookies, login, password, false, out req);
            if (res != null)
            {
                sc = res.StatusCode;
                if (sc == HttpStatusCode.OK)
                {
                    string ct = res.ContentType.ToLower();
                    if (ct.IndexOf("text", StringComparison.Ordinal) == -1)
                    {
                        b = true;
                    }
                }
                res.Close();
            }

            MainForm.LogMessageToFile("Status " + sc + " at " + source, "Uri Checker");

            return b;
        }

        private static bool SendRTSPReq(string addr, string login, string password)
        {
            bool b = false;
            try
            {
                var uri = new Uri(addr);

                var request = "OPTIONS " + addr + " RTSP/1.0\r\n" +
                              "CSeq: 1\r\n" +
                              "User-Agent: iSpy\r\n" +
                              "Accept: */*\r\n";

                if (!String.IsNullOrEmpty(login))
                {
                    var authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(login + ":" + password));
                    request +="Authorization: Basic " + authInfo+"\r\n";
                }

                request += "\r\n";

                IPAddress host = IPAddress.Parse(uri.DnsSafeHost);
                var hostep = new IPEndPoint(host, uri.Port);

                var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                               {ReceiveTimeout = 2000};
                sock.Connect(hostep);

                var response = sock.Send(Encoding.UTF8.GetBytes(request));
                if (response > 0)
                {
                    var bytesReceived = new byte[200];
                    var bytes = sock.Receive(bytesReceived, bytesReceived.Length, 0);
                    string resp = Encoding.ASCII.GetString(bytesReceived, 0, bytes);
                    if (resp.IndexOf("200 OK", StringComparison.Ordinal) != -1)
                    {
                        b = true;
                    }
                    MainForm.LogMessageToFile("RTSP attempt: " + resp + " at " + addr);
                }
                sock.Close();
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
            }
            return b;
        }

        private void ListCameras(string make, string model)  {

            var m = MainForm.Sources.FirstOrDefault(p => p.name == make);
            if (m == null)
            {
                MessageBox.Show(this, LocRm.GetString("NoSourcesAvailable"));
                ShowPanel(pnlConfig);
                return;
            }
                      
            var cand = m.url.ToList();
            var added = new List<string>();
            if (model!="" && model.ToUpper()!="OTHER")
            {
                string mdl = model.ToUpper();
                cand = cand.Where(p => String.IsNullOrEmpty(p.version) || p.version.ToUpper() == mdl).ToList();
            }
            cand = cand.OrderBy(p => p.Source).ToList();

            pnlOptions.SuspendLayout();
            foreach (var u in cand)
            {
                string addr = GetAddr(u);
                if (!added.Contains(addr))
                {
                    AddCamera(addr, m, u);
                    added.Add(addr);
                }
            }
            pnlOptions.ResumeLayout();

            int i = 0;
            while (i < pnlOptions.Controls.Count)
            {
                if (pnlOptions.Controls[i].Enabled)
                {
                    ((RadioButton)pnlOptions.Controls[i]).Checked = true;
                    break;
                }
                i++;
            }
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.RowIndex < _dt.Rows.Count)
            {
                _drSelected = _dt.Rows[e.RowIndex];
                txtIPAddress.Text = _drSelected[0].ToString();
                numPort.Value = Convert.ToInt32(_drSelected[1]);
                if (_drSelected[2].ToString() == "iSpyServer")
                    ddlMake.SelectedItem = "iSpy Camera Server";
            }
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtIPAddress_KeyUp(object sender, KeyEventArgs e)
        {
            btnBack.Enabled = ddlMake.SelectedIndex > -1 && txtIPAddress.Text.Trim() != "";
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var d = new downloader
                        {
                            Url = MainForm.Website + "/getcontent.aspx?name=sources2",
                            SaveLocation = Program.AppDataPath + @"XML\Sources.xml"
                        };
            d.ShowDialog(this);
            if (d.DialogResult==DialogResult.OK)
            {
                MainForm.Sources = null;
                LoadSources();
            }
            d.Dispose();
        }

        private void ddlHost_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void FindCameras_FormClosing(object sender, FormClosingEventArgs e)
        {
            _exiting = true;
            QuitScanner();
            Application.DoEvents();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            Next();
        }

        private void Next()
        {
            if (pnlConfig.Visible)
            {
                MainForm.IPLISTED = tbl1.Enabled;
                MainForm.IPRTSP = chkRTSP.Checked;
                MainForm.IPHTTP = chkHTTP.Checked;
                if (MainForm.IPLISTED)
                {
                    if (ddlMake.SelectedIndex == -1)
                    {
                        MessageBox.Show(this, LocRm.GetString("ChooseMake"));
                        return;
                    }
                }
                else
                {
                    if (!MainForm.IPRTSP && !MainForm.IPHTTP)
                    {
                        MessageBox.Show(this, LocRm.GetString("ChooseOption"));
                        return;
                    }
                }
                ShowPanel(pnlLogin);
                return;
            }
            if (pnlLogin.Visible)
            {
                ShowPanel(pnlFindNetwork);
                return;
            }
            if (pnlFindNetwork.Visible)
            {
                string addr = txtIPAddress.Text.Trim();
                if (String.IsNullOrEmpty(addr))
                {
                    MessageBox.Show(this, LocRm.GetString("EnterIPAddress"));
                    return;
                }
                Uri nUrl = null;
                if (!Uri.TryCreate("http://"+addr, UriKind.Absolute, out nUrl))
                {
                    MessageBox.Show(this, LocRm.GetString("EnterIPDNSOnly"));
                    return;
                }

                AddConnections();
                return;
            }
            if (pnlConnect.Visible)
            {
                if (MainForm.IPLISTED && ddlMake.SelectedIndex == 0)
                {
                    ShowPanel(pnlConfig);
                    return;
                }

                string make = txtMake.Text;
                string model = txtModel.Text;
                
                if (MainForm.IPLISTED)
                {
                    make = ddlMake.SelectedItem.ToString();
                    if (ddlModel.SelectedIndex > 0)
                        model = ddlModel.SelectedItem.ToString().ToUpper();
                }
              

                ManufacturersManufacturerUrl s = null;
                for (int j = 0; j < pnlOptions.Controls.Count; j++)
                {
                    if (pnlOptions.Controls[j] is RadioButton)
                    {
                        if (((RadioButton)pnlOptions.Controls[j]).Checked)
                        {
                            s = (ManufacturersManufacturerUrl) (pnlOptions.Controls[j]).Tag;
                            break;
                        }
                    }
                }
                if (s == null)
                {
                    MessageBox.Show(this, LocRm.GetString("SelectURL"));
                    return;
                }

                FinalUrl = GetAddr(s);

                string source = s.Source;
                if (source == "VLC" && !_vlc)
                    source = "FFMPEG";

                switch (source)
                {
                    case "JPEG":
                        VideoSourceType = 0;
                        break;
                    case "MJPEG":
                        VideoSourceType = 1;
                        break;
                    case "FFMPEG":
                        VideoSourceType = 2;
                        break;
                    case "VLC":
                        VideoSourceType = 5;
                        break;
                }
                AudioSourceType = -1;
                if (!String.IsNullOrEmpty(s.AudioSource))
                {
                    switch (s.AudioSource.ToUpper())
                    {
                        case "FFMPEG":
                            AudioSourceType = 3;
                            break;
                        case "VLC":
                            AudioSourceType = 2;
                            if (!_vlc)
                                AudioSourceType = 3;
                            break;
                        case "WAVSTREAM":
                            AudioSourceType = 6;
                            break;
                    }
                    AudioUrl = GetAddr(s, true);
                }

                Ptzid = -1;

                if (!s.@fixed)
                {
                    string modellc = model.ToLower();
                    string n = make.ToLower();
                    bool quit = false;
                    foreach(var ptz in MainForm.PTZs)
                    {
                        int j = 0;
                        foreach(var m in ptz.Makes)
                        {
                            if (m.Name.ToLower() == n)
                            {
                                Ptzid = ptz.id;
                                Ptzentryid = j;
                                string mdl = m.Model.ToLower();
                                if (mdl == modellc || s.version.ToLower() == mdl)
                                {
                                    Ptzid = ptz.id;
                                    Ptzentryid = j;
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

                MainForm.IPUN = txtUsername.Text;
                MainForm.IPPASS = txtPassword.Text;
                MainForm.IPTYPE = make;
                MainForm.IPMODEL = model;
                MainForm.IPADDR = txtIPAddress.Text;
                MainForm.IPPORTS = txtPorts.Text;
                MainForm.IPPORT = (int)numPort.Value;
                MainForm.IPCHANNEL = txtChannel.Text.Trim();

                AudioModel = s.AudioModel;

                LastConfig.PromptSave = !MainForm.IPLISTED && MainForm.IPMODEL.Trim() != "";
                
                LastConfig.Iptype = MainForm.IPTYPE;
                LastConfig.Ipmodel = MainForm.IPMODEL;
                LastConfig.Prefix = s.prefix;
                LastConfig.Source = s.Source;
                LastConfig.URL = s.url;
                LastConfig.Cookies = s.cookies;
                LastConfig.Flags = s.flags;
                if (!String.IsNullOrEmpty(s.port))
                    LastConfig.Port = Convert.ToInt32(s.port);

                if (_dt != null)
                    MainForm.IPTABLE = _dt.Copy();
                DialogResult = DialogResult.OK;
                Close();
            }
        }


        public class LastConfig
        {
            public static bool PromptSave = false;
            public static string Iptype;
            public static string Ipmodel;
            public static string Prefix;
            public static string Source;
            public static string URL;
            public static string Cookies;
            public static string Flags;
            public static int Port;
        }

        

        private string GetAddr(ManufacturersManufacturerUrl s, bool audio = false)
        {
            Username = txtUsername.Text;
            Password = txtPassword.Text;
            Channel = txtChannel.Text.Trim();

            string addr = txtIPAddress.Text.Trim();
            Flags = s.flags;
            Cookies = s.cookies;

            var nPort = (int)numPort.Value;

            if (!String.IsNullOrEmpty(s.port))
                nPort = Convert.ToInt32(s.port);
            
            string connectUrl = s.prefix;

            if (!String.IsNullOrEmpty(Username))
            {
                connectUrl += Uri.EscapeDataString(Username);

                if (!String.IsNullOrEmpty(Password))
                    connectUrl += ":" + Uri.EscapeDataString(Password);
                connectUrl += "@";
                     
            }
            connectUrl += addr + ":" + nPort;

            string url = !audio?s.url:s.AudioURL;
            if (!url.StartsWith("/"))
                url = "/" + url;
            
            url = url.Replace("[USERNAME]", Uri.EscapeDataString(Username)).Replace("[PASSWORD]", Uri.EscapeDataString(Password));
            url = url.Replace("[CHANNEL]", txtChannel.Text.Trim());
            //defaults:
            url = url.Replace("[WIDTH]", "320");
            url = url.Replace("[HEIGHT]", "240");

            if (url.IndexOf("[AUTH]", StringComparison.Ordinal)!=-1)
            {
                string credentials = String.Format("{0}:{1}", Uri.EscapeDataString(Username), Uri.EscapeDataString(Password));
                byte[] bytes = Encoding.ASCII.GetBytes(credentials);
                url = url.Replace("[AUTH]", Convert.ToBase64String(bytes));
            }
                
            connectUrl += url;
            return connectUrl;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (pnlConnect.Visible)
            {
                QuitScanner();
                ShowPanel(pnlFindNetwork);
                return;
            }
            if (pnlFindNetwork.Visible)
            {
                ShowPanel(pnlLogin);
                return;
            }
            if (pnlLogin.Visible)
            {
                ShowPanel(pnlConfig);
            }

        }

        private void llblDownloadVLC_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Program.Platform == "x64")
                MessageBox.Show(this, LocRm.GetString("InstallVLCx64").Replace("[DIR]",Environment.NewLine+Program.AppPath+"VLC64"+Environment.NewLine));
            else
                MessageBox.Show(this, LocRm.GetString("InstallVLCx86"));
            MainForm.OpenUrl(Program.Platform == "x64" ? MainForm.VLCx64 : MainForm.VLCx86);
        }

        private void ddlModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MainForm.IPLISTED)
            {
                if (ddlModel.SelectedIndex > -1)
                {
                    txtModel.Text = ddlModel.SelectedItem.ToString();
                }
            }
        }

        private void rdoListed_CheckedChanged(object sender, EventArgs e)
        {
            tbl1.Enabled = rdoListed.Checked;
            tbl2.Enabled = !tbl1.Enabled;
        }

        private void rdoUnlisted_CheckedChanged(object sender, EventArgs e)
        {
            tbl2.Enabled = rdoUnlisted.Checked;
            tbl1.Enabled = !tbl2.Enabled;
        }

        private void chkRTSP_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void llblScan_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainForm.IPLISTED = false;
            MainForm.IPRTSP = true;
            MainForm.IPHTTP = true;

            AddConnections();
        }

        private void quitScannerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuitScanner();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CheckAndFilterResults();
        }

        private void CheckAndFilterResults()
        {
            if (_urlscanner != null && !_urlscanner.Join(TimeSpan.Zero))
            {
                QuitScanner();
            }
            _urlscanner = new Thread(ScanListedURLs);
            _urlscanner.Start();
            tsddScanner.Enabled = true;
        }
    }
}
