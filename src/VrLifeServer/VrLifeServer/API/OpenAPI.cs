using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.Logging;
using VrLifeServer.Networking;
using VrLifeServer.Networking.NetworkingModels;

namespace VrLifeServer.API
{
    class OpenAPI
    {
        public UDPNetworking<MainMessage> Networking { get => _udpNetworking; }
        private UDPNetworking<MainMessage> _udpNetworking;
        public Config Conf { get => _config; }
        private Config _config;

        public OpenAPI(UDPNetworking<MainMessage> udpNetworking, Config config)
        {
            this._udpNetworking = udpNetworking;
            this._config = config;
        }

        public ILogger CreateLogger(string className)
        {
            return new LoggerWrapper(className, this._config.Loggers);
        }

        
    }
}
