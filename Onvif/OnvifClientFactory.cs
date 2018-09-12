using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using iSpyApplication.Onvif.Behaviour;
using iSpyApplication.Onvif.Security;

namespace iSpyApplication.Onvif
{
    class OnvifClientFactory : IOnvifClientFactory
    {
        private SecurityToken _securityToken;

        static OnvifClientFactory()
        {
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, policyErrors) => { return true; };
        }

        public TService CreateClient<TService>(Uri uri, IConnectionParameters connectionParameters, MessageVersion messageEncodingVersion, int timeout)
        {
            return CreateClient<TService>(new EndpointAddress(uri), connectionParameters, messageEncodingVersion, timeout);
        }

        public TService CreateClient<TService>(EndpointAddress address, IConnectionParameters connectionParameters, MessageVersion messageEncodingVersion, int timeout)
        {
            CustomBinding binder = CreateCustomBinding(connectionParameters.ConnectionTimeout,
                connectionParameters.Credentials, messageEncodingVersion);

            binder.SendTimeout = binder.CloseTimeout = binder.ReceiveTimeout = binder.OpenTimeout = TimeSpan.FromSeconds(timeout);

            var factory = new ChannelFactory<TService>(binder);

            var clientInspector = new CustomMessageInspector();

            if (_securityToken != null)
                clientInspector.Headers.Add(new DigestSecurityHeader(connectionParameters.Credentials, _securityToken));

            var behavior = new CustomEndpointBehavior(clientInspector);

            factory.Endpoint.Behaviors.Add(behavior);

            if (factory.Credentials != null)
            {
                factory.Credentials.UserName.UserName = connectionParameters.Credentials.UserName;
                factory.Credentials.UserName.Password = connectionParameters.Credentials.Password;
            }

            TService service = factory.CreateChannel(address);
            return service;
        }

        public void SetSecurityToken(SecurityToken token)
        {
            _securityToken = token;
        }

        private static CustomBinding CreateCustomBinding(TimeSpan connectionTimeout, NetworkCredential credentials, MessageVersion messageVersion)
        {
            var binding = new CustomBinding(CreateBindingElements(messageVersion, false, credentials))
            {
                CloseTimeout = connectionTimeout,
                OpenTimeout = connectionTimeout,
                SendTimeout = connectionTimeout,
                ReceiveTimeout = connectionTimeout
            };

            return binding;
        }

        private static IEnumerable<BindingElement> CreateBindingElements(MessageVersion messageVersion, bool useTls, NetworkCredential credentials)
        {
            var encoding = new TextMessageEncodingBindingElement(messageVersion, new UTF8Encoding(false))
            {
                ReaderQuotas = { MaxStringContentLength = int.MaxValue }
            };

            yield return encoding;

            HttpTransportBindingElement transport = CreateTransportBindingElement(useTls);
            transport.MaxReceivedMessageSize = int.MaxValue;
            transport.KeepAliveEnabled = false;
            transport.MaxBufferSize = int.MaxValue;
            transport.ProxyAddress = null;
            transport.BypassProxyOnLocal = true;
            transport.UseDefaultWebProxy = false;
            transport.TransferMode = TransferMode.StreamedResponse;
            transport.AuthenticationScheme = credentials.IsEmpty() ? AuthenticationSchemes.Anonymous : AuthenticationSchemes.Basic;

            yield return transport;
        }

        private static HttpTransportBindingElement CreateTransportBindingElement(bool useTls)
        {
            if (!useTls)
                return new HttpTransportBindingElement();

            var transport = new HttpsTransportBindingElement { RequireClientCertificate = false };
            return transport;
        }
    }
}
