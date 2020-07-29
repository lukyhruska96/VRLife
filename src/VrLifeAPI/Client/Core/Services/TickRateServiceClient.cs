using System;
using System.Collections.Concurrent;
using System.Linq;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Client.Core.Services
{
    /// <summary>
    /// Interface TickRate služby.
    /// </summary>
    public interface ITickRateServiceClient : IServiceClient
    {
        /// <summary>
        /// ID posledního přijatého ticku serveru.
        /// </summary>
        ulong LastTick { get; }

        /// <summary>
        /// Buffer posledních přijatých snapshotů o maximální velikosti počtu ticků vykonaných za vteřinu.
        /// </summary>
        ConcurrentQueue<SnapshotData> SnapshotBuffer { get; }

        /// <summary>
        /// Žádost o nový snapshot.
        /// </summary>
        /// <returns>ServiceCallback s návratovou hodnotou nového snapshotu.</returns>
        IServiceCallback<SnapshotData> GetSnapshot();
    }
}
