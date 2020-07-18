using Assets.Scripts.API;
using System;

namespace VrLifeClient.API
{
    class ClosedAPI : IDisposable
    {
        private OpenAPI.OpenAPI _openApi;
        public OpenAPI.OpenAPI OpenAPI { get => _openApi; }

        private ServiceProvider _serviceProvider;
        public ServiceProvider Services { get => _serviceProvider; }

        private MiddlewareProvider _middlewareProvider;
        public MiddlewareProvider Middlewares { get => _middlewareProvider; }

        private MenuAPI.MenuAPI _menuAPI;
        public MenuAPI.MenuAPI MenuAPI { get => _menuAPI; }

        private HUDAPI.HUDAPI _hudAPI;
        public HUDAPI.HUDAPI HUDAPI { get => _hudAPI; }

        private GlobalAPI.GlobalAPI _globalAPI;
        public GlobalAPI.GlobalAPI GlobalAPI { get => _globalAPI; }

        private ObjectAPI.ObjectAPI _objectAPI;
        public ObjectAPI.ObjectAPI ObjectAPI { get => _objectAPI; }

        private DeviceAPI.DeviceAPI _deviceAPI;
        public DeviceAPI.DeviceAPI DeviceAPI { get => _deviceAPI; }

        public SceneController SceneController { get => SceneController.current; }

        public ClosedAPI(OpenAPI.OpenAPI openAPI, ServiceProvider serviceProvider, MiddlewareProvider middlewareProvider)
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
