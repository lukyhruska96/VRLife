using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VrLifeAPI.Common;
using VrLifeAPI.Common.Database;
using VrLifeAPI.Common.Logging.Logging;
using VrLifeServer.Database;
using VrLifeShared.Logging;

namespace VrLifeServer
{
    public class Config : IConfig
    {
        public IPAddress Listen { get => listen; }
        private IPAddress listen;

        public IPAddress ServerAddress { get => serverAddress; }
        private IPAddress serverAddress;

        public uint UdpPort { get => udpport; }
        private uint udpport;

        public bool IsMain { get => isMain; }
        private bool isMain;

        public string AppStoragePath { get => _appStoragePath; }
        private string _appStoragePath;

        public IPEndPoint MainServer { get => mainServer; }
        private IPEndPoint mainServer;

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
                throw new FormatException("'type' field not found.");
            }
            string type = obj["type"].Value<string>();
            switch (type)
            {
                case "file":
                    return ParseFileLogger(obj);
                case "console":
                    return new ConsoleLogger();
                default:
                    throw new FormatException("'type' field have unsupported value.");
            }
        }

        private static ILogger ParseFileLogger(JObject obj)
        {
            ILogger logger = null;
            if (!obj.ContainsKey("path"))
            {
                throw new FormatException("'path' field not found.");
            }
            string path = obj["path"].Value<string>();
            try
            {
                logger = new FileLogger(path);
            }
            catch (Exception e)
            {
                throw new FormatException("Unable to initialize text logger.", e);
            }
            return logger;
        }

        private static DatabaseConnectionStruct? ParseDatabase(JObject obj)
        {
            DatabaseConnectionStruct db;
            if (!obj.ContainsKey("type"))
            {
                throw new FormatException("'type' field not found.");
            }
            db.Type = obj["type"].Value<string>();
            if (!obj.ContainsKey("host"))
            {
                throw new FormatException("'host' field not found.");
            }
            db.Host = obj["host"].Value<string>();
            if (!obj.ContainsKey("port"))
            {
                throw new FormatException("'port' field not found.");
            }
            db.Port = obj["port"].Value<int>();
            if (!obj.ContainsKey("username"))
            {
                throw new FormatException("'username' field not found.");
            }
            db.Username = obj["username"].Value<string>();
            if (!obj.ContainsKey("password"))
            {
                throw new FormatException("'password' field not found.");
            }
            db.Password = obj["password"].Value<string>();
            if (!obj.ContainsKey("database"))
            {
                throw new FormatException("'database' field not found.");
            }
            db.Database = obj["database"].Value<string>();
            return db;
        }


        public static Config Parse(string text)
        {
            JObject obj;
            try
            {
                obj = JObject.Parse(text);
            }
            catch (JsonReaderException)
            {
                throw new FormatException("Config file could not be parsed as JSON file.");
            }

            return Parse(obj);
        }

        public static Config Parse(JObject obj)
        {
            Config conf = new Config();

            #region debug field
            if (!obj.ContainsKey("debug"))
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
                throw new FormatException("'listen' field not found.");
            }
            conf.listen = ParseAddress(obj["listen"].Value<string>());

            if (conf.listen == null)
            {
                throw new FormatException("'listen' field could not be parsed.");
            }
            #endregion

            #region serverAddress field
            if (!obj.ContainsKey("serverAddress"))
            {
                throw new FormatException("'serverAddress' field not found.");
            }
            conf.serverAddress = IPAddress.Parse(obj["serverAddress"].Value<string>());
            #endregion


            #region udp-port field
            if (!obj.ContainsKey("udp-port"))
            {
                throw new FormatException("'udp-port' field not found.");
            }
            int tmpPort = ParsePort(obj["udp-port"].Value<string>());
            if (tmpPort < 0)
            {
                throw new FormatException("'udp-port' field could not be parsed.");
            }
            conf.udpport = (uint)tmpPort;
            #endregion

            #region main field
            if (!obj.ContainsKey("main"))
            {
                throw new FormatException("'main' field not found.");
            }
            conf.isMain = obj["main"].Value<bool>();
            #endregion

            #region mainServer field
            if(!conf.isMain)
            {
                if(!obj.ContainsKey("mainServer"))
                {
                    throw new FormatException("'mainServer' field not found.");
                }
                string address = obj["mainServer"].Value<string>();
                string[] splitAddress = address.Split(":");
                conf.mainServer = new IPEndPoint(IPAddress.Parse(splitAddress[0]), int.Parse(splitAddress[1]));
            }
            #endregion

            #region appStoragePath field
            if (conf.isMain) {
                if (!obj.ContainsKey("appStoragePath"))
                {
                    throw new FormatException("'appStoragePath' field not found.");
                }
                string path = obj["appStoragePath"].Value<string>();
                conf._appStoragePath = path;
            }
            #endregion

                #region log field
                if (!obj.ContainsKey("log"))
            {
                throw new FormatException("'log' field not found.");
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
            conf.loggers.SetDebug(conf.Debug);
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
                throw new FormatException($"Config file {path} does not exist");
            }
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                if (!fs.CanRead)
                {
                    throw new FormatException("Not enough permissions to read config file");
                }
            }
            string configContent = File.ReadAllText(path);
            return Config.Parse(configContent);
        }
    }
}
