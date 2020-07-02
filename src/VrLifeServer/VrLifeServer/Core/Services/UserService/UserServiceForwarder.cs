using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using VrLifeServer.API.Forwarder;
using VrLifeServer.Core.Services.SystemService;
using VrLifeShared.Logging;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.UserService
{
    class UserServiceForwarder : IUserServiceForwarder
    {
        private ClosedAPI _api;
        private ILogger _log;
        private Dictionary<ulong, User> _userCache = new Dictionary<ulong, User>();

        public MainMessage HandleMessage(MainMessage msg)
        {
            if(msg.SenderIdCase == MainMessage.SenderIdOneofCase.ServerId)
            {
                return HandleServerMessage(msg);
            }
            return ISystemService.CreateRedirectMessage(msg, _api.OpenAPI.Config.MainServer);
        }

        private MainMessage HandleServerMessage(MainMessage msg)
        {
            if(msg.ServerId != 0)
            {
                return ISystemService.CreateErrorMessage(msg.MsgId, 0, 0, "Forwarders cannot communicate with each other.");
            }
            return ISystemService.CreateErrorMessage(msg.MsgId, 0, 0, "Communication between provider and forwarder is not supported in UserService.");
        }

        public void Init(ClosedAPI api)
        {
            this._api = api;
            this._log = api.OpenAPI.CreateLogger(this.GetType().Name);
        }

        public User GetUserById(ulong userId, bool cached = false)
        {
            if(cached && _userCache.TryGetValue(userId, out User user))
            {
                return user;
            }
            UserRequestMsg userRequestMsg = new UserRequestMsg();
            userRequestMsg.UserIdDetail = userId;
            MainMessage msg = new MainMessage();
            msg.UserMngMsg = new UserMngMsg();
            msg.UserMngMsg.UserMsg = new UserMsg();
            msg.UserMngMsg.UserMsg.UserRequestMsg = userRequestMsg;
            MainMessage response = _api.OpenAPI.Networking.Send(msg, _api.OpenAPI.Config.MainServer);
            if (response.MessageTypeCase != MainMessage.MessageTypeOneofCase.UserMngMsg)
            {
                if(ISystemService.IsError(response))
                {
                    _log.Error(response.SystemMsg.ErrorMsg.ErrorMsg_);
                }
                return null;
            }
            user = new User(response.UserMngMsg.UserMsg.UserDetailMsg);
            _userCache[userId] = user;
            return user;
        }

        public ulong? GetUserIdByClientId(ulong clientId)
        {
            UserRequestMsg userRequestMsg = new UserRequestMsg();
            userRequestMsg.UserByClientId = clientId;
            MainMessage msg = new MainMessage();
            msg.UserMngMsg = new UserMngMsg();
            msg.UserMngMsg.UserMsg = new UserMsg();
            msg.UserMngMsg.UserMsg.UserRequestMsg = userRequestMsg;
            MainMessage response = _api.OpenAPI.Networking.Send(msg, _api.OpenAPI.Config.MainServer);
            if(ISystemService.IsError(response))
            {
                _log.Error(response.SystemMsg.ErrorMsg.ErrorMsg_);
                return null;
            }
            User user = new User(response.UserMngMsg.UserMsg.UserDetailMsg);
            _userCache[user.Id] = user;
            return user.Id;
        }
    }
}
