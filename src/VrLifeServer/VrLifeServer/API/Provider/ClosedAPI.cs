using VrLifeAPI.Provider.API;
using VrLifeAPI.Provider.API.APIs;
using VrLifeServer.API.Provider.APIs;

namespace VrLifeServer.API.Provider
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
