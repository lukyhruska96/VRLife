using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VrLifeServer.Core.Services.RoomService;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.TickRateService
{
    class TickRoom
    {
        private const int DEFAULT_TICKBUFF_SIZE = 32;
        public Room Room { get; set; }
        public ConcurrentQueue<Snapshot> TickBuffer { get; set; } = null;
        private int _buffSize = 0;

        public Snapshot CurrentTick = Snapshot.Empty;

        private Task _tickTask = null;
        private bool _stopTick = false;

        public TickRoom(Room room, int buffSize = DEFAULT_TICKBUFF_SIZE)
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
                    if(TickBuffer.Count >= _buffSize)
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
            return snapshot.ToNetworkModel();
        }
    }
}
