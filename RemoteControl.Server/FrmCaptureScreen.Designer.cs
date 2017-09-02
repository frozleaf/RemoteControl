namespace RemoteControl.Server
{
    partial class FrmCaptureScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCaptureScreen));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripMenuItemCaptureMouse = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCaptureKeyboard = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSplitButton2 = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripMenuItemFPS1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFPS5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFPS10 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFPS15 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFPS60 = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton2,
            this.toolStripButtonSave,
            this.toolStripSeparator1,
            this.toolStripSplitButton1,
            this.toolStripSeparator2,
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
            this.toolStripButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSave.Image")));
            this.toolStripButtonSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(84, 28);
            this.toolStripButtonSave.Text = "保存屏幕";
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemCaptureMouse,
            this.toolStripMenuItemCaptureKeyboard});
            this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(72, 28);
            this.toolStripSplitButton1.Text = "捕捉操作";
            this.toolStripSplitButton1.ButtonClick += new System.EventHandler(this.toolStripSplitButton1_ButtonClick);
            // 
            // toolStripMenuItemCaptureMouse
            // 
            this.toolStripMenuItemCaptureMouse.Name = "toolStripMenuItemCaptureMouse";
            this.toolStripMenuItemCaptureMouse.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItemCaptureMouse.Text = "鼠标";
            this.toolStripMenuItemCaptureMouse.Click += new System.EventHandler(this.toolStripMenuItemCaptureMouse_Click);
            // 
            // toolStripMenuItemCaptureKeyboard
            // 
            this.toolStripMenuItemCaptureKeyboard.Name = "toolStripMenuItemCaptureKeyboard";
            this.toolStripMenuItemCaptureKeyboard.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItemCaptureKeyboard.Text = "键盘";
            this.toolStripMenuItemCaptureKeyboard.Click += new System.EventHandler(this.toolStripMenuItemCaptureKeyboard_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripSplitButton2
            // 
            this.toolStripSplitButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemFPS1,
            this.toolStripMenuItemFPS5,
            this.toolStripMenuItemFPS10,
            this.toolStripMenuItemFPS15,
            this.toolStripMenuItemFPS60});
            this.toolStripSplitButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton2.Image")));
            this.toolStripSplitButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton2.Name = "toolStripSplitButton2";
            this.toolStripSplitButton2.Size = new System.Drawing.Size(72, 28);
            this.toolStripSplitButton2.Text = "帧率选择";
            this.toolStripSplitButton2.ButtonClick += new System.EventHandler(this.toolStripSplitButton2_ButtonClick);
            // 
            // toolStripMenuItemFPS1
            // 
            this.toolStripMenuItemFPS1.Name = "toolStripMenuItemFPS1";
            this.toolStripMenuItemFPS1.Size = new System.Drawing.Size(90, 22);
            this.toolStripMenuItemFPS1.Tag = "1";
            this.toolStripMenuItemFPS1.Text = "1";
            this.toolStripMenuItemFPS1.Click += new System.EventHandler(this.toolStripMenuItemFPS_Click);
            // 
            // toolStripMenuItemFPS5
            // 
            this.toolStripMenuItemFPS5.Checked = true;
            this.toolStripMenuItemFPS5.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItemFPS5.Name = "toolStripMenuItemFPS5";
            this.toolStripMenuItemFPS5.Size = new System.Drawing.Size(90, 22);
            this.toolStripMenuItemFPS5.Tag = "5";
            this.toolStripMenuItemFPS5.Text = "5";
            this.toolStripMenuItemFPS5.Click += new System.EventHandler(this.toolStripMenuItemFPS_Click);
            // 
            // toolStripMenuItemFPS10
            // 
            this.toolStripMenuItemFPS10.Name = "toolStripMenuItemFPS10";
            this.toolStripMenuItemFPS10.Size = new System.Drawing.Size(90, 22);
            this.toolStripMenuItemFPS10.Tag = "10";
            this.toolStripMenuItemFPS10.Text = "10";
            this.toolStripMenuItemFPS10.Click += new System.EventHandler(this.toolStripMenuItemFPS_Click);
            // 
            // toolStripMenuItemFPS15
            // 
            this.toolStripMenuItemFPS15.Name = "toolStripMenuItemFPS15";
            this.toolStripMenuItemFPS15.Size = new System.Drawing.Size(90, 22);
            this.toolStripMenuItemFPS15.Tag = "15";
            this.toolStripMenuItemFPS15.Text = "15";
            this.toolStripMenuItemFPS15.Click += new System.EventHandler(this.toolStripMenuItemFPS_Click);
            // 
            // toolStripMenuItemFPS60
            // 
            this.toolStripMenuItemFPS60.Name = "toolStripMenuItemFPS60";
            this.toolStripMenuItemFPS60.Size = new System.Drawing.Size(90, 22);
            this.toolStripMenuItemFPS60.Tag = "60";
            this.toolStripMenuItemFPS60.Text = "60";
            this.toolStripMenuItemFPS60.Click += new System.EventHandler(this.toolStripMenuItemFPS_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDoubleClick);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 31);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(693, 443);
            this.panel1.TabIndex = 2;
            // 
            // FrmCaptureScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 474);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.KeyPreview = true;
            this.Name = "FrmCaptureScreen";
            this.Text = "抓取屏幕";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmCaptureScreen_FormClosing);
            this.Load += new System.EventHandler(this.FrmCaptureScreen_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmCaptureScreen_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FrmCaptureScreen_KeyUp);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCaptureMouse;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCaptureKeyboard;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFPS1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFPS5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFPS10;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFPS15;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFPS60;
    }
}