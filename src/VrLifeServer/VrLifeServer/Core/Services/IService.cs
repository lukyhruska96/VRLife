using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services
{
    interface IService
    {
        MainMessage HandleMessage(MainMessage msg);
        void Init();
    }
}
