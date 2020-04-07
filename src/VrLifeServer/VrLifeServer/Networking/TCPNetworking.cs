using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VrLifeServer.NetworkModels;

namespace VrLifeServer.Networking
{
    public class TCPNetworking : INetworking
    {
        private IPAddress ipAddress;
        private int port;
        private Socket listener;
        private Dictionary<int, Func<Message, Message>> handlers =
            new Dictionary<int, Func<Message, Message>>();
        private ConcurrentQueue<Message> queue = new ConcurrentQueue<Message>();
        private Semaphore semaphore = new Semaphore(0, MAX_SEMAPHORE);
        private Task listenningTask;
        private Task sendingTask;
        private const int BACKLOG = 100;
        private const int MAX_SEMAPHORE = 100;

        public TCPNetworking(IPAddress ipAddress, int port)
        {
            this.ipAddress = ipAddress;
            this.port = port;
            this.listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);
        }

        public void RegisterHandler(int appId, Func<Message, Message> handler)
        {
            lock(handlers)
            {
                handlers[appId] = handler;
            }
        }

        public void Send(Message req)
        {
            queue.Enqueue(req);
        }

        public void Start()
        {
            listenningTask = new Task(() =>
            {
                byte[] buff = new byte[1024];
                this.listener.Bind(new IPEndPoint(this.ipAddress, this.port));
                this.listener.Listen(BACKLOG);
                while (true)
                {
                    Socket handler = listener.Accept();
 
                    while (true)
                    {
                        int bytesRec = handler.Receive(buff);
                        data += Encoding.ASCII.GetString(buff, 0, bytesRec);
                        if (data.IndexOf("<EOF>") > -1)
                        {
                            break;
                        }
                    }

                    byte[] msg = Encoding.ASCII.GetBytes(data);

                    handler.Send(msg);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }, TaskCreationOptions.LongRunning);
            listenningTask.Start();
            sendingTask = new Task(() =>
            {
                while(true)
                {
                    NetworkStream ns = null;
                    TcpClient client = null;
                    try
                    {
                        semaphore.WaitOne();
                        Message msg;
                        while (!queue.TryDequeue(out msg))
                        {
                            Thread.Sleep(1);
                        }
                        client = new TcpClient(msg.Target);
                        ns = client.GetStream();
                        ns.Write(msg.Decode());
                    }
                    finally
                    {
                        if(ns != null)
                        {
                            ns.Close();
                        }
                        if(client != null)
                        {
                            client.Close();
                        }
                        semaphore.Release();
                    }
                }
            }, TaskCreationOptions.LongRunning);
            sendingTask.Start();
        }

        private static Message HandleRequest()
        {

        }
    }
}
