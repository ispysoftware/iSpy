using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using iSpyApplication.Onvif;
using iSpyApplication.Server;
using iSpyApplication.Utilities;

namespace iSpyApplication.CameraDiscovery
{
    public class CameraScanner
    {
        public Thread Urlscanner;
        private static readonly ManualResetEvent Finished = new ManualResetEvent(false);
        public string Make, Model;
        public string Username, Password;
        public int Channel;
        public Uri Uri;
        private volatile bool _quit;
        private List<Uri> _lp = new List<Uri>();

        public event EventHandler ScanComplete;
        public event EventHandler URLScan;
        public event EventHandler<ConnectionOptionEventArgs> URLFound;

        private URLDiscovery _discoverer;

        public void ScanCamera(ManufacturersManufacturer mm)
        {
            Stop();
            var l = new List<ManufacturersManufacturer>();
            if (mm != null)
                l.Add(mm);
            else
            {
                //scan all
                l.AddRange(MainForm.Sources);
            }
            _lp = new List<Uri>();
            _quit = false;
            Finished.Reset();
            
            Urlscanner = new Thread(() => ListCameras(l, Model));
            Urlscanner.Start();
        }

        public void Stop()
        {
            if (Running)
            {
                _quit = true;
                Finished.WaitOne(4000);
                _lp.Clear();
            }
        }

        public bool Running => Helper.ThreadRunning(Urlscanner);


        private void ListCameras(IEnumerable<ManufacturersManufacturer> mm, string model)
        {
            model = (model ?? "").ToLowerInvariant();
            //find http port
            _discoverer = new URLDiscovery(Uri);

            //check for onvif support first
            int i = 0;
            try
            {
                var httpUri = _discoverer.BaseUri.SetPort(_discoverer.HttpPort);

                //check for this devices 
                foreach (var d in Discovery.DiscoveredDevices)
                {
                    if (d.DnsSafeHost == Uri.DnsSafeHost)
                    {
                        httpUri = _discoverer.BaseUri.SetPort(d.Port);
                        break;
                    }
                }

                var onvifurl = httpUri + "onvif/device_service";
                var dev = new ONVIFDevice(onvifurl, Username, Password,0,8);
                if (dev.Profiles != null)
                {
                    foreach (var p in dev.Profiles)
                    {
                        var b = p?.VideoEncoderConfiguration?.Resolution;
                        if (b != null && b.Width > 0)
                        {
                            dev.SelectProfile(i);
                            var co = new ConnectionOption(onvifurl, null, 9, -1, null)
                            {
                                MediaIndex = i
                            };
                            URLFound?.Invoke(this,
                                new ConnectionOptionEventArgs(co));
                        }
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            foreach (var m in mm)
            {
                //scan selected model first
                var cand = m.url.Where(p => p.version.ToLowerInvariant() == model).ToList();
                Scan(cand);
                cand = m.url.Where(p => p.version.ToLowerInvariant() != model).ToList();
                Scan(cand);
                if (_quit)
                    break;
            }

            ScanComplete?.Invoke(this, EventArgs.Empty);
            Finished.Set();
        }

        private void Scan(List<ManufacturersManufacturerUrl> cand)
        {
            if (_quit || cand.Count == 0)
                return;

            var un = Uri.EscapeDataString(Username);
            var pwd = Uri.EscapeDataString(Password);

            foreach (var s in cand)
            {
                Uri audioUri = null;
                int audioSourceTypeID = -1;
                var addr = _discoverer.GetAddr(s, Channel, un, pwd);
                if (addr != null && !_lp.Contains(addr))
                {
                    _lp.Add(addr);
                    URLScan?.Invoke(addr, EventArgs.Empty);
                    bool found = _discoverer.TestAddress(addr, s, un, pwd);
                    if (found)
                    {
                        if (!string.IsNullOrEmpty(s.AudioSource))
                        {
                            audioUri = _discoverer.GetAddr(s, Channel, un, pwd, true);
                            audioSourceTypeID = Helper.GetSourceType(s.AudioSource, 1);
                        }
                        ManufacturersManufacturerUrl s1 = s;

                        URLFound?.Invoke(this,
                            new ConnectionOptionEventArgs(new ConnectionOption(addr, audioUri,
                                Helper.GetSourceType(s1.Source, 2), audioSourceTypeID, s1)));
                    }
                }

                if (_quit)
                    return;
            }
        }

    }



    public class ConnectionOptionEventArgs : EventArgs
    {
        public ConnectionOption Co;

        public ConnectionOptionEventArgs(ConnectionOption co)
        {
            Co = co;
        }
    }
}
