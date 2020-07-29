using System;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Common.Core.Services.AppService
{

    /// <summary>
    /// Interface aplikační služby na straně serveru.
    /// </summary>
    public interface IAppService : IService
    {
        /// <summary>
        /// Zpracování přijaté události pro aplikaci.
        /// 
        /// Odpověď je poté zabalena do síťovacího objektu EventResponse.
        /// </summary>
        /// <param name="msg">Hlavní přijatá zpráva.</param>
        /// <returns>Odpověď na událost ve formě byte array.</returns>
        byte[] HandleEvent(MainMessage msg);
    }
}
