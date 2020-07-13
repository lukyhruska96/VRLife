using Pomelo.EntityFrameworkCore.MySql.Migrations;
using System;
using System.Collections.Generic;
using System.Text;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.EventService
{
    interface IEventService : IService
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
    }
}
