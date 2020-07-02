using System;
using System.Collections.Generic;
using System.Text;

namespace VrLifeServer.Core.Services.TickRateService
{
    interface ITickRateServiceProvider : ITickRateService, IServiceProvider
    {
    }
}
