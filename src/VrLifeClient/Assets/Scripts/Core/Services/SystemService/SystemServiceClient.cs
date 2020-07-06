using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor.VersionControl;
using VrLifeClient.API;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeClient.Core.Services.SystemService
{
    class SystemServiceClient : IServiceClient
    {
        private ClosedAPI _api;
        public delegate void ForwarderLostEventHandler();
        public event ForwarderLostEventHandler ForwarderLost;
        public delegate void ProviderLostEventHandler();
        public event ProviderLostEventHandler ProviderLost;

        public void HandleMessage(MainMessage msg)
        {
            throw new NotImplementedException();
        }

        public void Init(ClosedAPI api)
        {
            this._api = api;
        }

        public static bool IsErrorMsg(MainMessage msg)
        {
            return msg.MessageTypeCase == MainMessage.MessageTypeOneofCase.SystemMsg && msg.SystemMsg.SystemMsgTypeCase == SystemMsg.SystemMsgTypeOneofCase.ErrorMsg;
        }

        public void Reset()
        {
        }

        public void OnForwarderLost()
        {
            ForwarderLost?.Invoke();
        }

        public void OnProviderLost()
        {
            ProviderLost?.Invoke();
        }
    }
}
