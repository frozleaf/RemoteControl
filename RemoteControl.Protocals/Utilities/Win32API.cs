using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace RemoteControl.Protocals
{
    public class Win32API
    {
        /// <summary>
        /// 从exe或dll文件中提取图标
        /// </summary>
        /// <param name="sFileName">exe或dll文件路径</param>
        /// <param name="niconIndex">从第几个图标开始提取,默认-1</param>
        /// <param name="pLargeIcons">返回的32x32图标数组</param>
        /// <param name="pSmallIcons">返回的16x16图标数组</param>
        /// <param name="nIcons">提取图标个数,默认0</param>
        /// <returns>返回提取的图标个数，如果iIconIndex=-1同时pLargeIcons和pSmallIcons都为null，则返回图标总数</returns>
        [System.Runtime.InteropServices.DllImport("shell32.dll")]
        public static extern int ExtractIconEx(string sFileName, int iIconIndex, IntPtr[] pLargeIcons, IntPtr[] pSmallIcons, int nIcons);

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern int BitBlt(
            IntPtr hdcDest,
            int nXDest,int nYDest, int nWidth,int nHeight,
            IntPtr hdcSrc,
            int nXSrc,int nYSrc,UInt32 dwRop);

        [DllImport("winmm.dll", EntryPoint = "mciSendString")]
        public static extern int mciSendString(string lpszCommand, string lpszReturnString, uint cchReturn, IntPtr hwndCallback);

        [DllImport("Shell32.dll", EntryPoint = "SHGetFileInfo", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

        [DllImport("User32.dll", EntryPoint = "DestroyIcon")]
        public static extern int DestroyIcon(IntPtr hIcon);

        public const uint SHGFI_ICON = 0x100;
        public const uint SHGFI_LARGEICON = 0x0; //大图标 32×32
        public const uint SHGFI_SMALLICON = 0x1; //小图标 16×16
        public const uint SHGFI_USEFILEATTRIBUTES = 0x10;

        /// <summary>
        /// 保存文件信息的结构体
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon; // 返回的图标句柄
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        // https://msdn.microsoft.com/en-us/library/ms646260(VS.85).aspx
        public const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        public const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        public const int MOUSEEVENTF_LEFTUP = 0x0004;
        public const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        public const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        public const int MOUSEEVENTF_MOVE = 0x0001;
        public const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        public const int MOUSEEVENTF_RIGHTUP = 0x0010;
        public const int MOUSEEVENTF_WHEEL = 0x0800;
        public const int MOUSEEVENTF_XDOWN = 0x0080;
        public const int MOUSEEVENTF_XUP = 0x0100;
        public const int MOUSEEVENTF_HWHEEL = 0x01000;

        [DllImport("user32.dll")]
        public static extern void mouse_event(
               int  dwFlags,
               int  dx,
               int  dy,
               int  dwData,
               int dwExtraInfo
            );

        [StructLayout(LayoutKind.Sequential)]
        public struct CursorPosition
        {
            [MarshalAs(UnmanagedType.I4)]
            public int x;
            [MarshalAs(UnmanagedType.I4)]
            public int y;
        }

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(ref CursorPosition lpPoint);

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        public static extern Int32 ShowWindow(Int32 hwnd, Int32 nCmdShow);
        public const Int32 SW_SHOW = 0x5;
        public const Int32 SW_HIDE = 0x0;

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern Int32 FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int Width, int Height, int flags);

        public const string Shell_TrayWnd_Name = "Shell_TrayWnd";

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hwnd);

        public enum SoundFlags
        {
            /// <summary>play synchronously (default)</summary>
            SND_SYNC = 0x0000,
            /// <summary>play asynchronously</summary>
            SND_ASYNC = 0x0001,
            /// <summary>silence (!default) if sound not found</summary>
            SND_NODEFAULT = 0x0002,
            /// <summary>pszSound points to a memory file</summary>
            SND_MEMORY = 0x0004,
            /// <summary>loop the sound until next sndPlaySound</summary>
            SND_LOOP = 0x0008,
            /// <summary>don’t stop any currently playing sound</summary>
            SND_NOSTOP = 0x0010,
            /// <summary>Stop Playing Wave</summary>
            SND_PURGE = 0x40,
            /// <summary>don’t wait if the driver is busy</summary>
            SND_NOWAIT = 0x00002000,
            /// <summary>name is a registry alias</summary>
            SND_ALIAS = 0x00010000,
            /// <summary>alias is a predefined id</summary>
            SND_ALIAS_ID = 0x00110000,
            /// <summary>name is file name</summary>
            SND_FILENAME = 0x00020000,
            /// <summary>name is resource name or atom</summary>
            SND_RESOURCE = 0x00040004
        }

        [DllImport("winmm.dll", SetLastError = true)]
        public static extern bool PlaySound(string pszSound, UIntPtr hmod, uint fdwSound);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetShortPathName(string path, StringBuilder shortPath, int shortPathLength); 
    }
}
