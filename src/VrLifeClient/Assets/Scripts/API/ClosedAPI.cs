using Assets.Scripts.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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

        public ClosedAPI(OpenAPI openAPI, ServiceProvider serviceProvider, MiddlewareProvider middlewareProvider)
        {
            this._openApi = openAPI;
            this._serviceProvider = serviceProvider;
            this._middlewareProvider = middlewareProvider;
            this._middlewareProvider.RedirectMsgHandler.SetListenner(OpenAPI.Networking);
        }
    }
}
