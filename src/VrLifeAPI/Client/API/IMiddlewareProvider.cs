using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeAPI.Networking.Middlewares;
using VrLifeAPI.Networking.NetworkingModels;
using VrLifeShared.Networking.Middlewares;

namespace Assets.Scripts.API
{
    /// <summary>
    /// Poskytovatel Middlewarů objektu IUDPNetworking.
    /// </summary>
    public interface IMiddlewareProvider
    {
        /// <summary>
        /// Instance ClientIdFiller dosazená 
        /// do instance IUDPNetworking dostupné z OpenAPI.
        /// </summary>
        IClientIdFiller ClientIdFiller { get; }
        /// <summary>
        /// Instance MsgIdIncrement dosazená 
        /// do instance IUDPNetworking dostupné z OpenAPI.
        /// </summary>
        IMsgIdIncrement MsgIdIncrement { get; }
        /// <summary>
        /// Instance RedirectMsgHandler dosazená 
        /// do instance IUDPNetworking dostupné z OpenAPI.
        /// </summary>
        IRedirectMsgHandler RedirectMsgHandler { get; }

        /// <summary>
        /// Seznam všech Middlewarů.
        /// </summary>
        /// <returns>Seznam všech Middlewarů.</returns>
        List<IMiddleware<MainMessage>> ToList();
    }
}
