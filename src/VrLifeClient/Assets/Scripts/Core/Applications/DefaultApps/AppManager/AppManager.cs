using Assets.Scripts.API.HUDAPI;
using Assets.Scripts.Core.Applications.MenuApp.MenuItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using VrLifeAPI;
using VrLifeAPI.Client.API;
using VrLifeAPI.Client.API.MenuAPI;
using VrLifeAPI.Client.Applications;
using VrLifeAPI.Client.Applications.BackgroundApp;
using VrLifeAPI.Client.Applications.DefaultApps.AppManager;
using VrLifeAPI.Client.Applications.GlobalApp;
using VrLifeAPI.Client.Applications.MenuApp;
using VrLifeAPI.Client.Applications.MenuApp.MenuItems;
using VrLifeAPI.Client.Applications.ObjectApp;
using VrLifeAPI.Common.Core.Services.AppService;
using VrLifeClient;
using VrLifeClient.API;
using VrLifeClient.API.HUDAPI;
using VrLifeClient.API.OpenAPI;
using VrLifeShared.Core;
using VrLifeShared.Core.Applications;

namespace Assets.Scripts.Core.Applications.DefaultApps.AppManager
{
    class AppManager : IAppManager
    {
        public const ulong APP_ID = 0;
        private const string NAME = "App Manager";
        private const string DESC = "App Manager for listing in App Package Store and communicating with AppService.";
        private IClosedAPI _api;
        private AppInfo _info;
        private MenuItemScrollable _root;
        private Dictionary<ulong, IMenuItemButton> _installButtons = new Dictionary<ulong, IMenuItemButton>();

        public AppManager()
        {
            _info = new AppInfo(APP_ID, NAME, DESC,
                new AppVersion(new int[] { 1, 0, 0 }), AppType.APP_MENU);
        }

        public AppInfo GetInfo()
        {
            return _info;
        }

        public void Dispose()
        {

        }

        private void OnEnabled()
        {
            ReRender();
        }

        private void InitMenuItems()
        {
            _root = new MenuItemScrollable("appsList", UnityEngine.TextAnchor.UpperLeft);
            _root.Enabled += OnEnabled;
            ReRender();
        }

        private IMenuItem CreateMenuItem(IAppPackageInfo item)
        {
            MenuItemGrid grid = new MenuItemGrid($"item{item.ID}", 5, 1);
            MenuItemText name = new MenuItemText("name");
            grid.AddChild(0, 0, 1, 1, name);
            name.SetText(item.Name);
            name.SetFontSize(5, 12);
            name.SetTextStyle(UnityEngine.FontStyle.Bold);
            name.SetAlignment(UnityEngine.TextAnchor.MiddleLeft);
            MenuItemText desc = new MenuItemText("desc");
            grid.AddChild(1, 0, 3, 1, desc);
            desc.SetText(item.Desc);
            desc.SetFontSize(5, 12);
            desc.SetAlignment(UnityEngine.TextAnchor.MiddleLeft);
            if(_api.Services.App.AllApps.ContainsKey(item.ID))
            {
                MenuItemText installed = new MenuItemText("installed");
                grid.AddChild(4, 0, 1, 1, installed);
                installed.SetAlignment(UnityEngine.TextAnchor.MiddleCenter);
                installed.SetTextStyle(UnityEngine.FontStyle.Bold);
                installed.SetText("Installed");
            }
            else
            {
                MenuItemButton install = new MenuItemButton("install");
                grid.AddChild(4, 0, 1, 1, install);
                install.SetText("Install");
                install.Clicked += () =>
                {
                    install.SetEnabled(false);
                    install.SetText("Installing...");
                    _api.Services.App.LoadApp(item.ID).Exec();
                };
                _installButtons.Add(item.ID, install);
            }
            return grid;
        }

        public void ReRender()
        {
            _root.GetChildren().ForEach(x => { _root.RemoveChild(x); x.Dispose(); });
            _installButtons.Clear();
            List<IAppPackageInfo> packages = _api.Services.App.ListAppPackages().Wait();
            packages.ForEach(x => _root.AddChildBottom(CreateMenuItem(x), 50));
        }

        private void OnAppLoaded(AppInfo app)
        {
            if(_installButtons.TryGetValue(app.ID, out IMenuItemButton button))
            {
                button.SetEnabled(false);
                button.SetText("Installed");
            }
        }

        public IMenuItem GetRootItem()
        {
            return _root;
        }

        public void Init(IOpenAPI api, IMenuAPI menuAPI, IHUDAPI hudAPI)
        {
            _api = api.GetClosedAPI(_info);
            _api.Services.App.AddedNewApp += OnAppLoaded;
            InitMenuItems();
        }

        public void HandleNotification(Notification notification)
        {

        }
    }
}
