using Assets.Scripts.Core.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Core.Applications.MenuApp.MenuItems
{
    class MenuItemImage : IMenuItem, IGOReadable
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
            CreateGameObject();
        }
        
        public void SetImage(Sprite img)
        {
            _ctrl.SetImage(img);
        }

        public void SetGif(Sprite[] frames, int fps)
        {
            _ctrl.SetGif(frames, fps);
        }
        
        private void CreateGameObject()
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
            return _gameObject;
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
            RectTransform local = _gameObject.GetComponent<RectTransform>();
            this.SetRectTransform(local, anchorMin, anchorMax, pivot);
        }

        public void SetPadding(float left, float top, float right, float bottom)
        {
            RectTransform rect = _gameObject.GetComponent<RectTransform>();
            rect.SetLTRB(left, top, right, bottom);
        }

        public void SetPadding(float horizontal, float vertical)
        {
            SetPadding(horizontal, vertical, horizontal, vertical);
        }
    }
}
