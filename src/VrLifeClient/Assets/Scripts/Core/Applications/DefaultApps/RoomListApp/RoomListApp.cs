using Assets.Scripts.API.MenuAPI;
using Assets.Scripts.Core.Applications.MenuApp;
using Assets.Scripts.Core.Applications.MenuApp.MenuItems;
using Assets.Scripts.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using VrLifeClient;
using VrLifeClient.API;
using VrLifeClient.Core.Services.RoomService;
using VrLifeShared.Core;
using VrLifeShared.Core.Applications;

namespace Assets.Scripts.Core.Applications.DefaultApps.RoomListApp
{
    class RoomListApp : IMenuApp
    {
        private const string LOADING_PATH = "Gifs/loading";
        private const string NAME = "RoomListApp";
        private const string DESC = "Default application for listing rooms and joining them.";
        public static readonly ulong APP_ID = 2;
        private MenuAPI _menuAPI;
        private ClosedAPI _api;
        private AppInfo _info = new AppInfo(APP_ID, NAME, DESC, AppType.APP_MENU);
        private MenuAppInfo _menuInfo = new MenuAppInfo();
        private MenuItemGrid _root;
        private MenuItemGrid _roomListGrid;
        private MenuItemImage _loading = null;

        public AppInfo GetInfo()
        {
            return _info;
        }

        public MenuAppInfo GetMenuInfo()
        {
            return _menuInfo;
        }

        public IMenuItem GetRootItem()
        {
            return _root;
        }

        public void Init(OpenAPI api, MenuAPI menuAPI)
        {
            this._menuAPI = menuAPI;
            this._api = VrLifeCore.GetClosedAPI(_info);
            CreateMenuItems();

        }

        private List<Room> GetRoomList(string contains = "", bool notEmpty = false, bool notFull = false)
        {
            List<IMenuItem> items = _roomListGrid.Clear();
            items.ForEach(x => x.Dispose());
            CreateLoadinGif();
            return _api.Services.Room.RoomList(contains, notEmpty, notFull).Wait();
        }

        private void RenderRoomList(List<Room> list)
        {
            _roomListGrid.RemoveChild(_loading);
            _loading.Dispose();
            int i = 0;
            foreach(Room r in list)
            {
                if(i >= 10)
                {
                    break;
                }
                MenuItemText roomName = new MenuItemText($"roomName{i}");
                roomName.SetText(r.Name);
                roomName.SetFontSize(5, 15);
                _roomListGrid.AddChild(0, i, 3, 1, roomName);
                MenuItemText roomCapacity = new MenuItemText($"roomCapacity{i}");
                roomCapacity.SetText($"{r.Players.Count}/{r.Capacity}");
                roomCapacity.SetFontSize(5, 15);
                _roomListGrid.AddChild(3, i, 1, 1, roomCapacity);
                MenuItemButton connect = new MenuItemButton($"roomConnect{i}");
                connect.SetText("Connect");
                connect.Clicked += () => ConnectRoom(r);
                _roomListGrid.AddChild(4, i, 2, 1, connect);
                i++;
            }
        }

        public void ChangeRoom(uint roomId)
        {
            List<Room> list = GetRoomList();
            Room r = list.Find(x => x.Id == roomId);
            if(r == null)
            {
                throw new RoomListAppException("Room with this id does not exist.");
            }
            ConnectRoom(r);
        }

        private void ConnectRoom(Room r)
        {
            _api.Services.Room.RoomExit(_api.Services.Room.CurrentRoom.Id, 
                _api.Services.Room.ForwarderAddress).Wait();
            _api.Services.Room.RoomEnter(r.Id, r.Address).Wait();
            SceneController.current.ToRoom();
        }

        private void CreateMenuItems()
        {
            _root = new MenuItemGrid("roomListGrid", 7, 7);
            MenuItemText roomText = new MenuItemText("roomText");
            roomText.SetText("Room List");
            roomText.SetTextColor(Color.black);
            roomText.SetAlignment(TextAnchor.MiddleCenter);
            _root.AddChild(0, 0, 2, 1, roomText);
            MenuItemText searchText = new MenuItemText("searchText");
            searchText.SetText("Search:");
            searchText.SetAlignment(TextAnchor.LowerLeft);
            searchText.SetFontSize(5, 7);
            _root.AddChild(0, 2, 2, 1, searchText);
            MenuItemInput searchInput = new MenuItemInput("searchInput");
            searchInput.SetPlaceholder("Name...");
            _root.AddChild(0, 3, 2, 1, searchInput);
            searchInput.SetPadding(10f, 0f, 10f, 10f);
            MenuItemCheckbox notEmpty = new MenuItemCheckbox("notEmpty");
            notEmpty.SetValue(false);
            notEmpty.SetText("Filter empty");
            _root.AddChild(0, 4, 2, 1, notEmpty);
            notEmpty.SetPadding(10f, 6f);
            MenuItemCheckbox notFull = new MenuItemCheckbox("notFull");
            notFull.SetValue(false);
            notFull.SetText("Filter full");
            _root.AddChild(0, 5, 2, 1, notFull);
            notFull.SetPadding(10f, 6f);
            MenuItemButton filter = new MenuItemButton("filter");
            filter.SetText("Filter");
            filter.Clicked += () =>
                {
                    List<Room> roomList = GetRoomList(searchInput.GetText(), notEmpty.IsChecked(), notFull.IsChecked());
                    RenderRoomList(roomList);
                };
            _root.AddChild(0, 6, 2, 1, filter);
            filter.SetPadding(20f, 4f);
            _roomListGrid = new MenuItemGrid("roomItems", 6, 10);
            _root.AddChild(2, 0, 5, 7, _roomListGrid);
        }

        private Sprite[] GetLoadingGif()
        {
            Sprite[] objs = Resources.LoadAll<Sprite>(LOADING_PATH);
            return objs.Cast<Sprite>().ToArray();
        }

        private void CreateLoadinGif()
        {
            _loading = new MenuItemImage("loading");
            _roomListGrid.AddChild(2, 2, 2, 5, _loading);
            _loading.SetGif(GetLoadingGif(), 12);
        }

        public void Dispose()
        {
            _root?.Dispose();
        }
    }
}
