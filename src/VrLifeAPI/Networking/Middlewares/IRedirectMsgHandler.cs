using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VrLifeAPI.Networking;
using VrLifeAPI.Networking.Middlewares;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeShared.Networking.Middlewares
{
    public interface IRedirectMsgHandler : IMiddleware<MainMessage>
    {
        void SetListenner(INetworking<MainMessage> networking);
    }
}
