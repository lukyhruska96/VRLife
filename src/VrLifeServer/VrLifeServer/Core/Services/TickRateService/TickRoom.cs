using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VrLifeServer.Core.Services.RoomService;

namespace VrLifeServer.Core.Services.TickRateService
{
    class TickRoom
    {
        private const int DEFAULT_TICKBUFF_SIZE = 32;
        public Room Room { get; set; }
        public TickState[] TickBuffer { get; set; } = null;
        public int LatestTickIdx { get; set; }

        private TickState _preTickBuff = new TickState(0);
        private Object _preTickLock = new Object();

        public TickRoom(Room room, int buffSize = DEFAULT_TICKBUFF_SIZE)
        {
            Room = room;
            TickBuffer = new TickState[buffSize];
            LatestTickIdx = buffSize - 1;
        }

        public void Start()
        {
            Task tick = new Task(this.Tick, TaskCreationOptions.LongRunning);
            tick.Start();
        }

        private void Tick()
        {
            Stopwatch sw = new Stopwatch();
            int tickTimeMs = 1000 / (int)Room.TickRate;
            ulong tickNum = ((ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - Room.StartTime) / (ulong)tickTimeMs;
            _preTickBuff.Tick = tickNum;
            while (true)
            {
                try
                {
                    sw.Restart();
                    lock(_preTickLock)
                    {
                        TickState state = _preTickBuff;
                        _preTickBuff = new TickState(++tickNum);
                    }
                    sw.Stop();
                    Thread.Sleep(Math.Max(0, tickTimeMs - (int)sw.ElapsedMilliseconds));
                }
                // Whatever happens, we don't want this Task to crash.
                catch (Exception) { }
            }
        }
    }
}
