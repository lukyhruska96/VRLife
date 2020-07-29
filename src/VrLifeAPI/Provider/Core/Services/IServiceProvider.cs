using VrLifeAPI.Common.Core.Services;
using VrLifeAPI.Provider.API;

namespace VrLifeAPI.Provider.Core.Services
{
    /// <summary>
    /// Interface služby na straně Providera.
    /// </summary>
    public interface IServiceProvider : IService
    {
        /// <summary>
        /// Inicializace služby.
        /// </summary>
        /// <param name="api">Instance ClosedAPI.</param>
        void Init(IClosedAPI api);
    }
}
