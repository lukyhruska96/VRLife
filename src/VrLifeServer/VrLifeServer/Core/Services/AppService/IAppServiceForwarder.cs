using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.API.Forwarder;

namespace VrLifeServer.Core.Services.AppService
{
    interface IAppServiceForwarder : IAppService, IServiceForwarder
    {
    }
}
