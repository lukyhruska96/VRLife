using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VrLifeServer.API;
using VrLifeServer.Core.Utils;
using VrLifeServer.Logging;
using VrLifeServer.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.SystemService
{
    class SystemServiceForwarder : ISystemService
    {
        private const long STATS_INTERVAL_MS = 1000;

        private ClosedAPI _api;
        private ILogger _log;

        public MainMessage HandleMessage(MainMessage msg)
        {
            throw new NotImplementedException();
        }

        public void Init(ClosedAPI api)
        {
            this._api = api;
            this._log = api.OpenAPI.CreateLogger(this.GetType().Name);
            InitStats();
        }

        private void InitStats()
        {
            uint cpuUsage = 0;
            long memoryTotal = 0;
            long memoryUsed = 0;
            bool firstRun = false;
            Task calculationT = new Task(() =>
            {
                while(true)
                {
                    cpuUsage = HwMonitor.GetCoreUsage();
                    Interlocked.Exchange(ref memoryTotal, (long) HwMonitor.GetTotalMemory());
                    Interlocked.Exchange(ref memoryUsed, (long) HwMonitor.GetUsedMemory());
                    firstRun = true;
                }
            }, TaskCreationOptions.LongRunning);
            Task sendingT = new Task(() =>
            {
                while(!firstRun)
                {
                    Thread.Sleep(1000);
                }
                while (true)
                {
                    SystemMsg sysMsg = new SystemMsg();
                    StatMsg statMsg = new StatMsg();
                    statMsg.CpuUsage = cpuUsage;
                    statMsg.MemoryTotal = (ulong) Interlocked.Read(ref memoryTotal);
                    statMsg.MemoryUsed = (ulong)Interlocked.Read(ref memoryUsed);
                    sysMsg.StatMsg = statMsg;
                    MainMessage msg = new MainMessage();
                    msg.SystemMsg = sysMsg;
                    Thread.Sleep((int)STATS_INTERVAL_MS);

                    _api.OpenAPI.Networking.Send(msg, _api.OpenAPI.Conf.MainServer, (_) => { }, (err) => {
                        _log.Error("There was an error during status sending to Main Server.");
                        Environment.Exit(1);
                    });
                }
            }, TaskCreationOptions.LongRunning);
            calculationT.Start();
            sendingT.Start();
        }
    }
}
