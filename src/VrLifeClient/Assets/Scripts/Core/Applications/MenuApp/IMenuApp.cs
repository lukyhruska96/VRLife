using Assets.Scripts.API.HUDAPI;
using Assets.Scripts.Core.Applications.MenuApp.MenuItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeClient.API;
using VrLifeClient.API.HUDAPI;
using VrLifeClient.API.MenuAPI;
using VrLifeClient.API.OpenAPI;

namespace Assets.Scripts.Core.Applications.MenuApp
{
    interface IMenuApp : IApplication
    {
        IMenuItem GetRootItem();
        void Init(OpenAPI api, MenuAPI menuAPI, HUDAPI hudAPI);
        void HandleNotification(Notification notification);
    }
}
