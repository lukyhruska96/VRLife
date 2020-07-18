using Assets.Scripts.Core.Applications.MenuApp.MenuItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeClient.API;
using VrLifeClient.API.OpenAPI;

namespace Assets.Scripts.Core.Applications.DefaultApps.FriendsManagementApp
{
    class FriendsSearch : IFriendsManagementView
    {
        private const int ROWS = 6;
        private MenuItemGrid _root;
        private OpenAPI _api;
        public FriendsSearch(OpenAPI api)
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
