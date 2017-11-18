namespace RemoteControl.Server
{
    partial class FrmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("我的电脑", 3, 3);
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("自动上线主机");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("HKEY_CLASSES_ROOT");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("HKEY_CURRENT_USER");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("HKEY_LOCAL_MACHINE");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("HKEY_USERS");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("HKEY_CURRENT_CONFIG");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("计算机", 1, 1, new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6,
            treeNode7});
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.listView1 = new RemoteControl.Server.SortableListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.复制全路径ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开文件隐藏模式ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.播放印业ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.停止播放音乐ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.远程下载到此处ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton12 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton8 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton9 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton10 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton11 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton13 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton14 = new System.Windows.Forms.ToolStripButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonStopPlayMusic = new System.Windows.Forms.Button();
            this.buttonPlayMusic = new System.Windows.Forms.Button();
            this.buttonRemoteDownloadWebUrl = new System.Windows.Forms.Button();
            this.buttonUnLockMouse = new System.Windows.Forms.Button();
            this.buttonLockMouse = new System.Windows.Forms.Button();
            this.buttonBlackScreen = new System.Windows.Forms.Button();
            this.buttonUnBlackScreen = new System.Windows.Forms.Button();
            this.buttonSendMessage = new System.Windows.Forms.Button();
            this.buttonOpenUrl = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonShutdownComputer = new System.Windows.Forms.Button();
            this.buttonRebootComputer = new System.Windows.Forms.Button();
            this.buttonCloseCD = new System.Windows.Forms.Button();
            this.buttonOpenCD = new System.Windows.Forms.Button();
            this.buttonSleepComputer = new System.Windows.Forms.Button();
            this.buttonExeCode = new System.Windows.Forms.Button();
            this.buttonHibernateComputer = new System.Windows.Forms.Button();
            this.buttonLockComputer = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonSelectSendCmdMode = new System.Windows.Forms.Button();
            this.textBoxCommandResponse = new System.Windows.Forms.TextBox();
            this.buttonSendCommand = new System.Windows.Forms.Button();
            this.textBoxCommandRequest = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.treeView2 = new System.Windows.Forms.TreeView();
            this.imgListRegistryKey = new System.Windows.Forms.ImageList(this.components);
            this.listView2 = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imgListRegistryValue = new System.Windows.Forms.ImageList(this.components);
            this.textBoxRegistryPath = new System.Windows.Forms.TextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.listView3 = new System.Windows.Forms.ListView();
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader16 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.刷新急速ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.刷新ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.结束进程ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBox2 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonCaptureVideo = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSettings = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件FToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemTools = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemUsualFolders = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSkins = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助HToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.skinEngine1 = new Sunisoft.IrisSkin.SkinEngine(((System.ComponentModel.Component)(this)));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "dvd");
            this.imageList2.Images.SetKeyName(1, "qq");
            this.imageList2.Images.SetKeyName(2, "command");
            this.imageList2.Images.SetKeyName(3, "computer");
            this.imageList2.Images.SetKeyName(4, "disk");
            this.imageList2.Images.SetKeyName(5, "folder");
            this.imageList2.Images.SetKeyName(6, "internet");
            this.imageList2.Images.SetKeyName(7, "lock");
            this.imageList2.Images.SetKeyName(8, "save");
            this.imageList2.Images.SetKeyName(9, "up");
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(24, 24);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 98);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.richTextBox1);
            this.splitContainer2.Size = new System.Drawing.Size(987, 530);
            this.splitContainer2.SplitterDistance = 444;
            this.splitContainer2.TabIndex = 6;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ItemSize = new System.Drawing.Size(72, 25);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(987, 444);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(979, 411);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "文件管理器";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.splitContainer1.Panel2.Controls.Add(this.listView1);
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip3);
            this.splitContainer1.Size = new System.Drawing.Size(973, 405);
            this.splitContainer1.SplitterDistance = 272;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList2;
            this.treeView1.ItemHeight = 24;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            treeNode1.ImageIndex = 3;
            treeNode1.Name = "treeNodeLocal";
            treeNode1.SelectedImageIndex = 3;
            treeNode1.Text = "我的电脑";
            treeNode2.ImageIndex = 6;
            treeNode2.Name = "treeNodeInternet";
            treeNode2.SelectedImageKey = "internet";
            treeNode2.Text = "自动上线主机";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(272, 405);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseDoubleClick);
            this.treeView1.MouseHover += new System.EventHandler(this.treeView1_MouseHover);
            this.treeView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseUp);
            // 
            // listView1
            // 
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader7});
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.LargeImageList = this.imageList1;
            this.listView1.Location = new System.Drawing.Point(0, 31);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(697, 374);
            this.listView1.SmallImageList = this.imageList1;
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView1_KeyDown);
            this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "名称";
            this.columnHeader1.Width = 232;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "大小(KB)";
            this.columnHeader2.Width = 130;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "修改日期";
            this.columnHeader3.Width = 127;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "类型";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.复制全路径ToolStripMenuItem,
            this.刷新ToolStripMenuItem,
            this.打开文件ToolStripMenuItem,
            this.打开文件隐藏模式ToolStripMenuItem,
            this.播放印业ToolStripMenuItem,
            this.停止播放音乐ToolStripMenuItem,
            this.远程下载到此处ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(197, 158);
            // 
            // 复制全路径ToolStripMenuItem
            // 
            this.复制全路径ToolStripMenuItem.Name = "复制全路径ToolStripMenuItem";
            this.复制全路径ToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.复制全路径ToolStripMenuItem.Text = "复制全路径";
            this.复制全路径ToolStripMenuItem.Click += new System.EventHandler(this.复制全路径ToolStripMenuItem_Click);
            // 
            // 刷新ToolStripMenuItem
            // 
            this.刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            this.刷新ToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.刷新ToolStripMenuItem.Text = "刷新";
            this.刷新ToolStripMenuItem.Click += new System.EventHandler(this.刷新ToolStripMenuItem_Click);
            // 
            // 打开文件ToolStripMenuItem
            // 
            this.打开文件ToolStripMenuItem.Name = "打开文件ToolStripMenuItem";
            this.打开文件ToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.打开文件ToolStripMenuItem.Text = "打开文件";
            this.打开文件ToolStripMenuItem.Click += new System.EventHandler(this.打开文件ToolStripMenuItem_Click);
            // 
            // 打开文件隐藏模式ToolStripMenuItem
            // 
            this.打开文件隐藏模式ToolStripMenuItem.Name = "打开文件隐藏模式ToolStripMenuItem";
            this.打开文件隐藏模式ToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.打开文件隐藏模式ToolStripMenuItem.Text = "打开文件（隐藏模式）";
            this.打开文件隐藏模式ToolStripMenuItem.Click += new System.EventHandler(this.打开文件隐藏模式ToolStripMenuItem_Click);
            // 
            // 播放印业ToolStripMenuItem
            // 
            this.播放印业ToolStripMenuItem.Name = "播放印业ToolStripMenuItem";
            this.播放印业ToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.播放印业ToolStripMenuItem.Text = "播放音乐";
            this.播放印业ToolStripMenuItem.Click += new System.EventHandler(this.播放音乐ToolStripMenuItem_Click);
            // 
            // 停止播放音乐ToolStripMenuItem
            // 
            this.停止播放音乐ToolStripMenuItem.Name = "停止播放音乐ToolStripMenuItem";
            this.停止播放音乐ToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.停止播放音乐ToolStripMenuItem.Text = "停止播放音乐";
            this.停止播放音乐ToolStripMenuItem.Click += new System.EventHandler(this.停止播放音乐ToolStripMenuItem_Click);
            // 
            // 远程下载到此处ToolStripMenuItem
            // 
            this.远程下载到此处ToolStripMenuItem.Name = "远程下载到此处ToolStripMenuItem";
            this.远程下载到此处ToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.远程下载到此处ToolStripMenuItem.Text = "远程下载到此处";
            this.远程下载到此处ToolStripMenuItem.Click += new System.EventHandler(this.远程下载到此处ToolStripMenuItem_Click);
            // 
            // toolStrip3
            // 
            this.toolStrip3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.toolStrip3.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton7,
            this.toolStripButton12,
            this.toolStripButton8,
            this.toolStripButton2,
            this.toolStripButton9,
            this.toolStripSeparator1,
            this.toolStripButton10,
            this.toolStripButton11,
            this.toolStripSeparator2,
            this.toolStripSplitButton1,
            this.toolStripSeparator3,
            this.toolStripButton13,
            this.toolStripButton14});
            this.toolStrip3.Location = new System.Drawing.Point(0, 0);
            this.toolStrip3.Name = "toolStrip3";
            this.toolStrip3.Size = new System.Drawing.Size(697, 31);
            this.toolStrip3.TabIndex = 0;
            this.toolStrip3.Text = "toolStrip3";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(28, 28);
            this.toolStripButton1.Text = "向上";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton7
            // 
            this.toolStripButton7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton7.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton7.Image")));
            this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton7.Name = "toolStripButton7";
            this.toolStripButton7.Size = new System.Drawing.Size(28, 28);
            this.toolStripButton7.Text = "复制";
            this.toolStripButton7.Click += new System.EventHandler(this.toolStripButton7_Click);
            // 
            // toolStripButton12
            // 
            this.toolStripButton12.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton12.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton12.Image")));
            this.toolStripButton12.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton12.Name = "toolStripButton12";
            this.toolStripButton12.Size = new System.Drawing.Size(28, 28);
            this.toolStripButton12.Text = "toolStripButton12";
            this.toolStripButton12.Click += new System.EventHandler(this.toolStripButton12_Click);
            // 
            // toolStripButton8
            // 
            this.toolStripButton8.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton8.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton8.Image")));
            this.toolStripButton8.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton8.Name = "toolStripButton8";
            this.toolStripButton8.Size = new System.Drawing.Size(28, 28);
            this.toolStripButton8.Text = "粘贴";
            this.toolStripButton8.Click += new System.EventHandler(this.toolStripButton8_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(28, 28);
            this.toolStripButton2.Text = "重命名";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton9
            // 
            this.toolStripButton9.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton9.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton9.Image")));
            this.toolStripButton9.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton9.Name = "toolStripButton9";
            this.toolStripButton9.Size = new System.Drawing.Size(28, 28);
            this.toolStripButton9.Text = "删除";
            this.toolStripButton9.Click += new System.EventHandler(this.toolStripButton9_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripButton10
            // 
            this.toolStripButton10.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton10.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton10.Image")));
            this.toolStripButton10.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton10.Name = "toolStripButton10";
            this.toolStripButton10.Size = new System.Drawing.Size(28, 28);
            this.toolStripButton10.Text = "新建文本文件";
            this.toolStripButton10.Click += new System.EventHandler(this.toolStripButton10_Click);
            // 
            // toolStripButton11
            // 
            this.toolStripButton11.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton11.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton11.Image")));
            this.toolStripButton11.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton11.Name = "toolStripButton11";
            this.toolStripButton11.Size = new System.Drawing.Size(28, 28);
            this.toolStripButton11.Text = "新建文件夹";
            this.toolStripButton11.Click += new System.EventHandler(this.toolStripButton11_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(40, 28);
            this.toolStripSplitButton1.Text = "视图";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripButton13
            // 
            this.toolStripButton13.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton13.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton13.Image")));
            this.toolStripButton13.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton13.Name = "toolStripButton13";
            this.toolStripButton13.Size = new System.Drawing.Size(28, 28);
            this.toolStripButton13.Text = "上传";
            this.toolStripButton13.Click += new System.EventHandler(this.toolStripButton13_Click);
            // 
            // toolStripButton14
            // 
            this.toolStripButton14.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton14.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton14.Image")));
            this.toolStripButton14.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton14.Name = "toolStripButton14";
            this.toolStripButton14.Size = new System.Drawing.Size(28, 28);
            this.toolStripButton14.Text = "下载";
            this.toolStripButton14.Click += new System.EventHandler(this.toolStripButton14_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(979, 411);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "远程控制命令";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonStopPlayMusic);
            this.groupBox2.Controls.Add(this.buttonPlayMusic);
            this.groupBox2.Controls.Add(this.buttonRemoteDownloadWebUrl);
            this.groupBox2.Controls.Add(this.buttonUnLockMouse);
            this.groupBox2.Controls.Add(this.buttonLockMouse);
            this.groupBox2.Controls.Add(this.buttonBlackScreen);
            this.groupBox2.Controls.Add(this.buttonUnBlackScreen);
            this.groupBox2.Controls.Add(this.buttonSendMessage);
            this.groupBox2.Controls.Add(this.buttonOpenUrl);
            this.groupBox2.Location = new System.Drawing.Point(672, 184);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(295, 223);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "常用操作";
            // 
            // buttonStopPlayMusic
            // 
            this.buttonStopPlayMusic.Location = new System.Drawing.Point(13, 143);
            this.buttonStopPlayMusic.Name = "buttonStopPlayMusic";
            this.buttonStopPlayMusic.Size = new System.Drawing.Size(122, 34);
            this.buttonStopPlayMusic.TabIndex = 4;
            this.buttonStopPlayMusic.Text = "停止播放音乐";
            this.buttonStopPlayMusic.UseVisualStyleBackColor = true;
            this.buttonStopPlayMusic.Click += new System.EventHandler(this.buttonStopPlayMusic_Click);
            // 
            // buttonPlayMusic
            // 
            this.buttonPlayMusic.Location = new System.Drawing.Point(13, 102);
            this.buttonPlayMusic.Name = "buttonPlayMusic";
            this.buttonPlayMusic.Size = new System.Drawing.Size(122, 34);
            this.buttonPlayMusic.TabIndex = 4;
            this.buttonPlayMusic.Text = "播放音乐";
            this.buttonPlayMusic.UseVisualStyleBackColor = true;
            this.buttonPlayMusic.Click += new System.EventHandler(this.buttonPlayMusic_Click);
            // 
            // buttonRemoteDownloadWebUrl
            // 
            this.buttonRemoteDownloadWebUrl.Location = new System.Drawing.Point(13, 183);
            this.buttonRemoteDownloadWebUrl.Name = "buttonRemoteDownloadWebUrl";
            this.buttonRemoteDownloadWebUrl.Size = new System.Drawing.Size(122, 34);
            this.buttonRemoteDownloadWebUrl.TabIndex = 4;
            this.buttonRemoteDownloadWebUrl.Text = "远程下载";
            this.buttonRemoteDownloadWebUrl.UseVisualStyleBackColor = true;
            this.buttonRemoteDownloadWebUrl.Click += new System.EventHandler(this.buttonRemoteDownloadWebUrl_Click);
            // 
            // buttonUnLockMouse
            // 
            this.buttonUnLockMouse.Location = new System.Drawing.Point(156, 142);
            this.buttonUnLockMouse.Name = "buttonUnLockMouse";
            this.buttonUnLockMouse.Size = new System.Drawing.Size(122, 34);
            this.buttonUnLockMouse.TabIndex = 4;
            this.buttonUnLockMouse.Text = "取消锁定鼠标";
            this.buttonUnLockMouse.UseVisualStyleBackColor = true;
            this.buttonUnLockMouse.Click += new System.EventHandler(this.buttonUnLockMouse_Click);
            // 
            // buttonLockMouse
            // 
            this.buttonLockMouse.Location = new System.Drawing.Point(156, 102);
            this.buttonLockMouse.Name = "buttonLockMouse";
            this.buttonLockMouse.Size = new System.Drawing.Size(122, 34);
            this.buttonLockMouse.TabIndex = 4;
            this.buttonLockMouse.Text = "锁定鼠标";
            this.buttonLockMouse.UseVisualStyleBackColor = true;
            this.buttonLockMouse.Click += new System.EventHandler(this.buttonLockMouse_Click);
            // 
            // buttonBlackScreen
            // 
            this.buttonBlackScreen.Location = new System.Drawing.Point(156, 20);
            this.buttonBlackScreen.Name = "buttonBlackScreen";
            this.buttonBlackScreen.Size = new System.Drawing.Size(122, 34);
            this.buttonBlackScreen.TabIndex = 4;
            this.buttonBlackScreen.Text = "黑屏";
            this.buttonBlackScreen.UseVisualStyleBackColor = true;
            this.buttonBlackScreen.Click += new System.EventHandler(this.buttonBlackScreen_Click);
            // 
            // buttonUnBlackScreen
            // 
            this.buttonUnBlackScreen.Location = new System.Drawing.Point(156, 60);
            this.buttonUnBlackScreen.Name = "buttonUnBlackScreen";
            this.buttonUnBlackScreen.Size = new System.Drawing.Size(122, 34);
            this.buttonUnBlackScreen.TabIndex = 4;
            this.buttonUnBlackScreen.Text = "取消黑屏";
            this.buttonUnBlackScreen.UseVisualStyleBackColor = true;
            this.buttonUnBlackScreen.Click += new System.EventHandler(this.buttonUnBlackScreen_Click);
            // 
            // buttonSendMessage
            // 
            this.buttonSendMessage.Location = new System.Drawing.Point(13, 60);
            this.buttonSendMessage.Name = "buttonSendMessage";
            this.buttonSendMessage.Size = new System.Drawing.Size(122, 34);
            this.buttonSendMessage.TabIndex = 4;
            this.buttonSendMessage.Text = "弹框";
            this.buttonSendMessage.UseVisualStyleBackColor = true;
            this.buttonSendMessage.Click += new System.EventHandler(this.buttonSendMessage_Click);
            // 
            // buttonOpenUrl
            // 
            this.buttonOpenUrl.Location = new System.Drawing.Point(13, 20);
            this.buttonOpenUrl.Name = "buttonOpenUrl";
            this.buttonOpenUrl.Size = new System.Drawing.Size(122, 34);
            this.buttonOpenUrl.TabIndex = 4;
            this.buttonOpenUrl.Text = "打开网页";
            this.buttonOpenUrl.UseVisualStyleBackColor = true;
            this.buttonOpenUrl.Click += new System.EventHandler(this.buttonOpenUrl_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonShutdownComputer);
            this.groupBox1.Controls.Add(this.buttonRebootComputer);
            this.groupBox1.Controls.Add(this.buttonCloseCD);
            this.groupBox1.Controls.Add(this.buttonOpenCD);
            this.groupBox1.Controls.Add(this.buttonSleepComputer);
            this.groupBox1.Controls.Add(this.buttonExeCode);
            this.groupBox1.Controls.Add(this.buttonHibernateComputer);
            this.groupBox1.Controls.Add(this.buttonLockComputer);
            this.groupBox1.Location = new System.Drawing.Point(670, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(297, 180);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "系统命令";
            // 
            // buttonShutdownComputer
            // 
            this.buttonShutdownComputer.Location = new System.Drawing.Point(15, 20);
            this.buttonShutdownComputer.Name = "buttonShutdownComputer";
            this.buttonShutdownComputer.Size = new System.Drawing.Size(122, 34);
            this.buttonShutdownComputer.TabIndex = 4;
            this.buttonShutdownComputer.Text = "关机";
            this.buttonShutdownComputer.UseVisualStyleBackColor = true;
            this.buttonShutdownComputer.Click += new System.EventHandler(this.buttonShutdownComputer_Click);
            // 
            // buttonRebootComputer
            // 
            this.buttonRebootComputer.Location = new System.Drawing.Point(15, 60);
            this.buttonRebootComputer.Name = "buttonRebootComputer";
            this.buttonRebootComputer.Size = new System.Drawing.Size(122, 34);
            this.buttonRebootComputer.TabIndex = 4;
            this.buttonRebootComputer.Text = "重启";
            this.buttonRebootComputer.UseVisualStyleBackColor = true;
            this.buttonRebootComputer.Click += new System.EventHandler(this.buttonRebootComputer_Click);
            // 
            // buttonCloseCD
            // 
            this.buttonCloseCD.Location = new System.Drawing.Point(160, 100);
            this.buttonCloseCD.Name = "buttonCloseCD";
            this.buttonCloseCD.Size = new System.Drawing.Size(122, 34);
            this.buttonCloseCD.TabIndex = 4;
            this.buttonCloseCD.Text = "关闭光驱";
            this.buttonCloseCD.UseVisualStyleBackColor = true;
            this.buttonCloseCD.Click += new System.EventHandler(this.buttonCloseCD_Click);
            // 
            // buttonOpenCD
            // 
            this.buttonOpenCD.Location = new System.Drawing.Point(160, 60);
            this.buttonOpenCD.Name = "buttonOpenCD";
            this.buttonOpenCD.Size = new System.Drawing.Size(122, 34);
            this.buttonOpenCD.TabIndex = 4;
            this.buttonOpenCD.Text = "打开光驱";
            this.buttonOpenCD.UseVisualStyleBackColor = true;
            this.buttonOpenCD.Click += new System.EventHandler(this.buttonOpenCD_Click);
            // 
            // buttonSleepComputer
            // 
            this.buttonSleepComputer.Location = new System.Drawing.Point(160, 20);
            this.buttonSleepComputer.Name = "buttonSleepComputer";
            this.buttonSleepComputer.Size = new System.Drawing.Size(122, 34);
            this.buttonSleepComputer.TabIndex = 4;
            this.buttonSleepComputer.Text = "睡眠";
            this.toolTip1.SetToolTip(this.buttonSleepComputer, "开机快，从内存恢复");
            this.buttonSleepComputer.UseVisualStyleBackColor = true;
            this.buttonSleepComputer.Click += new System.EventHandler(this.buttonSleepComputer_Click);
            // 
            // buttonExeCode
            // 
            this.buttonExeCode.Location = new System.Drawing.Point(160, 140);
            this.buttonExeCode.Name = "buttonExeCode";
            this.buttonExeCode.Size = new System.Drawing.Size(122, 34);
            this.buttonExeCode.TabIndex = 4;
            this.buttonExeCode.Text = "更新客户端";
            this.buttonExeCode.UseVisualStyleBackColor = true;
            this.buttonExeCode.Click += new System.EventHandler(this.buttonExeCode_Click);
            // 
            // buttonHibernateComputer
            // 
            this.buttonHibernateComputer.Location = new System.Drawing.Point(15, 140);
            this.buttonHibernateComputer.Name = "buttonHibernateComputer";
            this.buttonHibernateComputer.Size = new System.Drawing.Size(122, 34);
            this.buttonHibernateComputer.TabIndex = 4;
            this.buttonHibernateComputer.Text = "休眠";
            this.toolTip1.SetToolTip(this.buttonHibernateComputer, "开机快，从文件恢复");
            this.buttonHibernateComputer.UseVisualStyleBackColor = true;
            this.buttonHibernateComputer.Click += new System.EventHandler(this.buttonHibernateComputer_Click);
            // 
            // buttonLockComputer
            // 
            this.buttonLockComputer.Location = new System.Drawing.Point(15, 100);
            this.buttonLockComputer.Name = "buttonLockComputer";
            this.buttonLockComputer.Size = new System.Drawing.Size(122, 34);
            this.buttonLockComputer.TabIndex = 4;
            this.buttonLockComputer.Text = "锁定";
            this.buttonLockComputer.UseVisualStyleBackColor = true;
            this.buttonLockComputer.Click += new System.EventHandler(this.buttonLockComputer_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonSelectSendCmdMode);
            this.panel1.Controls.Add(this.textBoxCommandResponse);
            this.panel1.Controls.Add(this.buttonSendCommand);
            this.panel1.Controls.Add(this.textBoxCommandRequest);
            this.panel1.Location = new System.Drawing.Point(8, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(650, 402);
            this.panel1.TabIndex = 3;
            // 
            // buttonSelectSendCmdMode
            // 
            this.buttonSelectSendCmdMode.Location = new System.Drawing.Point(367, 361);
            this.buttonSelectSendCmdMode.Name = "buttonSelectSendCmdMode";
            this.buttonSelectSendCmdMode.Size = new System.Drawing.Size(16, 34);
            this.buttonSelectSendCmdMode.TabIndex = 3;
            this.buttonSelectSendCmdMode.Text = "↓";
            this.buttonSelectSendCmdMode.UseVisualStyleBackColor = true;
            this.buttonSelectSendCmdMode.Click += new System.EventHandler(this.buttonSelectSendCmdMode_Click);
            // 
            // textBoxCommandResponse
            // 
            this.textBoxCommandResponse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCommandResponse.BackColor = System.Drawing.Color.Black;
            this.textBoxCommandResponse.ForeColor = System.Drawing.Color.White;
            this.textBoxCommandResponse.Location = new System.Drawing.Point(3, 3);
            this.textBoxCommandResponse.Multiline = true;
            this.textBoxCommandResponse.Name = "textBoxCommandResponse";
            this.textBoxCommandResponse.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxCommandResponse.Size = new System.Drawing.Size(642, 279);
            this.textBoxCommandResponse.TabIndex = 0;
            // 
            // buttonSendCommand
            // 
            this.buttonSendCommand.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonSendCommand.Location = new System.Drawing.Point(267, 361);
            this.buttonSendCommand.Name = "buttonSendCommand";
            this.buttonSendCommand.Size = new System.Drawing.Size(101, 34);
            this.buttonSendCommand.TabIndex = 2;
            this.buttonSendCommand.Text = "发送";
            this.buttonSendCommand.UseVisualStyleBackColor = true;
            this.buttonSendCommand.Click += new System.EventHandler(this.buttonSendCommand_Click);
            // 
            // textBoxCommandRequest
            // 
            this.textBoxCommandRequest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCommandRequest.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxCommandRequest.Location = new System.Drawing.Point(5, 288);
            this.textBoxCommandRequest.Multiline = true;
            this.textBoxCommandRequest.Name = "textBoxCommandRequest";
            this.textBoxCommandRequest.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxCommandRequest.Size = new System.Drawing.Size(640, 67);
            this.textBoxCommandRequest.TabIndex = 1;
            this.textBoxCommandRequest.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxCommandRequest_KeyUp);
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.tabPage3.Controls.Add(this.splitContainer3);
            this.tabPage3.Controls.Add(this.textBoxRegistryPath);
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(979, 411);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "注册表编辑器";
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.treeView2);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.listView2);
            this.splitContainer3.Size = new System.Drawing.Size(979, 390);
            this.splitContainer3.SplitterDistance = 326;
            this.splitContainer3.TabIndex = 0;
            // 
            // treeView2
            // 
            this.treeView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView2.ImageIndex = 0;
            this.treeView2.ImageList = this.imgListRegistryKey;
            this.treeView2.ItemHeight = 20;
            this.treeView2.Location = new System.Drawing.Point(0, 0);
            this.treeView2.Name = "treeView2";
            treeNode3.Name = "节点1";
            treeNode3.Tag = "ClassesRoot";
            treeNode3.Text = "HKEY_CLASSES_ROOT";
            treeNode4.Name = "节点3";
            treeNode4.Tag = "CurrentUser ";
            treeNode4.Text = "HKEY_CURRENT_USER";
            treeNode5.Name = "节点4";
            treeNode5.Tag = "LocalMachine ";
            treeNode5.Text = "HKEY_LOCAL_MACHINE";
            treeNode6.Name = "节点5";
            treeNode6.Tag = "Users ";
            treeNode6.Text = "HKEY_USERS";
            treeNode7.Name = "节点6";
            treeNode7.Tag = "CurrentConfig ";
            treeNode7.Text = "HKEY_CURRENT_CONFIG";
            treeNode8.ImageIndex = 1;
            treeNode8.Name = "节点0";
            treeNode8.SelectedImageIndex = 1;
            treeNode8.Text = "计算机";
            this.treeView2.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode8});
            this.treeView2.SelectedImageIndex = 0;
            this.treeView2.Size = new System.Drawing.Size(326, 390);
            this.treeView2.TabIndex = 0;
            this.treeView2.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeView2_MouseDoubleClick);
            this.treeView2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeView2_MouseUp);
            // 
            // imgListRegistryKey
            // 
            this.imgListRegistryKey.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgListRegistryKey.ImageStream")));
            this.imgListRegistryKey.TransparentColor = System.Drawing.Color.Transparent;
            this.imgListRegistryKey.Images.SetKeyName(0, "key.png");
            this.imgListRegistryKey.Images.SetKeyName(1, "computer.png");
            // 
            // listView2
            // 
            this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.listView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView2.LabelWrap = false;
            this.listView2.Location = new System.Drawing.Point(0, 0);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(649, 390);
            this.listView2.SmallImageList = this.imgListRegistryValue;
            this.listView2.TabIndex = 0;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            this.listView2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listView2_MouseUp);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "名称";
            this.columnHeader4.Width = 145;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "类型";
            this.columnHeader5.Width = 115;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "数据";
            this.columnHeader6.Width = 298;
            // 
            // imgListRegistryValue
            // 
            this.imgListRegistryValue.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgListRegistryValue.ImageStream")));
            this.imgListRegistryValue.TransparentColor = System.Drawing.Color.Transparent;
            this.imgListRegistryValue.Images.SetKeyName(0, "REG_BINARY");
            this.imgListRegistryValue.Images.SetKeyName(1, "REG_DWORD");
            this.imgListRegistryValue.Images.SetKeyName(2, "REG_EXPAND_SZ");
            this.imgListRegistryValue.Images.SetKeyName(3, "RED_MULTI_SZ");
            this.imgListRegistryValue.Images.SetKeyName(4, "RED_QWORD");
            this.imgListRegistryValue.Images.SetKeyName(5, "REG_SZ");
            this.imgListRegistryValue.Images.SetKeyName(6, "REG_NONE");
            // 
            // textBoxRegistryPath
            // 
            this.textBoxRegistryPath.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBoxRegistryPath.Location = new System.Drawing.Point(0, 390);
            this.textBoxRegistryPath.Name = "textBoxRegistryPath";
            this.textBoxRegistryPath.ReadOnly = true;
            this.textBoxRegistryPath.Size = new System.Drawing.Size(979, 21);
            this.textBoxRegistryPath.TabIndex = 1;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.listView3);
            this.tabPage4.Location = new System.Drawing.Point(4, 29);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(979, 411);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "进程管理器";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // listView3
            // 
            this.listView3.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11,
            this.columnHeader12,
            this.columnHeader13,
            this.columnHeader14,
            this.columnHeader15,
            this.columnHeader16});
            this.listView3.ContextMenuStrip = this.contextMenuStrip2;
            this.listView3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView3.FullRowSelect = true;
            this.listView3.GridLines = true;
            this.listView3.Location = new System.Drawing.Point(3, 3);
            this.listView3.Name = "listView3";
            this.listView3.Size = new System.Drawing.Size(973, 405);
            this.listView3.TabIndex = 0;
            this.listView3.UseCompatibleStateImageBehavior = false;
            this.listView3.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "映像名称";
            this.columnHeader8.Width = 110;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "PID";
            this.columnHeader9.Width = 71;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "用户名";
            this.columnHeader10.Width = 84;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "CPU";
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "内存（专用工作集）";
            this.columnHeader12.Width = 145;
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "线程数";
            // 
            // columnHeader14
            // 
            this.columnHeader14.Text = "映像路径";
            this.columnHeader14.Width = 224;
            // 
            // columnHeader15
            // 
            this.columnHeader15.Text = "描述";
            this.columnHeader15.Width = 219;
            // 
            // columnHeader16
            // 
            this.columnHeader16.Text = "命令行";
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.刷新急速ToolStripMenuItem,
            this.刷新ToolStripMenuItem1,
            this.结束进程ToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(133, 70);
            // 
            // 刷新急速ToolStripMenuItem
            // 
            this.刷新急速ToolStripMenuItem.Name = "刷新急速ToolStripMenuItem";
            this.刷新急速ToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.刷新急速ToolStripMenuItem.Text = "刷新(急速)";
            this.刷新急速ToolStripMenuItem.Click += new System.EventHandler(this.刷新急速ToolStripMenuItem_Click);
            // 
            // 刷新ToolStripMenuItem1
            // 
            this.刷新ToolStripMenuItem1.Name = "刷新ToolStripMenuItem1";
            this.刷新ToolStripMenuItem1.Size = new System.Drawing.Size(132, 22);
            this.刷新ToolStripMenuItem1.Text = "刷新(详细)";
            this.刷新ToolStripMenuItem1.Click += new System.EventHandler(this.刷新ToolStripMenuItem1_Click);
            // 
            // 结束进程ToolStripMenuItem
            // 
            this.结束进程ToolStripMenuItem.Name = "结束进程ToolStripMenuItem";
            this.结束进程ToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.结束进程ToolStripMenuItem.Text = "结束进程";
            this.结束进程ToolStripMenuItem.Click += new System.EventHandler(this.结束进程ToolStripMenuItem_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(987, 82);
            this.richTextBox1.TabIndex = 5;
            this.richTextBox1.Text = "";
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 628);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(987, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(87, 17);
            this.toolStripStatusLabel1.Text = "自动上线：0台";
            // 
            // toolStrip2
            // 
            this.toolStrip2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripTextBox1,
            this.toolStripLabel2,
            this.toolStripLabel3,
            this.toolStripTextBox2});
            this.toolStrip2.Location = new System.Drawing.Point(0, 73);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(987, 25);
            this.toolStrip2.TabIndex = 2;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(68, 22);
            this.toolStripLabel1.Text = "当前连接：";
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.BackColor = System.Drawing.Color.Lavender;
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(200, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(32, 22);
            this.toolStripLabel2.Text = "      ";
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(68, 22);
            this.toolStripLabel3.Text = "电脑名称：";
            // 
            // toolStripTextBox2
            // 
            this.toolStripTextBox2.BackColor = System.Drawing.Color.Lavender;
            this.toolStripTextBox2.Name = "toolStripTextBox2";
            this.toolStripTextBox2.Size = new System.Drawing.Size(200, 25);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton4,
            this.toolStripButton3,
            this.toolStripButtonCaptureVideo,
            this.toolStripButtonSettings});
            this.toolStrip1.Location = new System.Drawing.Point(0, 25);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(987, 48);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(60, 45);
            this.toolStripButton4.Text = "自动上线";
            this.toolStripButton4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(60, 45);
            this.toolStripButton3.Text = "抓取屏幕";
            this.toolStripButton3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripButtonCaptureVideo
            // 
            this.toolStripButtonCaptureVideo.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonCaptureVideo.Image")));
            this.toolStripButtonCaptureVideo.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonCaptureVideo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCaptureVideo.Name = "toolStripButtonCaptureVideo";
            this.toolStripButtonCaptureVideo.Size = new System.Drawing.Size(60, 45);
            this.toolStripButtonCaptureVideo.Text = "视频语音";
            this.toolStripButtonCaptureVideo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonCaptureVideo.Click += new System.EventHandler(this.toolStripButtonCaptureVideo_Click);
            // 
            // toolStripButtonSettings
            // 
            this.toolStripButtonSettings.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSettings.Image")));
            this.toolStripButtonSettings.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSettings.Name = "toolStripButtonSettings";
            this.toolStripButtonSettings.Size = new System.Drawing.Size(84, 45);
            this.toolStripButtonSettings.Text = "配置服务程序";
            this.toolStripButtonSettings.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonSettings.Click += new System.EventHandler(this.toolStripButtonSettings_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件FToolStripMenuItem,
            this.ToolStripMenuItemTools,
            this.ToolStripMenuItemUsualFolders,
            this.ToolStripMenuItemSkins,
            this.帮助HToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(987, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件FToolStripMenuItem
            // 
            this.文件FToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.退出XToolStripMenuItem});
            this.文件FToolStripMenuItem.Name = "文件FToolStripMenuItem";
            this.文件FToolStripMenuItem.Size = new System.Drawing.Size(58, 21);
            this.文件FToolStripMenuItem.Text = "文件(&F)";
            // 
            // 退出XToolStripMenuItem
            // 
            this.退出XToolStripMenuItem.Name = "退出XToolStripMenuItem";
            this.退出XToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.退出XToolStripMenuItem.Text = "退出(&X)";
            this.退出XToolStripMenuItem.Click += new System.EventHandler(this.退出XToolStripMenuItem_Click);
            // 
            // ToolStripMenuItemTools
            // 
            this.ToolStripMenuItemTools.Name = "ToolStripMenuItemTools";
            this.ToolStripMenuItemTools.Size = new System.Drawing.Size(59, 21);
            this.ToolStripMenuItemTools.Text = "工具(&T)";
            // 
            // ToolStripMenuItemUsualFolders
            // 
            this.ToolStripMenuItemUsualFolders.Name = "ToolStripMenuItemUsualFolders";
            this.ToolStripMenuItemUsualFolders.Size = new System.Drawing.Size(82, 21);
            this.ToolStripMenuItemUsualFolders.Text = "常用目录(&F)";
            // 
            // ToolStripMenuItemSkins
            // 
            this.ToolStripMenuItemSkins.Name = "ToolStripMenuItemSkins";
            this.ToolStripMenuItemSkins.Size = new System.Drawing.Size(59, 21);
            this.ToolStripMenuItemSkins.Text = "皮肤(&S)";
            // 
            // 帮助HToolStripMenuItem
            // 
            this.帮助HToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.关于ToolStripMenuItem});
            this.帮助HToolStripMenuItem.Name = "帮助HToolStripMenuItem";
            this.帮助HToolStripMenuItem.Size = new System.Drawing.Size(61, 21);
            this.帮助HToolStripMenuItem.Text = "帮助(&H)";
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.关于ToolStripMenuItem.Text = "关于";
            this.关于ToolStripMenuItem.Click += new System.EventHandler(this.关于ToolStripMenuItem_Click);
            // 
            // skinEngine1
            // 
            this.skinEngine1.SerialNumber = "";
            this.skinEngine1.SkinFile = null;
            this.skinEngine1.SkinStreamMain = ((System.IO.Stream)(resources.GetObject("skinEngine1.SkinStreamMain")));
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(987, 650);
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmMain";
            this.Text = "远程控制服务端";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMain_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Sunisoft.IrisSkin.SkinEngine skinEngine1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件FToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助HToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButtonCaptureVideo;
        private System.Windows.Forms.ToolStripButton toolStripButtonSettings;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ToolStrip toolStrip3;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton7;
        private System.Windows.Forms.ToolStripButton toolStripButton8;
        private System.Windows.Forms.ToolStripButton toolStripButton9;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton10;
        private System.Windows.Forms.ToolStripButton toolStripButton11;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButton13;
        private System.Windows.Forms.ToolStripButton toolStripButton14;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.ToolStripButton toolStripButton12;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TreeView treeView2;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.Button buttonSendCommand;
        private System.Windows.Forms.TextBox textBoxCommandRequest;
        private System.Windows.Forms.TextBox textBoxCommandResponse;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonHibernateComputer;
        private System.Windows.Forms.Button buttonLockComputer;
        private System.Windows.Forms.Button buttonRebootComputer;
        private System.Windows.Forms.Button buttonShutdownComputer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonOpenUrl;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonSendMessage;
        private System.Windows.Forms.Button buttonLockMouse;
        private System.Windows.Forms.Button buttonUnBlackScreen;
        private System.Windows.Forms.Button buttonRemoteDownloadWebUrl;
        private System.Windows.Forms.Button buttonPlayMusic;
        private System.Windows.Forms.Button buttonStopPlayMusic;
        private System.Windows.Forms.Button buttonSleepComputer;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button buttonSelectSendCmdMode;
        private System.Windows.Forms.Button buttonBlackScreen;
        private System.Windows.Forms.Button buttonUnLockMouse;
        private System.Windows.Forms.Button buttonCloseCD;
        private System.Windows.Forms.Button buttonOpenCD;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 复制全路径ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.ListView listView3;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.ColumnHeader columnHeader14;
        private System.Windows.Forms.ColumnHeader columnHeader15;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem1;
        private System.Windows.Forms.ColumnHeader columnHeader16;
        private System.Windows.Forms.ToolStripMenuItem 结束进程ToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.Button buttonExeCode;
        private System.Windows.Forms.ToolStripMenuItem 打开文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开文件隐藏模式ToolStripMenuItem;
        private System.Windows.Forms.ImageList imgListRegistryKey;
        private System.Windows.Forms.ImageList imgListRegistryValue;
        private System.Windows.Forms.ToolStripMenuItem 退出XToolStripMenuItem;
        private System.Windows.Forms.TextBox textBoxRegistryPath;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSkins;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemTools;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemUsualFolders;
        private System.Windows.Forms.ToolStripMenuItem 刷新急速ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 播放印业ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 停止播放音乐ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 远程下载到此处ToolStripMenuItem;
        private SortableListView listView1;
    }
}

