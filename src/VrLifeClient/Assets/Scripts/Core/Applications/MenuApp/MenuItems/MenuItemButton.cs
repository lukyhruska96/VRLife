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
using VrLifeClient.API.MenuAPI;

namespace Assets.Scripts.Core.Applications.MenuApp.MenuItems
{
    class MenuItemButton : IMenuItemButton
    {

        private const string PREFAB_PATH = "MenuItems/MenuItemButton";
        private const MenuItemType _type = MenuItemType.MI_BUTTON;
        private MenuItemInfo _info;
        private GameObject _gameObject = null;
        private Text _text;
        private Button _button;

        public event Action Clicked;

        public MenuItemButton(string name)
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

        private IEnumerator CreateGameObject(AutoResetEvent ev)
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
            _button = _gameObject.GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
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

        public void SetBgColor(Color color)
        {
            AutoResetEvent ev = new AutoResetEvent(false);
            MenuItemUtils.RunCoroutineSync(_setBgColor(color, ev), ev);
        }

        private IEnumerator _setBgColor(Color c, AutoResetEvent ev)
        {
            Image img = _gameObject.GetComponent<Image>();
            img.color = c;
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
            _text.color = color;
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

        private void OnClick()
        {
            Clicked();
        }

        public void SetPadding(float left, float top, float right, float bottom)
        {
            AutoResetEvent ev = new AutoResetEvent(false);
            MenuItemUtils.ReferenceEquals(_SetPadding(left, top, right, bottom, ev), ev);
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


        public void SetEnabled(bool status)
        {
            AutoResetEvent ev = new AutoResetEvent(false);
            MenuItemUtils.RunCoroutineSync(_SetEnabled(status, ev), ev);
        }

        private IEnumerator _SetEnabled(bool status, AutoResetEvent ev)
        {
            _button.interactable = status;
            ev.Set();
            yield return null;
        }
    }
}
