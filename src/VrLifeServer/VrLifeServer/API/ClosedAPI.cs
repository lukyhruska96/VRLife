using System;
using System.Collections.Generic;
using System.Text;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeServer.API
{
    class ClosedAPI
    {
        private OpenAPI _openApi;
        public OpenAPI OpenAPI { get => _openApi; }

        private ServiceProvider _services;
        public ServiceProvider Services { get => _services; }

        public ClosedAPI(OpenAPI openAPI, ServiceProvider serviceProvider)
        {
            this._openApi = openAPI;
            this._services = serviceProvider;
        }
    }
}
