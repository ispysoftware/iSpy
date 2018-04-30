using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using iSpyApplication.Utilities;

namespace iSpyApplication.CameraDiscovery
{
    public class URLDiscovery
    {
        private int _httpPort = -1;
        private int _mediaPort = -1;
        public Uri BaseUri;

        public URLDiscovery(Uri baseUri)
        {
            BaseUri = baseUri;

            if (baseUri.Port > 0)
            {
                _httpPorts.Insert(0, baseUri.Port);
                _httpPorts = _httpPorts.Distinct().ToList();
            }

        }

        private readonly List<int> _httpPorts = new List<int> { 80, 8080, 443 };
        private readonly List<int> _mediaPorts = new List<int> { 554, 555, 8554, 1935, 37777 };

        public int HttpPort
        {
            get
            {
                if (_httpPort != -1)
                    return _httpPort;

                _httpPort = 80;

                foreach (var p in _httpPorts)
                {
                    if (TestPort(p))
                    {
                        _httpPort = p;
                        break;
                    }
                }

                return _httpPort;
            }
        }

        public int MediaPort
        {
            get
            {
                if (_mediaPort != -1)
                    return _mediaPort;

                _mediaPort = 554;

                foreach (var p in _mediaPorts)
                {
                    if (TestPort(p))
                    {
                        _mediaPort = p;
                        break;
                    }
                }

                return _mediaPort;
            }
        }

        private readonly Dictionary<int, bool> _testedPorts = new Dictionary<int, bool>();
        private bool TestPort(int port)
        {
            if (_testedPorts.ContainsKey(port))
                return _testedPorts[port];

            var b = TestSocket(BaseUri.SetPort(port));
            _testedPorts.Add(port, b);
            return b;
        }

        private int GetPort(ManufacturersManufacturerUrl s)
        {
            if (s.prefix.ToLowerInvariant().StartsWith("http"))
            {
                if (s.portSpecified && s.port != HttpPort)
                {
                    if (TestPort(s.port))
                        return s.port;

                }
                return HttpPort;
            }

            if (s.portSpecified && s.port != MediaPort)
            {
                if (TestPort(s.port))
                    return s.port;

            }

            return MediaPort;
        }

        public Uri GetAddr(ManufacturersManufacturerUrl s, int channel, string username, string password, bool audio = false)
        {
            string urlStart = s.prefix;
            username = username ?? "";
            password = password ?? "";

            if (!string.IsNullOrEmpty(username) && s.url.IndexOf("[TOKEN]", StringComparison.Ordinal) == -1)
            {
                urlStart += Uri.EscapeDataString(username);

                if (!string.IsNullOrEmpty(password))
                    urlStart += ":" + Uri.EscapeDataString(password);
                else
                    urlStart += ":";
                urlStart += "@";

            }

            string url = !audio ? s.url : s.AudioURL;
            if (!url.StartsWith("/"))
                url = "/" + url;


            url = url.Replace("[USERNAME]", Uri.EscapeDataString(username)).Replace("[PASSWORD]", Uri.EscapeDataString(password));
            url = url.Replace("[CHANNEL]", channel.ToString(CultureInfo.InvariantCulture).Trim());
            //defaults:
            url = url.Replace("[WIDTH]", "320");
            url = url.Replace("[HEIGHT]", "240");

            if (url.IndexOf("[AUTH]", StringComparison.Ordinal) != -1)
            {
                string credentials = $"{username}:{password}";
                byte[] bytes = Encoding.ASCII.GetBytes(credentials);
                url = url.Replace("[AUTH]", Convert.ToBase64String(bytes));
            }

            var connectUrl = urlStart + BaseUri.DnsSafeHost + ":" + GetPort(s);
            connectUrl += url;

            Uri uri = null;
            Uri.TryCreate(connectUrl, UriKind.Absolute, out uri);
            return uri;
        }


        private bool TestHttpUrl(Uri source, string cookies, string username, string password)
        {
            bool b = false;

            ConnectionFactory connectionFactory = new ConnectionFactory();
            HttpWebRequest _req;
            using (
                var res = connectionFactory.GetResponse(source.ToString(), cookies, "", "", username, password, "GET", "", "", false, out _req))
            {
                var sc = res?.StatusCode;
                if (sc == HttpStatusCode.OK)
                {
                    string ct = res.ContentType.ToLower();
                    if (ct.IndexOf("text", StringComparison.Ordinal) == -1)
                    {
                        b = true;
                    }
                }
            }

            return b;
        }

        private bool TestRtspUrl(Uri uri, string username, string password)
        {
            try
            {

                var request = "OPTIONS " + uri + " RTSP/1.0\r\n" +
                    "CSeq: 1\r\n" +
                    "User-Agent: iSpy\r\n" +
                    "Accept: */*\r\n";

                if (!string.IsNullOrEmpty(username))
                {
                    var authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(username + ":" + password));
                    request += "Authorization: Basic " + authInfo + "\r\n";
                }

                request += "\r\n";

                IPAddress host = IPAddress.Parse(uri.DnsSafeHost);

                var hostep = new IPEndPoint(host, uri.Port);

                using (
                    var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                    {
                        ReceiveTimeout = 2000
                    })
                {
                    sock.Connect(hostep);

                    var response = sock.Send(Encoding.UTF8.GetBytes(request));
                    if (response > 0)
                    {
                        var bytesReceived = new byte[200];
                        var bytes = sock.Receive(bytesReceived, bytesReceived.Length, 0);
                        string resp = Encoding.ASCII.GetString(bytesReceived, 0, bytes);
                        if (resp.IndexOf("200 OK", StringComparison.Ordinal) != -1)
                        {
                            return true;
                        }
                    }
                }

            }
            catch
            {
                // ignored
            }

            return false;
        }

        public bool TestSocket(Uri uri)
        {
            try
            {
                using (var tcpClient = new TcpClient())
                {
                    tcpClient.ReceiveTimeout = 2000;
                    tcpClient.Connect(uri.Host, uri.Port);
                    tcpClient.GetStream();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool TestAddress(Uri addr, ManufacturersManufacturerUrl u, string username, string password)
        {
            if (!TestPort(addr.Port))
                return false;

            bool found;
            switch (u.prefix.ToLowerInvariant())
            {
                case "http://":
                case "https://":
                    found = TestHttpUrl(addr, "", username, password);
                    break;
                case "rtsp://":
                    found = TestRtspUrl(addr, username, password);
                    break;
                default:
                    found = true;
                    break;
            }
            return found;
        }

    }
}
