using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.API;
using VrLifeServer.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services
{
    interface IServiceClient
    {
        void HandleMessage(MainMessage msg);

        void Init(ClosedAPI api);
    }
}
