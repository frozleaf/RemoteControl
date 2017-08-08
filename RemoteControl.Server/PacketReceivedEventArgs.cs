using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RemoteControl.Protocals;

namespace RemoteControl.Server
{
    class PacketReceivedEventArgs : EventArgs
    {
        public SocketSession Session;
        public ePacketType PacketType;
        public object Obj;
    }
}
