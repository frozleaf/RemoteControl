namespace Sunisoft.IrisSkin
{
    using System;
    using System.Runtime.InteropServices;

    internal class xf60be4f8d2d59066
    {
        [DllImport("kernel32.DLL", CharSet=CharSet.Auto)]
        public static extern bool FreeLibrary(int hModule);
        [DllImport("kernel32.DLL", CharSet=CharSet.Auto)]
        public static extern IntPtr GetCurrentThread();
        [DllImport("kernel32.DLL", CharSet=CharSet.Auto)]
        public static extern uint GetCurrentThreadId();
        [DllImport("kernel32.DLL", CharSet=CharSet.Auto)]
        public static extern int GetLastError();
        [DllImport("kernel32.DLL", CharSet=CharSet.Auto)]
        public static extern int LoadLibrary(string lpFileName);
    }
}

