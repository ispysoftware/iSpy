using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using iSpyApplication.Utilities;

namespace iSpyApplication
{
    public partial class NetworkTroubleshooter : Form
    {
        private readonly string NL = Environment.NewLine;
        public NetworkTroubleshooter()
        {
            InitializeComponent();
            Text = LocRm.GetString("troubleshooting");
            button1.Text = LocRm.GetString("OK");
            LocRm.GetString("retry");
        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void NetworkTroubleshooter_Load(object sender, EventArgs e)
        {
            UISync.Init(this);


            var t = new Thread(Troubleshooter) { IsBackground = true };
            t.Start();
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
                try
                {
                    _sync.BeginInvoke(action, null);
                }
                catch
                {
                }
            }
        }

        private const string webports = ",80,8080,8081,8280,8888,8887,9080,16080,";

        private void Troubleshooter()
        {
            //causes a reset of detected ip addresses
            MainForm.AddressIPv4 = MainForm.Conf.IPv4Address;
            MainForm.AddressIPv6 = MainForm.Conf.IPv6Address;

            UISync.Execute(() => rtbOutput.Clear());

            try
            {
                MainForm.StopAndStartServer();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            
            bool portMapOk = false;
            bool bIPv6 = MainForm.Conf.IPMode == "IPv6";
            UISync.Execute(() => button2.Enabled = false);

            UISync.Execute(() => rtbOutput.Text +=
                            NL + NL + "WARNING: Upcoming (April 2022) chrome web browser updates are likely to block access to iSpy (as it runs on http). If you can't access the portal try a different web browser (like FireFox). We highly recommend upgrading to our new platform Agent DVR which is unaffected by these new browser restrictions. You can load your iSpy config into Agent DVR and remote access is included under any subscription you may have for iSpy." + NL + NL + "See https://www.ispyconnect.com/userguide-agent-DVR.aspx#ispy for instructions."+NL+NL);

            string localserver = (MainForm.Conf.SSLEnabled?"https":"http")+"://" + MainForm.IPAddress + ":" + MainForm.Conf.LANPort;

            UISync.Execute(() => rtbOutput.Text += $"Local iSpy Server: {localserver}{NL}");
            
            if (webports.IndexOf(","+MainForm.Conf.LANPort+",", StringComparison.Ordinal)==-1)
            {
                UISync.Execute(() => rtbOutput.Text +=
                    $"Warning: Running a local server on a non-standard port ({MainForm.Conf.LANPort}) may cause web-browser security errors. Click the link above to test in your web browser.{NL}");
            }
            if (MainForm.IPAddress.StartsWith("169.254"))
            {
                UISync.Execute(() => rtbOutput.Text += NL+"Warning: Your network adaptor has assigned itself a link-local address (169.254.x.x). This means your PC is setup for DHCP but can't find a DHCP server and iSpy will be unavailable over your LAN. Try resetting your router."+NL);
            }
            if (MainForm.Conf.SSLEnabled)
                UISync.Execute(() => rtbOutput.Text += "Warning: Using SSL - disable SSL in settings if you are having problems with connecting."+NL);
            if (MainForm.Conf.SpecificIP)
                UISync.Execute(
                    () =>
                        rtbOutput.Text +=
                            "Warning: You are binding to a specific IP address. This can cause issues on systems with multiple NICs. Try unchecking the Bind To IP Address option in settings/ web server if you have problems." +
                            NL);
            
            UISync.Execute(() => rtbOutput.Text += "Checking local server... ");
            Application.DoEvents();
            string res = "";
            if (!loadurl(localserver, out res))
            {
                string res1 = res;
                UISync.Execute(() => rtbOutput.Text += $"Failed: {res1}{NL}");
                if (MainForm.MWS.Running)
                {
                    UISync.Execute(() => rtbOutput.Text += "Server reports it IS running" + NL);
                }
                else
                    UISync.Execute(() => rtbOutput.Text += "Server reports it IS NOT running - check the log file for errors (View-> Log File)" + NL);

                UISync.Execute(() => rtbOutput.Text += "Do you have a third party firewall or antivirus running (AVG/ zonealarm etc)?" + NL);

                

            }
            else
            {
                res = res.ToLower();
                if (res == "ok" || res.IndexOf("ispy", StringComparison.Ordinal) != -1)
                {
                    UISync.Execute(() => rtbOutput.Text += "OK");
                }
                else
                {
                    string res1 = res;
                    UISync.Execute(() => rtbOutput.Text += $"Unexpected output: {res1}");
                }
            }
            UISync.Execute(() => rtbOutput.Text += NL);
            UISync.Execute(() => rtbOutput.Text += "Checking WebServer... ");
            Application.DoEvents();
            if (!loadurl(MainForm.Webserver + "/webservices/ispyapi.asmx", out res))
            {
                UISync.Execute(() => rtbOutput.Text += "Webservices not responding.");
            }
            else
            {
                if (res.IndexOf("error occurred while", StringComparison.Ordinal)!=-1)
                    UISync.Execute(() => rtbOutput.Text += "Error with webservices. Please try again later (check your internet connection).");
                else
                    UISync.Execute(() => rtbOutput.Text += "OK");
            }
            UISync.Execute(() => rtbOutput.Text += NL);
            UISync.Execute(() => rtbOutput.Text += "Checking your firewall... ");
            Application.DoEvents();
            try
            {
                var fw = new FireWall();
                fw.Initialize();

                bool bOn;
                var r = fw.IsWindowsFirewallOn(out bOn);

                if (r == FireWall.FwErrorCode.FwNoerror)
                {
                    if (bOn)
                    {
                        string strApplication = Application.StartupPath + "\\iSpy.exe";
                        bool bEnabled = false;
                        fw.IsAppEnabled(strApplication, ref bEnabled);
                        if (!bEnabled)
                        {
                            UISync.Execute(
                                () =>
                                    rtbOutput.Text +=
                                        "iSpy is *NOT ENABLED* - add ispy.exe to the windows firewall allowed list");
                        }
                        else
                        {
                            UISync.Execute(() => rtbOutput.Text += "iSpy is enabled");
                        }
                    }
                    else
                    {
                        UISync.Execute(() => rtbOutput.Text += "Firewall is off");
                    }
                }
                else
                {
                    UISync.Execute(() => rtbOutput.Text += "Firewall error: " + r);
                }
            }
            catch (Exception ex)
            {
                UISync.Execute(() => rtbOutput.Text += "Firewall error: " + ex.Message);
                UISync.Execute(() => rtbOutput.Text += NL + LocRm.GetString("AddFirewallExceptionManually"));
            }
            UISync.Execute(() => rtbOutput.Text += NL);

            
            UISync.Execute(() => rtbOutput.Text += "Checking your account... ");

            var result = WsWrapper.TestConnection(MainForm.Conf.WSUsername, MainForm.Conf.WSPassword, false);
            if (result[0] != "OK")
            {
                UISync.Execute(() => rtbOutput.Text += result[0]);
            }
            else
            {
                string[] result1 = result;
                UISync.Execute(() => rtbOutput.Text += "Found: " + result1[2]);
                if (Convert.ToBoolean(result[1]))
                {
                    UISync.Execute(() => rtbOutput.Text += NL + "Your subscription is valid." + NL);
                    if (MainForm.Conf.IPMode == "IPv4")
                    {

                        UISync.Execute(() => rtbOutput.Text += "IPv4: Checking port mappings... " + NL);
                        try
                        {
                            if (NATControl.Mappings == null)
                            {
                                UISync.Execute(
                                    () =>
                                    rtbOutput.Text +=
                                    "IPv4 Port mappings are unavailable - set up port mapping manually, instructions here: http://portforward.com/english/routers/port_forwarding/routerindex.htm" +
                                    NL);
                            }
                            else
                            {
                                int j = 2;
                                while (!portMapOk && j > 0)
                                {
                                    int maps = 0;
                                    try
                                    {
                                        var enumerator = NATControl.Mappings.GetEnumerator();
                                        while (enumerator.MoveNext())
                                        {
                                            var map = (NATUPNPLib.IStaticPortMapping)enumerator.Current;
                                            UISync.Execute(
                                                () =>
                                                rtbOutput.Text +=
                                                map.ExternalPort + " -> " + map.InternalPort + " on " +
                                                map.InternalClient +
                                                " (" +
                                                map.Protocol + ")" + NL);
                                            if (map.ExternalPort == MainForm.Conf.ServerPort)
                                            {
                                                if (map.InternalPort != MainForm.Conf.LANPort)
                                                {
                                                    UISync.Execute(
                                                        () =>
                                                        rtbOutput.Text +=
                                                        "*** External port is routing to " + map.InternalPort +
                                                        " instead of " +
                                                        MainForm.Conf.LANPort + NL);
                                                }
                                                else
                                                {
                                                    if (map.InternalClient != MainForm.AddressIPv4)
                                                    {
                                                        UISync.Execute(
                                                            () =>
                                                            rtbOutput.Text +=
                                                            "*** Port is mapping to IP Address " + map.InternalClient +
                                                            " - should be " +
                                                            MainForm.AddressIPv4 +
                                                            ". Set a static IP address for your computer and then update the port mapping." +
                                                            NL);
                                                    }
                                                    else
                                                    {
                                                        portMapOk = true;
                                                    }
                                                }
                                            }
                                            maps++;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        UISync.Execute(
                                            () => rtbOutput.Text += "Port mapping lookup failed ("+ex.Message.Trim()+"). If the connection fails try resetting your router or manually configure port forwarding. " + NL);
                                        if (maps==0)
                                            throw;
                                    }
                                    if (!portMapOk)
                                    {
                                        //add port mapping
                                        UISync.Execute(() => rtbOutput.Text += "IPv4: Fixing port mapping... " + NL);
                                        if (!NATControl.SetPorts(MainForm.Conf.ServerPort, MainForm.Conf.LANPort))
                                        {
                                            UISync.Execute(
                                                () => rtbOutput.Text += LocRm.GetString("ErrorPortMapping") + NL);
                                        }

                                        j--;
                                        if (j > 0)
                                            UISync.Execute(
                                                () => rtbOutput.Text += "IPv4: Checking port mappings... " + NL);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.LogException(ex);
                           
                        }
                    }
                    

                    UISync.Execute(() => rtbOutput.Text += "Checking external access... "+NL);

                    result = WsWrapper.TestConnection(MainForm.Conf.WSUsername, MainForm.Conf.WSPassword, true);

                    if (result.Length>3 && result[3] != "")
                    {
                        MainForm.Conf.Loopback = MainForm.LoopBack = false;
                        UISync.Execute(() => rtbOutput.Text += "iSpyConnect is trying to contact your server at: "+result[6] + NL);
                        UISync.Execute(() => rtbOutput.Text += "Failed: " + result[3] + NL);
                        if (!bIPv6)
                        {
                            UISync.Execute(
                                () =>
                                rtbOutput.Text +=
                                "Your router should be configured to forward TCP traffic from WAN (external) port " +
                                MainForm.Conf.ServerPort + " to internal (LAN) port " +
                                MainForm.Conf.LANPort + " on IP address " + MainForm.AddressIPv4 +
                                NL);
                            if (portMapOk)
                            {
                                UISync.Execute(
                                    () =>
                                    rtbOutput.Text +=
                                    NL +
                                    "Your port mapping seems to be OK - try turning your router off and on again. Failing that we recommend checking with your ISP to see if they are blocking port " +
                                    MainForm.Conf.ServerPort +
                                    " or check if your antivirus protection (eset, zonealarm etc) is blocking iSpy. ");
                            }
                        }

                        if (MainForm.AddressListIPv4.Length > 1)
                        {
                            UISync.Execute(() => rtbOutput.Text += NL+"Warning: There are multiple network adaptors in your PC. Try selecting a different IP address to listen on in iSpy web settings or disable unused network adaptors and restart iSpy: " + NL);
                            foreach (var ip in MainForm.AddressListIPv4)
                            {
                                string ip1 = ip.ToString();
                                if (ip1 != MainForm.IPAddress)
                                    UISync.Execute(() => rtbOutput.Text += "\t" + ip1 + NL);
                            }
                        }
                        UISync.Execute(() => rtbOutput.Text += NL + NL + "Our new platform, Agent DVR doesn't require port forwarding and should work anywhere with an internet connection: http://www.ispyconnect.com/download.aspx");
                        UISync.Execute(() => rtbOutput.Text += NL + NL + "Please see the troubleshooting section here: http://www.ispyconnect.com/userguide-remote-access-troubleshooting.aspx");

                    }
                    else
                    {
                        if (result.Length == 1)
                        {
                            UISync.Execute(() => rtbOutput.Text +=
                                                 "Failed: Communication with webserver failed." + NL + NL);
                        }
                        else
                        {
                            UISync.Execute(() => rtbOutput.Text +=
                                "Success!" + NL + NL + "If you cannot access content locally please ensure 'Use LAN IP when available' is checked on " + MainForm.Webserver + "/account.aspx and also ensure you're using an up to date web browser.");

                            MainForm.Conf.Loopback = MainForm.LoopBack = true;
                        }
                            
                    }
                }
                else
                {
                    UISync.Execute(() => rtbOutput.Text += NL +
                                      "Not subscribed - local access only. http://www.ispyconnect.com/subscribe.aspx");
                }

            }
            UISync.Execute(() => rtbOutput.Text+=NL);
            Application.DoEvents();
            UISync.Execute(() => button2.Enabled = true);
        }

        private void rtbOutput_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            MainForm.OpenUrl(e.LinkText);
        }


        private bool loadurl(string url, out string result)
        {
            result = "";
            try
            {
                var httpWReq = (HttpWebRequest) WebRequest.Create(url);
                httpWReq.Timeout = 5000;
                httpWReq.Method = "GET";

                var myResponse = (HttpWebResponse) httpWReq.GetResponse();
                var s = myResponse.GetResponseStream();
                if (s != null)
                {
                    var read = new StreamReader(s);
                    result = read.ReadToEnd();
                }
                myResponse.Close();
                
                return true;
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var t = new Thread(Troubleshooter) { IsBackground = true };
            t.Start();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetText(rtbOutput.Text);
        }

    }
}
