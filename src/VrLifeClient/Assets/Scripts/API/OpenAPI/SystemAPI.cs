using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeClient.Core.Services.SystemService;

namespace VrLifeClient.API.OpenAPI
{
    class SystemAPI
    {
        private SystemServiceClient _systemService;
        public event SystemServiceClient.ForwarderLostEventHandler ForwarderLost
        {
            add { this._systemService.ForwarderLost += value; }
            remove { this._systemService.ForwarderLost -= value; }
        }
        public event SystemServiceClient.ProviderLostEventHandler ProviderLost
        {
            add { this._systemService.ProviderLost += value; }
            remove { this._systemService.ProviderLost -= value; }
        }

        public SystemAPI(SystemServiceClient systemService)
        {
            this._systemService = systemService;
        }
    }
}
