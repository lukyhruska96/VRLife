using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeClient.Core.Services.AppService;
using VrLifeClient.Core.Services.EventService;
using VrLifeClient.Core.Services.RoomService;
using VrLifeClient.Core.Services.SystemService;
using VrLifeClient.Core.Services.TickRateService;
using VrLifeClient.Core.Services.UserService;

namespace Assets.Scripts.API
{
    class ServiceProvider
    {
        private AppServiceClient _appServiceClient;
        public AppServiceClient App { get => _appServiceClient; }

        private EventServiceClient _eventServiceClient;
        public EventServiceClient Event { get => _eventServiceClient; }

        private RoomServiceClient _roomServiceClient;
        public RoomServiceClient Room { get => _roomServiceClient; }

        private SystemServiceClient _systemServiceClient;
        public SystemServiceClient System { get => _systemServiceClient; }

        private TickRateServiceClient _tickRateServiceClient;
        public TickRateServiceClient TickRate { get => _tickRateServiceClient; }

        private UserServiceClient _userServiceClient;
        public UserServiceClient User { get => _userServiceClient; }

        public ServiceProvider(
            AppServiceClient app,
            EventServiceClient ev,
            RoomServiceClient room,
            SystemServiceClient system,
            TickRateServiceClient tickRate,
            UserServiceClient user
            )
        {
            this._appServiceClient = app;
            this._eventServiceClient = ev;
            this._roomServiceClient = room;
            this._systemServiceClient = system;
            this._tickRateServiceClient = tickRate;
            this._userServiceClient = user;
        }
    }
}
