using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VrLifeServer.API;
using VrLifeServer.Logging;
using VrLifeServer.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services
{
    struct ComputingServer
    {
        public uint id;
        public uint cores;
        public ulong memory;
        public float cpuUsage;
        public float ramUsage;
    }

    class SystemServiceProvider : ISystemService
    {
        private const long STATS_INTERVAL_MS = 1000;

        private ClosedAPI _api;

        private ILogger _log;

        private List<ComputingServer> computingServers = new List<ComputingServer>();
        public MainMessage HandleMessage(MainMessage msg)
        {
            SystemMsg sysMsg = msg.SystemMsg;
            switch(sysMsg.SystemMsgTypeCase)
            {
                case SystemMsg.SystemMsgTypeOneofCase.HiMsg:
                    return HandleHiMsg(sysMsg.HiMsg);
                default:
                    return ISystemService.CreateErrorMessage(msg.MsgId, 0, 0, "Unknown message type");
            }
        }

        public void Init(ClosedAPI api)
        {
            this._api = api;
            this._log = api.OpenAPI.CreateLogger(this.GetType().Name);
            if(!VrLifeServer.Conf.IsMain)
            {
                InitStats();
            }
        }

        private void InitStats()
        {
            Task t = new Task(() => 
            {
                Stopwatch sw = new Stopwatch();
                while (true)
                {
                    sw.Restart();
                    SystemMsg sysMsg = new SystemMsg();
                    StatMsg statMsg = new StatMsg();
                    statMsg.CpuUsage = HwMonitor.GetCoreUsage();
                    statMsg.MemoryTotal = HwMonitor.GetTotalMemory();
                    statMsg.MemoryUsed = HwMonitor.GetUsedMemory();
                    sysMsg.StatMsg = statMsg;
                    MainMessage msg = new MainMessage();
                    msg.SystemMsg = sysMsg;
                    sw.Stop();

                    long ms = sw.ElapsedMilliseconds;
                    if (ms > STATS_INTERVAL_MS)
                    {
                        this._log.Warn("HW status calculation takes longer than specified interval.");
                    }
                    else
                    {
                        Thread.Sleep((int)(STATS_INTERVAL_MS - ms));
                    }

                _api.OpenAPI.Networking.Send(msg, VrLifeServer.Conf.MainServer, this.MainServerMessageHandler);
                }
            }, TaskCreationOptions.LongRunning);
            t.Start();
        }

        private MainMessage HandleHiMsg(HiMsg msg)
        {
            if (msg.Version != VrLifeServer.VERSION)
            {
                return ISystemService.CreateErrorMessage(0, 0, 0, "Not compatiable version");
            }
            MainMessage response = ISystemService.CreateOkMessage();
            response.ServerId = (uint)computingServers.Count;
            computingServers.Add(new ComputingServer { id = response.ServerId, cores = msg.Threads, memory = msg.Memory });
            return response;
        }

        private void MainServerMessageHandler(MainMessage msg)
        {
            throw new NotImplementedException();
        }
    }
}
