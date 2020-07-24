using System;
using System.Collections.ObjectModel;
using VrLifeAPI.Common.Core.Services.AppService;
using VrLifeAPI.Provider.Core.Applications;

namespace VrLifeAPI.Provider.Core.Services.AppService
{
    public interface IAppServiceProvider : IAppService, IServiceProvider
    {
        ReadOnlyDictionary<ulong, IApplicationProvider> Apps { get; }
        void RegisterApp(IApplicationProvider app);

    }
}
