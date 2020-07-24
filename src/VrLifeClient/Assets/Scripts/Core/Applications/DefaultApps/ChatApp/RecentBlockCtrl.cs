using Assets.Scripts.Core.Applications.MenuApp.MenuItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using VrLifeAPI.Client.API;
using VrLifeAPI.Client.Applications.DefaultApps.FriendsApp;
using VrLifeAPI.Client.Applications.MenuApp.MenuItems;
using VrLifeAPI.Common.Core.Applications.DefaultApps.FriendsApp;
using VrLifeClient.API.OpenAPI;
using VrLifeShared.Core.Applications.DefaultApps.ChatApp;
using VrLifeShared.Core.Applications.DefaultApps.ChatApp.NetworkingModels;
using VrLifeShared.Core.Applications.DefaultApps.FriendsApp;

namespace Assets.Scripts.Core.Applications.DefaultApps.ChatApp
{
    class RecentBlockCtrl
    {
        private const float BLOCK_HEIGHT = 40;
        private IOpenAPI _api;
        private MenuItemScrollable _root;
        private List<ChatObj> _chats = null;

        private const int MAX_MSG_LEN = 10;

        public delegate void ChatSelectEventHandler(ChatObj chat);
        public event ChatSelectEventHandler ChatSelected;

        public RecentBlockCtrl(IOpenAPI api)
        {
            _api = api;
            InitMenuItems();
        }

        public IMenuItem GetRoot()
        {
            return _root;
        }

        public void InitMenuItems()
        {
            _root = new MenuItemScrollable("chatRecentBlock", UnityEngine.TextAnchor.UpperCenter);
        }

        public void SetChatList(List<ChatObj> chats)
        {
            _chats = chats;
            Rerender();
        }

        public void Rerender()
        {
            if (_chats == null)
            {
                return;
            }
            List<IMenuItem> children = _root.GetChildren();
            children.ForEach(x => { _root.RemoveChild(x); x.Dispose(); });
            Dictionary<ulong, IFriendsAppUser> friends = _api.DefaultApps.Friends.ListFriends().Wait().ToDictionary(x => x.UserId, x => x);
            ulong userId = _api.User.UserId.Value;
            foreach (ChatObj chat in _chats)
            {
                if(chat.Messages.Count == 0)
                {
                    continue;
                }
                ulong otherUser = chat.User1 == userId ? chat.User2 : chat.User1;
                string username;
                if (friends.TryGetValue(otherUser, out IFriendsAppUser friend))
                {
                    username = friend.Username;
                }
                else
                {
                    username = "[unknown]";
                }
                _root.AddChildBottom(CreateUserBlock(username, chat), BLOCK_HEIGHT);
            }
        }

        private IMenuItem CreateUserBlock(string username, ChatObj chat)
        {
            MenuItemGrid item = new MenuItemGrid(username, 5, 3);
            MenuItemText usernameBlock = new MenuItemText("username");
            item.AddChild(0, 0, 4, 1, usernameBlock);
            usernameBlock.SetText(username);
            usernameBlock.SetFontSize(8, 15);
            usernameBlock.SetTextStyle(UnityEngine.FontStyle.Bold);
            MenuItemText lastMsgBlock = new MenuItemText("lastMsg");
            item.AddChild(0, 1, 4, 2, lastMsgBlock);
            string text = chat.Messages.Last().Message;
            if(text.Length > MAX_MSG_LEN)
            {
                text = text.Substring(0, MAX_MSG_LEN - 3);
                text += "...";
            }
            lastMsgBlock.SetText(text);
            lastMsgBlock.SetFontSize(5, 10);
            lastMsgBlock.SetAlignment(UnityEngine.TextAnchor.MiddleLeft);
            MenuItemButton openChat = new MenuItemButton("openChat");
            item.AddChild(4, 0, 1, 3, openChat);
            openChat.SetText(">");
            openChat.Clicked += () => ChatSelected?.Invoke(chat);
            return item;
        }
    }
}
