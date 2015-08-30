using System.IO;
using System.Net;
using System.Net.Sockets;

namespace iSpyApplication
{
    public class HttpRequest
    {
        public TcpClient TcpClient;
        public IPEndPoint EndPoint;
        public Stream Stream; //SSLStream or NetworkStream depending on client
        public RestartableReadStream RestartableStream;
        public byte[] Buffer;
        public string Ascii="";

        public void Destroy()
        {
            try
            {
                TcpClient?.Client?.Close();
                TcpClient = null;

                RestartableStream?.Close();
                RestartableStream = null;

                Stream?.Close();
                Stream = null;
                
                Buffer = null;
            }
            catch
            {
                // ignored
            }
        }
    }
}
