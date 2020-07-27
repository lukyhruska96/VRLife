using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using VrLifeAPI.Provider.Database;
using VrLifeAPI.Common.Logging.Logging;

namespace VrLifeAPI.Common
{
    public interface IConfig : IDisposable
    {
        IPAddress Listen { get; }

        IPAddress ServerAddress { get; }

        string AppStoragePath { get; }

        uint UdpPort { get; }

        bool IsMain { get; }

        IPEndPoint MainServer { get; }

        ILogger Loggers { get; }

        DatabaseConnectionStruct Database { get; }

        bool Debug { get; }
    }
}
