using System;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Client.API;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Client.Core.Services
{
    /// <summary>
    /// Interface pro všechny služby na straně klienta.
    /// </summary>
    public interface IServiceClient
    {

        /// <summary>
        /// Inicializace služby.
        /// </summary>
        /// <param name="api">Instance ClosedAPI.</param>
        void Init(IClosedAPI api);
    }
}
