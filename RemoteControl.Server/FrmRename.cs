using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RemoteControl.Server
{
    public partial class FrmRename : FrmOkCancelBase
    {
        public string NewName;

        public FrmRename(string oldName)
        {
            InitializeComponent();
            this.textBox1.Text = oldName;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.NewName = this.textBox1.Text.Trim();
            if (this.NewName.Length < 1)
            {
                MsgBox.ShowInfo("新名称不能为空！");
                return;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}
