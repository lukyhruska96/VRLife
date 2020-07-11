using Assets.Scripts.API.MenuAPI;
using Assets.Scripts.Core.Applications.MenuApp.MenuItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeClient.API;

namespace Assets.Scripts.Core.Applications.MenuApp
{
    interface IMenuApp : IApplication
    {
        MenuAppInfo GetMenuInfo();
        IMenuItem GetRootItem();
        void Init(OpenAPI api, MenuAPI menuAPI);
    }
}
