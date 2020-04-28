using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.Core.Services;

namespace VrLifeServer.API
{
    class ServiceProvider
    {
        private ISystemService _systemService;
        public ISystemService System { get => _systemService; }

        private EventService _eventService;
        public EventService Event { get => _eventService; }

        private TickRateService _tickRateService;
        public TickRateService TickRate { get => _tickRateService; }

        private RoomService _roomService;
        public RoomService Room { get => _roomService; }

        private UserService _userService;
        public UserService User { get => _userService; }

        private AppService _appService;
        public AppService App { get => _appService; }

        public ServiceProvider(ISystemService systemService, 
            EventService eventService, 
            TickRateService tickRateService, 
            RoomService roomService, 
            UserService userService, 
            AppService appService)
        {
            this._eventService = eventService;
            this._tickRateService = tickRateService;
            this._roomService = roomService;
            this._userService = userService;
            this._appService = appService;
        }
    }
}
