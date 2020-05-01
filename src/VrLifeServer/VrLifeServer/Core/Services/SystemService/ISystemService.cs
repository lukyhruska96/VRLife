using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.Core.Utils;
using VrLifeServer.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.SystemService
{
    interface ISystemService : IService
    {

        public static MainMessage CreateHelloMessage()
        {
            MainMessage mainMsg = new MainMessage();
            SystemMsg msg = new SystemMsg();
            HiMsg hiMsg = new HiMsg();
            hiMsg.Memory = HwMonitor.GetTotalMemory();
            hiMsg.Threads = HwMonitor.GetCoreCount();
            hiMsg.Version = VrLifeServer.VERSION;
            msg.HiMsg = hiMsg;
            mainMsg.SystemMsg = msg;
            return mainMsg;
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

        public static MainMessage CreateOkMessage(uint msgId = 0)
        {
            OkMsg okMsg = new OkMsg();
            okMsg.MsgId = msgId;
            SystemMsg sysMsg = new SystemMsg();
            sysMsg.OkMsg = okMsg;
            MainMessage msg = new MainMessage();
            msg.SystemMsg = sysMsg;
            return msg;
        }
    }
}
