using System;
using System.Net;
using System.Net.Sockets;
using VrLifeServer.NetworkModels;

namespace VrLifeServer.Networking
{
    public class UDPNetworking : INetworking
    {
        private IPAddress ipAddress;
        private int port;
        private Socket socket;

        public UDPNetworking(IPAddress ipAddress, int port)
        {
            this.ipAddress = ipAddress;
            this.port = port;
            this.socket = new Socket(ipAddress.AddressFamily,
                SocketType.Dgram, ProtocolType.Udp);
        }

        public void RegisterHandler(int appId, Func<Message, Message> handler)
        {
            throw new NotImplementedException();
        }

        public void Send(Message req)
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }
    }
}
