using System;
using NetFwTypeLib;

namespace iSpyApplication
{
    class FireWall
    {
        public enum FwErrorCode
        {
            FwNoerror = 0,
            FwErrInitialized,					// Already initialized or doesn't call Initialize()
            FwErrCreateSettingManager,		// Can't create an instance of the firewall settings manager
            FwErrLocalPolicy,				// Can't get local firewall policy
            FwErrProfile,						// Can't get the firewall profile
            FwErrFirewallIsEnabled,			// Can't get the firewall enable information
            FwErrFirewallEnabled,			// Can't set the firewall enable option
            FwErrInvalidArg,					// Invalid Arguments
            FwErrAuthApplications,			// Failed to get authorized application list
            FwErrAppEnabled,					// Failed to get the application is enabled or not
            FwErrCreateAppInstance,			// Failed to create an instance of an authorized application
            FwErrSysAllocString,			// Failed to alloc a memory for BSTR
            FwErrPutProcessImageName,		// Failed to put Process Image File Name to Authorized Application
            FwErrPutRegisterName,			// Failed to put a registered name
            FwErrAddToCollection,			// Failed to add to the Firewall collection
            FwErrRemoveFromCollection,		// Failed to remove from the Firewall collection
            FwErrGlobalOpenPorts,			// Failed to retrieve the globally open ports
            FwErrPortIsEnabled,				// Can't get the firewall port enable information
            FwErrPortEnabled,				// Can't set the firewall port enable option
            FwErrCreatePortInstance,		// Failed to create an instance of an authorized port
            FwErrSetPortNumber,				// Failed to set port number
            FwErrSetIPProtocol,				// Failed to set IP Protocol
            FwErrExceptionNotAllowed,		// Failed to get or put the exception not allowed
            FwErrNotificationDisabled,		// Failed to get or put the notification disabled
            FwErrUnicastMulticast,			// Failed to get or put the UnicastResponses To MulticastBroadcast Disabled Property 
            FwErrApplicationItem,            // Failed to returns the specified application if it is in the collection.
            FwErrSamePortExist,             // The port which you try to add is already existed.
            FwErrUnknown,                     // Unknown Error or Exception occured
        };

        INetFwProfile _mFirewallProfile;

        public FwErrorCode Initialize()
        {
            if (_mFirewallProfile != null)
                return FwErrorCode.FwErrInitialized;

            Type typFwMgr = Type.GetTypeFromCLSID(new Guid("{304CE942-6E39-40D8-943A-B913C40C9CD4}"));
            var fwMgr = (INetFwMgr)Activator.CreateInstance(typFwMgr);

            INetFwPolicy fwPolicy = fwMgr.LocalPolicy;
            if (fwPolicy == null)
                return FwErrorCode.FwErrLocalPolicy;

            try
            {
                _mFirewallProfile = fwPolicy.GetProfileByType(fwMgr.CurrentProfileType);
            }
            catch
            {
                return FwErrorCode.FwErrProfile;
            }

            return FwErrorCode.FwNoerror;
        }

        public FwErrorCode Uninitialize()
        {
            _mFirewallProfile = null;
            return FwErrorCode.FwNoerror;
        }

        public FwErrorCode IsWindowsFirewallOn(out bool bOn)
        {
            bOn = false;

            if (_mFirewallProfile == null)
                return FwErrorCode.FwErrInitialized;

            bOn = _mFirewallProfile.FirewallEnabled;

            return FwErrorCode.FwNoerror;
        }

        public FwErrorCode TurnOnWindowsFirewall()
        {
            if (_mFirewallProfile == null)
                return FwErrorCode.FwErrInitialized;

            // Check whether the firewall is off
            bool bFwOn;
            FwErrorCode ret = IsWindowsFirewallOn(out bFwOn);
            if (ret != FwErrorCode.FwNoerror)
                return ret;

            // If it is off now, turn it on
            if (!bFwOn)
                _mFirewallProfile.FirewallEnabled = true;

            return FwErrorCode.FwNoerror;
        }

        public FwErrorCode TurnOffWindowsFirewall()
        {
            if (_mFirewallProfile == null)
                return FwErrorCode.FwErrInitialized;

            // Check whether the firewall is off
            bool bFwOn;
            FwErrorCode ret = IsWindowsFirewallOn(out bFwOn);

            if (ret != FwErrorCode.FwNoerror)
                return ret;

            // If it is on now, turn it off
            if (bFwOn)
                _mFirewallProfile.FirewallEnabled = false;

            return FwErrorCode.FwNoerror;
        }

