using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrLifeClient.API;

namespace VrLifeAPI.Client.API.GlobalAPI
{
    /// <summary>
    /// API pro globální aplikace.
    /// </summary>
    public interface IGlobalAPI
    {
        /// <summary>
        /// Instance API pro objekty uživatelů v místnosti.
        /// </summary>
        IPlayersAPI Players { get; }
    }
}
