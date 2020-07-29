using System;
using System.Collections.ObjectModel;
using VrLifeAPI.Common.Core.Services.AppService;
using VrLifeAPI.Provider.Core.Applications;

namespace VrLifeAPI.Provider.Core.Services.AppService
{
    /// <summary>
    /// Interface aplikační služby na straně Providera
    /// </summary>
    public interface IAppServiceProvider : IAppService, IServiceProvider
    {
        /// <summary>
        /// Seznam instancí zinicializovaných aplikací na straně Providera.
        /// </summary>
        ReadOnlyDictionary<ulong, IApplicationProvider> Apps { get; }

        /// <summary>
        /// Registrace nové nezinicializaované aplikace.
        /// </summary>
        /// <param name="app">Instance nezinicializované aplikace.</param>
        void RegisterApp(IApplicationProvider app);

    }
}
