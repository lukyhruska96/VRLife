using VrLifeAPI.Common.Core.Services.EventService;
using VrLifeAPI.Provider.API;

namespace VrLifeAPI.Provider.Core.Services.EventService
{
    /// <summary>
    /// Interface služby událostí na straně Providera.
    /// </summary>
    public interface IEventServiceProvider : IEventService, IServiceProvider
    {
    }
}
