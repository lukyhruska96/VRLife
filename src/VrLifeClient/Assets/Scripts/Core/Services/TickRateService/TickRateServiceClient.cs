using Assets.Scripts.Core.Services;
using Assets.Scripts.Core.Services.TickRateService;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using VrLifeClient.API;
using VrLifeClient.Core.Services.SystemService;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeClient.Core.Services.TickRateService
{
    class TickRateServiceClient : IServiceClient
    {
        private const uint SNAPSHOT_BUFFER_SIZE = 16;

        private ClosedAPI _api;

        private ulong _lastTick = 0;
        public ulong LastTick { get => _lastTick; }

        public ConcurrentQueue<SnapshotData> SnapshotBuffer { get; } = new ConcurrentQueue<SnapshotData>();

        private object _tickLock = new object();

        public void HandleMessage(MainMessage msg)
        {
            throw new NotImplementedException();
        }

        public void Init(ClosedAPI api)
        {
            this._api = api;
        }

        public ServiceCallback<SnapshotData> GetSnapshot()
        {
            return new ServiceCallback<SnapshotData>(() =>
            {
                lock(_tickLock)
                {
                    SnapshotRequest request = new SnapshotRequest();
                    request.UserId = _api.Services.User.UserId.Value;
                    request.LastRTT = _api.Services.Event.LastRTT;
                    request.LastTick = _lastTick;
                    MainMessage msg = new MainMessage();
                    msg.TickMsg = new TickMsg();
                    msg.TickMsg.SnapshotRequest = request;
                    MainMessage response = _api.OpenAPI.Networking.Send(msg, _api.Services.Room.ForwarderAddress);
                    if (SystemServiceClient.IsErrorMsg(response))
                    {
                        throw new TickRateServiceException(response.SystemMsg.ErrorMsg.ErrorMsg_);
                    }
                    SnapshotData data = response.TickMsg.SnapshotData;
                    if (data == null)
                    {
                        throw new TickRateServiceException("Unknown response.");
                    }
                    SnapshotBuffer.Enqueue(data);
                    if(SnapshotBuffer.Count > SNAPSHOT_BUFFER_SIZE)
                    {
                        while(!SnapshotBuffer.TryDequeue(out _));
                    }
                    _lastTick = data.TickNum;
                    return data;
                }
            });
        }
    }
}
