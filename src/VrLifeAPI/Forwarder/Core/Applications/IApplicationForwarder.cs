using System;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Forwarder.API;
using VrLifeAPI.Common.Core.Applications;
using VrLifeAPI.Common.Core.Services;

namespace VrLifeAPI.Forwarder.Core.Applications
{
    /// <summary>
    /// Interface aplikace na straně Forwardera
    /// </summary>
    public interface IApplicationForwarder : IApplicationServer
    {
        /// <summary>
        /// Inicializace aplikace.
        /// </summary>
        /// <param name="roomId">ID mísnosti instance.</param>
        /// <param name="api">Instance OpenAPI.</param>
        /// <param name="appStorage">Instance přiděleného AppStorage.</param>
        void Init(uint roomId, IOpenAPI api, IAppDataStorage appStorage);
    }
}
