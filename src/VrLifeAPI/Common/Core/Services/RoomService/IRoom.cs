using System;
using System.Collections.Generic;
using System.Net;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Common.Core.Services.RoomService
{

    /// <summary>
    /// Interface informací o místnosti na straně serveru
    /// </summary>
    public interface IRoom
    {
        /// <summary>
        /// ID místnosti
        /// </summary>
        uint Id { get; set; }

        /// <summary>
        /// Název místnosti
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Kapacita místnosti.
        /// </summary>
        uint Capacity { get; set; }

        /// <summary>
        /// Seznam ID uživatelů v dané místnosti.
        /// </summary>
        List<ulong> Players { get; }

        /// <summary>
        /// Adresa forwardera obstarávající tuto místnost.
        /// </summary>
        IPEndPoint Address { get; set; }

        /// <summary>
        /// TickRate dané mísnosti.
        /// </summary>
        uint TickRate { get; set; }

        /// <summary>
        /// Unixový čas v ms spuštění místnosti.
        /// 
        /// Vhodné pro výpočet času daného ticku,
        /// kdy tick 0 byl v čase StartTime.
        /// </summary>
        ulong StartTime { get; set; }

        /// <summary>
        /// Poslední čas aktivity uvnitř místnosti
        /// ve formátu Unix time v ms.
        /// </summary>
        long LastActivity { get; set; }

        /// <summary>
        /// Převod na síťovací model k odeslání.
        /// </summary>
        /// <returns>Síťovací model.</returns>
        RoomDetail ToNetworkModel();
    }
}
