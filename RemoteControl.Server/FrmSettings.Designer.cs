namespace RemoteControl.Server
{
    partial class FrmSettings
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
            this.button1 = new System.Windows.Forms.Button();
            this.buttonSaveServerSetting = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.checkBoxAppIcon = new System.Windows.Forms.CheckBox();
            this.pictureBoxAppIcon = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.pictureBoxAvatar = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxServiceName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonSelectIP = new System.Windows.Forms.Button();
            this.checkBoxHideClient = new System.Windows.Forms.CheckBox();
            this.buttonGenClient = new System.Windows.Forms.Button();
            this.textBoxServerPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxServerIP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.textBoxLocalServerPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAppIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAvatar)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(421, 341);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "取消";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonSaveServerSetting
            // 
            this.buttonSaveServerSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveServerSetting.Location = new System.Drawing.Point(340, 341);
            this.buttonSaveServerSetting.Name = "buttonSaveServerSetting";
            this.buttonSaveServerSetting.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveServerSetting.TabIndex = 5;
            this.buttonSaveServerSetting.Text = "确定";
            this.buttonSaveServerSetting.UseVisualStyleBackColor = true;
            this.buttonSaveServerSetting.Click += new System.EventHandler(this.buttonSaveServerSetting_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(8, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(492, 332);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.checkBoxAppIcon);
            this.tabPage1.Controls.Add(this.pictureBoxAppIcon);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.pictureBoxAvatar);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.textBoxServiceName);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.buttonSelectIP);
            this.tabPage1.Controls.Add(this.checkBoxHideClient);
            this.tabPage1.Controls.Add(this.buttonGenClient);
            this.tabPage1.Controls.Add(this.textBoxServerPort);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.textBoxServerIP);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(484, 306);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "客户端配置";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // checkBoxAppIcon
            // 
            this.checkBoxAppIcon.AutoSize = true;
            this.checkBoxAppIcon.Checked = true;
            this.checkBoxAppIcon.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAppIcon.Location = new System.Drawing.Point(79, 201);
            this.checkBoxAppIcon.Name = "checkBoxAppIcon";
            this.checkBoxAppIcon.Size = new System.Drawing.Size(15, 14);
            this.checkBoxAppIcon.TabIndex = 11;
            this.checkBoxAppIcon.UseVisualStyleBackColor = true;
            this.checkBoxAppIcon.CheckedChanged += new System.EventHandler(this.checkBoxAppIcon_CheckedChanged);
            // 
            // pictureBoxAppIcon
            // 
            this.pictureBoxAppIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxAppIcon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxAppIcon.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxAppIcon.Location = new System.Drawing.Point(176, 208);
            this.pictureBoxAppIcon.Name = "pictureBoxAppIcon";
            this.pictureBoxAppIcon.Size = new System.Drawing.Size(64, 64);
            this.pictureBoxAppIcon.TabIndex = 10;
            this.pictureBoxAppIcon.TabStop = false;
            this.pictureBoxAppIcon.Tag = "";
            this.pictureBoxAppIcon.Click += new System.EventHandler(this.pictureBoxAppIcon_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(100, 201);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "程序图标：";
            // 
            // pictureBoxAvatar
            // 
            this.pictureBoxAvatar.BackgroundImage = global::RemoteControl.Server.Properties.Resources._16238_100;
            this.pictureBoxAvatar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxAvatar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxAvatar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxAvatar.Location = new System.Drawing.Point(176, 127);
            this.pictureBoxAvatar.Name = "pictureBoxAvatar";
            this.pictureBoxAvatar.Size = new System.Drawing.Size(64, 64);
            this.pictureBoxAvatar.TabIndex = 8;
            this.pictureBoxAvatar.TabStop = false;
            this.pictureBoxAvatar.Tag = "16238_100.png";
            this.pictureBoxAvatar.Click += new System.EventHandler(this.pictureBoxAvatar_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(100, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 7;
            this.label5.Text = "上线图标：";
            // 
            // textBoxServiceName
            // 
            this.textBoxServiceName.Location = new System.Drawing.Point(171, 87);
            this.textBoxServiceName.Name = "textBoxServiceName";
            this.textBoxServiceName.Size = new System.Drawing.Size(201, 21);
            this.textBoxServiceName.TabIndex = 6;
            this.textBoxServiceName.Text = "360se.exe";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(100, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "服务名称：";
            // 
            // buttonSelectIP
            // 
            this.buttonSelectIP.Location = new System.Drawing.Point(378, 31);
            this.buttonSelectIP.Name = "buttonSelectIP";
            this.buttonSelectIP.Size = new System.Drawing.Size(34, 23);
            this.buttonSelectIP.TabIndex = 4;
            this.buttonSelectIP.Text = "选";
            this.buttonSelectIP.UseVisualStyleBackColor = true;
            this.buttonSelectIP.Click += new System.EventHandler(this.buttonSelectIP_Click);
            // 
            // checkBoxHideClient
            // 
            this.checkBoxHideClient.AutoSize = true;
            this.checkBoxHideClient.Checked = true;
            this.checkBoxHideClient.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxHideClient.Location = new System.Drawing.Point(276, 127);
            this.checkBoxHideClient.Name = "checkBoxHideClient";
            this.checkBoxHideClient.Size = new System.Drawing.Size(96, 16);
            this.checkBoxHideClient.TabIndex = 3;
            this.checkBoxHideClient.Text = "是否隐藏窗体";
            this.checkBoxHideClient.UseVisualStyleBackColor = true;
            // 
            // buttonGenClient
            // 
            this.buttonGenClient.Location = new System.Drawing.Point(362, 261);
            this.buttonGenClient.Name = "buttonGenClient";
            this.buttonGenClient.Size = new System.Drawing.Size(75, 23);
            this.buttonGenClient.TabIndex = 2;
            this.buttonGenClient.Text = "生成客户端";
            this.buttonGenClient.UseVisualStyleBackColor = true;
            this.buttonGenClient.Click += new System.EventHandler(this.buttonGenClient_Click);
            // 
            // textBoxServerPort
            // 
            this.textBoxServerPort.Location = new System.Drawing.Point(171, 60);
            this.textBoxServerPort.Name = "textBoxServerPort";
            this.textBoxServerPort.Size = new System.Drawing.Size(201, 21);
            this.textBoxServerPort.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(88, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "服务器端口：";
            // 
            // textBoxServerIP
            // 
            this.textBoxServerIP.Location = new System.Drawing.Point(171, 33);
            this.textBoxServerIP.Name = "textBoxServerIP";
            this.textBoxServerIP.Size = new System.Drawing.Size(201, 21);
            this.textBoxServerIP.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(100, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务器IP：";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.textBoxLocalServerPort);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(484, 306);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "服务端配置";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // textBoxLocalServerPort
            // 
            this.textBoxLocalServerPort.Location = new System.Drawing.Point(191, 33);
            this.textBoxLocalServerPort.Name = "textBoxLocalServerPort";
            this.textBoxLocalServerPort.Size = new System.Drawing.Size(201, 21);
            this.textBoxLocalServerPort.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(108, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "服务器端口：";
            // 
            // FrmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(509, 368);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonSaveServerSetting);
            this.Controls.Add(this.tabControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSettings";
            this.Text = "设置";
            this.Load += new System.EventHandler(this.FrmSettings_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAppIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAvatar)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox textBoxServerPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxServerIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonGenClient;
        private System.Windows.Forms.TextBox textBoxLocalServerPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBoxHideClient;
        private System.Windows.Forms.Button buttonSelectIP;
        private System.Windows.Forms.Button buttonSaveServerSetting;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBoxServiceName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBoxAvatar;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pictureBoxAppIcon;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkBoxAppIcon;

    }
}