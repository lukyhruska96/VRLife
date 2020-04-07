using System;
using VrLifeServer.NetworkModels;

namespace VrLifeServer.Networking
{
    public interface INetworking
    {
        void Send(Message req);
        void RegisterHandler(int appId, Func<Message, Message> handler);
        void Start();
    }
}
