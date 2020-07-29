using System;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Networking.Middlewares;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeShared.Networking.Middlewares
{
    /// <summary>
    /// Interface ServerIdFiller middlewaru.
    /// </summary>
    public interface IServerIdFiller : IMiddleware<MainMessage>
    {
        /// <summary>
        /// Nastavení ServerID, které bude
        /// doplňováno do každé odeslané zprávy.
        /// </summary>
        /// <param name="id">ID Serveru.</param>
        void SetId(uint id);
    }
}
