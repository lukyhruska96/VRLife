using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VrLifeServer.Database;
using VrLifeServer.Logging;

namespace VrLifeServer
{
    public class Config : IDisposable
    {
        public IPAddress Address { get => address; }
        private IPAddress address;

        public uint TcpPort { get => tcpport; }
        private uint tcpport;

        public uint UdpPort { get => udpport; }
        private uint udpport;

        public List<Applications.StatisticsConf> StatisticsConf { get; } = new
            List<Applications.StatisticsConf>();

        public bool IsMain { get => isMain; }
        private bool isMain;

        public IPEndPoint MainServer { get => mainServer; }
        private IPEndPoint mainServer;

        public IPEndPoint Forward { get => forward; }
        private IPEndPoint forward;

        public ILogger Loggers { get => loggers; }
        private LoggersContainer loggers = new LoggersContainer();

        public DatabaseConnectionStruct Database { get => database; }
        private DatabaseConnectionStruct database;

        public bool Debug { get => debug; }
        private bool debug;

        private static IPAddress ParseAddress(string str)
        {
            if (str == null)
            {
                return null;
            }
            IPAddress tmp;
            try
            {
                tmp = IPAddress.Parse(str);
            }
            catch (FormatException)
            {
                return null;
            }
            return tmp;
        }

        private static int ParsePort(string str)
        {
            int tmp;
            if (!int.TryParse(str, out tmp) || tmp < 0 || tmp > 1 << 16)
            {
                return -1;
            }
            return tmp;
        }

        private static Applications.StatisticsConf ParseStatistics(JObject obj)
        {
            if (!obj.ContainsKey("type"))
            {
                return null;
            }
            string type = obj["type"].Value<string>().ToLower();
            if (Array.IndexOf(Applications.Statistics.SUPPORTED_TYPES, type)
                == -1)
            {
                return null;
            }
            if (!obj.ContainsKey("address"))
            {
                return null;
            }
            IPAddress address = ParseAddress(obj["address"].Value<string>());
            if (address == null)
            {
                return null;
            }
            if (!obj.ContainsKey("port"))
            {
                return null;
            }
            int port = ParsePort(obj["port"].Value<string>());
            if (port < 0)
            {
                return null;
            }
            return new Applications.StatisticsConf(type, address, (uint)port);
        }

        private static IPEndPoint ParseEndPoint(string str)
        {
            if (str == null)
            {
                return null;
            }

            string[] forwardSep = str.Split(':');
            if (forwardSep.Length != 2)
            {
                return null;
            }

            IPAddress tmpAddr = ParseAddress(forwardSep[0]);
            if (tmpAddr == null)
            {
                return null;
            }

            int tmpPort = ParsePort(forwardSep[1]);
            if (tmpPort < 0)
            {
                return null;
            }

            return new IPEndPoint(tmpAddr, tmpPort);
        }

        private static ILogger ParseLogger(JObject obj)
        {
            if (!obj.ContainsKey("type"))
            {
                return null;
            }
            string type = obj["type"].Value<string>();
            switch (type)
            {
                case "text":
                    return ParseTextLogger(obj);
                default:
                    return null;
            }
        }

        private static ILogger ParseTextLogger(JObject obj)
        {
            ILogger logger = null;
            if (!obj.ContainsKey("path"))
            {
                return null;
            }
            string path = obj["path"].Value<string>();
            try
            {
                logger = new FileLogger(path);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Unable to initialize text logger: " + e.Message);
            }
            return logger;
        }

        private static DatabaseConnectionStruct? ParseDatabase(JObject obj)
        {
            DatabaseConnectionStruct db;
            if (!obj.ContainsKey("type"))
            {
                return null;
            }
            db.Type = obj["type"].Value<string>();
            if (!obj.ContainsKey("host"))
            {
                return null;
            }
            db.Host = obj["host"].Value<string>();
            if (!obj.ContainsKey("port"))
            {
                return null;
            }
            db.Port = obj["port"].Value<int>();
            if (!obj.ContainsKey("username"))
            {
                return null;
            }
            db.Username = obj["username"].Value<string>();
            if (!obj.ContainsKey("password"))
            {
                return null;
            }
            db.Password = obj["password"].Value<string>();
            if (!obj.ContainsKey("database"))
            {
                return null;
            }
            db.Database = obj["database"].Value<string>();
            return db;
        }


