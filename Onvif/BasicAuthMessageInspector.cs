using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace iSpyApplication.Onvif
{
    public class BasicAuthMessageInspector : IClientMessageInspector
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public BasicAuthMessageInspector(string username, string password)
        {
            Username = username;
            Password = password;
        }

        #region IClientMessageInspector Members

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
        }

        public object BeforeSendRequest(ref Message request, System.ServiceModel.IClientChannel channel)
        {
            string encoded = Convert.ToBase64String((Username + ":" + Password).ToUtf8());
            var httpRequestMessage = new HttpRequestMessageProperty();
            httpRequestMessage.Headers.Add("Authorization", "Basic " + encoded);
            request.Properties.Add(HttpRequestMessageProperty.Name, httpRequestMessage);

            return Convert.DBNull;
        }
        #endregion
    }
}
