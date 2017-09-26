using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using RemoteControl.Protocals;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace RemoteControl.Server
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Win32API.keybd_event(0x11, 0, 0, 0);
            //Win32API.keybd_event(18, 0, 0, 0);
            //Win32API.keybd_event(0x2E, 0, 0, 0);
            //Win32API.keybd_event(0x11, 0, 2, 0);
            //Win32API.keybd_event(18, 0, 2, 0);
            //Win32API.keybd_event(0x2E, 0, 2, 0);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }
    }
}
