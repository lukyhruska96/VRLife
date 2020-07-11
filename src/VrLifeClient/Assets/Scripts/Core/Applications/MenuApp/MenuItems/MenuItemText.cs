using Assets.Scripts.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Core.Applications.MenuApp.MenuItems
{
    class MenuItemText : IMenuItem, IGOReadable
    {
        private static readonly string PREFAB_PATH = @"MenuItems/MenuItemText";
        private static readonly MenuItemType _type = MenuItemType.MI_TEXT;
        private GameObject _gameObject;
        private MenuItemInfo _info;

        public MenuItemText(string name)
        {
            _info = new MenuItemInfo
            {
                Name = name,
                Type = _type,
                Parent = null
            };
            CreateGameObject();
        }

        public void SetText(string text)
        {
            _gameObject.GetComponent<Text>().text = text;
        }

        public void SetFontSize(int sizeMin, int sizeMax)
        {
            _gameObject.GetComponent<Text>().resizeTextMinSize = sizeMin;
            _gameObject.GetComponent<Text>().resizeTextMaxSize = sizeMax;
        }

        public void SetAlignment(TextAnchor anchor)
        {
            _gameObject.GetComponent<Text>().alignment = anchor;
        }

        public void SetTextColor(Color color)
        {
            _gameObject.GetComponent<Text>().color = color;
        }

        public List<IMenuItem> GetChildren()
        {
            return null;
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

        public void Dispose()
        {
            GameObject.Destroy(_gameObject);
        }

        private void CreateGameObject()
        {
            GameObject prefab = Resources.Load<GameObject>(PREFAB_PATH);
            if (prefab == null)
            {
                throw new MenuItemException("MenuItemText's prefab could not be found.");
            }
            prefab.SetActive(false);
            _gameObject = (GameObject)GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            _gameObject.name = _info.Name;
        }

        public GameObject GetGameObject()
        {
            return _gameObject;
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
