using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.TickRateService
{
    class Snapshot
    {
        public ulong Tick { get; set; }
        public ConcurrentDictionary<ulong, SkeletonState> SkeletonStates { get; set; }
        public ConcurrentDictionary<ulong, ObjectState> ObjectStates { get; set; }
        public static Snapshot Empty = new Snapshot();

        private Snapshot()
        {
            Tick = 0;
            SkeletonStates = new ConcurrentDictionary<ulong, SkeletonState>();
            ObjectStates = new ConcurrentDictionary<ulong, ObjectState>();
        }

        public Snapshot(Snapshot prevState)
        {
            Tick = prevState.Tick + 1;
            SkeletonStates = new ConcurrentDictionary<ulong, SkeletonState>(prevState.SkeletonStates);
            ObjectStates = new ConcurrentDictionary<ulong, ObjectState>(prevState.ObjectStates);
        }

        public SnapshotData ToNetworkModel()
        {
            SnapshotData data = new SnapshotData();
            data.TickNum = this.Tick;
            data.Skeletons.AddRange(this.SkeletonStates.Select(x => x.Value.ToNetworkModel()));
            data.Objects.AddRange(this.ObjectStates.Select(x => x.Value.ToNetworkModel()));
            return data;
        }
    }
}
