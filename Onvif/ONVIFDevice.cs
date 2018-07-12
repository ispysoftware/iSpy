using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text.RegularExpressions;
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
        public ONVIFDevice(string url, string username, string password, int rtspPort = 0)
        {
            Uri uri;
            if (!Uri.TryCreate(url, UriKind.Absolute, out uri))
                throw new ApplicationException("Uri: " + url + " not recognised.");

            _svcURL = url;//uri.Scheme + "://" + uri.Authority + "/onvif/device_service";

            Username = username;
            Password = password;
            URL = uri;
            RtspPort = rtspPort;
        }

        private readonly string _svcURL;
        private MediaEndpoint[] _mediaEndpoints;
        private string realm, nonce, qop, opaque, cnonce;
        private int RtspPort;

        public DeviceClient.DeviceClient Client;
        public PTZClient PTZClient;

        public void ResetConnection()
        {
            _mediaEndpoints = null;
            _profiles = null;
            _mediaClient = null;
            _timeOffset = -1;
            nonce = realm = qop = opaque = cnonce = null;
            _auth = OnvifAuthMode.None;

        }
        public MediaEndpoint[] MediaEndpoints
        {
            get
            {
                if (_mediaEndpoints != null)
                    return _mediaEndpoints;

                Connect();
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
        public enum OnvifAuthMode
        {
            None,UsernameToken,Basic,Digest
        }

        private OnvifAuthMode _auth = OnvifAuthMode.None;

        private Profile[] _profiles;

        private string GetDigestHeaderAttribute(string attributeName, string digestAuthHeader)
        {
            var regHeader = new Regex($@"{attributeName}=""([^""]*)""");
            var matchHeader = regHeader.Match(digestAuthHeader);
            if (matchHeader.Success)
                return matchHeader.Groups[1].Value;
            throw new ApplicationException($"Header {attributeName} not found");
        }

        

        private void Connect()
        {
            while (true)
            {
                try
                {
                    try
                    {
                        var profiles = MediaClient.GetProfiles().ToArray();                        
                        var streamSetup = new StreamSetup
                        {
                            Stream = StreamType.RTPUnicast,
                            Transport = new Transport { Protocol = TransportProtocol.RTSP }
                        };
                        List<MediaEndpoint> uris = new List<MediaEndpoint>();

                        foreach (var p in profiles)
                        {
                            
                            var l = MediaClient.GetStreamUri(streamSetup, p.token);
                            //make sure using correct ip address (for external access)
                            var u = new UriBuilder(l.Uri) { Host = URL.Host };
                            if (RtspPort > 0)
                                u.Port = RtspPort;

                            l.Uri = u.ToString();
                            if (!string.IsNullOrEmpty(Username))
                            {
                                l.Uri = l.Uri.ReplaceFirst("://","://" + Uri.EscapeDataString(Username) + ":" +Uri.EscapeDataString(Password) + "@");
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
                        _mediaEndpoints = uris.ToArray();
                        _profiles = profiles;
                    }
                    catch (ProtocolException pex)
                    {
                        var wex = pex.InnerException as WebException;
                        if (wex != null)
                            throw wex;
                    }

                    break;
                }
                catch (Exception ex)
                {
                    _mediaClient = null;
                    if (!(ex is TimeoutException))
                    {
                        var wex = ex as WebException;
                        var wwwAuthenticateHeader = wex?.Response?.Headers["WWW-Authenticate"];
                        if (!string.IsNullOrEmpty(wwwAuthenticateHeader) && _auth != OnvifAuthMode.Digest)
                        {
                            try
                            {
                                realm = GetDigestHeaderAttribute("realm", wwwAuthenticateHeader);
                                nonce = GetDigestHeaderAttribute("nonce", wwwAuthenticateHeader);
                                qop = GetDigestHeaderAttribute("qop", wwwAuthenticateHeader);
                                opaque = GetDigestHeaderAttribute("opaque", wwwAuthenticateHeader);
                                cnonce = new Random().Next(123400, 9999999).ToString();
                                if (!string.IsNullOrEmpty(nonce))
                                {
                                    _auth = OnvifAuthMode.Digest;
                                    continue;
                                }
                            }
                            catch (ApplicationException aex)
                            {
                                //not a digest request
                            }

                        }

                        switch (_auth)
                        {
                            case OnvifAuthMode.None:
                                _auth = OnvifAuthMode.Basic;
                                continue;
                            case OnvifAuthMode.Basic:
                                _auth = OnvifAuthMode.UsernameToken;
                                continue;
                            case OnvifAuthMode.UsernameToken:
                                if (!string.IsNullOrEmpty(realm))
                                {
                                    _auth = OnvifAuthMode.Digest;
                                    continue;
                                }

                                break;
                        }
                    }

                    _auth = OnvifAuthMode.None;
                    Logger.LogException(ex,"Onvif Auth");
                    break;
                }
            }
        }
        public Profile[] Profiles
        {
            get
            {
                if (_profiles != null)
                    return _profiles;

                Connect();
                return _profiles;
            }
        }
        public string Username { get; set; }
        public string Password { get; set; }
        public Uri URL { get; set; }

        private MediaClient _mediaClient;
        private double _timeOffset = -1;
        private MediaClient MediaClient
        {
            get
            {
                if (_mediaClient != null)
                {
                    return _mediaClient;
                }

                EndpointAddress serviceAddress = new EndpointAddress(_svcURL);

                var httpBinding = new HttpTransportBindingElement
                                     {
                                         AuthenticationScheme = AuthenticationSchemes.Digest
                                     };

                var messageElement = new TextMessageEncodingBindingElement
                                     {
                                         MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None)
                                     };

                CustomBinding binder = new CustomBinding(messageElement, httpBinding);
                Client = new DeviceClient.DeviceClient(binder, serviceAddress);
                binder.SendTimeout = binder.CloseTimeout = binder.ReceiveTimeout = binder.OpenTimeout = TimeSpan.FromSeconds(8);

                AuthBehavior behaviour = GenerateBehaviour();
                Client.Endpoint.Behaviors.Add(behaviour);
                if (_timeOffset < 0)
                {
                    try
                    {
                        //ensure date and time are in sync
                        //add basic auth for compat with some cameras
                        var sdt = Client.GetSystemDateAndTime();
                        var d = sdt.UTCDateTime.Date;
                        var t = sdt.UTCDateTime.Time;

                        var dt = new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, t.Second);
                        _timeOffset = behaviour.TimeOffset = (DateTime.UtcNow - dt).TotalSeconds;
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex, "ONVIF time query");
                    }
                }

                var caps = Client.GetCapabilities(new[] {CapabilityCategory.All});
                var ep = new EndpointAddress(GetEndPointUri(serviceAddress.Uri, caps.Media.XAddr, caps));
                _mediaClient = new MediaClient(binder, ep);
                _mediaClient.Endpoint.Behaviors.Add(GenerateBehaviour(ep));

                if (caps.PTZ != null)
                {
                    ep = new EndpointAddress(GetEndPointUri(serviceAddress.Uri, caps.PTZ.XAddr, caps));
                    PTZClient = new PTZClient(binder, ep );
                    PTZClient.Endpoint.Behaviors.Add(GenerateBehaviour(ep));
                }
                return _mediaClient;
            }
        }

        private AuthBehavior GenerateBehaviour(EndpointAddress ep = null)
        {
            return new AuthBehavior(Username, Password, _timeOffset, ep)
                   {
                       nonce = nonce,
                       realm = realm,
                       qop = qop,
                       opaque = opaque,
                       cnonce = cnonce,
                       AuthMode = _auth
                   };
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
