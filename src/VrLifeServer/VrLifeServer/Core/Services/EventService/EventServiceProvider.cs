using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.API;
using VrLifeServer.API.Provider;
using VrLifeServer.Core.Services.SystemService;
using VrLifeShared.Logging;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.EventService
{
    class EventServiceProvider : IEventServiceProvider
    {
        private ClosedAPI _api;
        private ILogger _log;
        public MainMessage HandleMessage(MainMessage msg)
        {
            if(msg.EventMsg.EventMsgTypeCase != EventMsg.EventMsgTypeOneofCase.EventDataMsg)
            {
                const string inv_type = "Invalid type of event msg.";
                _log.Error(inv_type);
                return ISystemService.CreateErrorMessage(msg.MsgId, 0, 0, inv_type);
            }
            if(msg.EventMsg.EventDataMsg.AppTypeCase != EventDataMsg.AppTypeOneofCase.None)
            {
                return _api.Services.App.HandleEvent(msg);
            }
            const string err_msg = "Main server does not handle events of this type.";
            _log.Error(err_msg);
            return ISystemService.CreateErrorMessage(0, 0, 0, err_msg);
        }

        public void Init(ClosedAPI api)
        {
            this._api = api;
            this._log = api.OpenAPI.CreateLogger(this.GetType().Name);
        }
    }
}
