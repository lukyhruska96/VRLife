using System;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Common.Core.Services.RoomService;

namespace VrLifeAPI.Forwarder.Core.Services.RoomService
{
    public delegate void UserConnectEventHandler(ulong userId, uint roomId);
    public delegate void UserDisconnectEventHandler(ulong userId, uint roomId, string reason);
    public delegate void RoomCreateEventHandler(IRoom room);
    public delegate void RoomDeleteEventHandler(uint roomId);

    /// <summary>
    /// Interface služby místností na straně Forwardera
    /// </summary>
    public interface IRoomServiceForwarder : IRoomService, IServiceForwarder
    {
        /// <summary>
        /// ID Místnosti podle ID uživatele, který se v ní nachází.
        /// 
        /// null v případě, že uživatel není připojený do žádné místnosti.
        /// </summary>
        /// <param name="userId">ID uživatele.</param>
        /// <returns>ID místnosti.</returns>
        uint? RoomByUserId(ulong userId);

        /// <summary>
        /// Seznam ID uživatelů připojených do dané místnosti.
        /// </summary>
        /// <param name="roomId">ID místnosti.</param>
        /// <returns>Seznam ID uživatelů.</returns>
        ulong[] GetConnectedUsers(uint roomId);


        /// <summary>
        /// Event vyvolaný v případě, že se nový užiatel připojil do dané místnosti.
        /// </summary>
        event UserConnectEventHandler UserConnected;

        /// <summary>
        /// Event volaný v případě, že se uživatel odpojil od dané místnosti.
        /// </summary>
        event UserDisconnectEventHandler UserDisconnected;

        /// <summary>
        /// Event vyvolaný v případě, že byla daná místnost smazána.
        /// </summary>
        event RoomDeleteEventHandler RoomDeleted;

        /// <summary>
        /// Event vyvolaný v případě, že byla daná místnost vytvořena.
        /// </summary>
        event RoomCreateEventHandler RoomCreated;
    }
}
