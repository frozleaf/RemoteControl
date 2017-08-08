using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using RemoteControl.Protocals;

namespace RemoteControl.Server
{
    class RemoteControlServer
    {
        private Dictionary<string, Socket> _oServerDic = new Dictionary<string, Socket>();
        private Dictionary<string, Socket> _oClientDic = new Dictionary<string, Socket>();
        private object ServerDicLocker = new object();
        private object ClentDicLocker = new object();

        public event EventHandler<ClientConnectedEventArgs> ClientConnected;
        public event EventHandler<ClientConnectedEventArgs> ClientDisconnected;
        public event EventHandler<PacketReceivedEventArgs> PacketReceived;

        public RemoteControlServer()
        {

        }

        public void Start(List<string> lstIP, int iServerPort)
        {
            for (int i = 0; i < lstIP.Count; i++)
			{
                string sServerIP = lstIP[i];
                Socket oServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                oServer.Bind(new IPEndPoint(IPAddress.Parse(sServerIP), iServerPort));
                oServer.Listen(10);
                StartServerAccept(oServer);
                _oServerDic.Add(oServer.LocalEndPoint.ToString(), oServer);
			}
        }

        private void StartServerAccept(Socket oServer)
        {
            Thread serverAcceptThread = new Thread(() =>
            {
                string sessionId = oServer.LocalEndPoint.ToString();
                Socket oCient = null;
                while (true)
                {
                    try
                    {
                        oCient = oServer.Accept();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Serer Accept Error," + ex.Message);
                        // 监测服务是否已经关闭
                        if(!_oServerDic.ContainsKey(sessionId))
                            return;

                        continue;
                    }
                    DoClientConnected(oCient);
                    StartClientRecv(oCient);
                }
            });
            serverAcceptThread.Name = serverAcceptThread + "->" + oServer.LocalEndPoint.ToString();
            serverAcceptThread.IsBackground = true;
            serverAcceptThread.Start();
        }

        private void DoClientConnected(Socket oCient)
        {
            _oClientDic.Add(oCient.RemoteEndPoint.ToString(), oCient);
            if (ClientConnected != null)
            {
                ClientConnectedEventArgs args = new ClientConnectedEventArgs(new SocketSession(oCient.RemoteEndPoint, oCient));
                ClientConnected(this, args);
            } 
        }

        private void StartClientRecv(Socket oClient)
        {
            string sessionId = oClient.RemoteEndPoint.ToString();
            SocketSession session = new SocketSession(sessionId, oClient);
            new Thread(() =>
            {
                byte[] buffer = new byte[1024];
                int recvSize = -1;
                List<byte> data = new List<byte>();
                while (true)
                {
                    try
                    {
                        recvSize = oClient.Receive(buffer);
                        if (recvSize < 1)
                            continue;

                        for (int i = 0; i < recvSize; i++)
                        {
                            data.Add(buffer[i]);
                        }
                        while (data.Count >= 4)
                        {
                            int packetLength = BitConverter.ToInt32(data.ToArray(), 0);
                            if (data.Count < packetLength)
                            {
                                break;
                            }
                            DoRecvBytes(session, data.SplitBytes(0, packetLength));
                            data.RemoveRange(0, packetLength);
                        }
                    }
                    catch (Exception ex)
                    {
                        //MsgBox.ShowInfo(ex.Message+"\r\n"+ex.StackTrace);
                        Console.WriteLine(ex.Message);
                        DoClientDisConnected(sessionId, oClient);
                        break;
                    }
                }
            }) { IsBackground = true,Name="StartClientRecv" }.Start();
        }

        private void DoRecvBytes(SocketSession session, byte[] packet)
        {
            ePacketType packetType;
            object obj;
            PacketFactory.DecodeObject(packet, out packetType, out obj);
            if (PacketReceived != null)
            {
                PacketReceivedEventArgs args = new PacketReceivedEventArgs();
                args.PacketType = packetType;
                args.Obj = obj;
                args.Session = session;
                PacketReceived(this, args);
            }
        }

        private void DoClientDisConnected(string sessionId, Socket oClient)
        {
            lock (ClentDicLocker)
            {
                _oClientDic.Remove(sessionId); 
            }
            if (ClientDisconnected != null)
            {
                ClientConnectedEventArgs args = new ClientConnectedEventArgs(new SocketSession(sessionId, oClient));
                ClientDisconnected(this, args);
            } 
        }

        public void Stop()
        {
            foreach (var item in _oServerDic)
            {
                try
                {
                    item.Value.Close();
                }
                catch (Exception ex)
                {
                }
            }
            _oServerDic.Clear();
            foreach (var item in _oClientDic)
            {
                try
                {
                    item.Value.Close();
                }
                catch (Exception ex)
                {
                }
            }
            _oClientDic.Clear();
        }
    }
}
