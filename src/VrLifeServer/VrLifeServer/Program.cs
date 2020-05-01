using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Threading;
using VrLifeServer.Core;
using VrLifeServer.Core.Utils;
using VrLifeServer.Database;
using VrLifeServer.Logging;
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

            // Config file loading
            Init();
            conf.Loggers.Info("Logger has been initialized.");

            if(conf.IsMain)
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

    }
}
