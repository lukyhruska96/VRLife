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
    class MenuItemButton : IMenuItem, IGOReadable
    {
        private const string PREFAB_PATH = "MenuItems/MenuItemButton";
        private const MenuItemType _type = MenuItemType.MI_BUTTON;
        private MenuItemInfo _info;
        private GameObject _gameObject;
        private Text _text;

        public delegate void ClickEventHandler();
        public event ClickEventHandler Clicked;

        public MenuItemButton(string name)
        {
            _info = new MenuItemInfo
            {
                Type = _type,
                Name = name,
                Parent = null
            };
            CreateGameObject();
        }

        private void CreateGameObject()
        {
            GameObject prefab = Resources.Load<GameObject>(PREFAB_PATH);
            if (prefab == null)
            {
                throw new MenuItemException("MenuItemButton's prefab could not be found.");
            }
            prefab.SetActive(false);
            _gameObject = (GameObject)GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            _gameObject.name = _info.Name;
            _text = _gameObject.transform.Find("Text").GetComponent<Text>();
            _gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
        }

        public void SetText(string text)
        {
            _text.text = text;
        }

        public void SetBgColor(Color color)
        {
            Image img = _gameObject.GetComponent<Image>();
            img.color = color;
        }

        public void SetTextColor(Color color)
        {
            _text.color = color;
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
            RectTransform local = _gameObject.GetComponent<RectTransform>();
            this.SetRectTransform(local, anchorMin, anchorMax, pivot);
        }

        private void OnClick()
        {
            Clicked();
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
