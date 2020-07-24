using VrLifeAPI.Networking.NetworkingModels;
using VrLifeAPI.Provider.API;
using VrLifeAPI.Provider.Core.Services.TickRateService;
using VrLifeServer.API.Provider;

namespace VrLifeServer.Core.Services.TickRateService
{
    class TickRateServiceProvider : ITickRateServiceProvider
    {
        public MainMessage HandleMessage(MainMessage msg)
        {
            return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, "Provider cannot receive TickRateMsg.");
        }

        public void Init(IClosedAPI api)
        {
        }
    }
}
