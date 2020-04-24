using Google.Protobuf;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VrLifeServer.Networking.NetworkingModels;

namespace VrLifeServer.Networking
{
    public class TCPNetworking<T> : INetworking<T> where T : IMessage<T>
    {

        public void Send(T req, IPEndPoint address, Action<T> callback, Action<Exception> err)
        {
            throw new NotImplementedException();
        }

        public void StartListening()
        {
            throw new NotImplementedException();
        }
    }
}
