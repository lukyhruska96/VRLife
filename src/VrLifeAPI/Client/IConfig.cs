using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VrLifeAPI.Client
{
    /// <summary>
    /// Interface konfigurace na straně klienta
    /// </summary>
    public interface IConfig
    {
        /// <summary>
        /// Adresa hlavního serveru.
        /// </summary>
        IPEndPoint MainServer { get; set; }
        
    }
}
