using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Forwarder.API;

namespace VrLifeServer.API.Forwarder
{
    class ClosedAPI : IClosedAPI
    {
        private IOpenAPI _openApi;
        public IOpenAPI OpenAPI { get => _openApi; }

        private IServiceProvider _services;
        public IServiceProvider Services { get => _services; }

        public ClosedAPI(IOpenAPI openAPI, IServiceProvider serviceProvider)
        {
            this._openApi = openAPI;
            this._services = serviceProvider;
        }
    }
}
