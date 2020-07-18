using Assets.Scripts.API;
using Assets.Scripts.API.OpenAPI;
using Assets.Scripts.Core.Applications.DefaultApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeClient.Core.Services.AppService;
using VrLifeShared.Logging;
using VrLifeShared.Networking;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeClient.API.OpenAPI
{
    class OpenAPI
    {
        public UDPNetworking<MainMessage> Networking { get => _udpNetworking; }
        private UDPNetworking<MainMessage> _udpNetworking;

        public Config Config { get => _config; }
        private Config _config;

        public UserAPI User { get => _user; }
        private UserAPI _user;

        public RoomAPI Room { get => _room; }
        private RoomAPI _room;

        public EventAPI Event { get => _event; }
        private EventAPI _event;

        public TickAPI TickRate { get => _tick; }
        private TickAPI _tick;

        public SystemAPI System { get => _system; }
        private SystemAPI _system;

        public DefaultApps DefaultApps { get => _defaultApps; }
        private DefaultApps _defaultApps;

        public AppAPI App { get => _app; }
        private AppAPI _app;

        private ServiceProvider _serviceProvider;

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
