using System;
using System.Collections.Generic;
using System.Text;
using VrLifeClient.API;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeClient.Core.Services.EventService
{
    class EventServiceClient : IServiceClient
    {
        private ClosedAPI _api;

        public void HandleMessage(MainMessage msg)
        {
            throw new NotImplementedException();
        }

        public void Init(ClosedAPI api)
        {
            _api = api;
        }
    }
}
