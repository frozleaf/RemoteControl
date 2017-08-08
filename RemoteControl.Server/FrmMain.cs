using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using RemoteControl.Protocals;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace RemoteControl.Server
{
    public partial class FrmMain : FrmBase
    {
        public const string APP_TITLE = "远程控制服务端";
        private int clientCount = 0;
        private TreeNode InternetTreeNode { get { return this.treeView1.Nodes[1]; } }
        private SocketSession currentSession = null;
        private Dictionary<string, Action<ResponseStartGetScreen>> sessionScreenHandlers = new Dictionary<string, Action<ResponseStartGetScreen>>();
        private Dictionary<string, Action<ResponseStartCaptureVideo>> sessionVideoHandlers = new Dictionary<string, Action<ResponseStartCaptureVideo>>();
        private SendCommandHotKey sendCommandHotKey = SendCommandHotKey.Enter; 

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
        }

        private void initSkinMenus()
        {
            RSCApplication.lstSkins = RSCApplication.GetAllSkinFiles();
            if (RSCApplication.lstSkins.Count > 0)
            {
                ToolStripMenuItem menuSkins = new ToolStripMenuItem("皮肤(&S)");
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
                    menuSkins.DropDownItems.Add(menuSkin);
                }
                this.menuStrip1.Items.Insert(this.menuStrip1.Items.Count - 1, menuSkins);
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
                                this.imageList1.Images.Add(extension, Utils.GetIcon(extension, true));
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
            this.clientCount--;
            refreshClientCountShow();
            doOutput(oClient.SocketId.ToString() + " 下线了！");
        }

        private void actChangeSkin(string sSkinFile)
        {
            this.skinEngine1.SkinFile = sSkinFile;
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
            SocketSession oSession = e.Node.Tag as SocketSession;
            if (oSession != null)
            {
                this.toolStripTextBox1.Text = oSession.SocketId;
            }
            else
            {
                this.toolStripTextBox1.Text = string.Empty;
            }
        }

        private void treeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TreeViewHitTestInfo hitTestInfo = this.treeView1.HitTest(e.Location);
            if (hitTestInfo != null && hitTestInfo.Node != null)
            {
                SocketSession client = hitTestInfo.Node.Tag as SocketSession;
                this.currentSession = client;
                if (client != null)
                {
                    byte[] packet = PacketFactory.EncodeOject(ePacketType.PACKET_GET_DRIVES_REQUEST, null);
                    client.SocketObj.Send(packet);
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

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            string localFilePath = ofd.FileName;
            string remoteFileDir = this.listView1.Tag as string;
            if (remoteFileDir == null)
                return;
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
                }
                fs.Close();
                fs.Dispose();
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

                var sfd = new SaveFileDialog();
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

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listView1.Tag == null)
                return;

            RequestGetSubFilesOrDirs req = new RequestGetSubFilesOrDirs();
            req.parentDir = this.listView1.Tag.ToString();

            PostRequstWithCurrentSession(ePacketType.PACKET_GET_SUBFILES_OR_DIRS_REQUEST, req);
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
            frm.ShowDialog();
            frm.Dispose();
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
    }
}
