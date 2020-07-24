using Assets.Scripts.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeAPI.Client.API.OpenAPI;
using VrLifeAPI.Client.Services;
using VrLifeAPI.Networking.NetworkingModels;
using VrLifeClient.Core.Services.TickRateService;

namespace VrLifeClient.API.OpenAPI
{
    class TickAPI : ITickAPI
    {
        private ITickRateServiceClient _tickRateService;
        
        public TickAPI(ITickRateServiceClient tickRateService)
        {
            this._tickRateService = tickRateService;
        }

        public IServiceCallback<SnapshotData> GetSnapshot()
        {
            return _tickRateService.GetSnapshot();
        }

        public SnapshotData[] GetSnapshotBuffer()
        {
            return _tickRateService.SnapshotBuffer.ToArray();
        }
    }
}
