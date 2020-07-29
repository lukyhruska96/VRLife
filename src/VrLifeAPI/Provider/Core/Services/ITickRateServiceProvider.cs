using System;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Common.Core.Services.TickRateService;

namespace VrLifeAPI.Provider.Core.Services.TickRateService
{

    /// <summary>
    /// Interface TickRate serveru na straně Providera.
    /// </summary>
    public interface ITickRateServiceProvider : ITickRateService, IServiceProvider
    {
    }
}
