using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.API.Provider;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.TickRateService
{
    class TickRateServiceProvider : ITickRateServiceProvider
    {
        public MainMessage HandleMessage(MainMessage msg)
        {
            throw new NotImplementedException();
        }

        public void Init(ClosedAPI api)
        {
        }
    }
}
