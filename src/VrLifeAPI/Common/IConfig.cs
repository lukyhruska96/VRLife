using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using VrLifeAPI.Provider.Database;
using VrLifeAPI.Common.Logging.Logging;

namespace VrLifeAPI.Common
{
    /// <summary>
    /// Interface konfiguračního souboru na straně serveru
    /// </summary>
    public interface IConfig : IDisposable
    {

        /// <summary>
        /// Adresa na které server poslouchá.
        /// </summary>
        IPAddress Listen { get; }

        /// <summary>
        /// Veřejná adresa serveru pro přístup z WAN.
        /// </summary>
        IPAddress ServerAddress { get; }

        /// <summary>
        /// Cesta k uložišti balíčků aplikací.
        /// </summary>
        string AppStoragePath { get; }

        /// <summary>
        /// Port na kterém server poslouchá.
        /// </summary>
        uint UdpPort { get; }

        /// <summary>
        /// Flag, zda se jedná o hlavní server.
        /// </summary>
        bool IsMain { get; }

        /// <summary>
        /// Adresa hlavního serveru. Required v případě Forwarder Serveru.
        /// </summary>
        IPEndPoint MainServer { get; }

        /// <summary>
        /// Hlavní Logger
        /// </summary>
        ILogger Loggers { get; }

        /// <summary>
        /// Připojovací údaje k databázi. Required v případě Provider Serveru.
        /// </summary>
        DatabaseConnectionStruct Database { get; }

        /// <summary>
        /// Stav debug režimu.
        /// </summary>
        bool Debug { get; }
    }
}
