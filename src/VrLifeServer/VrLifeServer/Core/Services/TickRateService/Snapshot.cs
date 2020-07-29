using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using VrLifeAPI.Common.Core.Services.TickRateService;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.TickRateService
{
    class Snapshot
    {
        public ulong Tick { get; set; }
        public ConcurrentDictionary<ulong, SkeletonState> SkeletonStates { get; set; }
        public ConcurrentDictionary<(ulong, ulong), ObjectState> ObjectStates { get; set; }
        public static Snapshot Empty = new Snapshot();

        private Snapshot()
        {
            Tick = 0;
            SkeletonStates = new ConcurrentDictionary<ulong, SkeletonState>();
            ObjectStates = new ConcurrentDictionary<(ulong, ulong), ObjectState>();
        }

        public Snapshot(Snapshot prevState)
        {
            Tick = prevState.Tick + 1;
            SkeletonStates = new ConcurrentDictionary<ulong, SkeletonState>(prevState.SkeletonStates);
            ObjectStates = new ConcurrentDictionary<(ulong, ulong), ObjectState>(prevState.ObjectStates);
        }

        public SnapshotData ToNetworkModel()
        {
            SnapshotData data = new SnapshotData();
            data.TickNum = this.Tick;
            data.Skeletons.AddRange(this.SkeletonStates.Select(x => x.Value.ToNetworkModel()));
            data.Objects.AddRange(this.ObjectStates.Select(x => x.Value.ToNetworkModel()));
            return data;
        }

        public static Snapshot MakeDiff(Snapshot from, Snapshot to)
        {
            Snapshot val = new Snapshot();
            val.Tick = to.Tick;
            var dict = to.SkeletonStates
                .Select(x => from.SkeletonStates.TryGetValue(x.Key, out SkeletonState val) ? x.Value - val : x.Value)
                .ToDictionary(x => x.UserId, x => x);
            foreach(var keypair in dict)
            {
                while (!val.SkeletonStates.TryAdd(keypair.Key, keypair.Value));
            }
            foreach(var keypair in to.ObjectStates)
            {
                while (!val.ObjectStates.TryAdd(keypair.Key, keypair.Value));
            }
            return val;
        }
    }
}
