using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using iSpyApplication.DeviceClient;
using iSpyApplication.DeviceMedia;
using iSpyApplication.DevicePTZ;
using iSpyApplication.Utilities;
using DateTime = System.DateTime;
using VideoEncoderConfiguration = iSpyApplication.DeviceMedia.VideoEncoderConfiguration;
using VideoSourceConfiguration = iSpyApplication.DeviceMedia.VideoSourceConfiguration;

namespace iSpyApplication.Onvif
{
    public class ONVIFDevice
    {
        public ONVIFDevice(string url, string username, string password)
        {
            Uri uri;
            if (!Uri.TryCreate(url, UriKind.Absolute, out uri))
                throw new ApplicationException("Uri: " + url + " not recognised.");

            _svcURL = uri.Scheme + "://" + uri.Authority + "/onvif/device_service";

            Username = username;
            Password = password;
            URL = uri;

        }

        private readonly string _svcURL;
        private MediaEndpoint[] _mediaEndpoints;

        public DeviceClient.DeviceClient Client;
        public PTZClient PTZClient;

        public void ResetConnection()
        {
            _mediaEndpoints = null;
            _profiles = null;
            _mediaClient = null;

        }
        public MediaEndpoint[] MediaEndpoints
        {
            get
            {
                if (_mediaEndpoints != null)
                    return _mediaEndpoints;


                var mc = MediaClient;
                if (mc != null)
                {
                    try
                    {
                        var streamSetup = new StreamSetup
                                          {
                                              Stream = StreamType.RTPUnicast,
                                              Transport = new Transport {Protocol = TransportProtocol.RTSP}
                                          };
                        List<MediaEndpoint> uris = new List<MediaEndpoint>();
                        var profiles = Profiles;
                        if (profiles != null)
                        {
                            foreach (var p in Profiles)
                            {
                                try
                                {
                                    var l = mc.GetStreamUri(streamSetup, p.token);
                                    //make sure using correct ip address (for external access)
                                    var u = new UriBuilder(l.Uri) {Host = URL.Host};
                                    l.Uri = u.ToString();
                                    if (!string.IsNullOrEmpty(Username))
                                    {
                                        l.Uri = l.Uri.ReplaceFirst("://",
                                            "://" + Uri.EscapeDataString(Username) + ":" +
                                            Uri.EscapeDataString(Password) + "@");
                                    }

                                    var s = p?.VideoEncoderConfiguration;
                                    if (s != null)
                                        uris.Add(new MediaEndpoint(l, s));
                                    else
                                    {
                                        var e = p?.VideoSourceConfiguration;
                                        if (e != null)
                                            uris.Add(new MediaEndpoint(l, e));
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Logger.LogException(ex, "Onvif device (1)");
                                    break;
                                }
                            }

                            _mediaEndpoints = uris.ToArray();
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex, "Onvif device (2)");
                    }
                }
                return _mediaEndpoints;

            }
        }

        public Profile Profile;
        public MediaEndpoint Endpoint;

        public class MediaEndpoint
        {
            public MediaUri URI;
            public int Width,Height;

            public MediaEndpoint(MediaUri uri, VideoEncoderConfiguration config)
            {
                URI = uri;
                Width = config.Resolution.Width;
                Height = config.Resolution.Height;
            }

            public MediaEndpoint(MediaUri uri, VideoSourceConfiguration config)
            {
                URI = uri;
                Width = config.Bounds.width;
                Height = config.Bounds.height;
            }

            public override string ToString()
            {
                return Width + "x" + Height + ": " + URI.Uri;
            }
        }


        public void SelectProfile(int profileIndex)
        {
            var p = Profiles;
            if (p != null && profileIndex < p.Length)
            {
                if (Profiles.Length > profileIndex)
                    Profile = Profiles[profileIndex];
                if (MediaEndpoints.Length > profileIndex)
                    Endpoint = MediaEndpoints[profileIndex];
            }
                
        }

        private bool _useDigest = false;
        private Profile[] _profiles;
        public Profile[] Profiles
        {
            get
            {
                if (_profiles != null)
                    return _profiles;

                if (MediaClient != null)
                {
                    while (true)
                    {
                        try
                        {
                            _profiles = MediaClient.GetProfiles().ToArray();
                            break;
                        }
                        catch (Exception ex)
                        {
                            if (!_useDigest)
                            {
                                _mediaClient = null;
                                _useDigest = true;
                                continue;
                            }
                            _useDigest = false;
                            Logger.LogException(ex);
                            break;
                        }
                    }
                }
                return _profiles;
            }
        }
        public string Username { get; set; }
        public string Password { get; set; }
        public Uri URL { get; set; }

        private MediaClient _mediaClient;
        public MediaClient MediaClient
        {
            get
            {
                if (_mediaClient != null)
                {
                    return _mediaClient;
                }
                try
                {
                    EndpointAddress serviceAddress = new EndpointAddress(_svcURL);

                    HttpTransportBindingElement httpBinding = new HttpTransportBindingElement
                    {
                        AuthenticationScheme = AuthenticationSchemes.Digest
                    };

                    var messageElement = new TextMessageEncodingBindingElement
                     {
                         MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None)
                     };

                    CustomBinding binder = new CustomBinding(messageElement, httpBinding);
                    Client = new DeviceClient.DeviceClient(binder, serviceAddress);
                    binder.SendTimeout = binder.CloseTimeout = binder.ReceiveTimeout = binder.OpenTimeout = TimeSpan.FromSeconds(5);
                    
                    double diff = 0;

                    var basic = new BasicAuthBehaviour(Username, Password);
                    bool useAuth = !string.IsNullOrEmpty(Username);
                    if (useAuth)
                    {
                        Client.Endpoint.Behaviors.Add(basic);
                    }
                    try
                    {
                        //ensure date and time are in sync
                        //add basic auth for compat with some cameras
                        var sdt = Client.GetSystemDateAndTime();
                        var d = sdt.UTCDateTime.Date;
                        var t = sdt.UTCDateTime.Time;

                        var dt = new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, t.Second);
                        diff = (DateTime.UtcNow - dt).TotalSeconds;
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex, "ONVIF time query");
                    }
                    
                    PasswordDigestBehavior digest = new PasswordDigestBehavior(Username, Password, diff);
                    if (_useDigest)
                        Client.Endpoint.Behaviors.Add(digest);

                    var caps = Client.GetCapabilities(new[] { CapabilityCategory.All});

                    _mediaClient = new MediaClient(binder, new EndpointAddress(GetEndPointUri(serviceAddress.Uri, caps.Media.XAddr, caps)));

                    _mediaClient.Endpoint.Behaviors.Add(basic);
                    if (_useDigest)
                        _mediaClient.Endpoint.Behaviors.Add(digest);

                    if (caps.PTZ != null)
                    {
                        PTZClient = new PTZClient(binder,
                            new EndpointAddress(GetEndPointUri(serviceAddress.Uri, caps.PTZ.XAddr, caps)));
                        PTZClient.Endpoint.Behaviors.Add(basic);
                        if (_useDigest)
                            PTZClient.Endpoint.Behaviors.Add(digest);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex, "Onvif auth");
                }
                return _mediaClient;
            }
        }

        private Uri GetEndPointUri(Uri deviceUri, string xAddr, DeviceClient.Capabilities caps)
        {
            if (string.IsNullOrEmpty(xAddr))
                return deviceUri;

            var url = new Uri(xAddr, UriKind.RelativeOrAbsolute);

            if (!url.IsAbsoluteUri)
                return new Uri(deviceUri, url);

            if (deviceUri.Host != url.Host)
            {
                if (url.HostNameType == UriHostNameType.IPv4)
                {
                    var internalDeviceUrl = new Uri(caps.Device.XAddr);
                    if (internalDeviceUrl.Host == url.Host)
                    {
                        if (internalDeviceUrl.Port == url.Port && internalDeviceUrl.Scheme == url.Scheme)
                        {
                            return Relocate(url, deviceUri.Host, deviceUri.Port);
                        }
                        return Relocate(url, deviceUri.Host);
                    }
                    
                }
            }
            return url;
        }

        private static Uri Relocate(Uri uri, string host)
        {
            if (!uri.IsAbsoluteUri)
            {
                return uri;
            }
            var ub = new UriBuilder(uri)
                     {
                         UserName = uri.UserInfo,
                         Host = host
                     };
            return ub.Uri;
        }
        private static Uri Relocate(Uri uri, string host, int port)
        {
            if (!uri.IsAbsoluteUri)
            {
                return uri;
            }
            var ub = new UriBuilder(uri)
                     {
                         UserName = uri.UserInfo,
                         Host = host,
                         Port = port
                     };
            return ub.Uri;
        }
    }
}
