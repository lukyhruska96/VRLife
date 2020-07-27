using System.Collections.Generic;
using VrLifeAPI;
using VrLifeAPI.Client.API.ObjectAPI;
using VrLifeAPI.Client.API.OpenAPI;
using VrLifeAPI.Client.Applications;
using VrLifeAPI.Client.Applications.BackgroundApp;
using VrLifeAPI.Client.Applications.GlobalApp;
using VrLifeAPI.Client.Applications.MenuApp;
using VrLifeAPI.Client.Applications.ObjectApp;
using VrLifeAPI.Client.Core.Services;
using VrLifeClient.Core.Services.AppService;

namespace Assets.Scripts.API.OpenAPI
{
    class AppAPI : IAppAPI
    {
        private IAppServiceClient _service;
        public AppAPI(IAppServiceClient service)
        {
            _service = service;
        }

        public IServiceCallback<byte[]> SendAppMsg(AppInfo app, byte[] data, AppMsgRecipient recipient)
        {
            return _service.SendAppMsg(app, data, recipient);
        }

        public List<IMenuApp> MenuApps { get => _service.MenuApps; }
        public List<IGlobalApp> GlobalApps { get => _service.GlobalApps; }
        public List<IBackgroundApp> BackgroundApps { get => _service.BackgroundApps; }
        public List<IObjectApp> ObjectApps { get => _service.ObjectApps; }
        public Dictionary<ulong, IApplication> AllApps { get => _service.AllApps; }
    }
}
