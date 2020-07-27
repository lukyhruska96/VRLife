using System;
using System.Collections.Concurrent;
using System.Linq;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Client.Core.Services
{
    public interface ITickRateServiceClient : IServiceClient
    {
        ulong LastTick { get; }

        ConcurrentQueue<SnapshotData> SnapshotBuffer { get; }

        IServiceCallback<SnapshotData> GetSnapshot();
    }
}
