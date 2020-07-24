using System;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Common.Core.Services.TickRateService;

namespace VrLifeAPI.Provider.Core.Services.TickRateService
{
    public interface ITickRateServiceProvider : ITickRateService, IServiceProvider
    {
    }
}
