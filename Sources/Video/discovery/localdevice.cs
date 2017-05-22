using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using iSpyApplication.Utilities;
using iSpyPRO.DirectShow;

namespace iSpyApplication.Sources.Video.discovery
{
    class LocalDevice: IDisposable
    {
        private const string VideoFormatString = "{0} x {1} ({3} bit up to {2} fps)";
        private const string SnapshotFormatString = "{0} x {1} ({2} bit)";

        private readonly FilterInfoCollection _videoDevices;
        private VideoCaptureDevice _videoCaptureDevice;
        private VideoInput[] _availableVideoInputs;
        
        public int FrameRate = 0;

        
        public LocalDevice()
        {
            try
            {
                _videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex,"LocalDevice");
            }
        }

        
        public List<MainForm.ListItem> Devices
        {
            get
            {
                var l = new List<MainForm.ListItem>();
                if (_videoDevices!=null)
                {
                    l.AddRange(from FilterInfo dev in _videoDevices select new MainForm.ListItem(dev.Name, dev.MonikerString));
                }
                return l;
            }
        }

        public void Inspect(string moniker)
        {
            _videoCaptureDevice = new VideoCaptureDevice(moniker);
        }

        public List<MainForm.ListItem> Inputs
        {
            get
            {
                var ret = new List<MainForm.ListItem>();
                if (_videoCaptureDevice!=null)
                {
                    _availableVideoInputs = _videoCaptureDevice.AvailableCrossbarVideoInputs;
                    ret.AddRange(_availableVideoInputs.Select(input => new MainForm.ListItem($"{input.Index}: {input.Type}", input.Index.ToString(CultureInfo.InvariantCulture))));
                }
                return ret;
            }
        }

        public bool SupportsSnapshots => _videoCaptureDevice.SnapshotCapabilities.Any();

        public bool SupportsVideo => _videoCaptureDevice.VideoCapabilities.Any();

        public List<MainForm.ListItem> VideoResolutions
        {
            get
            {
                var ret = new List<MainForm.ListItem>();
                if (_videoCaptureDevice != null)
                {
                    VideoCapabilities[] videoCapabilities = _videoCaptureDevice.VideoCapabilities;
                    foreach (VideoCapabilities capabilty in videoCapabilities)
                    {
                        string item = string.Format(VideoFormatString, capabilty.FrameSize.Width, Math.Abs(capabilty.FrameSize.Height), capabilty.AverageFrameRate, capabilty.BitCount);
                        if (ret.FirstOrDefault(p => p.ToString() == item) == null)
                        {
                            ret.Add(new MainForm.ListItem(item,item));
                        }
                    }
                }
                return ret;
            }
        }

        public List<MainForm.ListItem> SnapshotResolutions
        {
            get
            {
                var ret = new List<MainForm.ListItem>();
                if (_videoCaptureDevice != null)
                {
                    VideoCapabilities[] snapshotCapabilities = _videoCaptureDevice.SnapshotCapabilities;
                    foreach (VideoCapabilities capabilty in snapshotCapabilities)
                    {
                        string item = string.Format(SnapshotFormatString, capabilty.FrameSize.Width, Math.Abs(capabilty.FrameSize.Height), capabilty.BitCount);
                        if (ret.FirstOrDefault(p => p.ToString() == item) == null)
                        {
                            ret.Add(new MainForm.ListItem(item, item));
                        }
                    }
                }
                return ret;
            }
        }

        private bool _disposed;
        // Public implementation of Dispose pattern callable by consumers. 
        public void Dispose()
        {
            Dispose(true);
        }

        // Protected implementation of Dispose pattern. 
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _videoCaptureDevice?.Dispose();
            }

            // Free any unmanaged objects here. 
            //
            _disposed = true;
        }
    }
}
