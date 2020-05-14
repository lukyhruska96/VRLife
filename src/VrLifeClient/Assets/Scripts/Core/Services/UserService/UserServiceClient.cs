using Assets.Scripts.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Events;
using VrLifeClient.API;
using VrLifeShared.Logging;
using VrLifeShared.Networking;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeClient.Core.Services.UserService
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

        public ServiceCallback SignIn(string username, string password)
        {
            return new ServiceCallback(() => {
                AuthMsg auth = new AuthMsg();
                auth.Username = username;
                auth.Password = password;
                UserMngMsg userMngMsg = new UserMngMsg();
                userMngMsg.AuthMsg = auth;
                MainMessage msg = new MainMessage();
                msg.UserMngMsg = userMngMsg;
                MainMessage response = _api.OpenAPI.Networking.Send(msg, _api.OpenAPI.Config.MainServer);
                if(response.SystemMsg.SystemMsgTypeCase == SystemMsg.SystemMsgTypeOneofCase.ErrorMsg)
                {
                    throw new ErrorMsgException(response.SystemMsg.ErrorMsg);
                }
                _api.Middlewares.ClientIdFiller.SetId(response.ClientId);
            });
        }

        public ServiceCallback SignUp(string username, string password)
        {
            return new ServiceCallback(() =>
            {
                UserDetailMsg user = new UserDetailMsg();
                user.Username = username;
                user.Password = password;
                UserRequestMsg userRequestMsg = new UserRequestMsg();
                userRequestMsg.CreateQuery = user;
                UserMsg userMsg = new UserMsg();
                userMsg.UserRequestMsg = userRequestMsg;
                UserMngMsg userMngMsg = new UserMngMsg();
                userMngMsg.UserMsg = userMsg;
                MainMessage msg = new MainMessage();
                msg.UserMngMsg = userMngMsg;
                MainMessage response = _api.OpenAPI.Networking.Send(msg, _api.OpenAPI.Config.MainServer);
                if (response.SystemMsg.SystemMsgTypeCase == SystemMsg.SystemMsgTypeOneofCase.ErrorMsg)
                {
                    throw new ErrorMsgException(response.SystemMsg.ErrorMsg);
                }
            });
        }
    }
}
