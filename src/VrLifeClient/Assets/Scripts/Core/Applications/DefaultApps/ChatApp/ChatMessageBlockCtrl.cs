using Assets.Scripts.Core.Applications.MenuApp.MenuItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using VrLifeClient.API.OpenAPI;
using VrLifeShared.Core.Applications.DefaultApps.ChatApp;
using VrLifeShared.Core.Applications.DefaultApps.ChatApp.NetworkingModels;

namespace Assets.Scripts.Core.Applications.DefaultApps.ChatApp
{
    class ChatMessageBlockCtrl : IDisposable
    {
        private const int HEADER_FONT_SIZE = 10;
        private const int FONT_SIZE = 10;
        private const int CHARS_PER_LINE = 50;
        private OpenAPI _api;
        private MenuItemGrid _root;
        private ChatObj _currChat = null;
        private MenuItemScrollable _chatItem;
        private MenuItemGrid _bottomBlock;
        private MenuItemButton _sendButton;
        private MenuItemInput _msgInput;
        private MenuItemText _headerItem;
        private ulong _userId;
        private ulong _toUserId;
        private string _toUsername;

        public delegate void MessageSendEventHandler(ulong to, string msg);
        public event MessageSendEventHandler MessageSend;

        public ChatMessageBlockCtrl(OpenAPI api)
        {
            _api = api;
            if(!_api.User.UserId.HasValue)
            {
                throw new ChatAppException("You must be logged in to use chat app.");
            }
            _userId = _api.User.UserId.Value;
            InitMenuItems();
        }

        public void InitMenuItems()
        {
            _root = new MenuItemGrid("chatBlock", 1, 10);
            _headerItem = new MenuItemText("headerChat");
            _root.AddChild(0, 0, 1, 1, _headerItem);
            _headerItem.SetText("None");
            _headerItem.SetFontSize(2, 20);
            _headerItem.SetAlignment(UnityEngine.TextAnchor.MiddleLeft);
            _chatItem = new MenuItemScrollable("chatScrollable", UnityEngine.TextAnchor.LowerCenter);
            _root.AddChild(0, 1, 1, 7, _chatItem);
            _bottomBlock = new MenuItemGrid("bottomPartChatBlock", 5, 1);
            _root.AddChild(0, 8, 1, 2, _bottomBlock);
            _msgInput = new MenuItemInput("chatMsgInput");
            _bottomBlock.AddChild(0, 0, 4, 1, _msgInput);
            _msgInput.SetPlaceholder("Message...");
            _msgInput.SetLineType(UnityEngine.UI.InputField.LineType.MultiLineSubmit);
            _msgInput.onSubmit += OnSendClicked;
            _sendButton = new MenuItemButton("chatSendButton");
            _bottomBlock.AddChild(4, 0, 1, 1, _sendButton);
            _sendButton.SetText("Send");
            _sendButton.Clicked += OnSendClicked;
        }

        public IMenuItem GetRoot()
        {
            return _root;
        }

        public void OpenChat(ChatObj chat)
        {
            _currChat = chat;
            _toUserId = chat.User1 == _userId ? chat.User2 : chat.User1;
            _toUsername = _api.DefaultApps.Friends.ListFriends().Wait().Find(x => x.UserId == _toUserId)?.Username;
            _headerItem.SetText(_toUsername);
            if (_toUsername == null)
            {
                _toUsername = "[unknown]";
            }
            Rerender();
        }

        private void Rerender()
        {
            if(_currChat == null)
            {
                return;
            }
            _chatItem.GetChildren().ForEach(x => { _chatItem.RemoveChild(x); x.Dispose(); });
            foreach(ChatObjMsg msg in _currChat.Messages)
            {
                _chatItem.AddChildBottom(CreateMessageObj(msg), CalculateHeight(msg.Message));
            }
        }

        public void Update(List<ChatObjMsg> msgs)
        {
            if(_currChat == null)
            {
                return;
            }
            foreach(ChatObjMsg msg in msgs.Where(x => x.From == _toUserId))
            {
                _chatItem.AddChildBottom(CreateMessageObj(msg), CalculateHeight(msg.Message));
            }
        }

        public IMenuItem CreateMessageObj(ChatObjMsg msg)
        {
            int lines = (int)Math.Ceiling(msg.Message.Length / (float)CHARS_PER_LINE) + 1;
            MenuItemGrid item = new MenuItemGrid(msg.Time.ToString(), 1, lines);
            MenuItemText header = new MenuItemText("header");
            item.AddChild(0, 0, 1, 1, header);
            header.SetTextStyle(FontStyle.Bold);
            header.SetFontSize(5, HEADER_FONT_SIZE);
            TextAnchor alignment;
            if(msg.From == _userId)
            {
                alignment = TextAnchor.MiddleRight;
                header.SetText("You");
            }
            else
            {
                alignment = TextAnchor.MiddleLeft;
                header.SetText(_toUsername);
            }
            header.SetAlignment(alignment);
            string text = msg.Message;
            int i;
            string textPart;
            MenuItemText message;
            for (i = 0; i < lines - 2; ++i)
            {
                textPart = text.Substring(i * CHARS_PER_LINE, CHARS_PER_LINE);
                message = new MenuItemText($"message_line{i+1}");
                message.SetFontSize(5, FONT_SIZE);
                message.SetText(textPart);
                message.SetAlignment(alignment);
                item.AddChild(0, i + 1, 1, 1, message);
            }
            textPart = text.Substring(i * CHARS_PER_LINE);
            message = new MenuItemText($"message_line{i + 1}");
            message.SetFontSize(5, FONT_SIZE);
            message.SetText(textPart);
            message.SetAlignment(alignment);
            item.AddChild(0, i + 1, 1, 1, message);
            return item;
        }

        private void OnSendClicked()
        {
            if (_currChat == null)
            {
                return;
            }
            string msg = _msgInput.GetText();
            _msgInput.SetText("");
            if(msg.Trim() == "")
            {
                return;
            }
            ulong to = _currChat.User1 == _userId ? _currChat.User2 : _currChat.User1;
            MessageSend?.Invoke(to, msg);
            ChatObjMsg msgObj = new ChatObjMsg((ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), 
                _userId, to, msg);
            _currChat.AddMessage(msgObj);
            _chatItem.AddChildBottom(CreateMessageObj(msgObj), CalculateHeight(msg));
        }

        private float CalculateHeight(string msg)
        {
            return (int)Math.Ceiling(msg.Length / (float)CHARS_PER_LINE) * FONT_SIZE + HEADER_FONT_SIZE;
        }

        public void Dispose()
        {
            _root.Dispose();
        }
    }
}
