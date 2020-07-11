using Assets.Scripts.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeClient.Core.Services.UserService;
using VrLifeShared.Networking.NetworkingModels;

namespace Assets.Scripts.API.OpenAPI
{
    class UserAPI
    {
        private UserServiceClient _userService;
        public ulong? UserId { get => _userService.UserId; }

        public UserAPI(UserServiceClient userService)
        {
            this._userService = userService;
        }

        public ServiceCallback<bool> Register(string username, string password)
        {
            return this._userService.Register(username, password);
        }

        public ServiceCallback<bool> Login(string username, string password)
        {
            return this._userService.Login(username, password);
        }

        public ServiceCallback<UserListMsg> CurrentRoomUsers()
        {
            return this._userService.CurrentRoomUsers();
        }

        public void Logout()
        {
            this._userService.Reset();
        }
    }
}
