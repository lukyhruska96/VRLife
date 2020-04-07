using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using VrLifeServer.Logging;

namespace VrLifeServer
{
    public class VrLifeServer
    {
        public const string CONFIG_FILE = "config.json";
        public static Config Conf { get => conf == null ? VrLifeServer.LoadConf() : conf; set => conf = value; }
        private static Config conf = null;

        public static int Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

            // Config file loading
            VrLifeServer.LoadConf();
            while (true) { }

            return 0;
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
