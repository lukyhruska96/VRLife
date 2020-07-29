using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VrLifeAPI.Networking;
using VrLifeAPI.Networking.Middlewares;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeShared.Networking.Middlewares
{
    /// <summary>
    /// Interface RedirectMsgHandler middlewaru.
    /// </summary>
    public interface IRedirectMsgHandler : IMiddleware<MainMessage>
    {
        /// <summary>
        /// Nastavení instance síťového serveru.
        /// </summary>
        /// <param name="networking">Instance síťového serveru.</param>
        void SetListenner(INetworking<MainMessage> networking);
    }
}
