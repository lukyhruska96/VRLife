using Google.Protobuf;
using System.Numerics;
using VrLifeAPI.Common.Core.Utils;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Common.Core.Services.TickRateService
{
    /// <summary>
    /// Informace o stavu objektu ve snapshotu.
    /// </summary>
    public struct ObjectState
    {
        /// <summary>
        /// ID objektové aplikace.
        /// </summary>
        public ulong AppId { get; set; }

        /// <summary>
        /// ID instance objektové aplikace.
        /// </summary>
        public ulong AppInstanceId { get; set; }

        /// <summary>
        /// Souřadnice středu aplikace v místnosti.
        /// </summary>
        public Vector3 Center { get; set; }

        /// <summary>
        /// Převod na síťovací objekt k odeslání.
        /// </summary>
        /// <returns>Síťovací objekt.</returns>
        public GameObject ToNetworkModel()
        {
            GameObject o = new GameObject();
            o.AppId = AppId;
            o.AppInstanceId = AppInstanceId;
            o.Center = Center.ToCoord();
            return o;
        }

    }
}
