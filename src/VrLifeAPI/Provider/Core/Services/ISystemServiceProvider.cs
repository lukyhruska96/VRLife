using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using VrLifeAPI.Common.Core.Services.SystemService;

namespace VrLifeAPI.Provider.Core.Services.SystemService
{
    /// <summary>
    /// Interface systémové služby na straně Providera.
    /// </summary>
    public interface ISystemServiceProvider : ISystemService, IServiceProvider
    {
        /// <summary>
        /// Kontrola, zda daný server s Providerem komunikuje.
        /// </summary>
        /// <param name="serverId">ID serveru.</param>
        /// <returns>true - odpovídá, false - neodpovídá</returns>
        bool IsAlive(ulong serverId);

        /// <summary>
        /// Získání adresy serveru podle jeho ID
        /// </summary>
        /// <param name="serverId">ID serveru</param>
        /// <returns>IP daného serveru.</returns>
        IPEndPoint GetAddressById(uint serverId);
    }
}
