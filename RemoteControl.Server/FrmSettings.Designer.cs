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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.checkBoxHideClient = new System.Windows.Forms.CheckBox();
            this.buttonGenClient = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.buttonSaveServerSetting = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonSelectIP = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(438, 339);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.buttonSelectIP);
            this.tabPage1.Controls.Add(this.checkBoxHideClient);
            this.tabPage1.Controls.Add(this.buttonGenClient);
            this.tabPage1.Controls.Add(this.textBox2);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.textBox1);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(430, 313);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "客户端配置";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // checkBoxHideClient
            // 
            this.checkBoxHideClient.AutoSize = true;
            this.checkBoxHideClient.Location = new System.Drawing.Point(105, 97);
            this.checkBoxHideClient.Name = "checkBoxHideClient";
            this.checkBoxHideClient.Size = new System.Drawing.Size(96, 16);
            this.checkBoxHideClient.TabIndex = 3;
            this.checkBoxHideClient.Text = "是否隐藏窗体";
            this.checkBoxHideClient.UseVisualStyleBackColor = true;
            // 
            // buttonGenClient
            // 
            this.buttonGenClient.Location = new System.Drawing.Point(231, 175);
            this.buttonGenClient.Name = "buttonGenClient";
            this.buttonGenClient.Size = new System.Drawing.Size(75, 23);
            this.buttonGenClient.TabIndex = 2;
            this.buttonGenClient.Text = "生成客户端";
            this.buttonGenClient.UseVisualStyleBackColor = true;
            this.buttonGenClient.Click += new System.EventHandler(this.buttonGenClient_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(105, 60);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(201, 21);
            this.textBox2.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "服务器端口：";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(105, 33);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(201, 21);
            this.textBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务器IP：";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.buttonSaveServerSetting);
            this.tabPage2.Controls.Add(this.textBox3);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(430, 313);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "服务端配置";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // buttonSaveServerSetting
            // 
            this.buttonSaveServerSetting.Location = new System.Drawing.Point(236, 128);
            this.buttonSaveServerSetting.Name = "buttonSaveServerSetting";
            this.buttonSaveServerSetting.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveServerSetting.TabIndex = 4;
            this.buttonSaveServerSetting.Text = "保存";
            this.buttonSaveServerSetting.UseVisualStyleBackColor = true;
            this.buttonSaveServerSetting.Click += new System.EventHandler(this.buttonSaveServerSetting_Click);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(123, 28);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(201, 21);
            this.textBox3.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(40, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "服务器端口：";
            // 
            // buttonSelectIP
            // 
            this.buttonSelectIP.Location = new System.Drawing.Point(312, 31);
            this.buttonSelectIP.Name = "buttonSelectIP";
            this.buttonSelectIP.Size = new System.Drawing.Size(34, 23);
            this.buttonSelectIP.TabIndex = 4;
            this.buttonSelectIP.Text = "选";
            this.buttonSelectIP.UseVisualStyleBackColor = true;
            this.buttonSelectIP.Click += new System.EventHandler(this.buttonSelectIP_Click);
            // 
            // FrmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 339);
            this.Controls.Add(this.tabControl1);
            this.Name = "FrmSettings";
            this.Text = "设置";
            this.Load += new System.EventHandler(this.FrmSettings_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonGenClient;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonSaveServerSetting;
        private System.Windows.Forms.CheckBox checkBoxHideClient;
        private System.Windows.Forms.Button buttonSelectIP;

    }
}