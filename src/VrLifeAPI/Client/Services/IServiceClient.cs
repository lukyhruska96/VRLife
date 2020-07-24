using System;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Client.API;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Client.Services
{
    public interface IServiceClient
    {
        void HandleMessage(MainMessage msg);

        void Init(IClosedAPI api);
    }
}
