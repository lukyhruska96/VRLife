using Assets.Scripts.API.HUDAPI;
using Assets.Scripts.Core.Applications.DefaultApps.FriendsApp;
using Assets.Scripts.Core.Applications.MenuApp;
using Assets.Scripts.Core.Applications.MenuApp.MenuItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VrLifeClient.API;
using VrLifeClient.API.HUDAPI;
using VrLifeClient.API.MenuAPI;
using VrLifeClient.API.OpenAPI;
using VrLifeShared.Core.Applications;
using VrLifeShared.Core.Applications.DefaultApps.FriendsApp;
using VrLifeShared.Networking.NetworkingModels;

namespace Assets.Scripts.Core.Applications.DefaultApps.FriendsManagementApp
{
    class FriendsManagementApp : IMenuApp
    {
        public const ulong APP_ID = 3;
        private const string NAME = "Friends Management";
        private const string DESC = "Provides ability manage friends in menu UI.";
        private AppInfo _info = new AppInfo(APP_ID, NAME, DESC, AppType.APP_MENU);
        private MenuAPI _menuAPI;
        private OpenAPI _api;
        private MenuItemGrid _root;
        private CurrentRoom _currentRoom;
        private FriendsList _friendsList;
        private FriendsSearch _friendSearch;
        private FriendRequests _friendRequests;
        private MenuItemButton _currentRoomButton;
        private MenuItemButton _friendsListButton;
        private MenuItemButton _friendSearchButton;
        private MenuItemButton _friendRequestsButton;
        private Color _activeButtonBg = Color.white;
        private Color _activeButtonFg = Color.black;
        private Color _inactiveButtonBg = Color.gray;
        private Color _inactiveButtonFg = Color.white;
        private IFriendsManagementView _active;

        public void Dispose()
        {
            _root.RemoveChild(_active.GetRoot());
            _root.Dispose();
            _currentRoom.GetRoot().Dispose();
            _friendsList.GetRoot().Dispose();
            _friendSearch.GetRoot().Dispose();
            _friendRequests.GetRoot().Dispose();
        }

        public AppInfo GetInfo()
        {
            return _info;
        }

        public IMenuItem GetRootItem()
        {
            return _root;
        }

        public void Init(OpenAPI api, MenuAPI menuAPI, HUDAPI hudAPI)
        {
            _api = api;
            _menuAPI = menuAPI;
            InitMenuItems();
            _root.Enabled += OnEnabled;
        }

        private void InitMenuItems()
        {
            // MAIN BODY
            _root = new MenuItemGrid("friendsManagement", 4, 7);
            _currentRoomButton = new MenuItemButton("currentRoomButton");
            _currentRoomButton.SetText("Current Room");
            _currentRoomButton.Clicked += SetCurrentRoomActive;
            _root.AddChild(0, 0, 1, 1, _currentRoomButton);
            _friendsListButton = new MenuItemButton("friendsListButton");
            _friendsListButton.SetText("My Friends");
            _friendsListButton.Clicked += SetFriendsListActive;
            _root.AddChild(1, 0, 1, 1, _friendsListButton);
            _friendSearchButton = new MenuItemButton("friendSearchButton");
            _friendSearchButton.SetText("Find Friends");
            _friendSearchButton.Clicked += SetFriendSearchActive;
            _root.AddChild(2, 0, 1, 1, _friendSearchButton);
            _friendRequestsButton = new MenuItemButton("friendRequestsButton");
            _friendRequestsButton.SetText("Friend Requests");
            _friendRequestsButton.Clicked += SetFriendRequestsActive;
            _root.AddChild(3, 0, 1, 1, _friendRequestsButton);

            _currentRoom = new CurrentRoom(_api);
            _friendRequests = new FriendRequests(_api);
            _friendsList = new FriendsList(_api);
            _friendSearch = new FriendsSearch(_api);
            SetCurrentRoomActive();
        }

        private void SetAllInactive()
        {
            SetInactive(_currentRoomButton);
            SetInactive(_friendsListButton);
            SetInactive(_friendSearchButton);
            SetInactive(_friendRequestsButton);
        }

        private void SetInactive(MenuItemButton btn)
        {
            btn.SetBgColor(_inactiveButtonBg);
            btn.SetTextColor(_inactiveButtonFg);
        }

        private void SetActive(MenuItemButton btn)
        {
            btn.SetBgColor(_activeButtonBg);
            btn.SetTextColor(_activeButtonFg);
        }

        private void SetActive(IFriendsManagementView item)
        {
            if(_active != null)
            {
                _root.RemoveChild(_active.GetRoot());
            }
            _root.AddChild(0, 1, 4, 6, item.GetRoot());
            _active = item;
        }

        private void SetCurrentRoomActive()
        {
            if(_active == _currentRoom)
            {
                return;
            }
            SetAllInactive();
            SetActive(_currentRoomButton);
            SetActive(_currentRoom);
            _currentRoom.Refresh();
        }

        private void SetFriendsListActive()
        {
            if (_active == _friendsList)
            {
                return;
            }
            SetAllInactive();
            SetActive(_friendsListButton);
            SetActive(_friendsList);
            _friendsList.Refresh();
        }

        private void SetFriendSearchActive()
        {
            if (_active == _friendSearch)
            {
                return;
            }
            SetAllInactive();
            SetActive(_friendSearchButton);
            SetActive(_friendSearch);
            _friendSearch.Refresh();
        }

        private void SetFriendRequestsActive()
        {
            if (_active == _friendRequests)
            {
                return;
            }
            SetAllInactive();
            SetActive(_friendRequestsButton);
            SetActive(_friendRequests);
            _friendRequests.Refresh();
        }

        private void OnEnabled()
        {
            _active?.Refresh();
        }

        public void HandleNotification(Notification notification)
        {

        }
    }
}
