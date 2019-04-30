using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Math;
using iSpyApplication.iSpyWS;
using iSpyApplication.Utilities;

namespace iSpyApplication
{
    public static class WsWrapper
    {
        private static iSpyAPI _wsa;
        private static string _externalIP = "";
        private static bool _websitelive = true;
        internal static DateTime LastLiveCheck = Helper.Now;
        private static readonly int[] PingDelays = { 60, 90, 120, 240 };
        private static int _pingIndex = 0;

        public static iSpyAPI Wsa
        {
            get
            {
                if (_wsa != null)
                    return _wsa;

                _wsa = new iSpyAPI
                    {
                        Url = MainForm.WebserverSecure + "/webservices/ispyapi.asmx",
                        Timeout = 15000,
                    };
                _wsa.Disposed += WsaDisposed;
                _wsa.SyncCompleted += WsaSyncCompleted;
                _wsa.PingAliveCompleted += WsaPingAliveCompleted;
                _wsa.SendAlertCompleted += WsaSendAlertCompleted;
                _wsa.SendContentCompleted += WsaSendContentCompleted;
                _wsa.SendAlertWithImageCompleted += WsaSendAlertWithImageCompleted;
                _wsa.SendSMSCompleted += WsaSendSMSCompleted;
                _wsa.SendTweetCompleted += WsaSendTweetCompleted;
                _wsa.DisconnectCompleted += WsaDisconnectCompleted;
                
                return _wsa;
            }
        }

        static void WsaDisposed(object sender, EventArgs e)
        {
            _wsa = null;
        }

        public static string WebservicesDisabledMessage => LocRm.GetString("WebservicesDisabled");

        public static bool WebsiteLive
        {
            get { return _websitelive; }
            set
            {
                if (_websitelive && !value)
                {
                    //disconnected
                    if (!string.IsNullOrEmpty(MainForm.Conf.AlertOnDisconnect))
                    {
                        try
                        {
                            Process.Start(MainForm.Conf.AlertOnDisconnect);
                        }catch (Exception ex)
                        {
                            Logger.LogException(ex);
                        }
                    }
                }
                if (!_websitelive && value)
                {
                    //reconnected
                    if (!string.IsNullOrEmpty(MainForm.Conf.AlertOnReconnect))
                    {
                        try
                        {
                            Process.Start(MainForm.Conf.AlertOnReconnect);
                        }catch (Exception ex)
                        {
                            Logger.LogException(ex);
                        }
                    }
                    
                    _websitelive = true;
                    if (Connect()=="OK")
                        ForceSync();
                }
                _websitelive = value;
            }
        }

        public static void SendAlert(string emailAddress, string subject, string message)
        {
            if (MainForm.Conf.UseSMTP)
            {
                Mailer.Send(emailAddress, subject, message);
                return;
            }

            if (!Enabled)
                return;
            Debug.WriteLine("WEBSERVICE CALL: SendAlertAsync");
            Wsa.SendAlertAsync(MainForm.Conf.WSUsername, MainForm.Conf.WSPassword,
                                        emailAddress, subject, message, Guid.NewGuid());
        }

        public static void SendContent(string emailAddress, string subject, string message)
        {
            if (MainForm.Conf.UseSMTP)
            {
                Mailer.Send(emailAddress, subject, message);
                return;
            }

            if (!Enabled)
                return;
            Debug.WriteLine("WEBSERVICE CALL: SendContentAsync");
            Wsa.SendContentAsync(MainForm.Conf.WSUsername, MainForm.Conf.WSPassword, emailAddress, subject, message,Guid.NewGuid());
        }
        private static bool Enabled => MainForm.Conf.ServicesEnabled && MainForm.Conf.Subscribed && WebsiteLive && !LoginFailed;

        public static void SendAlertWithImage(string emailAddress, string subject, string message, byte[] imageData)
        {
            if (MainForm.Conf.UseSMTP)
            {
                Mailer.Send(emailAddress, subject, message, imageData);
                return;
            }

            if (!Enabled)
                return;
            if (imageData.Length == 0)
            {
                SendAlert(emailAddress, subject, message);
                return;
            }
            Debug.WriteLine("WEBSERVICE CALL: SendAlertWithImageAsync");
            Wsa.SendAlertWithImageAsync(MainForm.Conf.WSUsername, MainForm.Conf.WSPassword, emailAddress, subject, message, imageData,Guid.NewGuid());
        }

        public static string ExternalIPv4(bool refresh, out bool success)
        {

            if (_externalIP != "" && !refresh)
            {
                success = true;
                return _externalIP;
            }
            if (WebsiteLive || refresh)
            {
                try
                {
                    Debug.WriteLine("WEBSERVICE CALL: RemoteAddress");
                    _externalIP = Wsa.RemoteAddress();
                    WebsiteLive = true;

                }
                catch (Exception ex)
                {
                    Logger.LogException(ex, "Webservices");
                    WebsiteLive = false;
                }
                if (WebsiteLive)
                {
                    success = true;
                    return _externalIP;
                }
            }
            if (_externalIP != "")
            {
                success = true;
                return _externalIP;
            }

            success = false;
            return LocRm.GetString("Unavailable");
        }