        public static Config Parse(string text)
        {
            Config conf = new Config();
            JObject obj;
            try
            {
                obj = JObject.Parse(text);
            }
            catch (JsonReaderException)
            {
                return null;
            }

            #region debug field
            if(!obj.ContainsKey("debug"))
            {
                conf.debug = false;
            }
            else
            {
                conf.debug = obj["debug"].Value<bool>();
            }
            #endregion

            #region listen field
            if (!obj.ContainsKey("listen"))
            {
                return null;
            }
            conf.address = ParseAddress(obj["listen"].Value<string>());
            #endregion

            #region address field
            if (conf.address == null)
            {
                return null;
            }
            #endregion

            #region tcp-port field
            if (!obj.ContainsKey("tcp-port"))
            {
                return null;
            }
            int tmpPort = ParsePort(obj["tcp-port"].Value<string>());
            if (tmpPort < 0)
            {
                return null;
            }
            conf.tcpport = (uint)tmpPort;
            #endregion

            #region udp-port field
            if (!obj.ContainsKey("udp-port"))
            {
                return null;
            }
            tmpPort = ParsePort(obj["udp-port"].Value<string>());
            if (tmpPort < 0)
            {
                return null;
            }
            conf.udpport = (uint)tmpPort;
            #endregion

            #region statistics field
            if (!obj.ContainsKey("statistics"))
            {
                return null;
            }
            JArray arr = obj["statistics"].Value<JArray>();
            foreach (JObject each in arr)
            {
                Applications.StatisticsConf tmpConf = ParseStatistics(each);
                if (tmpConf == null)
                {
                    return null;
                }
                conf.StatisticsConf.Add(tmpConf);
            }
            #endregion

            #region main field
            if (!obj.ContainsKey("main"))
            {
                return null;
            }
            conf.isMain = obj["main"].Value<bool>();
            #endregion

            #region mainServer field
            if(!conf.isMain)
            {
                if(!obj.ContainsKey("mainServer"))
                {
                    return null;
                }
                string address = obj["mainServer"].Value<string>();
                string[] splitAddress = address.Split(":");
                conf.mainServer = new IPEndPoint(IPAddress.Parse(splitAddress[0]), int.Parse(splitAddress[1]));
            }
            #endregion

            #region forward field
            if (!obj.ContainsKey("forward"))
            {
                return null;
            }
            string forwardStr = obj["forward"].Value<string>();
            conf.forward = ParseEndPoint(forwardStr);
            if (conf.forward == null)
            {
                return null;
            }
            #endregion

            #region log field
            if (!obj.ContainsKey("log"))
            {
                return null;
            }
            JArray logArray = obj["log"].Value<JArray>();
            foreach (JObject tmpObject in logArray)
            {
                ILogger tmpLogger = ParseLogger(tmpObject);
                if (tmpLogger != null)
                {
                    conf.loggers.Add(tmpLogger);
                }
            }
            #endregion

            #region database field
            if (obj.ContainsKey("database"))
            {
                DatabaseConnectionStruct? db = ParseDatabase(obj["database"].Value<JObject>());
                if (db.HasValue)
                {
                    conf.database = db.Value;
                }
            }
            #endregion

            return conf;
        }

        public void Dispose()
        {
            loggers.Dispose();
        }

        public static Config Init(string path)
        {
            if (!File.Exists(path))
            {
                Console.Error.WriteLine($"Config file {path} does not exist");
                return null;
            }
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                if (!fs.CanRead)
                {
                    Console.Error.WriteLine("Not enough permissions to read config file");
                    return null;
                }
            }
            string configContent = File.ReadAllText(path);
            return Config.Parse(configContent);
        }
    }
}
