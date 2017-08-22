using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using RemoteControl.Protocals;

namespace RemoteControl.Protocals
{
    public class SocketSession
    {
        public Socket SocketObj { get; private set; }
        public string SocketId { get; private set; }

        public SocketSession(string sId, Socket oSocket)
        {
            this.SocketId = sId;
            this.SocketObj = oSocket;
        }

        public SocketSession(EndPoint sId, Socket oSocket) : this((sId as IPEndPoint).ToString(), oSocket)
        {
            
        }

        public void Send(ePacketType packetType, object obj)
        {
            try
            {
                this.SocketObj.Send(PacketFactory.EncodeOject(packetType, obj));
            }
            catch (Exception ex)
            {
                Console.WriteLine("SocketSession Error:" + ex.Message);
            }
        }
    }
}