        public static string ProductLatestVersion(int productId)
        {
            string r = "";
            if (WebsiteLive)
            {
                try
                {
                    //call the real website...
                    using (var ws = new iSpyAPI())
                    {
                        Debug.WriteLine("WEBSERVICE CALL: ProductLatestVersionGet");
                        r = ws.ProductLatestVersionGet(productId);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                    WebsiteLive = false;
                }
                if (WebsiteLive)
                    return r;
            }
            return LocRm.GetString("iSpyDown");
        }

        public static void SendSms(string smsNumber, string message)
        {
            if (!Enabled)
                return;
            Debug.WriteLine("WEBSERVICE CALL: SendSMS");
            Wsa.SendSMSAsync(MainForm.Conf.WSUsername, MainForm.Conf.WSPassword, smsNumber, message, Guid.NewGuid());
        }

        public static void SendTweet(string message)
        {
            if (!Enabled)
                return;
            Debug.WriteLine("WEBSERVICE CALL: Tweet");
            Wsa.SendTweetAsync(MainForm.Conf.WSUsername, MainForm.Conf.WSPassword, message, Guid.NewGuid());
        }

        public static void ForceSync()
        {
            ForceSync(MainForm.IPAddress, MainForm.Conf.LANPort);
        }

        private static void ForceSync(string internalIPAddress, int internalPort)
        {
            if (LoginFailed || !WebsiteLive || !MainForm.Conf.ServicesEnabled || MainForm.ShuttingDown)
                return;

            string settings = MainForm.MWS.GetObjectList();

            MainForm.NeedsSync = false;
            int port = MainForm.Conf.ServerPort;
            if (MainForm.Conf.IPMode == "IPv6")
                port = MainForm.Conf.LANPort;

            Debug.WriteLine("WEBSERVICE CALL: ForceSync");
            Wsa.SyncAsync(MainForm.Conf.WSUsername, MainForm.Conf.WSPassword, port, internalIPAddress, internalPort, settings, MainForm.Conf.IPMode == "IPv4", MainForm.IPAddress, Guid.NewGuid());
        }


        private static Thread _pingRequestThread = null;
        public static void PingServer()
        {
            if (!MainForm.Conf.ServicesEnabled || LoginFailed || MainForm.ShuttingDown)
                return;

            if (LastLiveCheck < Helper.Now.AddSeconds(0 - (PingDelays[_pingIndex])))
            {
                if (_pingRequestThread == null)
                {
                    _pingRequestThread = new Thread(DoPingRequest);
                    _pingRequestThread.Start();
                }
            }
        }

        private static string lastResponse = "";
        public static void DoPingRequest()
        {
            int port = MainForm.Conf.ServerPort;
            if (MainForm.Conf.IPMode == "IPv6")
                port = MainForm.Conf.LANPort;

            LastLiveCheck = Helper.Now;
            string[] r = null;
            try
            {
                //using IPAddress in both as the website determines remoteip for ipv4 or uses ipv6 for both internal and external

                r = Wsa.PingAlive(MainForm.Conf.WSUsername, MainForm.Conf.WSPassword, port, MainForm.Conf.IPMode == "IPv4", MainForm.IPAddress, MainForm.IPAddress);
                if (r[0] != lastResponse)
                {
                    Logger.LogMessage("Ping: "+r[0]);
                }
                lastResponse = r[0];
                WebsiteLive = true;
                _pingIndex = 0;
            }
            catch(Exception ex)
            {
                WebsiteLive = false;
                Logger.LogException(ex,"Ping");
                _pingIndex = Math.Min(_pingIndex+1, PingDelays.Length-1);
            }
            _pingRequestThread = null;
        }

        public static void Disconnect()
        {
            if (MainForm.Conf.ServicesEnabled && WebsiteLive && !LoginFailed)
            {
                int port = MainForm.Conf.ServerPort;
                if (MainForm.Conf.IPMode == "IPv6")
                    port = MainForm.Conf.LANPort;
                 
                try
                {
                    Wsa.Disconnect(MainForm.Conf.WSUsername, MainForm.Conf.WSPassword, port);
                }
                catch
                {
                }
            }
        }

        static void WsaPingAliveCompleted(object sender, PingAliveCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                string[] r = e.Result;
                
                if (r.Length > 1)
                {
                    
                    if (MainForm.Conf.ServicesEnabled)
                    {
                        if (!MainForm.MWS.Running)
                        {
                            MainForm.StopAndStartServer();
                        }
                    }
                    
                    WebsiteLive = true;

                    if (MainForm.Conf.IPMode == "IPv4")
                        _externalIP = r[1];
                }
                else
                {
                    WebsiteLive = false;
                }
            }
            else
            {
                WebsiteLive = false;
            }
        }

