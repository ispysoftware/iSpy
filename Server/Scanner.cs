using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading;
using iSpyApplication.Utilities;

namespace iSpyApplication.Server
{
    public class Scanner
    {
        private const int MaxThreads = 10;
        public event EventHandler<DeviceFoundEventArgs> DeviceFound;
        public event EventHandler ScanFinished;

        public void PortScannerManager(IEnumerable<string> ipranges, IEnumerable<int> ports)
        {

            var manualEvents = new ManualResetEvent[MaxThreads];
            int j = 0;
            for (int k = 0; k < MaxThreads; k++)
            {
                manualEvents[k] = new ManualResetEvent(true);
            }

            Logger.LogMessage("Scanning LAN", "NetworkDiscovery");


            if (!MainForm.ShuttingDown)
            {
                j = 0;
                foreach (string shost in ipranges)
                {
                    for (int i = 0; i < 255; i++)
                    {
                        string ip = shost.Replace("x", i.ToString(CultureInfo.InvariantCulture));
                        int k = j;
                        manualEvents[k].Reset();
                        IPAddress ipa;
                        if (IPAddress.TryParse(ip, out ipa))
                        {
                            var scanner = new Thread(p => PortScanner(ports, ipa, manualEvents[k]));
                            scanner.Start();

                            j = WaitHandle.WaitAny(manualEvents);
                        }
                        if (MainForm.ShuttingDown)
                            break;
                    }
                    if (MainForm.ShuttingDown)
                        break;
                }
            }

            if (j > 0)
                WaitHandle.WaitAll(manualEvents);

            if (!MainForm.ShuttingDown)
            {
                ScanFinished?.Invoke(this, EventArgs.Empty);
            }
        }

        public void ARPScannerManager(IEnumerable<IPAddress> ips, IEnumerable<int> ports)
        {

            var manualEvents = new ManualResetEvent[MaxThreads];
            int j = 0;
            for (int k = 0; k < MaxThreads; k++)
            {
                manualEvents[k] = new ManualResetEvent(true);
            }

            Logger.LogMessage("ARP Scan", "NetworkDiscovery");


            if (!MainForm.ShuttingDown)
            {
                j = 0;
                foreach (IPAddress ip in ips)
                {
                    int k = j;
                    manualEvents[k].Reset();
                    IPAddress ipa = ip;

                    var scanner = new Thread(p => PortScanner(ports, ipa, manualEvents[k]));
                    scanner.Start();

                    j = WaitHandle.WaitAny(manualEvents);

                    if (MainForm.ShuttingDown)
                        break;
                }
            }

            if (j > 0)
                WaitHandle.WaitAll(manualEvents);

            if (!MainForm.ShuttingDown)
            {
                ScanFinished?.Invoke(this, EventArgs.Empty);
            }
        }


        private void PortScanner(IEnumerable<int> ports, IPAddress ipaddress, ManualResetEvent mre)
        {
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
                    request.KeepAlive = false;

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
                        Logger.LogMessage("Web error from " + ipaddress + ":" + iport + " " + ex.Message, "NetworkDiscovery");
                    }
                    if (response != null)
                    {
                        Logger.LogMessage("Web response from " + ipaddress + ":" + iport + " " +
                                                    response.StatusCode, "NetworkDiscovery");
                        if (response.Headers != null)
                        {
                            string webserver = "yes";
                            foreach (string k in response.Headers.AllKeys)
                            {
                                if (k.ToLower().Trim() == "server")
                                    webserver = response.Headers[k];
                            }
                            int iport1 = iport;
                            if (DeviceFound != null)
                            {
                                var nd = new NetworkDevice(ipaddress, iport1, hostname, webserver);
                                DeviceFound(this, new DeviceFoundEventArgs(nd));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogMessage("Web error from " + ipaddress + ":" + iport + " " + ex.Message, "NetworkDiscovery");

                }
            }
            mre.Set();
        }

    }

    public class DeviceFoundEventArgs : EventArgs
    {
        public NetworkDevice Device;

        // Constructor
        public DeviceFoundEventArgs(NetworkDevice device)
        {
            Device = device;
        }
    }

}
