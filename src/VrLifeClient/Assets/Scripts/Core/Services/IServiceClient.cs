using System;
using System.Collections.Generic;
using System.Text;
using VrLifeClient.API;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeClient.Core.Services
{
    interface IServiceClient
    {
        void HandleMessage(MainMessage msg);

        void Init(ClosedAPI api);
    }
}