        static void WsaSendAlertCompleted(object sender, SendAlertCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Logger.LogException(e.Error);
            }
            else
            {
                if (e.Result != "OK")
                    Logger.LogError("Send Alert: " + e.Result);
            }

        }

        static void WsaSendContentCompleted(object sender, SendContentCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Logger.LogException(e.Error);
            }
            else
            {
                if (e.Result != "OK")
                    Logger.LogError("Send Content: " + e.Result);    
            }
            

        }

        static void WsaDisconnectCompleted(object sender, DisconnectCompletedEventArgs e)
        {
            //var m = e.Result;

        }

        static void WsaSendAlertWithImageCompleted(object sender, SendAlertWithImageCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Logger.LogException(e.Error);
            }
            else
            {
                if (e.Result != "OK")
                    Logger.LogError("Send Alert With Image: " + e.Result);
            }

        }

        static void WsaSendSMSCompleted(object sender, SendSMSCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Logger.LogException(e.Error);
            }
            else
            {
                if (e.Result != "OK")
                    Logger.LogError("Send SMS: " + e.Result);
            }

        }

        static void WsaSendTweetCompleted(object sender, SendTweetCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Logger.LogException(e.Error);
            }
            else
            {
                if (e.Result != "OK")
                    Logger.LogError("Send Tweet: " + e.Result);
            }

        }

        static void WsaSyncCompleted(object sender, SyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                WebsiteLive = false;
                MainForm.NeedsSync = true;
                Logger.LogException(e.Error);
            }
            else
            {
                if (e.Result != "OK")
                    Logger.LogError("Sync: " + e.Result);
            }
        }

        

        public static string Connect()
        {
            return Connect(MainForm.LoopBack);
        }

        public static string Connect(bool tryLoopback)
        {
            if (!MainForm.Conf.ServicesEnabled)
                return WebservicesDisabledMessage;
            string r = "";
            
            int port = MainForm.Conf.ServerPort;
            if (MainForm.Conf.IPMode == "IPv6")
                port = MainForm.Conf.LANPort;

            try
            {
                if (MainForm.CustomWebserver)
                    r = Wsa.Connect2(MainForm.Conf.WSUsername, MainForm.Conf.WSPassword, port, MainForm.Identifier, tryLoopback, Application.ProductVersion,
                        MainForm.Conf.ServerName, MainForm.Conf.IPMode == "IPv4", MainForm.IPAddress, MainForm.Affiliateid,
                        X509.SslEnabled);
                else
                    r = Wsa.Connect4(MainForm.Conf.WSUsername, MainForm.Conf.WSPassword, port,
                    MainForm.Identifier, tryLoopback, Application.ProductVersion,
                    MainForm.Conf.ServerName, MainForm.Conf.IPMode == "IPv4", MainForm.IPAddress, MainForm.Affiliateid,
                    X509.SslEnabled);
                if (r == "OK" && tryLoopback)
                {
                    MainForm.LoopBack = true;
                }
                else
                    MainForm.LoopBack = false;

                WebsiteLive = true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                WebsiteLive = false;
            }

            if (WebsiteLive)
            {
                LoginFailed = r == "Webservices_LoginFailed" || r == "Expired";
                Expired = r == "Expired";
                if (r != "OK")
                {
                    Logger.LogError("Webservices: " + r);
                    return LocRm.GetString(r);
                }
                return r;
            }

            return LocRm.GetString("iSpyDown");
        }

        public static bool LoginFailed;
        public static bool Expired;

        public static string[] TestConnection(string username, string password, bool tryLoopback)
        {
            var r = new string[] {};

            int port = MainForm.Conf.ServerPort;
            if (MainForm.Conf.IPMode == "IPv6")
                port = MainForm.Conf.LANPort;

            try
            {
                if (MainForm.CustomWebserver)
                    r = Wsa.TestConnection2(username, password, port, MainForm.Identifier, tryLoopback, MainForm.Conf.IPMode == "IPv4", MainForm.IPAddress, X509.SslEnabled);
                else
                    r = Wsa.TestConnection3(username, password, port, MainForm.Identifier, tryLoopback, MainForm.Conf.IPMode == "IPv4", MainForm.IPAddress, X509.SslEnabled);
                WebsiteLive = true;
            }
            catch (Exception ex)
            {
                WebsiteLive = false;
                Logger.LogException(ex);
            }
            if (WebsiteLive)
            {
                LoginFailed = (r[0] == "Webservices_LoginFailed" || r[0]=="Expired");
                if (r.Length == 1 && r[0] != "OK")
                {
                    r[0] = LocRm.GetString(r[0]);                    
                    Logger.LogError("Webservices: "+r[0]);
                }
                if (r.Length > 3 && r[3] != "")
                {
                    r[3] = LocRm.GetString(r[3]);
                    Logger.LogError("Webservices: " + r[3]);
                }
                return r;
            }
            
            return new[] { LocRm.GetString("iSpyDown") };
        }
    }
}