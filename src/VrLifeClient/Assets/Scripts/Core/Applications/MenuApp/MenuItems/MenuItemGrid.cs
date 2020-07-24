using Assets.Scripts.Core.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using VrLifeAPI;
using VrLifeAPI.Client.API.MenuAPI;
using VrLifeAPI.Client.Applications.MenuApp.MenuItems;
using VrLifeClient;

namespace Assets.Scripts.Core.Applications.MenuApp.MenuItems
{
    class MenuItemGrid : IMenuItemGrid
    {

        public event Action<IMenuItem, int, int> ItemClicked;

        public event Action Enabled;

        public event Action Disabled;

        private const string PREFAB_PATH = "MenuItems/MenuItemGrid";
        private const MenuItemType _type = MenuItemType.MI_GRID;
        private MenuItemInfo _info;
        private GameObject _gameObject = null;
        private IMenuItem[,] _grid;
        private Dictionary<string, IMenuItem> _itemDict = new Dictionary<string, IMenuItem>();
        private int width, height;

        public MenuItemGrid(string name, int xNum, int yNum)
        {
            _info = new MenuItemInfo()
            {
                Name = name,
                Type = _type,
                Parent = null
            };
            if(xNum <= 0  || yNum <= 0)
            {
                throw new MenuItemException("Invalid arguments xNum or yNum.");
            }
            width = xNum;
            height = yNum;
            _grid = new IMenuItem[height, width];
            AutoResetEvent ev = new AutoResetEvent(false);
            MenuItemUtils.RunCoroutineSync(CreateGameObject(ev), ev);
        }

        public GameObject GetGameObject()
        {
            return _gameObject == null ? null : _gameObject;
        }

        public MenuItemInfo GetInfo()
        {
            return _info;
        }

        public void Dispose()
        {
            foreach(IMenuItem item in _itemDict.Values)
            {
                item.Dispose();
            }
            GameObject.Destroy(_gameObject);
            _itemDict.Clear();
            _grid = new IMenuItem[height, width];
        }

        public List<IMenuItem> GetChildren()
        {
            return _itemDict.Values.ToList();
        }

        public List<IMenuItem> Clear()
        {
            List<IMenuItem> list = _itemDict.Values.ToList();
            list.ForEach(x => RemoveChild(x));
            return list;
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

        public void AddChild(int x, int y, int colWidth, int colHeight, IMenuItem item)
        {
            if (x < 0 || y < 0 || x + colWidth - 1 >= width ||
                y + colHeight - 1 >= height || colWidth <= 0 || colHeight <= 0)
            {
                throw new ArgumentException("Coordinates out of range.");
            }
            if (item == null)
            {
                throw new ArgumentException("Item can not be null.");
            }
            if (!IsEmptyGrid(x, y, colWidth, colHeight))
            {
                throw new MenuItemException("Intersection with another menu item.");
            }
            MenuItemInfo info = item.GetInfo();
            if (_itemDict.ContainsKey(info.Name))
            {
                throw new MenuItemException("Name must be unique inside same layer.");
            }
            _itemDict.Add(info.Name, item);
            AutoResetEvent ev = new AutoResetEvent(false);
            MenuItemUtils.RunCoroutineSync(_AddChild(x, y, colWidth, colHeight, item, ev), ev);
        }

        private IEnumerator _AddChild(int x, int y, int colWidth, int colHeight, IMenuItem item, AutoResetEvent ev)
        {
            AddItemGrid(x, y, colWidth, colHeight, item);
            ((IGOReadable)item).GetGameObject().transform.SetParent(_gameObject.transform);
            item.GetInfo().Parent = this;
            (Vector2 anchorMin, Vector2 anchorMax) = GridToRect(x, y, colWidth, colHeight);
            item.SetRectTransform(
                anchorMin,
                anchorMax,
                new Vector2(0f, 0f));
            // everything must be set before OnEnable is called inside inserted prefab
            ((IGOReadable)item).GetGameObject().SetActive(true);
            ev.Set();
            yield return null;
        }

        private (Vector2 anchorMin, Vector2 anchorMax) GridToRect(int x, int y, int width, int height)
        {
            RectTransform parent = _gameObject.GetComponent<RectTransform>(); 
            Vector2 anchorMin = new Vector2((float)x / this.width, (float)(this.height - y - height) / this.height);
            Vector2 anchorMax = new Vector2((float)(x + width) / this.width, (float)(this.height - y) / this.height);
            return (anchorMin, anchorMax);
        }

        public IMenuItem RemoveChild(IMenuItem child)
        {
            MenuItemInfo info = child.GetInfo();
            if(_itemDict.ContainsKey(info.Name))
            {
                RemoveObjectInGrid(child);
                _itemDict.Remove(info.Name);
                AutoResetEvent ev = new AutoResetEvent(false);
                MenuItemUtils.RunCoroutineSync(_RemoveChild(child, ev), ev);
            }
            else
            {
                return null;
            }
            return child;
        }

        public IEnumerator _RemoveChild(IMenuItem child, AutoResetEvent ev)
        {
            yield return new WaitUntil(() => _gameObject == null);
            ((IGOReadable)child).GetGameObject()?.transform.SetParent(null);
            child.GetInfo().Parent = null;
            ev.Set();
            yield return null;
        }

        private bool IsEmptyGrid(int x, int y, int width, int height)
        {
            for(int i = y; i < y+height; ++i)
            {
                for(int j = x; j < x+width; ++j)
                {
                    if(_grid[i, j] != null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void AddItemGrid(int x, int y, int width, int height, IMenuItem item)
        {
            for (int i = y; i < y + height; ++i)
            {
                for (int j = x; j < x + width; ++j)
                {
                    _grid[i, j] = item;
                }
            }
        }

        private void RemoveObjectInGrid(IMenuItem item)
        {
            for(int i = 0; i < height; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    if(_grid[i, j] == item)
                    {
                        _grid[i, j] = null;
                    }
                }
            }
        }

        private IEnumerator CreateGameObject(AutoResetEvent ev)
        {
            GameObject prefab = Resources.Load<GameObject>(PREFAB_PATH);
            if (prefab == null)
            {
                throw new MenuItemException("MenuItemGrid's prefab could not be found.");
            }
            prefab.SetActive(false);
            _gameObject = (GameObject) GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            _gameObject.name = _info.Name;
            MenuItemGridController controller = _gameObject.GetComponent<MenuItemGridController>();
            controller.PointerClicked += OnPointerClick;
            controller.Enabled += OnEnabled;
            controller.Disabled += OnDisabled;
            ev.Set();
            yield return null;
        }

        private (int x, int y) PositionToGridIdx(Vector2 localPosition)
        {
            RectTransform rectTransform = _gameObject.GetComponent<RectTransform>();
            float width = rectTransform.rect.width;
            float height = rectTransform.rect.height;
            float columnWidth = width / this.width;
            float rowHeight = height / this.height;
            return ((int)(localPosition.x / columnWidth), (int)(localPosition.y / rowHeight));
        }

        private void OnPointerClick(Vector2 localPosition)
        {
            (int x, int y) = PositionToGridIdx(localPosition);
            if (_grid[y, x] != null) {
                Debug.Log($"x:y {x}:{y}");
                ItemClicked?.Invoke(_grid[y, x], x, y);
            }
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

        private void OnEnabled()
        {
            Enabled?.Invoke();
        }

        private void OnDisabled()
        {
            Disabled?.Invoke();
        }
    }
}
