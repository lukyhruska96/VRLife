using System;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Common.Core.Services.AppService;
using VrLifeAPI.Forwarder.Core.Applications;

namespace VrLifeAPI.Forwarder.Core.Services.AppService
{
    /// <summary>
    /// Interface aplikační služby na straně Forwardera
    /// </summary>
    public interface IAppServiceForwarder : IAppService, IServiceForwarder
    {
        /// <summary>
        /// Registrace instace aplikace pro stranu Forwardera v dané místnosti.
        /// </summary>
        /// <param name="roomId">ID místnosti.</param>
        /// <param name="app">Instance aplikace.</param>
        void RegisterApp(uint roomId, IApplicationForwarder app);
    }
}
