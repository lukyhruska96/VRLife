using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.API.Provider;
using VrLifeServer.Core.Services.SystemService;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.TickRateService
{
    class TickRateServiceProvider : ITickRateServiceProvider
    {
        public MainMessage HandleMessage(MainMessage msg)
        {
            return ISystemService.CreateErrorMessage(msg.MsgId, 0, 0, "Provider cannot receive TickRateMsg.");
        }

        public void Init(ClosedAPI api)
        {
        }
    }
}
