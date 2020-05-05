using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace VrLifeServer.Core
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
    class Statistics
    {
        public static readonly string[] SUPPORTED_TYPES = { "mqtt" };
    }
}
