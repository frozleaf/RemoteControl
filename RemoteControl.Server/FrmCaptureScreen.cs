using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RemoteControl.Protocals;

namespace RemoteControl.Server
{
    public partial class FrmCaptureScreen : FrmBase
    {
        private SocketSession oSession;

        public FrmCaptureScreen(SocketSession session)
        {
            InitializeComponent();
            this.oSession = session;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            button.Checked = !button.Checked;
            if (button.Checked)
            {
                RequestStartGetScreen req = new RequestStartGetScreen();
                req.fps = 1;
                oSession.Send(ePacketType.PACKET_START_CAPTURE_SCREEN_REQUEST, req);
            }
            else
            {
                oSession.Send(ePacketType.PACKET_STOP_CAPTURE_SCREEN_REQUEST, null);
            }
        }

        public void HandleScreen(ResponseStartGetScreen resp)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<ResponseStartGetScreen>(HandleScreen), resp);
                return;
            }
            this.pictureBox1.Image = resp.GetImage();
        }

        private void FrmCaptureScreen_Load(object sender, EventArgs e)
        {
            // Panel增加滚动条
            this.panel1.AutoScroll = true;
            // 根据图像大小，自动调节控件和Image的尺寸
            this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            if (this.pictureBox1.Image != null)
            {
                Image img = (Image)this.pictureBox1.Image.Clone();
                string fileName = "";
                SaveFileDialog dialog = new SaveFileDialog();
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    fileName = dialog.FileName;
                    try
                    {
                        img.Save(fileName);
                        img.Dispose();
                        MsgBox.ShowInfo("保存成功!");
                    }
                    catch (Exception ex)
                    {
                        MsgBox.ShowInfo("保存失败，" + ex.Message);
                    }
                }
            }
            else
            {
                MsgBox.ShowInfo("暂无图像，无法保存！");
            }
        }
    }
}
