using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VrLifeShared.Core.Applications.DefaultApps.ChatApp.NetworkingModels;

namespace VrLifeShared.Core.Applications.DefaultApps.ChatApp
{
    public class ChatObj
    {
        public ulong LatestUpdate { get; set; }
        public ulong User1 { get; private set; }
        public ulong User2 { get; private set; }
        public List<ChatObjMsg> Messages { get; private set; } = new List<ChatObjMsg>();

        public ChatObj(ulong user1, ulong user2)
        {
            User1 = user1;
            User2 = user2;
            LatestUpdate = 0;
        }

        public ChatObj(JObject jobj)
        {
            if (!jobj.ContainsKey("latestUpdate") || 
                !jobj.ContainsKey("user1") || 
                !jobj.ContainsKey("user2") || 
                !jobj.ContainsKey("messages"))
            {
                throw new FormatException("Invalid json format.");
            }
            User1 = jobj["user1"].Value<ulong>();
            User2 = jobj["user2"].Value<ulong>();
            LatestUpdate = jobj["latestUpdate"].Value<ulong>();
            JArray msgArray = jobj["messages"].Value<JArray>();
            foreach(JObject obj in msgArray)
            {
                Messages.Add(new ChatObjMsg(obj));
            }
        }

        public ChatObj(ChatObjectMsg message)
        {
            User1 = message.User1;
            User2 = message.User2;
            foreach(ChatMessage msg in message.Messages)
            {
                Messages.Add(new ChatObjMsg(msg.Time, msg.From, msg.To, msg.Message));
            }
        }

        public void CopyChat(ChatObj obj)
        {
            LatestUpdate = obj.LatestUpdate;
            User1 = obj.User1;
            User2 = obj.User2;
            lock(this)
            {
                Messages.Clear();
                Messages.AddRange(obj.Messages);
            }
        }

        public void AddMessage(ChatObjMsg msg)
        {
            AddMessage(msg.Time, msg.From, msg.To, msg.Message);
        }

        public void AddMessage(ulong time, ulong from, ulong to, string text)
        {
            if(time > LatestUpdate)
            {
                LatestUpdate = time;
            }
            lock(this)
            {
                Messages.Add(new ChatObjMsg(time, from, to, text));
            }
        }

        public ChatObjectMsg ToNetworkModel()
        {
            ChatObjectMsg chat = new ChatObjectMsg();
            chat.LatestUpdate = LatestUpdate;
            chat.User1 = User1;
            chat.User2 = User2;
            chat.Messages.AddRange(Messages.Select(x => x.ToNetworkModel()));
            return chat;
        }
    }
}
