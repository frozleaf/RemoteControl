using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RemoteControl.Server
{
    public partial class FrmAbout : FrmBase
    {
        public FrmAbout()
        {
            InitializeComponent();
            base.EnableCancelButton = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.label2.Text = "版本：" + Application.ProductVersion;
        }

        private void FrmAbout_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < this.Controls.Count; i++)
            {
                this.Controls[i].Click += (o, args) =>
                {
                    base.Quit();
                };
            }
            this.Click += (o, args) =>
            {
                base.Quit();
            };
        }
    }
}
