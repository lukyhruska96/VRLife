using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.Core.Applications.DefaultApps;
using VrLifeServer.Core.Services.AppService;
using VrLifeShared.Core.Applications;
using VrLifeShared.Logging;
using VrLifeShared.Networking;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeServer.API.Forwarder
{
    class OpenAPI
    {
        public UDPNetworking<MainMessage> Networking { get => _udpNetworking; }
        private UDPNetworking<MainMessage> _udpNetworking;
        public Config Config { get => _config; }
        private Config _config;

        private ClosedAPI _closedAPI;
        private bool init = false;

        public DefaultAppsForwarder Apps { get; private set; } = new DefaultAppsForwarder();

        public OpenAPI(UDPNetworking<MainMessage> udpNetworking, Config config)
        {
            this._udpNetworking = udpNetworking;
            this._config = config;
        }

        public void Init(ClosedAPI api)
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

        public ClosedAPI GetClosedAPI(AppInfo app)
        {
            return Permissions.IsAllowed(app) ? _closedAPI : null;
        }
        
    }
}
