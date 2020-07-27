using Assets.Scripts.Core.Services;
using Assets.Scripts.Core.Services.TickRateService;
using Assets.Scripts.Core.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VrLifeAPI.Client.API;
using VrLifeAPI.Client.Core.Services;
using VrLifeAPI.Common.Core.Utils;
using VrLifeAPI.Networking.NetworkingModels;
using VrLifeClient.API;
using VrLifeClient.Core.Services.SystemService;

namespace VrLifeClient.Core.Services.TickRateService
{
    class TickRateServiceClient : ITickRateServiceClient
    {
        private const uint SNAPSHOT_BUFFER_SIZE = 16;

        private IClosedAPI _api;

        private ulong _lastTick = 0;
        public ulong LastTick { get => _lastTick; }

        public ConcurrentQueue<SnapshotData> SnapshotBuffer { get; } = new ConcurrentQueue<SnapshotData>();

        private object _tickLock = new object();

        public void HandleMessage(MainMessage msg)
        {
            throw new NotImplementedException();
        }

        public void Init(IClosedAPI api)
        {
            this._api = api;
            this._api.Services.Room.RoomExited += Reset;
        }

        public IServiceCallback<SnapshotData> GetSnapshot()
        {
            return new ServiceCallback<SnapshotData>(() =>
            {
                lock(_tickLock)
                {
                    if(!_api.Services.User.UserId.HasValue)
                    {
                        throw new TickRateServiceException("UserId cannot be null.");
                    }
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
                    int tickDiff = (int)(data.TickNum - _lastTick);
                    if (tickDiff < _api.Services.Room.CurrentRoom.TickRate && _lastTick != 0)
                    {
                        data = SnapshotBuffer.Last().AddDiff(data);
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

        private void Reset()
        {
            lock (_tickLock)
            {
                _lastTick = 0;
                while (SnapshotBuffer.Count != 0)
                {
                    SnapshotBuffer.TryDequeue(out _);
                }
            }
        }
    }
}
