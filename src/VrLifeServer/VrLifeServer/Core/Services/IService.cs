using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.API;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services
{
    interface IService
    {
        MainMessage HandleMessage(MainMessage msg);
    }
}
