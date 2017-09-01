using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AForge.Video.DirectShow;
using AForge.Video;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace RemoteControl.Client.CamCapture
{
    public partial class FrmMain : Form
    {
        #region 私有字段

        private System.Collections.Concurrent.ConcurrentDictionary<string, Socket> _clients = new System.Collections.Concurrent.ConcurrentDictionary<string, Socket>();
        private Socket _broadcastServer = null;
        private string _broadcastServerIP = "127.0.0.1";
        private int _broadcastServerPort = 9001;
        private bool _isHide = true;
        private int _fps = 1;
        private int _intervalMilliSec = 1000;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="isHide">是否隐藏窗体</param>
        public FrmMain(bool isHide, int fps)
        {
            InitializeComponent();
            _isHide = isHide;
            _fps = fps;
            _intervalMilliSec = 1000 / fps;
        } 

        #endregion

        #region 窗体载入

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_Load(object sender, EventArgs e)
        {
            if (_isHide)
            {
                this.ShowInTaskbar = false;
                this.Hide();
            }
            StartCamCapture();
            new Thread(() => StartTransportServerInternal()) { IsBackground = true }.Start();
            new Thread(() => StartBroadcastInternal()) { IsBackground = true }.Start();
        } 

        #endregion

        #region 窗体关闭

        /// <summary>
        /// 窗体关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        } 

        #endregion

        #region 摄像头操作

        private bool StartCamCapture()
        {
            try
            {
                var collection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                if (collection.Count < 1)
                    return false;

                var videoSource = new VideoCaptureDevice(collection[0].MonikerString);
                //videoSource.DesiredFrameRate = 1;
                //videoSource.NewFrame += new NewFrameEventHandler(videoSource_NewFrame);
                this.videoSourcePlayer1.VideoSource = videoSource;
                this.videoSourcePlayer1.Start();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Broadcast(eventArgs.Frame);
        }

        private void StopCamCapture()
        {
            this.videoSourcePlayer1.Stop();
        }
 
        #endregion

        #region 数据广播

        private void StartBroadcastInternal()
        {
            Bitmap bmp = null;
            while (true)
            {
                bmp = (Bitmap)this.Invoke(new Func<Bitmap>(() =>
                {
                    return this.videoSourcePlayer1.GetCurrentVideoFrame();
                }));
                if (bmp != null)
                {
                    Broadcast(bmp);
                    bmp.Dispose();
                }
                Thread.Sleep(_intervalMilliSec);
            }
        }              
                
        private void StartTransportServerInternal()
        {
            if (_broadcastServer != null)
                return;
            try
            {
                _broadcastServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _broadcastServer.Bind(new IPEndPoint(IPAddress.Parse(_broadcastServerIP), _broadcastServerPort));
                _broadcastServer.Listen(10);
                new Thread(()=>{
                    Socket c = null;
                    string id = null;
                    while (true)
                    {
                        try
                        {
                            c = _broadcastServer.Accept();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Environment.Exit(0);
                        }
                        id = c.RemoteEndPoint.ToString();
                        if (!_clients.ContainsKey(id))
                        {
                            while (!_clients.TryAdd(id, c)) ;
                        }
                    }
                }) { IsBackground=true}.Start();
            }
            catch (Exception ex)
            {
                Environment.Exit(0);
            }
        }

        private void StopTransportServer()
        {
            try
            {
                if (_broadcastServer != null)
                {
                    _broadcastServer.Close();
                    _broadcastServer = null;
                }
            }
            catch (Exception ex)
            {
            }
        } 

        private void Broadcast(Bitmap bmp)
        {
            if (_broadcastServer != null)
            {
                byte[] data = null;
                DateTime captureTime = DateTime.Now;
                try
                {
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                    {
                        // 改为jpeg后，小很多
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        data = ms.GetBuffer();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("生成图片字节数组失败，" + ex.Message);
                    return;
                }
                foreach (var pair in _clients)
                {
                    try
                    {
                        pair.Value.Send(Encode(captureTime, data));
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("您的主机中的软件中止了一个已建立的连接"))
                        {
                            Socket s = null;
                            _clients.TryRemove(pair.Key, out s);
                        }
                        Console.WriteLine("广播到" + pair.Key + "失败," + ex.Message);
                    }
                }
                data = null;
                Output(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + " 已广播");
            }
        }

        private byte[] Encode(DateTime captureTime, byte[] captureData)
        {
            List<byte> data = new List<byte>();
            data.AddRange(BitConverter.GetBytes(8+captureData.Length));
            data.AddRange(BitConverter.GetBytes(captureTime.Ticks));
            data.AddRange(captureData);

            //System.IO.File.WriteAllBytes(@"d:\1.jpg", captureData);

            return data.ToArray();
        }

        #endregion

        #region 状态输出函数

        /// <summary>
        /// 状态输出函数
        /// </summary>
        /// <param name="str"></param>
        private void Output(string str)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(Output), str);
                return;
            }
            this.toolStripStatusLabel3.Text = str;
        } 

        #endregion
    }
}
