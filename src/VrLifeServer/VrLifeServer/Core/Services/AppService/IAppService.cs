using System;
using System.Collections.Generic;
using System.Text;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.AppService
{

    interface IAppService : IService
    {
        MainMessage HandleEvent(MainMessage msg);
    }
}
