using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RemoteControl.Protocals;
using log4net;
using System.IO;
using System.Drawing.Imaging;
using RemoteControl.Server.Utils;

namespace RemoteControl.Server
{
    public partial class FrmCaptureScreen : FrmBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(FrmCaptureScreen));
        private SocketSession oSession;
        private bool _isCaptureMouse = false;
        private bool _isCaptureKeyboard = false;

        public FrmCaptureScreen(SocketSession session)
        {
            InitializeComponent();
            this.oSession = session;
            this.FormClosed += new FormClosedEventHandler(FrmCaptureScreen_FormClosed);
        }

        void FrmCaptureScreen_FormClosed(object sender, FormClosedEventArgs e)
        {
            oSession.Send(ePacketType.PACKET_STOP_CAPTURE_SCREEN_REQUEST, null);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ToolStripButton button = sender as ToolStripButton;
            button.Checked = !button.Checked;
            if (button.Checked)
            {
                RequestStartGetScreen req = new RequestStartGetScreen();
                req.fps = 5;
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
            try
            {
                this.pictureBox1.Image = resp.GetImage();
            }
            catch (Exception ex)
            {
                Logger.Error("HandleScreen", ex);
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
            if (this.pictureBox1.Image != null)
            {
                string fileName = "";
                // 直接从picturebox中调用save()的话，容易出现“GDI+ 发生一般性错误”。
                // 此处用bitmap对象中专一次
                using (Bitmap bmp = new Bitmap(this.pictureBox1.Image))
                {
                    SaveFileDialog dialog = new SaveFileDialog();
                    dialog.Filter = "*.bmp|*.bmp|*.jpg;*.jpeg|*.jpg;*.jpeg|*.*|*.*";
                    dialog.FilterIndex = 1;
                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        fileName = dialog.FileName;
                        try
                        {
                            using (var ms = new MemoryStream())
                            {
                                bmp.Save(ms, ImageFormat.Jpeg);
                                System.IO.File.WriteAllBytes(fileName, ms.ToArray());
                            }
                            MsgBox.Info("保存成功!");
                        }
                        catch (Exception ex)
                        {
                            MsgBox.Info("保存失败，" + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MsgBox.Info("暂无图像，无法保存！");
            }
        }

        #region 鼠标操作事件
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if(_isCaptureMouse)
            {
                RequestMouseEvent req = new RequestMouseEvent();
                req.MouseButton = (eMouseButtons)e.Button;
                req.MouseOperation = eMouseOperations.MouseDown;
                req.MouseLocation = e.Location;
                this.oSession.Send(ePacketType.PACKET_MOUSE_EVENT_REQUEST, req);
                Console.WriteLine(req);
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (_isCaptureMouse)
            {
                RequestMouseEvent req = new RequestMouseEvent();
                req.MouseButton = (eMouseButtons)e.Button;
                req.MouseOperation = eMouseOperations.MouseUp;
                req.MouseLocation = e.Location;
                this.oSession.Send(ePacketType.PACKET_MOUSE_EVENT_REQUEST, req);
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            //if (_isCaptureMouse)
            //{
            //    RequestMouseEvent req = new RequestMouseEvent();
            //    req.MouseButton = (eMouseButtons)e.Button;
            //    req.MouseOperation = eMouseOperations.MouseMove;
            //    req.MouseLocation = e.Location;
            //    this.oSession.Send(ePacketType.PACKET_MOUSE_EVENT_REQUEST, req);
            //}
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (_isCaptureMouse)
            {
                RequestMouseEvent req = new RequestMouseEvent();
                req.MouseButton = (eMouseButtons)e.Button;
                req.MouseOperation = eMouseOperations.MouseDoubleClick;
                req.MouseLocation = e.Location;
                this.oSession.Send(ePacketType.PACKET_MOUSE_EVENT_REQUEST, req);
            }
        } 
        #endregion

        #region 键盘操作事件

        private void FrmCaptureScreen_KeyDown(object sender, KeyEventArgs e)
        {
            if (_isCaptureKeyboard)
            {
                RequestKeyboardEvent req = new RequestKeyboardEvent();
                req.KeyOperation = eKeyboardOpe.KeyDown;
                req.KeyCode = (eKeyboardKeys)e.KeyCode;
                req.KeyValue = e.KeyValue;
                req.KeyData = (eKeyboardKeys)e.KeyData;
                this.oSession.Send(ePacketType.PACKET_KEYBOARD_EVENT_REQUEST, req); 
            }
        }

        private void FrmCaptureScreen_KeyUp(object sender, KeyEventArgs e)
        {
            if (_isCaptureKeyboard)
            {
                RequestKeyboardEvent req = new RequestKeyboardEvent();
                req.KeyOperation = eKeyboardOpe.KeyUp;
                req.KeyCode = (eKeyboardKeys)e.KeyCode;
                req.KeyValue = e.KeyValue;
                req.KeyData = (eKeyboardKeys)e.KeyData;
                this.oSession.Send(ePacketType.PACKET_KEYBOARD_EVENT_REQUEST, req);
            }
        }
        #endregion

        #region 帧率选择
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

                int fps = Convert.ToInt32(item.Tag);
                item.Checked = true;
                RequestStartGetScreen req = new RequestStartGetScreen();
                req.fps = fps;
                oSession.Send(ePacketType.PACKET_START_CAPTURE_SCREEN_REQUEST, req);
            }
        }

        /// <summary>
        /// 帧率选择点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripSplitButton2_ButtonClick(object sender, EventArgs e)
        {
            toolStripSplitButton2.ShowDropDown();
        } 
        #endregion

        #region 捕捉操作选择

        /// <summary>
        /// 捕获鼠标操作按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemCaptureMouse_Click(object sender, EventArgs e)
        {
            toolStripMenuItemCaptureMouse.Checked = !toolStripMenuItemCaptureMouse.Checked;
            _isCaptureMouse = toolStripMenuItemCaptureMouse.Checked;
        }

        /// <summary>
        /// 捕获键盘操作按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemCaptureKeyboard_Click(object sender, EventArgs e)
        {
            toolStripMenuItemCaptureKeyboard.Checked = !toolStripMenuItemCaptureKeyboard.Checked;
            _isCaptureKeyboard = toolStripMenuItemCaptureKeyboard.Checked;
        }

        /// <summary>
        /// 捕获操作点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            toolStripSplitButton1.ShowDropDown();
        } 
        #endregion

        #region 窗体关闭前事件

        /// <summary>
        /// 窗体关闭前事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmCaptureScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            oSession.Send(ePacketType.PACKET_STOP_CAPTURE_SCREEN_REQUEST, null);
        } 

        #endregion

        private void ctrlAltDelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Win32API.keybd_event(0x11, 0, 0, 0);
            Win32API.keybd_event(18, 0, 0, 0);
            Win32API.keybd_event(0x2E, 0, 0, 0);
        }
    }
}
