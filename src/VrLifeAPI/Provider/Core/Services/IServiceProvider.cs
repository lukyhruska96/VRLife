using VrLifeAPI.Common.Core.Services;
using VrLifeAPI.Provider.API;

namespace VrLifeAPI.Provider.Core.Services
{
    public interface IServiceProvider : IService
    {
        void Init(IClosedAPI api);
    }
}
