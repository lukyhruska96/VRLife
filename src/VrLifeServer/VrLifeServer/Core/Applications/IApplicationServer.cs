using System;
using VrLifeServer.API;
using VrLifeServer.Core.Services.UserService;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeServer.Applications
{
    enum SenderType
    {
        USER, SERVER
    }

    struct MsgContext
    {
        public ulong msgId;
        public SenderType senderType;
        public ulong senderId;

        public MsgContext(MainMessage msg)
        {
            msgId = msg.MsgId;
            senderType = msg.SenderIdCase == MainMessage.SenderIdOneofCase.ClientId ? SenderType.USER: SenderType.SERVER;
            senderId = senderType == SenderType.USER ? msg.ClientId : msg.ServerId;
        }
    }

    interface IApplicationServer : IApplication
    {
        byte[] HandleMessage(byte[] data, int size, MsgContext ctx);

        byte[] HandleEvent(EventDataMsg eventData, MsgContext ctx);
    }
}
