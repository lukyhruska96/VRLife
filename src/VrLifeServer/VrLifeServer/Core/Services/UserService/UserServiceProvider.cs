using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.API;
using VrLifeServer.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.UserService
{
    class UserServiceProvider : IUserService
    {
        private ClosedAPI _api;
        public MainMessage HandleMessage(MainMessage msg)
        {
            
        }

        public void Init(ClosedAPI api)
        {
            this._api = api;
        }
    }
}
