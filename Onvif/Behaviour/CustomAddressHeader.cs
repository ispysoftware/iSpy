using System.ServiceModel.Channels;
using System.Xml;

namespace iSpyApplication.Onvif.Behaviour
{
    class CustomAddressHeader : AddressHeader
    {
        private readonly XmlElement _xmlElement;

        public override string Name { get; }
        public override string Namespace { get; }

        public CustomAddressHeader(XmlElement xmlElement)
        {
            _xmlElement = xmlElement;

            Name = xmlElement.LocalName;
            Namespace = xmlElement.NamespaceURI;
        }

        protected override void OnWriteAddressHeaderContents(XmlDictionaryWriter writer)
        {
            _xmlElement.WriteContentTo(writer);
        }
    }
}