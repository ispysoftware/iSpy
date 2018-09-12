using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using iSpyApplication.Onvif.Security;

namespace iSpyApplication.Onvif
{
    internal interface IOnvifClientFactory
    {
        TService CreateClient<TService>(Uri uri, IConnectionParameters connectionParameters, MessageVersion messageEncodingVersion, int timeout);
        TService CreateClient<TService>(EndpointAddress address, IConnectionParameters connectionParameters, MessageVersion messageEncodingVersion, int timeout);
        void SetSecurityToken(SecurityToken token);
    }

    public interface IConnectionParameters
    {
        Uri ConnectionUri { get; }

        NetworkCredential Credentials { get; }

        TimeSpan ConnectionTimeout { get; }
    }
}
