using Assets.Scripts.Core.Applications.DefaultApps.FriendsManagementApp;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ElementScripts.Room.Menu
{
    class MenuAPICoroutines : MonoBehaviour
    {
        public static MenuAPICoroutines current = null;
        private static ulong coroutines = 0;
        private ConcurrentDictionary<ulong, Coroutine> _coroutines = new ConcurrentDictionary<ulong, Coroutine>();
        private ConcurrentQueue<(ulong, IEnumerator)> _startCoroutines = new ConcurrentQueue<(ulong, IEnumerator)>();
        private ConcurrentQueue<Coroutine> _stopCoroutines = new ConcurrentQueue<Coroutine>();
        private void Awake()
        {
            current = this;
        }

        private void OnDestroy()
        {
            current = null;
        }

        public void DelCoroutine(ulong id)
        {
            if(_coroutines.TryGetValue(id, out Coroutine c))
            {
                _stopCoroutines.Enqueue(c);
            }
        }

        public ulong AddCoroutine(IEnumerator coroutine)
        {
            _startCoroutines.Enqueue((coroutines, coroutine));
            return coroutines++;
        }

        private void Update()
        {
            while(_startCoroutines.TryDequeue(out (ulong, IEnumerator) en))
            {
                Coroutine c = StartCoroutine(en.Item2);
                _coroutines.TryAdd(en.Item1, c);
            }
            while(_stopCoroutines.TryDequeue(out Coroutine c))
            {
                StopCoroutine(c);
            }
        }
    }
}
