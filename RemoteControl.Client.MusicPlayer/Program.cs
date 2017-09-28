using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text;

namespace RemoteControl.Client.MusicPlayer
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length != 1 && !System.IO.File.Exists(args[0]))
            {
                Environment.Exit(0);
                return;
            }
            StringBuilder shortFileName = new StringBuilder(1024);
            GetShortPathName(args[0], shortFileName, shortFileName.Capacity);
            PlayMusic(shortFileName.ToString());
            Application.Run(new FrmMain());
        }

        static void PlayMusic(string path)
        {
            int ret1 = mciSendString("open \"" + path + "\" alias mymusic", "", 0, IntPtr.Zero);
            int ret2 = mciSendString("play mymusic", "", 0, IntPtr.Zero);
        }

        static void StopMusic()
        {
            mciSendString("close mymusic", null, 0, IntPtr.Zero);//关闭
        }

        [DllImport("winmm.dll", EntryPoint = "mciSendString")]
        public static extern int mciSendString(string lpszCommand, string lpszReturnString, uint cchReturn, IntPtr hwndCallback);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetShortPathName(string path, StringBuilder shortPath, int shortPathLength); 
    }
}
