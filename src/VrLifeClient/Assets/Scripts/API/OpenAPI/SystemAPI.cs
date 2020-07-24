using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeAPI.Client.API.OpenAPI;
using VrLifeAPI.Client.Services;
using VrLifeClient.Core.Services.SystemService;

namespace VrLifeClient.API.OpenAPI
{
    class SystemAPI : ISystemAPI
    {
        private ISystemServiceClient _systemService;
        public event Action ForwarderLost
        {
            add { this._systemService.ForwarderLost += value; }
            remove { this._systemService.ForwarderLost -= value; }
        }
        public event Action ProviderLost
        {
            add { this._systemService.ProviderLost += value; }
            remove { this._systemService.ProviderLost -= value; }
        }

        public SystemAPI(ISystemServiceClient systemService)
        {
            this._systemService = systemService;
        }
    }
}
