using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.API;
using VrLifeServer.API.Forwarder;
using VrLifeServer.Core.Services.SystemService;
using VrLifeShared.Logging;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.EventService
{
    class EventServiceForwarder : IEventServiceForwarder
    {
        private ClosedAPI _api;
        private ILogger _log;
        public MainMessage HandleMessage(MainMessage msg)
        {
            EventMsg eventMsg = msg.EventMsg;
            if(eventMsg.AppTypeCase != EventMsg.AppTypeOneofCase.None)
            {
                ulong? userId = _api.Services.User.GetUserIdByClientId(msg.ClientId);
                if(!userId.HasValue)
                {
                    return ISystemService.CreateErrorMessage(msg.MsgId, 0, 0, "Unauthenticated client.");
                }
                return _api.Services.App.HandleEvent(eventMsg, userId.Value, eventMsg.InstanceId);
            }
            // TODO
            return HandleEvent(msg);
        }

        public void Init(ClosedAPI api)
        {
            this._api = api;
            this._log = api.OpenAPI.CreateLogger(this.GetType().Name);
        }

        public MainMessage HandleEvent(MainMessage msg)
        {
            return ISystemService.CreateOkMessage(msg.MsgId);
        }
    }
}
