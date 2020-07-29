using VrLifeAPI.Provider.Core.Services.AppService;
using VrLifeAPI.Provider.Core.Services.EventService;
using VrLifeAPI.Provider.Core.Services.RoomService;
using VrLifeAPI.Provider.Core.Services.SystemService;
using VrLifeAPI.Provider.Core.Services.TickRateService;
using VrLifeAPI.Provider.Core.Services.UserService;

namespace VrLifeAPI.Provider.API
{
    /// <summary>
    /// Interface poskytovatele služeb na straně Providera.
    /// </summary>
    public interface IServiceProvider
    {
        ISystemServiceProvider System { get; }
        IEventServiceProvider Event { get; }
        ITickRateServiceProvider TickRate { get; }
        IRoomServiceProvider Room { get; }
        IUserServiceProvider User { get; }
        IAppServiceProvider App { get; }
    }
}
