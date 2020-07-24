using Assets.Scripts.Core.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using VrLifeAPI;
using VrLifeAPI.Client.API.MenuAPI;
using VrLifeAPI.Client.Applications.MenuApp.MenuItems;
using VrLifeClient;

namespace Assets.Scripts.Core.Applications.MenuApp.MenuItems
{
    class MenuItemImage : IMenuItemImage
    {

        private const string PREFAB_PATH = "MenuItems/MenuItemImage";
        private const MenuItemType _type = MenuItemType.MI_IMAGE;
        private GameObject _gameObject;
        private MenuItemInfo _info;
        private MenuItemImageController _ctrl;

        public MenuItemImage(string name)
        {
            _info = new MenuItemInfo
            {
                Type = _type,
                Name = name,
                Parent = null
            };
            AutoResetEvent ev = new AutoResetEvent(false);
            MenuItemUtils.RunCoroutineSync(CreateGameObject(ev), ev);
        }
        
        public void SetImage(Sprite img)
        {
            AutoResetEvent ev = new AutoResetEvent(false);
            MenuItemUtils.RunCoroutineSync(_SetImage(img, ev), ev);
        }

        private IEnumerator _SetImage(Sprite img, AutoResetEvent ev)
        {
            _ctrl.SetImage(img);
            ev.Set();
            yield return null;
        }

        public void SetGif(Sprite[] frames, int fps)
        {
            AutoResetEvent ev = new AutoResetEvent(false);
            MenuItemUtils.RunCoroutineSync(_SetGif(frames, fps, ev), ev);
        }
        private IEnumerator _SetGif(Sprite[] frames, int fps, AutoResetEvent ev)
        {
            _ctrl.SetGif(frames, fps);
            ev.Set();
            yield return null;
        }

        private IEnumerator CreateGameObject(AutoResetEvent ev)
        {
            GameObject prefab = Resources.Load<GameObject>(PREFAB_PATH);
            if (prefab == null)
            {
                throw new MenuItemException("MenuItemImage's prefab could not be found.");
            }
            prefab.SetActive(false);
            _gameObject = (GameObject)GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            _gameObject.name = _info.Name;
            _ctrl = _gameObject.GetComponent<MenuItemImageController>();
            ev.Set();
            yield return null;
        }

        public void Dispose()
        {
            GameObject.Destroy(_gameObject);
        }

        public List<IMenuItem> GetChildren()
        {
            return null;
        }

        public GameObject GetGameObject()
        {
            return _gameObject == null ? null : _gameObject;
        }

        public MenuItemInfo GetInfo()
        {
            return _info;
        }

        public IMenuItem RemoveChild(IMenuItem child)
        {
            return null;
        }

        public void SetRectTransform(Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot)
        {
            AutoResetEvent ev = new AutoResetEvent(false);
            MenuItemUtils.RunCoroutineSync(_SetRectTransform(anchorMin, anchorMax, pivot, ev), ev);
        }

        private IEnumerator _SetRectTransform(Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, AutoResetEvent ev)
        {
            RectTransform local = _gameObject.GetComponent<RectTransform>();
            this.SetRectTransform(local, anchorMin, anchorMax, pivot);
            ev.Set();
            yield return null;
        }

        public void SetPadding(float left, float top, float right, float bottom)
        {
            AutoResetEvent ev = new AutoResetEvent(false);
            MenuItemUtils.RunCoroutineSync(_SetPadding(left, top, right, bottom, ev), ev);
        }

        private IEnumerator _SetPadding(float left, float top, float right, float bottom, AutoResetEvent ev)
        {
            RectTransform rect = _gameObject.GetComponent<RectTransform>();
            rect.SetLTRB(left, top, right, bottom);
            ev.Set();
            yield return null;
        }

        public void SetPadding(float horizontal, float vertical)
        {
            SetPadding(horizontal, vertical, horizontal, vertical);
        }
    }
}
