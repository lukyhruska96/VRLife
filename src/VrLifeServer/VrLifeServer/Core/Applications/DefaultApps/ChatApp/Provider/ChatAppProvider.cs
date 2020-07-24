using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.API.Provider;
using VrLifeServer.Core.Services.AppService;
using VrLifeShared.Core.Applications;
using VrLifeShared.Core.Applications.DefaultApps.ChatApp;
using VrLifeShared.Core.Applications.DefaultApps.ChatApp.NetworkingModels;
using VrLifeServer.Core.Applications.DefaultApps.VoiceChatApp.Forwarder;
using VrLifeServer.Core.Services.UserService;
using System.Linq;
using Google.Protobuf;
using System.Diagnostics;
using VrLifeServer.Core.Services.EventService;
using System.Threading.Tasks;
using System.Threading;
using VrLifeAPI.Provider.Core.Applications.DefaultApps.ChatApp;
using VrLifeAPI;
using VrLifeAPI.Networking.NetworkingModels;
using VrLifeAPI.Common.Core.Applications;
using VrLifeAPI.Provider.API;
using VrLifeAPI.Provider.Core.Services.AppService;
using VrLifeAPI.Common.Core.Services;

namespace VrLifeServer.Core.Applications.DefaultApps.ChatApp.Provider
{
    class ChatAppProvider : IChatAppProvider
    {
        public const ulong APP_ID = 4;
        private const string NAME = "ChatApp";
        private const string DESC = "Default application for chatting with all your friends.";
        private AppInfo _info = new AppInfo(APP_ID, NAME, DESC, new AppVersion(new int[] { 1, 0, 0 }), AppType.APP_MENU);
        private Dictionary<ulong, Dictionary<ulong, ChatObj>> _chats = new Dictionary<ulong, Dictionary<ulong, ChatObj>>();
        private List<ChatObj> _allChats = new List<ChatObj>();
        private Dictionary<ulong, Queue<ChatObjMsg>> _newMsgs = new Dictionary<ulong, Queue<ChatObjMsg>>();

        private ChatAppData _chatAppData;
        private IOpenAPI _api;

        public void Dispose()
        {
            Backup();
        }

        public AppInfo GetInfo()
        {
            return _info;
        }

        public byte[] HandleEvent(EventDataMsg eventData, MsgContext ctx)
        {
            if (ctx.senderType != SenderType.USER)
            {
                throw new ChatAppException("Server can not use this app.");
            }
            ulong? userId = _api.User.ClientId2UserId(ctx.senderId);
            if (!userId.HasValue)
            {
                throw new ChatAppException("You must be signedIn to add some friends.");
            }
            switch ((ChatAppEventType)eventData.EventType)
            {
                case ChatAppEventType.GET_UPDATE:
                    return GetMsgUpdate(userId.Value);
                default:
                    throw new ChatAppException("Unknown event type.");
            }
        }

        public byte[] HandleMessage(byte[] data, int size, MsgContext ctx)
        {
            if (ctx.senderType != SenderType.USER)
            {
                throw new ChatAppException("Server can not use this app.");
            }
            ChatAppMsg appMsg = ChatAppMsg.Parser.ParseFrom(data);
            if(appMsg == null)
            {
                throw new ChatAppException("Unknown message format.");
            }
            ChatRequest request = appMsg.Request;
            if(request == null)
            {
                throw new ChatAppException("Invalid message type.");
            }
            ulong? userId = _api.User.ClientId2UserId(ctx.senderId);
            if (!userId.HasValue)
            {
                throw new ChatAppException("You must be signedIn to add some friends.");
            }
            switch (request.RequestTypeCase)
            {
                case ChatRequest.RequestTypeOneofCase.SendRequest:
                    return SendMessage(userId.Value, request.SendRequest.To, request.SendRequest.Message, ctx);
                case ChatRequest.RequestTypeOneofCase.ListRequest:
                    return ListRequest(userId.Value, ctx);
                default:
                    throw new ChatAppException("Unknown request type.");

            }
        }

