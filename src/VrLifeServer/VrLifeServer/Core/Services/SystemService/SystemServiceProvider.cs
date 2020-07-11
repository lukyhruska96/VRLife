using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VrLifeServer.API.Provider;
using VrLifeServer.Core.Utils;
using VrLifeShared.Logging;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.SystemService
{
    class ComputingServer
    {
        private const long MAX_MS_TOLERANCE = 5000;
        public uint id;
        public uint cores;
        public ulong memory;
        public float cpuUsage;
        public float ramUsage;
        public IPEndPoint address;
        public long lastResponse;

        public bool IsAlive { get => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - lastResponse < MAX_MS_TOLERANCE; }
    }

    class SystemServiceProvider : ISystemServiceProvider
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
            computingServers.Add(
                new ComputingServer { id = response.ServerId, cores = msg.Threads, 
                memory = msg.Memory, address = new IPEndPoint(msg.Address, msg.Port),
                lastResponse = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()});
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
            computingServers[(int)serverId].lastResponse = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            _log.Debug($"server {serverId} status: CPU: {statMsg.CpuUsage}%, RAM: {statMsg.MemoryUsed} MB");
            return ISystemService.CreateOkMessage((uint)msg.MsgId);
        }

        public MainMessage CreateHelloMessage()
        {
            return ISystemService.CreateHelloMessage(_api.OpenAPI.Config);
        }

        public IPEndPoint GetAddressById(uint serverId)
        {
            if(serverId >= computingServers.Count)
            {
                return null;
            }
            return computingServers[(int)serverId].address;
        }

        public bool IsAlive(ulong serverId)
        {
            if(serverId >= (ulong)computingServers.Count)
            {
                return false;
            }
            return computingServers[(int)serverId].IsAlive;
        }
    }
}
