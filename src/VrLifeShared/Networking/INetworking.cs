using Google.Protobuf;
using System;
using System.Net;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeShared.Networking
{
    public interface INetworking<T>
    {
        void Send(T req, IPEndPoint address, Action<T> callback, Action<Exception> err);
        void StartListening();
    }
}
