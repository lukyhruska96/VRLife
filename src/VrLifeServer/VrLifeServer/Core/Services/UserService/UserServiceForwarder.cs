using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using VrLifeServer.API;
using VrLifeServer.Core.Services.SystemService;
using VrLifeShared.Logging;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.UserService
{
    class UserServiceForwarder : IUserService
    {
        private ClosedAPI _api;
        private ILogger _log;

        public ulong GetUserId(uint clientId)
        {
            throw new NotImplementedException();
        }

        public MainMessage HandleMessage(MainMessage msg)
        {
            return ISystemService.CreateRedirectMessage(msg, _api.OpenAPI.Config.MainServer);
        }

        public void Init(ClosedAPI api)
        {
            this._api = api;
            this._log = api.OpenAPI.CreateLogger(this.GetType().Name);
        }
    }
}
