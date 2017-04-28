using System;
using System.Globalization;
using System.Security.Cryptography;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;

namespace iSpyApplication.Onvif
{
    public class PasswordDigestMessageInspector : IClientMessageInspector
    {
        private const string Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
        private const string XMLAuth= "<UsernameToken xmlns=\"{4}\"><Username>{0}</Username><Password Type=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordDigest\">{1}</Password><Nonce EncodingType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary\">{2}</Nonce><Created xmlns=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\">{3}</Created></UsernameToken>";
        public string Username { get; set; }
        public string Password { get; set; }
        private readonly double _timeOffset;

        public PasswordDigestMessageInspector(string username, string password, double timeOffset)
        {
            Username = username;
            Password = password;
            _timeOffset = timeOffset;
        }

        #region IClientMessageInspector Members

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
        }

        public object BeforeSendRequest(ref Message request, System.ServiceModel.IClientChannel channel)
        {
            string created = DateTime.UtcNow.AddSeconds(0 - _timeOffset)
                .ToString("yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture) + "Z";

            byte[] b = new byte[20];
            new Random().NextBytes(b);

            byte[] digestbuf = new byte[b.Length + Encoding.UTF8.GetByteCount(created + Password)];
            b.CopyTo(digestbuf, 0);
            Encoding.UTF8.GetBytes(created + Password).CopyTo(digestbuf, b.Length);

            SHA1 sha1 = SHA1.Create();
            string digest = Convert.ToBase64String(sha1.ComputeHash(digestbuf));

            string xml = string.Format(XMLAuth, Username, digest, Convert.ToBase64String(b), created, Namespace);
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


            // complete
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
