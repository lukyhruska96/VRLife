using System;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Common.Core.Services;
using VrLifeAPI.Forwarder.API;

namespace VrLifeAPI.Forwarder.Core.Services
{
    /// <summary>
    /// Interface Služby na straně Forwardera
    /// </summary>
    public interface IServiceForwarder : IService
    {
        /// <summary>
        /// Inicializaice služby.
        /// </summary>
        /// <param name="api">Instance ClosedAPI.</param>
        void Init(IClosedAPI api);
    }
}
