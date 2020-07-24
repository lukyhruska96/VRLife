using System;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Common.Core.Applications
{
    public enum SenderType
    {
        USER, SERVER
    }

    public struct MsgContext
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

    public interface IApplicationServer : IApplication
    {
        byte[] HandleMessage(byte[] data, int size, MsgContext ctx);

        byte[] HandleEvent(EventDataMsg eventData, MsgContext ctx);
    }
}
