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
            if(args.Length == 1&&args[0]=="/s")
            {
                isHide = false;
            }
            Application.Run(new FrmMain(isHide));
        }
    }
}
