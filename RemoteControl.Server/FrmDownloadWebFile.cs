using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RemoteControl.Server
{
    public partial class FrmDownloadWebFile : FrmOkCancelBase
    {
        public string WebUrl;
        public string DestFilePath;

        public FrmDownloadWebFile()
        {
            InitializeComponent();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.WebUrl = this.textBox1.Text;
            this.DestFilePath = this.textBox2.Text;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.WebUrl = null;
            this.DestFilePath = null;
            this.Close();
        }
    }
}
