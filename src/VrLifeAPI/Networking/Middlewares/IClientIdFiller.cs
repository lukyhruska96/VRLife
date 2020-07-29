using System;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Networking.Middlewares;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeShared.Networking.Middlewares
{
    /// <summary>
    /// Interface ClientIDFiller middlewaru
    /// </summary>
    public interface IClientIdFiller : IMiddleware<MainMessage>
    {
        /// <summary>
        /// nastavení ID klienta, které bude doplňováno
        /// do každé odeslané zprávy.
        /// </summary>
        /// <param name="id">ID klienta.</param>
        void SetId(ulong id);
    }
}
