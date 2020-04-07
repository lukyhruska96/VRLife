using System;
using System.Net;

namespace VrLifeServer.Applications
{
    public class StatisticsConf
    {
        public string Type { get => type; }
        public string type;
        public IPAddress Addr { get => addr; }
        public IPAddress addr;
        public uint Port { get => port; }
        public uint port;

        public StatisticsConf(string type, IPAddress addr, uint port)
        {
            this.type = type;
            this.addr = addr;
            this.port = port;
        }
    }

    public class Statistics : IApplication
    {
        public static readonly string[] SUPPORTED_TYPES = { "mqtt" };
        public void End()
        {
            throw new NotImplementedException();
        }

        public int Init()
        {
            return 0;
        }

        public void Run()
        {
            throw new NotImplementedException();
        }
    }
}
