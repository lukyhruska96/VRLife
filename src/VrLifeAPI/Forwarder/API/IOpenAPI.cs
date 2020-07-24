using VrLifeAPI.Common;
using VrLifeAPI.Common.Logging.Logging;
using VrLifeAPI.Forwarder.Core.Applications.DefaultApps;
using VrLifeAPI.Networking;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Forwarder.API
{
    public interface IOpenAPI
    {
        IUDPNetworking<MainMessage> Networking { get; }
        IConfig Config { get; }

        IDefaultAppsForwarder Apps { get; }

        void Init(IClosedAPI api);

        ILogger CreateLogger(string className);

        IClosedAPI GetClosedAPI(AppInfo app);
        
    }
}
