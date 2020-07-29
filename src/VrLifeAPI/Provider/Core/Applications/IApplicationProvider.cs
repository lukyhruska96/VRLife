using System.Collections.ObjectModel;
using VrLifeAPI.Common.Core.Applications;
using VrLifeAPI.Common.Core.Services;
using VrLifeAPI.Provider.API;
using VrLifeAPI.Provider.Core.Services.AppService;

namespace VrLifeAPI.Provider.Core.Applications
{
    /// <summary>
    /// Interface aplikace na straně Providera.
    /// </summary>
    public interface IApplicationProvider : IApplicationServer
    {
        /// <summary>
        /// Inicializace aplikace.
        /// </summary>
        /// <param name="api">Instance OpenAPI.</param>
        /// <param name="appDataService">Instance AppDataService k přístupu do databáze.</param>
        /// <param name="appDataStorage">Instance AppDataStorage k přípstupu do vyhrazené složky.</param>
        void Init(IOpenAPI api, IAppDataService appDataService, IAppDataStorage appDataStorage);

    }
}
