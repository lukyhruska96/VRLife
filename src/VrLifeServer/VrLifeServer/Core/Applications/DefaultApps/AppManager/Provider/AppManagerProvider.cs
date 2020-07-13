using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.API.Provider;
using VrLifeServer.Applications;
using VrLifeServer.Core.Services.AppService;
using VrLifeServer.Database.DbModels;
using VrLifeShared.Core.Applications;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeServer.Core.Applications.DefaultApps.AppManager.Provider
{
    class AppManagerProvider : IApplicationProvider
    {
        public const ulong APP_ID = 0;
        private const string NAME = "App Manager";
        private const string DESC = "App Manager for every loaded application. Storing all their instances.";
        private AppInfo _info = new AppInfo(APP_ID, NAME, DESC, AppType.APP_BACKGROUND);
        private AppDataService _appData;
        private ClosedAPI _api;
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

        public void Init(OpenAPI api, AppDataService appDataService)
        {
            _api = api.GetClosedAPI(_info);
            _appData = appDataService;
        }
    }
}
