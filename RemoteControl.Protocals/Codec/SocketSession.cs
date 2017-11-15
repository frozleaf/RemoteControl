using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using RemoteControl.Protocals;
using RemoteControl.Protocals.Codec;

namespace RemoteControl.Protocals
{
    public class SocketSession
    {
        public Socket SocketObj { get; private set; }
        public string SocketId { get; private set; }
        public string HostName { get; private set; }
        public string AppPath { get; private set; }
        public string OnlineAvatar { get; private set; }

        public SocketSession(string sId, Socket oSocket)
        {
            this.SocketId = sId;
            this.SocketObj = oSocket;
        }

        public SocketSession(EndPoint sId, Socket oSocket) : this((sId as IPEndPoint).ToString(), oSocket)
        {
            
        }

        /// <summary>
        /// 关闭会话
        /// </summary>
        public void Close()
        {
            try
            {
                if (SocketObj != null)
                {
                    SocketObj.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 设置主机名
        /// </summary>
        /// <param name="hostName"></param>
        public void SetHostName(string hostName)
        {
            this.HostName = hostName;
        }

        public void SetAppPath(string appPath)
        {
            this.AppPath = appPath;
        }

        public void SetOnlineAvatar(string avatar)
        {
            this.OnlineAvatar = avatar;
        }

        public string GetSocketIPById()
        {
            if(SocketId==null)
                return string.Empty;
            string[] array = SocketId.Split(':');
            if (array.Length != 2)
                return string.Empty;
            return array[0];
        }

        public void Send(ePacketType packetType, object obj)
        {
            try
            {
                this.SocketObj.Send(CodecFactory.Instance.EncodeOject(packetType, obj));
            }
            catch (Exception ex)
            {
                Console.WriteLine("SocketSession Error:" + ex.Message);
            }
        }
    }
}
