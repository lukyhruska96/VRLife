﻿using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using VrLifeServer.Core.Utils;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.SystemService
{
    interface ISystemService : IService
    {

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
            redirectMsg.ReceivedMsg =  ByteString.CopyFrom(recvMsg.ToByteArray());
            MainMessage msg = new MainMessage();
            msg.SystemMsg = new SystemMsg();
            msg.SystemMsg.RedirectMsg = redirectMsg;
            return msg;
        }

        public static MainMessage CreateRedirectMessage(MainMessage recvMsg, IPEndPoint ip)
        {
            return ISystemService.CreateRedirectMessage(recvMsg, (int)ip.Address.ToInt(), ip.Port);
        }

        public static MainMessage CreateHelloMessage(Config conf)
        {
            MainMessage mainMsg = new MainMessage();
            SystemMsg msg = new SystemMsg();
            HiMsg hiMsg = new HiMsg();
            hiMsg.Address = conf.ServerAddress.ToInt();
            hiMsg.Port = (int)conf.UdpPort;
            hiMsg.Memory = HwMonitor.GetTotalMemory();
            hiMsg.Threads = HwMonitor.GetCoreCount();
            hiMsg.Version = VrLifeServer.VERSION;
            msg.HiMsg = hiMsg;
            mainMsg.SystemMsg = msg;
            return mainMsg;
        }

        public static bool IsError(MainMessage msg)
        {
            return msg.MessageTypeCase == MainMessage.MessageTypeOneofCase.SystemMsg &&
                msg.SystemMsg.SystemMsgTypeCase == SystemMsg.SystemMsgTypeOneofCase.ErrorMsg;
        }
        public MainMessage CreateHelloMessage();

        public IPEndPoint GetAddressById(uint serverId);
    }
}
