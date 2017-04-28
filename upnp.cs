using System;
using System.Threading;
using iSpyApplication.Utilities;
using NATUPNPLib;

namespace iSpyApplication
{
    public static class NATControl
    {
        public static UPnPNAT NAT = new UPnPNAT();
        private static IStaticPortMappingCollection _mappings;

        public static IStaticPortMappingCollection Mappings
        {
            get
            {
                if (_mappings==null)
                {
                    //looking at NATEventManager seems to populate the collection... :S
                    try
                    {
                        if (NAT.NATEventManager != null)
                            _mappings = NAT.StaticPortMappingCollection;
                    }
                    catch
                    {
                        // ignored
                    }
                }
                
                return _mappings;
            }
        }

        public static bool SetPorts(int wanPort, int lanPort)
        {
            bool b = false;
            int i = 3;
            while (Mappings == null && i > 0)
            {
                Thread.Sleep(2000);
                i--;
            }

            if (Mappings != null)
            {
                try
                {
                    Mappings.Remove(wanPort, "TCP");
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
                try
                {
                    Mappings.Add(wanPort, "TCP", lanPort, MainForm.AddressIPv4, true, "iSpy");
                    b = true;
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }

            return b;
        }
    }
}