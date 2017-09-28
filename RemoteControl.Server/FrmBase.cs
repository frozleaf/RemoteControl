using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RemoteControl.Server
{
    public partial class FrmBase : Form
    {
        private bool enableCancelButton;

        public bool EnableCancelButton
        {
            get { return enableCancelButton; }
            set { enableCancelButton = value; }
        }

        public FrmBase()
        {
            InitializeComponent();
        }

        private void FrmBase_Load(object sender, EventArgs e)
        {
            if (this.enableCancelButton)
            {
                Button btn = new Button();
                btn.Click += (o, args) =>
                    {
                        Quit();
                    };
                this.CancelButton = btn;
            }
        }

        protected void Quit()
        {
            if (this.Modal)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
            else
            {
                this.Close();
            }
        }
    }
}
