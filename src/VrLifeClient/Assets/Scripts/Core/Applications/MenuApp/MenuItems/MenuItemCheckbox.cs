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
    class MenuItemCheckbox : IMenuItem, IGOReadable
    {
        public delegate void ValueChangeEventHandler(bool val);
        public event ValueChangeEventHandler ValueChanged;
        private const string PREFAB_PATH = "MenuItems/MenuItemCheckbox";
        private const MenuItemType _type = MenuItemType.MI_CHECKBOX;
        private MenuItemInfo _info;
        private GameObject _gameObject;
        private Toggle _toggle;
        private Text _text;

        public MenuItemCheckbox(string name)
        {
            _info = new MenuItemInfo
            {
                Type = _type,
                Name = name,
                Parent = null
            };
            CreateGameObject();
        }

        public bool IsChecked()
        {
            return _toggle.isOn;
        }

        public void SetValue(bool val)
        {
            _toggle.isOn = val;
        }

        public void SetText(string text)
        {
            _text.text = text;
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

        public void SetPadding(float left, float top, float right, float bottom)
        {
            RectTransform rect = _gameObject.GetComponent<RectTransform>();
            rect.SetLTRB(left, top, right, bottom);
        }

        public void SetPadding(float horizontal, float vertical)
        {
            SetPadding(horizontal, vertical, horizontal, vertical);
        }

        public void SetRectTransform(Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot)
        {
            RectTransform local = _gameObject.GetComponent<RectTransform>();
            this.SetRectTransform(local, anchorMin, anchorMax, pivot);
        }

        private void CreateGameObject()
        {
            GameObject prefab = Resources.Load<GameObject>(PREFAB_PATH);
            if (prefab == null)
            {
                throw new MenuItemException("MenuItemCheckbox's prefab could not be found.");
            }
            prefab.SetActive(false);
            _gameObject = (GameObject)GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            _gameObject.name = _info.Name;
            _toggle = _gameObject.GetComponent<Toggle>();
            _toggle.onValueChanged.AddListener(OnClick);
            _text = _gameObject.transform.Find("Label").GetComponent<Text>();
        }

        public void OnClick(bool val)
        {
            ValueChanged?.Invoke(val);
        }
    }
}
