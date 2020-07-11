using Assets.Scripts.Core.Applications.BackgroundApp;
using Assets.Scripts.Core.Applications.GlobalApp;
using Assets.Scripts.Core.Applications.MenuApp;
using Assets.Scripts.Core.Applications.ObjectApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeClient;
using VrLifeClient.API;
using VrLifeShared.Core;
using VrLifeShared.Core.Applications;

namespace Assets.Scripts.Core.Applications.DefaultApps.AppManager
{
    class AppManager : IBackgroundApp
    {
        public const ulong APP_ID = 0;
        private const string NAME = "App Manager";
        private const string DESC = "App Manager for every loaded application. Storing all their instances.";
        private ClosedAPI _api;
        private AppInfo _info;

        public List<IMenuApp> MenuApps { get => _api.Services.App.MenuApps; }
        public List<IGlobalApp> GlobalApps { get => _api.Services.App.GlobalApps; }
        public List<IObjectApp> ObjectApps { get => _api.Services.App.ObjectApps; }
        public List<IBackgroundApp> BackgroundApps { get => _api.Services.App.BackgroundApps; }

        public AppManager()
        {
            _info = new AppInfo(APP_ID, NAME, DESC, AppType.APP_BACKGROUND);
        }
        public void Init(OpenAPI api)
        {
            _api = VrLifeCore.GetClosedAPI(_info);
        }

        public AppInfo GetInfo()
        {
            return _info;
        }

        public void Dispose()
        {

        }
    }
}
