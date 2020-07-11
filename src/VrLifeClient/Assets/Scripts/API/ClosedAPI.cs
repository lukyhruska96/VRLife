using Assets.Scripts.API;
using Assets.Scripts.API.GlobalAPI;
using Assets.Scripts.API.MenuAPI;
using Assets.Scripts.API.ObjectAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace VrLifeClient.API
{
    class ClosedAPI
    {
        private OpenAPI _openApi;
        public OpenAPI OpenAPI { get => _openApi; }

        private ServiceProvider _serviceProvider;
        public ServiceProvider Services { get => _serviceProvider; }

        private MiddlewareProvider _middlewareProvider;
        public MiddlewareProvider Middlewares { get => _middlewareProvider; }

        private MenuAPI _menuAPI;
        public MenuAPI MenuAPI { get => _menuAPI; }

        private GlobalAPI _globalAPI;
        public GlobalAPI GlobalAPI { get => _globalAPI; }

        private ObjectAPI _objectAPI;
        public ObjectAPI ObjectAPI { get => _objectAPI; }

        public SceneController SceneController { get => SceneController.current; }

        public ClosedAPI(OpenAPI openAPI, ServiceProvider serviceProvider, MiddlewareProvider middlewareProvider)
        {
            this._openApi = openAPI;
            this._menuAPI = new MenuAPI(this);
            this._globalAPI = new GlobalAPI(this);
            this._serviceProvider = serviceProvider;
            this._middlewareProvider = middlewareProvider;
            this._middlewareProvider.RedirectMsgHandler.SetListenner(OpenAPI.Networking);
        }
    }
}
