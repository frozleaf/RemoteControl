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
    public partial class FrmDownload : FrmBase
    {
        private long _fileSize;
        private Action _cancelAction;
        public FrmDownload(Action cancelAction, string sourceFile, string destFile, long fileSize):this(cancelAction, sourceFile,destFile,fileSize, true)
        {
        }
        public FrmDownload(Action cancelAction, string sourceFile, string destFile, long fileSize, bool isDownloadMode)
        {
            InitializeComponent();

            this._cancelAction = cancelAction;
            this.label2.Text = String.Format("正在{2}文件 {0} 到 {1} 目录中...", 
                System.IO.Path.GetFileName(sourceFile),
                System.IO.Path.GetDirectoryName(destFile),
                isDownloadMode?"下载":"上传");
            this._fileSize = fileSize;
            this.label1.Text = string.Format("{0}/{1}", 0, this._fileSize);
            this.Text = isDownloadMode?"下载文件":"上传文件";
        }

        public void UpdateProgress(long recvedBytes)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<long>(UpdateProgress), recvedBytes);
                return;
            }
            this.label1.Text = string.Format("{0}/{1}", recvedBytes, this._fileSize);
            this.progressBar1.Maximum = 100;
            this.progressBar1.Value = (int)(recvedBytes * 1.0 / this._fileSize * 100);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_cancelAction != null)
            {
                _cancelAction();
            }
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}
