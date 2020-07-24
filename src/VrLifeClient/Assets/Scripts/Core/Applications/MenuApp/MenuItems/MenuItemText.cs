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
    class MenuItemText : IMenuItemText
    {

        private static readonly string PREFAB_PATH = @"MenuItems/MenuItemText";
        private static readonly MenuItemType _type = MenuItemType.MI_TEXT;
        private GameObject _gameObject = null;
        private MenuItemInfo _info;

        public MenuItemText(string name)
        {
            _info = new MenuItemInfo
            {
                Name = name,
                Type = _type,
                Parent = null
            };
            AutoResetEvent ev = new AutoResetEvent(false);
            MenuItemUtils.RunCoroutineSync(CreateGameObject(ev), ev);
        }

        public void SetText(string text)
        {
            AutoResetEvent ev = new AutoResetEvent(false);
            MenuItemUtils.RunCoroutineSync(_SetText(text, ev), ev);
        }

        private IEnumerator _SetText(string text, AutoResetEvent ev)
        {
            _gameObject.GetComponent<Text>().text = text;
            ev.Set();
            yield return null;
        }

        public void SetFontSize(int sizeMin, int sizeMax)
        {
            AutoResetEvent ev = new AutoResetEvent(false);
            MenuItemUtils.RunCoroutineSync(_SetFontSize(sizeMin, sizeMax, ev), ev);
        }

        private IEnumerator _SetFontSize(int sizeMin, int sizeMax, AutoResetEvent ev)
        {
            _gameObject.GetComponent<Text>().resizeTextMinSize = sizeMin;
            _gameObject.GetComponent<Text>().resizeTextMaxSize = sizeMax;
            ev.Set();
            yield return null;
        }

        public void SetAlignment(TextAnchor anchor)
        {
            AutoResetEvent ev = new AutoResetEvent(false);
            MenuItemUtils.RunCoroutineSync(_SetAlignment(anchor, ev), ev);
        }

        private IEnumerator _SetAlignment(TextAnchor anchor, AutoResetEvent ev)
        {
            _gameObject.GetComponent<Text>().alignment = anchor;
            ev.Set();
            yield return null;
        }

        public void SetTextColor(Color color)
        {
            AutoResetEvent ev = new AutoResetEvent(false);
            MenuItemUtils.RunCoroutineSync(_SetTextColor(color, ev), ev);
        }

        private IEnumerator _SetTextColor(Color color, AutoResetEvent ev)
        {
            _gameObject.GetComponent<Text>().color = color;
            ev.Set();
            yield return null;
        }

        public void SetTextStyle(FontStyle style)
        {
            AutoResetEvent ev = new AutoResetEvent(false);
            MenuItemUtils.RunCoroutineSync(_SetTextStyle(style, ev), ev);
        }

        private IEnumerator _SetTextStyle(FontStyle style, AutoResetEvent ev)
        {
            _gameObject.GetComponent<Text>().fontStyle = style;
            ev.Set();
            yield return null;
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

        public void Dispose()
        {
            GameObject.Destroy(_gameObject);
        }

        private IEnumerator CreateGameObject(AutoResetEvent ev)
        {
            GameObject prefab = Resources.Load<GameObject>(PREFAB_PATH);
            if (prefab == null)
            {
                throw new MenuItemException("MenuItemText's prefab could not be found.");
            }
            prefab.SetActive(false);
            _gameObject = (GameObject)GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            _gameObject.name = _info.Name;
            ev.Set();
            yield return null;
        }

        public GameObject GetGameObject()
        {
            return _gameObject == null ? null : _gameObject;
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
