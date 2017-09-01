using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RemoteControl.Client.CamCapture
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
            bool isHide = true;
            int fps = 1;
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                string argLower = arg.ToLower();
                if (argLower.StartsWith("/s"))
                {
                    isHide = false;
                }
                else if (argLower.StartsWith("/fps:"))
                {
                    fps = Convert.ToInt32(arg.Substring(arg.IndexOf(":")+1));
                }
            }
            Application.Run(new FrmMain(isHide, fps));
        }
    }
}
