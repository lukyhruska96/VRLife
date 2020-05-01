using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.Core.Services;
using VrLifeServer.Core.Services.AppService;
using VrLifeServer.Core.Services.EventService;
using VrLifeServer.Core.Services.RoomService;
using VrLifeServer.Core.Services.SystemService;
using VrLifeServer.Core.Services.TickRateService;
using VrLifeServer.Core.Services.UserService;

namespace VrLifeServer.API
{
    class ServiceProvider
    {
        private ISystemService _systemService;
        public ISystemService System { get => _systemService; }

        private IEventService _eventService;
        public IEventService Event { get => _eventService; }

        private ITickRateService _tickRateService;
        public ITickRateService TickRate { get => _tickRateService; }

        private IRoomService _roomService;
        public IRoomService Room { get => _roomService; }

        private IUserService _userService;
        public IUserService User { get => _userService; }

        private IAppService _appService;

        public IAppService App { get => _appService; }

        public ServiceProvider(ISystemService systemService,
            IEventService eventService,
            ITickRateService tickRateService,
            IRoomService roomService,
            IUserService userService,
            IAppService appService)
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
