

using VrLifeAPI.Forwarder.API;
using VrLifeAPI.Forwarder.Core.Services.AppService;
using VrLifeAPI.Forwarder.Core.Services.EventService;
using VrLifeAPI.Forwarder.Core.Services.RoomService;
using VrLifeAPI.Forwarder.Core.Services.SystemService;
using VrLifeAPI.Forwarder.Core.Services.TickRateService;
using VrLifeAPI.Forwarder.Core.Services.UserService;

namespace VrLifeServer.API.Forwarder
{
    class ServiceProvider : IServiceProvider
    {
        private ISystemServiceForwarder _systemService;
        public ISystemServiceForwarder System { get => _systemService; }

        private IEventServiceForwarder _eventService;
        public IEventServiceForwarder Event { get => _eventService; }

        private ITickRateServiceForwarder _tickRateService;
        public ITickRateServiceForwarder TickRate { get => _tickRateService; }

        private IRoomServiceForwarder _roomService;
        public IRoomServiceForwarder Room { get => _roomService; }

        private IUserServiceForwarder _userService;
        public IUserServiceForwarder User { get => _userService; }

        private IAppServiceForwarder _appService;
        public IAppServiceForwarder App { get => _appService; }

        public ServiceProvider(ISystemServiceForwarder systemService,
            IEventServiceForwarder eventService,
            ITickRateServiceForwarder tickRateService,
            IRoomServiceForwarder roomService,
            IUserServiceForwarder userService,
            IAppServiceForwarder appService)
        {
            this._systemService = systemService;
            this._eventService = eventService;
            this._tickRateService = tickRateService;
            this._roomService = roomService;
            this._userService = userService;
            this._appService = appService;
        }
    }
}
