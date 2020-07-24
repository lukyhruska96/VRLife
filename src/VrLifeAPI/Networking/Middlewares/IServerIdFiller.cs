using System;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Networking.Middlewares;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeShared.Networking.Middlewares
{
    public interface IServerIdFiller : IMiddleware<MainMessage>
    {
        void SetId(uint id);
    }
}
