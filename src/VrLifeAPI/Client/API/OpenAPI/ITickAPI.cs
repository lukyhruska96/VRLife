using VrLifeAPI.Client.Core.Services;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Client.API.OpenAPI
{
    /// <summary>
    /// API pro komunikaci s TickRateService pomocí OpenAPI
    /// </summary>
    public interface ITickAPI
    {
        /// <summary>
        /// Vrátí snapshot aktuálního ticku serveru.
        /// </summary>
        /// <returns>ServiceCallback s návratovou hodnotou snapshotu aktuálního ticku.</returns>
        IServiceCallback<SnapshotData> GetSnapshot();

        /// <summary>
        /// Vrátí uložený buffer posledních získaných snapshotu 
        /// o maximální velikosti tickRate dané místnosti.
        /// </summary>
        /// <returns>Pole posledních získaných snapshotů.</returns>
        SnapshotData[] GetSnapshotBuffer();
    }
}
