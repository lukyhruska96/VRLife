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
    /// <summary>
    /// Interface poskytovatele služeb na straně Forwardera
    /// </summary>
    public interface IServiceProvider
    {
        /// <summary>
        /// Instance systémové služby
        /// </summary>
        ISystemServiceForwarder System { get; }

        /// <summary>
        /// Instance služby událostí
        /// </summary>
        IEventServiceForwarder Event { get; }

        /// <summary>
        /// Instance TickRate služby
        /// </summary>
        ITickRateServiceForwarder TickRate { get; }

        /// <summary>
        /// Instance služby místností
        /// </summary>
        IRoomServiceForwarder Room { get; }

        /// <summary>
        /// Instance uživatelské služby
        /// </summary>
        IUserServiceForwarder User { get; }

        /// <summary>
        /// Instance aplikační služby.
        /// </summary>
        IAppServiceForwarder App { get; }
    }
}
