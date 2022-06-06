using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using iSpyApplication.Cloud;
using iSpyApplication.Controls;
using iSpyApplication.Properties;
using iSpyApplication.Sources.Audio;
using iSpyApplication.Sources.Audio.streams;
using iSpyApplication.Sources.Audio.talk;
using iSpyApplication.Sources.Video;
using iSpyApplication.Utilities;
using NAudio.Wave;
using Color = System.Drawing.Color;
using DateTime = System.DateTime;
using File = System.IO.File;
using Image = System.Drawing.Image;
using IPAddress = System.Net.IPAddress;
using ThreadState = System.Threading.ThreadState;

namespace iSpyApplication.Server
{
    public class RemoteCommandEventArgs : EventArgs
    {
        public string Command;
        public int ObjectId;
        public int ObjectTypeId;

        // Constructor
        public RemoteCommandEventArgs(string command, int objectid, int objecttypeid)
        {
            Command = command;
            ObjectId = objectid;
            ObjectTypeId = objecttypeid;
        }
    }

    public partial class LocalServer: IDisposable
    {
        //private static readonly List<Socket> MySockets = new List<Socket>();
        private static List<String> _allowedIPs, _allowedReferers;
        private static readonly object StaticThreadLock = new object();
        //private static int _socketindex;
        public string ServerRoot;
        private Hashtable _mimetypes;
        private TcpListener _myListener;
        public int NumErr;
        private Thread _th;
        private readonly object _threadLock = new object();
        public WebSocketServer WebSocketServer;

        public bool ServerStartupFailed = false;


        public Hashtable MimeTypes
        {
            get
            {
                if (_mimetypes == null)
                {
                    _mimetypes = new Hashtable();
                    using (var sr = new StreamReader(ServerRoot + @"data\mime.Dat"))
                    {
                        string sLine;
                        while ((sLine = sr.ReadLine()) != null)
                        {
                            sLine = sLine.Trim();

                            if (sLine.Length > 0)
                            {
                                //find the separator
                                int iStartPos = sLine.IndexOf(";", StringComparison.Ordinal);

                                // Convert to lower case
                                sLine = sLine.ToLower();

                                string sMimeExt = sLine.Substring(0, iStartPos);
                                string sMimeType = sLine.Substring(iStartPos + 1);
                                _mimetypes.Add(sMimeExt, sMimeType);
                            }
                        }
                    }
                }
                return _mimetypes;
            }
        }


        public bool Running
        {
            get
            {
                lock (_threadLock)
                {
                    if (_th == null)
                        return false;

                    try
                    {
                        return !_th.Join(TimeSpan.Zero);
                    }
                    catch
                    {
                        return true;
                    }
                }
            }
        }

        private IPAddress ListenerAddress
        {
            get
            {
                switch (MainForm.Conf.IPMode)
                {
                    case "IPv6":
                        if (!MainForm.Conf.SpecificIP)
                            return IPAddress.IPv6Any;
                        return IPAddress.Parse(MainForm.AddressIPv6);
                    default:
                        if (!MainForm.Conf.SpecificIP)
                            return IPAddress.Any;
                        return IPAddress.Parse(MainForm.AddressIPv4);
                }
            }
        }

        public string StartServer()
        {
            bool ssl = false;
            if (!string.IsNullOrEmpty(MainForm.Conf.SSLCertificate))
                X509.LoadCertificate(MainForm.Conf.SSLCertificate);
            else
            {
                if (ssl)
                {
                    bool b;
                    var ip = WsWrapper.ExternalIPv4(true, out b);
                    if (b)
                        X509.CreateCertificate(ip);
                }
            }

            string message = "";
            try
            {
                _myListener = new TcpListener(ListenerAddress, MainForm.Conf.LANPort) { ExclusiveAddressUse = false };
                if (MainForm.Conf.IPMode=="IPv6")
                {
                     _myListener.AllowNatTraversal(true);
                }
                _myListener.Start(200);

                WebSocketServer?.Close();

                WebSocketServer = new WebSocketServer(this);
                ServerStartupFailed = false;
            }
            catch (Exception e)
            {
                Logger.LogException(e,"Server");
                StopServer();
                message = "Could not start local iSpy server - please select a different LAN port in settings. The port specified is in use. See the log file for more information.";
                ServerStartupFailed = true;
            }
            if (message != "")
            {
                Logger.LogMessage(message, "Server");
                return message;
            }
            try 
            {
                //start the thread which calls the method 'StartListen'
                if (Running)
                {
                    while (_th.ThreadState == ThreadState.AbortRequested)
                    {
                        Application.DoEvents();
                    }
                }
                
            }
            catch (Exception e)
            {
                message = e.Message;
                Logger.LogException(e, "Server");
            }

            lock (_threadLock)
            {
                _th = new Thread(StartListen) {IsBackground = true};
                _th.Start();
            }
            return message;
        }

        public void StopServer()
        {
        
            if (_connectedSockets != null)
            {
                    
                try
                {
                    ClientConnected.Set();
                    lock (_connectedSocketsSyncHandle)
                    {
                        foreach (var req in _connectedSockets)
                        {
                            req.TcpClient.Client.Close();
                        }
                    }
                }
                catch (SocketException ex)
                {
                    //During one socket disconnected we can faced exception
                    Logger.LogException(ex, "Server");
                }
            }

            if (_myListener?.Server != null)
            {
                try
                {
                    _myListener.Server.Close(0);
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex, "Server");
                }
                finally
                {
                    _myListener.Stop();
                }
                _myListener = null;
            }
            WebSocketServer?.Close();
            WebSocketServer = null;
        }


        /// <summary>
        /// This function takes FileName as Input and returns the mime type..
        /// </summary>
        /// <param name="sRequestedFile">To indentify the Mime Type</param>
        /// <returns>Mime Type</returns>
        public string GetMimeType(string sRequestedFile)
        {
            if (sRequestedFile == "")
                return "";
            String sMimeType = "";

            // Convert to lowercase
            sRequestedFile = sRequestedFile.ToLower();

            int iStartPos = sRequestedFile.LastIndexOf(".", StringComparison.Ordinal);
            if (iStartPos == -1)
                return "text/javascript";
            string sFileExt = sRequestedFile.Substring(iStartPos);

            if (MimeTypes.ContainsKey(sFileExt))
                sMimeType = MimeTypes[sFileExt].ToString();

            return sMimeType;
        }

        private bool SendHeader(string sHttpVersion, string sMimeHeader, int iTotBytes, string sStatusCode, int cacheDays,
                               HttpRequest req)
        {
            return SendHeader(sHttpVersion, sMimeHeader, iTotBytes, sStatusCode, cacheDays, req, "", false);
        }
        private bool SendHeader(string sHttpVersion, string sMimeHeader, int iTotBytes, string sStatusCode, int cacheDays,
                               HttpRequest req, string fileName, bool gZip)
        {
            string sBuffer = "";

            // if Mime type is not provided set default to text/html
            if (sMimeHeader.Length == 0)
            {
                sMimeHeader = "text/html"; // Default Mime Type is text/html
            }

            sBuffer += sHttpVersion + sStatusCode + "\r\n";
            sBuffer += "Server: iSpy\r\n";
            if (fileName!="")
            {
                sBuffer += "Content-Type: application/octet-stream\r\n";
                sBuffer += "Content-Disposition: attachment; filename=\"" + fileName + "\"\r\n";
            }
            else
                sBuffer += "Content-Type: " + sMimeHeader + "\r\n";
            //sBuffer += "X-Content-Type-Options: nosniff\r\n";
            sBuffer += "Accept-Ranges: bytes\r\n";
            sBuffer += "Access-Control-Allow-Origin: *\r\n";

            if (iTotBytes > -1)
                sBuffer += "Content-Length: " + iTotBytes + "\r\n";
            if (gZip)
            {
                sBuffer += "Content-Encoding: gzip\r\n";
            }
            //sBuffer += "Cache-Control:Date: Tue, 25 Jan 2011 08:18:53 GMT\r\nExpires: Tue, 08 Feb 2011 05:06:38 GMT\r\nConnection: keep-alive\r\n";
            if (cacheDays > 0)
            {
                //this is needed for video content to work in chrome/android
                DateTime d = DateTime.UtcNow;
                sBuffer += "Cache-Control: Date: " + d.ToUniversalTime().ToString("r") +
                           "\r\nLast-Modified: Tue, 01 Jan 2011 12:00:00 GMT\r\nExpires: " +
                           d.AddDays(cacheDays).ToUniversalTime().ToString("r") + "\r\nConnection: keep-alive\r\n";
            }
            else
            {
                sBuffer +=
                    "Pragma: no-cache\r\nExpires: Fri, 30 Oct 1998 14:19:41 GMT\r\nCache-directive: no-cache\r\nCache-control: no-cache\r\nPragma: no-cache\r\nExpires: 0\r\n";
            }


            sBuffer += "\r\n";

            return SendToBrowser(sBuffer, req);
        }


        private bool SendHeaderWithRange(string sHttpVersion, string sMimeHeader, int iStartBytes, int iEndBytes,
                                        int iTotBytes, string sStatusCode, int cacheDays, HttpRequest req, string fileName)
        {
            string sBuffer = "";

            // if Mime type is not provided set default to text/html
            if (sMimeHeader.Length == 0)
            {
                sMimeHeader = "text/html"; // Default Mime Type is text/html
            }

            sBuffer += sHttpVersion + sStatusCode + "\r\n";
            sBuffer += "Server: iSpy\r\n";
            if (fileName != "")
            {
                sBuffer += "Content-Type: application/octet-stream\r\n";
                sBuffer += "Content-Disposition: attachment; filename=\"" + fileName + "\"\r\n";
            }
            else
                sBuffer += "Content-Type: " + sMimeHeader + "\r\n";

            //sBuffer += "X-Content-Type-Options: nosniff\r\n";
            sBuffer += "Accept-Ranges: bytes\r\n";
            sBuffer += "Content-Range: bytes " + iStartBytes + "-" + iEndBytes + "/" + (iTotBytes) + "\r\n";
            sBuffer += "Content-Length: " + (iEndBytes - iStartBytes + 1) + "\r\n";
            if (cacheDays > 0)
            {
                //this is needed for video content to work in chrome/android
                DateTime d = DateTime.UtcNow;
                sBuffer += "Cache-Control: Date: " + d.ToUniversalTime().ToString("r") +
                           "\r\nLast-Modified: Tue, 01 Jan 2011 12:00:00 GMT\r\nExpires: " +
                           d.AddDays(cacheDays).ToUniversalTime().ToString("r") + "\r\nConnection: keep-alive\r\n";
            }

            sBuffer += "\r\n";
            byte[] bSendData = Encoding.UTF8.GetBytes(sBuffer);

            return SendToBrowser(bSendData, req);
        }

        public bool SendResponse(string sHttpVersion, string sMimeType, string sData, string statusCode, int cacheDays, HttpRequest req, bool gZip = false)
        {
            var b = Encoding.UTF8.GetBytes(sData);
            return SendResponse(sHttpVersion, sMimeType, b, statusCode, cacheDays, req, gZip);
        }

        public bool SendResponse(string sHttpVersion, string sMimeType, byte[] bData, string statusCode, int cacheDays, HttpRequest req, bool gZip = false)
        {
            if (SendHeader(sHttpVersion, sMimeType, bData.Length, statusCode, cacheDays, req, "", gZip))
                return SendToBrowser(bData, req);
            return false;

        }

        public bool SendToBrowser(string sData, HttpRequest req)
        {
            return SendToBrowser(Encoding.UTF8.GetBytes(sData), req);
        }

        /// <summary>
        /// Sends data to the browser (client)
        /// </summary>
        /// <param name="bSendData">Byte Array</param>
        /// <param name="req">HTTP Request</param>
        public bool SendToBrowser(byte[] bSendData, HttpRequest req)
        {
            try
            {
                if (req.TcpClient.Client.Connected)
                {
                    req.Stream.Write(bSendData,0,bSendData.Length);
                    return true;
                    //if (req.TcpClient.Client.Send(bSendData) == -1)
                    //  Logger.LogException(new Exception("Socket Error cannot Send Packet"));
                }
            }
            catch (SocketException)
            {
                //connection error
            }
            catch (Exception)
            {
                //dropped connection
            }
            return false;
        }

        /// <summary>
        /// Sends data to the browser (client)
        /// </summary>
        /// <param name="bSendData">Byte Array</param>
        /// <param name="datalength"></param>
        /// <param name="req">HTTP Request</param>
        public void SendToBrowser(byte[] bSendData, int datalength, HttpRequest req)
        {
            try
            {
                if (req.TcpClient.Client.Connected)
                {
                    req.Stream.Write(bSendData, 0, datalength);
                    //if (req.TcpClient.Client.Send(bSendData) == -1)
                    //  Logger.LogException(new Exception("Socket Error cannot Send Packet"));
                }
            }
            catch (SocketException)
            {
                //connection error
            }
            catch (Exception e)
            {
                Logger.LogException(e, "Server");
            }
        }

        public bool ThumbnailCallback()
        {
            return false;
        }

        public static AutoResetEvent ClientConnected = new AutoResetEvent(false);
        private List<HttpRequest> _connectedSockets;
        private readonly object _connectedSocketsSyncHandle = new object();

