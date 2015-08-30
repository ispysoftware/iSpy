using System;
using System.Net;
using onvif.services;

namespace iSpyApplication
{
    public class DeviceDescriptionHolder
    {
        public Uri[] Uris;
        public string Address;
        public bool IsInvalidUris;
        public NetworkCredential Account;
        public string Name, Location, DeviceIconUri;
        public Profile[] Profiles;
        public string URL;
    }
}
