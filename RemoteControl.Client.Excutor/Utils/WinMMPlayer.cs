using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace RemoteControl.Client.Excutor.Utils
{
    class WinMMPlayer
    {
        [DllImport("winmm.dll", EntryPoint = "mciSendString")]
        private static extern int mciSendString(string lpszCommand, string lpszReturnString, uint cchReturn, IntPtr hwndCallback);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern int GetShortPathName(string path, StringBuilder shortPath, int shortPathLength);

        public void PlayMusic(string path)
        {
            path = ToShortFileName(path);
            int ret1 = mciSendString("open \"" + path + "\" alias mymusic", "", 0, IntPtr.Zero);
            int ret2 = mciSendString("play mymusic", "", 0, IntPtr.Zero);
        }

        public void StopMusic()
        {
            mciSendString("close mymusic", null, 0, IntPtr.Zero);//关闭
        }

        private string ToShortFileName(string filePath)
        {
            StringBuilder shortFileName = new StringBuilder(1024);
            GetShortPathName(filePath, shortFileName, shortFileName.Capacity);

            return shortFileName.ToString();
        }
    }
}
