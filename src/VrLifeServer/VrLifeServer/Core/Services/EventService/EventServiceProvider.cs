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
            const string err_msg = "Main server does not handle this type of message.";
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
