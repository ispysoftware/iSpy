using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace iSpyApplication.Sources.Video.proxies
{
    public class ws
    {
        public ws(string url)
        {
            using (var ws = new WebSocket(url))
            {
                ws.OnMessage += (sender, e) =>
                                    Console.WriteLine("Laputa says: " + e.Data);

                ws.Connect();
                ws.Send("BALUS");
                Console.ReadKey(true);
            }
        }
    }
}
