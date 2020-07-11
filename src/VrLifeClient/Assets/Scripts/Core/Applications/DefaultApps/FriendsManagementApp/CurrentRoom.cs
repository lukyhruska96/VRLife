using Assets.Scripts.Core.Applications.MenuApp.MenuItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VrLifeClient.API;
using VrLifeShared.Networking.NetworkingModels;

namespace Assets.Scripts.Core.Applications.DefaultApps.FriendsManagementApp
{
    class CurrentRoom : IFriendsManagementView
    {
        private const int ROWS = 6;
        private OpenAPI _api;
        private MenuItemGrid _root;
        private List<ulong> _friends = null;
        private List<UserDetailMsg> _users = null;
        private const string LOADING_PATH = "Gifs/loading";
        public CurrentRoom(OpenAPI api)
        {
            _api = api;
            InitMenuItems();
        }

        public void InitMenuItems()
        {
            _root = new MenuItemGrid("CurrentRoom", 4, ROWS+1);
        }

        public MenuItemGrid GetRoot()
        {
            return _root;
        }

        public void Refresh()
        {
            _root.GetChildren().ForEach(x => x.Dispose());
            _root.Clear();
            MenuItemImage loading = new MenuItemImage("loadingGif");
            _root.AddChild(2, 2, 2, 5, loading);
            loading.SetGif(GetLoadingGif(), 12);

            _users = _api.User.CurrentRoomUsers().Wait().Users.ToList();
            _users.Remove(_users.Find(x => x.UserId == _api.User.UserId));
            _friends = _api.Apps.Friends.ListFriends().Wait().Select(x => x.UserId).ToList();

            _root.Clear();
            loading.Dispose();

            RenderPage(0);
        }

        private void RenderPage(int pageNum)
        {
            if(_friends == null || _users == null)
            {
                Refresh();
            }
            _root.GetChildren().ForEach(x => x.Dispose());
            _root.Clear();
            for(int i = 0; i <= ROWS; ++i)
            {
                if(pageNum * ROWS + i >= _users.Count)
                {
                    break;
                }
                UserDetailMsg user = _users[pageNum * ROWS + i];
                MenuItemText username = new MenuItemText($"username{user.Username}");
                username.SetText(user.Username);
                _root.AddChild(0, i, 3, 1, username);
                if (!_friends.Contains(user.UserId))
                {
                    MenuItemButton addFriend = new MenuItemButton($"addFriend{user.Username}");
                    addFriend.SetText("Add Friend");
                    addFriend.Clicked += () =>
                    {
                        _api.Apps.Friends.SendFriendRequest(user.UserId).Wait();
                        MenuItemText requestSent = new MenuItemText("requestSent");
                        _root.RemoveChild(addFriend);
                        addFriend.Dispose();
                        _root.AddChild(3, i, 1, 1, requestSent);
                    };
                    _root.AddChild(3, i, 1, 1, addFriend);
                }
            }
            if(pageNum > 0)
            {
                MenuItemButton prevPage = new MenuItemButton("prevPage");
                prevPage.SetText("<");
                prevPage.Clicked += () =>
                {
                    RenderPage(pageNum - 1);
                };
                _root.AddChild(0, ROWS, 1, 1, prevPage);
            }
            if((pageNum + 1) * ROWS < _users.Count)
            {
                MenuItemButton nextPage = new MenuItemButton("nextPage");
                nextPage.SetText(">");
                nextPage.Clicked += () =>
                {
                    RenderPage(pageNum + 1);
                };
                _root.AddChild(3, ROWS, 1, 1, nextPage);
            }
        }

        private Sprite[] GetLoadingGif()
        {
            Sprite[] objs = Resources.LoadAll<Sprite>(LOADING_PATH);
            return objs.Cast<Sprite>().ToArray();
        }
    }
}
