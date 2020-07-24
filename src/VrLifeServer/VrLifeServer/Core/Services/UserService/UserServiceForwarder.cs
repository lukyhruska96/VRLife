using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using VrLifeAPI.Common.Core.Services.UserService;
using VrLifeAPI.Common.Logging.Logging;
using VrLifeAPI.Forwarder.API;
using VrLifeAPI.Forwarder.Core.Services.UserService;
using VrLifeAPI.Networking.NetworkingModels;
using VrLifeServer.API.Forwarder;

namespace VrLifeServer.Core.Services.UserService
{
    class UserServiceForwarder : IUserServiceForwarder
    {
        private IClosedAPI _api;
        private ILogger _log;
        private Dictionary<ulong, User> _userCache = new Dictionary<ulong, User>();
        private Dictionary<ulong, ulong?> _clientId2UserId = new Dictionary<ulong, ulong?>();

        public MainMessage HandleMessage(MainMessage msg)
        {
            if(msg.SenderIdCase == MainMessage.SenderIdOneofCase.ServerId)
            {
                return HandleServerMessage(msg);
            }
            else if(msg.UserMngMsg.UserMsg.UserRequestMsg.ListQuery != null)
            {
                return HandleListRoomUsers(msg);
            }
            return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateRedirectMessage(msg, _api.OpenAPI.Config.MainServer);
        }

        private MainMessage HandleListRoomUsers(MainMessage msg)
        {
            ulong? userId = _clientId2UserId[msg.ClientId];
            if (!userId.HasValue)
            {
                return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, "Unauthorized request.");
            }
            uint? roomId = _api.Services.Room.RoomByUserId(userId.Value);
            if (!roomId.HasValue)
            {
                return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, "User must be connected to some Room.");
            }
            ulong[] users = _api.Services.Room.GetConnectedUsers(roomId.Value);
            if(users == null)
            {
                users = new ulong[0];
            }
            UserListMsg userListMsg = new UserListMsg();
            userListMsg.Users.AddRange(users.Select(x => User.Get(x)).Where(x => x != null).Select(x => x.ToMessage()));
            MainMessage response = new MainMessage();
            response.UserMngMsg = new UserMngMsg();
            response.UserMngMsg.UserMsg = new UserMsg();
            response.UserMngMsg.UserMsg.UserListMsg = userListMsg;
            return response;
        }

        private MainMessage HandleServerMessage(MainMessage msg)
        {
            if(msg.ServerId != 0)
            {
                return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, "Forwarders cannot communicate with each other.");
            }
            return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, "Communication between provider and forwarder is not supported in UserService.");
        }

        public void Init(IClosedAPI api)
        {
            this._api = api;
            this._log = api.OpenAPI.CreateLogger(this.GetType().Name);
        }

        public IUser GetUserById(ulong userId, bool cached = false)
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
                if(VrLifeAPI.Common.Core.Services.ServiceUtils.IsError(response))
                {
                    _log.Error(response.SystemMsg.ErrorMsg.ErrorMsg_);
                }
                return null;
            }
            user = new User(response.UserMngMsg.UserMsg.UserDetailMsg);
            _userCache[userId] = user;
            return user;
        }

        public ulong? GetUserIdByClientId(ulong clientId, bool cached = false)
        {
            if(cached &&_clientId2UserId.TryGetValue(clientId, out ulong? val))
            { 
                return val;
            }
            UserRequestMsg userRequestMsg = new UserRequestMsg();
            userRequestMsg.UserByClientId = clientId;
            MainMessage msg = new MainMessage();
            msg.UserMngMsg = new UserMngMsg();
            msg.UserMngMsg.UserMsg = new UserMsg();
            msg.UserMngMsg.UserMsg.UserRequestMsg = userRequestMsg;
            MainMessage response = _api.OpenAPI.Networking.Send(msg, _api.OpenAPI.Config.MainServer);
            if(VrLifeAPI.Common.Core.Services.ServiceUtils.IsError(response))
            {
                _log.Error(response.SystemMsg.ErrorMsg.ErrorMsg_);
                return null;
            }
            User user = new User(response.UserMngMsg.UserMsg.UserDetailMsg);
            _userCache[user.Id] = user;
            _clientId2UserId[clientId] = user.Id;
            return user.Id;
        }

        public bool FastCheckUserId(ulong clientId, ulong userId)
        {
            return userId == GetUserIdByClientId(clientId, true) || userId == GetUserIdByClientId(clientId);
        }
    }
}
