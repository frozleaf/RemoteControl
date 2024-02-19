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
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 24)]
        public string ServerIP; // 服务器ip地址或域名
        public int ServerPort; // 服务器端口
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=24)]
        public string OnlineAvatar; // 客户端上线图标名
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 24)]
        public string ServiceName; // 客户端启动时的服务名

        public void SetHostNameOrAddress(string ip)
        {
            this.ServerIP = ip;
        }

        public string GetHostNameOrAddress()
        {
            return this.ServerIP;
        }

        public void InitHeader()
        {
            this.Header = new byte[] { 0xff, 0xff, 0xff, 0xff };
        }
    }
}
