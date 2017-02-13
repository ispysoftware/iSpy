using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace iSpyApplication.Utilities
{
    public static class ConnectionFactory
    {

        private static string CalculateMd5Hash(
            string input)
        {
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = MD5.Create().ComputeHash(inputBytes);
            var sb = new StringBuilder();
            foreach (var b in hash)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        public static string GrabHeaderVar(
            string varName,
            string header)
        {
            var regHeader = new Regex($@"{varName}=""([^""]*)""");
            var matchHeader = regHeader.Match(header);
            if (matchHeader.Success)
                return matchHeader.Groups[1].Value;
            throw new ApplicationException($"Header {varName} not found");
        }

        private static int _nc;

        private static string GetDigestHeader(string url, string username, string password, string realm, string nonce, string cnonce, string qop)
        {
            _nc++;
            var uri = new Uri(url);
            var dir = uri.PathAndQuery;
            var ha1 = CalculateMd5Hash($"{username}:{realm}:{password}");
            var ha2 = CalculateMd5Hash($"{"GET"}:{dir}");
            var digestResponse =
                CalculateMd5Hash($"{ha1}:{nonce}:{_nc:00000000}:{cnonce}:{qop}:{ha2}");

            return $"Digest username=\"{username}\", realm=\"{realm}\", nonce=\"{nonce}\", uri=\"{dir}\", " +
                   $"algorithm=MD5, response=\"{digestResponse}\", qop={qop}, nc={_nc:00000000}, cnonce=\"{cnonce}\"";


        }

        public static HttpWebResponse GetResponse(string source, string method, out HttpWebRequest request)
        {
            var uri = new Uri(source);
            string username = "", password = "";
            if (!string.IsNullOrEmpty(uri.UserInfo))
            {
                var lp = uri.UserInfo.Split(':');
                if (lp.Length > 1)
                {
                    username = lp[0];
                    password = lp[1];
                }
            }
            var co = new ConnectionOptions
            {
                channel = "",
                cookies = "",
                headers = "",
                method = method,
                password = password,
                proxy = null,
                requestTimeout = 5000,
                source = source,
                userAgent = "",
                username = username,
                useSeparateConnectionGroup = false,
                useHttp10 = false

            };
            return GetResponse(co, out request);
        }
        public static HttpWebResponse GetResponse(string source, string cookies, string headers, string userAgent, string username, string password, string method, string channel, bool useHttp10, out HttpWebRequest request)
        {
            var co = new ConnectionOptions
            {
                channel = channel,
                cookies = cookies,
                headers = headers,
                method = method,
                password = password,
                proxy = null,
                requestTimeout = 5000,
                source = source,
                userAgent = userAgent,
                username = username,
                useSeparateConnectionGroup = false,
                useHttp10 = useHttp10
            };
            return GetResponse(co, out request);
        }

        private static readonly List<DigestConfig> Digests = new List<DigestConfig>();

        private static void AddDigest(DigestConfig digest)
        {
            lock (Lock)
            {
                Digests.Add(digest);
            }
        }

        private static DigestConfig GetDigest(string host)
        {
            lock (Lock)
            {
                Digests.RemoveAll(p => p.Created > DateTime.UtcNow.AddHours(-1));
                return Digests.FirstOrDefault(p => p.Host == host);
            }
        }

        private class DigestConfig
        {
            public string Host, Authorization;
            public DateTime Created;

            public DigestConfig(string host, string authorization)
            {
                Host = host;
                Authorization = authorization;
                Created = DateTime.UtcNow;
            }
        }

        private static HttpWebResponse GetResponse(ConnectionOptions co, out HttpWebRequest request)
        {
            request = GetRequest(co);
            HttpWebResponse response = null;

            try
            {
                if (co.data != null)
                {
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(co.data, 0, co.data.Length);
                    }
                }
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                // Try to fix a 401 exception by adding a Authorization header
                if (ex.Response == null || ((HttpWebResponse)ex.Response).StatusCode != HttpStatusCode.Unauthorized)
                    return null;
                response?.Close();
                response = TryDigestRequest(co, ex);
            }
            catch (Exception ex)
            {
                Logger.LogExceptionToFile(ex, "Connection Factory");
                response = null;
            }

            return response;
        }

        public static void BeginGetResponse(ConnectionOptions co, EventHandler successCallback)
        {
            var request = GetRequest(co);
            co.callback += successCallback;


            var myRequestState = new RequestState { Request = request, ConnectionOptions = co };
            try
            {
                if (co.data != null)
                {
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(co.data, 0, co.data.Length);
                    }
                }
                request.BeginGetResponse(FinishRequest, myRequestState);
            }
            catch (Exception ex)
            {
                co.ExecuteCallback(false);
                Logger.LogExceptionToFile(ex, "Connection Factory");
            }

            
        }

        private static void FinishRequest(IAsyncResult result)
        {
            var myRequestState = (RequestState)result.AsyncState;
            WebRequest myWebRequest = myRequestState.Request;
            if (myWebRequest == null)
                return;
            // End the Asynchronous request.
            try
            {
                myRequestState.Response = myWebRequest.EndGetResponse(result);
                myRequestState.Response.Close();
                myRequestState.ConnectionOptions.ExecuteCallback(true);

            }
            catch (WebException ex)
            {
                if (ex.Response == null || ((HttpWebResponse)ex.Response).StatusCode != HttpStatusCode.Unauthorized)
                {
                    Logger.LogExceptionToFile(ex, "Connection Factory");
                    myRequestState.ConnectionOptions.ExecuteCallback(false);
                }
                else
                {
                    myRequestState.Response = TryDigestRequest(myRequestState.ConnectionOptions, ex);
                }
            }
            catch (Exception ex)
            {
                myRequestState.ConnectionOptions.ExecuteCallback(false);
                Logger.LogExceptionToFile(ex, "Connection Factory");
            }
        }

        private static HttpWebResponse TryDigestRequest(ConnectionOptions co, WebException ex)
        {
            HttpWebResponse response;
            try
            {
                var wwwAuthenticateHeader = ex.Response.Headers["WWW-Authenticate"];
                if (wwwAuthenticateHeader == null)
                    return null;
                if (wwwAuthenticateHeader.StartsWith("Basic", true, CultureInfo.InvariantCulture))
                    return null; //already failed

                var realm = GrabHeaderVar("realm", wwwAuthenticateHeader);
                var nonce = GrabHeaderVar("nonce", wwwAuthenticateHeader);
                var qop = GrabHeaderVar("qop", wwwAuthenticateHeader);

                string cnonce = new Random().Next(123400, 9999999).ToString(CultureInfo.InvariantCulture);

                var request = GetRequest(co);

                string authorization = GetDigestHeader(co.source, co.username, co.password, realm, nonce, cnonce, qop);
                request.Headers["Authorization"] = authorization;

                response = (HttpWebResponse)request.GetResponse();
                Uri uri;
                if (Uri.TryCreate(co.source, UriKind.Absolute, out uri))
                {
                    if (uri != null)
                    {
                        AddDigest(new DigestConfig(uri.Host, authorization));
                    }
                }
                co.ExecuteCallback(true);
            }
            catch (ApplicationException)
            {
                //headers missing for digest
                response = null;
                co.ExecuteCallback(false);
            }
            catch (Exception ex2)
            {
                Logger.LogExceptionToFile(ex2, "Digest");
                response = null;
                co.ExecuteCallback(false);
            }
            return response;
        }

        public static HttpWebRequest GetRequest(ConnectionOptions co)
        {
            Uri uri;
            if (Uri.TryCreate(co.source, UriKind.Absolute, out uri))
            {
                if (string.IsNullOrEmpty(co.username))
                {
                    var ui = uri.UserInfo.Split(':');
                    if (ui.Length > 1)
                    {
                        co.username = ui[0];
                        co.password = ui[1];
                    }
                }
            }
            var request = GenerateRequest(co);
            if (uri != null)
            {
                var dc = GetDigest(uri.Host);
                if (dc != null)
                    request.Headers["Authorization"] = dc.Authorization;
            }

            switch (co.method)
            {
                case "PUT":
                case "POST":
                    string postData = "";
                    var i = co.source.IndexOf("?", StringComparison.Ordinal);
                    if (i > -1 && i < co.source.Length)
                        postData = co.source.Substring(i + 1);


                    var encoding = new ASCIIEncoding();

                    byte[] data = encoding.GetBytes(postData);

                    request.Method = co.method;
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = data.Length;
                    co.data = data;

                    break;
            }
            return request;
        }

        private static readonly object Lock = new object();

        private static HttpWebRequest GenerateRequest(ConnectionOptions co)
        {
            var request = (HttpWebRequest)WebRequest.Create(co.source);

            // set user agent
            if (!string.IsNullOrEmpty(co.userAgent))
            {
                request.UserAgent = co.userAgent;
            }

            // set proxy
            if (co.proxy != null)
            {
                request.Proxy = co.proxy;
            }

            if (co.useHttp10)
                request.ProtocolVersion = HttpVersion.Version10;

            // set timeout value for the request
            request.Timeout = request.ServicePoint.ConnectionLeaseTimeout = request.ServicePoint.MaxIdleTime = co.requestTimeout;
            request.AllowAutoRedirect = true;
            request.AllowWriteStreamBuffering = true;
            request.AllowAutoRedirect = true;
            request.KeepAlive = true;
            request.SendChunked = false;

            // set login and password
            if (!string.IsNullOrEmpty(co.username))
                request.Credentials = new NetworkCredential(co.username, co.password);
            // set connection group name
            if (co.useSeparateConnectionGroup)
                request.ConnectionGroupName = request.GetHashCode().ToString(CultureInfo.InvariantCulture);
            // force basic authentication through extra headers if required

            var authInfo = "";
            if (!string.IsNullOrEmpty(co.username))
            {
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(co.username + ":" + co.password));
                request.Headers["Authorization"] = "Basic " + authInfo;
            }


            if (!string.IsNullOrEmpty(co.cookies))
            {
                co.cookies = co.cookies.Replace("[AUTH]", authInfo);
                co.cookies = co.cookies.Replace("[USERNAME]", co.username);
                co.cookies = co.cookies.Replace("[PASSWORD]", co.password);
                co.cookies = co.cookies.Replace("[CHANNEL]", co.channel);
                var myContainer = new CookieContainer();
                string[] coll = co.cookies.Split(';');
                foreach (var ckie in coll)
                {
                    if (!string.IsNullOrEmpty(ckie))
                    {
                        string[] nv = ckie.Split('=');
                        if (nv.Length == 2)
                        {
                            var cookie = new Cookie(nv[0].Trim(), nv[1].Trim());
                            myContainer.Add(new Uri(request.RequestUri.ToString()), cookie);
                        }
                    }
                }
                request.CookieContainer = myContainer;
            }

            if (!string.IsNullOrEmpty(co.headers))
            {
                co.headers = co.headers.Replace("[AUTH]", authInfo);
                string[] coll = co.headers.Split(';');
                foreach (var hdr in coll)
                {
                    if (!string.IsNullOrEmpty(hdr))
                    {
                        string[] nv = hdr.Split('=');
                        if (nv.Length == 2)
                        {
                            request.Headers.Add(nv[0], nv[1]);
                        }
                    }
                }
            }
            return request;
        }
    }
}