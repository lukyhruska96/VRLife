using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.API;
using VrLifeServer.Core.Services.SystemService;
using VrLifeShared.Logging;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.EventService
{
    class EventServiceForwarder : IEventService
    {
        private ClosedAPI _api;
        private ILogger _log;
        public MainMessage HandleMessage(MainMessage msg)
        {
            EventMsg eventMsg = msg.EventMsg;
            if(eventMsg.AppTypeCase != EventMsg.AppTypeOneofCase.None)
            {
                ulong userId = _api.Services.User.GetUserId(msg.ClientId);
                return _api.Services.App.HandleEvent(eventMsg, userId, eventMsg.InstanceId);
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
