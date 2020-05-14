using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VrLifeShared.Logging;
using VrLifeShared.Networking;
using VrLifeShared.Networking.Middlewares;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeClient
{
    class Init : MonoBehaviour
    {
        private VrLifeClient _client;
        public VrLifeClient Client { get => _client; }
        private void Awake()
        {
            Config config = new Config();
            config.MainServer = new IPEndPoint(IPAddress.Loopback, 8766);
            config.Loggers = new FileLogger("./VrLifeClient");
            _client = new VrLifeClient(config);
            _client.Init();
        }
    }
}
