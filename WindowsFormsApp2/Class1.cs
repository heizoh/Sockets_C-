using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    public class TCPServer
    {
        private ManualResetEvent AllDone = new ManualResetEvent(false);

        public IPEndPoint IPEndpoint { get; }


    }
}
