using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.Core.Services.AppService;
using VrLifeShared.Core.Applications.DefaultApps.ChatApp;

namespace VrLifeServer.Core.Applications.DefaultApps.ChatApp.Provider
{
    class ChatAppData
    {
        private AppDataService _appData;
        private object _lock = new object();
        public ChatAppData(AppDataService appData)
        {
            _appData = appData;
        }

        public void BackupMsgs(List<ChatObj> chats)
        {
            ulong idx = 0;
            lock(_lock)
            {
                foreach(ChatObj chat in chats)
                {
                    ulong min = Math.Min(chat.User1, chat.User2);
                    ulong max = Math.Max(chat.User1, chat.User2);
                    JObject obj = new JObject();
                    obj.Add("user1", min);
                    obj.Add("user2", max);
                    obj.Add("latestUpdate", chat.LatestUpdate);
                    JArray messages = new JArray();
                    foreach(ChatObjMsg msg in chat.Messages)
                    {
                        JObject jmsg = new JObject();
                        jmsg.Add("time", msg.Time);
                        jmsg.Add("to", msg.To);
                        jmsg.Add("from", msg.From);
                        jmsg.Add("message", msg.Message);
                        messages.Add(jmsg);
                    }
                    obj.Add("messages", messages);
                    DataValue val = new DataValue("chat", obj.ToString());
                    _appData.UpdateOrInsert(idx++, val);
                }
            }
        }

        public List<ChatObj> LoadChats()
        {
            List<ChatObj> chats = new List<ChatObj>();
            lock(_lock)
            {
                List<DataValue> data = _appData.List("chat");
                foreach(DataValue val in data)
                {
                    JObject obj = JObject.Parse(val.StringVal);
                    if(obj == null)
                    {
                        continue;
                    }
                    chats.Add(new ChatObj(obj));
                }
            }
            return chats;
        }

        public void ClearMsgs()
        {
            _appData.Clear();
        }
    }
}
