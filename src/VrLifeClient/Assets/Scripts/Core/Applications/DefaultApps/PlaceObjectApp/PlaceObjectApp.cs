using Assets.Scripts.API.HUDAPI;
using Assets.Scripts.Core.Applications.MenuApp.MenuItems;
using VrLifeAPI;
using VrLifeAPI.Client.API;
using VrLifeAPI.Client.API.MenuAPI;
using VrLifeAPI.Client.Applications.DefaultApps.PlaceObjectApp;
using VrLifeAPI.Client.Applications.MenuApp.MenuItems;
using VrLifeAPI.Client.Applications.ObjectApp;
using VrLifeClient.API.HUDAPI;

namespace Assets.Scripts.Core.Applications.DefaultApps.PlaceObjectApp
{
    class PlaceObjectApp : IPlaceObjectApp
    {
        private IMenuItemScrollable _root = null;
        private AppInfo _info = new AppInfo(6, "PlaceObjectApp", "Placing object application into room.", 
            new AppVersion(new int[] { 1, 0, 0 }), AppType.APP_MENU);

        private IOpenAPI _api;
        private IMenuAPI _menuAPI;
        private IHUDAPI _hudAPI;

        public void Dispose()
        {
            _root?.Dispose();
        }

        public AppInfo GetInfo()
        {
            return _info;
        }

        public IMenuItem GetRootItem()
        {
            return _root;
        }

        public void HandleNotification(Notification notification)
        {

        }

        public void Init(IOpenAPI api, IMenuAPI menuAPI, IHUDAPI hudAPI)
        {
            _api = api;
            _menuAPI = menuAPI;
            _hudAPI = hudAPI;
            PrepareMenuItems();
        }

        private void PrepareMenuItems()
        {
            _root = _menuAPI.CreateScrollable("placeObjectScrollable", UnityEngine.TextAnchor.UpperCenter);
            _root.Enabled += OnEnabled;
            Rerender();
        }

        private void Rerender()
        {
            _root.GetChildren().ForEach(x => { _root.RemoveChild(x); x.Dispose(); });
            _api.App.ObjectApps.ForEach(x => _root.AddChildBottom(CreateMenuItem(x), 50));
        }

        private void OnEnabled()
        {
            Rerender();
        }

        private IMenuItem CreateMenuItem(IObjectApp app)
        {
            AppInfo info = app.GetInfo();
            IMenuItemGrid grid = _menuAPI.CreateGrid($"item{info.ID}", 5, 1);
            IMenuItemText name = _menuAPI.CreateText("name");
            grid.AddChild(0, 0, 1, 1, name);
            name.SetText(info.Name);
            name.SetFontSize(5, 12);
            name.SetTextStyle(UnityEngine.FontStyle.Bold);
            name.SetAlignment(UnityEngine.TextAnchor.MiddleLeft);
            IMenuItemText desc = _menuAPI.CreateText("desc");
            grid.AddChild(1, 0, 3, 1, desc);
            desc.SetText(info.Description);
            desc.SetFontSize(5, 12);
            desc.SetAlignment(UnityEngine.TextAnchor.MiddleLeft);
            MenuItemButton place = new MenuItemButton("place");
            grid.AddChild(4, 0, 1, 1, place);
            place.SetText("Place");
            place.Clicked += () =>
            {
                ObjectAPIController.current?.StartPlacing(app);
            };
            return grid;
        }
    }
}
