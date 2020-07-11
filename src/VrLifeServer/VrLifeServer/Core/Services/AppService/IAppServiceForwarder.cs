using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.API.Forwarder;
using VrLifeServer.Applications;

namespace VrLifeServer.Core.Services.AppService
{
    interface IAppServiceForwarder : IAppService, IServiceForwarder
    {
        void RegisterApp(uint roomId, IApplicationForwarder app);
    }
}
