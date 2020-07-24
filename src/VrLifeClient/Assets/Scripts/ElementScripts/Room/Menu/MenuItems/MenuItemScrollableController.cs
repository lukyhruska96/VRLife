using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ElementScripts.Room.Menu.MenuItems
{
    class MenuItemScrollableController : MonoBehaviour
    {
        public event Action Enabled;
        public event Action Disabled;

        private void OnEnable()
        {
            Enabled?.Invoke();
        }

        private void OnDisable()
        {
            Disabled?.Invoke();
        }
    }
}
