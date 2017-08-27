using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace RemoteControl.Plugin.Updater
{
    public partial class FrmMain : Form
    {
        public FrmMain(string[] args)
        {
            InitializeComponent();
            if (args.Length == 1)
            {
                string clientFilePath = args[0];
                GenerateUpdater(clientFilePath);
                MessageBox.Show("生成成功！");
                Environment.Exit(0);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Filter = "*.exe|*.exe|*.dat|*.dat|所有文件(*.*)|*.*";
            dialog.FilterIndex = 1;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string clientFilePath = dialog.FileName;
                GenerateUpdater(clientFilePath);
                MessageBox.Show("生成成功！");
            }
        }

        private void GenerateUpdater(string clientFilePath)
        {
            byte[] hostFileData = System.IO.File.ReadAllBytes(Application.ExecutablePath);
            byte[] clientFileData = System.IO.File.ReadAllBytes(clientFilePath);

            string destFilePath = System.IO.Path.GetDirectoryName(clientFilePath) + "\\" +
                System.IO.Path.GetFileNameWithoutExtension(clientFilePath) + ".dll";

            int clientStartPos = hostFileData.Length;
            using (FileStream fs = new FileStream(destFilePath, FileMode.Create, FileAccess.Write))
            {
                fs.Write(hostFileData, 0, hostFileData.Length);
                fs.Write(clientFileData, 0, clientFileData.Length);
                byte[] buffer = BitConverter.GetBytes(clientStartPos);
                fs.Write(buffer, 0, buffer.Length);
            }
        }
    }
}
