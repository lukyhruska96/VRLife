using System;
using VrLifeAPI;
using VrLifeAPI.Common.Core.Applications;
using VrLifeAPI.Common.Core.Services;
using VrLifeAPI.Networking.NetworkingModels;
using VrLifeAPI.Provider.API;
using VrLifeAPI.Provider.Core.Applications;
using VrLifeAPI.Provider.Core.Services.AppService;

namespace TestApp
{
    public class TestApp : IApplicationProvider
    {
        private AppInfo _info;

        public TestApp()
        {
            _info = new AppInfo(42, GetType().Name, "Super secret app.", new AppVersion(new int[] { 1, 0, 0 }), AppType.APP_MENU);
        }

        public void Dispose()
        {
        }

        public AppInfo GetInfo()
        {
            return _info;
        }

        public byte[] HandleEvent(EventDataMsg eventData, MsgContext ctx)
        {
            return null;
        }

        public byte[] HandleMessage(byte[] data, int size, MsgContext ctx)
        {
            return BitConverter.GetBytes(42);
        }

        public void Init(IOpenAPI api, IAppDataService appDataService, IAppDataStorage appDataStorage)
        {

        }
    }
}