        //This method Accepts new connection and
        //First it receives the welcome massage from the client,
        //Then it sends the Current date time to the Client.
        private void StartListen()
        {
            _connectedSockets = new List<HttpRequest>();
            NumErr = 0;

            while (!MainForm.ShuttingDown && NumErr < 5 && _myListener != null)
            {
                try
                {
                    _myListener?.BeginAcceptTcpClient(DoAcceptClientCallback, _myListener);

                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex, "Server");
                    break;
                }
                // Wait until a connection is made and processed before  
                // continuing.
                ClientConnected?.WaitOne(); // Wait until a client has begun handling an event
                ClientConnected?.Reset();
            }
        }

        

        private static bool ClientValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (!MainForm.Conf.SSLIgnoreErrors)
            {
                switch (sslPolicyErrors)
                {
                    case SslPolicyErrors.RemoteCertificateNameMismatch:
                        Logger.LogError("Client name mismatch. End communication", "Server");
                        return false;
                    case SslPolicyErrors.RemoteCertificateNotAvailable:
                        Logger.LogError("Client's certificate not available. End communication", "Server");
                        return false;
                    case SslPolicyErrors.RemoteCertificateChainErrors:
                        Logger.LogError("Client's certificate validation failed. End communication", "Server");
                        return false;
                }
            }

            //Logger.Log(LogLevel.Information, "Client's authentication succeeded ...\n");
            return true;
        }

        private void OnAuthenticateAsServer(IAsyncResult result)
        {
            HttpRequest req = null;
            try
            {
                req = result.AsyncState as HttpRequest;
                var sslStream = req?.Stream as SslStream;

                if (sslStream != null)
                {
                    sslStream.EndAuthenticateAsServer(result);
                    if (!sslStream.IsAuthenticated)
                    {
                        DisconnectRequest(req);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                DisconnectRequest(req);
                return;
            }

            req?.Stream?.BeginRead(req.Buffer, 0, req.Buffer.Length, ReadCallback, req);
        }

        public void DoAcceptClientCallback(IAsyncResult ar)
        {
            ClientConnected.Set();
            if (MainForm.ShuttingDown)
                return;
            try
            {
                var listener = (TcpListener) ar.AsyncState;
                TcpClient myClient = listener.EndAcceptTcpClient(ar);

                var endPoint = (IPEndPoint)myClient.Client.RemoteEndPoint;
                var req = new HttpRequest
                          {EndPoint = endPoint, TcpClient = myClient, Buffer = new byte[myClient.ReceiveBufferSize]};

                lock (_connectedSocketsSyncHandle)
                {
                    var cs = _connectedSockets.FirstOrDefault(p => p.EndPoint.ToString() == endPoint.ToString());
                    if (cs != null)
                    {
                        cs.TcpClient.Client.Close();
                        _connectedSockets.Remove(cs);
                    }

                    SetDesiredKeepAlive(myClient.Client);
                    _connectedSockets.Add(req);
                }

                var mySocket = myClient.Client;

                if (MainForm.Conf.IPMode== "IPv6")
                    mySocket.SetIPProtectionLevel(IPProtectionLevel.Unrestricted);
                    
                   
                if (mySocket.Connected)
                {
                    mySocket.NoDelay = true;
                    mySocket.ReceiveBufferSize = 8192;
                    mySocket.ReceiveTimeout = mySocket.SendTimeout = 4000;
                    try
                    {
                        if (X509.SslCertificate != null)
                        {
                            req.RestartableStream = new RestartableReadStream(req.TcpClient.GetStream());
                            req.Stream = new SslStream(req.RestartableStream, true, ClientValidationCallback);
                            try
                            {
                                ((SslStream) req.Stream).BeginAuthenticateAsServer(X509.SslCertificate,
                                    true, SslProtocols.Tls12,
                                    //MainForm.Conf.SSLClientRequired, SslProtocols.Default,
                                    MainForm.Conf.SSLCheckRevocation, OnAuthenticateAsServer, req);
                            }
                            catch
                            {
                                DisconnectRequest(req);
                            }
                        }
                        else
                        {
                            req.Stream = req.TcpClient.GetStream();
                            req.Stream.BeginRead(req.Buffer, 0, req.Buffer.Length, ReadCallback, req);
                        }

                    }
                    catch (SocketException ex)
                    {
                        //ignore connection timeout errors
                        if (ex.ErrorCode != 10060)
                        {
                            Logger.LogException(ex, "Server");
                            NumErr++;

                        }
                        DisconnectRequest(req);
                    }
                    catch (IOException)
                    {
                        //no data (port scan?)
                        DisconnectRequest(req);
                    }
                }
            }
            catch(ObjectDisposedException)
            {
                //socket closed already
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Server");
                NumErr++;
            }
        }

        private void ReadCallback(IAsyncResult result)
        {
            var req = result.AsyncState as HttpRequest;
            if (req != null)
            {
                try
                {
                    var read = req.Stream.EndRead(result);
                    if (read > 0)
                    {
                        req.UTF8 += Encoding.UTF8.GetString(req.Buffer, 0, read);
                        req.BytesRead += read;
                    }



                    //flash xml request
                    if (req.UTF8.StartsWith("<policy-file-request/>"))
                    {
                        req.TcpClient.Client.SendFile(Program.AppPath + @"WebServerRoot\crossdomain.xml");
                        DisconnectRequest(req);
                        NumErr = 0;
                        return;
                    }

                    //talk in socket call
                    if (req.UTF8.StartsWith("TALK,"))
                    {
                        if (req.UTF8.Length > 10)
                        {
                            string[] cfg = req.UTF8.Substring(0, 10).Split(',');
                            int cid = Convert.ToInt32(cfg[1]);

                            var feed = new Thread(p => AudioIn(req, cid)) {IsBackground = true};
                            feed.Start();
                            return;
                        }
                    }

                    int iStartPos = req.UTF8.IndexOf("\r\n\r\n", StringComparison.Ordinal);
                    if (iStartPos>-1)
                    {
                        List<string> headers = req.UTF8.Substring(0, iStartPos).Split('\n').ToList();
                        var cl = headers.FirstOrDefault(p => p.Trim().StartsWith("Content-Length", StringComparison.OrdinalIgnoreCase));

                        var v = cl?.Split(':');
                        if (v?.Length > 1)
                        {
                            int icl = Convert.ToInt32(v[1].Trim());
                            if (icl > req.BytesRead - iStartPos - 4)
                            {
                                req.Stream.BeginRead(req.Buffer, 0, req.Buffer.Length, ReadCallback, req);
                                return;
                            }
                        }
                        ProcessRequest(req.UTF8, req);
                    }
                    else
                    {
                        if (read==0)
                            DisconnectRequest(req);
                        else
                        {
                            if (req.UTF8.Length>200000)
                                throw new Exception("Incoming request is too long");
                            
                            req.Stream.BeginRead(req.Buffer, 0, req.Buffer.Length, ReadCallback, req);
                        }
                            
                    }
                }
                catch
                {
                    //connection closed
                    DisconnectRequest(req);
                }
            }        
        }

        private void ProcessRequest(string sBuffer, HttpRequest req)
        {
            if (sBuffer.Length < 5)
            {
                DisconnectRequest(req);
                return;
            }
            try
            {
                //make a byte array and receive data from the client 
                string sHttpVersion;
                string resp;
                String sMimeType;
                bool bServe;

                var iMeth = sBuffer.IndexOf(" ", StringComparison.Ordinal);
                if (iMeth == -1)
                    goto Finish;

                var m = sBuffer.Substring(0, iMeth);
                switch (m)
                {
                    case "OPTIONS":
                        string sResponse = "HTTP/1.1 200 OK\r\n";
                        sResponse += "Server: iSpy\r\n";
                        sResponse += "Access-Control-Allow-Origin: *\r\n";
                        sResponse += "Access-Control-Allow-Methods: GET,POST,OPTIONS\r\n";
                        sResponse += "Access-Control-Allow-Headers: Origin, X-Requested-With, Content-Type, Accept\r\n";
                        sResponse += "Connection: close\r\n";
                        sResponse += "\r\n";
                        SendToBrowser(sResponse, req);
                        goto Finish;
                    case "GET":
                    case "POST":
                        break;
                    default:
                        goto Finish;

                }

                string sRequest;
                string sPhysicalFilePath;
                string errMessage;
                string sRequestedFile;
                try
                {

                    string sErrorMessage;
                    string sLocalDir;
                    string sDirName;
                    string sFileName;
                    ParseRequest(ServerRoot, sBuffer, out sRequest, out sRequestedFile,
                        out sErrorMessage,
                        out sLocalDir, out sDirName, out sPhysicalFilePath, out sHttpVersion,
                        out sFileName, out sMimeType, out bServe, out errMessage,req);
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex,"Server (parse request)");
                    goto Finish;
                }
                if (!bServe && string.IsNullOrEmpty(sRequestedFile))
                {
                    resp = "iSpy is running. Access this server via the website";
                    if (errMessage != "")
                        resp += " (" + errMessage + ")";
                    SendResponse(sHttpVersion, "text/text", resp, " 200 OK", 0, req);
                    goto Finish;
                }

                if (!bServe)
                {
                    if (errMessage != "")
                        errMessage = " (" + errMessage + ")";
                    resp = "{\"server\":\"iSpy\",\"error\":\"Authentication failed" + errMessage +
                           "\",\"errorType\":\"authentication\"}";
                    SendResponse(sHttpVersion, "text/javascript", resp, " 200 OK", 0, req);
                    goto Finish;
                }

                resp = ProcessCommandInternal(sRequest);

                if (resp != "")
                {
                    bool gzip = resp.Length > 400 && MainForm.Conf.EnableGZip &&
                                HeaderEnabled(sBuffer, "Accept-Encoding", "gzip");

                    if (gzip)
                    {
                        var arr = Gzip(Encoding.UTF8.GetBytes(resp));
                        SendResponse(sHttpVersion, "text/javascript", arr, " 200 OK", 0, req, true);
                    }
                    else
                    {
                        SendResponse(sHttpVersion, "text/javascript", resp, " 200 OK", 0, req, false);
                    }
                }
                else //not a js request
                {
                    string cmd = sRequest.Trim('/').ToLower();
                    int i = cmd.IndexOf("?", StringComparison.Ordinal);
                    if (i > -1)
                        cmd = cmd.Substring(0, i);

                    if (cmd.StartsWith("get /"))
                        cmd = cmd.Substring(5);
                    if (cmd.StartsWith("post /"))
                        cmd = cmd.Substring(6);

                    int oid, otid;
                    int.TryParse(GetVar(sRequest, "oid"), out oid);
                    int.TryParse(GetVar(sRequest, "ot"), out otid);
                    switch (cmd)
                    {
                        case "logfile":
                            SendLogFile(sHttpVersion, req);
                            break;
                        case "getlogfile":
                            SendLogFile(sPhysicalFilePath, sHttpVersion, req);
                            break;
                        case "livefeed":
                            SendLiveFeed(sPhysicalFilePath, sHttpVersion, req);
                            break;
                        case "desktopfeed":
                            SendDesktop(sPhysicalFilePath, sHttpVersion, req);
                            break;
                        case "loadgrab":
                        case "loadgrab.jpg":
                            SendGrab(sPhysicalFilePath, sHttpVersion, req);
                            break;
                        case "loadimage":
                        case "loadimage.jpg":
                            SendImage(sPhysicalFilePath, sHttpVersion, req);
                            break;
                        case "floorplanfeed":
                            SendFloorPlanFeed(sPhysicalFilePath, sHttpVersion, req);
                            break;
                        case "audiofeed.mp3":
                            SendAudioFeed(Enums.AudioStreamMode.MP3, sBuffer, sPhysicalFilePath, req);
                            return;
                        case "loadobject.json":
                            LoadJson(sPhysicalFilePath, sRequest, sBuffer, sHttpVersion, req);
                            break;
                        case "saveobject.json":
                            SaveJson(sPhysicalFilePath, sHttpVersion, sBuffer, req);
                            break;
                        case "video.mjpg":
                        case "video.cgi":
                        case "video.mjpeg":
                        case "video.jpg":
                        case "mjpegfeed":
                            SendMJPEGFeed(sPhysicalFilePath, req);
                            return;
                        case "loadclip.flv":
                        case "loadclip.fla":
                        case "loadclip.mp3":
                        case "loadclip.mp4":
                        case "loadclip.avi":
                            SendClip(sPhysicalFilePath, sBuffer, sHttpVersion, req, false);
                            break;
                        case "downloadclip.avi":
                        case "downloadclip.mp3":
                        case "downloadclip.mp4":
                            SendClip(sPhysicalFilePath, sBuffer, sHttpVersion, req, true);
                            break;
                        case "websocket":
                            WebSocketServer.ConnectSocket(sBuffer, req);
                            return;
                        default:
                            if (sPhysicalFilePath.IndexOf('?') != -1)
                            {
                                sPhysicalFilePath = sPhysicalFilePath.Substring(0, sPhysicalFilePath.IndexOf('?'));
                            }

                            if (!File.Exists(sPhysicalFilePath))
                            {
                                ServeNotFound(sHttpVersion, req);
                            }
                            else
                            {
                                ServeFile(sHttpVersion, sPhysicalFilePath, sMimeType, req);
                            }
                            break;
                    }
                }

                Finish:
                DisconnectRequest(req);
                NumErr = 0;
            }
            catch (SocketException ex)
            {
                //ignore connection timeout errors
                if (ex.ErrorCode != 10060)
                {
                    Logger.LogException(ex, "Server");
                    NumErr++;

                }
                DisconnectRequest(req);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex,"Server");
            }
        }

        private static bool HeaderEnabled(string req, string header, string val)
        {
            header = header.ToLower();
            req = req.ToLower();
            val = val.ToLower();

            var p = req.Split(Environment.NewLine.ToCharArray());
            foreach(var s in p)
            {
                if (!s.StartsWith(header)) continue;
                var v = s.Split(':');
                if (v.Length>1)
                {
                    string[] l = v[1].Split(',');

                    return l.Any(lp => lp.Trim() == val);
                }
                return false;
            }
            return false;
        }

        private static byte[] Gzip(byte[] bytes)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                using (GZipStream gzipStream = new GZipStream(outStream, CompressionMode.Compress))
                using (MemoryStream srcStream = new MemoryStream(bytes))
                    srcStream.CopyTo(gzipStream);
                return outStream.ToArray();
            }
        }

        private static void SetDesiredKeepAlive(Socket socket)
        {
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            const uint time = 10000;
            const uint interval = 20000;
            SetKeepAlive(socket, true, time, interval);
        }
        static void SetKeepAlive(Socket s, bool on, uint time, uint interval)
        {
            /* the native structure
            struct tcp_keepalive {
            ULONG onoff;
            ULONG keepalivetime;
            ULONG keepaliveinterval;
            };
            */

            // marshal the equivalent of the native structure into a byte array
            const uint dummy = 0;
            var inOptionValues = new byte[Marshal.SizeOf(dummy) * 3];
            BitConverter.GetBytes((uint)(on ? 1 : 0)).CopyTo(inOptionValues, 0);
            BitConverter.GetBytes(time).CopyTo(inOptionValues, Marshal.SizeOf(dummy));
            BitConverter.GetBytes(interval).CopyTo(inOptionValues, Marshal.SizeOf(dummy) * 2);
            // of course there are other ways to marshal up this byte array, this is just one way

            // call WSAIoctl via IOControl
            s.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);

        }

        

        private void SendClip(String sPhysicalFilePath, string sBuffer, string sHttpVersion, HttpRequest req, bool downloadFile)
        {
            int oid = Convert.ToInt32(GetVar(sPhysicalFilePath, "oid"));
            int ot =  Convert.ToInt32(GetVar(sPhysicalFilePath, "ot"));


            string dir = Helper.GetMediaDirectory(ot, oid); 
            if (ot==1)
            {
                dir += @"audio\"+MainForm.Microphones.Single(p => p.id == oid).directory + @"\";
            }
            if (ot==2)
            {
                dir += @"video\" + MainForm.Cameras.Single(p => p.id == oid).directory + @"\";
            }
            string fn = dir+GetVar(sPhysicalFilePath, "fn");

            int iStartBytes = 0;
            int iEndBytes = 0;
            bool isrange = false;

            if (sBuffer.IndexOf("Range: bytes=", StringComparison.Ordinal) != -1)
            {
                string[] headers = sBuffer.Split(Environment.NewLine.ToCharArray());
                foreach (string h in headers)
                {
                    if (h.StartsWith("Range:"))
                    {
                        string[] range = (h.Substring(h.IndexOf("=", StringComparison.Ordinal) + 1)).Split('-');
                        iStartBytes = Convert.ToInt32(range[0]);
                        if (range[1] != "")
                        {
                            iEndBytes = Convert.ToInt32(range[1]);
                        }
                        else
                        {
                            iEndBytes = -1;
                        }
                        isrange = true;
                        break;
                    }
                }
            }


            var fi = new FileInfo(fn);
            int iTotBytes = Convert.ToInt32(fi.Length);
            if (iEndBytes == -1)
                iEndBytes = iTotBytes - 1;
            if (!File.Exists(fn))
            {
                SendHeader(sHttpVersion, "text/HTML", 0, " 440 OK", 0, req);
                return;
            }
            
            byte[] bytes;
            var fs = new FileStream(fn, FileMode.Open, FileAccess.Read, FileShare.Read);
            
            using (var reader = new BinaryReader(fs))
            {

                if (!isrange)
                {
                    bytes = new byte[fs.Length];
                    while ((reader.Read(bytes, 0, bytes.Length)) != 0)
                    {
                    }
                }
                else
                {
                    bytes = new byte[iEndBytes - iStartBytes + 1];
                    reader.BaseStream.Seek(iStartBytes, SeekOrigin.Begin);
                    bytes = reader.ReadBytes(bytes.Length);
                }
            }

            string sMimeType = GetMimeType(fn);
            
            string filename = fi.Name;

            if (downloadFile)
                filename = fi.Name.Replace("_", "").Replace("-", "");

            if (isrange)
            {
                SendHeaderWithRange(sHttpVersion, sMimeType, iStartBytes, iEndBytes, iTotBytes, " 206 Partial Content", 20, req, filename);
            }
            else
            {
                SendHeader(sHttpVersion, sMimeType, iTotBytes, " 200 OK", 20, req, filename, false);
            }
            
            SendToBrowser(bytes, req);
        }

        private void ServeNotFound(string sHttpVersion, HttpRequest req)
        {
            const string resp = "iSpy server is running";
            SendResponse(sHttpVersion, "", resp, " 200 OK", 0, req);
        }

        public static List<String> AllowedIPs
        {
            get
            {
                if (_allowedIPs != null)
                    return _allowedIPs;
                lock (StaticThreadLock)
                {
                    if (_allowedIPs != null)
                        return _allowedIPs;

                    var ips = MainForm.Conf.AllowedIPList.Split(',').ToList();
                    ips.Add("127.0.0.1");
                    ips.RemoveAll(p => p == "");
                    Thread.MemoryBarrier();
                    _allowedIPs = ips;
                    return ips;
                }
            }
        }

        public static void ReloadAllowedIPs()
        {
            lock (StaticThreadLock)
            {
                _allowedIPs = null;
            }
        }

        public static List<String> AllowedReferrers
        {
            get
            {
                if (_allowedReferers != null)
                    return _allowedReferers;
                lock (StaticThreadLock)
                {
                    if (_allowedReferers != null)
                        return _allowedReferers;

                    var refs = MainForm.Conf.Referers.Split(',').ToList();
                    refs.Add("http://www.ispyconnect.com/*");
                    refs.Add("https://www.ispyconnect.com/*");
                    refs.RemoveAll(p => p == "");
                    Thread.MemoryBarrier();
                    _allowedReferers = refs;
                    return refs;
                }
            }
            set { _allowedReferers = value; }
        }

        public static void ReloadAllowedReferrers()
        {
            lock (StaticThreadLock)
            {
                _allowedReferers = null;
            }
        }

        private static string GetFromBuffer(string buffer, string field)
        {
            var ba = buffer.Split(Environment.NewLine.ToCharArray());
            foreach (var s in ba)
            {
                int i = s.IndexOf(":", StringComparison.Ordinal);
                if (i>-1 && i<s.Length-1)
                {
                    string r1 = s.Substring(0, i).Trim();
                    string r2 = s.Substring(i + 1).Trim();
                    if (string.Compare(r1, field, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        return r2;
                    }
                }
            }
            return "";
        }

        private void ParseRequest(string sMyWebServerRoot, string sBuffer, out string sRequest,
                                  out string sRequestedFile, out string sErrorMessage, out string sLocalDir,
                                  out string sDirName, out string sPhysicalFilePath, out string sHttpVersion,
                                  out string sFileName, out string sMimeType, out bool bServe, out string errMessage, HttpRequest req)
        {
            sErrorMessage = "";
            bServe = false;
            bool bHasReferer = true;
            errMessage = "";

            if (AllowedReferrers.Count>2)
            {
                string referer = GetFromBuffer(sBuffer,"Referer");
                if (!string.IsNullOrEmpty(referer))
                {
                    bHasReferer = AllowedReferrers.Any(r => Regex.IsMatch(referer, r));
                    if (!bHasReferer)
                        errMessage = "Referer check failed";
                }

            }

            if (AllowedIPs.Count > 1) //always has one entry (localhost)
            {
                string sClientIP = req.EndPoint.ToString();

                sClientIP = sClientIP.Substring(0, sClientIP.LastIndexOf(":", StringComparison.Ordinal)).Trim();
                sClientIP = sClientIP.Replace("[", "").Replace("]", "");

                bServe = AllowedIPs.Any(ip => Regex.IsMatch(sClientIP, ip));
                if (!bServe)
                    errMessage = "IP blocked by allow list";
            }

            int iStartPos = sBuffer.IndexOf("HTTP", 1, StringComparison.Ordinal);

            sHttpVersion = sBuffer.Substring(iStartPos, 8);
            sRequest = sBuffer.Substring(0, iStartPos - 1);
            sRequest = sRequest.Replace("\\", "/");

            if (sRequest.IndexOf("command.txt", StringComparison.Ordinal) != -1)
            {
                sRequest = sRequest.Replace("Video/", "Video|");
                sRequest = sRequest.Replace("Audio/", "Audio|");
            }
            
            iStartPos = sRequest.IndexOf("/", StringComparison.Ordinal) + 1;

            sRequestedFile = sRequest.Substring(iStartPos);

            GetDirectoryPath(sRequest, sMyWebServerRoot, out sLocalDir, out sDirName);
            
            if (sLocalDir.Length == 0)
            {
                sErrorMessage = "<H2>Error!! Requested Directory does not exists</H2><Br>";
                SendResponse(sHttpVersion, "", sErrorMessage, " 404 Not Found", 0, req);
                throw new Exception("Requested Directory does not exist (" + sLocalDir + ")");
            }

            ParseMimeType(sRequestedFile, out sFileName, out sMimeType);

            sPhysicalFilePath = (sLocalDir + sRequestedFile).Replace("%20", " ").ToLower();
            
            bool bHasAuth = sRequestedFile.ToLower() == "crossdomain.xml" || CheckAuth(sPhysicalFilePath);


            bServe = (sMimeType != "") && (bServe || (bHasAuth && bHasReferer));
        }

        private void ServeFile(string sHttpVersion, string sFileName, String sMimeType,
                               HttpRequest req)
        {
            var fi = new FileInfo(sFileName);
            int iTotBytes = Convert.ToInt32(fi.Length);

            byte[] bytes;
            var fs = new FileStream(sFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            
            using (var reader = new BinaryReader(fs))
            {
                bytes = new byte[fs.Length];
                while ((reader.Read(bytes, 0, bytes.Length)) != 0)
                {
                }
            }

            SendResponse(sHttpVersion, "", bytes, " 200 OK", 20, req);
        }

        private static string GetVar(string url, string var)
        {
            int i = url.IndexOf("&"+ var + "=", StringComparison.OrdinalIgnoreCase);
            if (i == -1)
                i = url.IndexOf("?" + var + "=", StringComparison.OrdinalIgnoreCase);
            if (i == -1)
            {
                return "";
            }
            var val = url.Substring(i);
            val = val.Substring(val.IndexOf("=", StringComparison.Ordinal) + 1);
            int j = val.IndexOf("&", StringComparison.Ordinal);
            if (j != -1)
                val = val.Substring(0, j);
            return Uri.UnescapeDataString(val);
        }

        private bool CheckAccess(string group, string groups)
        {
            if (!string.IsNullOrEmpty(groups))
            {
                var g = groups.ToLower().Split(',');
                if (g.Contains(group.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }
        
        internal string ProcessCommandInternal(string sRequest)
        {
            string cmd = Uri.UnescapeDataString(sRequest.Trim('/').ToLower().Trim());
            string resp = "";
            
            //hack for axis server commands
            
            if (cmd.StartsWith("get /") || cmd.StartsWith("get ?"))
                cmd = cmd.Substring(5);

            cmd = cmd.Trim('?');

            int i = cmd.IndexOf("?", StringComparison.Ordinal);
            if (i != -1)
                cmd = cmd.Substring(0, i);

            int oid, otid;
            int.TryParse(GetVar(sRequest, "oid"), out oid);
            int.TryParse(GetVar(sRequest, "ot"), out otid);
            string group = GetVar(sRequest, "group");
            
            string func = GetVar(sRequest, "jsfunc").Replace("%27","'");
            string fn = GetVar(sRequest, "fn");          

            if (!string.IsNullOrEmpty(group))
            {
                resp = MainForm.Cameras.Where(cam => CheckAccess(@group, cam.settings.accessgroups)).Aggregate(resp, (current, cam) => DoCommand(sRequest, 2, current, cmd, cam.id, fn, ref func));
                resp = MainForm.Microphones.Where(mic => CheckAccess(@group, mic.settings.accessgroups)).Aggregate(resp, (current, mic) => DoCommand(sRequest, 1, current, cmd, mic.id, fn, ref func));
            }
            else
            {
                resp = DoCommand(sRequest, otid, resp, cmd, oid, fn, ref func);
            }

            if (func!="")
                resp = func.Replace("result", "\"" + resp + "\"");
            return resp;
        }

        private string DoCommand(string sRequest, int otid, string resp, string cmd, int oid, string fn, ref string func)
        {
            string temp = "", folderpath;
            string[] files;
            long sdl = 0, edl = 0;
            string sd, ed;
            int page;
            var io = MainForm.InstanceReference.GetISpyControl(otid, oid);
            switch (cmd)
            {
                case "command.txt": //legacy (test connection)
                case "connect":
                    resp = "{\"auth\":\"" + MainForm.Identifier + "\",\"status\":\"OK\"}";
                    break;
                case "recordswitch":
                    if (io != null)
                    {
                        resp = io.RecordSwitch(!(io.Recording || io.ForcedRecording)) + ",OK";
                    }
                    else
                        resp = "stopped,Control not found,OK";

                    break;
                case "saveobjects":
                {
                    MainForm.InstanceReference.SaveObjectList(false);
                    resp = "OK";
                }
                    break;
                case "reloaddatafile":
                    {
                        if (io != null)
                        {
                            io.LoadFileList();
                        }
                        else
                        {
                            foreach (var c in MainForm.InstanceReference.ControlList)
                            {
                                c.LoadFileList();
                            }
                        }
                        resp = "OK";
                    }
                    break;
                case "savedatafile":
                    {
                        if (io != null)
                        {
                            io.SaveFileList();
                        }
                        else
                        {
                            foreach (var c in MainForm.InstanceReference.ControlList)
                            {
                                c.SaveFileList();
                            }
                        }
                        resp = "OK";
                    }
                    break;
                case "record":
                    if (otid == 1)
                    {
                        VolumeLevel vw = MainForm.InstanceReference.GetVolumeLevel(oid);
                        if (vw != null)
                        {
                            resp = vw.RecordSwitch(true) + ",OK";
                        }
                        else
                            resp = "Microphone not found,OK";
                    }
                    if (otid == 2)
                    {
                        CameraWindow cw = MainForm.InstanceReference.GetCameraWindow(oid);
                        if (cw != null)
                        {
                            resp = cw.RecordSwitch(true) + ",OK";
                        }
                        else
                            resp = "Camera not found,OK";
                    }
                    if (otid == 0)
                    {
                        MainForm.InstanceReference.RecordAll(true);
                    }
                    break;
                case "alert":
                    if (otid == 1)
                    {
                        var vl = MainForm.InstanceReference.GetVolumeLevel(oid);
                        if (vl != null)
                        {
                            vl.Alert(this, EventArgs.Empty);
                            resp = "OK";
                        }
                        else
                            resp = "Microphone not found,OK";
                    }

                    if (otid == 2)
                    {
                        var cw = MainForm.InstanceReference.GetCameraWindow(oid);
                        if (cw != null)
                        {
                            cw.Alert(this, EventArgs.Empty);
                            resp = "OK";
                        }
                        else
                            resp = "Camera not found,OK";
                    }

                    break;
                case "recordoff":
                case "recordstop":
                    if (otid == 1)
                    {
                        var vw = MainForm.InstanceReference.GetVolumeLevel(oid);
                        if (vw != null)
                        {
                            resp = vw.RecordSwitch(false) + ",OK";
                        }
                        else
                            resp = "Microphone not found,OK";
                    }
                    if (otid == 2)
                    {
                        var cw = MainForm.InstanceReference.GetCameraWindow(oid);
                        if (cw != null)
                        {
                            resp = cw.RecordSwitch(false) + ",OK";
                        }
                        else
                            resp = "Camera not found,OK";
                    }
                    if (otid == 0)
                    {
                        MainForm.InstanceReference.RecordAll(false);
                    }
                    break;
                case "snapshot":
                    if (otid == 2)
                    {
                        var cw = MainForm.InstanceReference.GetCameraWindow(oid);
                        cw?.SaveFrame();
                    }
                    else
                    {
                        MainForm.InstanceReference.SnapshotAll();
                    }
                    resp = "OK";
                    break;
                case "ping":
                    resp = "OK";
                    break;
                case "allon":
                    MainForm.InstanceReference.SwitchObjects(false, true);
                    resp = "OK";
                    break;
                case "alloff":
                    MainForm.InstanceReference.SwitchObjects(false, false);
                    resp = "OK";
                    break;
                case "recordondetecton":
                    if (otid == 1)
                    {
                        VolumeLevel vw = MainForm.InstanceReference.GetVolumeLevel(oid);
                        if (vw != null)
                        {
                            vw.Micobject.detector.recordondetect = true;
                            vw.Micobject.detector.recordonalert = false;
                        }
                    }
                    if (otid == 2)
                    {
                        CameraWindow cw = MainForm.InstanceReference.GetCameraWindow(oid);
                        if (cw != null)
                        {
                            cw.Camobject.detector.recordondetect = true;
                            cw.Camobject.detector.recordonalert = false;
                        }
                    }
                    if (otid == 0)
                    {
                        MainForm.InstanceReference.RecordOnDetect(true);
                    }
                    resp = "OK";
                    break;
                case "shutdown":
                    (new Thread(() => MainForm.InstanceReference.ExternalClose())).Start();
                    break;
                case "recordonalerton":
                    if (otid == 1)
                    {
                        VolumeLevel vw = MainForm.InstanceReference.GetVolumeLevel(oid);
                        if (vw != null)
                        {
                            vw.Micobject.detector.recordonalert = true;
                            vw.Micobject.detector.recordondetect = false;
                        }
                    }
                    if (otid == 2)
                    {
                        CameraWindow cw = MainForm.InstanceReference.GetCameraWindow(oid);
                        if (cw != null)
                        {
                            cw.Camobject.detector.recordonalert = true;
                            cw.Camobject.detector.recordondetect = false;
                        }
                    }
                    if (otid == 0)
                    {
                        MainForm.InstanceReference.RecordOnAlert(true);
                    }
                    resp = "OK";
                    break;
                case "recordingoff":
                    if (otid == 1)
                    {
                        VolumeLevel vw = MainForm.InstanceReference.GetVolumeLevel(oid);
                        if (vw != null)
                        {
                            vw.Micobject.detector.recordonalert = false;
                            vw.Micobject.detector.recordondetect = false;
                        }
                    }
                    if (otid == 2)
                    {
                        CameraWindow cw = MainForm.InstanceReference.GetCameraWindow(oid);
                        if (cw != null)
                        {
                            cw.Camobject.detector.recordonalert = false;
                            cw.Camobject.detector.recordondetect = false;
                        }
                    }
                    if (otid == 0)
                    {
                        MainForm.InstanceReference.RecordOnAlert(false);
                        MainForm.InstanceReference.RecordOnDetect(false);
                    }
                    resp = "OK";
                    break;
                case "alerton":
                    if (otid == 1)
                    {
                        VolumeLevel vw = MainForm.InstanceReference.GetVolumeLevel(oid);
                        if (vw != null)
                        {
                            vw.Micobject.alerts.active = true;
                        }
                    }
                    if (otid == 2)
                    {
                        CameraWindow cw = MainForm.InstanceReference.GetCameraWindow(oid);
                        if (cw != null)
                        {
                            cw.Camobject.alerts.active = true;
                        }
                    }
                    if (otid == 0)
                    {
                        MainForm.InstanceReference.AlertsActive(true);
                    }
                    resp = "OK";
                    break;
                case "alertoff":
                    if (otid == 1)
                    {
                        VolumeLevel vw = MainForm.InstanceReference.GetVolumeLevel(oid);
                        if (vw != null)
                        {
                            vw.Micobject.alerts.active = false;
                        }
                    }
                    if (otid == 2)
                    {
                        CameraWindow cw = MainForm.InstanceReference.GetCameraWindow(oid);
                        if (cw != null)
                        {
                            cw.Camobject.alerts.active = false;
                        }
                    }
                    if (otid == 0)
                    {
                        MainForm.InstanceReference.AlertsActive(false);
                    }
                    resp = "OK";
                    break;
                case "setmask":
                    resp = "NOK";
                    if (otid == 2)
                    {
                        CameraWindow cw = MainForm.InstanceReference.GetCameraWindow(oid);

                        if (cw != null)
                        {
                            if (File.Exists(fn))
                            {
                                cw.Camobject.settings.maskimage = fn;
                                try
                                {
                                    cw.Camobject.settings.maskimage = fn;
                                    if (cw.Camera != null)
                                        cw.Camera.Mask = (Bitmap) Image.FromFile(fn);
                                    resp = "OK";
                                }
                                catch
                                {
                                    // ignored
                                }
                            }
                            else
                            {
                                cw.Camobject.settings.maskimage = "";
                                if (cw.Camera != null)
                                    cw.Camera.Mask = null;
                                resp = "Mask not found";
                            }
                        }
                    }
                    break;
                case "allscheduledon":
                    MainForm.InstanceReference.SwitchObjects(true, true);
                    resp = "OK";
                    break;
                case "allscheduledoff":
                    MainForm.InstanceReference.SwitchObjects(true, false);
                    resp = "OK";
                    break;
                case "applyschedule":
                    MainForm.InstanceReference.ApplySchedule();
                    break;
                case "bringonline":
                    io?.Enable();
                    resp = "OK";
                    break;
                case "triggeralarm":
                    io?.Alert(this,EventArgs.Empty);

                    resp = "OK";
                    break;
                case "setframerate":
                    {
                        CameraWindow cw = MainForm.InstanceReference.GetCameraWindow(oid);
                        if (cw != null)
                        {
                            var fr = Convert.ToInt32(GetVar(sRequest, "rate"));
                            if (fr < 1) fr = 1;
                            cw.Camobject.settings.maxframerate = fr;
                        }
                        resp = "OK";
                    }
                    break;
                case "setrecordingframerate":
                    {
                        CameraWindow cw = MainForm.InstanceReference.GetCameraWindow(oid);
                        if (cw != null)
                        {
                            int fr = Convert.ToInt32(GetVar(sRequest, "rate"));
                            if (fr < 1) fr = 1;
                            cw.Camobject.settings.maxframeraterecord = fr;
                        }
                        resp = "OK";
                    }
                    break;
                case "triggerdetect":
                    if (otid == 1)
                    {
                        VolumeLevel vw = MainForm.InstanceReference.GetVolumeLevel(oid);
                        vw?.TriggerDetect(this);
                    }
                    else
                    {
                        CameraWindow cw = MainForm.InstanceReference.GetCameraWindow(oid);
                        cw?.Camera?.TriggerDetect(this);
                    }
                    resp = "OK";
                    break;
                case "triggerplugin":
                {
                    CameraWindow cw = MainForm.InstanceReference.GetCameraWindow(oid);
                    cw?.Camera?.TriggerPlugin();
                }
                    resp = "OK";
                    break;
                case "smscmd":
                case "executecmd":
                    int commandIndex = Convert.ToInt32(GetVar(sRequest, "id"));
                    objectsCommand oc = MainForm.RemoteCommands.SingleOrDefault(p => p.id == commandIndex);

                    if (oc != null)
                    {
                        try
                        {
                            if (oc.command.StartsWith("ispy ") || oc.command.StartsWith("ispypro.exe "))
                            {
                                string cmd2 =
                                    oc.command.Substring(oc.command.IndexOf(" ", StringComparison.Ordinal) + 1).Trim();

                                int k = cmd2.ToLower().IndexOf("commands ", StringComparison.Ordinal);
                                if (k != -1)
                                {
                                    cmd2 = cmd2.Substring(k + 9);
                                }
                                cmd2 = cmd2.Trim('"');
                                string[] commands = cmd2.Split('|');
                                foreach (string command2 in commands)
                                {
                                    if (!string.IsNullOrEmpty(command2))
                                    {
                                        MainForm.ProcessCommandInternal(command2.Trim('"'));
                                    }
                                }
                            }
                            else
                            {
                                Process.Start(oc.command);
                            }

                            resp = "Command Executed.,OK";
                        }
                        catch (Exception ex)
                        {
                            Logger.LogException(ex, "Server");
                            resp = "Command Failed: " + ex.Message + ",OK";
                        }
                    }
                    else
                        resp = "OK";
                    break;
                case "takeoffline":
                    io?.Disable();

                    resp = "OK";
                    break;
                case "deletefile":
                    if (otid == 1)
                    {
                        try
                        {
                            string subdir = Helper.GetDirectory(1, oid);
                            if (subdir != "")
                            {
                                FileOperations.Delete(Helper.GetMediaDirectory(1, oid) + "audio\\" + subdir + @"\" + fn);
                                var vl = MainForm.InstanceReference.GetVolumeLevel(oid);
                                if (vl != null)
                                {
                                    vl.RemoveFile(fn);
                                    MainForm.NeedsMediaRefresh = Helper.Now;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Logger.LogException(e);
                        }
                    }
                    if (otid == 2)
                    {
                        try
                        {
                            string subdir = Helper.GetDirectory(2, oid);
                            if (subdir != "")
                            {
                                FileOperations.Delete(Helper.GetMediaDirectory(2, oid) + "video\\" + subdir + @"\" + fn);
                                var vl = MainForm.InstanceReference.GetCameraWindow(oid);
                                if (vl != null)
                                {
                                    vl.RemoveFile(fn);
                                    MainForm.NeedsMediaRefresh = Helper.Now;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Logger.LogException(e);
                        }
                    }
                    resp = "OK";
                    break;
                case "playfile":
                    {
                        try
                        {
                            if (fn.Contains("../") || fn.Contains(@"..\"))
                            {
                                throw new Exception("Request blocked (directory traversal)");
                            }
                            string d = Helper.GetMediaDirectory(otid, oid);
                            string subdir = Helper.GetDirectory(otid, oid);
                            if (!File.Exists(fn))
                                throw new Exception("File does not exist");

                            var file = new FileInfo(fn);
                            if (!file.DirectoryName.ToLower().StartsWith(d.ToLower()))
                                throw new Exception("Request blocked (outside media directory)");

                            
                            switch (otid)
                            {
                                case 1:
                                    d = d + "audio\\";
                                    break;
                                case 2:
                                    d = d + "video\\";
                                    break;
                            }
                            d += subdir;

                            try
                            {
                                Process.Start(d + @"\" + fn);
                            }
                            catch (Exception ex)
                            {
                                Logger.LogException(ex, "Playback");
                                resp = ex.Message;
                            }
                            resp = "OK";
                        }
                        catch (Exception ex)
                        {
                            Logger.LogException(ex, "Server");
                            resp = ex.Message;
                        }
                    }
                    break;
                case "archive":
                    resp = "Archive failed. Check storage settings.";
                {
                    files = GetVar(sRequest, "filelist").Trim('|').Split('|');
                    string d = "audio";
                    if (otid == 2)
                        d = "video";

                    folderpath = Helper.GetMediaDirectory(otid, oid) + d + "\\" +
                                 Helper.GetDirectory(otid, oid) + "\\";

                    foreach (string fn3 in files)
                    {
                        if (Helper.ArchiveFile(io, folderpath + fn3)!="NOK")
                            resp = "OK";
                        else
                            break;
                    }
                }
                    break;
                case "deleteall":
                    Helper.DeleteAllContent(otid, oid);
                    if (otid == 1)
                    {
                        var vl = MainForm.InstanceReference.GetVolumeLevel(oid);
                        if (vl != null)
                        {
                            vl.ClearFileList();
                            MainForm.NeedsMediaRefresh = Helper.Now;
                        }
                    }

                    if (otid == 2)
                    {
                        var cw = MainForm.InstanceReference.GetCameraWindow(oid);
                        if (cw != null)
                        {
                            cw.ClearFileList();
                            MainForm.NeedsMediaRefresh = Helper.Now;
                        }
                    }
                    resp = "OK";
                    break;
                //case "uploadyoutube":
                //{
                //    bool b;
                //    resp = YouTubeUploader.Upload(oid, Helper.GetFullPath(otid, oid) + fn, out b) + ",OK";
                //}
                //    break;
                case "uploadcloud":
                {
                    bool b;
                    resp = CloudGateway.Upload(otid, oid, Helper.GetFullPath(otid, oid) + fn, out b) + ",OK";
                }
                    break;
                case "kinect_tilt_up":
                {
                    var c = MainForm.InstanceReference.GetCameraWindow(oid);
                    if (c != null)
                    {
                        try
                        {
                            ((KinectStream) c.Camera.VideoSource).Tilt += 4;
                        }
                        catch (Exception ex)
                        {
                            Logger.LogException(ex, "Server");
                        }
                    }

                    resp = "OK";
                }
                    break;
                case "kinect_tilt_down":
                {
                    var c = MainForm.InstanceReference.GetCameraWindow(oid);
                    if (c != null)
                    {
                        try
                        {
                            ((KinectStream) c.Camera.VideoSource).Tilt -= 4;
                        }
                        catch (Exception ex)
                        {
                            Logger.LogException(ex, "Server");
                        }
                    }
                    resp = "OK";
                }
                    break;
                case "removeobject":
                    if (otid == 1)
                    {
                        VolumeLevel vw = MainForm.InstanceReference.GetVolumeLevel(oid);
                        if (vw != null)
                        {
                            MainForm.InstanceReference.RemoveMicrophone(vw, false);
                        }
                    }
                    else
                    {
                        CameraWindow cw = MainForm.InstanceReference.GetCameraWindow(oid);
                        if (cw != null)
                        {
                            MainForm.InstanceReference.RemoveCamera(cw, false);
                        }
                    }
                    MainForm.NeedsSync = true;
                    resp = "OK";
                    break;
                case "addobject":
                    int sourceIndex = Convert.ToInt32(GetVar(sRequest, "stid"));
                    int width = Convert.ToInt32(GetVar(sRequest, "w"));
                    int height = Convert.ToInt32(GetVar(sRequest, "h"));
                    string name = GetVar(sRequest, "name");
                    string url = GetVar(sRequest, "url").Replace("\\", "/");
                    MainForm.InstanceReference.AddObjectExternal(otid, sourceIndex, width, height, name, url);
                    MainForm.NeedsSync = true;
                    resp = "OK";
                    break;
                case "synthtocam":
                {
                    var txt = GetVar(sRequest, "text");                
                    var cw = MainForm.InstanceReference.GetCameraWindow(oid);
                    if (cw != null)
                    {
                        SpeechSynth.Say(txt,cw);
                    }
                    resp = "OK";
                }
                break;
                case "changesetting":
                    string field = GetVar(sRequest, "field");
                    string value = GetVar(sRequest, "value");

                    if (otid == 1)
                    {
                        VolumeLevel vw = MainForm.InstanceReference.GetVolumeLevel(oid);
                        switch (field)
                        {
                            case "notifyondisconnect":
                                vw.Micobject.settings.notifyondisconnect = Convert.ToBoolean(value);
                                break;
                            case "recordondetect":
                                vw.Micobject.detector.recordondetect = Convert.ToBoolean(value);
                                if (vw.Micobject.detector.recordondetect)
                                    vw.Micobject.detector.recordonalert = false;
                                break;
                            case "recordonalert":
                                vw.Micobject.detector.recordonalert = Convert.ToBoolean(value);
                                if (vw.Micobject.detector.recordonalert)
                                    vw.Micobject.detector.recordondetect = false;
                                break;
                            case "recordoff":
                                vw.Micobject.detector.recordonalert = false;
                                vw.Micobject.detector.recordondetect = false;
                                break;
                            case "scheduler":
                                vw.Micobject.schedule.active = Convert.ToBoolean(value);
                                break;
                            case "alerts":
                                vw.Micobject.alerts.active = Convert.ToBoolean(value);
                                break;
                                //case "sendemailonalert":
                                //    vw.Micobject.notifications.sendemail = Convert.ToBoolean(value);
                                //    break;
                                //case "sendsmsonalert":
                                //    vw.Micobject.notifications.sendsms = Convert.ToBoolean(value);
                                //    break;
                            case "minimuminterval":
                                int mi;
                                int.TryParse(value, out mi);
                                vw.Micobject.alerts.minimuminterval = mi;
                                break;
                            case "accessgroups":
                                vw.Micobject.settings.accessgroups = value;
                                break;
                            case "name":
                                vw.Micobject.name = value;
                                break;
                        }
                    }
                    else
                    {
                        CameraWindow cw = MainForm.InstanceReference.GetCameraWindow(oid);
                        switch (field)
                        {
                            case "notifyondisconnect":
                                cw.Camobject.settings.notifyondisconnect = Convert.ToBoolean(value);
                                break;
                            case "ftpenabled":
                            case "ftp":
                                cw.Camobject.ftp.enabled = Convert.ToBoolean(value);
                                break;
                            case "recordondetect":
                                cw.Camobject.detector.recordondetect = Convert.ToBoolean(value);
                                if (cw.Camobject.detector.recordondetect)
                                    cw.Camobject.detector.recordonalert = false;
                                break;
                            case "recordonalert":
                                cw.Camobject.detector.recordonalert = Convert.ToBoolean(value);
                                if (cw.Camobject.detector.recordonalert)
                                    cw.Camobject.detector.recordondetect = false;
                                break;
                            case "ftprecordings":
                                cw.Camobject.recorder.ftpenabled = Convert.ToBoolean(value);
                                break;
                            case "recordoff":
                                cw.Camobject.detector.recordonalert = false;
                                cw.Camobject.detector.recordondetect = false;
                                break;
                            case "scheduler":
                                cw.Camobject.schedule.active = Convert.ToBoolean(value);
                                break;
                            case "alerts":
                                cw.Camobject.alerts.active = Convert.ToBoolean(value);
                                break;
                            case "timelapseon":
                                cw.Camobject.recorder.timelapseenabled = Convert.ToBoolean(value);
                                break;
                            case "timelapse":
                                int tl;
                                int.TryParse(value, out tl);
                                cw.Camobject.recorder.timelapse = Math.Max(tl, 1);
                                break;
                            case "timelapseframes":
                                int tlf;
                                int.TryParse(value, out tlf);
                                cw.Camobject.recorder.timelapseframes = Math.Max(tlf,1);
                                break;
                            case "maxframerate":
                                int mfr;
                                int.TryParse(value, out mfr);
                                cw.Camobject.settings.maxframerate = Math.Max(mfr,1);
                                break;
                            case "maxframeraterecord":
                                int mfrr;
                                int.TryParse(value, out mfrr);
                                cw.Camobject.settings.maxframeraterecord = Math.Max(mfrr,1);
                                break;                                    
                            case "localsaving":
                                cw.Camobject.ftp.savelocal = Convert.ToBoolean(value);
                                break;
                            case "cloudimages":
                                cw.Camobject.settings.cloudprovider.images = Convert.ToBoolean(value);
                                break;
                            case "cloudrecording":
                                cw.Camobject.settings.cloudprovider.recordings = Convert.ToBoolean(value);
                                break;                                
                            case "ptz":
                                if (value != "")
                                {
                                    try
                                    {
                                        if (value.StartsWith("ispydir_"))
                                        {
                                            cw.PTZ.SendPTZCommand(
                                                (Enums.PtzCommand) Convert.ToInt32(value.Replace("ispydir_", "")));
                                        }
                                        else
                                            cw.PTZ.SendPTZCommand(value);
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.LogError(LocRm.GetString("Validate_Camera_PTZIPOnly") + ": " +
                                                                ex.Message, "Server");
                                    }
                                }
                                break;
                            case "minimuminterval":
                                int mi;
                                int.TryParse(value, out mi);
                                cw.Camobject.alerts.minimuminterval = mi;
                                break;
                            case "accessgroups":
                                cw.Camobject.settings.accessgroups = value;
                                break;
                            case "name":
                                cw.Camobject.name = value;
                                break;
                        }
                    }
                    resp = "OK";
                    break;
                case "getcontentlist":
                    page = Convert.ToInt32(GetVar(sRequest, "page"));

                    sd = GetVar(sRequest, "startdate");
                    ed = GetVar(sRequest, "enddate");
                    int pageSize = Convert.ToInt32(GetVar(sRequest, "pagesize"));
                    int ordermode = Convert.ToInt32(GetVar(sRequest, "ordermode"));
                    if (sd != "")
                        sdl = Convert.ToInt64(sd);
                    if (ed != "")
                        edl = Convert.ToInt64(ed);


                    switch (otid)
                    {
                        case 1:
                            VolumeLevel vl = MainForm.InstanceReference.GetVolumeLevel(oid);
                            if (vl != null)
                            {
                                List<FilesFile> lFi = vl.FileList.Where(f => f.Filename.EndsWith(".mp3")).ToList();
                                if (sdl > 0)
                                    lFi = lFi.FindAll(f => f.CreatedDateTicks > sdl).ToList();
                                if (edl > 0)
                                    lFi = lFi.FindAll(f => f.CreatedDateTicks < edl).ToList();
                                func = func.Replace("resultcount", lFi.Count.ToString(CultureInfo.InvariantCulture));

                                switch (ordermode)
                                {
                                    case 1:
                                        //default
                                        break;
                                    case 2:
                                        lFi = lFi.OrderByDescending(p => p.DurationSeconds).ToList();
                                        break;
                                    case 3:
                                        lFi = lFi.OrderByDescending(p => p.MaxAlarm).ToList();
                                        break;
                                    case 4:
                                        lFi = lFi.OrderByDescending(p => p.CreatedDateTicks).ToList();
                                        break;
                                }


                                var lResults = lFi.Skip(pageSize*page).Take(pageSize).ToList();
                                temp = lResults.Aggregate("",
                                                          (current, fi) =>
                                                          current +
                                                          (fi.Filename + "|" + FormatBytes(fi.SizeBytes) + "|" +
                                                           String.Format(
                                                               CultureInfo.InvariantCulture,
                                                               "{0:0.000}", fi.MaxAlarm) + ","));
                                resp = temp.Trim(',');
                            }
                            break;
                        case 2:
                            CameraWindow cw = MainForm.InstanceReference.GetCameraWindow(oid);
                            if (cw != null)
                            {
                                List<FilesFile> lFi2 = cw.FileList.ToList();
                                if (sdl > 0)
                                    lFi2 = lFi2.FindAll(f => f.CreatedDateTicks > sdl).ToList();
                                if (edl > 0)
                                    lFi2 = lFi2.FindAll(f => f.CreatedDateTicks < edl).ToList();
                                func = func.Replace("resultcount", lFi2.Count.ToString(CultureInfo.InvariantCulture));

                                switch (ordermode)
                                {
                                    case 1:
                                        //default
                                        break;
                                    case 2:
                                        lFi2 = lFi2.OrderByDescending(p => p.DurationSeconds).ToList();
                                        break;
                                    case 3:
                                        lFi2 = lFi2.OrderByDescending(p => p.MaxAlarm).ToList();
                                        break;
                                    case 4:
                                        lFi2 = lFi2.OrderByDescending(p => p.CreatedDateTicks).ToList();
                                        break;
                                }

                                var lResults2 = lFi2.Skip(pageSize*page).Take(pageSize).ToList();
                                temp = lResults2.Aggregate("",
                                                           (current, fi) =>
                                                           current +
                                                           (fi.Filename + "|" + FormatBytes(fi.SizeBytes) + "|" +
                                                            string.Format(
                                                                CultureInfo.InvariantCulture,
                                                                "{0:0.000}", fi.MaxAlarm) + ","));
                                resp = temp.Trim(',');
                            }
                            break;
                    }
                    break;
                case "getcontentcounts":
                    sd = GetVar(sRequest, "startdate");
                    ed = GetVar(sRequest, "enddate");
                    if (sd != "")
                        sdl = Convert.ToInt64(sd);
                    if (ed != "")
                        edl = Convert.ToInt64(ed);
                    string oclall = "";
                    foreach (objectsCamera oc1 in MainForm.Cameras)
                    {
                        CameraWindow cw = MainForm.InstanceReference.GetCameraWindow(oc1.id);

                        List<FilesFile> lFi2 = cw.FileList.ToList();
                        if (sdl > 0)
                            lFi2 = lFi2.FindAll(f => f.CreatedDateTicks > sdl).ToList();
                        if (edl > 0)
                            lFi2 = lFi2.FindAll(f => f.CreatedDateTicks < edl).ToList();
                        oclall += "2," + oc1.id + "," + lFi2.Count + "|";
                    }
                    foreach (objectsMicrophone om1 in MainForm.Microphones)
                    {
                        VolumeLevel vl = MainForm.InstanceReference.GetVolumeLevel(om1.id);
                        List<FilesFile> lFi = vl.FileList.Where(f => f.Filename.EndsWith(".mp3")).ToList();
                        if (sdl > 0)
                            lFi = lFi.FindAll(f => f.CreatedDateTicks > sdl).ToList();
                        if (edl > 0)
                            lFi = lFi.FindAll(f => f.CreatedDateTicks < edl).ToList();
                        oclall += "1," + om1.id + "," + lFi.Count + "|";
                    }
                    resp = oclall.Trim('|');
                    break;
                case "getfloorplanalerts2":
                    {
                        string cfg = "";

                        foreach (objectsFloorplan ofp in MainForm.FloorPlans)
                        {
                            FloorPlanControl fpc = MainForm.InstanceReference.GetFloorPlan(ofp.id);
                            if (fpc?.ImgPlan != null)
                            {
                                var lat = fpc.LastAlertTimestamp;
                                var lrt = fpc.LastRefreshTimestamp;
                                cfg += "{oid:" + ofp.id + ",alertTimestamp:" +
                                       lat.ToString(CultureInfo.InvariantCulture) +
                                       ",refreshTimestamp:" + lrt.ToString(CultureInfo.InvariantCulture) +
                                       ",last_oid:" + fpc.LastOid +
                                       ",last_otid:" + fpc.LastOtid + "},";
                            }
                        }
                        func = func.Replace("data", "[" + cfg.Trim(',') + "]");
                    }

                    resp = "OK";
                    break;
                case "getfloorplans2":
                    {
                        string cfg = "";

                        foreach (objectsFloorplan ofp in MainForm.FloorPlans)
                        {
                            FloorPlanControl fpc = MainForm.InstanceReference.GetFloorPlan(ofp.id);
                            if (fpc?.ImgPlan != null)
                            {
                                var lat = fpc.LastAlertTimestamp;
                                var lrt = fpc.LastRefreshTimestamp;

                                cfg += "{oid: " + ofp.id + ", name: \"" +
                                       ofp.name.Replace("\"", "") + "\", refreshTimestamp: " +
                                       lrt.ToString(CultureInfo.InvariantCulture) + ", alertTimestamp: " +
                                       lat.ToString(CultureInfo.InvariantCulture) + ", width:" +
                                       fpc.ImageWidth + ", height:" + fpc.ImageHeight + ", groups:\"" +
                                       ofp.accessgroups.Replace("\n", " ").Replace("\"", "") + "\",areas:[";

                                cfg += ofp.objects.@object.Aggregate(temp,
                                                                     (current, ofpo) =>
                                                                     current +
                                                                     ("{oid: " + ofpo.id + ",ot: " +
                                                                      (ofpo.type == "camera" ? 2 : 1) + ", x:" + (ofpo.x) +
                                                                      ",y:" + (ofpo.y) + "},"));
                                cfg = cfg.Trim(',');
                                cfg += "]},";
                            }
                        }
                        func = func.Replace("data", "[" + cfg.Trim(',') + "]");
                    }
                    resp = "OK";
                    break;
                case "getgraph":
                    FilesFile ff = null;
                    switch (otid)
                    {
                        case 1:
                            VolumeLevel vl = MainForm.InstanceReference.GetVolumeLevel(oid);
                            if (vl != null)
                            {
                                ff = vl.FileList.FirstOrDefault(p => p.Filename == fn);
                            }
                            break;
                        case 2:
                            CameraWindow cw = MainForm.InstanceReference.GetCameraWindow(oid);
                            if (cw != null)
                            {
                                ff = cw.FileList.FirstOrDefault(p => p.Filename == fn);
                            }
                            break;
                    }
                    if (ff != null)
                    {
                        func = func.Replace("data", "\"" + ff.AlertData + "\"");
                        func = func.Replace("duration", "\"" + ff.DurationSeconds + "\"");
                        func = func.Replace("threshold",
                                            String.Format(CultureInfo.InvariantCulture, "{0:0.000}",
                                                          ff.TriggerLevel));
                    }
                    else
                    {
                        func = func.Replace("data", "\"\"");
                        func = func.Replace("duration", "0");
                        func = func.Replace("threshold", "0");
                    }
                    resp = "OK";
                    break;
                case "graphall":
                    {
                        List<FilesFile> ffs = null;
                        switch (otid)
                        {
                            case 1:
                                VolumeLevel vl = MainForm.InstanceReference.GetVolumeLevel(oid);
                                if (vl != null)
                                {
                                    ffs = vl.FileList.ToList();
                                }
                                break;
                            case 2:
                                CameraWindow cw = MainForm.InstanceReference.GetCameraWindow(oid);
                                if (cw != null)
                                {
                                    ffs = cw.FileList.ToList();
                                }
                                break;
                        }
                        if (ffs != null)
                        {
                            sd = GetVar(sRequest, "startdate");
                            ed = GetVar(sRequest, "enddate");

                            if (sd != "")
                                sdl = Convert.ToInt64(sd);
                            if (ed != "")
                                edl = Convert.ToInt64(ed);

                            if (sdl > 0)
                                ffs = ffs.FindAll(f => f.CreatedDateTicks > sdl).ToList();
                            if (edl > 0)
                                ffs = ffs.FindAll(f => f.CreatedDateTicks < edl).ToList();

                            var sb = new StringBuilder();
                            foreach (FilesFile f in ffs)
                            {
                                sb.Append((f.CreatedDateTicks.UnixTicks())).Append("|").Append(
                                    String.Format(CultureInfo.InvariantCulture, "{0:0.000}", f.MaxAlarm)).Append("|").Append(
                                        f.DurationSeconds.ToString(CultureInfo.InvariantCulture)).Append("|").Append(f.Filename)
                                    .Append(",");
                            }
                            temp = sb.ToString();
                            func = func.Replace("data", "\"" + temp.Trim(',') + "\"");
                        }
                        else
                        {
                            func = func.Replace("data", "\"\"");
                        }
                        resp = "OK";
                    }
                    break;
                case "getevents":
                    {
                        string num = GetVar(sRequest, "num");
                        if (num == "")
                            num = "500";
                        int n = Convert.ToInt32(num);


                        List<FilePreview> ffs =
                            MainForm.MasterFileList.OrderByDescending(p => p.CreatedDateTicks).ToList();

                        sd = GetVar(sRequest, "startdate");
                        ed = GetVar(sRequest, "enddate");

                        sdl = sd != "" ? Convert.ToInt64(sd) : 0;
                        edl = ed != "" ? Convert.ToInt64(ed) : long.MaxValue;

                        if (sdl > 0)
                            ffs = ffs.FindAll(f => f.CreatedDateTicks > sdl); //.ToList();
                        if (edl < long.MaxValue)
                            ffs = ffs.FindAll(f => f.CreatedDateTicks < edl); //.ToList();


                        //return max of 1000 at a time
                        ffs = ffs.Take(n).ToList();
                        var sb = new StringBuilder();
                        sb.Append("[");
                        foreach (var f in ffs)
                        {
                            sb.Append("{ot:");
                            sb.Append(f.ObjectTypeId);
                            sb.Append(",oid:");
                            sb.Append(f.ObjectId);
                            sb.Append(",created:");
                            sb.Append(String.Format(CultureInfo.InvariantCulture, "{0:0.00}",
                                                    f.CreatedDateTicks.UnixTicks()));
                            sb.Append(",maxalarm:");
                            sb.Append(String.Format(CultureInfo.InvariantCulture, "{0:0.0}",
                                                    f.MaxAlarm));
                            sb.Append(",duration: ");
                            sb.Append(f.Duration);
                            sb.Append(",filename:\"");
                            sb.Append(f.Filename);
                            sb.Append("\"},");
                        }
                        temp = sb.ToString().Trim(',') + "]";
                        func = func.Replace("data", temp);
                    }
                    resp = "OK";
                    break;
                case "editgridview":
                    {
                        string index = GetVar(sRequest, "index");
                        var cids = GetVar(sRequest, "ids").Split(',').Where(x => int.TryParse(x, out _)).Select(int.Parse).ToList();
                        int ind = Convert.ToInt32(index);
                        var cg = MainForm.Conf.GridViews.ToList()[ind];

                        int cols = Convert.ToInt32(Math.Ceiling(Math.Sqrt(cids.Count())));
                        int rows = Convert.ToInt32(Math.Ceiling(cids.Count() / Convert.ToDouble(cols)));

                        if (cg != null)
                        {
                            cg.Columns = cols;
                            cg.Rows = rows;
                            var gi = new List<configurationGridGridItem>();
                            int j = 0;
                            foreach(var id in cids)
                            {
                                if (id > 0)
                                {
                                    gi.Add(new configurationGridGridItem() { GridIndex = j, CycleDelay = 4, Item = new configurationGridGridItemItem[] { new configurationGridGridItemItem() { ObjectID = id, TypeID = 2 } } });
                                }
                                j++;
                            }
                            cg.GridItem = gi.ToArray();
                            MainForm.Conf.GridViews[ind] = cg;
                        }

                        MainForm.InstanceReference.ShowGridViewRemote(index);
                        resp = "OK";
                    }
                    break;
                case "showgridview":
                    {
                        string index = GetVar(sRequest, "index");
                        MainForm.InstanceReference.ShowGridViewRemote(index);
                        resp = "OK";
                    }
                    break;
                case "closegridview":
                    {
                        string index = GetVar(sRequest, "index");
                        MainForm.InstanceReference.CloseGridViewRemote(index);
                        resp = "OK";
                    }
                    break;
                case "setresize":
                    {
                        var cw = MainForm.InstanceReference.GetCameraWindow(oid);
                        bool resize = GetVar(sRequest, "resize") != "false";
                        if (cw != null)
                        {
                            if (cw.Camobject.settings.resize != resize)
                            {
                                cw.Camobject.settings.resize = resize;
                                cw.Restart();
                            }
                        }
                        resp = "OK";
                    }
                    break;
                case "enablegridcameras":
                    {
                        string index = GetVar(sRequest, "index");
                        var cids = GetVar(sRequest, "ids").Split(',').Where(x => int.TryParse(x, out _)).Select(int.Parse).ToList();
                        int ind = Convert.ToInt32(index);
                        var cg = MainForm.Conf.GridViews.ToList()[ind];
                        int cols = cg.Columns;
                        int rows = cg.Rows;

                        if (cg != null)
                        {
                            foreach(var gi in cg.GridItem)
                            {
                                if (gi.Item != null && gi.Item.Length > 0)
                                {
                                    foreach (var i in gi.Item)
                                    {
                                        CameraWindow cw = MainForm.InstanceReference.GetCameraWindow(i.ObjectID);
                                        if (cw != null)
                                        {
                                            if (cids.Contains(i.ObjectID))
                                            {
                                                cw.Enable();
                                            }
                                            else
                                                cw.Disable();
                                        }
                                    }
                                }
                            }
                        }
                        resp = "OK";
                    }
                    break;
                case "getgrabs":
                    sd = GetVar(sRequest, "startdate");
                    ed = GetVar(sRequest, "enddate");

                    if (sd != "")
                        sdl = Convert.ToInt64(sd);
                    if (ed != "")
                        edl = Convert.ToInt64(ed);

                    string grabs = "";
                    foreach (objectsCamera oc1 in MainForm.Cameras)
                    {
                        var dirinfo = new DirectoryInfo(Helper.GetMediaDirectory(2, oc1.id) + "video\\" +
                                                        oc1.directory + "\\grabs\\");

                        var lFi = new List<FileInfo>();
                        lFi.AddRange(dirinfo.GetFiles());
                        lFi =
                            lFi.FindAll(
                                f =>
                                f.Extension.ToLower() == ".jpg" && (sdl == 0 || f.CreationTime.Ticks > sdl) &&
                                (edl == 0 || f.CreationTime.Ticks < edl));
                        lFi = lFi.OrderByDescending(f => f.CreationTime).ToList();

                        int max = 25;
                        if (lFi.Count > 0)
                        {
                            foreach (var f in lFi)
                            {
                                grabs += (oc1.name + "|" + oc1.id + "|" + f.Name + ",");
                                max--;
                                if (max == 0)
                                    break;
                            }
                        }
                    }
                    func = func.Replace("data", "\"" + grabs.Trim(',') + "\"");
                    resp = "OK";
                    break;
                case "getlogfilelist":
                    {
                        var dirinfo = new DirectoryInfo(Program.AppDataPath);
                        var lFi = new List<FileInfo>();
                        lFi.AddRange(dirinfo.GetFiles());
                        lFi = lFi.FindAll(f => f.Extension.ToLower() == ".htm" && f.Name.StartsWith("log_"));
                        lFi = lFi.OrderByDescending(f => f.CreationTime).ToList();
                        string logs = lFi.Aggregate("", (current, f) => current + (f.Name + ","));
                        func = func.Replace("data", "\"" + logs.Trim(',') + "\"");
                        resp = "OK";
                    }
                    break;
                case "getcameragrabs":
                {
                    sd = GetVar(sRequest, "startdate");
                    ed = GetVar(sRequest, "enddate");
                    int pagesize = Convert.ToInt32(GetVar(sRequest, "pagesize"));
                    page = Convert.ToInt32(GetVar(sRequest, "page"));
                    if (sd != "")
                        sdl = Convert.ToInt64(sd);
                    if (ed != "")
                        edl = Convert.ToInt64(ed);

                    var grablist = new StringBuilder("");
                    var ocgrab = MainForm.Cameras.FirstOrDefault(p => p.id == oid);
                    if (ocgrab != null)
                    {
                        var dirinfo = new DirectoryInfo(Helper.GetMediaDirectory(2, oid) + "video\\" +
                                                        ocgrab.directory + "\\grabs\\");

                        var lFi = new List<FileInfo>();
                        lFi.AddRange(dirinfo.GetFiles());
                        lFi =
                            lFi.FindAll(
                                f =>
                                    f.Extension.ToLower() == ".jpg" && (sdl == 0 || f.CreationTime.Ticks > sdl) &&
                                    (edl == 0 || f.CreationTime.Ticks < edl));
                        lFi = lFi.OrderByDescending(f => f.CreationTime).ToList();
                        func = func.Replace("total", lFi.Count.ToString(CultureInfo.InvariantCulture));
                        lFi = lFi.Skip(page*pagesize).Take(pagesize).ToList();

                        int max = 10000;
                        if (lFi.Count > 0)
                        {
                            foreach (var f in lFi)
                            {
                                grablist.Append(f.Name);
                                grablist.Append(",");
                                max--;
                                if (max == 0)
                                    break;
                            }
                        }
                    }
                    else
                        func = func.Replace("total", "0");
                    func = func.Replace("data", "\"" + grablist.ToString().Trim(',') + "\"");
                    resp = "OK";
                }
                    break;                    
                case "getptzcommands":
                    int ptzid = Convert.ToInt32(GetVar(sRequest, "ptzid"));
                    string cmdlist = "";

                    switch (ptzid)
                    {
                        default:
                            PTZSettings2Camera ptz = MainForm.PTZs.SingleOrDefault(p => p.id == ptzid);
                            if (ptz?.ExtendedCommands?.Command != null)
                            {
                                cmdlist = ptz.ExtendedCommands.Command.Aggregate("",
                                    (current, extcmd) =>
                                        current +
                                        ("<option value=\\\"" + Uri.EscapeDataString(extcmd.Value) +
                                         "\\\">" + extcmd.Name.Trim() +
                                         "</option>"));
                            }
                            break;
                        case -2:
                        case -1: //digital (none)
                        case -6: //(none)
                            break;
                        case -3:
                        case -4:
                            cmdlist = PTZController.PelcoCommands.Aggregate(cmdlist,
                                                                            (current, c) =>
                                                                            current +
                                                                            ("<option value=\\\"" + Uri.EscapeDataString(c) + "\\\">" + c +
                                                                             "</option>"));
                            break;
                        case -5:
                            CameraWindow cw = MainForm.InstanceReference.GetCameraWindow(oid);
                            if (cw?.PTZ?.ONVIFPresets.Length > 0)
                            {
                                cmdlist = cw.PTZ.ONVIFPresets.Aggregate(cmdlist,
                                    (current, c) =>
                                        current +
                                        ("<option value=\\\"" + Uri.EscapeDataString(c.Name) + "\\\">" + c +
                                         "</option>"));
                            }
                            break;
                    }

                    func = func.Replace("data", "\"" + cmdlist.Trim(',') + "\"");
                    resp = "OK";
                    break;
                case "massdeletegrabs":
                    files = GetVar(sRequest, "filelist").Trim('|').Split('|');

                    folderpath = Helper.GetMediaDirectory(otid, oid) + "video\\" +
                                 Helper.GetDirectory(otid, oid) + "\\grabs\\";

                    foreach (string fn3 in files)
                    {
                        FileOperations.Delete(folderpath + fn3);
                    }
                    resp = "OK";
                    break;
                case "closeaudio":
                    //deprecated
                    resp = "OK";
                    break;
                case "stoplisten":
                    {
                        VolumeLevel vw = MainForm.InstanceReference.GetVolumeLevel(oid);
                        if (vw != null && vw.Listening)
                        {
                            var cc = vw.CameraControl;
                            if (cc != null && vw.Listening)
                                cc.Listen(); //switch off
                            else
                                vw.Listening = false;
                        }
                    }
                    resp = "OK";
                    break;
                case "startlisten":
                    {
                        VolumeLevel vw = MainForm.InstanceReference.GetVolumeLevel(oid);
                        if (vw != null && !vw.Listening)
                        {
                            var cc = vw.CameraControl;
                            if (cc != null)
                            {
                                cc.Listen();
                            }
                            else
                                vw.Listening = true;
                        }
                    }
                    resp = "OK";
                    break;
                case "stoptalk":
                    {
                        CameraWindow cw = MainForm.InstanceReference.GetCameraWindow(oid);
                        if (cw != null && cw.Talking)
                        {
                            cw.Talk();
                        }
                    }
                    resp = "OK";
                    break;
                case "starttalk":
                    {
                        CameraWindow cw = MainForm.InstanceReference.GetCameraWindow(oid);
                        if (cw != null && !cw.Talking)
                        {
                            cw.Talk();
                        }
                    }
                    resp = "OK";
                    break;
                case "massdelete":
                    files = GetVar(sRequest, "filelist").Trim('|').Split('|');
                    string dir = "audio";
                    if (otid == 2)
                        dir = "video";

                    folderpath = Helper.GetMediaDirectory(otid, oid) + dir + "\\" +
                                 Helper.GetDirectory(otid, oid) + "\\";

                    VolumeLevel vlUpdate = null;
                    CameraWindow cwUpdate = null;
                    if (otid == 1)
                    {
                        vlUpdate = MainForm.InstanceReference.GetVolumeLevel(oid);
                        if (vlUpdate == null)
                        {
                            resp = "OK";
                            break;
                        }
                    }
                    if (otid == 2)
                    {
                        cwUpdate = MainForm.InstanceReference.GetCameraWindow(oid);
                        if (cwUpdate == null)
                        {
                            resp = "OK";
                            break;
                        }
                    }
                    foreach (string fn3 in files)
                    {
                        var fi = new FileInfo(folderpath +
                                              fn3);
                        string ext = fi.Extension.Trim();
                        FileOperations.Delete(folderpath + fn3);
                        if (otid == 2)
                        {
                            FileOperations.Delete(folderpath + "thumbs\\" + fn3.Replace(ext, ".jpg"));
                            FileOperations.Delete(folderpath + "thumbs\\" + fn3.Replace(ext, "_large.jpg"));
                        }
                        string filename1 = fn3;
                        if (otid == 1)
                        {
                            vlUpdate?.RemoveFile(filename1);
                        }
                        if (otid == 2)
                        {
                            cwUpdate?.RemoveFile(filename1);
                        }
                    }
                    MainForm.NeedsMediaRefresh = Helper.Now;
                    resp = "OK";
                    break;
                case "getobjectlist":
                    //for 3rd party APIs
                    resp = GetObjectList(otid,oid);
                    break;
                case "getservername":
                    resp = MainForm.Conf.ServerName + ",OK";
                    break;
                case "getcontrolpanel":
                    int port = Convert.ToInt32(GetVar(sRequest, "port"));

                    string disabled = "";
                    if (!MainForm.Conf.Subscribed)
                        disabled = " disabled=\"disabled\" title=\"Not Subscribed\"";

                    if (otid == 1)
                    {
                        VolumeLevel vw = MainForm.InstanceReference.GetVolumeLevel(oid);
                        string html = "<table cellspacing=\"3px\">";
                        string strChecked = "";


                        if (vw.Micobject.alerts.active) strChecked = "checked=\"checked\"";
                        html += "<tr><td colspan=\"2\"><strong>" + LocRm.GetString("Alerts") + "</strong></td></tr>";
                        html += "<tr><td>" + LocRm.GetString("AlertsEnabled") +
                                "</td><td><input type=\"checkbox\" onclick=\"send_changesetting(" + otid + "," +
                                oid + "," + port + ",'alerts',this.checked)\" " + strChecked + "/></td></tr>";

                        strChecked = "";
                        if (vw.Micobject.settings.notifyondisconnect) strChecked = "checked=\"checked\"";

                        html += "<tr><td>" + LocRm.GetString("SendEmailOnDisconnect") + "</td><td><input type=\"checkbox\"" +
                                disabled + " onclick=\"send_changesetting(" + otid + "," + oid + "," + port +
                                ",'notifyondisconnect',this.checked)\" " + strChecked + "/></td></tr>";

                        html += "<tr><td>" + LocRm.GetString("DistinctAlertInterval") +
                                "</td><td><input style=\"width:50px\" type=\"text\" value=\"" +
                                vw.Micobject.alerts.minimuminterval + "\" onblur=\"send_changesetting(" + otid + "," +
                                oid + "," + port + ",'minimuminterval',this.value)\"/> " + LocRm.GetString("Seconds") +
                                "</td></tr>";

                        html += "<tr><td colspan=\"2\"><strong>" + LocRm.GetString("AccessGroups") + "</strong></td></tr>";
                        html += "<tr><td>" + LocRm.GetString("AccessGroups") +
                                "</td><td><input style=\"width:100px\" type=\"text\" value=\"" +
                                vw.Micobject.settings.accessgroups + "\" onblur=\"send_changesetting(" + otid + "," +
                                oid + "," + port + ",'accessgroups',this.value)\"/></td></tr>";

                        html += "<tr><td colspan=\"2\"><strong>" + LocRm.GetString("Scheduler") + "</strong></td></tr>";
                        strChecked = "";
                        if (vw.Micobject.schedule.active) strChecked = "checked=\"checked\"";

                        html += "<tr><td>" + LocRm.GetString("ScheduleActive") +
                                "</td><td><input type=\"checkbox\" onclick=\"send_changesetting(" + otid + "," +
                                oid + "," + port + ",'scheduler',this.checked)\" " + strChecked + "/>";

                        string schedule = vw.ScheduleDetails.Where(s => s != "").Aggregate("", (current, s) => current + (s + "<br/>"));
                        if (schedule != "")
                            html +=
                                "<div style=\"width:450px;height:100px;overflow-y:auto;background-color:#ddd;padding:5px\">" +
                                schedule + "</div>";
                        html += "</td></tr>";

                        html += "<tr><td colspan=\"2\"><strong>" + LocRm.GetString("RecordingSettings") + "</strong></td></tr>";

                        strChecked = "";

                        if (!vw.Micobject.detector.recordondetect && !vw.Micobject.detector.recordondetect)
                            strChecked = "checked=\"checked\"";

                        html += "<tr><td>" + LocRm.GetString("NoRecord") +
                                "</td><td><input type=\"radio\" name=\"record_opts\" onclick=\"send_changesetting(" + otid + "," +
                                oid + "," + port + ",'recordoff',this.checked)\" " + strChecked + "/></td></tr>";

                        strChecked = "";
                        if (vw.Micobject.detector.recordondetect) strChecked = "checked=\"checked\"";

                        html += "<tr><td>" + LocRm.GetString("RecordOnDetect") +
                                "</td><td><input type=\"radio\" name=\"record_opts\" onclick=\"send_changesetting(" + otid + "," +
                                oid + "," + port + ",'recordondetect',this.checked)\" " + strChecked + "/></td></tr>";

                        strChecked = "";
                        if (vw.Micobject.detector.recordonalert) strChecked = "checked=\"checked\"";

                        html += "<tr><td>" + LocRm.GetString("RecordOnAlert") +
                                "</td><td><input type=\"radio\" name=\"record_opts\" onclick=\"send_changesetting(" + otid + "," +
                                oid + "," + port + ",'recordonalert',this.checked)\" " + strChecked + "/></td></tr>";


                        html += "</table>";
                        resp += html.Replace("\"", "\\\"");
                    }
                    else
                    {
                        CameraWindow cw = MainForm.InstanceReference.GetCameraWindow(oid);
                        string html = "<table cellspacing=\"3px\">";
                        string strChecked = "";
                        if (cw.Camobject.alerts.active) strChecked = "checked=\"checked\"";
                        html += "<tr><td colspan=\"2\"><strong>" + LocRm.GetString("Alerts") + "</strong></td></tr>";
                        html += "<tr><td>" + LocRm.GetString("AlertsEnabled") +
                                "</td><td><input type=\"checkbox\" onclick=\"send_changesetting(" + otid + "," +
                                oid + "," + port + ",'alerts',this.checked)\" " + strChecked + "/></td></tr>";

                        strChecked = "";
                        if (cw.Camobject.settings.notifyondisconnect) strChecked = "checked=\"checked\"";

                        html += "<tr><td>" + LocRm.GetString("SendEmailOnDisconnect") + "</td><td><input type=\"checkbox\"" +
                                disabled + " onclick=\"send_changesetting(" + otid + "," + oid + "," + port +
                                ",'notifyondisconnect',this.checked)\" " + strChecked + "/></td></tr>";

                        html += "<tr><td>" + LocRm.GetString("DistinctAlertInterval") +
                                "</td><td><input style=\"width:50px\" type=\"text\" value=\"" +
                                cw.Camobject.alerts.minimuminterval + "\" onblur=\"send_changesetting(" + otid + "," +
                                oid + "," + port + ",'minimuminterval',this.value)\"/> " + LocRm.GetString("Seconds") +
                                "</td></tr>";

                        html += "<tr><td colspan=\"2\"><strong>" + LocRm.GetString("AccessGroups") + "</strong></td></tr>";

                        html += "<tr><td>" + LocRm.GetString("AccessGroups") +
                                "</td><td><input style=\"width:100px\" type=\"text\" value=\"" +
                                cw.Camobject.settings.accessgroups + "\" onblur=\"send_changesetting(" + otid + "," +
                                oid + "," + port + ",'accessgroups',this.value)\"/></td></tr>";


                        html += "<tr><td colspan=\"2\"><strong>" + LocRm.GetString("Scheduler") + "</strong></td></tr>";
                        strChecked = "";
                        if (cw.Camobject.schedule.active) strChecked = "checked=\"checked\"";

                        html += "<tr><td valign=\"top\">" + LocRm.GetString("ScheduleActive") +
                                "</td><td><input type=\"checkbox\" onclick=\"send_changesetting(" + otid + "," +
                                oid + "," + port + ",'scheduler',this.checked)\" " + strChecked + "/>";

                        string schedule = cw.ScheduleDetails.Where(s => s != "").Aggregate("", (current, s) => current + (s + "<br/>"));
                        if (schedule != "")
                            html +=
                                "<div class=\"settings_scheduler\">" +
                                schedule + "</div>";
                        html += "</td></tr>";

                        html += "<tr><td colspan=\"2\"><strong>" + LocRm.GetString("RecordingSettings") + "</strong></td></tr>";

                        strChecked = "";

                        if (!cw.Camobject.detector.recordondetect)
                            strChecked = "checked=\"checked\"";

                        html += "<tr><td>" + LocRm.GetString("NoRecord") +
                                "</td><td><input type=\"radio\" name=\"record_opts\" onclick=\"send_changesetting(" + otid + "," +
                                oid + "," + port + ",'recordoff',this.checked)\" " + strChecked + "/></td></tr>";

                        strChecked = "";

                        if (cw.Camobject.detector.recordondetect) strChecked = "checked=\"checked\"";

                        html += "<tr><td>" + LocRm.GetString("RecordOnDetect") +
                                "</td><td><input type=\"radio\" name=\"record_opts\" onclick=\"send_changesetting(" + otid + "," +
                                oid + "," + port + ",'recordondetect',this.checked)\" " + strChecked + "/></td></tr>";

                        strChecked = "";

                        if (cw.Camobject.detector.recordonalert) strChecked = "checked=\"checked\"";

                        html += "<tr><td>" + LocRm.GetString("RecordOnAlert") +
                                "</td><td><input type=\"radio\" name=\"record_opts\" onclick=\"send_changesetting(" + otid + "," +
                                oid + "," + port + ",'recordonalert',this.checked)\" " + strChecked + "/></td></tr>";

                        strChecked = "";
                        if (cw.Camobject.recorder.ftpenabled) strChecked = "checked=\"checked\"";
                        html += "<tr><td>" + LocRm.GetString("RecordingFTP");
                        html += "</td><td><input type=\"checkbox\" onclick=\"send_changesetting(" + otid + "," +
                                oid + "," + port + ",'ftprecordings',this.checked)\" " + strChecked + "/></td></tr>";

                        html += "<tr><td colspan=\"2\"><strong>" + LocRm.GetString("TimelapseRecording") +
                                "</strong></td></tr>";

                        strChecked = "";
                        if (cw.Camobject.recorder.timelapseenabled) strChecked = "checked=\"checked\"";
                        html += "<tr><td>" + LocRm.GetString("TimelapseRecording") +
                                "</td><td><input type=\"checkbox\" onclick=\"send_changesetting(" + otid + "," +
                                oid + "," + port + ",'timelapseon',this.checked)\" " + strChecked + "/></td></tr>";
                        html += "<tr><td>" + LocRm.GetString("Movie") +
                                "</td><td><input style=\"width:50px\" type=\"text\" value=\"" +
                                cw.Camobject.recorder.timelapse + "\" onblur=\"send_changesetting(" + otid + "," +
                                oid + "," + port + ",'timelapse',this.value)\"/> " +
                                LocRm.GetString("savesAFrameToAMovieFileNS") + "</td></tr>";
                        html += "<tr><td>" + LocRm.GetString("Images") +
                                "</td><td><input style=\"width:50px\" type=\"text\" value=\"" +
                                cw.Camobject.recorder.timelapseframes + "\" onblur=\"send_changesetting(" + otid + "," +
                                oid + "," + port + ",'timelapseframes',this.value)\"/> " +
                                LocRm.GetString("savesAFrameEveryNSecondsn") + "</td></tr>";

                        html += "<tr><td colspan=\"2\"><strong>" + LocRm.GetString("FrameLimits") +
                                "</strong></td></tr>";

                        html += "<tr><td>" + LocRm.GetString("Viewing") +
                                "</td><td><input style=\"width:50px\" type=\"text\" value=\"" +
                                cw.Camobject.settings.maxframerate + "\" onblur=\"send_changesetting(" + otid + "," +
                                oid + "," + port + ",'maxframerate',this.value)\"/></td></tr>";
                        html += "<tr><td>" + LocRm.GetString("Recording") +
                                "</td><td><input style=\"width:50px\" type=\"text\" value=\"" +
                                cw.Camobject.settings.maxframeraterecord + "\" onblur=\"send_changesetting(" + otid + "," +
                                oid + "," + port + ",'maxframeraterecord',this.value)\"/></td></tr>";



                        html += "<tr><td colspan=\"2\"><strong>" + LocRm.GetString("SaveFramesFtp") +
                                "</strong></td></tr>";


                        strChecked = "";

                        if (cw.Camobject.ftp.enabled)
                            strChecked = "checked=\"checked\"";

                        html += "<tr><td>" + LocRm.GetString("Enabled") +
                                "</td><td><input type=\"checkbox\" onclick=\"send_changesetting(" + otid + "," +
                                oid + "," + port + ",'ftpenabled',this.checked)\" " + strChecked + "/></td></tr>";

                        strChecked = "";

                        if (cw.Camobject.ftp.savelocal)
                            strChecked = "checked=\"checked\"";

                        html += "<tr><td>" + LocRm.GetString("LocalSavingEnabled") +
                                "</td><td><input type=\"checkbox\" onclick=\"send_changesetting(" + otid + "," +
                                oid + "," + port + ",'localsaving',this.checked)\" " + strChecked + "/></td></tr>";

                        
                        html += "<tr><td colspan=\"2\"><strong>" + LocRm.GetString("Cloud") + "</strong></td></tr>";

                        strChecked = "";

                        if (cw.Camobject.settings.cloudprovider.images)
                            strChecked = "checked=\"checked\"";

                        html += "<tr><td>" + LocRm.GetString("AutomaticallyUploadImages") +
                                "</td><td><input type=\"checkbox\" onclick=\"send_changesetting(" + otid + "," +
                                oid + "," + port + ",'cloudimages',this.checked)\" " + strChecked + "/></td></tr>";

                        strChecked = "";

                        if (cw.Camobject.settings.cloudprovider.recordings)
                            strChecked = "checked=\"checked\"";

                        html += "<tr><td>" + LocRm.GetString("AutomaticallyUploadRecordings") +
                                "</td><td><input type=\"checkbox\" onclick=\"send_changesetting(" + otid + "," +
                                oid + "," + port + ",'cloudrecording',this.checked)\" " + strChecked + "/></td></tr>";
                        


                        html += "</table>";
                        resp += html.Replace("\"", "\\\"");
                    }
                    break;
                case "getcmdlist":
                    var l = "";
                    foreach (objectsCommand ocmd in MainForm.RemoteCommands)
                    {
                        string n = ocmd.name;
                        if (n.StartsWith("cmd_"))
                        {
                            n = LocRm.GetString(ocmd.name);
                        }
                        l += ocmd.id + "|" + n.Replace("|", " ").Replace(",", " ") + ",";
                    }
                    resp = l.Trim(',');
                    break;
                case "previewlist":
                    var top100 =
                        MainForm.MasterFileList.Where(f => f.ObjectTypeId == 2).OrderByDescending(
                            p => p.CreatedDateTicks).Take(MainForm.Conf.PreviewItems).ToList();
                    resp = top100.Aggregate("", (current, file) => current + (file.Filename + "|" + file.Name.Replace("|", "") + "|" + file.Duration + ","));
                    resp = resp.Trim(',');
                    if (resp == "")
                        resp = "OK";
                    break;
                case "getobjectconfig":
                    {
                        string cfg = "";
                        switch (otid)
                        {
                            case 1:
                                VolumeLevel vl = MainForm.InstanceReference.GetVolumeLevel(oid);
                                if(vl != null)
                                {
                                    var ie = vl.IsEnabled;
                                    var fr = vl.ForcedRecording;
                                    cfg = "ot: 1, oid:" + oid + ", port: " + MainForm.Conf.ServerPort + ", online: " +
                                          ie.ToString().ToLower() + ",recording: " +
                                          fr.ToString().ToLower() + ", width:320, height:40";
                                    cfg += ", errorState:" + vl.AudioSourceErrorState.ToString().ToLowerInvariant();
                                }
                                break;
                            case 2:
                                CameraWindow cw = MainForm.InstanceReference.GetCameraWindow(oid);
                                if (cw != null)
                                {
                                    var fr = cw.ForcedRecording;
                                    var en = cw.IsEnabled;
                                    string[] res = cw.Camobject.resolution.Split('x');
                                    string micpairid = "-1";
                                    if (cw.VolumeControl != null)
                                        micpairid = cw.VolumeControl.Micobject.id.ToString(CultureInfo.InvariantCulture);
                                    cfg = "ot: 2, oid:" + oid + ", micpairid: " + micpairid + ", port: " +
                                          MainForm.Conf.ServerPort + ",online: " + en.ToString().ToLower() +
                                          ",recording: " + fr.ToString().ToLower() + ", width:" + res[0] +
                                          ", height:" + res[1] + ", talk:" +
                                          (cw.Camobject.settings.audiomodel != "None").ToString().ToLower();
                                    cfg += ", errorState:" + cw.VideoSourceErrorState.ToString().ToLowerInvariant();
                                }
                                break;
                        }
                        func = func.Replace("cfg", "{" + cfg + "}");
                        resp = "OK";
                    }
                    break;
                case "togglealertmode":
                    {
                        switch (otid)
                        {
                            case 1:
                                VolumeLevel vl = MainForm.InstanceReference.GetVolumeLevel(oid);
                                if (vl != null)
                                {
                                    switch (vl.Micobject.alerts.mode)
                                    {
                                        case "sound":
                                            vl.Micobject.alerts.mode = "nosound";
                                            break;
                                        case "nosound":
                                            vl.Micobject.alerts.mode = "sound";
                                            break;
                                    }
                                }
                                break;
                            case 2:
                                CameraWindow cw = MainForm.InstanceReference.GetCameraWindow(oid);
                                if (cw != null)
                                {
                                    switch (cw.Camobject.alerts.mode)
                                    {
                                        case "movement":
                                            cw.Camobject.alerts.mode = "nomovement";
                                            break;
                                        case "nomovement":
                                            cw.Camobject.alerts.mode = "movement";
                                            break;
                                    }
                                }
                                break;
                        }
                        resp = "OK";
                    }
                    break;
            }
            return resp;
        }

        private static void GetDirectoryPath(String sRequest, String sMyWebServerRoot, out String sLocalDir,
                                             out String sDirName)
        {
            try
            {
                sDirName = sRequest.Substring(sRequest.IndexOf("/", StringComparison.Ordinal));
                sDirName = sDirName.Substring(0, sDirName.LastIndexOf("/", StringComparison.Ordinal));

                if (sDirName == "/")
                    sLocalDir = sMyWebServerRoot;
                else
                {
                    if (sDirName.ToLower().StartsWith(@"/video/"))
                    {
                        
                        string sfile = sRequest.Substring(sRequest.LastIndexOf("/", StringComparison.Ordinal) + 1);
                        int iind = Convert.ToInt32(sfile.Substring(0, sfile.IndexOf("_", StringComparison.Ordinal)));

                        sLocalDir = Helper.GetMediaDirectory(2, iind) + "video\\";
                        sLocalDir += Helper.GetDirectory(2, iind) + "\\";
                        if (sfile.Contains(".jpg"))
                            sLocalDir += "thumbs\\";
                    }
                    else
                    {
                        if (sDirName.ToLower().StartsWith(@"/audio/"))
                        {
                            
                            string sfile = sRequest.Substring(sRequest.LastIndexOf("/", StringComparison.Ordinal) + 1);
                            int iind = Convert.ToInt32(sfile.Substring(0, sfile.IndexOf("_", StringComparison.Ordinal)));
                            sLocalDir = Helper.GetMediaDirectory(1, iind) + "video\\";
                            sLocalDir += Helper.GetDirectory(1, iind) + "\\";
                        }
                        else
                            sLocalDir = sMyWebServerRoot + sDirName.Replace("../", "").Replace("/", @"\");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Failed to get path for request: " + sRequest + " (" + sMyWebServerRoot + ") - " + ex.Message, "Server");
                sLocalDir = "";
                sDirName = "";
            }
        }

        private void ParseMimeType(String sRequestedFile, out string sFileName, out String sMimeType)
        {
            sFileName = sRequestedFile;

            int i = sFileName.IndexOf("?", StringComparison.Ordinal);
            if (i != -1)
                sFileName = sFileName.Substring(0, i);
            i = sFileName.IndexOf("&", StringComparison.Ordinal);
            if (i != -1)
                sFileName = sFileName.Substring(0, i);
            
            sMimeType = GetMimeType(sFileName);
        }

        private static bool CheckAuth(string sPhysicalFilePath)
        {
            return GetVar(sPhysicalFilePath, "auth") == MainForm.Identifier;
        }

        private void SendLogFile(string sHttpVersion, HttpRequest req)
        {
            var fi = new FileInfo(Program.AppDataPath + "log_" + Logger.NextLog + ".htm");
            int iTotBytes = Convert.ToInt32(fi.Length);
            byte[] bytes;
            var fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
            using (var reader = new BinaryReader(fs))
            {
                bytes = new byte[iTotBytes];
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                bytes = reader.ReadBytes(bytes.Length);
            }

            SendResponse(sHttpVersion, "text/html", bytes, " 200 OK", 20, req);
        }

        private void SendLogFile(string sPhysicalFilePath, string sHttpVersion, HttpRequest req)
        {
            string fn = GetVar(sPhysicalFilePath, "fn");
            //prevent filesystem access
            if (fn.IndexOf("./", StringComparison.Ordinal) != -1)
                return;

            var fi = new FileInfo(Program.AppDataPath + fn);
            int iTotBytes = Convert.ToInt32(fi.Length);
            byte[] bytes;
            var fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
            

            using (var reader = new BinaryReader(fs))
            {
                bytes = new byte[iTotBytes];
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                bytes = reader.ReadBytes(bytes.Length);
            }

            SendResponse(sHttpVersion, "text/html", bytes, " 200 OK", 20, req);
        }
        
        private static byte[] _cameraRemoved;
        private static byte[] CameraRemoved
        {
            get
            {
                if (_cameraRemoved == null)
                {
                    using (var ms = new MemoryStream())
                    {
                        Resources.cam_removed.Save(ms, ImageFormat.Jpeg);
                        _cameraRemoved = new Byte[ms.Length];
                        ms.Position = 0;
                        // load the byte array with the image
                        ms.Read(_cameraRemoved, 0, (int)ms.Length);
                    }
                }
                return _cameraRemoved;
            }   
        }

            
        private static byte[] _cameraConnecting;
        private static byte[] CameraConnecting
        {
            get
            {
                if (_cameraConnecting == null)
                {
                    using (var ms = new MemoryStream())
                    {
                        Resources.cam_connecting.Save(ms, ImageFormat.Jpeg);
                        _cameraConnecting = new Byte[ms.Length];
                        ms.Position = 0;
                        // load the byte array with the image
                        ms.Read(_cameraConnecting, 0, (int) ms.Length);
                    }
                }
                return _cameraConnecting;
            }   
        }

        private void SendDesktop(String sPhysicalFilePath, string sHttpVersion, HttpRequest req)
        {
            int oid = Convert.ToInt32(GetVar(sPhysicalFilePath, "oid"));
            string size = GetVar(sPhysicalFilePath, "size");
            int w, h;
            var oc = MainForm.Cameras.FirstOrDefault(p=>p.id==oid);
            GetWidthHeight(size, out w, out h);
            if (oc != null)
            {
                int si;
                int.TryParse(oc.settings.videosourcestring, out si);
                if (Screen.AllScreens.Length <= si)
                    si = 0;

                Screen s = Screen.AllScreens[si];
                using (var imageStream = new MemoryStream())
                {
                    using (var target = new Bitmap(w, h, PixelFormat.Format24bppRgb))
                    {
                        using (var scr = new Bitmap(s.WorkingArea.Width, s.WorkingArea.Height, PixelFormat.Format24bppRgb))
                        {
                            using (Graphics g = Graphics.FromImage(scr))
                            {
                                try
                                {
                                    g.CopyFromScreen(s.Bounds.X, s.Bounds.Y, 0, 0,
                                        new Size(s.WorkingArea.Width, s.WorkingArea.Height));
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception("Error grabbing screen (" + ex.Message +
                                                        ") - disable screensaver.");
                                    //probably remote desktop or screensaver has kicked in

                                }
                            }
                            using (Graphics g = Graphics.FromImage(target))
                            {
                                g.CompositingMode = CompositingMode.SourceCopy;
                                g.CompositingQuality = CompositingQuality.HighSpeed;
                                g.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                                g.SmoothingMode = SmoothingMode.None;
                                g.InterpolationMode = InterpolationMode.Default;
                                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                                try
                                {
                                    g.DrawImage(scr,0,0,w,h);

                                    target.Save(imageStream, ImageFormat.Jpeg);

                                    // make byte array the same size as the image

                                    var imageContent = new Byte[imageStream.Length];
                                    imageStream.Position = 0;
                                    // load the byte array with the image
                                    imageStream.Read(imageContent, 0, (int)imageStream.Length);

                                    // rewind the memory stream

                                    SendResponse(sHttpVersion, "image/jpeg", imageContent, " 200 OK", 0, req);
                                }
                                catch
                                {
                                    //probably remote desktop or screensaver has kicked in

                                }
                            }
                        }
                    }
                }

            }
            
        }

        private void SendLiveFeed(String sPhysicalFilePath, string sHttpVersion, HttpRequest req)
        {
            string cameraId = GetVar(sPhysicalFilePath, "oid");
            string size = GetVar(sPhysicalFilePath, "size");
            bool maintainAR = GetVar(sPhysicalFilePath, "keepAR") == "true";
            bool overlay = GetVar(sPhysicalFilePath, "overlay") != "";
            bool thumb = GetVar(sPhysicalFilePath, "thumb") != "";
            bool full = !thumb && GetVar(sPhysicalFilePath, "full") != "";
            bool disposeFrame = false;
            Bitmap img = null;
            int w = 320, h = 240;
            try
            {
                CameraWindow cw = MainForm.InstanceReference.GetCameraWindow(Convert.ToInt32(cameraId));
                
                using (var imageStream = new MemoryStream())
                {
                    if (cw == null)
                    {
                        img = Resources.cam_removed;
                    }
                    else
                    {
                        if (cw.IsEnabled)
                        {
                            img = cw.LastFrame;
                            if (img == null || cw.VideoSourceErrorState)
                            {
                                img = thumb ? Resources.cam_connecting : Resources.cam_connecting_large;
                            }
                            else
                                disposeFrame = true;
                        }
                        else
                        {
                            img = thumb ? Resources.cam_offline : Resources.cam_offline_large;
                        }    
                    }
                    
                    if (thumb)
                    {
                        w = 96;
                        h = 72;
                    }
                    else
                    {
                        if (full)
                        {
                            w = img.Width;
                            h = img.Height;
                        }
                        else
                        {
                            if (size!="")
                                GetWidthHeight(size, out w, out h);
                        }
                    }

                    var bmpFinal = new Bitmap(w, h);
                    using (Graphics g = Graphics.FromImage(bmpFinal))
                    {
                        g.CompositingMode = CompositingMode.SourceCopy;
                        g.CompositingQuality = CompositingQuality.HighSpeed;
                        g.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                        g.SmoothingMode = SmoothingMode.None;
                        g.InterpolationMode = InterpolationMode.Default;
                        g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                        g.Clear(Color.White);

                        
                        if (maintainAR)
                        {
                            double ar = Convert.ToDouble(img.Height)/Convert.ToDouble(img.Width);
                            int neww = w;
                            int newh = Convert.ToInt32(w*ar);
                            if (newh > h)
                            {
                                newh = h;
                                neww = Convert.ToInt32(h/ar);
                            }
                            //offset for centering
                            try
                            {
                                g.DrawImage(img, (w - neww)/2, (h - newh)/2, neww, newh);
                            }
                            catch
                            {
                                //cam offline?
                            }
                        }
                        else
                        {
                            g.DrawImage(img, 0, 0, w, h);
                        }
                        
                        
                        g.CompositingMode = CompositingMode.SourceOver;
                        if (overlay && cw!=null)
                        {
                            g.FillRectangle(MainForm.OverlayBackgroundBrush, 0, 0 + h - 20, w, 20);
                            g.DrawString(cw.Camobject.name, MainForm.Drawfont, Brushes.White, 2, h - 17);
                        }

                    }
                    bmpFinal.Save(imageStream, ImageFormat.Jpeg);
                            
                    // make byte array the same size as the image

                    var imageContent = new Byte[imageStream.Length];
                    imageStream.Position = 0;
                    // load the byte array with the image
                    imageStream.Read(imageContent, 0, (int) imageStream.Length);


                    SendResponse(sHttpVersion, "image/jpeg", imageContent, " 200 OK", 0, req);

                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Server");
            }

            if (disposeFrame)
                img?.Dispose();
        }

        

        private void SendImage(String sPhysicalFilePath, string sHttpVersion, HttpRequest req)
        {
            int oid = Convert.ToInt32(GetVar(sPhysicalFilePath, "oid"));
            string fn = GetVar(sPhysicalFilePath, "fn");
            
            try
            {
                CameraWindow cw = MainForm.InstanceReference.GetCameraWindow(Convert.ToInt32(oid));
                if (cw == null)
                {
                    SendResponse(sHttpVersion, "image/jpeg", CameraRemoved, " 200 OK", 0, req);
                }
                else
                {
                    string sFileName = Helper.GetMediaDirectory(2, oid) + "Video/" + cw.Camobject.directory +
                                       "/thumbs/" + fn;

                    if (!File.Exists(sFileName))
                    {
                        sFileName = Program.AppPath + @"WebServerRoot\notfound.jpg";
                    }


                    var fs = new FileStream(sFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    
                    string size = GetVar(sPhysicalFilePath, "size");
                    byte[] bytes;
                    if (size != "")
                    {
                        int w,h;
                            
                        GetWidthHeight(size, out w, out h);
                        Image myThumbnail = Image.FromStream(fs).GetThumbnailImage(w, h, ThumbnailCallback,
                                                                                    IntPtr.Zero);

                        using (var ms = new MemoryStream())
                        {
                            myThumbnail.Save(ms, ImageFormat.Jpeg);
                            myThumbnail.Dispose();

                            bytes = new Byte[ms.Length];
                            ms.Position = 0;
                            // load the byte array with the image
                            ms.Read(bytes, 0, (int) ms.Length);
                        }
                    }
                    else
                    {

                        using (var reader = new BinaryReader(fs))
                        {
                            bytes = new byte[fs.Length];
                            while ((reader.Read(bytes, 0, bytes.Length)) != 0)
                            {
                            }
                        }
                    }
                    SendResponse(sHttpVersion, "image/jpeg", bytes, " 200 OK", 30, req);
                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Server");
            }
        }

        private void GetWidthHeight(string size, out int w, out int h)
        {
            string[] wh = size.Split('x');
            w = 320;
            h = 240;
            if (wh.Length == 2)
            {
                double dw, dh;
                double.TryParse(wh[0], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out dw);
                double.TryParse(wh[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out dh);
                w = Convert.ToInt32(dw);
                h = Convert.ToInt32(dh);
            }
        }

        private void SendGrab(String sPhysicalFilePath, string sHttpVersion, HttpRequest req)
        {
            int oid = Convert.ToInt32(GetVar(sPhysicalFilePath, "oid"));
            string fn = GetVar(sPhysicalFilePath, "fn");
            try
            {
                CameraWindow cw = MainForm.InstanceReference.GetCameraWindow(Convert.ToInt32(oid));
                if (cw == null)
                {
                    SendResponse(sHttpVersion, "image/jpeg", CameraRemoved, " 200 OK", 0, req);
                }
                else
                {
                    string sFileName = Helper.GetMediaDirectory(2, oid) + "Video/" + cw.Camobject.directory +
                                       "/grabs/" + fn;

                    if (!File.Exists(sFileName))
                    {
                        sFileName = Program.AppPath + @"WebServerRoot\notfound.jpg";
                    }
                    var fs = new FileStream(sFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    
                    // Create a reader that can read bytes from the FileStream.
                    string size = GetVar(sPhysicalFilePath, "size");
                    byte[] bytes;
                    if (size != "")
                    {
                        int w, h;
                        GetWidthHeight(size, out w, out h);
                        using (var myThumbnail = Image.FromStream(fs).GetThumbnailImage(w, h, ThumbnailCallback,
                            IntPtr.Zero))
                        {
                            // put the image into the memory stream
                            using (var ms = new MemoryStream())
                            {
                                myThumbnail.Save(ms, ImageFormat.Jpeg);

                                bytes = new Byte[ms.Length];
                                ms.Position = 0;
                                // load the byte array with the image
                                ms.Read(bytes, 0, (int) ms.Length);
                            }
                        }
                    }
                    else
                    {

                        using (var reader = new BinaryReader(fs))
                        {
                            bytes = new byte[fs.Length];
                            while ((reader.Read(bytes, 0, bytes.Length)) != 0)
                            {
                            }
                        }
                    }
                    SendResponse(sHttpVersion, "image/jpeg", bytes, " 200 OK", 30, req);
                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Server");
            }
        }

        private void SendFloorPlanFeed(String sPhysicalFilePath, string sHttpVersion, HttpRequest req)
        {
            string floorplanid = GetVar(sPhysicalFilePath, "floorplanid");
            try
            {
                var fpc = MainForm.InstanceReference.GetFloorPlan(Convert.ToInt32(floorplanid));
                if (fpc == null)
                {
                    SendResponse(sHttpVersion, "image/jpeg", CameraRemoved, " 200 OK", 0, req);
                }
                else
                {
                    if (fpc.ImgPlan == null)
                    {
                        SendResponse(sHttpVersion, "image/jpeg", CameraConnecting, " 200 OK", 0, req);
                    }
                    else
                    {
                        int w = 320, h = 240;
                        bool done = false;
                        using (var ms = new MemoryStream())
                        {
                            if (sPhysicalFilePath.IndexOf("thumb", StringComparison.Ordinal) != -1)
                            {
                                w = 96;
                                h = 72;
                            }
                            else
                            {
                                if (sPhysicalFilePath.IndexOf("full", StringComparison.Ordinal) != -1)
                                {
                                    fpc.ImgView.Save(ms, ImageFormat.Jpeg);
                                    done = true;
                                }
                                else
                                {
                                    string size = GetVar(sPhysicalFilePath, "size");
                                    if (size != "")
                                    {
                                        GetWidthHeight(size, out w, out h);
                                    }
                                }
                            }


                            if (!done)
                            {
                                var img = (Image)fpc.ImgView.Clone();
                                var myThumbnail = img.GetThumbnailImage(w, h, null, IntPtr.Zero);

                                // put the image into the memory stream

                                myThumbnail.Save(ms, ImageFormat.Jpeg);
                                myThumbnail.Dispose();
                                img.Dispose();
                            }


                            // make byte array the same size as the image

                            var imageContent = new Byte[ms.Length];
                            ms.Position = 0;
                            // load the byte array with the image
                            ms.Read(imageContent, 0, (int)ms.Length);

                            // rewind the memory stream

                            SendResponse(sHttpVersion, "image/jpeg", imageContent, " 200 OK", 0, req);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex,"Server");
            }
        }

        private void SendMJPEGFeed(String sPhysicalFilePath, HttpRequest req)
        {
            string scamid = GetVar(sPhysicalFilePath,"oid");
            string size = GetVar(sPhysicalFilePath, "size");
            bool basicCt = GetVar(sPhysicalFilePath, "basicct") != "";
            bool maintainAR = GetVar(sPhysicalFilePath, "keepAR") == "true";
            bool overlay = GetVar(sPhysicalFilePath, "overlay") != "false";
            int w = 320, h = 240;
            
            if (size != "")
            {
                GetWidthHeight(size, out w, out h);           
            }
            if (sPhysicalFilePath.IndexOf("thumb", StringComparison.Ordinal) != -1)
            {
                w = 96;
                h = 72;
            }
            else
            {
                if (sPhysicalFilePath.IndexOf("full", StringComparison.Ordinal) != -1)
                {
                    w = -1;
                    h = -1;
                }
            }

            try
            {
                var feed2 = new Thread(p => MJPEGFeedMulti(scamid, req, w, h, basicCt, maintainAR, overlay)) {IsBackground = true};
                feed2.Start();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Server");
            }
        }

        private void MJPEGFeedMulti(string cameraids, HttpRequest req, int w, int h, bool basicContentType, bool maintainAspectRatio, bool includeOverlay)
        {
            String sResponse = "";
            bool useDefault = cameraids == "-1";
            if (useDefault)
                cameraids = MainForm.Conf.DeviceDriverDefault;

            sResponse += "HTTP/1.1 200 OK\r\n";
            sResponse += "Server: iSpy\r\n";
            sResponse += "Expires: 0\r\n";
            sResponse += "Pragma: no-cache\r\n";
            sResponse += "Cache-Control: no-cache, must-revalidate\r\n";
            sResponse += "Access-Control-Allow-Origin: *\r\n";
            if (!basicContentType)
                sResponse += "Content-Type: multipart/x-mixed-replace; boundary=--myboundary";
            else
                sResponse += "Content-Type: text/html; boundary=--myboundary";
            var overlayBackgroundBrush = new SolidBrush(Color.FromArgb(128, 0, 0, 0));
            var drawfont = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular, GraphicsUnit.Pixel);
            
            
            try
            {
                var cams = GetCameraWindows(cameraids, ref w, ref h);
                if (cams.Count > 0)
                {

                    int cols = Convert.ToInt32(Math.Ceiling(Math.Sqrt(cams.Count)));
                    int rows = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(cams.Count)/cols));

                    int camw = Convert.ToInt32(Convert.ToDouble(w)/Convert.ToDouble(cols));
                    int camh = Convert.ToInt32(Convert.ToDouble(h)/Convert.ToDouble(rows));


                    while (req.TcpClient.Client.Connected)
                    {
                        if (useDefault)
                        {
                            if (cameraids != MainForm.Conf.DeviceDriverDefault &&
                                !string.IsNullOrEmpty(MainForm.Conf.DeviceDriverDefault))
                            {
                                cams = GetCameraWindows(MainForm.Conf.DeviceDriverDefault, ref w, ref h);

                                cols = Convert.ToInt32(Math.Ceiling(Math.Sqrt(cams.Count)));
                                rows = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(cams.Count)/cols));

                                camw = Convert.ToInt32(Convert.ToDouble(w)/Convert.ToDouble(cols));
                                camh = Convert.ToInt32(Convert.ToDouble(h)/Convert.ToDouble(rows));
                                cameraids = MainForm.Conf.DeviceDriverDefault;
                            }
                        }
                        var bmpFinal = new Bitmap(w, h);
                        Graphics g = Graphics.FromImage(bmpFinal);
                        g.CompositingMode = CompositingMode.SourceCopy;
                        g.CompositingQuality = CompositingQuality.HighSpeed;
                        g.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                        g.SmoothingMode = SmoothingMode.None;
                        g.InterpolationMode = InterpolationMode.Default;
                        g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                        g.Clear(Color.White);

                        int j = 0, k = 0;

                        foreach (CameraWindow cw in cams)
                        {
                            int x = j*camw;
                            int y = k*camh;
                            j++;
                            if (j == cols)
                            {
                                j = 0;
                                k++;
                            }

                            bool disposeFrame = false;

                            Bitmap img = Resources.cam_offline_large;
                            if (cw.IsEnabled)
                            {
                                img = cw.LastFrame;
                                if (img == null)
                                    img = Resources.cam_connecting_large;
                                else
                                {
                                    if (cw.VideoSourceErrorState)
                                    {
                                        using (Graphics g2 = Graphics.FromImage(img))
                                        {
                                            var img2 = Resources.connecting;
                                            g2.DrawImage(img2, img.Width - img2.Width - 2, 2, img2.Width, img2.Height);
                                        }
                                    }
                                    disposeFrame = true;
                                }

                            }


                            if (maintainAspectRatio)
                            {
                                double ar = Convert.ToDouble(img.Height)/Convert.ToDouble(img.Width);
                                int neww = camw;
                                int newh = Convert.ToInt32(camw*ar);
                                if (newh > camh)
                                {
                                    newh = camh;
                                    neww = Convert.ToInt32(camh/ar);
                                }
                                //offset for centering
                                try
                                {
                                    g.DrawImage(img, x + (camw - neww)/2, y + (camh - newh)/2, neww, newh);
                                }
                                catch (Exception)
                                {
                                    //cam offline?
                                }

                            }
                            else
                            {
                                try
                                {
                                    g.CompositingMode = CompositingMode.SourceCopy;
                                    g.DrawImage(img, x, y, camw, camh);
                                }
                                catch (Exception)
                                {
                                    //cam offline?
                                }
                            }

                            g.CompositingMode = CompositingMode.SourceOver;
                            if (includeOverlay)
                            {
                                g.FillRectangle(overlayBackgroundBrush, x, y + camh - 20, camw, 20);
                                g.DrawString(cw.Camobject.name, drawfont, Brushes.White, x + 2, y + camh - 17);
                            }

                            if (disposeFrame)
                                img.Dispose();

                        }

                        using (var imageStream = new MemoryStream())
                        {
                            bmpFinal.Save(imageStream, ImageFormat.Jpeg);

                            imageStream.Position = 0;
                            // load the byte array with the image             
                            bmpFinal.Dispose();
                            byte[] imageArray = imageStream.GetBuffer();
                            sResponse +=
                                "\r\n\r\n--myboundary\r\nContent-type: image/jpeg\r\nContent-length: " +
                                imageArray.Length + "\r\n\r\n";

                            byte[] bSendData = Encoding.UTF8.GetBytes(sResponse);

                            SendToBrowser(bSendData, req);
                            sResponse = "";
                            SendToBrowser(imageArray, req);
                        }

                        Thread.Sleep(MainForm.Conf.MJPEGStreamInterval); //throttle it
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Server");
            }
            overlayBackgroundBrush.Dispose();
            drawfont.Dispose();
            DisconnectRequest(req);

        }

        private List<CameraWindow> GetCameraWindows(string cameraids, ref int w, ref int h)
        {
            var cams = new List<CameraWindow>();
            string[] camids = cameraids.Split(',');
            bool nw = w == -1;
            if (nw)
            {
                w = 0;
                h = 0;
            }
            foreach (string c in camids)
            {
                if (!string.IsNullOrEmpty(c))
                {
                    var cw = MainForm.InstanceReference.GetCameraWindow(Convert.ToInt32(c));
                    if (cw != null)
                    {
                        if (nw)
                        {
                            w += cw.Camobject.width;
                            h += cw.Camobject.height;
                        }
                        cams.Add(cw);
                    }
                }
            }
            if (cams.Count == 0)
            {
                Logger.LogError("Camera list invalid","MJPEG multi feed");
            }
            return cams;
        }

        public void DisconnectRequest(HttpRequest req)
        {
            lock (_connectedSocketsSyncHandle)
            {
                _connectedSockets.Remove(req);
            }
            req.Destroy();
        }

        private void SendAudioFeed(Enums.AudioStreamMode streamMode, String sBuffer, String sPhysicalFilePath, HttpRequest req)
        {
            string micId = GetVar(sPhysicalFilePath, "micid");
            try
            {
                VolumeLevel vl = MainForm.InstanceReference.GetVolumeLevel(Convert.ToInt32(micId));
                if (vl!=null && vl.IsEnabled)
                {
                    string sResponse = "";

                    sResponse += "HTTP/1.1 200 OK\r\n";
                    sResponse += "Server: iSpy\r\n";
                    sResponse += "Access-Control-Allow-Origin: *\r\n";
                    bool sendend = false;

                    int iStartBytes = 0;
                    if (sBuffer.IndexOf("Range: bytes=", StringComparison.Ordinal) != -1)
                    {
                        var headers = sBuffer.Split(Environment.NewLine.ToCharArray());
                        foreach (string h in headers)
                        {
                            if (!h.StartsWith("Range:")) continue;
                            string[] range = (h.Substring(h.IndexOf("=", StringComparison.Ordinal) + 1)).Split('-');
                            iStartBytes = Convert.ToInt32(range[0]);
                            break;
                        }
                    }
                    if (iStartBytes != 0)
                    {
                        sendend = true;
                    }

                    switch (streamMode)
                    {
                        case Enums.AudioStreamMode.PCM:
                            sResponse += "Content-Type: audio/x-wav\r\n";
                            sResponse += "Transfer-Encoding: chunked\r\n";
                            sResponse += "Connection: close\r\n";
                            sResponse += "\r\n";
                            break;
                        case Enums.AudioStreamMode.MP3:
                            sResponse += "Content-Type: audio/mpeg\r\n";
                            sResponse += "Transfer-Encoding: chunked\r\n";
                            sResponse += "Connection: close\r\n";
                            sResponse += "\r\n";
                            break;
                        //case Enums.AudioStreamMode.M4A:
                        //    sResponse += "Content-Type: audio/aac\r\n";
                        //    sResponse += "Transfer-Encoding: chunked\r\n";
                        //    sResponse += "Connection: close\r\n";
                        //    sResponse += "\r\n";
                        //    break;
                    }


                    byte[] bSendData = Encoding.UTF8.GetBytes(sResponse);

                    SendToBrowser(bSendData, req);

                    if (sendend)
                    {
                        SendToBrowser(Encoding.UTF8.GetBytes(0.ToString("X") + "\r\n"), req);
                    }
                    else
                    {
                        //MySockets.Remove(mySocket);
                        vl.OutSockets.Add(req);
                    }
                }
                else
                {
                    DisconnectRequest(req);
                    NumErr = 0;
                
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Server");
            }
        }
        
        public string FormatBytes(long bytes)
        {
            const int scale = 1024;
            var orders = new[] {"GB", "MB", "KB", "Bytes"};
            var max = (long) Math.Pow(scale, orders.Length - 1);

            foreach (string order in orders)
            {
                if (bytes > max)
                    return String.Format(CultureInfo.InvariantCulture, "{0:##.##} {1}",
                                         decimal.Divide(bytes, max), order);

                max /= scale;
            }
            return "0 Bytes";
        }

        internal string GetObjectList(int ot = 0, int oid =0)
        {
            string resp = "";
            if (MainForm.Cameras != null && (ot==0 || ot==2))
            {
                var l = MainForm.Cameras.OrderBy(p => p.name).ToList();
                foreach (objectsCamera oc in l)
                {
                    if (oid != 0 && oid != oc.id)
                        continue;
                    CameraWindow cw = MainForm.InstanceReference.GetCameraWindow(oc.id);
                    if (cw != null)
                    {
                        bool onlinestatus = cw.IsEnabled;
                        bool talkconfigured = oc.settings.audiomodel != "None";
                        resp += "2," + oc.id + "," + onlinestatus.ToString().ToLower() + "," +
                                oc.name.Replace(",", "&comma;") + "," + GetStatus(onlinestatus) + "," +
                                oc.description.Replace(",", "&comma;").Replace("\n", " ") + "," +
                                oc.settings.accessgroups.Replace(",", "&comma;").Replace("\n", " ") + "," + oc.ptz + "," + talkconfigured.ToString().ToLower() +"," + oc.settings.micpair + ","+cw.Recording.ToString().ToLowerInvariant() + Environment.NewLine;
                    }
                }
            }
            if (MainForm.Microphones != null && (ot == 0 || ot == 1))
            {
                var l = MainForm.Microphones.OrderBy(p => p.name).ToList();
                foreach (objectsMicrophone om in l)
                {
                    if (oid != 0 && oid != om.id)
                        continue;
                    VolumeLevel vl = MainForm.InstanceReference.GetVolumeLevel(om.id);
                    if (vl!=null)
                    {
                        bool onlinestatus = vl.IsEnabled;
                        bool recording = vl.Recording;
                        resp += "1," + om.id + "," + onlinestatus.ToString().ToLower() + "," +
                            om.name.Replace(",", "&comma;") + "," + GetStatus(onlinestatus) + "," +
                            om.description.Replace(",", "&comma;").Replace("\n", " ") + "," +
                            om.settings.accessgroups.Replace(",", "&comma;").Replace("\n", " ") + ","+recording.ToString().ToLowerInvariant()+ Environment.NewLine;
                    }
                }
            }

            resp += "OK";
            return resp;
        }

        internal static string GetStatus(bool active)
        {
            return active ? "Online" : "Offline";
        }

        private void AudioIn(HttpRequest req, int cameraId)
        {
            CameraWindow cw = MainForm.InstanceReference.GetCameraWindow(cameraId);
            if (cw == null)
                return;
            var wf = new WaveFormat(22050, 16, 1);
            var ds = new AudioInStream {RecordingFormat = wf};
            var talkTarget = TalkHelper.GetTalkTarget(cw.Camobject, ds); 
            
            ds.Start();
            talkTarget.Start();
            ds.PacketSize = 4410;
            var bBuffer = new byte[ds.PacketSize*4];
            try
            {
                int j = 0;
                bool pktComplete = false;
                DateTime dt = Helper.Now;
                while (req.TcpClient.Client.Connected) // && talkTarget.Connected)
                {
                    while (!pktComplete && req.TcpClient.Client.Connected)
                    {
                        int i = req.TcpClient.Client.Receive(bBuffer, j, ds.PacketSize, SocketFlags.None);
                        if (i == 0)
                            goto Finish;
                        j += i;
                        while (j >= ds.PacketSize)
                        {
                            var data = new byte[ds.PacketSize];
                            Buffer.BlockCopy(bBuffer, 0, data, 0, ds.PacketSize);
                            ds.AddSamples(data);
                            int ms = Convert.ToInt32((Helper.Now - dt).TotalMilliseconds);
                            if (ms < 40)
                                Thread.Sleep(40 - ms);
                            dt = Helper.Now;
                            pktComplete = true;
                            Buffer.BlockCopy(bBuffer, ds.PacketSize, bBuffer, 0, j - ds.PacketSize);
                            j = j - ds.PacketSize;

                        }
                    }
                    pktComplete = false;
                }
            }
            catch
            {
                // ignored
            }
            Finish:
                DisconnectRequest(req);
                ds.Stop();
                talkTarget.Stop();
            
        }



        private bool _disposed;
        // Public implementation of Dispose pattern callable by consumers. 
        public void Dispose()
        {
            Dispose(true);
        }

        // Protected implementation of Dispose pattern. 
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                WebSocketServer?.Close();
            }

            // Free any unmanaged objects here. 
            //
            _disposed = true;
        }

    }

   
}