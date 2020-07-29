using System;
using System.Collections.Generic;
using System.Net;
using VrLifeAPI.Client.Core.Wrappers;

namespace VrLifeAPI.Client.Core.Services
{
    /// <summary>
    /// Interface pro službu místností.
    /// </summary>
    public interface IRoomServiceClient : IServiceClient
    {
        /// <summary>
        /// Adresa forwardera obstarávajícího aktuální místnost.
        /// null v případě, že klient není připojený k žádné místnosti.
        /// </summary>
        IPEndPoint ForwarderAddress { get; }

        /// <summary>
        /// Objekt aktuálně připojené mísnosti.
        /// </summary>
        IRoom CurrentRoom { get; }

        /// <summary>
        /// Event volaný v případě, že se uživatel odpojil od místnosti.
        /// </summary>
        event Action RoomExited;

        /// <summary>
        /// Event volaný v případě, že se uživatel připojil do nové místnosti.
        /// Při přepojení je zaručeno, že je vždy nejprve zavolán event RoomExited
        /// </summary>
        event Action RoomEntered;

        /// <summary>
        /// Dotaz o získání detailních informací o místnosti.
        /// </summary>
        /// <param name="roomId">ID místnosti.</param>
        /// <returns>Objekt popisující danou místnost.</returns>
        IServiceCallback<IRoom> RoomDetail(uint roomId);

        /// <summary>
        /// Seznam všech dostupných místností splňující specifikované filtry.
        /// </summary>
        /// <param name="contains">Text obsažený v názvu hledaných místností.</param>
        /// <param name="notEmpty">Místnost nesmí být prázdná.</param>
        /// <param name="notFull">Místnost nesmí být plná.</param>
        /// <returns>ServiceCallback s návratovou hodnotou seznamu místností splňující krytéria ve filtru.</returns>
        IServiceCallback<List<IRoom>> RoomList(string contains = "", bool notEmpty = false, bool notFull = false);

        /// <summary>
        /// Žádost o vytvoření nové místnosti.
        /// </summary>
        /// <param name="name">Název nové místnosti.</param>
        /// <param name="capacity">Kapacita nové místnosti.</param>
        /// <returns>ServiceCallback s návratovou hodnotou detailu vytvořené místnosti.</returns>
        IServiceCallback<IRoom> RoomCreate(string name, uint capacity);

        /// <summary>
        /// Žádost o vstup do místnosti.
        /// </summary>
        /// <param name="roomId">ID místnosti.</param>
        /// <returns>ServiceCallback s návrotovou hodnotou detailu místnosti, do které byl uživatel připojen.</returns>
        IServiceCallback<IRoom> RoomEnter(uint roomId);

        /// <summary>
        /// Žádost o vstup do místnosti.
        /// 
        /// Tato verze je používána v případě, že uživatel ví, že daný forwarder obstarává tuto místnost,
        /// ale nemusí být ještě načtena providerem.
        /// 
        /// Interval načítání nových místností na straně providera je 10 sekund.
        /// </summary>
        /// <param name="roomId">ID místnosti.</param>
        /// <param name="address">Adresa forwardera, který obstarává danou místnosti.</param>
        /// <returns>ServiceCallback s návrotovou hodnotou detailu místnosti, do které byl uživatel připojen.</returns>
        IServiceCallback<IRoom> RoomEnter(uint roomId, IPEndPoint address);

        /// <summary>
        /// Žádost o opuštění místnosti.
        /// </summary>
        /// <param name="roomId">ID místnosti.</param>
        /// <returns>Service Callback bez návratové hodnoty.</returns>
        IServiceCallback<bool> RoomExit(uint roomId);

        /// <summary>
        /// Žádost o opuštění místnosti.
        /// 
        ///Tato verze je používána v případě, že uživatel ví, že daný forwarder obstarává tuto místnost,
        /// ale nemusí být ještě načtena providerem.
        /// </summary>
        /// <param name="roomId">ID místnosti.</param>
        /// <param name="address">Adresa forwardera obstarávajícího danou místnost.</param>
        /// <returns></returns>
        IServiceCallback<bool> RoomExit(uint roomId, IPEndPoint address);

        /// <summary>
        /// Vyvolání události RoomExited
        /// </summary>
        void OnRoomExit();

        /// <summary>
        /// Vyvolání události RoomEntered
        /// </summary>
        void OnRoomEnter();
    }
}
