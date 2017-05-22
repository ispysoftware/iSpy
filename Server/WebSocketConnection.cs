using System;
using System.Net.Sockets;
using System.Text;

namespace iSpyApplication.Server
{
    public class DataReceivedEventArgs: EventArgs
    {
        public string Data { get; private set; }
        public DataReceivedEventArgs(string data)
        {
            Data = data;
        }
    }

    public delegate void DataReceivedEventHandler(object sender, DataReceivedEventArgs e);
    public delegate void WebSocketDisconnectedEventHandler(object sender, EventArgs e);

    public class WebSocketConnection : IDisposable
    {

        #region Private members
        private readonly byte[] _dataBuffer;
        private int _dataBufferOffset;
        private bool _discard;
        #endregion

        /// <summary>
        /// An event that is triggered whenever the connection has read some data from the client
        /// </summary>
        public event DataReceivedEventHandler DataReceived;

        public event WebSocketDisconnectedEventHandler Disconnected;

        /// <summary>
        /// GUID for the connection - thought it might be usable in some way
        /// </summary>
        public Guid GUID { get; private set; }

        /// <summary>
        /// Gets the socket used for the connection
        /// </summary>
        public Socket ConnectionSocket { get; private set; }

        /// <summary>
        /// Gets the original request used for the connection
        /// </summary>
        public HttpRequest Request { get; private set; }

        #region Constructors
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="req">The request on which to establish the connection</param>
        public WebSocketConnection(HttpRequest req)
            : this(req, 1024)
        {

        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="req">The request on which to establish the connection</param>
        /// <param name="bufferSize">The size of the buffer used to receive data</param>
        public WebSocketConnection(HttpRequest req, int bufferSize)
        {
            Request = req;
            ConnectionSocket = req.TcpClient.Client;
            _dataBuffer = new byte[bufferSize];
            GUID = Guid.NewGuid();
            Listen();            
        }
        #endregion

        /// <summary>
        /// Invoke the DataReceived event, called whenever the client has finished sending data.
        /// </summary>
        protected virtual void OnDataReceived(DataReceivedEventArgs e)
        {
            DataReceived?.Invoke(this, e);
        }

        /// <summary>
        /// Listens for incoming data
        /// </summary>
        private void Listen()
        {
            ConnectionSocket.BeginReceive(_dataBuffer, _dataBufferOffset, _dataBuffer.Length - _dataBufferOffset, 0, Read, null);
        }

        /// <summary>
        /// Send a string to the client
        /// </summary>
        /// <param name="str">the string to send to the client</param>
        public void Send(string str)
        {
            if (ConnectionSocket!=null && ConnectionSocket.Connected)
            {
                try
                {
                    ConnectionSocket.Send(EncodeMessageToSend(str));
                }
                catch
                {
                    Disconnected?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private string DecodeMessage(byte[] bytes, out bool complete)
        {
            complete = false;
            byte b = bytes[1];
            int dataLength = 0;
            int totalLength = 0;
            int keyIndex = 0;

            if (b - 128 <= 125)
            {
                dataLength = b - 128;
                keyIndex = 2;
                totalLength = dataLength + 6;
            }

            if (b - 128 == 126)
            {
                dataLength = BitConverter.ToInt16(new [] { bytes[3], bytes[2] }, 0);
                keyIndex = 4;
                totalLength = dataLength + 8;
            }

            if (b - 128 == 127)
            {
                dataLength = (int)BitConverter.ToInt64(new [] { bytes[9], bytes[8], bytes[7], bytes[6], bytes[5], bytes[4], bytes[3], bytes[2] }, 0);
                keyIndex = 10;
                totalLength = dataLength + 14;
            }

            if (totalLength > bytes.Length)
                return "";

            byte[] key = { bytes[keyIndex], bytes[keyIndex + 1], bytes[keyIndex + 2], bytes[keyIndex + 3] };

            int dataIndex = keyIndex + 4;
            int count = 0;
            for (int i = dataIndex; i < totalLength; i++)
            {
                bytes[i] = (byte)(bytes[i] ^ key[count % 4]);
                count++;
            }
            complete = true;
            return Encoding.UTF8.GetString(bytes, dataIndex, dataLength);
        }

        private static Byte[] EncodeMessageToSend(String message)
        {
            Byte[] bytesRaw = Encoding.UTF8.GetBytes(message);
            var frame = new Byte[10];

            Int32 indexStartRawData;
            Int32 length = bytesRaw.Length;

            frame[0] = 129;
            if (length <= 125)
            {
                frame[1] = (Byte)length;
                indexStartRawData = 2;
            }
            else if (length >= 126 && length <= 65535)
            {
                frame[1] = 126;
                frame[2] = (Byte)((length >> 8) & 255);
                frame[3] = (Byte)(length & 255);
                indexStartRawData = 4;
            }
            else
            {
                frame[1] = 127;
                frame[2] = (Byte)((length >> 56) & 255);
                frame[3] = (Byte)((length >> 48) & 255);
                frame[4] = (Byte)((length >> 40) & 255);
                frame[5] = (Byte)((length >> 32) & 255);
                frame[6] = (Byte)((length >> 24) & 255);
                frame[7] = (Byte)((length >> 16) & 255);
                frame[8] = (Byte)((length >> 8) & 255);
                frame[9] = (Byte)(length & 255);

                indexStartRawData = 10;
            }

            var response = new Byte[indexStartRawData + length];

            Int32 i, reponseIdx = 0;

            //Add the frame bytes to the response
            for (i = 0; i < indexStartRawData; i++)
            {
                response[reponseIdx] = frame[i];
                reponseIdx++;
            }

            //Add the data bytes to the response
            for (i = 0; i < length; i++)
            {
                response[reponseIdx] = bytesRaw[i];
                reponseIdx++;
            }

            return response;
        }

        /// <summary>
        /// reads the incoming data and triggers the DataReceived event when done
        /// </summary>
        private void Read(IAsyncResult ar)
        {
            int sizeOfReceivedData = 0;
            try
            {
                var cs = ConnectionSocket;
                if (cs == null)
                    return;
                sizeOfReceivedData = cs.EndReceive(ar);
            }
            catch (SocketException)
            {

            }
            catch (ObjectDisposedException)
            {

            }
            catch (NullReferenceException)
            {

            }

            if (sizeOfReceivedData > 0)
            {
                bool complete;
                var msg = DecodeMessage(_dataBuffer, out complete);

                if (complete)
                {
                    _dataBufferOffset = 0;

                    if (DataReceived != null && !_discard)
                        DataReceived(this, new DataReceivedEventArgs(msg));

                    _discard = false;
                }
                else
                {
                    _dataBufferOffset += sizeOfReceivedData;
                }
                if (_dataBufferOffset >= _dataBuffer.Length)
                {
                    _dataBufferOffset = 0;
                    _discard = true;
                }
                // continue listening for more data
                Listen();
            }
            else // the socket is closed
            {
                Disconnected?.Invoke(this, EventArgs.Empty);
            }
        }

        #region cleanup
        /// <summary>
        /// Closes the socket
        /// </summary>
        public void Close()
        {
            if (ConnectionSocket != null)
            {
                ConnectionSocket.Close();
                ConnectionSocket = null;
            }
            Disconnected?.Invoke(this, EventArgs.Empty);
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
                ConnectionSocket?.Close();
            }

            // Free any unmanaged objects here. 
            //
            _disposed = true;
        }
        #endregion
    }
}
