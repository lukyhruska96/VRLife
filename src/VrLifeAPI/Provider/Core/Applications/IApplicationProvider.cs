using System.Collections.ObjectModel;
using VrLifeAPI.Common.Core.Applications;
using VrLifeAPI.Common.Core.Services;
using VrLifeAPI.Provider.API;
using VrLifeAPI.Provider.Core.Services.AppService;

namespace VrLifeAPI.Provider.Core.Applications
{
    public interface IApplicationProvider : IApplicationServer
    {
        void Init(IOpenAPI api, IAppDataService appDataService, IAppDataStorage appDataStorage);

    }
}
