using System;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Common.Core.Services.AppService;
using VrLifeAPI.Forwarder.Core.Applications;

namespace VrLifeAPI.Forwarder.Core.Services.AppService
{
    public interface IAppServiceForwarder : IAppService, IServiceForwarder
    {
        void RegisterApp(uint roomId, IApplicationForwarder app);
    }
}
