using Assets.Scripts.Core.Applications.MenuApp.MenuItems;
using VrLifeAPI.Client.API;

namespace Assets.Scripts.Core.Applications.DefaultApps.FriendsManagementApp
{
    class FriendsSearch : IFriendsManagementView
    {
        private const int ROWS = 6;
        private MenuItemGrid _root;
        private IOpenAPI _api;
        public FriendsSearch(IOpenAPI api)
        {
            _api = api;
            InitMenuItems();
        }

        private void InitMenuItems()
        {
            _root = new MenuItemGrid("FriendsSearch", 4, ROWS + 1);
        }

        public MenuItemGrid GetRoot()
        {
            return _root;
        }

        public void Refresh()
        {

        }
    }
}
