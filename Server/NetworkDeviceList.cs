using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using iSpyApplication.Utilities;

namespace iSpyApplication.Server
{
    public static class NetworkDeviceList
    {
        private static readonly BindingList<NetworkDevice> NetworkDeviceBindingList = new BindingList<NetworkDevice>();

        public static BindingList<NetworkDevice> List => NetworkDeviceBindingList;

        public static void Add(NetworkDevice fp)
        {
            if (List.Count(p => Equals(p.IPAddress, fp.IPAddress) && p.Port == fp.Port) == 0)
                List.Add(fp);
        }

        public static void RemoveAll()
        {
            var l = NetworkDeviceBindingList.ToList();
            for (int i = 0; i < l.Count(); i++)
            {
                NetworkDeviceBindingList.Remove(l[i]);
            }
        }

        public static void Remove(IPAddress ipAddress, int port)
        {
            var l = NetworkDeviceBindingList.Where(p => Equals(p.IPAddress, ipAddress) && p.Port == port).ToList();
            for (int i = 0; i < l.Count(); i++)
            {
                NetworkDeviceBindingList.Remove(l[i]);
            }
        }

        private static Dictionary<string, string> _arpList;

        public static Dictionary<string, string> ARPList
        {
            get
            {
                if (_arpList != null)
                    return _arpList;
                RefreshARP();
                return _arpList;
            }
        }

        public static void RefreshARP()
        {
            _arpList = new Dictionary<string, string>();
            _arpList.Clear();
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
                            string ip = parts[0];
                            string mac = parts[1];
                            if (!_arpList.ContainsKey(ip))
                                _arpList.Add(ip, mac);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "ARP Table");
            }
            if (_arpList.Count > 0)
            {
                foreach (var nd in List)
                {
                    string mac;
                    ARPList.TryGetValue(nd.IPAddress.ToString(), out mac);
                    nd.MAC = mac;

                }
            }
        }

        private static StreamReader ExecuteCommandLine(string file, string arguments = "")
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

            return process?.StandardOutput;
        }
    }

    public class NetworkDevice
    {
        public IPAddress IPAddress { get; }

        public string DeviceName { get; }

        public string WebServer { get; }

        public string MAC { get; set; }

        public int Port { get; }

        public NetworkDevice(IPAddress ipAddress, int port, string deviceName, string webServer)
        {
            IPAddress = ipAddress;
            DeviceName = deviceName;
            WebServer = webServer;
            Port = port;
            string mac;
            NetworkDeviceList.ARPList.TryGetValue(ipAddress.ToString(), out mac);
            MAC = mac;

        }





    }
}
