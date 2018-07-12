using System;
using System.Globalization;
using System.Runtime.Remoting.Channels;
using System.Security.Cryptography;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;

namespace iSpyApplication.Onvif
{
    public class AuthMessageInspector : IClientMessageInspector
    {
        private const string Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
        private const string XMLAuth = "<UsernameToken xmlns=\"{4}\"><Username>{0}</Username><Password Type=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordDigest\">{1}</Password><Nonce EncodingType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary\">{2}</Nonce><Created xmlns=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\">{3}</Created></UsernameToken>";
        private const string DigestAuth = "Digest username=\"{0}\",realm=\"{1}\",nonce=\"{2}\",uri=\"{3}\",cnonce=\"{4}\",nc={5:00000000},qop={6},response=\"{7}\",opaque=\"{8}\"";

        public string Username { get; set; }
        public string Password { get; set; }
        public ONVIFDevice.OnvifAuthMode AuthMode;

        public string realm, nonce, qop, opaque, cnonce;
        public double TimeOffset;
        private int counter = 0;
        public Uri uri;

        public AuthMessageInspector(string username, string password, double timeOffset, ONVIFDevice.OnvifAuthMode authMode)
        {
            Username = username;
            Password = password;
            TimeOffset = timeOffset;
            AuthMode = authMode;
        }

        #region IClientMessageInspector Members

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
        }
        private string ComputeMd5Hash(string input)
        {
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = MD5.Create().ComputeHash(inputBytes);
            var sb = new StringBuilder();
            foreach (var b in hash)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
        public object BeforeSendRequest(ref Message request, System.ServiceModel.IClientChannel channel)
        {
            switch (AuthMode)
            {
                case ONVIFDevice.OnvifAuthMode.Basic:
                    string encoded = Convert.ToBase64String((Username + ":" + Password).ToUtf8());
                    var httpRequestMessage = new HttpRequestMessageProperty();
                    httpRequestMessage.Headers.Add("Authorization", "Basic " + encoded);
                    request.Properties.Add(HttpRequestMessageProperty.Name, httpRequestMessage);

                    break;
                case ONVIFDevice.OnvifAuthMode.UsernameToken:
                {
                    string created = DateTime.UtcNow.AddSeconds(0 - TimeOffset)
                                         .ToString("yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture) + "Z";

                    byte[] b = new byte[20];
                    new Random().NextBytes(b);

                    byte[] digestbuf = new byte[b.Length + Encoding.UTF8.GetByteCount(created + Password)];
                    b.CopyTo(digestbuf, 0);
                    Encoding.UTF8.GetBytes(created + Password).CopyTo(digestbuf, b.Length);

                    SHA1 sha1 = SHA1.Create();
                    string digest = Convert.ToBase64String(sha1.ComputeHash(digestbuf));

                    string xml = string.Format(XMLAuth, Username, digest, Convert.ToBase64String(b), created,
                        Namespace);
                    var token = GetElement(xml);

                    MessageHeader securityHeader = MessageHeader.CreateHeader("Security", Namespace, token, true);

                    request.Headers.Add(securityHeader);
                    int limit = request.Headers.Count;
                    for (int i = 0; i < limit; ++i)
                    {
                        //remove the debugger xml packet
                        if (request.Headers[i].Name.Equals("VsDebuggerCausalityData"))
                        {
                            request.Headers.RemoveAt(i);
                            break;
                        }
                    }

                }
                    break;
                case ONVIFDevice.OnvifAuthMode.Digest:
                {
                    string ha1, ha2;
                    ha1 = ComputeMd5Hash($"{Username}:{realm}:{Password}");
                    ha2 = ComputeMd5Hash($"POST:{uri.PathAndQuery}");
                    var digestResponse = ComputeMd5Hash($"{ha1}:{nonce}:{counter:00000000}:{cnonce}:{qop}:{ha2}");
                    string digestHeader = string.Format(DigestAuth, Username, realm, nonce, uri.AbsolutePath, cnonce, counter, qop, digestResponse, opaque);
                    HttpRequestMessageProperty httpRequest = new HttpRequestMessageProperty();
                    httpRequest.Headers.Add("Authorization", digestHeader);
                    request.Properties.Add(HttpRequestMessageProperty.Name, httpRequest);
                    counter++;
                }
                    break;
            }
            return Convert.DBNull;
        }

        private static XmlElement GetElement(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            return doc.DocumentElement;
        }
        #endregion
    }
}
