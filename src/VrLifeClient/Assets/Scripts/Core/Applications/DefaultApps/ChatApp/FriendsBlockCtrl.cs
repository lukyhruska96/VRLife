using Assets.Scripts.Core.Applications.MenuApp.MenuItems;
using Assets.Scripts.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeClient.API.OpenAPI;
using VrLifeShared.Core.Applications.DefaultApps.FriendsApp;

namespace Assets.Scripts.Core.Applications.DefaultApps.ChatApp
{
    class FriendsBlockCtrl
    {
        private OpenAPI _api;
        private MenuItemScrollable _root;

        public delegate void FriendSelectEventHandler(ulong userId);
        public event FriendSelectEventHandler FriendSelected;
        public FriendsBlockCtrl(OpenAPI api)
        {
            _api = api;
            InitMenuItems();
            Update();
        }

        public IMenuItem GetRoot()
        {
            return _root;
        }

        private void InitMenuItems()
        {
            _root = new MenuItemScrollable("friendsBlock", UnityEngine.TextAnchor.UpperCenter);
        }

        public void Update()
        {
            _root.GetChildren().ForEach(x => { _root.RemoveChild(x); x.Dispose(); });
            List<FriendsAppUser> friends = _api.DefaultApps.Friends.ListFriends().Wait();
            foreach(FriendsAppUser friend in friends)
            {
                _root.AddChildBottom(CreateBlock(friend), 20);
            }
        }

        private IMenuItem CreateBlock(FriendsAppUser friend)
        {
            MenuItemGrid item = new MenuItemGrid(friend.Username, 5, 1);
            MenuItemText username = new MenuItemText("username");
            item.AddChild(0, 0, 4, 1, username);
            username.SetText(friend.Username);
            username.SetFontSize(8, 15);
            username.SetTextStyle(UnityEngine.FontStyle.Bold);
            MenuItemButton selectButton = new MenuItemButton("selectButton");
            item.AddChild(4, 0, 1, 1, selectButton);
            selectButton.SetText(">");
            selectButton.Clicked += () => FriendSelected?.Invoke(friend.UserId);
            return item;
        }
    }
}
