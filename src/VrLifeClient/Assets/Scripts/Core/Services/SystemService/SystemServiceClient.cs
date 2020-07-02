using System;
using System.Collections.Generic;
using System.Text;
using VrLifeClient.API;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeClient.Core.Services.SystemService
{
    class SystemServiceClient : IServiceClient
    {
        private ClosedAPI _api;

        public void HandleMessage(MainMessage msg)
        {
            throw new NotImplementedException();
        }

        public void Init(ClosedAPI api)
        {
            this._api = api;
        }

        public static bool IsErrorMsg(MainMessage msg)
        {
            return msg.MessageTypeCase == MainMessage.MessageTypeOneofCase.SystemMsg && msg.SystemMsg.SystemMsgTypeCase == SystemMsg.SystemMsgTypeOneofCase.ErrorMsg;
        }
    }
}
