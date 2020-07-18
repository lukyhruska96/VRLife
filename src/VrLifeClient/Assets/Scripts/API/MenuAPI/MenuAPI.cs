using Assets.Scripts.API.HUDAPI;
using Assets.Scripts.ElementScripts.Room.Menu;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VrLifeClient.API;

namespace VrLifeClient.API.MenuAPI
{
    class MenuAPI
    {
        private ClosedAPI _api;
        public MenuAPI(ClosedAPI api)
        {
            _api = api;
        }

        public Coroutine StartCoroutine(IEnumerator coroutine)
        {
            if(MenuAPICoroutines.current == null)
            {
                return null;
            }
            return MenuAPICoroutines.current.StartCoroutine(coroutine);
        }

        public void StopCoroutine(Coroutine coroutine)
        {
            MenuAPICoroutines.current.StopCoroutine(coroutine);
        }
    }
}
