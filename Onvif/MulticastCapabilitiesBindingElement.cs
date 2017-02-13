using System;
using System.ServiceModel.Channels;

namespace iSpyApplication.Onvif
{
    public class MulticastCapabilitiesBindingElement : BindingElement, IBindingMulticastCapabilities
    {
        private readonly bool _isMulticast;
        public MulticastCapabilitiesBindingElement(bool isMulticast)
        {
            _isMulticast = isMulticast;
        }
        public override T GetProperty<T>(BindingContext context)
        {
            if (typeof(T) == typeof(IBindingMulticastCapabilities))
            {
                return (T)(object)this;
            }
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            return context.GetInnerProperty<T>();
        }
        bool IBindingMulticastCapabilities.IsMulticast
        {
            get { return _isMulticast; }
        }

        public override BindingElement Clone()
        {
            return this;
        }
    }
}