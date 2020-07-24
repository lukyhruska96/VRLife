using Assets.Scripts.API.HUDAPI;
using Assets.Scripts.Core.Applications.MenuApp.MenuItems;
using Assets.Scripts.ElementScripts.Room.Menu;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using VrLifeAPI.Client.API.MenuAPI;
using VrLifeAPI.Client.Applications.MenuApp.MenuItems;

namespace VrLifeClient.API.MenuAPI
{
    class MenuAPI : IMenuAPI
    {
        private ClosedAPI _api;
        public MenuAPI(ClosedAPI api)
        {
            _api = api;
        }

        public ulong StartCoroutine(IEnumerator coroutine)
        {
            if(MenuAPICoroutines.current == null)
            {
                return ulong.MaxValue;
            }
            return MenuAPICoroutines.current.AddCoroutine(coroutine);
        }

        public void StopCoroutine(ulong id)
        {
            MenuAPICoroutines.current.DelCoroutine(id);
        }

        public YieldInstruction WaitForSeconds(float sec)
        {
            return new UnityEngine.WaitForSeconds(sec);
        }

        public CustomYieldInstruction WaitUntil(Func<bool> predicate)
        {
            return new UnityEngine.WaitUntil(predicate);
        }

        public IMenuItemButton CreateButton(string name)
        {
            return new MenuItemButton(name);
        }

        public IMenuItemCheckbox CreateCheckBox(string name)
        {
            return new MenuItemCheckbox(name);
        }

        public IMenuItemGrid CreateGrid(string name, int width, int height)
        {
            return new MenuItemGrid(name, width, height);
        }

        public IMenuItemImage CreateImage(string name)
        {
            return new MenuItemImage(name);
        }

        public IMenuItemInput CreateInput(string name)
        {
            return new MenuItemInput(name);
        }

        public IMenuItemScrollable CreateScrollable(string name, TextAnchor layout)
        {
            return new MenuItemScrollable(name, layout);
        }

        public IMenuItemText CreateText(string name)
        {
            return new MenuItemText(name);
        }
    }
}
