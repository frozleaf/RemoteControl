using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using RemoteControl.Protocals;

namespace RemoteControl.Server
{
    class ClientConnectedEventArgs : EventArgs
    {
        public ClientConnectedEventArgs(SocketSession client)
        {
            this.Client = client;
        }

        public SocketSession Client { get; private set; }
    }
}
