using Google.Protobuf;
using System;
using System.Net;

namespace VrLifeAPI.Networking
{
    public interface INetworking<T>
    {
        void SendAsync(T req, IPEndPoint address, Action<T> callback, Action<Exception> err = null);
        T Send(T req, IPEndPoint address);
        void StartListening();
    }
}
