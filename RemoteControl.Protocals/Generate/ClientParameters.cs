using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Runtime.InteropServices;

namespace RemoteControl.Protocals
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ClientParameters
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Header; // 头部标示字节
        public long ServerIP; // 服务器ip地址
        public int ServerPort; // 服务器端口
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=24)]
        public string OnlineAvatar; // 客户端上线图标名
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 24)]
        public string ServiceName; // 客户端启动时的服务名

        public void SetServerIP(string ip)
        {
            this.ServerIP = IPAddress.Parse(ip).Address;
        }

        public string GetServerIP()
        {
            return new IPAddress(this.ServerIP).ToString();
        }

        public IPEndPoint GetIPEndPoint()
        {
            return new IPEndPoint(new IPAddress(this.ServerIP), this.ServerPort);
        }

        public void InitHeader()
        {
            this.Header = new byte[] { 0xff, 0xff, 0xff, 0xff };
        }
    }
}
