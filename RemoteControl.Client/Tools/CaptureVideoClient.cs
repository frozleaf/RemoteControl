using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using RemoteControl.Protocals;

namespace RemoteControl.Client
{
    class CaptureVideoClient
    {
        public static event EventHandler MessagerReceived;

        private static Socket socket;

        public static void Connect(string ip, int port)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
            new Thread(() =>
                {
                    byte[] buffer = new byte[1024];
                    List<byte> data = new List<byte>();
                    while (true)
                    {
                        try
                        {
                            int size = socket.Receive(buffer);
                            if (size < 1)
                                continue;
                            for (int i = 0; i < size; i++)
                            {
                                data.Add(buffer[i]);
                            }

                            while (data.Count >= 4)
                            {
                                int len = BitConverter.ToInt32(data.ToArray(), 0) + 4;
                                if (data.Count >= len)
                                {
                                    DoMessagerReceived(data.SplitBytes(4, len-4).ToList());
                                    data.RemoveRange(0, len);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            return;
                        }
                    }
                }) { IsBackground = true }.Start();
        }

        public static void Close()
        {
            if (socket != null)
            {
                try
                {
                    socket.Close();
                }
                catch (Exception ex)
                {
                }
            }
        }

        private static void DoMessagerReceived(List<byte> data)
        {
            if (MessagerReceived != null)
            {
                MessagerReceived(data, new EventArgs());
            }
        }
    }
}
