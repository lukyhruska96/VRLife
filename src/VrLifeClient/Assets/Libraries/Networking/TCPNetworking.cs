using Google.Protobuf;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeShared.Networking
{
    public class TCPNetworking<T> : INetworking<T> where T : IMessage<T>
    {

        public void SendAsync(T req, IPEndPoint address, Action<T> callback, Action<Exception> err)
        {
            throw new NotImplementedException();
        }

        public T Send(T req, IPEndPoint address)
        {
            throw new NotImplementedException();
        }

        public void StartListening()
        {
            throw new NotImplementedException();
        }
    }
}
