using Assets.Scripts.Core.Applications;
using Assets.Scripts.Core.Applications.BackgroundApp;
using Assets.Scripts.Core.Applications.GlobalApp;
using Assets.Scripts.Core.Applications.MenuApp;
using Assets.Scripts.Core.Applications.ObjectApp;
using Assets.Scripts.Core.Services.AppService;
using System;
using System.Collections.Generic;
using System.Text;
using VrLifeClient.API;
using VrLifeShared.Core.Applications;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeClient.Core.Services.AppService
{
    class AppServiceClient : IServiceClient
    {
        private ClosedAPI _api;
        public Dictionary<ulong, IApplication> AllApps = new Dictionary<ulong, IApplication>();
        public List<IMenuApp> MenuApps { get; private set; } = new List<IMenuApp>();
        public List<IBackgroundApp> BackgroundApps { get; private set; } = new List<IBackgroundApp>();
        public List<IGlobalApp> GlobalApps { get; private set; } = new List<IGlobalApp>();
        public List<IObjectApp> ObjectApps { get; private set; } = new List<IObjectApp>();

        public void HandleMessage(MainMessage msg)
        {
            AppMsg appMsg = msg.AppMsg;

        }

        public void Init(ClosedAPI api)
        {
            this._api = api;
            this._api.Services.Room.RoomExited += Reset;
            this._api.Services.Room.RoomEntered += OnRoomEnter;
        }

        public void RegisterApp(IApplication app)
        {
            if(app == null)
            {
                throw new AppServiceException("Given app cannot be null.");
            }
            AppInfo info = app.GetInfo();
            if(AllApps.ContainsKey(info.ID))
            {
                app.Dispose();
                throw new AppServiceException("Another instance of this app is already registered.");
            }
            switch (info.Type)
            {
                case AppType.APP_BACKGROUND:
                    RegisterBackgroundApp((IBackgroundApp)app);
                    break;
                case AppType.APP_MENU:
                    RegisterMenuApp((IMenuApp)app);
                    break;
                case AppType.APP_GLOBAL:
                    RegisterGlobalApp((IGlobalApp)app);
                    break;
                case AppType.APP_OBJECT:
                    RegisterObjectApp((IObjectApp)app);
                    break;
                default:
                    throw new AppServiceException("Unknown application type.");
            }
            AllApps.Add(info.ID, app);
        }

        private void InitDefaultApps()
        {
            foreach(IApplication app in _api.OpenAPI.Apps)
            {
                RegisterApp(app);
            }
        }

        private void RegisterMenuApp(IMenuApp app)
        {
            app.Init(_api.OpenAPI, _api.MenuAPI);
            MenuApps.Add(app);
        }

        private void RegisterBackgroundApp(IBackgroundApp app)
        {
            app.Init(_api.OpenAPI);
            BackgroundApps.Add(app);
        }

        private void RegisterGlobalApp(IGlobalApp app)
        {
            app.Init(_api.OpenAPI, _api.GlobalAPI);
            GlobalApps.Add(app);
        }

        private void RegisterObjectApp(IObjectApp app)
        {
            app.Init(_api.OpenAPI, _api.ObjectAPI);
        }

        public void Reset()
        {
            foreach(IApplication app in AllApps.Values)
            {
                app.Dispose();
            }
            AllApps.Clear();
            MenuApps.Clear();
            GlobalApps.Clear();
            BackgroundApps.Clear();
            ObjectApps.Clear();
        }

        private void OnRoomEnter()
        {
            InitDefaultApps();
        }
    }
}
