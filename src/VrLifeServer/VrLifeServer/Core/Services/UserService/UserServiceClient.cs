using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.API;
using VrLifeServer.Logging;
using VrLifeServer.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.UserService
{
    class UserServiceClient : IServiceClient
    {
        private ClosedAPI _api;
        private ILogger _log;
        public void HandleMessage(MainMessage msg)
        {
            _log.Error("Cannot handle this type of message.");
        }

        public void Init(ClosedAPI api)
        {
            this._api = api;
            this._log = api.OpenAPI.CreateLogger(this.GetType().Name);
        }
    }
}
