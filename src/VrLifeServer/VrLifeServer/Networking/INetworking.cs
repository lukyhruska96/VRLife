using Google.Protobuf;
using System;
using System.Net;
using VrLifeServer.Networking.NetworkingModels;

namespace VrLifeServer.Networking
{
    public interface INetworking<T>
    {
        void Send(T req, IPEndPoint address, Action<T> callback, Action<Exception> err);
        void StartListening();
    }
}
