using System;
using System.Collections.Generic;
using System.Text;
using VrLifeClient.API;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeClient.Core.Services.AppService
{
    class AppServiceClient : IServiceClient
    {
        private ClosedAPI _api;

        public void HandleMessage(MainMessage msg)
        {
            AppMsg appMsg = msg.AppMsg;

        }

        public void Init(ClosedAPI api)
        {
            this._api = api;

        }
    }
}
