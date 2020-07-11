using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.API.Provider;
using VrLifeServer.Applications;

namespace VrLifeServer.Core.Services.AppService
{
    interface IAppServiceProvider : IAppService, IServiceProvider
    {
        void RegisterApp(IApplicationProvider app);

    }
}
