using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.API.Forwarder;
using VrLifeServer.Applications;
using VrLifeShared.Core.Applications;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeServer.Core.Applications.DefaultApps.AppManager.Forwarder
{
    class AppManagerForwarder : IApplicationForwarder
    {
        public const ulong APP_ID = 0;
        private const string NAME = "App Manager";
        private const string DESC = "App Manager for every loaded application. Storing all their instances.";
        private ClosedAPI _api;
        private AppInfo _info = new AppInfo(APP_ID, NAME, DESC, AppType.APP_BACKGROUND);

        public void Dispose()
        {

        }

        public AppInfo GetInfo()
        {
            return _info;
        }

        public byte[] HandleEvent(EventDataMsg eventData, MsgContext context)
        {
            throw new NotImplementedException();
        }

        public AppMsg HandleMessage(byte[] data, int size, MsgContext context)
        {
            throw new NotImplementedException();
        }

        public void Init(OpenAPI api)
        {
            _api = api.GetClosedAPI(_info);
        }
    }
}
