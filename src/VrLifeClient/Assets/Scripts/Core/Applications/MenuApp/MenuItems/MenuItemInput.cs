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
    class MenuItemInput : IMenuItemInput
    {

        private const string PREFAB_PATH = "MenuItems/MenuItemInput";
        private const MenuItemType _type = MenuItemType.MI_INPUT;
        private MenuItemInfo _info;
        private GameObject _gameObject = null;
        private InputField _input;

        public event Action<string> ValueChanged;

        public event Action<string> EditEnded;

        public event Action onSubmit;

        public MenuItemInput(string name)
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
            AutoResetEvent ev = new AutoResetEvent(false);
            MenuItemUtils.RunCoroutineSync(_SetPlaceholder(text, ev), ev);
        }

        private IEnumerator _SetPlaceholder(string text, AutoResetEvent ev)
        {
            _input.placeholder.GetComponent<Text>().text = text;
            ev.Set();
            yield return null;
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

        private IEnumerator CreateGameObject(AutoResetEvent ev)
        {
            GameObject prefab = Resources.Load<GameObject>(PREFAB_PATH);
            if (prefab == null)
            {
                throw new MenuItemException("MenuItemInput's prefab could not be found.");
            }
            prefab.SetActive(false);
            _gameObject = (GameObject)GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            _gameObject.name = _info.Name;
            _gameObject.GetComponent<MenuItemInputController>().onSubmit += OnSubmit;
            _input = _gameObject.GetComponent<InputField>();
            _input.onValueChanged.AddListener(OnValueChanged);
            _input.onEndEdit.AddListener(OnEditEnded);
            ev.Set();
            yield return null;
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

        private void OnSubmit()
        {
            onSubmit?.Invoke();
        }
    }
}
