using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using System.Net;

namespace RemoteControl.Protocals
{
    public class Utils
    {
        /// <summary>
        /// 根据扩展名（或文件名）获取系统中所关联的图标
        /// </summary>
        /// <param name="extension">扩展名（或文件名），例如：.exe</param>
        /// <param name="isLargeIcon">是否返回大图标</param>
        /// <returns></returns>
        public static Icon GetIcon(string extension, bool isLargeIcon)
        {
            Win32API.SHFILEINFO shfi = new Win32API.SHFILEINFO();

            IntPtr pIcon;
            if (isLargeIcon)
            {
                pIcon = Win32API.SHGetFileInfo(extension, 0, ref shfi, (uint)Marshal.SizeOf(shfi),
                    Win32API.SHGFI_ICON | Win32API.SHGFI_USEFILEATTRIBUTES | Win32API.SHGFI_LARGEICON);
            }
            else
            {
                pIcon = Win32API.SHGetFileInfo(extension, 0, ref shfi, (uint)Marshal.SizeOf(shfi),
                    Win32API.SHGFI_ICON | Win32API.SHGFI_USEFILEATTRIBUTES | Win32API.SHGFI_SMALLICON);
            }
            Icon icon = Icon.FromHandle(shfi.hIcon).Clone() as Icon;

            Win32API.DestroyIcon(shfi.hIcon);

            return icon;
        }

        public static List<string> GetIPAddressV4()
        {
            List<string> result = new List<string>();

            IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (var ip in ips)
            {
                if (ip.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
                    continue;
                result.Add(ip.ToString());
            }

            return result;
        }

        public static string GetAppPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}
