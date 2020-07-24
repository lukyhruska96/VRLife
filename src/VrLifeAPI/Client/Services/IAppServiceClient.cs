using System;
using System.Collections.Generic;
using VrLifeAPI.Client.Applications;
using VrLifeAPI.Client.Applications.BackgroundApp;
using VrLifeAPI.Client.Applications.GlobalApp;
using VrLifeAPI.Client.Applications.MenuApp;
using VrLifeAPI.Client.Applications.ObjectApp;
using VrLifeAPI.Common.Core.Services.AppService;

namespace VrLifeAPI.Client.Services
{
    public enum AppMsgRecipient
    {
        FORWARDER,
        PROVIDER
    }
    public interface IAppServiceClient : IServiceClient
    {
        Dictionary<ulong, IApplication> AllApps { get; }
        List<IMenuApp> MenuApps { get; }
        List<IBackgroundApp> BackgroundApps { get; }
        List<IGlobalApp> GlobalApps { get; }
        List<IObjectApp> ObjectApps { get; }

        event Action<AppInfo> AddedNewApp;

        void RegisterApp(IApplication app);
        IServiceCallback<byte[]> SendAppMsg(AppInfo app, byte[] data, AppMsgRecipient recipient);
        IServiceCallback<List<IAppPackageInfo>> ListAppPackages();
        IServiceCallback<bool> LoadApp(ulong appId);
    }
}
