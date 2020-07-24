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
    class MenuItemCheckbox : IMenuItemCheckbox
    {
        public event Action<bool> ValueChanged;

        private const string PREFAB_PATH = "MenuItems/MenuItemCheckbox";
        private const MenuItemType _type = MenuItemType.MI_CHECKBOX;
        private MenuItemInfo _info;
        private GameObject _gameObject = null;
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
            AutoResetEvent ev = new AutoResetEvent(false);
            MenuItemUtils.RunCoroutineSync(CreateGameObject(ev), ev);
        }

        public bool IsChecked()
        {
            return _toggle.isOn;
        }

        public void SetValue(bool val)
        {
            AutoResetEvent ev = new AutoResetEvent(false);
            MenuItemUtils.RunCoroutineSync(_SetValue(val, ev), ev);
        }

        private IEnumerator _SetValue(bool val, AutoResetEvent ev)
        {
            _toggle.isOn = val;
            ev.Set();
            yield return null;
        }

        public void SetText(string text)
        {
            AutoResetEvent ev = new AutoResetEvent(false);
            MenuItemUtils.RunCoroutineSync(_SetText(text, ev), ev);
        }

        private IEnumerator _SetText(string text, AutoResetEvent ev)
        {
            _text.text = text;
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
                throw new MenuItemException("MenuItemCheckbox's prefab could not be found.");
            }
            prefab.SetActive(false);
            _gameObject = (GameObject)GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            _gameObject.name = _info.Name;
            _toggle = _gameObject.GetComponent<Toggle>();
            _toggle.onValueChanged.AddListener(OnClick);
            _text = _gameObject.transform.Find("Label").GetComponent<Text>();
            ev.Set();
            yield return null;
        }

        public void OnClick(bool val)
        {
            ValueChanged?.Invoke(val);
        }
    }
}
