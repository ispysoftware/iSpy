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

        private static string GetDigestHeader(string url, string username, string password, string realm, string nonce, string cnonce, string qop )
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

        public static HttpWebResponse GetResponse(string source, bool post, out HttpWebRequest request)
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
            return GetResponse(source, "", "", "", null, false, true, 5000, username, password, post, out request);
        }

        public static HttpWebResponse GetResponse(string source, string cookies, string username, string password, bool post, out HttpWebRequest request)
        {
            return GetResponse(source, cookies, "", "", null, false, true, 5000, username, password, post, out request);
        }

        private static readonly List<DigestConfig> Digests = new List<DigestConfig>();

        public static void AddDigest(DigestConfig digest)
        {
            lock (Lock)
            {
                Digests.Add(digest);
            }
        }

        public static DigestConfig GetDigest(string host)
        {
            lock (Lock)
            {
                Digests.RemoveAll(p => p.Created > DateTime.UtcNow.AddHours(-1));
                return Digests.FirstOrDefault(p => p.Host == host);
            }
        }

        public class DigestConfig
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

        public static HttpWebResponse GetResponse(string source, string cookies, string headers, string userAgent, IWebProxy proxy, bool useHttp10, bool useSeparateConnectionGroup, int requestTimeout, string username, string password, bool post, out HttpWebRequest request)
        {
            request = GetRequest(source, cookies, headers, userAgent, proxy, useHttp10, useSeparateConnectionGroup, requestTimeout, username, password,post);
            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse) request.GetResponse();
            }
            catch (WebException ex)
            {
                // Try to fix a 401 exception by adding a Authorization header
                if (ex.Response == null || ((HttpWebResponse) ex.Response).StatusCode != HttpStatusCode.Unauthorized)
                    return null;
                response?.Close();

                response = TryDigestRequest(source, cookies, headers, userAgent, proxy, useHttp10, useSeparateConnectionGroup, requestTimeout, username, password,post,ex);
            }
            catch
            {
                response = null;
            }

            return response;
        }

        public static HttpWebResponse TryDigestRequest(string source, string cookies, string headers, string userAgent,
            IWebProxy proxy, bool useHttp10, bool useSeparateConnectionGroup, int requestTimeout, string username,
            string password, bool post, WebException ex)
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

                var request = GetRequest(source, cookies, headers, userAgent, proxy, useHttp10,
                    useSeparateConnectionGroup, requestTimeout, username, password, post);

                string authorization = GetDigestHeader(source, username, password, realm, nonce, cnonce, qop);
                request.Headers["Authorization"] = authorization;

                response = (HttpWebResponse) request.GetResponse();
                Uri uri;
                if (Uri.TryCreate(source, UriKind.Absolute, out uri))
                {
                    if (uri != null)
                    {
                        AddDigest(new DigestConfig(uri.Host, authorization));
                    }
                }
            }
            catch (ApplicationException)
            {
                //headers missing for digest
                response = null;
            }
            catch (Exception ex2)
            {
                MainForm.LogExceptionToFile(ex2,"Digest");
                response = null;
            }
            return response;
        }

        public static HttpWebRequest GetRequest(string source, string cookies, string headers, string userAgent, IWebProxy proxy, bool useHttp10, bool useSeparateConnectionGroup, int requestTimeout, string username, string password, bool post)
        {
            Uri uri;
            if (Uri.TryCreate(source, UriKind.Absolute, out uri))
            {
                if (string.IsNullOrEmpty(username))
                {
                    var ui = uri.UserInfo.Split(':');
                    if (ui.Length > 1)
                    {
                        username = ui[0];
                        password = ui[1];
                    }
                }
            }
            var request = GenerateRequest(source, cookies, headers, userAgent, proxy, useHttp10, useSeparateConnectionGroup, requestTimeout, username, password);
            if (uri!=null)
            {
                var dc = GetDigest(uri.Host);
                if (dc != null)
                    request.Headers["Authorization"] = dc.Authorization;
            }

            if (post)
            {

                var i = source.IndexOf("?", StringComparison.Ordinal);
                if (i > -1 && i < source.Length)
                {
                    var encoding = new ASCIIEncoding();
                    string postData = source.Substring(i + 1);
                    byte[] data = encoding.GetBytes(postData);

                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = data.Length;

                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
            }
            return request;
        }

        private static readonly object Lock = new object();

        private static HttpWebRequest GenerateRequest(string source, string cookies, string headers, string userAgent, IWebProxy proxy, bool useHttp10, bool useSeparateConnectionGroup, int requestTimeout, string username, string password)
        {
            var request = (HttpWebRequest)WebRequest.Create(source);

            // set user agent
            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }

            // set proxy
            if (proxy != null)
            {
                request.Proxy = proxy;
            }

            if (useHttp10)
                request.ProtocolVersion = HttpVersion.Version10;

            // set timeout value for the request
            request.Timeout = request.ServicePoint.ConnectionLeaseTimeout = request.ServicePoint.MaxIdleTime = requestTimeout;
            request.AllowAutoRedirect = true;
            request.AllowWriteStreamBuffering = true;
            request.AllowAutoRedirect = true;
            request.KeepAlive = true;
            request.SendChunked = false;

            // set login and password
            if (!String.IsNullOrEmpty(username))
                request.Credentials = new NetworkCredential(username, password);
            // set connection group name
            if (useSeparateConnectionGroup)
                request.ConnectionGroupName = request.GetHashCode().ToString(CultureInfo.InvariantCulture);
            // force basic authentication through extra headers if required

            var authInfo = "";
            if (!string.IsNullOrEmpty(username))
            {
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(username + ":" + password));
                request.Headers["Authorization"] = "Basic " + authInfo;
            }
            

            if (!string.IsNullOrEmpty(cookies))
            {
                cookies = cookies.Replace("[AUTH]", authInfo);
                cookies = cookies.Replace("[USERNAME]", username);
                cookies = cookies.Replace("[PASSWORD]", password);
                var myContainer = new CookieContainer();
                string[] coll = cookies.Split(';');
                foreach (var ckie in coll)
                {
                    if (!String.IsNullOrEmpty(ckie))
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

            if (!String.IsNullOrEmpty(headers))
            {
                headers = headers.Replace("[AUTH]", authInfo);
                string[] coll = headers.Split(';');
                foreach (var hdr in coll)
                {
                    if (!String.IsNullOrEmpty(hdr))
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