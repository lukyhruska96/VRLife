using System;
using System.Collections.Generic;
using System.Net;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Client.Core.Wrappers
{
    /// <summary>
    /// Interface detailu místnosti
    /// </summary>
    public interface IRoom
    {
        /// <summary>
        /// ID dané místnosti
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
        /// Adresa forwardera spravujícího tuto místnost.
        /// </summary>
        IPEndPoint Address { get; set; }

        /// <summary>
        /// Tick rate dané místnosti.
        /// </summary>
        uint TickRate { get; set; }

        /// <summary>
        /// Unix time v ms od spuštění místnosti.
        /// 
        /// Vhodně k přepočítání času daného ticku,
        /// kdy tick 0 byl zaznamenán v čase StartTime.
        /// </summary>
        ulong StartTime { get; set; }

        /// <summary>
        /// Getter stavu, zda je místnost plná.
        /// </summary>
        /// <returns>Stav, zda je místnost plná.</returns>
        bool IsFull();

        /// <summary>
        /// Getter stavu, zda je místnost prázdná.
        /// </summary>
        /// <returns>Stav, zda je místnost prázdná.</returns>
        bool IsEmpty();

        /// <summary>
        /// Převod detailů o místnosti na síťový objekt k odeslání.
        /// </summary>
        /// <returns>Síťový objekt stavu místnosti.</returns>
        RoomDetail ToNetworkModel();
    }
}
