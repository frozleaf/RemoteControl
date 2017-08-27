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

        private void StartServerAccept(Socket server)
        {
            Thread serverAcceptThread = new Thread(() =>
            {
                string sessionId = server.LocalEndPoint.ToString();
                Socket client = null;
                while (true)
                {
                    try
                    {
                        client = server.Accept();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Serer Accept Error," + ex.Message);
                        // 监测服务是否已经关闭
                        if(!_oServerDic.ContainsKey(sessionId))
                            return;

                        continue;
                    }
                    // 创建会话对象
                    SocketSession session = new SocketSession(client.RemoteEndPoint, client);
                    DoClientConnected(session);
                    StartClientRecv(session);
                }
            });
            serverAcceptThread.Name = serverAcceptThread + "->" + server.LocalEndPoint.ToString();
            serverAcceptThread.IsBackground = true;
            serverAcceptThread.Start();
        }

        private void DoClientConnected(SocketSession session)
        {
            _oClientDic.Add(session.SocketId, session.SocketObj);
            if (ClientConnected != null)
            {
                ClientConnectedEventArgs args = new ClientConnectedEventArgs(session);
                ClientConnected(this, args);
            } 
        }

        private void StartClientRecv(SocketSession session)
        {
            new Thread(() =>
            {
                byte[] buffer = new byte[1024];
                int recvSize = -1;
                List<byte> data = new List<byte>();
                while (true)
                {
                    try
                    {
                        recvSize = session.SocketObj.Receive(buffer);
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
                        Console.WriteLine(ex.Message);
                        DoClientDisConnected(session);
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

        private void DoClientDisConnected(SocketSession session)
        {
            lock (ClentDicLocker)
            {
                _oClientDic.Remove(session.SocketId); 
            }
            if (ClientDisconnected != null)
            {
                ClientConnectedEventArgs args = new ClientConnectedEventArgs(session);
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
