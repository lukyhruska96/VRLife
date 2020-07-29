using Adrenak.UniMic;
using Google.Protobuf;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VrLifeAPI;
using VrLifeAPI.Client.API;
using VrLifeAPI.Client.API.GlobalAPI;
using VrLifeAPI.Client.Applications.DefaultApps.VoiceChatApp;
using VrLifeAPI.Client.Core.Services;
using VrLifeShared.Core.Applications.DefaultApps.VoiceChatApp.NetworkingModels;

namespace Assets.Scripts.Core.Applications.DefaultApps.VoiceChatApp
{
    class VoiceChatApp : IVoiceChatApp
    {
        public static readonly ulong APP_ID = 5;
        private const string NAME = "VoiceChatApp";
        private const string DESC = "Default application for room voice chat.";
        private AppInfo _info = new AppInfo(APP_ID, NAME, DESC,
            new AppVersion(new int[] { 1, 0, 0 }), AppType.APP_GLOBAL);
        private Dictionary<ulong, ulong> _lastSamples = new Dictionary<ulong, ulong>();
        private IClosedAPI _api;
        private IGlobalAPI _globalAPI;

        public void Dispose()
        {
            _api.DeviceAPI.Microphone.MicrophoneData -= OnMicrophoneData;
        }

        public AppInfo GetInfo()
        {
            return _info;
        }

        public void Init(IOpenAPI api, IGlobalAPI globalAPI)
        {
            _api = api.GetClosedAPI(_info);
            _globalAPI = globalAPI;
            _api.DeviceAPI.Microphone.MicrophoneData += OnMicrophoneData;
        }

        private void OnMicrophoneData(ulong sampleNum, float[] data)
        {
            VoiceChatMsg msg = new VoiceChatMsg();
            msg.Request = new VoiceChatRequest();
            msg.Request.SampleId = sampleNum;
            msg.Request.Data.AddRange(data);
            byte[] response = _api.Services.App
                .SendAppMsg(_info, msg.ToByteArray(), AppMsgRecipient.FORWARDER)
                .Wait();
            VoiceChatMsg responseMsg = VoiceChatMsg.Parser.ParseFrom(response);
            if(responseMsg == null)
            {
                throw new VoiceChatAppException("Could not parse voice chat response.");
            }
            VoiceChatData responseData = responseMsg.Data;
            if(responseData == null)
            {
                throw new VoiceChatAppException("Unexpected response.");
            }
            responseData.Data
                .ToList()
                .ForEach(x => ProcessResponseData(x));
        }

        private void ProcessResponseData(VoiceChatDataObj obj)
        {
            if (!_lastSamples.ContainsKey(obj.UserId))
            {
                _lastSamples.Add(obj.UserId, obj.SampleId);
            }
            else if(obj.SampleId <= _lastSamples[obj.UserId])
            {
                return;
            }
            _lastSamples[obj.UserId] = obj.SampleId;
            AudioClip clip = AudioClip.Create(obj.UserId.ToString(),
                _api.DeviceAPI.Microphone.SampleLength, 1, _api.DeviceAPI.Microphone.Frequency, false);
            clip.SetData(obj.Data.ToArray(), 0);
            _globalAPI.Players.GetAvatar(obj.UserId)?.GetSoundPlayer().Play(clip);
        }
    }
}
