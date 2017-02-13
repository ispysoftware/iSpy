using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Discovery;
using System.Xml;
using iSpyApplication.Utilities;

namespace iSpyApplication.Onvif
{
    public static class Discovery
    {
        private static ServiceHost _host;
        private static AnnouncementService _announcementSrv;
        private static readonly List<DiscoveryClient> DiscoveryClients = new List<DiscoveryClient>();
        //public static event EventHandler DiscoveryComplete;
        public static List<string> DiscoveredDevices = new List<string>();
        private static readonly List<XmlQualifiedName> CtNs = new List<XmlQualifiedName> { new XmlQualifiedName("NetworkVideoTransmitter", @"http://www.onvif.org/ver10/network/wsdl"), new XmlQualifiedName("Device", @"http://www.onvif.org/ver10/device/wsdl") };

        public static void FindDevices()
        {
            NetworkChange.NetworkAddressChanged += NetworkChangeNetworkAddressChanged;

            StartService();
            DiscoverAdapters();
        }

        private static void NetworkChangeNetworkAddressChanged(object sender, EventArgs e)
        {
            Logger.LogMessageToFile("Network Change");
            DiscoveredDevices.Clear();
            DiscoverAdapters();
        }

        private static void StartService()
        {

            _announcementSrv = new AnnouncementService();
            var epAnnouncement = new UdpAnnouncementEndpoint(DiscoveryVersion.WSDiscoveryApril2005);
            ((CustomBinding)epAnnouncement.Binding).Elements.Insert(0, new MulticastCapabilitiesBindingElement(true));

            _announcementSrv.OnlineAnnouncementReceived += Announcement_srv_OnlineAnnouncementReceived;
            _announcementSrv.OfflineAnnouncementReceived += AnnouncementSrvOfflineAnnouncementReceived;

            _host = new ServiceHost(_announcementSrv);
            _host.AddServiceEndpoint(epAnnouncement);

            _host.Open();
        }

        private static void DiscoverAdapters()
        {

            var nics = NetworkInterface.GetAllNetworkInterfaces();

            lock (Lock)
            {
                DiscoveryClients.Clear();
            }
            
            foreach (var adapter in nics)
            {
                DiscoverAdapter(adapter);
            }
        }

        
        

        private static void AnnouncementSrvOfflineAnnouncementReceived(object sender, AnnouncementEventArgs args)
        {
            var uris = args?.EndpointDiscoveryMetadata?.ListenUris;
            if (uris == null)
                return;
            foreach (var uri in uris)
            {
                RemoveDevice(uri.ToString());
            }
        }

        private static void Announcement_srv_OnlineAnnouncementReceived(object sender, AnnouncementEventArgs args)
        {
            CheckDevice(args.EndpointDiscoveryMetadata);
        }

        private static void CheckDevice(EndpointDiscoveryMetadata epMeta)
        {
            var uris = epMeta?.ListenUris;
            if (uris == null || uris.Count==0)
                return;

            if (CtNs.Any(ctn => epMeta.ContractTypeNames.Contains(ctn)))
            {
                foreach (var uri in uris)
                {
                    AddDevice(uri.ToString());
                }
                return;
            }
            var names = epMeta.ContractTypeNames.Aggregate("", (current, ctn) => current + (ctn.Name + " (" + ctn.Namespace + ") "));
            Logger.LogMessageToFile("Ignored device at "+uris[0]+"  "+names);
        }

        private static void AddDevice(string uri)
        {
            lock(Lock) 
                if (!DiscoveredDevices.Contains(uri))
                {
                    DiscoveredDevices.Add(uri);
                    Logger.LogMessageToFile("Added device: "+uri);
                }
        }

        private static void RemoveDevice(string uri)
        {
            lock (Lock)
            {
                DiscoveredDevices.RemoveAll(p => p == uri);
                Logger.LogMessageToFile("Removed device: " + uri);
            }
        }

        public static bool DiscoveryFinished
        {
            get { return DiscoveryClients.Count == 0; }
        }

        private static readonly object Lock = new object();
        //private static void DiscoveryClientFindCompleted(object sender, FindCompletedEventArgs e)
        //{
        //    lock (_lock)
        //    {
        //        var c = (DiscoveryClient)sender;
        //        var dc = DiscoveryClients;
        //        try
        //        {
        //            if (dc.Contains(c))
        //                dc.Remove(c);
        //        }
        //        catch (Exception)
        //        {

        //        }
        //        if (dc.Count == 0)
        //        {
        //            Logger.LogMessageToFile("Discovery completed");
        //            DiscoveryComplete?.Invoke(null, EventArgs.Empty);
        //        }
        //    }
        //}

        private static void DiscoveryClientFindProgressChanged(object sender, FindProgressChangedEventArgs args)
        {
            CheckDevice(args.EndpointDiscoveryMetadata);
        }

        private static void DiscoverAdapter(NetworkInterface adapter)
        {
            if (!adapter.GetIPProperties().MulticastAddresses.Any())
                return; // most of VPN adapters will be skipped
            if (!adapter.SupportsMulticast)
                return; // multicast is meaningless for this type of connection
            if (OperationalStatus.Up != adapter.OperationalStatus)
                return; // this adapter is off or not connected
            //IPv4InterfaceProperties p = adapter.GetIPProperties().GetIPv4Properties();
            //if (null == p)
            //    continue; // IPv4 is not configured on this adapter

            var dc = GenerateDiscoveryClients(adapter.Id);
            if (dc != null)
                DiscoveryClients.AddRange(dc);
        }

        private static DiscoveryClient[] GenerateDiscoveryClients(string adapterId)
        {
            var dcs = new[]
                      {
                          BuildDiscoveryClient(adapterId, DiscoveryVersion.WSDiscovery11),
                          BuildDiscoveryClient(adapterId, DiscoveryVersion.WSDiscoveryApril2005),
                          BuildDiscoveryClient(adapterId, DiscoveryVersion.WSDiscoveryCD1)

                      };

            return dcs;
        }

        private static DiscoveryClient BuildDiscoveryClient(string adapterId, DiscoveryVersion version)
        {
            var epDiscovery = new UdpDiscoveryEndpoint(version);
            var b = (CustomBinding)epDiscovery.Binding;
            if (b == null)
                return null;

            epDiscovery.TransportSettings.MulticastInterfaceId = adapterId;
            b.Elements.Insert(0, new MulticastCapabilitiesBindingElement(true));

            var discoveryClient = new DiscoveryClient(epDiscovery);

            discoveryClient.FindProgressChanged += DiscoveryClientFindProgressChanged;
            //discoveryClient.FindCompleted += DiscoveryClientFindCompleted;

            FindCriteria findCriteria = new FindCriteria
            {
                Duration = TimeSpan.FromMinutes(5),
                MaxResults = int.MaxValue
            };

            foreach (var ctn in CtNs)
                findCriteria.ContractTypeNames.Add(ctn);

            discoveryClient.FindAsync(findCriteria);
            return discoveryClient;
        }

    }
}
