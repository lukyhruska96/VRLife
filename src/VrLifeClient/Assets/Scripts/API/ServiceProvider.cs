using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeAPI.Client.API;
using VrLifeAPI.Client.Services;
using VrLifeClient.Core.Services.AppService;
using VrLifeClient.Core.Services.EventService;
using VrLifeClient.Core.Services.RoomService;
using VrLifeClient.Core.Services.SystemService;
using VrLifeClient.Core.Services.TickRateService;
using VrLifeClient.Core.Services.UserService;

namespace Assets.Scripts.API
{
    class ServiceProvider : IServiceProvider
    {
        private AppServiceClient _appServiceClient;
        public IAppServiceClient App { get => _appServiceClient; }

        private EventServiceClient _eventServiceClient;
        public IEventServiceClient Event { get => _eventServiceClient; }

        private RoomServiceClient _roomServiceClient;
        public IRoomServiceClient Room { get => _roomServiceClient; }

        private SystemServiceClient _systemServiceClient;
        public ISystemServiceClient System { get => _systemServiceClient; }

        private TickRateServiceClient _tickRateServiceClient;
        public ITickRateServiceClient TickRate { get => _tickRateServiceClient; }

        private UserServiceClient _userServiceClient;
        public IUserServiceClient User { get => _userServiceClient; }

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
