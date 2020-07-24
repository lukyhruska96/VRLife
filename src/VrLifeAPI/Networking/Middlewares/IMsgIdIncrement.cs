using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using VrLifeAPI.Networking.Middlewares;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeShared.Networking.Middlewares
{
    public interface IMsgIdIncrement : IMiddleware<MainMessage>
    {
    }
}
