using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Threading;
using VrLifeServer.Core;
using VrLifeServer.Core.Utils;
using VrLifeServer.Database;
using VrLifeShared.Logging;
using static System.Net.Mime.MediaTypeNames;

namespace VrLifeServer
{
    public class VrLifeServer
    {
        private const string CONFIG_FILE = "config.json";

        public const string VERSION = "0.0.1";

        private static Config conf = null;

        public static int Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

#if DEBUG
            OnDebug();
#else

            // Config file loading
            Init();
            conf.Loggers.Info("Logger has been initialized.");

            if (conf.IsMain)
            {
                conf.Loggers.Info("Configurating Main Server...");
                MainServer server = new MainServer();
                server.Init(conf);
                conf.Loggers.Info("Starting Main Server...");
                server.Start();
            }
            else
            {
                conf.Loggers.Info("Configurating Computing Server...");
                ComputingServer server = new ComputingServer();
                server.Init(conf);
                conf.Loggers.Info("Starting Computing Server...");
                server.Start();
            }
            conf.Loggers.Info("Server is running");
#endif
            while (true) {
                    Thread.Sleep(1000);    
            }
        }

        public static void Init()
        {
            try
            {
                conf = LoadConf();
            }
            catch(FormatException ex)
            {
                Console.Error.WriteLine(ex.Message);
                Environment.Exit(1);
            }
            VrLifeDbContext.SetConfig(conf);
        }

        private static Config LoadConf()
        {
            string configPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                VrLifeServer.CONFIG_FILE);
            return Config.Init(configPath);
        }

        public static void OnProcessExit(object sender, EventArgs e)
        {
            if (conf == null) return;
            conf.Loggers.Dispose();
        }


        private static void OnDebug()
        {
            JObject consoleLog = new JObject();
            consoleLog["type"] = "console";


            JObject mainConf = new JObject();
            mainConf["debug"] = true;
            mainConf["listen"] = "0.0.0.0";
            mainConf["tcp-port"] = 8765;
            mainConf["udp-port"] = 8766;
            mainConf["main"] = true;
            JObject database = new JObject();
            database["type"] = "mysql";
            database["host"] = "localhost";
            database["port"] = 3306;
            database["username"] = "dev";
            database["password"] = "dev123";
            database["database"] = "dev";
            mainConf["database"] = database;
            JArray mainLogs = new JArray();
            JObject mainLog = new JObject();
            mainLog["type"] = "file";
            mainLog["path"] = "./vrlife-main-server.log";
            mainLogs.Add(mainLog);
            mainLogs.Add(consoleLog);
            mainConf["log"] = mainLogs;


            JObject compConf = new JObject();
            compConf["debug"] = true;
            compConf["listen"] = "0.0.0.0";
            compConf["tcp-port"] = 8865;
            compConf["udp-port"] = 8866;
            compConf["main"] = false;
            compConf["mainServer"] = "127.0.0.1:8766";
            JArray compLogs = new JArray();
            JObject compLog = new JObject();
            compLog["type"] = "file";
            compLog["path"] = "./vrlife-comp-server.log";
            compLogs.Add(compLog);
            compLogs.Add(consoleLog);
            compConf["log"] = compLogs;

            Config mainConfig = Config.Parse(mainConf);
            Config compConfig = Config.Parse(compConf);
            VrLifeDbContext.SetConfig(mainConfig);

            MainServer mainServer = new MainServer();
            mainServer.Init(mainConfig);
            mainServer.Start();

            ComputingServer compServer = new ComputingServer();
            compServer.Init(compConfig);
            compServer.Start();

        }

    }
}
