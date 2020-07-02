using System;
using VrLifeServer.API;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeServer.Applications
{
    enum SenderType
    {
        USER, SERVER
    }

    struct MsgContext
    {
        long msgId;
        SenderType senderType;
        int senderId;
    }

    interface IApplicationServer
    {
        AppMsg HandleMessage(byte[] data, int size, MsgContext context);
    }
}
