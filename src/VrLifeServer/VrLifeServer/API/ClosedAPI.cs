using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.Networking.NetworkingModels;

namespace VrLifeServer.API
{
    class ClosedAPI
    {
        public OpenAPI OpenAPI { get => _openApi; }
        private OpenAPI _openApi;

        private ServiceProvider _services;
        public ServiceProvider Services { get => _services; }

        public ClosedAPI(OpenAPI openAPI, ServiceProvider serviceProvider)
        {
            this._openApi = OpenAPI;
            this._services = serviceProvider;
        }
    }
}
