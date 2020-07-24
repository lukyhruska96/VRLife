using System;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI;
using VrLifeAPI.Common;
using VrLifeAPI.Common.Logging.Logging;
using VrLifeAPI.Forwarder.API;
using VrLifeAPI.Forwarder.Core.Applications.DefaultApps;
using VrLifeAPI.Networking;
using VrLifeAPI.Networking.NetworkingModels;
using VrLifeServer.Core.Applications.DefaultApps;
using VrLifeServer.Core.Services.AppService;
using VrLifeShared.Core.Applications;
using VrLifeShared.Logging;
using VrLifeShared.Networking;

namespace VrLifeServer.API.Forwarder
{
    class OpenAPI : IOpenAPI
    {
        public IUDPNetworking<MainMessage> Networking { get => _udpNetworking; }
        private IUDPNetworking<MainMessage> _udpNetworking;
        public IConfig Config { get => _config; }
        private IConfig _config;

        private IClosedAPI _closedAPI;
        private bool init = false;

        public IDefaultAppsForwarder Apps { get; private set; } = new DefaultAppsForwarder();

        public OpenAPI(UDPNetworking<MainMessage> udpNetworking, IConfig config)
        {
            this._udpNetworking = udpNetworking;
            this._config = config;
        }

        public void Init(IClosedAPI api)
        {
            if (!init)
            {
                _closedAPI = api;
                init = true;
            }
        }

        public ILogger CreateLogger(string className)
        {
            return new LoggerWrapper(className, this._config.Loggers);
        }

        public IClosedAPI GetClosedAPI(AppInfo app)
        {
            return Permissions.IsAllowed(app) ? _closedAPI : null;
        }
        
    }
}
