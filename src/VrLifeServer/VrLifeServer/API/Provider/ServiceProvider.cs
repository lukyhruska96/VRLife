
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Provider.API;
using VrLifeAPI.Provider.Core.Services.AppService;
using VrLifeAPI.Provider.Core.Services.EventService;
using VrLifeAPI.Provider.Core.Services.RoomService;
using VrLifeAPI.Provider.Core.Services.SystemService;
using VrLifeAPI.Provider.Core.Services.TickRateService;
using VrLifeAPI.Provider.Core.Services.UserService;

namespace VrLifeServer.API.Provider
{
    class ServiceProvider : IServiceProvider
    {
        private ISystemServiceProvider _systemService;
        public ISystemServiceProvider System { get => _systemService; }

        private IEventServiceProvider _eventService;
        public IEventServiceProvider Event { get => _eventService; }

        private ITickRateServiceProvider _tickRateService;
        public ITickRateServiceProvider TickRate { get => _tickRateService; }

        private IRoomServiceProvider _roomService;
        public IRoomServiceProvider Room { get => _roomService; }

        private IUserServiceProvider _userService;
        public IUserServiceProvider User { get => _userService; }

        private IAppServiceProvider _appService;
        public IAppServiceProvider App { get => _appService; }

        public ServiceProvider(ISystemServiceProvider systemService,
            IEventServiceProvider eventService,
            ITickRateServiceProvider tickRateService,
            IRoomServiceProvider roomService,
            IUserServiceProvider userService,
            IAppServiceProvider appService)
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
