using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using VrLifeAPI.Common.Core.Utils;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Common.Core.Services
{
    public static class ServiceUtils
    {
        public static EventResponse CreateErrorResponse(ulong msgId, uint errType, uint errCode, string errMsg = null)
        {
            EventResponse msg = new EventResponse();
            msg.Error = new ErrorMsg();
            msg.Error.MsgId = msgId;
            msg.Error.ErrorType = errType;
            msg.Error.ErrorCode = errCode;
            msg.Error.ErrorMsg_ = errMsg;
            return msg;
        }

        public static MainMessage CreateErrorMessage(ulong msgId, uint errType, uint errCode, string errMsg = null)
        {
            ErrorMsg errorMsg = new ErrorMsg();
            errorMsg.MsgId = msgId;
            errorMsg.ErrorType = errType;
            errorMsg.ErrorCode = errCode;
            errorMsg.ErrorMsg_ = errMsg;
            SystemMsg sysMsg = new SystemMsg();
            sysMsg.ErrorMsg = errorMsg;
            MainMessage msg = new MainMessage();
            msg.SystemMsg = sysMsg;
            return msg;

        }

        public static MainMessage CreateOkMessage(ulong msgId = 0)
        {
            OkMsg okMsg = new OkMsg();
            okMsg.MsgId = msgId;
            SystemMsg sysMsg = new SystemMsg();
            sysMsg.OkMsg = okMsg;
            MainMessage msg = new MainMessage();
            msg.SystemMsg = sysMsg;
            return msg;
        }

        public static MainMessage CreateRedirectMessage(MainMessage recvMsg, int address, int port)
        {
            RedirectMsg redirectMsg = new RedirectMsg();
            redirectMsg.Address = address;
            redirectMsg.Port = port;
            redirectMsg.ReceivedMsg = ByteString.CopyFrom(recvMsg.ToByteArray());
            MainMessage msg = new MainMessage();
            msg.SystemMsg = new SystemMsg();
            msg.SystemMsg.RedirectMsg = redirectMsg;
            return msg;
        }

        public static MainMessage CreateRedirectMessage(MainMessage recvMsg, IPEndPoint ip)
        {
            return ServiceUtils.CreateRedirectMessage(recvMsg, (int)ip.Address.ToInt(), ip.Port);
        }

        public static bool IsError(MainMessage msg)
        {
            return msg.MessageTypeCase == MainMessage.MessageTypeOneofCase.SystemMsg &&
                msg.SystemMsg.SystemMsgTypeCase == SystemMsg.SystemMsgTypeOneofCase.ErrorMsg;
        }
    }
}
