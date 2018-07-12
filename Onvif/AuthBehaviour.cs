using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace iSpyApplication.Onvif
{
    public class AuthBehavior : IEndpointBehavior
    {
        public ONVIFDevice.OnvifAuthMode AuthMode;
        public string Username { get; set; }
        public string Password { get; set; }
        
        public AuthMessageInspector AMI;

        public double TimeOffset;
        public string realm, nonce, qop, opaque, cnonce;
        public Uri Uri;

        public AuthBehavior(string username, string password, double timeOffset, EndpointAddress ep = null)
        {
            Username = username;
            Password = password;
            TimeOffset = timeOffset;
            Uri = ep?.Uri;
        }

        #region IEndpointBehavior Members

        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }


        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new AuthMessageInspector(Username, Password, TimeOffset, AuthMode)
                                                {
                                                    realm = realm,
                                                    nonce = nonce,
                                                    qop = qop,
                                                    opaque = opaque,
                                                    cnonce = cnonce,
                                                    uri = Uri
                                                });
        
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {

        }

        public void Validate(ServiceEndpoint endpoint)
        {

        }

        #endregion
    }
}
