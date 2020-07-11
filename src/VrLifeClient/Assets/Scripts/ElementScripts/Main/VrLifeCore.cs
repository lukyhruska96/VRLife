using Assets.Scripts.API;
using System.Net;
using UnityEngine;
using VrLifeClient.API;
using VrLifeShared.Core.Applications;

namespace VrLifeClient
{
    class VrLifeCore : MonoBehaviour
    {
        private static VrLifeClient _client;

        private static OpenAPI _api;
        public static OpenAPI API { get => _api; }
        public static bool IsExiting { get; private set; } = false;


        void Awake()
        {
            Config config = new Config();
            config.MainServer = new IPEndPoint(IPAddress.Loopback, 8766);
            _client = new VrLifeClient(config);
            _client.Init();
            _api = _client.OpenAPI;
        }

        private void OnDestroy()
        {
            if(API.Room.CurrentRoom != null)
            {
                Debug.Log("Room exiting");
                IsExiting = true;
                API.Room.RoomExit(API.Room.CurrentRoom.Id).Wait();
            }
        }

        public static ClosedAPI GetClosedAPI(AppInfo info)
        {
            if(Permissions.IsAllowed(info))
            {
                return _client.ClosedAPI;
            }
            return null;
        }

    }
}
