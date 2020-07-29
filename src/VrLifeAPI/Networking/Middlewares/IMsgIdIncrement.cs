using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using VrLifeAPI.Networking.Middlewares;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeShared.Networking.Middlewares
{
    /// <summary>
    /// Interface MsgIdIncrement middlewaru.
    /// </summary>
    public interface IMsgIdIncrement : IMiddleware<MainMessage>
    {
    }
}
