using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using VrLifeServer.API;
using VrLifeServer.Core.Services.SystemService;
using VrLifeServer.Logging;
using VrLifeServer.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.UserService
{
    class UserServiceForwarder : IUserService
    {
        private ClosedAPI _api;
        private ILogger _log;
        public MainMessage HandleMessage(MainMessage msg)
        {
            if (msg.MessageTypeCase != MainMessage.MessageTypeOneofCase.UserMngMsg)
            {
                _log.Error("Cannot handle this type of message.");
                return ISystemService.CreateErrorMessage(0, 0, 0,
                    this.GetType().Name + ": Cannot handle this type of message.");
            }
            return ISystemService.CreateRedirectMessage(msg, _api.OpenAPI.Conf.MainServer);
        }

        public void Init(ClosedAPI api)
        {
            this._api = api;
            this._log = api.OpenAPI.CreateLogger(this.GetType().Name);
        }
    }
}
