using System.Collections.Generic;
using VrLifeAPI.Client.Applications;
using VrLifeAPI.Client.Applications.BackgroundApp;
using VrLifeAPI.Client.Applications.GlobalApp;
using VrLifeAPI.Client.Applications.MenuApp;
using VrLifeAPI.Client.Applications.ObjectApp;
using VrLifeAPI.Client.Core.Services;

namespace VrLifeAPI.Client.API.OpenAPI
{
    public interface IAppAPI
    {
        IServiceCallback<byte[]> SendAppMsg(AppInfo app, byte[] data, AppMsgRecipient recipient);
        List<IMenuApp> MenuApps { get; }
        List<IGlobalApp> GlobalApps { get; }
        List<IBackgroundApp> BackgroundApps { get; }
        List<IObjectApp> ObjectApps { get; }
        Dictionary<ulong, IApplication> AllApps { get; }
    }
}