        public FwErrorCode IsAppEnabled(string strProcessImageFileName, ref bool bEnable)
        {
            if (_mFirewallProfile == null)
                return FwErrorCode.FwErrInitialized;

            if (strProcessImageFileName.Length == 0)
                return FwErrorCode.FwErrInvalidArg;

            INetFwAuthorizedApplications fwApps = _mFirewallProfile.AuthorizedApplications;
            if (fwApps == null)
                return FwErrorCode.FwErrAuthApplications;

            try
            {
                INetFwAuthorizedApplication fwApp = fwApps.Item(strProcessImageFileName);
                // If FAILED, the appliacation is not in the collection list
                if (fwApp == null)
                    return FwErrorCode.FwErrApplicationItem;

                bEnable = fwApp.Enabled;
            }
            catch (System.IO.FileNotFoundException)
            {
                bEnable = false;
            }

            return FwErrorCode.FwNoerror;
        }

        public FwErrorCode AddApplication(string strProcessImageFileName, string strRegisterName)
        {
            if (_mFirewallProfile == null)
                return FwErrorCode.FwErrInitialized;

            if (strProcessImageFileName.Length == 0 || strRegisterName.Length == 0)
                return FwErrorCode.FwErrInvalidArg;

            // First of all, check the application is already authorized;
            bool bAppEnable = true;
            FwErrorCode nError = IsAppEnabled(strProcessImageFileName, ref bAppEnable);
            if (nError != FwErrorCode.FwNoerror)
                return nError;

            // Only add the application if it isn't authorized
            if (bAppEnable == false)
            {
                // Retrieve the authorized application collection
                INetFwAuthorizedApplications fwApps = _mFirewallProfile.AuthorizedApplications;

                if (fwApps == null)
                    return FwErrorCode.FwErrAuthApplications;

                // Create an instance of an authorized application
                Type typeFwApp = Type.GetTypeFromCLSID(new Guid("{EC9846B3-2762-4A6B-A214-6ACB603462D2}"));

                var fwApp = (INetFwAuthorizedApplication)Activator.CreateInstance(typeFwApp);

                // Set the process image file name
                fwApp.ProcessImageFileName = strProcessImageFileName;
                fwApp.Name = strRegisterName;

                try
                {
                    fwApps.Add(fwApp);
                }
                catch
                {
                    return FwErrorCode.FwErrAddToCollection;
                }

            }

            return FwErrorCode.FwNoerror;
        }

        public FwErrorCode RemoveApplication(string strProcessImageFileName)
        {
            if (_mFirewallProfile == null)
                return FwErrorCode.FwErrInitialized;
            if (strProcessImageFileName.Length == 0)
                return FwErrorCode.FwErrInvalidArg;

            bool bAppEnable = true;
            FwErrorCode nError = IsAppEnabled(strProcessImageFileName, ref bAppEnable);

            if (nError != FwErrorCode.FwNoerror)
                return nError;

            // Only remove the application if it is authorized
            if (bAppEnable)
            {
                // Retrieve the authorized application collection
                INetFwAuthorizedApplications fwApps = _mFirewallProfile.AuthorizedApplications;
                if (fwApps == null)
                    return FwErrorCode.FwErrAuthApplications;

                try
                {
                    fwApps.Remove(strProcessImageFileName);
                }
                catch
                {
                    return FwErrorCode.FwErrRemoveFromCollection;
                }
            }

            return FwErrorCode.FwNoerror;
        }

        public FwErrorCode IsPortEnabled(int nPortNumber, NET_FW_IP_PROTOCOL_ ipProtocol, ref bool bEnable)
        {
            if (_mFirewallProfile == null)
                return FwErrorCode.FwErrInitialized;

            // Retrieve the open ports collection
            INetFwOpenPorts fwOpenPorts = _mFirewallProfile.GloballyOpenPorts;
            if (fwOpenPorts == null)
                return FwErrorCode.FwErrGlobalOpenPorts;

            // Get the open port
            try
            {
                INetFwOpenPort fwOpenPort = fwOpenPorts.Item(nPortNumber, ipProtocol);
                bEnable = fwOpenPort != null && fwOpenPort.Enabled;
            }
            catch (System.IO.FileNotFoundException)
            {
                bEnable = false;
            }

            return FwErrorCode.FwNoerror;
        }

