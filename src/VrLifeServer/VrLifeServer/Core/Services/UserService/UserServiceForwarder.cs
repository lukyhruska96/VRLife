using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.API;
using VrLifeServer.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.UserService
{
    class UserServiceForwarder : IUserService
    {
        public MainMessage HandleMessage(MainMessage msg)
        {
            throw new NotImplementedException();
        }

        public void Init(ClosedAPI api)
        {

        }
    }
}
