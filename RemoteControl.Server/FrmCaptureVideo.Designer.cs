namespace RemoteControl.Server
{
    partial class FrmCaptureVideo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCaptureVideo));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripSplitButton();
            this.实时保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSplitButton2 = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripMenuItemFPS1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFPS2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFPS3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFPS4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFPS5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFPS8 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFPS10 = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton2,
            this.toolStripButtonSave,
            this.toolStripSplitButton2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(693, 31);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(89, 28);
            this.toolStripButton2.Text = "开始监控";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.实时保存ToolStripMenuItem});
            this.toolStripButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSave.Image")));
            this.toolStripButtonSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(96, 28);
            this.toolStripButtonSave.Text = "保存屏幕";
            this.toolStripButtonSave.ButtonClick += new System.EventHandler(this.toolStripButtonSave_ButtonClick);
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // 实时保存ToolStripMenuItem
            // 
            this.实时保存ToolStripMenuItem.Name = "实时保存ToolStripMenuItem";
            this.实时保存ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.实时保存ToolStripMenuItem.Text = "实时保存";
            this.实时保存ToolStripMenuItem.Click += new System.EventHandler(this.实时保存ToolStripMenuItem_Click);
            // 
            // toolStripSplitButton2
            // 
            this.toolStripSplitButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemFPS1,
            this.toolStripMenuItemFPS2,
            this.toolStripMenuItemFPS3,
            this.toolStripMenuItemFPS4,
            this.toolStripMenuItemFPS5,
            this.toolStripMenuItemFPS8,
            this.toolStripMenuItemFPS10});
            this.toolStripSplitButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton2.Image")));
            this.toolStripSplitButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton2.Name = "toolStripSplitButton2";
            this.toolStripSplitButton2.Size = new System.Drawing.Size(72, 28);
            this.toolStripSplitButton2.Text = "帧率选择";
            // 
            // toolStripMenuItemFPS1
            // 
            this.toolStripMenuItemFPS1.Name = "toolStripMenuItemFPS1";
            this.toolStripMenuItemFPS1.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItemFPS1.Tag = "1";
            this.toolStripMenuItemFPS1.Text = "1";
            this.toolStripMenuItemFPS1.Click += new System.EventHandler(this.toolStripMenuItemFPS_Click);
            // 
            // toolStripMenuItemFPS2
            // 
            this.toolStripMenuItemFPS2.Checked = true;
            this.toolStripMenuItemFPS2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItemFPS2.Name = "toolStripMenuItemFPS2";
            this.toolStripMenuItemFPS2.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItemFPS2.Tag = "2";
            this.toolStripMenuItemFPS2.Text = "2";
            this.toolStripMenuItemFPS2.Click += new System.EventHandler(this.toolStripMenuItemFPS_Click);
            // 
            // toolStripMenuItemFPS3
            // 
            this.toolStripMenuItemFPS3.Name = "toolStripMenuItemFPS3";
            this.toolStripMenuItemFPS3.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItemFPS3.Tag = "3";
            this.toolStripMenuItemFPS3.Text = "3";
            this.toolStripMenuItemFPS3.Click += new System.EventHandler(this.toolStripMenuItemFPS_Click);
            // 
            // toolStripMenuItemFPS4
            // 
            this.toolStripMenuItemFPS4.Name = "toolStripMenuItemFPS4";
            this.toolStripMenuItemFPS4.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItemFPS4.Tag = "4";
            this.toolStripMenuItemFPS4.Text = "4";
            this.toolStripMenuItemFPS4.Click += new System.EventHandler(this.toolStripMenuItemFPS_Click);
            // 
            // toolStripMenuItemFPS5
            // 
            this.toolStripMenuItemFPS5.Name = "toolStripMenuItemFPS5";
            this.toolStripMenuItemFPS5.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItemFPS5.Tag = "5";
            this.toolStripMenuItemFPS5.Text = "5";
            this.toolStripMenuItemFPS5.Click += new System.EventHandler(this.toolStripMenuItemFPS_Click);
            // 
            // toolStripMenuItemFPS8
            // 
            this.toolStripMenuItemFPS8.Name = "toolStripMenuItemFPS8";
            this.toolStripMenuItemFPS8.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItemFPS8.Tag = "8";
            this.toolStripMenuItemFPS8.Text = "8";
            this.toolStripMenuItemFPS8.Click += new System.EventHandler(this.toolStripMenuItemFPS_Click);
            // 
            // toolStripMenuItemFPS10
            // 
            this.toolStripMenuItemFPS10.Name = "toolStripMenuItemFPS10";
            this.toolStripMenuItemFPS10.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItemFPS10.Tag = "10";
            this.toolStripMenuItemFPS10.Text = "10";
            this.toolStripMenuItemFPS10.Click += new System.EventHandler(this.toolStripMenuItemFPS_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 31);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(693, 421);
            this.panel1.TabIndex = 2;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 452);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(693, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(92, 17);
            this.toolStripStatusLabel1.Text = "图像采集时间：";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(92, 17);
            this.toolStripStatusLabel2.Text = "图像返回时间：";
            // 
            // FrmCaptureVideo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 474);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FrmCaptureVideo";
            this.Text = "抓取视频";
            this.Load += new System.EventHandler(this.FrmCaptureScreen_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripSplitButton toolStripButtonSave;
        private System.Windows.Forms.ToolStripMenuItem 实时保存ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFPS1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFPS2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFPS3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFPS4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFPS5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFPS8;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFPS10;
    }
}