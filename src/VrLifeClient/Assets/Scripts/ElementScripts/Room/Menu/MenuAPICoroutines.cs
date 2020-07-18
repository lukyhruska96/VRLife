using Assets.Scripts.Core.Applications.DefaultApps.FriendsManagementApp;
using System;
using System.Collections;
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
        private void Awake()
        {
            current = this;
        }

        private void OnDestroy()
        {
            current = null;
        }
    }
}
