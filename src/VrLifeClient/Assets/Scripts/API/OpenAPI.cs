using Assets.Scripts.API;
using Assets.Scripts.API.OpenAPI;
using Assets.Scripts.Core.Applications.DefaultApps;
using VrLifeAPI;
using VrLifeAPI.Client;
using VrLifeAPI.Client.API;
using VrLifeAPI.Client.API.OpenAPI;
using VrLifeAPI.Client.Applications.DefaultApps;
using VrLifeAPI.Common.Logging.Logging;
using VrLifeAPI.Networking;
using VrLifeAPI.Networking.NetworkingModels;
using VrLifeShared.Logging;
using VrLifeShared.Networking;

namespace VrLifeClient.API.OpenAPI
{
    class OpenAPI : IOpenAPI
    {
        public IUDPNetworking<MainMessage> Networking { get => _udpNetworking; }
        private IUDPNetworking<MainMessage> _udpNetworking;

        public IConfig Config { get => _config; }
        private Config _config;

        public IUserAPI User { get => _user; }
        private UserAPI _user;

        public IRoomAPI Room { get => _room; }
        private RoomAPI _room;

        public IEventAPI Event { get => _event; }
        private EventAPI _event;

        public ITickAPI TickRate { get => _tick; }
        private TickAPI _tick;

        public ISystemAPI System { get => _system; }
        private SystemAPI _system;

        public IDefaultApps DefaultApps { get => _defaultApps; }
        private DefaultApps _defaultApps;

        public IAppAPI App { get => _app; }
        private AppAPI _app;

        private ServiceProvider _serviceProvider;

        public IClosedAPI GetClosedAPI(AppInfo info)
        {
            return VrLifeCore.GetClosedAPI(info);
        }

        public ILogger CreateLogger(string className)
        {
            if(_config.Loggers == null)
            {
                return null;
            }
            return new LoggerWrapper(className, _config.Loggers);
        }

        public OpenAPI(UDPNetworking<MainMessage> udpNetworking, Config config, ServiceProvider serviceProvider)
        {
            this._udpNetworking = udpNetworking;
            this._config = config;
            this._serviceProvider = serviceProvider;
            this._user = new UserAPI(serviceProvider.User);
            this._room = new RoomAPI(serviceProvider.Room);
            this._event = new EventAPI(serviceProvider.Event);
            this._tick = new TickAPI(serviceProvider.TickRate);
            this._system = new SystemAPI(serviceProvider.System);
            this._app = new AppAPI(serviceProvider.App);
            this._defaultApps = new DefaultApps();
        }
    }
}
