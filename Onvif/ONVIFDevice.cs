using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using iSpyApplication.Onvif.Security;
using iSpyApplication.OnvifServices;
using iSpyApplication.Utilities;
using DateTime = System.DateTime;

namespace iSpyApplication.Onvif
{
    public class ONVIFDevice
    {
        private const string DefaultDeviceServicePath = "/onvif/device_service";
        private readonly NetworkCredential _credential;
        private static readonly TimeSpan ReconnectionDelay = TimeSpan.FromSeconds(5);
        //private CancellationTokenSource _cancellationTokenSource;
        //private IDeviceEventReceiver _deviceEventReceiver;
        public event EventHandler<ConnectionStateInfo> ConnectionStateChanged;
        //public event EventHandler<DeviceEvent> EventReceived;
        //public event EventHandler Stopped;

        private readonly IOnvifClientFactory _onvifClientFactory;
        private readonly string _deviceServicePath;
        private readonly IConnectionParameters _connectionParameters;
        private readonly int _rtspPort;
        private Capabilities12 _deviceCapabilities;
        private MediaEndpoint[] _mediaEndpoints;
        private Profile[] _profiles;
        public bool Initialized;
        private int _timeout;

        public Profile Profile;
        public MediaEndpoint StreamEndpoint;
        public PTZ PTZ;
        public Space1DDescription DefaultZSpeed;
        public Space2DDescription DefaultPTSpeed;

        public ONVIFDevice(string serviceUrl, string username, string password, int rtspPort, int timeout)
        {
            Uri uri;
            if (!Uri.TryCreate(serviceUrl, UriKind.Absolute, out uri))
                throw new ApplicationException("Uri: " + serviceUrl + " not recognised.");

            _credential = new NetworkCredential(username, password);
            _timeout = timeout;
            ServiceUri = uri;
            _rtspPort = rtspPort;

            _connectionParameters = new ConnectionParameters(ServiceUri, _credential, TimeSpan.FromSeconds(5));
            _onvifClientFactory = new OnvifClientFactory();
            _deviceServicePath = _connectionParameters.ConnectionUri.AbsolutePath;

            if (_deviceServicePath == "/")
                _deviceServicePath = DefaultDeviceServicePath;
        }

        public MediaEndpoint[] MediaEndpoints
        {
            get
            {
                if (_mediaEndpoints != null && _mediaEndpoints.Length > 0)
                    return _mediaEndpoints;

                Connect();
                return _mediaEndpoints;
            }
        }

        public class MediaEndpoint
        {
            public MediaUri Uri;
            public int Width, Height;

            public MediaEndpoint(MediaUri uri, VideoEncoderConfiguration config)
            {
                Uri = uri;
                Width = config.Resolution.Width;
                Height = config.Resolution.Height;
            }

            public MediaEndpoint(MediaUri uri, VideoSourceConfiguration config)
            {
                Uri = uri;
                Width = config.Bounds.width;
                Height = config.Bounds.height;
            }

            public override string ToString()
            {
                return Width + "x" + Height + ": " + Uri.Uri;
            }
        }

        public void SelectProfile(int profileIndex)
        {
            var p = Profiles;
            if (p.Length > 0)
            {
                profileIndex = Math.Min(profileIndex, p.Length - 1);
                profileIndex = Math.Max(0, profileIndex);
                Profile = p[profileIndex];
                StreamEndpoint = MediaEndpoints[profileIndex];
            }
            else
            {
                Profile = null;
                StreamEndpoint = null;
            }

        }

