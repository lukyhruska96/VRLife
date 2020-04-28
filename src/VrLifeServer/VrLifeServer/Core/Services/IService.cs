using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.API;
using VrLifeServer.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services
{
    interface IService
    {
        MainMessage HandleMessage(MainMessage msg);
        void Init(ClosedAPI api);
    }
}
