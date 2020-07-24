using System;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Common.Core.Services.AppService
{

    public interface IAppService : IService
    {
        byte[] HandleEvent(MainMessage msg);
    }
}
