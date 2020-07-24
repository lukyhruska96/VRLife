using Assets.Scripts.API;
using VrLifeAPI.Client.API;
using VrLifeAPI.Client.API.ClosedAPI;
using VrLifeAPI.Client.API.GlobalAPI;
using VrLifeAPI.Client.API.MenuAPI;
using VrLifeAPI.Client.API.ObjectAPI;
using VrLifeClient.API.HUDAPI;

namespace VrLifeClient.API
{
    class ClosedAPI : IClosedAPI
    {
        private IOpenAPI _openApi;
        public IOpenAPI OpenAPI { get => _openApi; }

        private ServiceProvider _serviceProvider;
        public IServiceProvider Services { get => _serviceProvider; }

        private MiddlewareProvider _middlewareProvider;
        public IMiddlewareProvider Middlewares { get => _middlewareProvider; }

        private MenuAPI.MenuAPI _menuAPI;
        public IMenuAPI MenuAPI { get => _menuAPI; }

        private HUDAPI.HUDAPI _hudAPI;
        public IHUDAPI HUDAPI { get => _hudAPI; }

        private GlobalAPI.GlobalAPI _globalAPI;
        public IGlobalAPI GlobalAPI { get => _globalAPI; }

        private ObjectAPI.ObjectAPI _objectAPI;
        public IObjectAPI ObjectAPI { get => _objectAPI; }

        private DeviceAPI.DeviceAPI _deviceAPI;
        public IDeviceAPI DeviceAPI { get => _deviceAPI; }

        public SceneController SceneController { get => SceneController.current; }

        public ClosedAPI(IOpenAPI openAPI, ServiceProvider serviceProvider, MiddlewareProvider middlewareProvider)
        {
            this._openApi = openAPI;
            this._serviceProvider = serviceProvider;
            this._middlewareProvider = middlewareProvider;
            this._middlewareProvider.RedirectMsgHandler.SetListenner(_openApi.Networking);

            this._menuAPI = new MenuAPI.MenuAPI(this);
            this._hudAPI = new HUDAPI.HUDAPI(this);
            this._globalAPI = new GlobalAPI.GlobalAPI(this);
            this._objectAPI = new ObjectAPI.ObjectAPI(this);
            this._deviceAPI = new DeviceAPI.DeviceAPI();
        }

        public void Dispose()
        {
            _deviceAPI.Dispose();
        }
    }
}
