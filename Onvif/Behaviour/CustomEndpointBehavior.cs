using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace iSpyApplication.Onvif.Behaviour
{
    class CustomEndpointBehavior : IEndpointBehavior
    {
        private readonly IClientMessageInspector _clientInspector;

        public CustomEndpointBehavior(IClientMessageInspector clientInspector)
        {
            _clientInspector = clientInspector;
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(_clientInspector);
        }
    }
}
