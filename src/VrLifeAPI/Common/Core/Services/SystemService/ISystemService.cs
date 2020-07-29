using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Common.Core.Services.SystemService
{

    /// <summary>
    /// Interface systémové služby na straně serveru.
    /// </summary>
    public interface ISystemService : IService
    {

        /// <summary>
        /// Vytvoření hello zprávy.
        /// </summary>
        /// <returns>Síťovací objekt.</returns>
        MainMessage CreateHelloMessage();
    }
}
