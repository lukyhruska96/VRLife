using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VrLifeServer.API;
using VrLifeServer.Logging;
using VrLifeServer.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.SystemService
{
    class ComputingServer
    {
        public uint id;
        public uint cores;
        public ulong memory;
        public float cpuUsage;
        public float ramUsage;
    }

    class SystemServiceProvider : ISystemService
    {

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
                case SystemMsg.SystemMsgTypeOneofCase.StatMsg:
                    return HandleStatMsg(msg);
                default:
                    return ISystemService.CreateErrorMessage(msg.MsgId, 0, 0, "Unknown message type");
            }
        }

        public void Init(ClosedAPI api)
        {
            this._api = api;
            this._log = api.OpenAPI.CreateLogger(this.GetType().Name);
            // empty object to fill 0 index
            this.computingServers.Clear();
            this.computingServers.Add(new ComputingServer());
            this._log.Debug("Loger initialized.");
        }

        private MainMessage HandleHiMsg(HiMsg msg)
        {
            this._log.Debug("Received Hi message.");
            if (msg.Version != VrLifeServer.VERSION)
            {
                this._log.Debug("Not compatiable version of client.");
                return ISystemService.CreateErrorMessage(0, 0, 0, "Not compatiable version");
            }
            MainMessage response = ISystemService.CreateOkMessage();
            response.ServerId = (uint)computingServers.Count;
            this._log.Debug($"Sending {response.ServerId} as a new ServerID.");
            computingServers.Add(new ComputingServer { id = response.ServerId, cores = msg.Threads, memory = msg.Memory });
            return response;
        }

        private MainMessage HandleStatMsg(MainMessage msg)
        {
            _log.Debug("In HandleStatMsg method");
            uint serverId = msg.ServerId;
            if(serverId >= computingServers.Count)
            {
                _log.Error("Unknown Computing Server.");
                return ISystemService.CreateErrorMessage(msg.MsgId, 0, 0, "Unknown Computing Server.");
            }
            StatMsg statMsg = msg.SystemMsg.StatMsg;
            computingServers[(int)serverId].cpuUsage = statMsg.CpuUsage;
            computingServers[(int)serverId].ramUsage = statMsg.MemoryUsed;
            _log.Debug($"server {serverId} status: CPU: {statMsg.CpuUsage}%, RAM: {statMsg.MemoryUsed} MB");
            return ISystemService.CreateOkMessage((uint)msg.MsgId);
        }
    }
}
