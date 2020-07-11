using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.API.Provider;
using VrLifeServer.Core.Services.AppService;

namespace VrLifeServer.Applications
{
    interface IApplicationProvider : IApplicationServer
    {
        void Init(OpenAPI api, AppDataService appDataService);
    }
}
