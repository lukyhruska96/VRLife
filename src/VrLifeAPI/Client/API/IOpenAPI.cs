using VrLifeAPI.Client.API.OpenAPI;
using VrLifeAPI.Client.Applications.DefaultApps;
using VrLifeAPI.Networking;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Client.API
{
    public interface IOpenAPI
    {
        IUDPNetworking<MainMessage> Networking { get; }
        IConfig Config { get; }

        IUserAPI User { get; }

        IRoomAPI Room { get; }

        IEventAPI Event { get; }

        ITickAPI TickRate { get; }

        ISystemAPI System { get; }

        IDefaultApps DefaultApps { get; }

        IAppAPI App { get; }
        IClosedAPI GetClosedAPI(AppInfo info);
    }
}
