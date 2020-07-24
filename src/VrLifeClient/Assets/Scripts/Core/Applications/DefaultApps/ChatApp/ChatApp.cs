using Assets.Scripts.API.HUDAPI;
using Assets.Scripts.Core.Applications.MenuApp;
using Assets.Scripts.Core.Applications.MenuApp.MenuItems;
using Google.Protobuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using VrLifeAPI;
using VrLifeAPI.Client.API;
using VrLifeAPI.Client.API.MenuAPI;
using VrLifeAPI.Client.API.OpenAPI;
using VrLifeAPI.Client.Applications.DefaultApps.ChatApp;
using VrLifeAPI.Client.Applications.MenuApp.MenuItems;
using VrLifeAPI.Client.Services;
using VrLifeAPI.Networking.NetworkingModels;
using VrLifeClient.API.HUDAPI;
using VrLifeClient.API.MenuAPI;
using VrLifeClient.API.OpenAPI;
using VrLifeClient.Core.Services.AppService;
using VrLifeClient.Core.Services.EventService;
using VrLifeShared.Core.Applications;
using VrLifeShared.Core.Applications.DefaultApps.ChatApp;
using VrLifeShared.Core.Applications.DefaultApps.ChatApp.NetworkingModels;

namespace Assets.Scripts.Core.Applications.DefaultApps.ChatApp
{
    class ChatApp : IChatApp
    {
        private const float UPDATE_INTERVAL_SEC = 2f;
        public const ulong APP_ID = 4;
        private const string LOADING_PATH = "Gifs/loading";
        private const string NAME = "ChatApp";
        private const string DESC = "Default application for chatting with all your friends.";
        private AppInfo _info = new AppInfo(APP_ID, NAME, DESC,
            new AppVersion(new int[] { 1, 0, 0 }), AppType.APP_MENU);
        private IMenuAPI _menuAPI;
        private IHUDAPI _hudAPI;
        private IOpenAPI _api;
        private MenuItemGrid _root;
        private IMenuItem _active = null;
        private ChatMessageBlockCtrl _chatBlock;
        private FriendsBlockCtrl _friendsBlock;
        private RecentBlockCtrl _recentBlock;
        private MenuItemGrid _leftPanel;
        private bool enabled = false;
        private ulong _chatCoroutine = ulong.MaxValue;
        private ulong _userId;

        private Dictionary<ulong, ChatObj> _chats = null;

        public void Dispose()
        {
            _root.Dispose();
            if(_chatCoroutine != ulong.MaxValue)
            {
                _menuAPI.StopCoroutine(_chatCoroutine);
            }
        }

        public AppInfo GetInfo()
        {
            return _info;
        }

        public IMenuItem GetRootItem()
        {
            return _root;
        }

        public void Init(IOpenAPI api, IMenuAPI menuAPI, IHUDAPI hudAPI)
        {
            _api = api;
            _menuAPI = menuAPI;
            _hudAPI = hudAPI; 
            _userId = _api.User.UserId.Value;
            _chatBlock = new ChatMessageBlockCtrl(api);
            _chatBlock.MessageSend += OnMessageSend;
            _recentBlock = new RecentBlockCtrl(api);
            _recentBlock.ChatSelected += OnChatSelected;
            _friendsBlock = new FriendsBlockCtrl(api);
            _friendsBlock.FriendSelected += OnFriendSelected;
            InitMenuItems();
            _root.Enabled += OnEnabled;
            _root.Disabled += OnDisabled;
            RenderFriends();
        }

        public void HandleNotification(Notification notification)
        {
            if(!ulong.TryParse(notification.ActionPath, out ulong userId))
            {
                return;
            }
            if(!_chats.ContainsKey(userId))
            {
                return;
            }
            _chatBlock.OpenChat(_chats[userId]);
        }
        
        private void InitChats()
        {
            _chatCoroutine = _menuAPI.StartCoroutine(ChatCoroutine());
        }

        private IEnumerator ChatCoroutine()
        {
            ChatAppMsg msg = new ChatAppMsg();
            msg.Request = new ChatRequest();
            msg.Request.ListRequest = true;
            var req = _api.App.SendAppMsg(_info, msg.ToByteArray(), AppMsgRecipient.PROVIDER);
            yield return req.WaitCoroutine();
            if(req.HasException)
            {
                throw req.Exception;
            }
            if(req.Result != null)
            {
                ChatAppMsg response = ChatAppMsg.Parser.ParseFrom(req.Result);
                if (response == null || response.List == null)
                {
                    throw new ChatAppException("Unknown response.");
                }
                _chats = response.List.List
                    .Select(x => new ChatObj(x))
                    .ToDictionary(x => x.User1 == _userId ? x.User2 : x.User1, x => x);
                yield return null;
            }
            else
            {
                _chats = new Dictionary<ulong, ChatObj>();
            }
            _recentBlock.SetChatList(_chats.Select(x => x.Value).ToList());
            yield return null;
            while (true)
            {
                EventDataMsg updateReq = new EventDataMsg();
                updateReq.AppId = APP_ID;
                updateReq.EventType = (uint)ChatAppEventType.GET_UPDATE;
                var updateReqService = _api.Event.SendEvent(updateReq, EventRecipient.PROVIDER);
                yield return updateReqService.WaitCoroutine();
                if(updateReqService.HasException)
                {
                    yield return new WaitForSeconds(UPDATE_INTERVAL_SEC);
                    continue;
                }
                ChatUpdateMsg updateMsg = ChatUpdateMsg.Parser.ParseFrom(updateReqService.Result);
                if(updateMsg == null)
                {
                    yield return new WaitForSeconds(UPDATE_INTERVAL_SEC);
                    continue;
                }
                List<ChatObjMsg> msgs = updateMsg.Messages
                    .Select(x => new ChatObjMsg(x))
                    .ToList();
                Update(msgs);
                yield return new WaitForSeconds(UPDATE_INTERVAL_SEC);
            }
        }

