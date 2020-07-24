using Assets.Scripts.API;
using System.Net;
using UnityEngine;
using VrLifeAPI;
using VrLifeAPI.Client.API;
using VrLifeClient.API;
using VrLifeClient.API.OpenAPI;
using VrLifeShared.Core.Applications;

namespace VrLifeClient
{
    class VrLifeCore : MonoBehaviour
    {
        private static VrLifeClient _client;

        private static OpenAPI _api;
        public static OpenAPI API { get => _api; }
        public static bool IsExiting { get; private set; } = false;

        public static int MainThreadID { get; private set; }

        public static bool IsMainThread
        {
            get { return System.Threading.Thread.CurrentThread.ManagedThreadId == MainThreadID; }
        }


        void Awake()
        {
            Config config = new Config();
            config.MainServer = new IPEndPoint(IPAddress.Loopback, 8766);
            _client = new VrLifeClient(config);
            _client.Init();
            _api = _client.OpenAPI;
        }

        private void Start()
        {
            MainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
        }

        private void OnDestroy()
        {
            if(API.Room.CurrentRoom != null)
            {
                Debug.Log("Room exiting");
                IsExiting = true;
                API.Room.RoomExit(API.Room.CurrentRoom.Id).Wait();
            }
            _client?.Dispose();
        }

        public static IClosedAPI GetClosedAPI(AppInfo info)
        {
            if(Permissions.IsAllowed(info))
            {
                return _client.ClosedAPI;
            }
            return null;
        }

        

    }
}
