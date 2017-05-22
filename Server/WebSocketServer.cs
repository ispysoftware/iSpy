using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using iSpyApplication.Utilities;
using Timer = System.Timers.Timer;

namespace iSpyApplication.Server
{
    public enum ServerLogLevel { Nothing, Subtle, Verbose };
    public delegate void ClientConnectedEventHandler(object sender, EventArgs e);

    public class WebSocketServer: IDisposable
    {
        const string WsKey = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";

        public event ClientConnectedEventHandler ClientConnected;

        /// <summary>
        /// How much information do you want, the server to post to the stream
        /// </summary>
        public ServerLogLevel LogLevel = ServerLogLevel.Subtle;

        /// <summary>
        /// Gets the connections of the server
        /// </summary>
        public List<WebSocketConnection> Connections { get; }

        private readonly Timer _tmrBroadcast;

        private readonly LocalServer _localServer;

        public WebSocketServer(LocalServer server)
        {
            _localServer = server;
            Connections = new List<WebSocketConnection>();
            _tmrBroadcast = new Timer(1000);
            _tmrBroadcast.Elapsed += TmrBroadcastElapsed;
            _tmrBroadcast.Start();
        }

        void TmrBroadcastElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _tmrBroadcast.Stop();
            try
            {
                var l = _broadCastEvents.ToList();
                foreach (var d in l)
                {
                    Connections.ForEach(a => a.Send(d));
                }
                _broadCastEvents.Clear();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex,"Broadcast Websockets");
            }
            _tmrBroadcast.Start();
        }

        public void ConnectSocket(string headers, HttpRequest req)
        {
            ShakeHands(headers.Split(Environment.NewLine.ToCharArray()),req.TcpClient.Client);
            var clientConnection = new WebSocketConnection(req);
            Connections.Add(clientConnection);
            clientConnection.Disconnected += ClientDisconnected;
            ClientConnected?.Invoke(clientConnection, EventArgs.Empty);

            clientConnection.DataReceived += DataReceivedFromClient;
        }

        public void Close()
        {
            _tmrBroadcast.Elapsed -= TmrBroadcastElapsed;
            if (Connections != null)
            {
                var cl = Connections.ToList();
                foreach (var conn in cl)
                {
                    try
                    {
                        conn.DataReceived -= DataReceivedFromClient;
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex, "Websocket Server");

                    }
                }
            }   
        }

        void ClientDisconnected(object sender, EventArgs e)
        {
            var sconn = sender as WebSocketConnection;
            var c = Connections;
            if (sconn != null && c != null && c.Count>0)
            {
                try
                {
                    c.Remove(sconn);
                    _localServer.DisconnectRequest(sconn.Request);
                }
                catch
                {
                }
            }
        }

        void DataReceivedFromClient(object sender, DataReceivedEventArgs e)
        {
            var sconn = sender as WebSocketConnection;
            if (sconn != null)
            {
                switch (e.Data)
                {
                    case "communication test":
                        sconn.Send("communication success");
                        break;
                }
            }
        }


        private readonly List<string> _broadCastEvents = new List<string>(); 
        /// <summary>
        /// send a string to all the clients
        /// </summary>
        /// <param name="data">the string to send</param>
        public void SendToAll(string data)
        {
            //delay by a second to prevent flooding
            try
            {
                if (_broadCastEvents.Count < 3 && !_broadCastEvents.Contains(data))
                    _broadCastEvents.Add(data);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex,"Broadcast SendToAll");
            }
        }       

        private static string ComputeWebSocketHandshakeSecurityHash09(string secWebSocketKey)
        {
            string combinekey = secWebSocketKey + WsKey;

            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] sha1Hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(combinekey));
            string secWebSocketAccept = Convert.ToBase64String(sha1Hash);

            return secWebSocketAccept;
        }
        /// <summary>
        /// Takes care of the initial handshaking between the the client and the server
        /// </summary>
        private static void ShakeHands(IEnumerable<string> headers, Socket conn)
        {
            var stream = new NetworkStream(conn);
            using (var writer = new StreamWriter(stream))
            {
                // send handshake to the client
                writer.WriteLine("HTTP/1.1 101 Switching Protocols");
                writer.WriteLine("Upgrade: WebSocket");
                writer.WriteLine("Connection: Upgrade");
                foreach (var s in headers)
                {
                    if (s.StartsWith("Sec-WebSocket-Key"))
                    {
                        var nv = s.Split(':');
                        if (nv.Length > 1)
                        {
                            var key = nv[1].Trim();
                            writer.WriteLine("Sec-WebSocket-Accept: "+ComputeWebSocketHandshakeSecurityHash09(key));
                            break;
                        }

                    }
                }
                writer.WriteLine("");
            }
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
                _tmrBroadcast?.Dispose();
            }

            // Free any unmanaged objects here. 
            //
            _disposed = true;
        }
    }
}
