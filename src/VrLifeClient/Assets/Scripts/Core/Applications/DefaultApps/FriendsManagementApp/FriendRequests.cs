using Assets.Scripts.Core.Applications.MenuApp.MenuItems;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VrLifeAPI.Client.API;
using VrLifeAPI.Client.Applications.DefaultApps.FriendsApp;
using VrLifeAPI.Common.Core.Applications.DefaultApps.FriendsApp;

namespace Assets.Scripts.Core.Applications.DefaultApps.FriendsManagementApp
{
    class FriendRequests : IFriendsManagementView
    {
        private const int ROWS = 6;
        private MenuItemGrid _root;
        private IOpenAPI _api;
        private const string LOADING_PATH = "Gifs/loading";
        private List<IFriendsAppUser> _requests = null;
        public FriendRequests(IOpenAPI api)
        {
            _api = api;
            InitMenuItems();
        }

        private void InitMenuItems()
        {
            _root = new MenuItemGrid("FriendRequests", 5, ROWS + 1);
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

            _requests = _api.DefaultApps.Friends.GetFriendRequests().Wait();

            _root.Clear();
            loading.Dispose();

            RenderPage(0);
        }

        private void RenderPage(int pageNum)
        {
            if (_requests == null)
            {
                Refresh();
            }
            _root.GetChildren().ForEach(x => x.Dispose());
            _root.Clear();
            for (int i = 0; i <= ROWS; ++i)
            {
                if (pageNum * ROWS + i >= _requests.Count)
                {
                    break;
                }
                IFriendsAppUser user = _requests[pageNum * ROWS + i];
                MenuItemText username = new MenuItemText($"username{user.UserId}");
                username.SetText(user.Username);
                _root.AddChild(0, i, 3, 1, username);
                MenuItemButton acceptBtn = new MenuItemButton($"accept{user.UserId}");
                acceptBtn.SetText("Accept");
                acceptBtn.Clicked += () =>
                {
                    _api.DefaultApps.Friends.AcceptFriendRequest(user.UserId).Wait();
                    Refresh();
                };
                _root.AddChild(3, i, 1, 1, acceptBtn);
                MenuItemButton rejectBtn = new MenuItemButton($"reject{user.UserId}");
                rejectBtn.SetText("Reject");
                rejectBtn.Clicked += () => {
                    _api.DefaultApps.Friends.DeleteFriendRequest(user.UserId).Wait();
                    Refresh();
                };
                _root.AddChild(4, i, 1, 1, rejectBtn);
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
            if ((pageNum + 1) * ROWS < _requests.Count)
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
