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
        private ILogger _logger;

        public OpenAPI(UDPNetworking<MainMessage> udpNetworking, ILogger logger)
        {
            this._udpNetworking = udpNetworking;
            this._logger = logger;
        }

        public ILogger CreateLogger(string className)
        {
            return new LoggerWrapper(className, this._logger);
        }

        
    }
}
