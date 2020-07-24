using Assets.Scripts.API.HUDAPI;
using VrLifeAPI.Client.API;
using VrLifeAPI.Client.API.MenuAPI;
using VrLifeAPI.Client.Applications.MenuApp.MenuItems;
using VrLifeClient.API.HUDAPI;

namespace VrLifeAPI.Client.Applications.MenuApp
{
    public interface IMenuApp : IApplication
    {
        IMenuItem GetRootItem();
        void Init(IOpenAPI api, IMenuAPI menuAPI, IHUDAPI hudAPI);
        void HandleNotification(Notification notification);
    }
}
