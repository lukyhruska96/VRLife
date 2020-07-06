using Assets.Scripts.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeClient.Core.Services.TickRateService;
using VrLifeShared.Networking.NetworkingModels;

namespace Assets.Scripts.API.OpenAPI
{
    class TickAPI
    {
        private TickRateServiceClient _tickRateService;
        
        public TickAPI(TickRateServiceClient tickRateService)
        {
            this._tickRateService = tickRateService;
        }

        public ServiceCallback<SnapshotData> GetSnapshot()
        {
            return _tickRateService.GetSnapshot();
        }

        public SnapshotData[] GetSnapshotBuffer()
        {
            return _tickRateService.SnapshotBuffer.ToArray();
        }
    }
}
