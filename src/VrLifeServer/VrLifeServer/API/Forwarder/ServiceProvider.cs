using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.Core.Services.AppService;
using VrLifeServer.Core.Services.EventService;
using VrLifeServer.Core.Services.RoomService;
using VrLifeServer.Core.Services.SystemService;
using VrLifeServer.Core.Services.TickRateService;
using VrLifeServer.Core.Services.UserService;

namespace VrLifeServer.API.Forwarder
{
    class ServiceProvider
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