        private void Connect()
        {
            if (Initialized)
                return;
            try
            {
                DateTime deviceTime = GetDeviceTime();
                if (!_connectionParameters.Credentials.IsEmpty())
                {
                    byte[] nonceBytes = new byte[20];
                    var random = new Random();
                    random.NextBytes(nonceBytes);

                    var token = new SecurityToken(deviceTime, nonceBytes);

                    _onvifClientFactory.SetSecurityToken(token);
                }

                _deviceCapabilities = GetDeviceCapabilities();
                if (_deviceCapabilities?.Media?.XAddr == null)
                    throw new ApplicationException("No media endpoints found");

                var mediaUri = new Uri(_deviceCapabilities.Media.XAddr);
                var ep = new EndpointAddress(GetServiceUri(mediaUri.PathAndQuery));
                var mediaClient = _onvifClientFactory.CreateClient<Media>(ep, _connectionParameters, MessageVersion.Soap12, _timeout);

                var profiles = mediaClient.GetProfiles(new GetProfilesRequest()).Profiles.ToList();

                var streamSetup = new StreamSetup
                {
                    Stream = StreamType.RTPUnicast,
                    Transport = new Transport { Protocol = TransportProtocol.RTSP }
                };
                List<MediaEndpoint> uris = new List<MediaEndpoint>();

                for (var i = 0; i < profiles.Count(); i++)
                {
                    var p = profiles[i];
                    MediaUri l;
                    try
                    {
                        l = mediaClient.GetStreamUri(streamSetup, p.token);
                    }
                    catch (Exception ex)
                    {
                        profiles.Remove(p);
                        i--;
                        Logger.LogException(ex);
                        continue;
                    }

                    //make sure using correct ip address (for external access)
                    var u = new UriBuilder(l.Uri) { Host = ServiceUri.Host };
                    if (_rtspPort > 0)
                        u.Port = _rtspPort;

                    l.Uri = u.ToString();
                    if (!string.IsNullOrEmpty(_credential.UserName))
                    {
                        l.Uri = l.Uri.ReplaceFirst("://",
                            "://" + Uri.EscapeDataString(_credential.UserName) + ":" +
                            Uri.EscapeDataString(_credential.Password) + "@");
                    }

                    var s = p.VideoEncoderConfiguration;
                    if (s != null)
                        uris.Add(new MediaEndpoint(l, s));
                    else
                    {
                        var e = p.VideoSourceConfiguration;
                        if (e != null)
                            uris.Add(new MediaEndpoint(l, e));
                    }

                }

                _mediaEndpoints = uris.ToArray();
                _profiles = profiles.ToArray();

                DefaultPTSpeed = new Space2DDescription
                                 {
                                     XRange = new FloatRange { Max = 1, Min = -1 },
                                     YRange = new FloatRange { Max = 1, Min = -1 },
                                     URI = null
                                 };
                DefaultZSpeed = new Space1DDescription
                                {
                                    XRange = new FloatRange { Max = 1, Min = -1 },
                                    URI = null
                                };
                try
                {
                    if (_deviceCapabilities.PTZ != null)
                    {
                        var ptzUri = new Uri(_deviceCapabilities.PTZ.XAddr);
                        ep = new EndpointAddress(GetServiceUri(ptzUri.PathAndQuery));
                        PTZ = _onvifClientFactory.CreateClient<PTZ>(ep, _connectionParameters, MessageVersion.Soap12,
                            _timeout);

                        try
                        {
                            var gc = PTZ.GetConfigurationsAsync(new GetConfigurationsRequest()).Result;
                            var ptzcfg = PTZ.GetConfigurationOptions(gc.PTZConfiguration[0].NodeToken);
                            DefaultPTSpeed = ptzcfg.Spaces.ContinuousPanTiltVelocitySpace[0];
                            DefaultZSpeed = ptzcfg.Spaces.ContinuousZoomVelocitySpace[0];
                        }
                        catch (Exception ex)
                        {
                            //ignore - use defaults
                            Logger.LogException(ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex, "Onvif PTZ");
                }

                Initialized = true;
            }
            catch (Exception ex)
            {
                _profiles = new Profile[] { };
                _mediaEndpoints = new MediaEndpoint[] { };
                Logger.LogException(ex, "ONVIF Device");
            }
        }

        private DateTime GetDeviceTime()
        {
            Device deviceClient = CreateDeviceClient();
            SystemDateTime deviceSystemDateTime = deviceClient.GetSystemDateAndTime();

            DateTime deviceTime;
            if (deviceSystemDateTime.UTCDateTime == null)
                deviceTime = DateTime.UtcNow;
            else
            {
                deviceTime = new DateTime(deviceSystemDateTime.UTCDateTime.Date.Year,
                    deviceSystemDateTime.UTCDateTime.Date.Month,
                    deviceSystemDateTime.UTCDateTime.Date.Day, deviceSystemDateTime.UTCDateTime.Time.Hour,
                    deviceSystemDateTime.UTCDateTime.Time.Minute, deviceSystemDateTime.UTCDateTime.Time.Second, 0,
                    DateTimeKind.Utc);
            }

            return deviceTime;
        }
        private Capabilities12 GetDeviceCapabilities()
        {
            Device deviceClient = CreateDeviceClient();

            GetCapabilitiesResponse capabilitiesResponse = deviceClient.GetCapabilities(new GetCapabilitiesRequest(new[] { CapabilityCategory.All }));

            return capabilitiesResponse.Capabilities;
        }

        //public void UnsubscribeEvents()
        //{
            //_cancellationTokenSource?.Cancel();
        //}
        //public bool SubscribeToEvents()
        //{
        //    if (_cancellationTokenSource == null)
        //    {
        //        if (_deviceCapabilities == null)
        //        {
        //            Connect();
        //        }

        //        if (_deviceCapabilities?.Events?.WSPullPointSupport != true)
        //        {
        //            Logger.LogError("Pull events not supported on this Onvif device");
        //            return false;
        //        }

        //        _cancellationTokenSource = new CancellationTokenSource();
        //        var receiverFactory = new DeviceEventReceiverFactory();

        //        var connectionParameters = new ConnectionParameters(ServiceUri, _credential, TimeSpan.FromSeconds(_timeout));
        //        _deviceEventReceiver = receiverFactory.Create(connectionParameters, _timeout);
        //        _deviceEventReceiver.EventReceived += DeviceEventReceiverOnEventReceived;

        //        Task.Run(() => ReceiveEventsAsync(_deviceEventReceiver, _cancellationTokenSource.Token));
        //    }
        //    return true;
        //}

        //private void DeviceEventReceiverOnEventReceived(object sender, DeviceEvent deviceEvent)
        //{
        //    EventReceived?.Invoke(sender, deviceEvent);
        //}

        //private async void ReceiveEventsAsync(IDeviceEventReceiver deviceEventReceiver, CancellationToken token)
        //{
        //    while (!token.IsCancellationRequested)
        //    {
        //        try
        //        {
        //            OnStateChanged(new ConnectionStateInfo(
        //                $"Connecting to {deviceEventReceiver.ConnectionParameters.ConnectionUri}..."));

        //            await deviceEventReceiver.ConnectAsync(token).ConfigureAwait(false);

        //            OnStateChanged(new ConnectionStateInfo("Connection is established. Receiving..."));

        //            await deviceEventReceiver.ReceiveAsync(token);
        //        }
        //        catch (OperationCanceledException)
        //        {
        //            break;
        //        }
        //        catch (OutOfMemoryException)
        //        {
        //            throw;
        //        }
        //        catch (Exception e)
        //        {
        //            OnStateChanged(new ConnectionStateInfo($"Connection error: {e.Message}"));
        //        }

        //        try
        //        {
        //            await Task.Delay(ReconnectionDelay, token);
        //        }
        //        catch (OperationCanceledException)
        //        {
        //            break;
        //        }
        //    }
        //    _cancellationTokenSource.Dispose();
        //    _cancellationTokenSource = null;
        //    Stopped?.Invoke(this, EventArgs.Empty);
        //}

        protected virtual void OnStateChanged(ConnectionStateInfo e)
        {
            ConnectionStateChanged?.Invoke(this, e);
        }

        public Profile[] Profiles
        {
            get
            {
                if (_profiles != null && _profiles.Length > 0)
                    return _profiles;

                Connect();
                return _profiles;
            }
        }

        public Uri ServiceUri { get; set; }

        private Device CreateDeviceClient()
        {
            Uri deviceServiceUri = GetServiceUri(_deviceServicePath);

            var deviceClient = _onvifClientFactory.CreateClient<Device>(deviceServiceUri, _connectionParameters, MessageVersion.Soap12, _timeout);

            return deviceClient;
        }

        private Uri GetServiceUri(string serviceRelativePath)
        {
            return new Uri(_connectionParameters.ConnectionUri, serviceRelativePath);
        }
    }
}