        private void InitMenuItems()
        {
            _root = new MenuItemGrid("chatAppGrid", 6, 1);
            MenuItemGrid leftGrid = new MenuItemGrid("leftPanelGrid", 1, 7);
            _root.AddChild(0, 0, 1, 1, leftGrid);
            _root.AddChild(1, 0, 5, 1, _chatBlock.GetRoot());
            MenuItemGrid leftPanelTabs = new MenuItemGrid("leftPanelTabs", 2, 1);
            leftGrid.AddChild(0, 0, 1, 1, leftPanelTabs);
            MenuItemText recentText = new MenuItemText("recentText");
            recentText.SetText("Recent");
            recentText.SetFontSize(5, 20);
            recentText.SetAlignment(UnityEngine.TextAnchor.MiddleCenter);
            leftPanelTabs.AddChild(0, 0, 1, 1, recentText);
            MenuItemText friendsText = new MenuItemText("friendsText");
            friendsText.SetText("Friends");
            friendsText.SetFontSize(5, 20);
            friendsText.SetAlignment(UnityEngine.TextAnchor.MiddleCenter);
            leftPanelTabs.AddChild(1, 0, 1, 1, friendsText);
            leftPanelTabs.ItemClicked += OnLeftPanelTabClick;
            _leftPanel = leftGrid;
        }

        private void OnEnabled()
        {
            enabled = true;
            if(_chatCoroutine == null)
            {
                InitChats();
            }
        }

        private void OnDisabled()
        {
            enabled = false;
        }

        private void OnLeftPanelTabClick(IMenuItem item, int x, int y)
        {
            if(x == 0)
            {
                RenderRecent();
            }
            else if(x == 1)
            {
                RenderFriends();
            }
        }

        private void SetActiveLeftPanel(IMenuItem item)
        {
            if(_active != null)
            {
                _leftPanel.RemoveChild(_active);
            }
            _leftPanel.AddChild(0, 1, 1, 6, item);
            _active = item;
        }

        private void RenderRecent()
        {
            if(_active == _recentBlock.GetRoot())
            {
                return;
            }
            SetActiveLeftPanel(_recentBlock.GetRoot());
        }

        private void RenderFriends()
        {
            if(_active == _friendsBlock.GetRoot())
            {
                return;
            }
            SetActiveLeftPanel(_friendsBlock.GetRoot());
            _friendsBlock.Update();
        }

        private void OnMessageSend(ulong to, string msg)
        {
            ChatAppMsg chatMsg = new ChatAppMsg();
            chatMsg.Request = new ChatRequest();
            chatMsg.Request.SendRequest = new ChatSend();
            chatMsg.Request.SendRequest.To = to;
            chatMsg.Request.SendRequest.Message = msg;
            _api.App.SendAppMsg(_info, chatMsg.ToByteArray(), 
                AppMsgRecipient.PROVIDER).Exec();
        }

        private void OnChatSelected(ChatObj chat)
        {
            _chatBlock.OpenChat(chat);
        }

        private void OnFriendSelected(ulong userId)
        {
            if(_chats == null)
            {
                return;
            }
            if(!_chats.ContainsKey(userId))
            {
                _chats.Add(userId, new ChatObj(_api.User.UserId.Value, userId));
            }
            _chatBlock.OpenChat(_chats[userId]);
        }

        private void Update(List<ChatObjMsg> msgs)
        {
            foreach (ChatObjMsg msg in msgs)
            {
                if(!_chats.ContainsKey(msg.From))
                {
                    _chats.Add(msg.From, new ChatObj(_userId, msg.From));
                }
                SendNotification(msg);
                _chats[msg.From].AddMessage(msg);
            }
            _chatBlock.Update(msgs);
            _recentBlock.SetChatList(_chats.Select(x => x.Value).ToList());
        }

        private void SendNotification(ChatObjMsg msg)
        {
            if(!enabled)
            {
                Notification notification = new Notification(_info, msg.Message, $"{msg.From}");
                _hudAPI.ShowNotification(notification);
            }
        }
    }
}
