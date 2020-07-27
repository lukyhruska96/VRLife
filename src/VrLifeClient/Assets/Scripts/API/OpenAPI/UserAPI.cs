using Assets.Scripts.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeAPI.Client.API.OpenAPI;
using VrLifeAPI.Client.Core.Services;
using VrLifeAPI.Networking.NetworkingModels;
using VrLifeClient.Core.Services.UserService;

namespace VrLifeClient.API.OpenAPI
{
    class UserAPI : IUserAPI
    {
        private IUserServiceClient _userService;
        public ulong? UserId { get => _userService.UserId; }

        public UserAPI(IUserServiceClient userService)
        {
            this._userService = userService;
        }

        public IServiceCallback<bool> Register(string username, string password)
        {
            return this._userService.Register(username, password);
        }

        public IServiceCallback<bool> Login(string username, string password)
        {
            return this._userService.Login(username, password);
        }

        public IServiceCallback<UserListMsg> CurrentRoomUsers()
        {
            return this._userService.CurrentRoomUsers();
        }

        public void Logout()
        {
            this._userService.Reset();
        }
    }
}
