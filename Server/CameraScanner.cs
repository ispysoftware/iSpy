using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace iSpyApplication.Server
{
    class CameraScanner
    {
        public Thread Urlscanner;
        public string Make, Model;
        public string Username, Password;
        public int Channel;
        public Uri Uri;
        public volatile bool Quiturlscanner;

        public event EventHandler ScanComplete;
        public event EventHandler URLScan;
        public event EventHandler<ConnectionOptionEventArgs> URLFound;

        public void QuitScanner()
        {
            Quiturlscanner = true;
            if (Urlscanner != null)
            {
                try
                {
                    var i = 0;
                    while (!Urlscanner.Join(TimeSpan.Zero) && i < 10)
                    {
                        Thread.Sleep(200);
                        i++;
                    }
                    if (!Urlscanner.Join(TimeSpan.Zero))
                        Urlscanner.Abort();
                }
                catch
                {
                }
                Urlscanner = null;
            }
        }

        public void ScanCamera(ManufacturersManufacturer m)
        {
            var l = new List<ManufacturersManufacturer> { m };
            var luri = new List<Uri>();
            Urlscanner = new Thread(() => ListCameras(l, ref luri));
            Urlscanner.Start();
        }



        public void ScanCameras()
        {
            QuitScanner();
            Urlscanner = new Thread(DoScanCamera);
            Urlscanner.Start();
        }

        public void StopScan()
        {
            Quiturlscanner = true;
        }

        private void DoScanCamera()
        {
            Quiturlscanner = false;
            var lp = new List<Uri>();
            var m = MainForm.Sources.Where(p => String.Equals(p.name, Make, StringComparison.CurrentCultureIgnoreCase)).ToList();
            m.AddRange(MainForm.Sources.Where(p => !String.Equals(p.name, Make, StringComparison.CurrentCultureIgnoreCase)).ToList());
            ListCameras(m, ref lp);
        }

        private void ListCameras(IEnumerable<ManufacturersManufacturer> m, ref List<Uri> lp)
        {
            foreach (var s in m)
            {
                var cand = s.url.ToList();
                cand = cand.OrderBy(p => p.Source).ToList();

                foreach (var u in cand)
                {
                    Uri addr;
                    Uri audioUri = null;
                    int audioSourceTypeID = -1;
                    switch (u.prefix.ToUpper())
                    {
                        default:
                            addr = Conf.GetAddr(u, Uri, Channel, Username, Password);
                            if (!lp.Contains(addr))
                            {
                                lp.Add(addr);
                                URLScan?.Invoke(addr, EventArgs.Empty);
                                if (Helper.TestHttpurl(addr.ToString(), u.cookies, Username, Password))
                                {
                                    if (!string.IsNullOrEmpty(u.AudioSource))
                                    {
                                        audioUri = Conf.GetAddr(u, Uri, Channel, Username, Password, true);
                                        audioSourceTypeID = Conf.GetSourceType(u.AudioSource, 1);
                                    }

                                    ManufacturersManufacturerUrl u1 = u;
                                    URLFound?.Invoke(this,
                                        new ConnectionOptionEventArgs(new ConnectionOption(addr, audioUri, Conf.GetSourceType(u1.Source, 2), audioSourceTypeID, u1)));
                                }
                            }

                            break;
                        case "RTSP://":
                            addr = Conf.GetAddr(u, Uri, Channel, Username, Password);
                            if (!lp.Contains(addr))
                            {
                                lp.Add(addr);
                                URLScan?.Invoke(addr, EventArgs.Empty);
                                if (Helper.TestRtspurl(addr, Username, Password))
                                {
                                    if (!string.IsNullOrEmpty(u.AudioSource))
                                    {
                                        audioUri = Conf.GetAddr(u, Uri, Channel, Username, Password, true);
                                        audioSourceTypeID = Conf.GetSourceType(u.AudioSource, 1);
                                    }
                                    ManufacturersManufacturerUrl u1 = u;

                                    URLFound?.Invoke(this,
                                        new ConnectionOptionEventArgs(new ConnectionOption(addr, audioUri,
                                            Conf.GetSourceType(u1.Source, 2), audioSourceTypeID, u1)));
                                }
                            }
                            break;
                    }
                    if (Quiturlscanner)
                    {
                        ScanComplete?.Invoke(this, EventArgs.Empty);
                        return;
                    }
                }
            }
            ScanComplete?.Invoke(this, EventArgs.Empty);
        }



    }

    public class ConnectionOptionEventArgs : EventArgs
    {
        public ConnectionOption CO;

        public ConnectionOptionEventArgs(ConnectionOption co)
        {
            CO = co;
        }
    }
}
