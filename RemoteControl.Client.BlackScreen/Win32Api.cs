using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace RemoteControl.Client.BlackScreen
{
    class Win32Api
    {
        [DllImport("user32.dll")]
        public static extern Int32 ShowWindow(Int32 hwnd, Int32 nCmdShow);
        public const Int32 SW_SHOW = 0x5; 
        public const Int32 SW_HIDE = 0x0;

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern Int32 FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int Width, int Height, int flags);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hwnd);
    }
}
