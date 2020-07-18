using Assets.Scripts.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Core.Applications.MenuApp.MenuItems
{
    class MenuItemScrollable : IMenuItem, IGOReadable
    {
        private const string PREFAB_PATH = "MenuItems/MenuItemScrollable";
        private const MenuItemType _type = MenuItemType.MI_SCROLLABLE;
        private Dictionary<string, (GameObject, IMenuItem)> _items = 
            new Dictionary<string, (GameObject, IMenuItem)>();
        private MenuItemInfo _info;
        private GameObject _gameObject;
        private ScrollRect _scroll;
        private GameObject _dataView;
        private VerticalLayoutGroup _layout;

        public delegate void ScrollValueChangeEventHandler(float val);
        public event ScrollValueChangeEventHandler ScrollValueChanged;

        public MenuItemScrollable(string name, TextAnchor layout)
        {
            _info = new MenuItemInfo
            {
                Type = _type,
                Name = name,
                Parent = null
            };
            CreateGameObject();
            if((int)layout < 3)
            {
                _dataView.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1);
            }
            else if((int)layout < 6)
            {
                _dataView.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            }
            else
            {
                _dataView.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0);
            }
        }

        public void Dispose()
        {
            foreach (var item in _items.Values)
            {
                item.Item2.Dispose();
            }
            GameObject.Destroy(_gameObject);
        }

        public List<IMenuItem> GetChildren()
        {
            return _items.Values.Select(x => x.Item2).ToList();
        }

        public GameObject GetGameObject()
        {
            return _gameObject == null ? null : _gameObject;
        }

        public MenuItemInfo GetInfo()
        {
            return _info;
        }

        public void Clear()
        {
            List<IMenuItem> items = GetChildren();
            foreach(IMenuItem item in items)
            {
                RemoveChild(item);
            }
        }

        public void AddChildTop(IMenuItem child, float height)
        {
            AddChild(child, height, 0);
        }

        public void AddChildBottom(IMenuItem child, float height)
        {
            AddChild(child, height, _items.Count);
        }

        public void AddChild(IMenuItem child, float height, int idx)
        {
            if(child == null)
            {
                throw new ArgumentException("Item can not be null.");
            }
            MenuItemInfo info = child.GetInfo();
            if (_items.ContainsKey(info.Name))
            {
                throw new MenuItemException("Name must be unique inside same layer.");
            }
            if(idx > _items.Count)
            {
                throw new MenuItemException("Index out of range.");
            }
            GameObject go = ((IGOReadable)child).GetGameObject();
            if(go == null)
            {
                throw new MenuItemException("Object was already destroyed.");
            }
            GameObject wrapper = CreateWrapper(go, height);
            wrapper.transform.SetSiblingIndex(idx);
            info.Parent = this;
            _items.Add(info.Name, (wrapper, child));
        }

        public IMenuItem RemoveChild(IMenuItem child)
        {
            if(child == null)
            {
                return null;
            }
            MenuItemInfo info = child.GetInfo();
            if(!_items.ContainsKey(info.Name) || info.Parent != this)
            {
                return null;
            }
            var item = _items[info.Name];
            GameObject go = ((IGOReadable)item.Item2).GetGameObject();
            if(go != null)
            {
                go.SetActive(false);
                go.transform.SetParent(null);
            }
            if(item.Item1 != null)
            {
                GameObject.Destroy(item.Item1);
            }
            _items.Remove(info.Name);
            return item.Item2;
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
                throw new MenuItemException("MenuItemInput's prefab could not be found.");
            }
            prefab.SetActive(false);
            _gameObject = (GameObject)GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            _gameObject.name = _info.Name;
            _scroll = _gameObject.GetComponent<ScrollRect>();
            _scroll.onValueChanged.AddListener(OnValueChanged);
            _dataView = _gameObject.transform.Find("DataView").gameObject;
            _layout = _dataView.GetComponent<VerticalLayoutGroup>();
        }

        private void OnValueChanged(Vector2 value)
        {
            ScrollValueChanged?.Invoke(value.y);
        }

        private GameObject CreateWrapper(GameObject item, float height)
        {
            GameObject obj = new GameObject();
            obj.AddComponent(typeof(RectTransform));
            obj.transform.SetParent(_dataView.transform);
            obj.GetComponent<RectTransform>()
                .SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = 
                new Vector3(obj.transform.localPosition.x, obj.transform.localPosition.y, 0);
            obj.name = "ItemWrapper";
            item.transform.SetParent(obj.transform);
            item.SetActive(true);
            item.GetComponent<RectTransform>().anchorMin = Vector2.zero;
            item.GetComponent<RectTransform>().anchorMax = Vector2.one;
            item.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            item.GetComponent<RectTransform>().offsetMax = Vector2.zero;
            item.transform.localRotation = Quaternion.identity;
            item.transform.localScale = Vector3.one;
            item.transform.localPosition =
                new Vector3(item.transform.localPosition.x, item.transform.localPosition.y, 0);
            return obj;
        }
    }
}
