using System;
using System.Collections.Generic;
using System.Net;
using VrLifeAPI.Common.Logging.Logging;
using VrLifeAPI.Networking.NetworkingModels;
using VrLifeAPI.Provider.API;
using VrLifeAPI.Provider.Core.Services.SystemService;
using VrLifeServer.API.Provider;

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

        private IClosedAPI _api;

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
                    return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, "Unknown message type");
            }
        }

        public void Init(IClosedAPI api)
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
                return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(0, 0, 0, "Not compatiable version");
            }
            MainMessage response = VrLifeAPI.Common.Core.Services.ServiceUtils.CreateOkMessage();
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
                return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateErrorMessage(msg.MsgId, 0, 0, "Unknown Computing Server.");
            }
            StatMsg statMsg = msg.SystemMsg.StatMsg;
            computingServers[(int)serverId].cpuUsage = statMsg.CpuUsage;
            computingServers[(int)serverId].ramUsage = statMsg.MemoryUsed;
            computingServers[(int)serverId].lastResponse = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            _log.Info($"server {serverId} status: CPU: {statMsg.CpuUsage}%, RAM: {statMsg.MemoryUsed} MB");
            return VrLifeAPI.Common.Core.Services.ServiceUtils.CreateOkMessage((uint)msg.MsgId);
        }

        public MainMessage CreateHelloMessage()
        {
            return ServiceUtils.CreateHelloMessage(_api.OpenAPI.Config);
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
