using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VrLifeAPI.Common.Core.Services.RoomService;
using VrLifeAPI.Networking.NetworkingModels;
using VrLifeServer.Core.Services.RoomService;

namespace VrLifeServer.Core.Services.TickRateService
{
    class TickRoom
    {
        private const int DEFAULT_TICKBUFF_SIZE = 32;
        public IRoom Room { get; set; }
        public ConcurrentQueue<Snapshot> TickBuffer { get; set; } = null;
        private int _buffSize = 0;

        public Snapshot CurrentTick = Snapshot.Empty;

        private Task _tickTask = null;
        private bool _stopTick = false;

        public TickRoom(IRoom room, int buffSize = DEFAULT_TICKBUFF_SIZE)
        {
            Room = room;
            TickBuffer = new ConcurrentQueue<Snapshot>();
            _buffSize = buffSize;
        }

        public void Start()
        {
            if(_tickTask != null)
            {
                return;
            }
            _tickTask = new Task(this.Tick, TaskCreationOptions.LongRunning);
            _tickTask.Start();
        }

        public void Stop()
        {
            this._stopTick = true;
            // clean up
            _tickTask.ContinueWith(_ => { _tickTask = null; });
        }

        private void Tick()
        {
            float tickTimeMs = 1000f / Room.TickRate;
            ulong tickNum = ((ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - Room.StartTime) / (ulong)tickTimeMs;
            CurrentTick.Tick = tickNum;
            while (!_stopTick)
            {
                try
                {
                    Snapshot state = CurrentTick;
                    CurrentTick = new Snapshot(state);
                    TickBuffer.Enqueue(state);
                    if(TickBuffer.Count > _buffSize)
                    {
                        while (!TickBuffer.TryDequeue(out _)) { }
                    }
                    ulong tickUnixTime = (ulong)(CurrentTick.Tick * tickTimeMs) + Room.StartTime;
                    Thread.Sleep(Math.Max(0, (int)((long)tickUnixTime - DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())));
                }
                // Whatever happens, we don't want this Task to crash.
                catch (Exception) { }
            }
        }

        public SnapshotData GetSnapshotData(ulong lastTick, uint rtt)
        {
            Snapshot[] snapshots = TickBuffer.ToArray();
            Snapshot snapshot = snapshots.Last();
            int tickDiff = (int)(snapshot.Tick - lastTick);
            if (lastTick == 0 || tickDiff >= Room.TickRate)
            {
                return snapshot.ToNetworkModel();
            }
            else
            {
                Snapshot lastTickSnapshot = snapshots[snapshots.Length - tickDiff - 1];
                return Snapshot.MakeDiff(lastTickSnapshot, snapshot).ToNetworkModel();
            }
        }
    }
}
