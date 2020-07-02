using Assets.Scripts.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeClient.Core.Services.UserService;

namespace Assets.Scripts.API.OpenAPI
{
    class UserAPI
    {
        private UserServiceClient _userService;

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
    }
}
