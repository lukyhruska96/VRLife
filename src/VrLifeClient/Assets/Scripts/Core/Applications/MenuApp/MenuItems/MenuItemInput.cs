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
    class MenuItemInput : IMenuItem, IGOReadable
    {
        private const string PREFAB_PATH = "MenuItems/MenuItemInput";
        private const MenuItemType _type = MenuItemType.MI_INPUT;
        private MenuItemInfo _info;
        private GameObject _gameObject;
        private InputField _input;

        public delegate void ValueChangeEventHandler(string value);
        public event ValueChangeEventHandler ValueChanged;

        public delegate void EditEndEventHandler(string value);
        public event EditEndEventHandler EditEnded;

        public MenuItemInput(string name)
        {
            _info = new MenuItemInfo
            {
                Type = _type,
                Name = name,
                Parent = null
            };
            CreateGameObject();
        }

        public void Dispose()
        {
            GameObject.Destroy(_gameObject);
        }

        public string GetText()
        {
            return _input.text;
        }

        public void SetText(string text)
        {
            _input.text = text;
        }

        public void SetCharLimit(int limit)
        {
            _input.characterLimit = limit;
        }

        public void SetPlaceholder(string text)
        {
            _input.placeholder.GetComponent<Text>().text = text;
        }

        public void SetContentType(InputField.ContentType contentType)
        {
            _input.contentType = contentType;
        }

        public void SetLineType(InputField.LineType lineType)
        {
            _input.lineType = lineType;
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

        private void CreateGameObject()
        {
            GameObject prefab = Resources.Load<GameObject>(PREFAB_PATH);
            if (prefab == null)
            {
                throw new MenuItemException("MenuItemInput's prefab could not be found.");
            }
            prefab.SetActive(false);
            _gameObject = (GameObject)GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            _gameObject.name = _info.Name;
            _input = _gameObject.GetComponent<InputField>();
            _input.onValueChanged.AddListener(OnValueChanged);
            _input.onEndEdit.AddListener(OnEditEnded);
        }

        private void OnValueChanged(string val)
        {
            ValueChanged?.Invoke(val);
        }

        private void OnEditEnded(string val)
        {
            EditEnded?.Invoke(val);
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
