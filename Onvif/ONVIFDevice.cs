using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using iSpyApplication.DeviceClient;
using iSpyApplication.DeviceMedia;
using iSpyApplication.DevicePTZ;
using iSpyApplication.Utilities;
using DateTime = System.DateTime;
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
            URL = url;

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
                        foreach (var p in Profiles)
                        {
                            try
                            {
                                var l = mc.GetStreamUri(streamSetup, p.token);
                                var s = p?.VideoSourceConfiguration;
                                uris.Add(new MediaEndpoint(l,s));
                            }
                            catch (Exception ex)
                            {
                                Logger.LogExceptionToFile(ex, "Onvif device (1)");
                                break;
                            }
                        }
                        _mediaEndpoints = uris.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Logger.LogExceptionToFile(ex, "Onvif device (2)");
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
            public VideoSourceConfiguration Config;

            public MediaEndpoint(MediaUri uri, VideoSourceConfiguration config)
            {
                URI = uri;
                Config = config;
            }

            public override string ToString()
            {
                return Config.Bounds.width + "x" + Config.Bounds.height + ": " + URI.Uri;
            }
        }


        public void SelectProfile(int profileIndex)
        {
            var p = Profiles;
            if (p != null && profileIndex < p.Length)
            {
                Profile = Profiles[profileIndex];
                Endpoint = MediaEndpoints[profileIndex];
            }
                
        }


        private VideoSourceConfiguration[] _vconfigs;
        public VideoSourceConfiguration[] VideoSourceConfigurations
        {
            get
            {
                if (_vconfigs != null)
                    return _vconfigs;


                var mc = MediaClient;
                if (mc != null)
                {
                    _vconfigs = mc.GetVideoSourceConfigurations().ToArray();
                }
                return _vconfigs;
            }
        }

        private Profile[] _profiles;
        public Profile[] Profiles
        {
            get
            {
                if (_profiles != null)
                    return _profiles;

                var mc = MediaClient;
                if (mc != null)
                {
                    try
                    {
                        _profiles = mc.GetProfiles().ToArray();
                    }
                    catch (Exception ex)
                    {
                        Logger.LogExceptionToFile(ex);
                    }
                }
                return _profiles;
            }
        }
        public string Username { get; set; }
        public string Password { get; set; }
        public string URL { get; set; }

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
                                             MessageVersion =
                                                 MessageVersion.CreateVersion(EnvelopeVersion.Soap12,
                                                     AddressingVersion.None)
                                         };

                    CustomBinding binder = new CustomBinding(messageElement, httpBinding);
                    Client = new DeviceClient.DeviceClient(binder, serviceAddress);
                    binder.SendTimeout = binder.CloseTimeout = binder.ReceiveTimeout = binder.OpenTimeout = TimeSpan.FromSeconds(5);
                    

                    double diff = 0;

                    var basic = new BasicAuthBehaviour(Username, Password);
                    bool useAuth = !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
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
                        Logger.LogExceptionToFile(ex, "ONVIF time query");
                    }
                    PasswordDigestBehavior digest = new PasswordDigestBehavior(Username, Password, diff);
                    Client.Endpoint.Behaviors.Add(digest);

                    var caps = Client.GetCapabilities(new[] { CapabilityCategory.PTZ, CapabilityCategory.Media, CapabilityCategory.Device });
                    _mediaClient = new MediaClient(binder, new EndpointAddress(GetEndPointUri(serviceAddress.Uri, caps.Media.XAddr, caps)));
                    PTZClient = new PTZClient(binder, new EndpointAddress(GetEndPointUri(serviceAddress.Uri, caps.PTZ.XAddr, caps)));


                    _mediaClient.Endpoint.Behaviors.Add(basic);
                    PTZClient.Endpoint.Behaviors.Add(basic);

                    _mediaClient.Endpoint.Behaviors.Add(digest);
                    PTZClient.Endpoint.Behaviors.Add(digest);
                    
                }
                catch (Exception ex)
                {
                    Logger.LogExceptionToFile(ex, "Onvif auth");
                }
                return _mediaClient;
            }
        }

        public Uri GetEndPointUri(Uri deviceUri, string xAddr, DeviceClient.Capabilities caps)
        {
            if (String.IsNullOrEmpty(xAddr))
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

        public static Uri Relocate(Uri uri, string host)
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
        public static Uri Relocate(Uri uri, string host, int port)
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
