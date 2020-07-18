using Adrenak.UniMic;
using Assets.Scripts.Core.Applications.GlobalApp;
using Assets.Scripts.Core.Wrappers;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VrLifeClient;
using VrLifeClient.API;
using VrLifeClient.API.GlobalAPI;
using VrLifeClient.API.OpenAPI;
using VrLifeShared.Core.Applications;
using VrLifeShared.Core.Applications.DefaultApps.VoiceChatApp.NetworkingModels;
using VrLifeShared.Networking.NetworkingModels;

namespace Assets.Scripts.Core.Applications.DefaultApps.VoiceChatApp
{
    class VoiceChatApp : IGlobalApp
    {
        public static readonly ulong APP_ID = 5;
        private const string NAME = "VoiceChatApp";
        private const string DESC = "Default application for room voice chat.";
        private AppInfo _info = new AppInfo(APP_ID, NAME, DESC, AppType.APP_GLOBAL);
        private Dictionary<ulong, ulong> _lastSamples = new Dictionary<ulong, ulong>();
        private ClosedAPI _api;
        private GlobalAPI _globalAPI;

        public void Dispose()
        {
            _api.DeviceAPI.Microphone.MicrophoneData -= OnMicrophoneData;
        }

        public AppInfo GetInfo()
        {
            return _info;
        }

        public void Init(OpenAPI api, GlobalAPI globalAPI)
        {
            _api = VrLifeCore.GetClosedAPI(_info);
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
                .SendAppMsg(_info, msg.ToByteArray(), 
                    VrLifeClient.Core.Services.AppService.AppMsgRecipient.FORWARDER)
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
