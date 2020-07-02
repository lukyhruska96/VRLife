using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using VrLifeClient.API;
using VrLifeClient.Core.Services.RoomService;
using VrLifeShared.Logging;
using VrLifeShared.Networking;
using VrLifeShared.Networking.Middlewares;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeClient
{
    class VrLifeCore : MonoBehaviour
    {
        private VrLifeClient _client;
        public VrLifeClient Client { get => _client; }

        private static OpenAPI _api;
        public static OpenAPI API { get => _api; }

        public static Room Room { get; set; } = null;

        void Awake()
        {
            Config config = new Config();
            config.MainServer = new IPEndPoint(IPAddress.Loopback, 8766);
            config.Loggers = new FileLogger("./VrLifeClient");
            _client = new VrLifeClient(config);
            _client.Init();
            _api = _client.OpenAPI;
        }

        private void OnDestroy()
        {
            if(Room != null)
            {
                Debug.Log("Room exiting");
                API.Room.RoomExit(Room.Id).Wait();
            }
        }

    }
}
