using Assets.Scripts.Core.Applications.MenuApp.MenuItems;
using Assets.Scripts.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeAPI.Client.API;
using VrLifeAPI.Client.Applications.DefaultApps.FriendsApp;
using VrLifeAPI.Client.Applications.MenuApp.MenuItems;
using VrLifeAPI.Common.Core.Applications.DefaultApps.FriendsApp;
using VrLifeClient.API.OpenAPI;
using VrLifeShared.Core.Applications.DefaultApps.FriendsApp;

namespace Assets.Scripts.Core.Applications.DefaultApps.ChatApp
{
    class FriendsBlockCtrl
    {
        private IOpenAPI _api;
        private MenuItemScrollable _root;

        public delegate void FriendSelectEventHandler(ulong userId);
        public event FriendSelectEventHandler FriendSelected;
        public FriendsBlockCtrl(IOpenAPI api)
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
            List<IFriendsAppUser> friends = _api.DefaultApps.Friends.ListFriends().Wait();
            foreach(IFriendsAppUser friend in friends)
            {
                _root.AddChildBottom(CreateBlock(friend), 20);
            }
        }

        private IMenuItem CreateBlock(IFriendsAppUser friend)
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
