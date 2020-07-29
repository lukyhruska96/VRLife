using System;
using System.Collections.Generic;
using System.Linq;
using VrLifeAPI.Client.Core.Services;

namespace VrLifeAPI.Client.API
{

    /// <summary>
    /// Poskytovatel instancí služeb.
    /// </summary>
    public interface IServiceProvider
    {
        /// <summary>
        /// Instance AppService
        /// </summary>
        IAppServiceClient App { get; }

        /// <summary>
        /// Instance EventService
        /// </summary>
        IEventServiceClient Event { get; }

        /// <summary>
        /// Instance RoomService
        /// </summary>
        IRoomServiceClient Room { get; }

        /// <summary>
        /// Instance SystemService
        /// </summary>
        ISystemServiceClient System { get; }

        /// <summary>
        /// Instance TickRateService
        /// </summary>
        ITickRateServiceClient TickRate { get; }

        /// <summary>
        /// Instance UserService
        /// </summary>
        IUserServiceClient User { get; }
    }
}