        private void InitChat(ulong from, ulong to)
        {
            if (!_chats.ContainsKey(from))
            {
                _chats.Add(from, new Dictionary<ulong, ChatObj>());
            }
            if (!_chats.ContainsKey(to))
            {
                _chats.Add(to, new Dictionary<ulong, ChatObj>());
            }
            if (!_chats[from].ContainsKey(to))
            {
                ChatObj newChat = new ChatObj(Math.Min(from, to), Math.Max(from, to));
                _chats[from].Add(to, newChat);
                _chats[to].Add(from, newChat);
                _allChats.Add(newChat);
            }
        }

        private byte[] SendMessage(ulong from, ulong to, string message, MsgContext ctx)
        {
            InitChat(from, to);
            ChatObj chat = _chats[from][to];
            ChatObjMsg msg = new ChatObjMsg((ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), 
                from, to, message);
            lock(chat)
            {
                chat.Messages.Add(msg);
                AddToQueue(to, msg);
            }
            return null;
        }

        private byte[] ListRequest(ulong userId, MsgContext ctx)
        {
            ChatAppMsg msg = new ChatAppMsg();
            msg.List = new ChatList();
            if(_chats.ContainsKey(userId))
            {
                msg.List.List.AddRange(_chats[userId].Select(x => x.Value.ToNetworkModel()));
                if(_newMsgs.TryGetValue(userId, out Queue<ChatObjMsg> updates))
                {
                    updates.Clear();
                }
            }
            return msg.ToByteArray();
        }

        private byte[] GetMsgUpdate(ulong userId)
        {
            ChatUpdateMsg chatUpdate = new ChatUpdateMsg();
            if(_newMsgs.ContainsKey(userId))
            {
                chatUpdate.Messages.AddRange(_newMsgs[userId].Select(x => x.ToNetworkModel()));
                if (_newMsgs.TryGetValue(userId, out Queue<ChatObjMsg> updates))
                {
                    updates.Clear();
                }
            }
            return chatUpdate.ToByteArray();
        }

        public void Init(IOpenAPI api, IAppDataService appDataService, IAppDataStorage dataStorage)
        {
            _api = api;
            _chatAppData = new ChatAppData(appDataService);
            LoadChats();
            InitCleanup();
        }

        private void Backup()
        {
            _chatAppData.BackupMsgs(_allChats);
        }

        private void LoadChats()
        {
            List<ChatObj> chats = _chatAppData.LoadChats();
            _chats.Clear();
            _allChats.Clear();
            foreach(ChatObj obj in chats)
            {
                InitChat(obj.User1, obj.User2);
                _chats[obj.User1][obj.User2].CopyChat(obj);
            }
        }

        private void InitCleanup()
        {
            DateTimeOffset todayStart = new DateTimeOffset(DateTime.Today);
            if (_allChats.Count != 0)
            {
                if(_allChats.First().LatestUpdate < (ulong)todayStart.ToUnixTimeMilliseconds())
                {
                    Cleanup();
                }
            }
            Task t = new Task(() => {
                DateTimeOffset lastReset = todayStart;
                DateTimeOffset nextReset = lastReset + TimeSpan.FromDays(1);
                while (true)
                {
                    Thread.Sleep((int)(nextReset - DateTimeOffset.UtcNow).TotalMilliseconds);
                    Cleanup();
                    lastReset = nextReset;
                    nextReset = lastReset + TimeSpan.FromDays(1);
                }
            }, TaskCreationOptions.LongRunning);
            t.Start();
        }

        private void Cleanup()
        {
            _chatAppData.ClearMsgs();
            _allChats.Clear();
            _chats.Clear();
            _newMsgs.Clear();
        }

        private void AddToQueue(ulong recipient, ChatObjMsg msg)
        {
            if (!_newMsgs.ContainsKey(recipient))
            {
                _newMsgs[recipient] = new Queue<ChatObjMsg>();
            }
            _newMsgs[recipient].Enqueue(msg);
        }
    }
}
