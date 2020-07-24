using System;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Forwarder.Core.Services.AppService;
using VrLifeAPI.Forwarder.Core.Services.EventService;
using VrLifeAPI.Forwarder.Core.Services.RoomService;
using VrLifeAPI.Forwarder.Core.Services.SystemService;
using VrLifeAPI.Forwarder.Core.Services.TickRateService;
using VrLifeAPI.Forwarder.Core.Services.UserService;

namespace VrLifeAPI.Forwarder.API
{
    public interface IServiceProvider
    {
        ISystemServiceForwarder System { get; }
        IEventServiceForwarder Event { get; }
        ITickRateServiceForwarder TickRate { get; }
        IRoomServiceForwarder Room { get; }
        IUserServiceForwarder User { get; }
        IAppServiceForwarder App { get; }
    }
}
