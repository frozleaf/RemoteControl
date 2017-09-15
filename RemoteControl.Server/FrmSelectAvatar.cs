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
    public partial class FrmSelectAvatar : FrmBase
    {
        public string SelectedAvatarFile { get; private set; }
        public FrmSelectAvatar()
        {
            InitializeComponent();
            this.flowLayoutPanel1.FlowDirection = FlowDirection.LeftToRight;
            this.flowLayoutPanel1.AutoScroll = true;
            base.EnableCancelButton = true;
        }

        private void FrmSelectAvatar_Load(object sender, EventArgs e)
        {
            var files = RSCApplication.GetAllAvatarFiles();
            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];
                var avatar = new PictureBox();
                avatar.Width = 42;
                avatar.Height = 42;
                avatar.BackgroundImage = Image.FromFile(file);
                avatar.BackgroundImageLayout = ImageLayout.Stretch;
                avatar.Tag = file;
                avatar.Margin = new Padding(10, 5, 10, 5);
                avatar.MouseEnter += new EventHandler(avatar_MouseEnter);
                avatar.MouseLeave += new EventHandler(avatar_MouseLeave);
                avatar.MouseDoubleClick += new MouseEventHandler(avatar_MouseDoubleClick);
                this.flowLayoutPanel1.Controls.Add(avatar);
            }
        }

        void avatar_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            this.SelectedAvatarFile = pb.Tag as string;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        void avatar_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            pb.BorderStyle = BorderStyle.None;
        }

        void avatar_MouseEnter(object sender, EventArgs e)
        {

            PictureBox pb = sender as PictureBox;
            pb.BorderStyle = BorderStyle.FixedSingle;
        }
    }
}
