using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VrLifeAPI;
using VrLifeAPI.Common.Core.Applications;
using VrLifeAPI.Common.Core.Services;
using VrLifeAPI.Forwarder.API;
using VrLifeAPI.Forwarder.Core.Applications.VoiceChatApp;
using VrLifeAPI.Networking.NetworkingModels;
using VrLifeServer.API.Forwarder;
using VrLifeServer.Core.Services.UserService;
using VrLifeShared.Core.Applications;
using VrLifeShared.Core.Applications.DefaultApps.VoiceChatApp.NetworkingModels;

namespace VrLifeServer.Core.Applications.DefaultApps.VoiceChatApp.Forwarder
{
    class VoiceChatAppForwarder : IVoiceChatAppForwarder
    {

        public static readonly ulong APP_ID = 5;
        private const string NAME = "VoiceChatApp";
        private const string DESC = "Default application for room voice chat.";
        private AppInfo _info = new AppInfo(APP_ID, NAME, DESC, new AppVersion(new int[] { 1, 0, 0 }), AppType.APP_GLOBAL);
        private Dictionary<ulong, (ulong, float[])> _lastData = new Dictionary<ulong, (ulong, float[])>();
        private IClosedAPI _api;
        private uint _roomId;

        public void Dispose()
        {

        }

        public AppInfo GetInfo()
        {
            return _info;
        }

        public byte[] HandleEvent(EventDataMsg eventData, MsgContext ctx)
        {
            throw new NotImplementedException();
        }

        public byte[] HandleMessage(byte[] data, int size, MsgContext ctx)
        {
            if (ctx.senderType != SenderType.USER)
            {
                throw new VoiceChatAppForwarderException("Only client machine can call this event.");
            }
            ulong? userId = _api.Services.User.GetUserIdByClientId(ctx.senderId, true);
            if (!userId.HasValue)
            {
                throw new VoiceChatAppForwarderException("You must be signedIn to add some friends.");
            }
            VoiceChatMsg msg = VoiceChatMsg.Parser.ParseFrom(data);
            if(msg == null)
            {
                throw new VoiceChatAppForwarderException("Unknown request.");
            }
            VoiceChatRequest request = msg.Request;
            if(request == null)
            {
                throw new VoiceChatAppForwarderException("Wrong type of request.");
            }
            _lastData[userId.Value] = (request.SampleId, request.Data.ToArray());
            VoiceChatMsg response = new VoiceChatMsg();
            response.Data = new VoiceChatData();
            foreach(var pair in _lastData)
            {
                if(pair.Key == userId)
                {
                    continue;
                }
                VoiceChatDataObj obj = new VoiceChatDataObj();
                obj.UserId = pair.Key;
                obj.SampleId = pair.Value.Item1;
                obj.Data.AddRange(pair.Value.Item2);
                response.Data.Data.Add(obj);
            }
            return response.ToByteArray();
        }

        public void Init(uint roomId, IOpenAPI api, IAppDataStorage dataStorage)
        {
            _roomId = roomId;
            _api = api.GetClosedAPI(_info);
            _api.Services.Room.UserDisconnected += OnUserDisconnected;
        }

        public void OnUserDisconnected(ulong userId, uint roomId, string reason)
        {
            if(roomId == _roomId)
            {
                _lastData.Remove(userId);
            }
        }
    }
}
