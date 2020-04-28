using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Threading;
using VrLifeServer.Core;
using VrLifeServer.Logging;

namespace VrLifeServer
{
    public class VrLifeServer
    {
        private const string CONFIG_FILE = "config.json";

        public const string VERSION = "0.0.1";

        private static Config conf = null;


        public static Config Conf { get => conf == null ? VrLifeServer.LoadConf() : conf; set => conf = value; }
        public static ILogger Logger { get => VrLifeServer.Conf.Loggers; }


        public static int Main(string[] args)
        {
            ulong memory = HwMonitor.GetTotalMemory();
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

            // Config file loading
            VrLifeServer.LoadConf();
            if(VrLifeServer.Conf.IsMain)
            {
                MainServer server = new MainServer();
                server.Init();
                server.Start();
            }
            else
            {
                ComputingServer server = new ComputingServer();
                server.Init();
                server.Start();
            }
            while (true) {
                    Thread.Sleep(1000);    
            }
        }

        public static void Init()
        {
            if(conf == null)
            {
                LoadConf();
            }
        }

        private static Config LoadConf()
        {
            string configPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                VrLifeServer.CONFIG_FILE);
            VrLifeServer.Conf = Config.Init(configPath);
            return VrLifeServer.Conf;
        }

        public static void OnProcessExit(object sender, EventArgs e)
        {
            if (VrLifeServer.Conf == null) return;
            VrLifeServer.Conf.Loggers.Dispose();
        }

    }
}
