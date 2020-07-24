using System.Net;
using VrLifeAPI.Client;
using VrLifeAPI.Common.Logging.Logging;

namespace VrLifeClient
{
    class Config : IConfig
    {
        public ILogger Loggers { get; set; }
        public IPEndPoint MainServer { get; set; }
        
    }
}
