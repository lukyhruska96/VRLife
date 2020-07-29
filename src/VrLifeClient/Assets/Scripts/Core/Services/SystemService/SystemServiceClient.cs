using System;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Client.API;
using VrLifeAPI.Client.Core.Services;
using VrLifeAPI.Networking.NetworkingModels;
using VrLifeClient.API;

namespace VrLifeClient.Core.Services.SystemService
{
    class SystemServiceClient : ISystemServiceClient
    {
        private IClosedAPI _api;

        public event Action ForwarderLost;

        public event Action ProviderLost;


        public void Init(IClosedAPI api)
        {
            this._api = api;
        }

        public static bool IsErrorMsg(MainMessage msg)
        {
            return msg.MessageTypeCase == MainMessage.MessageTypeOneofCase.SystemMsg && msg.SystemMsg.SystemMsgTypeCase == SystemMsg.SystemMsgTypeOneofCase.ErrorMsg;
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
