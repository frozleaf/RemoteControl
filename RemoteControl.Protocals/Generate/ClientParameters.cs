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
        public byte[] Header;
        public long ServerIP;
        public int ServerPort;

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
