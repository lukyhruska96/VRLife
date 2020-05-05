﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeShared.Logging;
using VrLifeShared.Networking;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeClient.API
{
    class OpenAPI
    {
        public UDPNetworking<MainMessage> Networking { get => _udpNetworking; }
        private UDPNetworking<MainMessage> _udpNetworking;

        public Config Config { get => _config; }
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