using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using VrLifeShared.Core.Applications.DefaultApps.ChatApp.NetworkingModels;

namespace VrLifeShared.Core.Applications.DefaultApps.ChatApp
{
    public struct ChatObjMsg
    {
        public ulong Time { get; private set; }
        public ulong To { get; private set; }
        public ulong From { get; private set; }
        public string Message { get; private set; }

        public ChatObjMsg(ulong time, ulong from, ulong to, string msg)
        {
            Time = time;
            From = from;
            To = to;
            Message = msg;
        }

        public ChatObjMsg(JObject obj)
        {
            Time = obj["time"].Value<ulong>();
            To = obj["to"].Value<ulong>();
            From = obj["from"].Value<ulong>();
            Message = obj["message"].Value<string>();
        }

        public ChatObjMsg(ChatMessage msg)
        {
            Time = msg.Time;
            From = msg.From;
            To = msg.To;
            Message = msg.Message;
        }

        public ChatMessage ToNetworkModel()
        {
            ChatMessage msg = new ChatMessage();
            msg.Time = Time;
            msg.From = From;
            msg.To = To;
            msg.Message = Message;
            return msg;
        }
    }
}
