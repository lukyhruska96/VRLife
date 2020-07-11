using Assets.Scripts.Core.Applications.MenuApp.MenuItems;
using Assets.Scripts.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.Networking.PlayerConnection;
using VrLifeClient.API;
using VrLifeShared.Core.Applications.DefaultApps.FriendsApp;

namespace Assets.Scripts.Core.Applications.DefaultApps.FriendsManagementApp
{
    class FriendsList : IFriendsManagementView
    {
        private const int ROWS = 6;
        private List<FriendsAppUser> _friends = null;
        private MenuItemGrid _root;
        private OpenAPI _api;
        private const string LOADING_PATH = "Gifs/loading";

        public FriendsList(OpenAPI api)
        {
            _api = api;
            InitMenuItems();
        }

        private void InitMenuItems()
        {
            _root = new MenuItemGrid("FriendsList", 5, ROWS + 1);
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

            _friends = _api.Apps.Friends.ListFriends().Wait();

            _root.Clear();
            loading.Dispose();

            RenderPage(0);
        }

        private void RenderPage(int pageNum)
        {
            if (_friends == null)
            {
                Refresh();
            }
            _root.GetChildren().ForEach(x => x.Dispose());
            _root.Clear();
            for (int i = 0; i <= ROWS; ++i)
            {
                if (pageNum * ROWS + i >= _friends.Count)
                {
                    break;
                }
                FriendsAppUser friend = _friends[pageNum * ROWS + i];
                MenuItemText username = new MenuItemText($"username{friend.UserId}");
                username.SetText(friend.Username);
                _root.AddChild(0, i, 3, 1, username);
                if(friend.CurrentRoomId.HasValue)
                {
                    MenuItemButton connectBtn = new MenuItemButton($"connect{friend.UserId}");
                    connectBtn.SetText("Connect");
                    connectBtn.Clicked += () =>
                    {
                        _api.Apps.RoomList.ChangeRoom(friend.CurrentRoomId.Value);
                    };
                    _root.AddChild(3, i, 1, 1, connectBtn);
                }
                MenuItemButton removeFriend = new MenuItemButton($"remove{friend.UserId}");
                removeFriend.SetText("Remove");
                removeFriend.Clicked += () =>
                {
                    _api.Apps.Friends.RemoveFriend(friend.UserId).Wait();
                    Refresh();
                };
                _root.AddChild(4, i, 1, 1, removeFriend);
            }
            if (pageNum > 0)
            {
                MenuItemButton prevPage = new MenuItemButton("prevPage");
                prevPage.SetText("<");
                prevPage.Clicked += () =>
                {
                    RenderPage(pageNum - 1);
                };
                _root.AddChild(0, ROWS, 1, 1, prevPage);
            }
            if ((pageNum + 1) * ROWS < _friends.Count)
            {
                MenuItemButton nextPage = new MenuItemButton("nextPage");
                nextPage.SetText(">");
                nextPage.Clicked += () =>
                {
                    RenderPage(pageNum + 1);
                };
                _root.AddChild(4, ROWS, 1, 1, nextPage);
            }
        }

        private Sprite[] GetLoadingGif()
        {
            Sprite[] objs = Resources.LoadAll<Sprite>(LOADING_PATH);
            return objs.Cast<Sprite>().ToArray();
        }
    }
}
