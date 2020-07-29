using System;
using VrLifeAPI.Client.Core.Services;
using VrLifeAPI.Client.Core.Wrappers;

namespace VrLifeAPI.Client.API.OpenAPI
{
    /// <summary>
    /// API pro komunikaci s RoomService pomocí OpenAPI
    /// </summary>
    public interface IRoomAPI
    {
        /// <summary>
        /// Event volaný v případě opuštění místnosti v které se lokální uživatel nachází.
        /// </summary>
        event Action RoomExited;
        /// <summary>
        /// Event volaný v případě, že uživatel se připojil do místnosti
        /// a základní struktura je již zinicializována.
        /// </summary>
        event Action RoomEntered;

        /// <summary>
        /// Getter objektu popisujícího místnost, 
        /// do které je lokální uživatel aktuálně připojený.
        /// </summary>
        IRoom CurrentRoom { get; }

        /// <summary>
        /// Veřejný caller eventu RoomEntered 
        /// (pro RoomState skript, který volá event 
        /// až v případě, že je ve fázi Start)
        /// </summary>
        void OnRoomEnter();

        /// <summary>
        /// Metoda k rychlému připojení do náhodné místnosti.
        /// </summary>
        /// <returns>
        /// ServiceCallback s návratovou hodnotou 
        /// IRoom popisující místnost, do které byl uživatel připojen.
        /// </returns>
        IServiceCallback<IRoom> QuickJoin();

        /// <summary>
        /// Metoda k odpojení od aktuálně připojené místnosti.
        /// roomId se v tomto případě musí shodovat s uloženým ID aktuální místnosti.
        /// </summary>
        /// <param name="roomId">ID místnosti z které chce uživatel odejít.</param>
        /// <returns>ServiceCallback bez návratové hodnoty.</returns>
        IServiceCallback<bool> RoomExit(uint roomId);
    }
}
