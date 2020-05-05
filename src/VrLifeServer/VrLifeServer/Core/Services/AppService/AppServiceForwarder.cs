using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.API;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.AppService
{
    class AppServiceForwarder : IAppService
    {

        public MainMessage HandleEvent(EventMsg msg, ulong userId, uint instanceId)
        {
            throw new NotImplementedException();
        }

        public MainMessage HandleMessage(MainMessage msg)
        {
            throw new NotImplementedException();
        }

        public void Init(ClosedAPI api)
        {

        }
    }
}
