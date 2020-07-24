using System;
using System.Collections.Generic;
using System.Linq;
using VrLifeAPI.Client.Services;

namespace VrLifeAPI.Client.API
{
    public interface IServiceProvider
    {
        IAppServiceClient App { get; }
        IEventServiceClient Event { get; }
        IRoomServiceClient Room { get; }
        ISystemServiceClient System { get; }
        ITickRateServiceClient TickRate { get; }
        IUserServiceClient User { get; }
    }
}
