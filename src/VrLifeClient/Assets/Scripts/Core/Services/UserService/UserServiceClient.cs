using Assets.Scripts.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Events;
using VrLifeClient.API;
using VrLifeClient.Core.Services.RoomService;
using VrLifeClient.Core.Services.SystemService;
using VrLifeShared.Logging;
using VrLifeShared.Networking;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeClient.Core.Services.UserService
{
    class UserServiceClient : IServiceClient
    {
        private ClosedAPI _api;

        private ulong? _userId;
        public ulong? UserId { get => _userId; }

        public delegate void UserLogoutEventHandler();
        public event UserLogoutEventHandler UserLoggedOut;

        public void HandleMessage(MainMessage msg)
        {

        }

        public void Init(ClosedAPI api)
        {
            this._api = api;
            this._api.Services.System.ProviderLost += Reset;
        }

        public ServiceCallback<UserListMsg> CurrentRoomUsers()
        {
            return new ServiceCallback<UserListMsg>(() => {
                Room r = _api.Services.Room.CurrentRoom;
                if (r == null)
                {
                    throw new UserException("User must be connected to some room.");
                }
                MainMessage msg = new MainMessage();
                msg.UserMngMsg = new UserMngMsg();
                msg.UserMngMsg.UserMsg = new UserMsg();
                msg.UserMngMsg.UserMsg.UserRequestMsg = new UserRequestMsg();
                msg.UserMngMsg.UserMsg.UserRequestMsg.ListQuery = new UserDetailMsg();
                MainMessage response = _api.OpenAPI.Networking.Send(msg, r.Address);
                if(SystemServiceClient.IsErrorMsg(response))
                {
                    throw new ErrorMsgException(response.SystemMsg.ErrorMsg);
                }
                UserListMsg listMsg = response.UserMngMsg.UserMsg.UserListMsg;
                if(listMsg == null)
                {
                    throw new ErrorMsgException("Unknown response.");
                }
                return listMsg;
            });
        }

        public ServiceCallback<bool> Login(string username, string password)
        {
            return new ServiceCallback<bool>(() => {
                AuthMsg auth = new AuthMsg();
                auth.Username = username;
                auth.Password = password;
                UserMngMsg userMngMsg = new UserMngMsg();
                userMngMsg.AuthMsg = auth;
                MainMessage msg = new MainMessage();
                msg.UserMngMsg = userMngMsg;
                MainMessage response = _api.OpenAPI.Networking.Send(msg, _api.OpenAPI.Config.MainServer);
                if(SystemServiceClient.IsErrorMsg(response))
                {
                    throw new ErrorMsgException(response.SystemMsg.ErrorMsg);
                }
                UserDetailMsg userDetail = response.UserMngMsg.UserMsg.UserDetailMsg;
                if(userDetail == null)
                {
                    throw new ErrorMsgException("Unknown response.");
                }
                _api.Middlewares.ClientIdFiller.SetId(response.ClientId);
                _userId = userDetail.UserId;
                return true;
            });
        }

        public ServiceCallback<bool> Register(string username, string password)
        {
            return new ServiceCallback<bool>(() =>
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
                return true;
            });
        }

        public void Reset()
        {
            _userId = null;
            OnUserLogout();
        }

        public void OnUserLogout()
        {
            UserLoggedOut?.Invoke();
        }
    }
}
