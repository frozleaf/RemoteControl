using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RemoteControl.Protocals;

namespace RemoteControl.Server
{
    public partial class FrmSendMessage : FrmOkCancelBase
    {
        public RequestMessageBox Request;

        public FrmSendMessage()
        {
            InitializeComponent();
        }

        private void FrmSendMessage_Load(object sender, EventArgs e)
        {
            UIUtil.BindTextBoxCtrlA(this.textBox1);
            UIUtil.BindTextBoxCtrlA(this.textBox2);
            // 初始化按钮类型
            string[] buttonTypeNames = Enum.GetNames(typeof(MessageBoxButtons));
            foreach (var item in buttonTypeNames)
            {
                RadioButton rb = new RadioButton();
                rb.Text = item;
                this.flowLayoutPanel1.Controls.Add(rb);
            }
            (this.flowLayoutPanel1.Controls[0] as RadioButton).Checked = true;
            // 初始化图标类型
            string[] iconTypeNames = Enum.GetNames(typeof(MessageBoxIcon));
            foreach (var item in iconTypeNames)
            {
                this.comboBox2.Items.Add(item);
            }
            this.comboBox2.Text = MessageBoxIcon.Information.ToString();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.Request = new RequestMessageBox();
            this.Request.Title = this.textBox1.Text;
            this.Request.Content = this.textBox2.Text;
            string buttonTypeStr = string.Empty;
            foreach (var item in this.flowLayoutPanel1.Controls)
            {
                RadioButton rb = item as RadioButton;
                if (rb.Checked)
                {
                    buttonTypeStr = rb.Text;
                }
            }
            this.Request.MessageBoxButtons = (int)Enum.Parse(typeof(MessageBoxButtons), buttonTypeStr);
            this.Request.MessageBoxIcons = (int)Enum.Parse(typeof(MessageBoxIcon), this.comboBox2.Text);

            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Request = null;
            this.Close();
        }
    }
}