        public FwErrorCode AddPort(int nPortNumber, NET_FW_IP_PROTOCOL_ ipProtocol, string strRegisterName)
        {
            if (_mFirewallProfile == null)
                return FwErrorCode.FwErrInitialized;

            bool bEnablePort = true;
            FwErrorCode nError = IsPortEnabled(nPortNumber, ipProtocol, ref bEnablePort);
            if (nError != FwErrorCode.FwNoerror)
                return nError;

            // Only add the port, if it isn't added to the collection
            if (bEnablePort == false)
            {
                // Retrieve the collection of globally open ports
                INetFwOpenPorts fwOpenPorts = _mFirewallProfile.GloballyOpenPorts;
                if (fwOpenPorts == null)
                    return FwErrorCode.FwErrGlobalOpenPorts;

                // Create an instance of an open port
                Type typeFwPort = Type.GetTypeFromCLSID(new Guid("{0CA545C6-37AD-4A6C-BF92-9F7610067EF5}"));
                var fwOpenPort = (INetFwOpenPort)Activator.CreateInstance(typeFwPort);

                // Set the port number
                fwOpenPort.Port = nPortNumber;

                // Set the IP Protocol
                fwOpenPort.Protocol = ipProtocol;

                // Set the registered name
                fwOpenPort.Name = strRegisterName;

                try
                {
                    fwOpenPorts.Add(fwOpenPort);
                }
                catch
                {
                    return FwErrorCode.FwErrAddToCollection;
                }
            }
            else
                return FwErrorCode.FwErrSamePortExist;

            return FwErrorCode.FwNoerror;
        }

        public FwErrorCode RemovePort(int nPortNumber, NET_FW_IP_PROTOCOL_ ipProtocol)
        {
            if (_mFirewallProfile == null)
                return FwErrorCode.FwErrInitialized;

            bool bEnablePort = false;
            FwErrorCode nError = IsPortEnabled(nPortNumber, ipProtocol, ref bEnablePort);
            if (nError != FwErrorCode.FwNoerror)
                return nError;

            // Only remove the port, if it is on the collection
            if (bEnablePort)
            {
                // Retrieve the collection of globally open ports
                INetFwOpenPorts fwOpenPorts = _mFirewallProfile.GloballyOpenPorts;
                if (fwOpenPorts == null)
                    return FwErrorCode.FwErrGlobalOpenPorts;

                try
                {
                    fwOpenPorts.Remove(nPortNumber, ipProtocol);
                }
                catch
                {
                    return FwErrorCode.FwErrRemoveFromCollection;
                }
            }

            return FwErrorCode.FwNoerror;
        }

        public FwErrorCode IsExceptionNotAllowed(ref bool bNotAllowed)
        {
            if (_mFirewallProfile == null)
                return FwErrorCode.FwErrInitialized;
            bNotAllowed = _mFirewallProfile.ExceptionsNotAllowed;

            return FwErrorCode.FwNoerror;
        }

        public FwErrorCode SetExceptionNotAllowed(bool bNotAllowed)
        {
            if (_mFirewallProfile == null)
                return FwErrorCode.FwErrInitialized;

            _mFirewallProfile.ExceptionsNotAllowed = bNotAllowed;

            return FwErrorCode.FwNoerror;
        }

        public FwErrorCode IsNotificationDiabled(ref bool bDisabled)
        {
            if (_mFirewallProfile == null)
                return FwErrorCode.FwErrInitialized;

            bDisabled = _mFirewallProfile.NotificationsDisabled;

            return FwErrorCode.FwNoerror;
        }

        public FwErrorCode SetNotificationDiabled(bool bDisabled)
        {
            if (_mFirewallProfile == null)
                return FwErrorCode.FwErrInitialized;

            _mFirewallProfile.NotificationsDisabled = bDisabled;

            return FwErrorCode.FwNoerror;
        }

        public FwErrorCode IsUnicastResponsesToMulticastBroadcastDisabled(ref bool bDisabled)
        {
            if (_mFirewallProfile == null)
                return FwErrorCode.FwErrInitialized;

            bDisabled = _mFirewallProfile.UnicastResponsesToMulticastBroadcastDisabled;

            return FwErrorCode.FwNoerror;
        }

        public FwErrorCode SetUnicastResponsesToMulticastBroadcastDisabled(bool bDisabled)
        {
            if (_mFirewallProfile == null)
                return FwErrorCode.FwErrInitialized;

            _mFirewallProfile.UnicastResponsesToMulticastBroadcastDisabled = bDisabled;

            return FwErrorCode.FwNoerror;
        }
    }
}
