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
using iSpyApplication.Controls;
using iSpyApplication.Onvif;
using iSpyApplication.Pelco;
using iSpyApplication.Server;
using iSpyApplication.Utilities;
using CameraScanner = iSpyApplication.CameraDiscovery.CameraScanner;

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
        public string tokenPath = "";
        public string tokenPost = "";
        public int tokenPort = 80;
        public static List<String> DnsEntries = new List<string>();

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
            pnlConfig.Dock = DockStyle.None;
            pnlConfig.Visible = false;
            pnlLogin.Dock = DockStyle.None;
            pnlLogin.Visible = false;
            pnlFindNetwork.Dock = DockStyle.None;
            pnlFindNetwork.Visible = false;
            pnlConnect.Dock = DockStyle.None;
            pnlConnect.Visible = false;

            p.Dock = DockStyle.Fill;
            p.Visible = true;

            btnBack.Enabled = p.Name != "pnlConfig";
        }

        private void RenderResources()
        {
            Text = LocRm.GetString("ConnectCamera");
            button1.Text = LocRm.GetString("ScanLocalNetwork");
            label4.Text = LocRm.GetString("IPAddress");
            label2.Text = LocRm.GetString("Username");
            label3.Text = LocRm.GetString("Password");
            label14.Text = LocRm.GetString("Model");
            label1.Text = label13.Text = LocRm.GetString("Make");
            //label6.Text = LocRm.GetString("Port");
            label5.Text = LocRm.GetString("ScanInstructions");
            btnBack.Text = LocRm.GetString("Back");
            btnNext.Text = LocRm.GetString("Next");
            label8.Text = LocRm.GetString("Adaptor");

            linkLabel1.Text = LocRm.GetString("GetLatestList");

            rdoListed.Text = LocRm.GetString("Listed");
            rdoUnlisted.Text = LocRm.GetString("NotListed");

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
            Logger.LogMessage("Scanning LAN");
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
                Logger.LogException(ex);
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
                Logger.LogMessage("Ping response from " + ipaddress);
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
                        request.AllowAutoRedirect = true;

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
                            Logger.LogMessage("Web error from " + ipaddress + ":" + iport + " " + ex.Message);
                        }
                        if (response != null)
                        {
                            Logger.LogMessage("Web response from " + ipaddress + ":" + response.ResponseUri.Port + " " +
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
                                    dr[1] = response.ResponseUri.Port;
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
                        Logger.LogMessage("Web error from " + ipaddress + ":" + iport + " " + ex.Message);

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
                Logger.LogException(ex);
            }

            var manager = new Thread(p => PortScannerManager(host)) { Name = "Port Scanner", IsBackground = true, Priority = ThreadPriority.Normal };
            manager.Start();
        }


        private HashSet<string> _hashdata;
        private void LoadSources()
        {
            var camDb = new List<AutoCompleteTextbox.TextEntry>();

            _hashdata = new HashSet<string>();

            foreach (var source in MainForm.Sources)
            {
                string name = source.name.Trim() + ": Unlisted";
                if (!_hashdata.Contains(name.ToUpper()))
                {
                    camDb.Add(new AutoCompleteTextbox.TextEntry(name));
                    _hashdata.Add(name.ToUpper());
                }

                foreach (var u in source.url)
                {
                    name = source.name.Trim();
                    if (!string.IsNullOrEmpty(u.version))
                        name += ": " + u.version.Trim();
                    if (!_hashdata.Contains(name.ToUpper()))
                    {
                        camDb.Add(new AutoCompleteTextbox.TextEntry(name));
                        _hashdata.Add(name.ToUpper());
                    }
                }
            }


            txtFindModel.AutoCompleteList = camDb;
            txtFindModel.MinTypedCharacters = 1;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        }

        private void ddlMake_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LoadModels();
        }
        private List<ConnectionOption> _devicescanResults = new List<ConnectionOption>();

        private CameraScanner _deviceScanner;
        public CameraScanner DeviceScanner
        {
            get
            {
                if (_deviceScanner == null)
                {
                    _deviceScanner = new CameraScanner();
                    _deviceScanner.URLFound += DeviceScannerURLFound;
                    _deviceScanner.URLScan += _deviceScanner_URLScan;
                    _deviceScanner.ScanComplete += _deviceScanner_ScanComplete;
                }
                return _deviceScanner;
            }
        }

        private void _deviceScanner_ScanComplete(object sender, EventArgs e)
        {
            UISync.Execute(() => tsslCurrent.Text = "finished");
        }

        private void _deviceScanner_URLScan(object sender, EventArgs e)
        {
            if (sender!=null)
                UISync.Execute(() => tsslCurrent.Text = sender.ToString());
        }

        private void AddConnections(Uri uri)
        {
            pnlOptions.Controls.Clear();
            tsslCurrent.Text = "Initialising...";
            pnlOptions.AutoScroll = true;
            ShowPanel(pnlConnect);

            string make = txtMake.Text;
            string model = txtModel.Text;
            if (MainForm.IPLISTED)
            {
                var mm = txtFindModel.Text.Split(':');

                make = mm[0].Trim();
                if (mm.Length > 1)
                    model = mm[1].Trim().ToUpper();
                
            }

            ManufacturersManufacturer m = null;

            if (!string.IsNullOrEmpty(make) && make.ToLowerInvariant() != "unlisted")
            {
                m = MainForm.Sources.FirstOrDefault(p => string.Equals(p.name, make, StringComparison.InvariantCultureIgnoreCase));
                make = m != null ? m.name : "";
            }

            _devicescanResults = new List<ConnectionOption>();
            DeviceScanner.Channel = 0;
            int.TryParse(txtChannel.Text, out DeviceScanner.Channel);
            DeviceScanner.Make = make;
            DeviceScanner.Model = model;
            DeviceScanner.Username = txtUsername.Text;
            DeviceScanner.Password = txtPassword.Text;
            DeviceScanner.Uri = uri;
            DeviceScanner.ScanCamera(m);

        }

        
        private void AddONVIF(string addr, ConnectionOption co)
        {
            string st = "ONVIF: "+addr;
            
            var rb = new RadioButton { Text = st, AutoSize = true, Tag = co };
            UISync.Execute(() => pnlOptions.Controls.Add(rb));

        }

        private void AddCamera(ConnectionOption e)
        {
            string source = e.Source;
            string st = source + ":";

            if (e.MmUrl == null)
            {
                //onvif
                st += ": " + e.URL.Replace("&", "&&");
            }
            else
            {
                if (!string.IsNullOrEmpty(e.MmUrl.version))
                    st += e.MmUrl.version;
                else
                    st += "Other";
                
                if (source == "VLC" && !_vlc)
                    source = "FFMPEG";
                st += ": " + e.URL.Replace("&", "&&");
            }

            var rb = new RadioButton { Text = st, AutoSize = true, Tag = e };
            if (source == "FFMPEG" || source == "VLC" || source=="ONVIF")
                rb.Font = new Font(rb.Font, FontStyle.Bold);

            UISync.Execute(() => pnlOptions.Controls.Add(rb));
 
        }

        void DeviceScannerURLFound(object sender, CameraDiscovery.ConnectionOptionEventArgs e)
        {
            _devicescanResults.Add(e.Co);
            AddCamera(e.Co);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.RowIndex < _dt.Rows.Count)
            {
                _drSelected = _dt.Rows[e.RowIndex];
                txtIPAddress.Text = _drSelected[0].ToString();
                numPort.Value = Convert.ToInt32(_drSelected[1]);
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
            //btnBack.Enabled = ddlMake.SelectedIndex > -1 && txtIPAddress.Text.Trim() != "";
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            var d = new downloader
                        {
                            Url = MainForm.ContentSource + "/getcontent.aspx?name=sources3",
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
            _scanner.Stop();
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
                    if (!_hashdata.Contains(txtFindModel.Text.ToUpper()))
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
                if (string.IsNullOrEmpty(addr))
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

                AddConnections(nUrl);
                return;
            }
            if (pnlConnect.Visible)
            {
                if (MainForm.IPLISTED && txtFindModel.Text == "")
                {
                    ShowPanel(pnlConfig);
                    return;
                }

                DeviceScanner.Stop();
                string make = txtMake.Text;
                string model = txtModel.Text;
                FinalUrl = "";
                Username = DeviceScanner.Username;
                Password = DeviceScanner.Password;

                if (MainForm.IPLISTED)
                {
                    var mm = txtFindModel.Text.Split(':');

                    make = mm[0].Trim();
                    model = "";
                    if (mm.Length > 1)
                        model = mm[1].Trim().ToUpper();
                }

                
                ConnectionOption s = null;
                for (int j = 0; j < pnlOptions.Controls.Count; j++)
                {
                    if (pnlOptions.Controls[j] is RadioButton)
                    {
                        if (((RadioButton)pnlOptions.Controls[j]).Checked)
                        {
                            var o = (pnlOptions.Controls[j]).Tag;
                            s = o as ConnectionOption;
                            if (s == null)
                                continue;
                            FinalUrl = s.URL;

                            VideoSourceType = s.VideoSourceTypeID;
                            AudioSourceType = s.AudioSourceTypeID;

                            Ptzid = -1;

                            if (s.MmUrl!=null && !s.MmUrl.@fixed)
                            {
                                string modellc = model.ToLower();
                                string n = make.ToLower();
                                bool quit = false;
                                foreach (var ptz in MainForm.PTZs)
                                {
                                    int k = 0;
                                    foreach (var m in ptz.Makes)
                                    {
                                        if (m.Name.ToLower() == n)
                                        {
                                            Ptzid = ptz.id;
                                            Ptzentryid = k;
                                            string mdl = m.Model.ToLower();
                                            if (mdl == modellc || s.MmUrl.version.ToLower() == mdl)
                                            {
                                                Ptzid = ptz.id;
                                                Ptzentryid = k;
                                                quit = true;
                                                break;
                                            }
                                        }
                                        k++;
                                    }
                                    if (quit)
                                        break;
                                }
                            }
                            
                            break;
                        }
                    }
                }

                if (string.IsNullOrEmpty(FinalUrl))
                {
                    MessageBox.Show(this, LocRm.GetString("SelectURL"));
                    return;
                }


                MainForm.IPUN = txtUsername.Text;
                MainForm.IPPASS = txtPassword.Text;
                MainForm.IPTYPE = make;
                MainForm.IPMODEL = model;
                MainForm.IPADDR = txtIPAddress.Text;
                MainForm.IPPORTS = txtPorts.Text;
                MainForm.IPPORT = (int)numPort.Value;
                MainForm.IPCHANNEL = txtChannel.Text.Trim();


                LastConfig.PromptSave = !MainForm.IPLISTED && MainForm.IPMODEL.Trim() != "" && VideoSourceType!=9;

                LastConfig.Iptype = MainForm.IPTYPE;
                LastConfig.Ipmodel = MainForm.IPMODEL;
                if (s?.MmUrl != null)
                {
                    AudioModel = s.MmUrl.AudioModel;
                    LastConfig.Prefix = s.MmUrl.prefix;
                    LastConfig.Source = s.Source;
                    LastConfig.URL = s.MmUrl.url;
                    LastConfig.Cookies = s.MmUrl.cookies;
                    LastConfig.Flags = s.MmUrl.flags;

                    tokenPath = s.MmUrl.tokenPath;
                    tokenPost = s.MmUrl.tokenPost;
                    tokenPort = s.MmUrl.tokenPort;


                    if (s.MmUrl.portSpecified)
                        LastConfig.Port = Convert.ToInt32(s.MmUrl.port);
                }
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

        

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (pnlConnect.Visible)
            {
                DeviceScanner.Stop();
                ShowPanel(pnlFindNetwork);
                return;
            }
            if (pnlFindNetwork.Visible)
            {
                _scanner.Stop();
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

        private void quitScannerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _scanner.Stop();
        }

        private CameraScanner _scanner = new CameraScanner();
        
    }
}
