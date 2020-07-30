using Assets.Scripts.API;
using System.Collections;
using System.Collections.Concurrent;
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


        private static ulong coroutines = 0;
        private static ConcurrentDictionary<ulong, Coroutine> _coroutines = new ConcurrentDictionary<ulong, Coroutine>();
        private static ConcurrentQueue<(ulong, IEnumerator)> _startCoroutines = new ConcurrentQueue<(ulong, IEnumerator)>();
        private static ConcurrentQueue<Coroutine> _stopCoroutines = new ConcurrentQueue<Coroutine>();

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



        public static void DelCoroutine(ulong id)
        {
            if (_coroutines.TryGetValue(id, out Coroutine c))
            {
                _stopCoroutines.Enqueue(c);
            }
        }

        public static ulong AddCoroutine(IEnumerator coroutine)
        {
            _startCoroutines.Enqueue((coroutines, coroutine));
            return coroutines++;
        }

        private void Update()
        {
            while (_startCoroutines.TryDequeue(out (ulong, IEnumerator) en))
            {
                Coroutine c = StartCoroutine(en.Item2);
                _coroutines.TryAdd(en.Item1, c);
            }
            while (_stopCoroutines.TryDequeue(out Coroutine c))
            {
                StopCoroutine(c);
            }
        }
    }
}
