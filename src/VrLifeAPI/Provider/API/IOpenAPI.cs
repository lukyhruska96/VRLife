using VrLifeAPI.Common;
using VrLifeAPI.Common.Logging.Logging;
using VrLifeAPI.Networking;
using VrLifeAPI.Networking.NetworkingModels;
using VrLifeAPI.Provider.API.APIs;
using VrLifeAPI.Provider.Core.Applications.DefaultApps;

namespace VrLifeAPI.Provider.API
{
    public interface IOpenAPI
    {
        IUDPNetworking<MainMessage> Networking { get; }
        IConfig Config { get; }
        IUserAPI User { get;}
        IDefaultAppsProvider Apps { get; }

        void Init(IClosedAPI api);

        ILogger CreateLogger(string className);

        IClosedAPI GetClosedAPI(AppInfo app);
    }
}
